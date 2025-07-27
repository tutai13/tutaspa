<template>
  <div class="chat-container">
    <!-- Sidebar -->
    <div class="sidebar">
      <div class="sidebar-header">
        <h2>Đoạn chat</h2>
        <div class="header-actions">
          <button class="btn-icon" @click=" ">
            <i class="fas fa-sync"></i>
          </button>
        </div>
      </div>

      <!-- Search -->
      <div class="search-container">
        <div class="search-box">
          <i class="fas fa-search"></i>
          <input type="text" placeholder="Tìm tên khách hàng" v-model="searchQuery" />
        </div>
      </div>

      <!-- Filters -->
      <div class="filters">
        <button 
          class="filter-btn" 
          :class="{ active: currentFilter === 'all' }"
          @click="setFilter('all')"
        >
          Tất cả
        </button>
        <button 
          class="filter-btn" 
          :class="{ active: currentFilter === 'online' }"
          @click="setFilter('online')"
        >
          Đang online
        </button>
        <button 
          class="filter-btn" 
          :class="{ active: currentFilter === 'unread' }"
          @click="setFilter('unread')"
        >
          Chưa đọc
        </button>
        <button 
          class="filter-btn" 
          :class="{ active: currentFilter === 'recent' }"
          @click="setFilter('recent')"
        >
          Gần đây
        </button>
      </div>

      <!-- Loading indicator -->
      <div v-if="isLoading" class="loading-indicator">
        <i class="fas fa-spinner fa-spin"></i>
        <span>Đang tải...</span>
      </div>

      <!-- Customer List -->
      <div class="customer-list" v-else>
        <div 
          v-for="customer in filteredCustomers" 
          :key="customer.sessionId || customer.userId"
          class="customer-item"
          :class="{ 
            active: (selectedCustomer && (
              (selectedCustomer.sessionId && selectedCustomer.sessionId === customer.sessionId) ||
              (selectedCustomer.userId && selectedCustomer.userId === customer.userId)
            )),
            online: customer.isOnline,
            'has-unread': customer.unreadCount > 0
          }"
          @click="selectCustomer(customer)"
        >
         
          <div class="customer-info">
            <div class="customer-name-row">
              <span class="customer-name">{{ customer.userName || customer.customerName }}</span>
            </div>
            <div class="last-message">
              <i v-if="customer.isFromAdmin" class="fas fa-reply message-icon"></i>
              {{ customer.lastMessage || 'Chưa có tin nhắn' }}
            </div>
            <div class="session-info">
              <!-- <span class="session-status" :class="customer.status">
                {{ getStatusText(customer.status) }}
              </span> -->
              <span v-if="customer.messageCount" class="message-count">
                • {{ customer.messageCount }} tin nhắn
              </span>
            </div>
          </div>
          
          <div class="customer-meta">
            <div class="time">{{ formatTime(customer.lastMessageTime || customer.startTime) }}</div>
            <div v-if="customer.unreadCount > 0" class="unread-count">
              {{ customer.unreadCount }}
            </div>
            <div v-if="!customer.isOnline && customer.status !== 'Closed'" class="offline-indicator">
              <i class="fas fa-circle"></i>
            </div>
          </div>
        </div>

        <!-- Empty state for filter -->
        <div v-if="filteredCustomers.length === 0" class="empty-filter">
          <i class="fas fa-search"></i>
          <p>Không tìm thấy cuộc trò chuyện nào</p>
        </div>
      </div>
    </div>

    <!-- Chat Area -->
    <div class="chat-area">
      <div v-if="selectedCustomer" class="chat-content">
        <!-- Chat Header -->
        <div class="chat-header">
          <div class="customer-info">
            <img 
              v-if="selectedCustomer.isOnline" 
              :src="'https://upload.wikimedia.org/wikipedia/commons/7/7c/Profile_avatar_placeholder_large.png?20150327203541'" 
              alt="Avatar" 
              class="customer-avatar">
            <div>
              <h3>{{ selectedCustomer.userName || selectedCustomer.customerName }}</h3>
              <div class="status-info">
                <span class="status" :class="{ online: selectedCustomer.isOnline }">
                  {{ selectedCustomer.isOnline ? 'Đang hoạt động' : 'Không hoạt động' }}
                </span>
                <!-- <span class="session-id">• ID: {{ selectedCustomer.sessionId || selectedCustomer.userId }}</span> -->
              </div>
            </div>
          </div>
          <div class="chat-actions">
            <button class="btn-icon" @click="assignCustomerToMe" v-if="!selectedCustomer.isAssigned">
              <i class="fas fa-user-plus" title="Nhận phụ trách"></i>
            </button>
            <button class="btn-icon" @click="refreshHistory">
              <i class="fas fa-sync" title="Làm mới"></i>
            </button>
            <button class="btn-icon" @click="startCall">
              <i class="fas fa-phone"></i>
            </button>
            <button class="btn-icon" @click="startVideoCall">
              <i class="fas fa-video"></i>
            </button>
            <button class="btn-icon" @click="showInfo">
              <i class="fas fa-info-circle"></i>
            </button>
          </div>
        </div>

        <!-- Messages -->
        <div class="messages-container" ref="messagesContainer" id="messagesContainer">
          <div v-if="isLoadingHistory" class="loading-history">
            <i class="fas fa-spinner fa-spin"></i>
            <span>Đang tải lịch sử...</span>
          </div>
          
          <div v-for="message in currentMessages" :key="message.messageId" class="message-wrapper">
            <div 
              class="message"
              :class="{ 'message-sent': message.isFromAdmin, 'message-received': !message.isFromAdmin }"
            >
              <div class="message-content">
                <p>{{ message.message }}</p>
              </div>
              <div class="message-time">
                {{ formatTime(message.timestamp) }}
                <i v-if="message.isRead && message.isFromAdmin" class="fas fa-check-double read-status"></i>
                <i v-else-if="message.isFromAdmin" class="fas fa-check sent-status"></i>
              </div>
            </div>
          </div>
          
          <!-- Load more button -->
          <div v-if="hasMoreMessages" class="load-more-container">
            <button class="load-more-btn" @click="loadMoreMessages" :disabled="isLoadingMore">
              <i v-if="isLoadingMore" class="fas fa-spinner fa-spin"></i>
              <span>{{ isLoadingMore ? 'Đang tải...' : 'Tải thêm tin nhắn' }}</span>
            </button>
          </div>
        </div>

        <!-- Closed notice -->
          <div v-if="selectedCustomer.status === 'Closed'" class="closed-notice">
            <i class="fas fa-lock"></i>
            Phiên chat đã kết thúc.
          </div>

        <!-- Input Area -->
        <div class="input-area" v-else>
          <div class="input-container">
            <button class="btn-icon">
              <i class="fas fa-plus"></i>
            </button>
            <button class="btn-icon">
              <i class="fas fa-image"></i>
            </button>
            <button class="btn-icon">
              <i class="fas fa-microphone"></i>
            </button>
            <button class="btn-icon">
              <i class="fas fa-gift"></i>
            </button>
            <div class="message-input">
              <input 
                type="text" 
                v-model="newMessage" 
                @keyup.enter="sendMessage"
                placeholder="Aa"
                ref="messageInput"
                :disabled="selectedCustomer.status === 'Closed'"
              />
              <button class="btn-icon">
                <i class="fas fa-smile"></i>
              </button>
            </div>
            <button 
              class="btn-icon send-btn" 
              @click="sendMessage" 
              :disabled="!newMessage.trim() || selectedCustomer.status === 'Closed'"
            >
              <i class="fas fa-paper-plane"></i>
            </button>
          </div>
          
          
        </div>
      </div>

      <!-- Empty State -->
      <div v-else class="empty-state">
        <div class="empty-icon">
          <i class="fas fa-comments"></i>
        </div>
        <h3>Chọn một cuộc trò chuyện</h3>
        <p>Chọn một khách hàng để bắt đầu trò chuyện</p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, computed, onMounted, onBeforeUnmount, nextTick, watch } from 'vue'
