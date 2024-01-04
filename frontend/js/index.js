// ELEMENTS

const register = document.getElementById('register');
const registerBtn = document.getElementById('submit-register');
const userInputRegister = document.getElementById('user-register');
const emailInputRegister = document.getElementById('email-register');
const passwordInputRegister = document.getElementById('password-register');
const warningRegister = document.getElementById('warning-register');
const popupRegister = document.querySelector('#popup-register');
const textPopupRegister = document.getElementById('user-registered');

const login = document.getElementById('login');
const loginBtn = document.getElementById('submit-login');
const userInputLogin = document.getElementById('user-login');
const passwordInputLogin = document.getElementById('password-login');
const warningLogin = document.getElementById('warning-login');

// GENERAL FUNCTIONS

document.body.addEventListener("click", function (e) {
    if (e.target.id === 'pass-register') {
        login.style.display = 'none';
        register.style.display = 'flex';
        warningRegister.style.display = 'none';
        userInputRegister.value = '';
        emailInputRegister.value = '';
        passwordInputRegister.value = '';
    }

    if (e.target.id === 'pass-login') {
        register.style.display = 'none';
        login.style.display = 'flex';
        userInputLogin.value = '';
        passwordInputLogin.value = '';
    }

    if (e.target.id === 'popup-register' || e.target.id === 'popup-text-register' || e.target.id === 'user') {
        popupRegister.style.display = 'none';
        userInputRegister.value = '';
        emailInputRegister.value = '';
        passwordInputRegister.value = '';
    }
})

// REGISTER

async function Register(user, email, password) {
    const url = 'http://localhost:5070/api/Auth/register';
    let data = {
        username: user,
        email: email,
        password: password
    };
    const options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    };
    try {
        const response = await fetch(url, options);
        const result = await response.text();
        return result;
    } catch (error) {
        console.error(error);
        return null;
    }
}

document.getElementById('submit-register').addEventListener('click', async function (e) {
    e.preventDefault();
    const formRegister = this.closest('form');
    if (formRegister.checkValidity()) {
        const userRegister = userInputRegister.value;
        const emailRegister = emailInputRegister.value;
        const passwordRegister = passwordInputRegister.value;
        if (userRegister === '' || emailRegister === '' || passwordRegister === '') {
            warningRegister.style.display = 'block';
            setTimeout(() => {
                warningRegister.style.display = 'none';
            }, 2000);
        } else {
            const result = await Register(userRegister, emailRegister, passwordRegister);
            if (result === 'User is already registered.') {
                warningRegister.style.display = 'block';
                setTimeout(() => {
                    warningRegister.style.display = 'none';
                }, 2000);
            } else {
                textPopupRegister.innerHTML = userRegister;
                popupRegister.style.display = 'block';
            }
        }
    } else {
        formRegister.reportValidity();
    }
})

// LOGIN

async function Login(user, password) {
    const url = 'http://localhost:5070/api/Auth/login';
    let data = {
        username: user,
        password: password
    };
    const options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    };
    try {
        const response = await fetch(url, options);
        const result = await response.text();
        return result;
    } catch (error) {
        console.error(error);
        return null;
    }
}

async function GetRols(user, password) {
    const url = 'http://localhost:5070/api/Auth/userrols';
    let data = {
        username: user,
        password: password
    }
    const options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data) 
    };
    try {
        const response = await fetch(url, options);
        const result = await response.json();
        return result;
    } catch (error) {
        console.error(error);
        return null;
    }
}

document.getElementById('submit-login').addEventListener('click', async function (e) {
    e.preventDefault();
    const formLogin = this.closest('form');
    if (formLogin.checkValidity()) {
        const userLogin = userInputLogin.value;
        const passwordLogin = passwordInputLogin.value;
        const result = await Login(userLogin, passwordLogin);
        if (userLogin === '' || passwordLogin === '' || result === 'Check your password and username.') {
            warningLogin.style.display = 'block';
            setTimeout(() => {
                warningLogin.style.display = 'none';
            }, 2000);
        } else {
            localStorage.setItem('token', result);
            localStorage.setItem('user', userLogin);
            let rols = await GetRols(userLogin, passwordLogin);
            if (rols.includes('Administrator')) {
                console.log('Admin');
            } else if (rols.includes('Person')) {
                console.log('Person');
            }
            let page = '/html/menu.html';
            window.location.href = page;
        }
    }
})