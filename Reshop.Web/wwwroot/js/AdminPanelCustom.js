const dropdownsLarge = document.querySelectorAll('.right-menu ul li.dropdown');
const menuButton = document.getElementById('menu_button');
const menu = document.querySelector('.right-menu');
const contentPage = document.querySelector('.content-page');

dropdownsLarge.forEach(element => {
    element.onclick = function () {
        element.classList.toggle('active');
    }
});

menuButton.onclick = function () {
    if (window.innerWidth <= 767) {
        menu.classList.toggle('d-block');
    } else {
        menu.classList.toggle('min');
    }

    contentPage.classList.toggle('big');
}

if (window.innerWidth <= 1026) {
    menu.classList.add('min');
    contentPage.classList.add('big');
} else {
    menu.classList.remove('min');
    contentPage.classList.remove('big');
}


if (window.innerWidth <= 767) {
    menu.classList.remove('d-block');
    contentPage.classList.remove('big');
}



window.addEventListener('resize', function () {
    if (window.innerWidth <= 1026) {
        menu.classList.add('min');
        contentPage.classList.add('big');
    } else {
        menu.classList.remove('min');
        contentPage.classList.remove('big');
    }


    if (window.innerWidth <= 767) {
        menu.classList.remove('d-block');
        contentPage.classList.remove('big');
    }
});


function readURL(input, imgId) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            document.getElementById(imgId).setAttribute('src', e.target.result)
        }

        reader.readAsDataURL(input.files[0]);
    }
}