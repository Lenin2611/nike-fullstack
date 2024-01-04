// ELEMENTS

const token = localStorage.getItem('token');
const user = localStorage.getItem('user');
const title = document.getElementById('title');
const container = document.getElementById('container');
const spam = document.getElementById('spam');

const url = 'http://localhost:5070/api/Auth/username';
const urlAll = 'http://localhost:5070/api/Product/';
const urlType = 'http://localhost:5070/api/Product/type';

// GENERAL FUNCTIONS

document.body.addEventListener('click', async function (e) {
    if (e.target.id === 'logout') {
        localStorage.setItem('token', null);
        window.location.href = '../index.html';
    }

    if (e.target.id === 'all') {
        inner('all');
    } else if (e.target.id === 'shirts') {
        inner('shirts');
    } else if (e.target.id === 'pants') {
        inner('pants');
    } else if (e.target.id === 'jackets') {
        inner('jackets');
    } else if (e.target.id === 'car') {
        inner('car');
    }

    if (e.target.className === 'add-product') {
        let id = e.target.id;
        const urlInCar = 'http://localhost:5070/api/Product/incar' + id + '?username=' + user;
        await inOutCar(urlInCar);
        spam.innerHTML = 'Product Added';
        spam.style.display = 'block';
        if (title.textContent.toLowerCase() === 'car') {
            inner('car');
        }
        setTimeout(() => {
            spam.style.display = 'none';
        }, 1000);
    }

    if (e.target.className === 'delete-product') {
        let id = e.target.id;
        const urlOutCar = 'http://localhost:5070/api/Product/outcar' + id + '?username=' + user;
        spam.innerHTML = 'Product Deleted';
        spam.style.display = 'block';
        setTimeout(() => {
            spam.style.display = 'none';
        }, 1000);
        await inOutCar(urlOutCar);
        await inner('car');
    }
})

// AUTHORIZATION

async function getDataWithBearerToken(url, token) {
    const response = await fetch(url, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
            'accept': 'text/plain'
        }
    });
    if (response.ok) {
        return true;
    } else {
        return false;
    }
}

async function checkToken() {
    let valid = await getDataWithBearerToken(url, token);
    let jsonPayload;
    try {
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        jsonPayload = decodeURIComponent(atob(base64).split('').map((c) => {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));
    } catch (error) {
        if (valid === false) {
            window.location.href = '../html/unauthorized.html';
        }
    }
}
checkToken();

// MENU

async function getClothes(url, token) {
    const options = {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        }
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

async function inOutCar(url) {
    const options = {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        }
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

async function inner(type) {
    try {
        title.innerHTML = type.toUpperCase();
        let result;
        if (type === 'car') {
            result = await getClothes(urlAll + type + user, token);
            console.log(urlAll + type + user);
            container.innerHTML = '';
            result.forEach((p) => {
                const div = document.createElement('div');
                div.classList.add('clothes');
                div.innerHTML = /*html*/`
                        <img class="image" src="${p.image}" alt="${p.name}">
                        <div class="details">
                            <h3 class="name">${p.name}</h3>
                            <p class="price"><strong>Size:</strong> ${p.size} <br><strong>Price:</strong> $${p.price}</p>
                            <div class="actions">
                                <button class="delete-product" id="${p.id}">-</button>
                                <p class="quantity" id="quantity">${p.quantityInCar}</p>
                                <button class="add-product" id="${p.id}">+</button>
                            </div>
                        </div>
                    `;
                container.append(div);
            })
        } else {
            if (type === 'all') {
                result = await getClothes(urlAll, token);
            } else {
                result = await getClothes(urlType + type, token);
            }
            container.innerHTML = '';
            result.forEach((p) => {
                const div = document.createElement('div');
                div.classList.add('clothes');
                div.innerHTML = /*html*/`
                        <img class="image" src="${p.image}" alt="${p.name}">
                        <div class="details">
                            <h3 class="name">${p.name}</h3>
                            <p class="price"><strong>Size:</strong> ${p.size} <br><strong>Price:</strong> $${p.price}</p>
                            <div class="actions">
                                <button class="add-product" id="${p.id}">+</button>
                            </div>
                        </div>
                    `;
                container.append(div);
            })
        }
    } catch (error) {
        console.error(error);
        container.innerHTML = 'There are no products in this section.';
    }
}
inner('all');