import * as signalR from '@microsoft/signalr'
import { useRouter } from 'vue-router'
import { getSignalRConnection, registerSignalREvent, getConnectionState, createSignalRConnection } from '../services/chatService'
import { authAPI } from '../services/authservice' // Thêm dòng này

// Router instance
const router = useRouter()

// Environment variable
const base_url = import.meta.env.VITE_CHAT_URL

// State
const searchQuery = ref('')
const selectedCustomer = ref(null)
const newMessage = ref('')
const customers = ref([])
const recentSessions = ref([])
const messages = reactive({})
const currentFilter = ref('all')
const isLoading = ref(true)
const isLoadingHistory = ref(false)
const isLoadingMore = ref(false)
const hasMoreMessages = ref(false)
const currentPage = ref(1)
const sessionId = router.currentRoute.value.query.sessionId || null
const connectionReady = ref(false)
const adminInfo = {
  adminId: 'admin_001',
  adminName: 'Admin Support'
}

// Computed
const filteredCustomers = computed(() => {
  let filtered = [...customers.value]
  
  // Apply search filter  
  if (searchQuery.value) {
    const query = searchQuery.value.toLowerCase()
    filtered = filtered.filter(customer =>
      (customer.userName || customer.customerName || '').toLowerCase().includes(query) ||
      (customer.lastMessage || '').toLowerCase().includes(query)
    )
  }
  
  // Apply category filter
  switch (currentFilter.value) {
    case 'online':
      filtered = filtered.filter(c => c.status !== 'Closed' && c.isOnline)
      break
    case 'unread':
      filtered = filtered.filter(c => c.unreadCount > 0)
      break
    case 'recent':
      filtered = filtered.filter(c => {
        const lastActivity = new Date(c.lastMessageTime || c.startTime)
        const hoursSinceLastActivity = (Date.now() - lastActivity) / (1000 * 60 * 60)
        return hoursSinceLastActivity <= 24
      })
      break
  }
  
  // Sort: online first, then by last message time
  return filtered.sort((a, b) => {
    if (a.isOnline && !b.isOnline) return -1
    if (!a.isOnline && b.isOnline) return 1
    
    const aTime = new Date(a.lastMessageTime || a.startTime)
    const bTime = new Date(b.lastMessageTime || b.startTime)
    return bTime - aTime
  })
})

