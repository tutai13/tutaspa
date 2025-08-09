<template>
  <div class="login-container">
    <div class="login-header">
      <h1>Đăng nhập</h1>
      <p>Chào mừng bạn quay trở lại!</p>
    </div>

    <form @submit.prevent="handleLogin">
      <div class="form-group">
        <label for="phone">Số điện thoại</label>
        <div class="input-wrapper">
          <i class="fas fa-phone input-icon"></i>
          <input 
            type="tel" 
            id="phone"
            class="form-control"
            v-model="form.phone"
            placeholder="Nhập số điện thoại"
            :class="{ 'error': errors.phone }"
          >
        </div>
        <transition name="fade">
          <div v-if="errors.phone" class="error-message">
            <i class="fas fa-exclamation-circle"></i>
            {{ errors.phone }}
          </div>
        </transition>
      </div>

      <div class="form-group">
        <label for="password">Mật khẩu</label>
        <div class="input-wrapper">
          <i class="fas fa-lock input-icon"></i>
          <input 
            :type="showPassword ? 'text' : 'password'"
            id="password"
            class="form-control"
            v-model="form.password"
            placeholder="Nhập mật khẩu"
            :class="{ 'error': errors.password }"
          >
          <i 
            :class="showPassword ? 'fas fa-eye-slash' : 'fas fa-eye'"
            class="password-toggle"
            @click="togglePassword"
          ></i>
        </div>
        <transition name="fade">
          <div v-if="errors.password" class="error-message">
            <i class="fas fa-exclamation-circle"></i>
            {{ errors.password }}
          </div>
        </transition>
      </div>

      <button 
        type="submit" 
        class="login-btn"
        :disabled="isLoading"
      >
        <span v-if="isLoading">
          <i class="fas fa-spinner fa-spin"></i> Đang đăng nhập...
        </span>
        <span v-else>
          <i class="fas fa-sign-in-alt"></i> Đăng nhập
        </span>
      </button>

      <div class="forgot-password">
        <a href="#" @click.prevent="forgotPassword">Quên mật khẩu?</a>
      </div>

      <div class="register-link">
        <p>Chưa có tài khoản?</p>
        <a href="#" @click.prevent="goToRegister">Đăng ký ngay</a>
      </div>
    </form>
  </div>
</template>

<script setup>
import notification from '../notification'
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { authAPI } from '../services/authservice' 

// Router instance
const router = useRouter()

// Reactive data
const form = reactive({
  phone: '',
  password: ''
})

const errors = reactive({
  phone: '',
  password: ''
})

// Refs
const showPassword = ref(false)
const isLoading = ref(false)

// Methods
const validateForm = () => {
  let isValid = true
  
  // Reset errors
  errors.phone = ''
  errors.password = ''

  // Validate phone
  if (!form.phone.trim()) {
    errors.phone = 'Vui lòng nhập số điện thoại'
    isValid = false
  } else if (!/^[0-9]{10,11}$/.test(form.phone.replace(/\s/g, ''))) {
    errors.phone = 'Số điện thoại không hợp lệ'
    isValid = false
  }

  // Validate password
  if (!form.password.trim()) {
    errors.password = 'Vui lòng nhập mật khẩu'
    isValid = false
  } else if (form.password.length < 6) {
    errors.password = 'Mật khẩu phải có ít nhất 6 ký tự'
    isValid = false
  }

  return isValid
}

const handleLogin = async () => {
  if (!validateForm()) return

  isLoading.value = true

  try {
    const result = await authAPI.login({
      PhoneNumber: form.phone,
      Password: form.password
    })

    if(result){
        notification.success("Đăng nhập thành công", {
            title : "",
            timer : 1500
        })

        setTimeout(() => {
            router.push('/')
        }, 1500)
        
        
        return ;    
    }


    notification.error("Đăng nhập thất bại")

    
  } catch (error) {
    console.error('Login error:', error)
    notification.error( error.message ? error.message : 'Đăng nhập thất bại. Vui lòng thử lại!' )
  } finally {
    isLoading.value = false
  }
}

const togglePassword = () => {
  showPassword.value = !showPassword.value
}

const forgotPassword = () => {
  // Chuyển hướng đến trang quên mật khẩu
  router.push('/forgot-password')
}

