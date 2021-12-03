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

function changeDetailImg(imgSrc) { document.getElementById("zoom").src = imgSrc; }

$(function () {
    let modal = document.getElementById('modal');
    $(".close").click(function () {
        modal.style.display = 'none';
    });
    window.onclick = function (event) {
        if (event.target == modal) {
            ShowToast('warning', 'برای خروج بر روی ضربدر کلیک کنید.');
        }
    }
});

function ShowToast(type, text, returnUrl) {

    let toast;

    switch (type) {
    case "success":
        toast = document.getElementById("toast-success");
        $("#toast-success").find("span").text(text);
        break;
    case "warning":
        toast = document.getElementById("toast-warning");
        $("#toast-warning").find("span").text(text);
        break;

    case "danger":
        toast = document.getElementById("toast-wrong");
        $("#toast-wrong").find("span").text(text);
        break;

    default:
        toast = document.getElementById("toast-success");
        break;
    }
    toast.className = "show";
    setTimeout(function () {
        toast.className = toast.className.replace("show", "");
        if (returnUrl != null) {
            window.location.href = returnUrl;
        }
    }, 3000);
}

function commaSep(number) {
    number += '';
    number = number.replace(',', ''); number = number.replace(',', ''); number = number.replace(',', '');
    number = number.replace(',', ''); number = number.replace(',', ''); number = number.replace(',', '');
    x = number.split('.');
    y = x[0];
    z = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(y))
        y = y.replace(rgx, '$1' + ',' + '$2');
    return y + z;
}