const currentMessages = computed(() => {
  if (!selectedCustomer.value) return []
  const sessionKey = selectedCustomer.value.sessionId || selectedCustomer.value.userId
  return messages[sessionKey] || []
})

// Helper function to wait for connection
async function waitForConnection(maxAttempts = 30) {
  let attempts = 0
  
  while (attempts < maxAttempts) {
    const conn = getSignalRConnection()
    const state = getConnectionState()
    
    console.log(`Connection attempt ${attempts + 1}, state: ${state}`)
    
    if (conn && state === 'Connected') {
      connectionReady.value = true
      return conn
    }
    
    // Wait 500ms before next attempt
    await new Promise(resolve => setTimeout(resolve, 500))
    attempts++
  }
  
  throw new Error(`Connection not ready after ${maxAttempts} attempts`)
}

// Safe invoke function
async function safeInvoke(methodName, ...args) {
  try {
    const conn = await waitForConnection()
    return await conn.invoke(methodName, ...args)
  } catch (error) {
    if (error.message && error.message.includes('unauthorized')) {
      // Thử refresh token qua authAPI
      try {
        const response = await authAPI.refreshToken()
        const newToken = response.accessToken
        if (newToken) {
          localStorage.setItem('accessToken', newToken)
          // Tạo lại connection với token mới
          await createSignalRConnection(newToken)
          // Thử lại lệnh invoke
          const conn = await waitForConnection()
          return await conn.invoke(methodName, ...args)
        } else {
          router.push('/login?return_url=chat')
        }
      } catch (refreshError) {
        console.error('Refresh token failed:', refreshError)
        router.push('/login?return_url=chat')
      }
    }
    console.error(`Error invoking ${methodName}:`, error)
    throw error
  }
}

// Lifecycle hooks
onMounted(async () => {
  await initializeSignalR()

  // Lấy sessionId từ route và tự động chọn customer, load lịch sử
  const sessionIdFromRoute = router.currentRoute.value.query.sessionId
  console.log('Session ID from route:', sessionIdFromRoute)
  if (sessionIdFromRoute) {
    // Đợi dữ liệu load xong
    await nextTick()
    // Tìm customer theo sessionId
    const customer = customers.value.find(
      c => c.sessionId === sessionIdFromRoute
    )
    if (customer) {
      await selectCustomer(customer)
    } else {
      // Nếu chưa có trong danh sách, thử lấy lịch sử trực tiếp
      selectedCustomer.value = {
        sessionId: sessionIdFromRoute,
        userId: null,
        userName: 'Khách hàng',
        customerName: 'Khách hàng',
        isOnline: false,
        status: '',
        unreadCount: 0,
        lastMessage: '',
        lastMessageTime: '',
        isAssigned: false
      }
      await selectCustomer(selectedCustomer.value)
    }
  }
})

onBeforeUnmount(() => {
  const conn = getSignalRConnection()
  if (conn) {
    conn.stop()
  }
})