const goToRegister = () => {
  // Chuyển hướng đến trang đăng ký
  router.push('/register')
}

// Emit events (nếu cần thiết cho parent component)
const emit = defineEmits(['login-success', 'forgot-password', 'go-to-register'])

// Có thể emit events thay vì navigate
const handleLoginWithEmit = async () => {
  if (!validateForm()) return

  isLoading.value = true

  try {
    const response = await authAPI.login({
      phone: form.phone,
      password: form.password
    })

    // Emit event cho parent component
    emit('login-success', {
      user: response.user,
      token: response.access_token
    })
    
  } catch (error) {
    console.error('Login error:', error)
    alert('Đăng nhập thất bại. Vui lòng thử lại!')
  } finally {
    isLoading.value = false
  }
}

const handleForgotPassword = () => {
  emit('forgot-password')
}

const handleGoToRegister = () => {
  emit('go-to-register')
}
</script>



<style scoped>
.login-container {
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(10px);
  border-radius: 20px;
  box-shadow: 0 15px 35px rgba(0, 0, 0, 0.1);
  padding: 40px;
  width: 100%;
  max-width: 400px;
  margin: 0 auto;
  border: 1px solid rgba(255, 255, 255, 0.2);
}

.login-header {
  text-align: center;
  margin-bottom: 30px;
}

.login-header h1 {
  color: #333;
  font-size: 28px;
  font-weight: 600;
  margin-bottom: 8px;
}

.login-header p {
  color: #666;
  font-size: 16px;
}

.form-group {
  position: relative;
  margin-bottom: 25px;
}

.form-group label {
  display: block;
  color: #555;
  font-weight: 500;
  margin-bottom: 8px;
  font-size: 14px;
}

.input-wrapper {
  position: relative;
}

.form-control {
  width: 100%;
  padding: 15px 20px 15px 50px;
  border: 2px solid #e1e5e9;
  border-radius: 12px;
  font-size: 16px;
  transition: all 0.3s ease;
  background: #fff;
}

.form-control:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.form-control.error {
  border-color: #e74c3c;
}

.input-icon {
  position: absolute;
  left: 18px;
  top: 50%;
  transform: translateY(-50%);
  color: #999;
  font-size: 16px;
}

.password-toggle {
  position: absolute;
  right: 18px;
  top: 50%;
  transform: translateY(-50%);
  color: #999;
  cursor: pointer;
  font-size: 16px;
  transition: color 0.3s ease;
}

.password-toggle:hover {
  color: #667eea;
}

.login-btn {
  width: 100%;
  padding: 16px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  border-radius: 12px;
  font-size: 16px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
  margin-bottom: 20px;
}

.login-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 25px rgba(102, 126, 234, 0.3);
}

.login-btn:active {
  transform: translateY(0);
}

.login-btn:disabled {
  opacity: 0.7;
  cursor: not-allowed;
  transform: none;
}

.forgot-password {
  text-align: center;
  margin-bottom: 20px;
}

.forgot-password a {
  color: #667eea;
  text-decoration: none;
  font-size: 14px;
  font-weight: 500;
  transition: color 0.3s ease;
}

.forgot-password a:hover {
  color: #764ba2;
  text-decoration: underline;
}

.register-link {
  text-align: center;
  padding-top: 20px;
  border-top: 1px solid #e1e5e9;
}

.register-link p {
  color: #666;
  font-size: 14px;
  margin-bottom: 8px;
}

.register-link a {
  color: #667eea;
  text-decoration: none;
  font-weight: 600;
  font-size: 16px;
  transition: color 0.3s ease;
}

.register-link a:hover {
  color: #764ba2;
  text-decoration: underline;
}

.error-message {
  color: #e74c3c;
  font-size: 14px;
  margin-top: 5px;
  display: flex;
  align-items: center;
  gap: 5px;
}

@media (max-width: 480px) {
  .login-container {
    padding: 30px 20px;
    margin: 10px;
  }
  
  .login-header h1 {
    font-size: 24px;
  }
}

.fade-enter-active, .fade-leave-active {
  transition: opacity 0.3s ease;
}

.fade-enter-from, .fade-leave-to {
  opacity: 0;
}
</style>