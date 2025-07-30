using API.DTOs.Auth;
using API.Models;
using API.Models.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;
using TutaSpa.API.IService;

namespace ChatSupport.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private const int MaxUserAssignToAdmin = 2;
        private static readonly ConcurrentDictionary<string, UserSession> _userSessions = new();
        private static readonly ConcurrentDictionary<string, string> _connectionToUser = new();
        private static readonly ConcurrentDictionary<string, AdminSession> _adminSessions = new();

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task JoinAsGuest(string guestName)
        {
            var userId = Context.ConnectionId;
            var userName = string.IsNullOrEmpty(guestName) ? $"Khách #{userId[..8]}" : guestName;

            var dbSession = await _chatService.CreateOrGetSessionAsync(
                customerId: userId,
                customerName: userName,
                customerIP: Context.GetHttpContext()?.Connection?.RemoteIpAddress?.ToString(),
                userAgent: Context.GetHttpContext()?.Request?.Headers["User-Agent"].ToString()
            );

            // Nếu phiên chat đã đóng thì không cho join
            if (dbSession.Status == ChatSessionStatus.Closed)
            {
                await Clients.Caller.SendAsync("ChatClosed", new {sessionId = dbSession.SessionId});
                return;
            }

            var session = new UserSession
            {
                UserId = userId,
                UserName = userName,
                ConnectionId = Context.ConnectionId,
                StartTime = DateTime.Now,
                IsOnline = true,
                SessionId = dbSession.SessionId
            };

            _userSessions.TryAdd(userId, session);
            _connectionToUser.TryAdd(Context.ConnectionId, userId);

            await Groups.AddToGroupAsync(Context.ConnectionId, "Customers");

            await TryAssignUserToAvailableAdmin(session);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Cashier")]
        public async Task JoinAsAdmin()
        {
            var adminId = GetAdminId();
            var adminName = GetAdminName();

            if (adminId == null || adminName == null)
            {
                await Clients.Caller.SendAsync("Error", "Phiên đăng nhập không hợp lệ");
                return;
            }

            if (_adminSessions.ContainsKey(adminId))
            {
                var oldSession = _adminSessions[adminId];
                await Clients.Client(oldSession.ConnectionId).SendAsync("ForceLogout", "Bạn đã đăng nhập từ thiết bị khác");
                _adminSessions.TryRemove(adminId, out _);
            }

            var adminSession = new AdminSession
            {
                AdminId = adminId,
                AdminName = adminName,
                ConnectionId = Context.ConnectionId
            };

            _adminSessions.TryAdd(adminId, adminSession);
            await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");

            await AutoAssignWaitingUsersToAdmin(adminSession);

            await Clients.GroupExcept("Admins", Context.ConnectionId).SendAsync("AdminJoined", new
            {
                AdminId = adminId,
                AdminName = adminName
            });
        }

        private async Task<bool> TryAssignUserToAvailableAdmin(UserSession user)
        {
            var availableAdmin = _adminSessions.Values
                .Where(a => a.AssignedUsers.Count < MaxUserAssignToAdmin)
                .OrderBy(a => a.AssignedUsers.Count)
                .FirstOrDefault();

            if (availableAdmin != null)
            {
                await AssignUserToAdmin(user, availableAdmin);

                await Clients.Caller.SendAsync("ChatSessionCreated", new
                {
                    SessionId = user.UserId,
                    UserName = user.UserName,
                    Message = $"Xin chào! Tôi là {availableAdmin.AdminName} rất vui được hỗ trợ bạn"
                });

                return true;
            }
            else if (_adminSessions.Count > 0)
            {
                await Clients.Group("Admins").SendAsync("NewCustomerJoined", new
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    JoinTime = user.StartTime,
                    AssignedAdminId = (string)null,
                    IsAssigned = false
                });

                await Clients.Caller.SendAsync("ChatSessionCreated", new
                {
                    SessionId = user.UserId,
                    UserName = user.UserName,
                    Message = $"Chào mừng {user.UserName}! Tất cả nhân viên hỗ trợ hiện đang bận. Hãy để lại tin nhắn và chúng tôi sẽ hỗ trợ bạn sớm nhất có thể."
                });
                return false;
            }
            else
            {
                await Clients.Caller.SendAsync("ChatSessionCreated", new
                {
                    SessionId = user.UserId,
                    UserName = user.UserName,
                    Message = $"Chào mừng {user.UserName}! Hiện tại chưa có nhân viên hỗ trợ online. Hãy để lại tin nhắn và chúng tôi sẽ phản hồi sớm nhất có thể."
                });
                return false;
            }
        }

        private async Task AssignUserToAdmin(UserSession user, AdminSession admin, AdminSession oldAdmin = null)
        {
            if (user.AssignedAdminId == admin.AdminId && admin.AssignedUsers.Contains(user.UserId))
                return;

            var oldAssignedAdminId = user.AssignedAdminId;

            if (admin.AssignedUsers.Count >= MaxUserAssignToAdmin)
                return;

            if (!string.IsNullOrEmpty(oldAssignedAdminId) && oldAssignedAdminId != admin.AdminId)
            {
                if (_adminSessions.TryGetValue(oldAssignedAdminId, out var oldAdminSession))
                {
                    oldAdminSession.AssignedUsers.Remove(user.UserId);
                }
            }

            user.AssignedAdminId = admin.AdminId;
            if (!admin.AssignedUsers.Contains(user.UserId))
                admin.AssignedUsers.Add(user.UserId);

            await _chatService.AssignAdminToSessionAsync(user.SessionId, admin.AdminId, admin.AdminName);

            var chatMessage = new ChatMessage
            {
                FromUserId = admin.AdminId,
                FromUserName = admin.AdminName,
                ToUserId = user.UserId,
                Message = $"Xin chào! Tôi là {admin.AdminName}, rất vui được hỗ trợ bạn hôm nay.",
                IsFromAdmin = true
            };

            if (string.IsNullOrEmpty(oldAssignedAdminId) || oldAssignedAdminId != admin.AdminId)
            {
                await Clients.Client(user.ConnectionId).SendAsync("AdminAssigned", new
                {
                    AdminId = admin.AdminId,
                    AdminName = admin.AdminName,
                    Message = $"Bạn đã được kết nối với {admin.AdminName}.",
                    ChatMessage = chatMessage
                });

                await _chatService.SaveMessageAsync(chatMessage, user.SessionId);
            }

            await Clients.Client(admin.ConnectionId).SendAsync("UserAssigned", new
            {
                SessionId = user.SessionId,
                CustomerId = user.UserId,
                CustomerName = user.UserName,
                StartTime = user.StartTime,
                IsOnline = user.IsOnline,
                LastMessage = "",
                LastMessageTime = user.StartTime,
                UnreadCount = user.WaitingMessages?.Count ?? 0,
                WaitingMessages = user.WaitingMessages
            });

            user.WaitingMessages.Clear();
        }

        private async Task AutoAssignWaitingUsersToAdmin(AdminSession admin)
        {
            var waitingUsers = _userSessions.Values
                .Where(u => u.IsOnline && string.IsNullOrEmpty(u.AssignedAdminId))
                .OrderBy(u => u.StartTime)
                .Take(MaxUserAssignToAdmin)
                .ToList();

            foreach (var user in waitingUsers)
            {
                if (admin.AssignedUsers.Count >= MaxUserAssignToAdmin)
                    break;

                await AssignUserToAdmin(user, admin);
                await Task.Delay(100);
            }
        }

        private async Task BroadcastCustomerListUpdate()
        {
            var customerList = _userSessions.Values
                .Where(u => u.IsOnline)
                .Select(u => new
                {
                    u.UserId,
                    u.UserName,
                    u.StartTime,
                    u.AssignedAdminId,
                    IsAssigned = !string.IsNullOrEmpty(u.AssignedAdminId),
                    LastMessage = "",
                    LastMessageTime = u.StartTime,
                    UnreadCount = 0
                })
                .OrderByDescending(u => u.StartTime)
                .ToList();

            await Clients.Group("Admins").SendAsync("CustomerListUpdated", customerList);
        }

        public async Task SendMessageToSupport(string message)
        {
            if (!_connectionToUser.TryGetValue(Context.ConnectionId, out var userId) ||
                !_userSessions.TryGetValue(userId, out var session))
            {
                await Clients.Caller.SendAsync("Error", "Phiên chat không hợp lệ");
                return;
            }

            // Kiểm tra trạng thái phiên chat
            var dbSession = await _chatService.GetSessionByIdAsync(session.SessionId);
            if (dbSession.Status == ChatSessionStatus.Closed)
            {
                await Clients.Caller.SendAsync("ChatClosed", new {sessionId = dbSession.SessionId});
                return;
            }

            var chatMessage = new ChatMessage
            {
                SessionId = session.SessionId,
                FromUserId = session.UserId,
                FromUserName = session.UserName,
                Message = message,
                IsFromAdmin = false
            };

            bool messageSentToAdmin = false;

            if (!string.IsNullOrEmpty(session.AssignedAdminId))
            {
                if (_adminSessions.TryGetValue(session.AssignedAdminId, out var assignedAdmin))
                {
                    chatMessage.ToUserId = assignedAdmin.AdminId;

                    await _chatService.SaveMessageAsync(chatMessage, session.SessionId);

                    await Clients.Client(assignedAdmin.ConnectionId).SendAsync("ReceiveMessage", chatMessage);
                    messageSentToAdmin = true;
                }
            }

            if (!messageSentToAdmin)
            {
                await _chatService.SaveMessageAsync(chatMessage, session.SessionId);

                session.WaitingMessages.Add(chatMessage);

                if (_adminSessions.Count > 0)
                {
                    await Clients.Group("Admins").SendAsync("ReceiveMessage", chatMessage);
                }

                await Clients.Caller.SendAsync("MessageQueued", new
                {
                    Message = "Tin nhắn của bạn đã được ghi nhận và đang chờ xử lý."
                });
            }

            await Clients.Caller.SendAsync("MessageSent", chatMessage);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Cashier")]
        public async Task SendMessageToCustomer(string customerId, string message)
        {
            if (!_userSessions.TryGetValue(customerId, out var customerSession))
            {
                await Clients.Caller.SendAsync("Error", "Khách hàng không tồn tại hoặc đã offline");
                return;
            }

            var adminSession = _adminSessions.Values.FirstOrDefault(a => a.ConnectionId == Context.ConnectionId);
            if (adminSession == null)
            {
                await Clients.Caller.SendAsync("Error", "Phiên admin không hợp lệ");
                return;
            }

            // Kiểm tra trạng thái phiên chat
            var dbSession = await _chatService.GetSessionByIdAsync(customerSession.SessionId);
            if (dbSession.Status == ChatSessionStatus.Closed)
            {
                await Clients.Caller.SendAsync("ChatClosed", new {sessionId = dbSession.SessionId});
                return;
            }

            var chatMessage = new ChatMessage
            {
                FromUserId = adminSession.AdminId,
                FromUserName = adminSession.AdminName,
                ToUserId = customerId,
                Message = message,
                IsFromAdmin = true
            };

            await _chatService.SaveMessageAsync(chatMessage, customerSession.SessionId);

            if (customerSession.IsOnline)
            {
                await Clients.Client(customerSession.ConnectionId).SendAsync("ReceiveMessage", chatMessage);
            }

            await Clients.Caller.SendAsync("MessageSent", chatMessage);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Cashier")]
        public async Task GetOnlineCustomers()
        {
            var onlineCustomers = new List<object>();

            foreach (var userSession in _userSessions.Values.Where(u => u.IsOnline))
            {
                try
                {
                    var lastMessage = await _chatService.GetLastMessageAsync(userSession.SessionId);
                    var unreadCount = await _chatService.GetUnreadMessageCountAsync(userSession.SessionId, false);

                    onlineCustomers.Add(new
                    {
                        userSession.UserId,
                        userSession.UserName,
                        userSession.StartTime,
                        userSession.AssignedAdminId,
                        IsAssigned = !string.IsNullOrEmpty(userSession.AssignedAdminId),
                        LastMessage = lastMessage?.Message ?? "",
                        LastMessageTime = lastMessage?.Timestamp ?? userSession.StartTime,
                        UnreadCount = unreadCount,
                        SessionId = userSession.SessionId
                    });
                }
                catch (Exception ex)
                {
                    onlineCustomers.Add(new
                    {
                        userSession.UserId,
                        userSession.UserName,
                        userSession.StartTime,
                        userSession.AssignedAdminId,
                        IsAssigned = !string.IsNullOrEmpty(userSession.AssignedAdminId),
                        LastMessage = "Error loading message",
                        LastMessageTime = userSession.StartTime,
                        UnreadCount = 0,
                        SessionId = userSession.SessionId
                    });
                }
            }

            await Clients.Caller.SendAsync("OnlineCustomers", onlineCustomers.OrderByDescending(c => ((DateTime)c.GetType().GetProperty("StartTime").GetValue(c))).ToList());
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Cashier")]
        public async Task GetRecentSessions(int hours = 24, int pageSize = 50)
        {
            try
            {
                var adminId = GetAdminId();
                var recentSessions = await _chatService.GetRecentSessionsAsync(adminId, hours, pageSize);

                var sessionList = recentSessions.Select(session => new
                {
                    session.SessionId,
                    session.CustomerId,
                    session.CustomerName,
                    session.AdminId,
                    session.AdminName,
                    session.StartTime,
                    session.EndTime,
                    Status = session.Status.ToString(),
                    IsOnline = _userSessions.ContainsKey(session.CustomerId) && _userSessions[session.CustomerId].IsOnline,
                    LastMessage = session.Messages?.LastOrDefault()?.Message ?? "",
                    LastMessageTime = session.Messages?.LastOrDefault()?.Timestamp ?? session.StartTime,
                    MessageCount = session.Messages?.Count ?? 0
                }).ToList();

                await Clients.Caller.SendAsync("RecentSessions", sessionList);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", $"Lỗi khi lấy danh sách session: {ex.Message}");
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Cashier")]
        public async Task GetChatHistory(string sessionId, int pageSize = 50, int pageNumber = 1)
        {
            try
            {
                var session = await _chatService.GetSessionByIdAsync(sessionId);
                var messages = await _chatService.GetChatHistoryAsync(sessionId, pageSize, pageNumber);

                var history = new
                {
                    CustomerId = session.CustomerId,
                    CustomerName = session.CustomerName,
                    SessionId = session.SessionId,
                    StartTime = session.StartTime,
                    Messages = messages.Select(m => new
                    {
                        m.MessageId,
                        m.FromUserId,
                        m.FromUserName,
                        m.ToUserId,
                        m.Message,
                        m.Timestamp,
                        m.IsFromAdmin,
                        m.Status,
                        m.IsRead
                    }).ToList(),
                    PageInfo = new
                    {
                        PageSize = pageSize,
                        PageNumber = pageNumber,
                        HasMore = messages.Count == pageSize
                    }
                };

                await Clients.Caller.SendAsync("ChatHistory", history);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", $"Lỗi khi lấy lịch sử chat: {ex.Message}");
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // Nếu là customer
            if (_connectionToUser.TryRemove(Context.ConnectionId, out var userId))
            {
                if (_userSessions.TryGetValue(userId, out var session))
                {
                    session.IsOnline = false;

                    // Đợi 7 giây để phân biệt disconnect thật sự hay F5
                    await Task.Delay(7000);

                    // Nếu user không reconnect lại
                    if (_userSessions.TryGetValue(userId, out var checkSession) && checkSession.IsOnline)
                        return;

                    // Đóng phiên chat trong database
                    await _chatService.CloseSessionAsync(session.SessionId);

                    // Thông báo cho admin nếu có
                    if (!string.IsNullOrEmpty(session.AssignedAdminId) &&
                        _adminSessions.TryGetValue(session.AssignedAdminId, out var assignedAdmin))
                    {
                        assignedAdmin.AssignedUsers.Remove(userId);

                        await Clients.Client(assignedAdmin.ConnectionId).SendAsync("CustomerDisconnected", new
                        {
                            sessionId = session.SessionId
                        });
                    }

                    await BroadcastCustomerListUpdate();
                }
            }


            // Nếu là admin
            var adminSession = _adminSessions.Values.FirstOrDefault(a => a.ConnectionId == Context.ConnectionId);
            if (adminSession != null)
            {
                var adminId = adminSession.AdminId;
                var oldConnId = adminSession.ConnectionId;

                // Đợi 7 giây để phân biệt disconnect thật sự hay F5
                await Task.Delay(7000);

                // Nếu admin đã reconnect thì bỏ qua
                if (_adminSessions.ContainsKey(adminId) && _adminSessions[adminId].ConnectionId != oldConnId)
                    return;

                // Xóa admin session
                _adminSessions.TryRemove(adminId, out _);

                // Đóng tất cả phiên chat mà admin này phụ trách
                var orphanedCustomerIds = new List<string>(adminSession.AssignedUsers);

                foreach (var customerId in orphanedCustomerIds)
                {
                    if (_userSessions.TryGetValue(customerId, out var customerSession))
                    {
                        // Đóng phiên chat trong database
                        await _chatService.CloseSessionAsync(customerSession.SessionId);

                        // Thông báo cho khách hàng
                        if (customerSession.IsOnline)
                        {
                            await Clients.Client(customerSession.ConnectionId).SendAsync("AdminDisconnected", new
                            {
                               sessionID  = customerSession.SessionId
                            });
                        }
                    }
                }

                await Clients.Group("Admins").SendAsync("AdminLeft", new
                {
                    AdminId = adminSession.AdminId,
                    AdminName = adminSession.AdminName
                });

                await BroadcastCustomerListUpdate();
            }

            await base.OnDisconnectedAsync(exception);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Cashier")]
        public async Task CloseSession(string sessionId)
        {
            // Đóng trạng thái phiên chat trong database
            await _chatService.CloseSessionAsync(sessionId);

            // Tìm user đang thuộc phiên này
            var userSession = _userSessions.Values.FirstOrDefault(u => u.SessionId == sessionId);

            if (userSession != null && userSession.IsOnline)
            {
                await Clients.Client(userSession.ConnectionId).SendAsync("ChatClosed", new { sessionId = sessionId });
            }
        }


        private string GetAdminId()
        {
            return Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
           ?? throw new UnauthorizedAccessException("User không hợp lệ");
        }

        private string GetAdminName()
        {
            return Context.User?.FindFirst(ClaimTypes.Name)?.Value ?? throw new UnauthorizedAccessException("User không hợp lệ");
        }
    }

    public class UserSession
    {
        public string SessionId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ConnectionId { get; set; }
        public DateTime StartTime { get; set; }
        public bool IsOnline { get; set; }
        public string? AssignedAdminId { get; set; }
        public List<ChatMessage> WaitingMessages { get; set; } = new();
    }

    public class AdminSession
    {
        public string AdminId { get; set; }
        public string AdminName { get; set; }
        public string ConnectionId { get; set; }
        public List<string> AssignedUsers { get; set; } = new();
    }
}