// Watch for filter changes
watch(currentFilter, () => {
  // Reset selection if current customer is filtered out
  if (selectedCustomer.value && !filteredCustomers.value.find(c => 
    (c.sessionId || c.userId) === (selectedCustomer.value.sessionId || selectedCustomer.value.userId)
  )) {
    selectedCustomer.value = null
  }
})

// Functions
async function initializeSignalR() {
  try {
    const token = getAuthToken()
    
    // Create connection
    const conn = await createSignalRConnection(token)
    
    if (!conn) {
      throw new Error('Failed to create SignalR connection')
    }

    // Wait for connection to be ready
    await waitForConnection()

    // Đăng ký các event qua chatService để an toàn
    registerSignalREvent('OnlineCustomers', handleOnlineCustomers)
    registerSignalREvent('RecentSessions', handleRecentSessions)
    registerSignalREvent('NewCustomerJoined', handleNewCustomerJoined)
    registerSignalREvent('UserAssigned', handleUserAssigned)
    registerSignalREvent('ReceiveMessage', handleReceiveMessage)
    registerSignalREvent('CustomerDisconnected', handleChatClosed)
    registerSignalREvent('ChatHistory', handleChatHistory)
    registerSignalREvent('MessageSent', handleMessageSent)
    registerSignalREvent('AdminAssigned', handleAdminAssigned)
    registerSignalREvent('Error', handleError)
    registerSignalREvent('ChatClosed', handleChatClosed);

    // Lắng nghe sự kiện reconnect thành công để load lại dữ liệu
    conn.onreconnected(async () => {
      console.log('SignalR reconnected, reload data...')
      connectionReady.value = false
      await waitForConnection()
      await loadInitialData()
    })

    // Load initial data after connection is ready
    await loadInitialData()

  } catch (error) {
    console.error('SignalR Connection Error:', error)
    isLoading.value = false
    connectionReady.value = false
    
    // Retry after 3 seconds
    setTimeout(() => {
      console.log('Retrying SignalR connection...')
      initializeSignalR()
    }, 3000)
  }
}

async function loadInitialData() {
  try {
    // Ensure connection is ready
    await waitForConnection()
    
    console.log('Loading initial data...')
    
    // Load data using safe invoke
    await Promise.all([
      safeInvoke('GetOnlineCustomers'),
      safeInvoke('GetRecentSessions', 72, 50)
    ])
    
    console.log('Initial data loaded successfully')
    
  } catch (error) {
    console.error('Error loading initial data:', error)
    
    // Retry after 2 seconds
    setTimeout(() => {
      console.log('Retrying load initial data...')
      loadInitialData()
    }, 2000)
  } finally {
    isLoading.value = false
  }
}


function handleChatClosed(data) {
  // data: { sessionId }
  // Cập nhật trạng thái Closed cho cả recentSessions và customers
  if (!data || !data.sessionId) return;

  // Cập nhật trong recentSessions
  const session = recentSessions.value.find(s => s.sessionId === data.sessionId);
  if (session) {
    session.status = 'Closed';
  }

  // Cập nhật trong customers
  const customer = customers.value.find(c => c.sessionId === data.sessionId);
  if (customer) {
    customer.status = 'Closed';
  }

  // Nếu đang mở phiên này thì cập nhật UI
  if (selectedCustomer.value && selectedCustomer.value.sessionId === data.sessionId) {
    selectedCustomer.value.status = 'Closed';
  }
}

function handleOnlineCustomers(onlineList) {
  console.log('Received online customers:', onlineList)
  // Update existing customers or add new ones
  onlineList.forEach(customer => {
    const existingIndex = customers.value.findIndex(c => 
      c.sessionId === customer.sessionId || c.userId === customer.userId
    )
    
    if (existingIndex !== -1) {
      // Update existing customer
      Object.assign(customers.value[existingIndex], {
        ...customer,
        isOnline: true
      })
    } else {
      // Add new online customer
      customers.value.unshift({
        ...customer,
        isOnline: true
      })
    }
  })
}

function handleRecentSessions(sessions) {
  console.log('Received recent sessions:', sessions)

  console.log(sessions)
  recentSessions.value = sessions
  
  // Merge with existing customers list
  sessions.forEach(session => {
    const existingIndex = customers.value.findIndex(c => 
      c.sessionId === session.sessionId || c.userId === session.customerId
    )
    
    const customerData = {
      sessionId: session.sessionId,
      userId: session.customerId,
      userName: session.customerName,
      customerName: session.customerName,
      adminId: session.adminId,
      adminName: session.adminName,
      startTime: session.startTime,
      endTime: session.endTime,
      status: session.status,
      isOnline: session.isOnline || false,
      lastMessage: session.lastMessage,
      lastMessageTime: session.lastMessageTime,
      messageCount: session.messageCount,
      unreadCount: 0, // Will be updated by message events
      isAssigned: !!session.adminId,
      isFromAdmin: session.lastMessage && session.lastMessage.includes('Xin chào') // Simple detection
    }
    
    if (existingIndex !== -1) {
      // Update existing
      Object.assign(customers.value[existingIndex], customerData)
    } else {
      // Add new
      customers.value.push(customerData)
    }
  })
  
  // Sort customers
  customers.value.sort((a, b) => {
    if (a.isOnline && !b.isOnline) return -1
    if (!a.isOnline && b.isOnline) return 1
    
    const aTime = new Date(a.lastMessageTime || a.startTime)
    const bTime = new Date(b.lastMessageTime || b.startTime)
    return bTime - aTime
  })
}

function handleNewCustomerJoined(customer) {
  console.log('New customer joined:', customer)
  const existingIndex = customers.value.findIndex(c => 
    c.userId === customer.userId || c.sessionId === customer.sessionId
  )
  
  const customerData = {
    userId: customer.userId,
    sessionId: customer.sessionId,
    userName: customer.userName,
    customerName: customer.customerName || customer.userName,
    startTime: customer.startTime || customer.joinTime,
    isOnline: true,
    unreadCount: customer.unreadCount || 0,
    lastMessage: '',
    lastMessageTime: customer.startTime || customer.joinTime,
    isAssigned: customer.isAssigned || false,
    adminId: customer.assignedAdminId,
    adminName: customer.assignedAdminName
  }
  
  if (existingIndex !== -1) {
    Object.assign(customers.value[existingIndex], customerData)
  } else {
    customers.value.unshift(customerData)
  }

  // Handle waiting messages
  if (customer.waitingMessages && customer.waitingMessages.length > 0) {
    customer.waitingMessages.forEach(message => {
      handleReceiveMessage(message)
    })
  }
}

function handleUserAssigned(assignment) {
  console.log('User assigned:', assignment)
  
  let customer = customers.value.find(c => c.userId === assignment.customerId)
  
  if (!customer) {
    // ➕ Nếu chưa có, thêm mới vào danh sách
    customer = {
      sessionId: assignment.sessionId,
      userId: assignment.customerId,
      userName: assignment.customerName,
      customerName: assignment.customerName,
      startTime: assignment.startTime,
      isOnline: assignment.isOnline,
      lastMessage: assignment.lastMessage || '',
      lastMessageTime: assignment.lastMessageTime || assignment.startTime,
      unreadCount: assignment.unreadCount || 0,
      isAssigned: true,
      adminId: assignment.adminId || adminInfo.adminId,
      adminName: assignment.adminName || adminInfo.adminName,
      waitingMessages: assignment.waitingMessages || [],
      isFromAdmin: false
    }

    customers.value.unshift(customer)
  } else {
    // ✅ Nếu đã có, cập nhật thông tin
    customer.isAssigned = true
    customer.adminId = assignment.adminId || adminInfo.adminId
    customer.adminName = assignment.adminName || adminInfo.adminName
    customer.unreadCount = assignment.unreadCount || 0
  }

  // Xử lý message chờ
  if (assignment.waitingMessages && assignment.waitingMessages.length > 0) {
    assignment.waitingMessages.forEach(message => {
      handleReceiveMessage(message)
    })
  }
}

function handleReceiveMessage(message) {
  console.log('Received message:', message)
  const sessionKey = message.sessionId || message.fromUserId

  if (!messages[sessionKey]) {
    messages[sessionKey] = []
  }
  
  // Avoid duplicate messages
  const existingMessage = messages[sessionKey].find(m => m.messageId === message.messageId)
  if (!existingMessage) {
    messages[sessionKey].push(message)
  }

  // Update customer info
  const customer = customers.value.find(c => 
    c.sessionId === sessionKey || 
    c.userId === message.fromUserId || 
    c.userId === message.toUserId
  )
  
  if (customer) {
    customer.lastMessage = message.message
    customer.lastMessageTime = message.timestamp
    customer.isFromAdmin = message.isFromAdmin

    // Update unread count if not currently selected
    if (!selectedCustomer.value || 
        (selectedCustomer.value.sessionId || selectedCustomer.value.userId) !== sessionKey) {
      if (!message.isFromAdmin) {
        customer.unreadCount = (customer.unreadCount || 0) + 1
      }
    }

    // Move to top of list
    const index = customers.value.indexOf(customer)
    if (index > 0) {
      customers.value.splice(index, 1)
      customers.value.unshift(customer)
    }
  }

  scrollToBottom()
}

function handleCustomerDisconnected(customer) {
  console.log('Customer disconnected:', customer)
  const existingCustomer = customers.value.find(c => c.userId === customer.userId)
  if (existingCustomer) {
    existingCustomer.isOnline = false
  }
}

function handleChatHistory(history) {
  console.log('Received chat history:', history)
  isLoadingHistory.value = false
  
  const sessionKey = history.sessionId
  console.log('→ messages[sessionKey] before set:', messages[sessionKey])
  if (currentPage.value === 1) {
    messages[sessionKey] = history.messages || []
  } else {
    // Prepend older messages
    const existingMessages = messages[sessionKey] || []
    messages[sessionKey] = [...(history.messages || []), ...existingMessages]
  }
  
  hasMoreMessages.value = history.pageInfo?.hasMore || false
  isLoadingMore.value = false
  
  if (currentPage.value === 1) {
    nextTick(() => scrollToBottom())
  }
}

function handleMessageSent(message) {
  // Message confirmed sent
  console.log('Message sent:', message)
}

function handleAdminAssigned(assignment) {
  console.log('Admin assigned:', assignment)
}

function handleError(error) {
  console.error('SignalR Error:', error)
  // You can add toast notification here
}

async function selectCustomer(customer) {
  selectedCustomer.value = customer
  customer.unreadCount = 0
  currentPage.value = 1
  hasMoreMessages.value = false

  const sessionId = customer.sessionId || customer.userId
  if (!sessionId) {
    console.error('No sessionId or userId found for customer:', customer)
    return
  }

  const sessionKey = sessionId
  messages[sessionKey] = []

  isLoadingHistory.value = true

  try {
    await safeInvoke('GetChatHistory', sessionId, 50, currentPage.value)
  } catch (error) {
    console.error('Error selecting customer:', error)
    isLoadingHistory.value = false
  }

  nextTick(() => {
    const input = document.getElementById('messageInput')
    if (input) input.focus()
  })
}

async function loadChatHistory(customer) {
  try {
    await safeInvoke('GetChatHistory', customer.sessionId, 50, currentPage.value)
  } catch (error) {
    console.error('Error loading chat history:', error)
    isLoadingHistory.value = false
  }
}

async function loadMoreMessages() {
  if (!selectedCustomer.value || isLoadingMore.value) return
  
  isLoadingMore.value = true
  currentPage.value += 1
  
  try {
    await loadChatHistory(selectedCustomer.value)
  } catch (error) {
    console.error('Error loading more messages:', error)
    isLoadingMore.value = false
    currentPage.value -= 1
  }
}

async function sendMessage() {
  if (!newMessage.value.trim() || !selectedCustomer.value) return

  const message = {
    fromUserId: adminInfo.adminId,
    fromUserName: adminInfo.adminName,
    toUserId: selectedCustomer.value.userId,
    message: newMessage.value,
    timestamp: new Date(),
    isFromAdmin: true,
    messageId: generateMessageId()
  }

  const sessionKey = selectedCustomer.value.sessionId || selectedCustomer.value.userId

  if (!messages[sessionKey]) {
    messages[sessionKey] = []
  }
  messages[sessionKey].push(message)

  selectedCustomer.value.lastMessage = newMessage.value
  selectedCustomer.value.lastMessageTime = new Date()

  try {
    await safeInvoke('SendMessageToCustomer', selectedCustomer.value.userId, newMessage.value)
    newMessage.value = ''
    scrollToBottom()
  } catch (error) {
    console.error('Error sending message:', error)
    // Remove the message from UI if failed
    const messageIndex = messages[sessionKey].findIndex(m => m.messageId === message.messageId)
    if (messageIndex !== -1) {
      messages[sessionKey].splice(messageIndex, 1)
    }
  }
}

async function assignCustomerToMe() {
  if (!selectedCustomer.value) return

  try {
    await safeInvoke('AssignCustomerToMe', selectedCustomer.value.userId)
    selectedCustomer.value.isAssigned = true
    selectedCustomer.value.adminId = adminInfo.adminId
    selectedCustomer.value.adminName = adminInfo.adminName
  } catch (error) {
    console.error('Error assigning customer:', error)
  }
}

async function refreshHistory() {
  if (!selectedCustomer.value) return
  
  currentPage.value = 1
  const sessionKey = selectedCustomer.value.sessionId || selectedCustomer.value.userId
  messages[sessionKey] = []
  
  isLoadingHistory.value = true
  await loadChatHistory(selectedCustomer.value)
}

async function refreshSessions() {
  isLoading.value = true
  connectionReady.value = false
  await loadInitialData()
}

function setFilter(filter) {
  currentFilter.value = filter
}

function scrollToBottom() {
  nextTick(() => {
    const container = document.getElementById('messagesContainer')
    if (container) {
      container.scrollTop = container.scrollHeight
    }
  })
}

function formatTime(timestamp) {
  const date = new Date(timestamp)
  const now = new Date()
  const diff = now - date

  if (diff < 60000) return 'Vừa xong'
  if (diff < 3600000) return `${Math.floor(diff / 60000)} phút`
  if (diff < 86400000) return `${Math.floor(diff / 3600000)} giờ`
  if (diff < 604800000) return `${Math.floor(diff / 86400000)} ngày`
  return date.toLocaleDateString('vi-VN')
}

function getStatusText(status) {
  const statusMap = {
    'Active': 'Đang hoạt động',
    'Waiting': 'Đang chờ',
    'Assigned': 'Đã được phân công',
    'Closed': 'Đã đóng',
    'Abandoned': 'Đã rời đi'
  }
  return statusMap[status] || status
}

function generateMessageId() {
  return Date.now().toString() + Math.random().toString(36).substr(2, 9)
}

function getAuthToken() {
  const token = localStorage.getItem('accessToken')
  if (!token) {
    router.push('/login')
  }
  return token
}

// Optional functions
function startCall() {
  console.log('Starting call with', selectedCustomer.value?.userName)
}

function startVideoCall() {
  console.log('Starting video call with', selectedCustomer.value?.userName)
}

function showInfo() {
  console.log('Showing info for', selectedCustomer.value?.userName)
}
</script>

<style scoped>
.chat-container {
  display: flex;
  height: 85vh;
  background: #fff; /* Nền trắng */
  color: #222;      /* Chữ đen */
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  overflow: hidden;
}

.sidebar {
  width: 360px;
  background: #f8f9fa; /* Sidebar sáng */
  border-right: 1px solid #e5e7eb;
  display: flex;
  flex-direction: column;
  min-height: 0;
}

.sidebar-header {
  padding: 16px 20px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-bottom: 1px solid #e5e7eb;
}

.sidebar-header h2 {
  margin: 0;
  font-size: 24px;
  font-weight: 700;
  color: #222;
}

.header-actions .btn-icon {
  background: #e5e7eb;
  color: #555;
}
.btn-icon {
  width: 36px;
  height: 36px;
  border-radius: 50%;
  border: none;
  background: #e5e7eb;
  color: #555;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: background 0.2s;
}
.btn-icon:hover {
  background: #d1d5db;
}

.search-container {
  padding: 16px 20px;
}

.search-box {
  position: relative;
  display: flex;
  align-items: center;
  background: #e5e7eb;
  border-radius: 20px;
  padding: 8px 12px;
}
.search-box i {
  color: #888;
  margin-right: 8px;
}
.search-box input {
  flex: 1;
  border: none;
  background: transparent;
  color: #222;
  font-size: 15px;
  outline: none;
}
.search-box input::placeholder {
  color: #888;
}

.filters {
  display: flex;
  gap: 8px;
  padding: 0 20px 16px;
}
.filter-btn {
  padding: 6px 12px;
  border-radius: 16px;
  border: none;
  background: #e5e7eb;
  color: #555;
  cursor: pointer;
  font-size: 14px;
  transition: all 0.2s;
}
.filter-btn.active {
  background: #2563eb;
  color: #fff;
}

.customer-list {
  flex: 1;
  overflow-y: auto;
  min-height: 0;
  background: #fff;
}

.customer-item {
  display: flex;
  align-items: center;
  padding: 12px 20px;
  cursor: pointer;
  transition: background 0.2s;
  background: #fff;
}
.customer-item:hover {
  background: #f1f5f9;
}
.customer-item.active {
  background: #2563eb;
  color: #fff;
}
.customer-item.active .customer-name,
.customer-item.active .last-message,
.customer-item.active .time {
  color: #fff;
}

.customer-avatar {
  position: relative;
  margin-right: 12px;
}
.customer-avatar img {
  width: 56px;
  height: 56px;
  border-radius: 50%;
  object-fit: cover;
}
.online-indicator {
  position: absolute;
  bottom: 2px;
  right: 2px;
  width: 14px;
  height: 14px;
  background: #42b883;
  border: 2px solid #f8f9fa;
  border-radius: 50%;
}

.customer-info {
  flex: 1;
  min-width: 0;
}
.customer-name {
  font-size: 15px;
  font-weight: 600;
  margin-bottom: 4px;
  color: #222;
}
.last-message {
  font-size: 13px;
  color: #555;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}
.customer-meta {
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  gap: 4px;
}
.time {
  font-size: 12px;
  color: #888;
}
.unread-count {
  background: #2563eb;
  color: #fff;
  font-size: 12px;
  font-weight: 600;
  padding: 2px 6px;
  border-radius: 10px;
  min-width: 18px;
  text-align: center;
}

/* Chat Area */
.chat-area {
  flex: 1;
  display: flex;
  flex-direction: column;
  background: #fff;
  min-width: 0;
  min-height: 0;
}
.chat-content {
  flex: 1;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
.chat-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 16px 20px;
  border-bottom: 1px solid #e5e7eb;
  background: #fff;
}
.chat-header .customer-info {
  display: flex;
  align-items: center;
  gap: 12px;
}
.chat-header .customer-info img {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  object-fit: cover;
}
.chat-header .customer-info h3 {
  margin: 0;
  font-size: 16px;
  font-weight: 600;
  color: #222;
}
.chat-header .customer-info .status {
  font-size: 12px;
  color: #888;
}
.chat-actions {
  display: flex;
  gap: 8px;
}

.messages-container {
  flex: 1;
  min-height: 0;
  height: 100%;
  overflow-y: auto;
  padding: 16px 20px;
  background: #fff;
}

.message-wrapper {
  margin-bottom: 8px;
}
.message {
  display: flex;
  flex-direction: column;
  max-width: 70%;
}
.message-sent {
  align-items: flex-end;
  margin-left: auto;
}
.message-received {
  align-items: flex-start;
}
.message-content {
  padding: 8px 12px;
  border-radius: 16px;
  margin-bottom: 2px;
}
.message-sent .message-content {
  background: #2563eb;
  color: #fff;
}
.message-received .message-content {
  background: #f1f5f9;
  color: #222;
}
.message-content p {
  margin: 0;
  font-size: 14px;
  line-height: 1.4;
}
.message-time {
  font-size: 11px;
  color: #888;
  padding: 0 12px;
}

.input-area {
  padding: 16px 20px;
  border-top: 1px solid #e5e7eb;
  background: #fff;
}
.input-container {
  display: flex;
  align-items: center;
  gap: 8px;
}
.message-input {
  flex: 1;
  display: flex;
  align-items: center;
  background: #e5e7eb;
  border-radius: 20px;
  padding: 8px 12px;
}
.message-input input {
  flex: 1;
  border: none;
  background: transparent;
  color: #222;
  font-size: 15px;
  outline: none;
}
.message-input input::placeholder {
  color: #888;
}
.send-btn {
  background: #2563eb !important;
  color: #fff !important;
}
.send-btn:disabled {
  background: #e5e7eb !important;
  color: #888 !important;
  cursor: not-allowed;
}

.empty-state {
  flex: 1;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  color: #888;
}
.empty-icon {
  font-size: 64px;
  margin-bottom: 16px;
  opacity: 0.5;
}
.empty-state h3 {
  margin: 0 0 8px 0;
  font-size: 20px;
  font-weight: 600;
  color: #222;
}
.empty-state p {
  margin: 0;
  font-size: 14px;
}
.closed-notice {
  margin-top: 10px;
  color: #b91c1c;
  background: #fef2f2;
  border: 1px solid #fecaca;
  border-radius: 8px;
  padding: 8px 12px;
  font-size: 14px;
  display: flex;
  align-items: center;
  gap: 8px;
}

/* Scrollbar */
.customer-list::-webkit-scrollbar,
.messages-container::-webkit-scrollbar {
  width: 4px;
}
.customer-list::-webkit-scrollbar-track,
.messages-container::-webkit-scrollbar-track {
  background: transparent;
}
.customer-list::-webkit-scrollbar-thumb,
.messages-container::-webkit-scrollbar-thumb {
  background: #e5e7eb;
  border-radius: 2px;
}
.customer-list::-webkit-scrollbar-thumb:hover,
.messages-container::-webkit-scrollbar-thumb:hover {
  background: #d1d5db;
}
</style>