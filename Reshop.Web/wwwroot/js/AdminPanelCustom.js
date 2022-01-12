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


function addImageToProduct(input, imgId) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            document.getElementById(imgId).setAttribute('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }

    // create new add image part

    var inputs = document.querySelectorAll('#images-part input');

    let lastElement = inputs[inputs.length - 1];

    if (input.files[0] == lastElement.files[0]) {
        var imageField = document.createElement('div');
        imageField.className = 'image-field';

        var imageArea = document.createElement('div');
        imageArea.className = 'image-area';

        var changeImage = document.createElement('a');
        changeImage.className = 'change-image';
        changeImage.setAttribute('href', 'javascript:void(0)');

        var imageInput = document.createElement('input');
        imageInput.setAttribute('type', 'file');
        imageInput.setAttribute('multiple', '');
        imageInput.setAttribute('name', 'Images');
        imageInput.setAttribute('class', 'ProfileAvatarInput');

        var imageI = document.createElement('i');
        imageI.setAttribute('class', 'far fa-camera');

        var areaImage = document.createElement('img');
        areaImage.setAttribute('src', '/images/pattern/placeholder-image.png');
        areaImage.setAttribute('alt', 'تصویر کالا');

        var imagesPart = document.querySelectorAll('#images-part .col-auto');

        var imagesPartLength = parseInt(imagesPart.length) + 1;

        areaImage.setAttribute('id', 'imgProduct' + imagesPartLength);

        imageInput.onchange = function () {
            addImageToProduct(this, 'imgProduct' + imagesPartLength);
        }

        var addImageSection = document.createElement('div');
        addImageSection.className = 'col-auto';


        changeImage.appendChild(imageInput);
        changeImage.appendChild(imageI);
        imageArea.appendChild(changeImage);
        imageArea.appendChild(areaImage);
        imageField.appendChild(imageArea);
        addImageSection.appendChild(imageField);

        var lastImage = imagesPart[imagesPart.length - 1];

        lastImage.after(addImageSection);
    }
}

function editImageOfProduct(input, imgId) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            document.getElementById(imgId).setAttribute('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }

    // create new add image part

    var inputs = document.querySelectorAll('#images-part input');

    var changedNum = parseInt(imgId.replace(/\D/g, ""));


    var isLast = false;
    
    if (inputs.length == changedNum) {
        isLast = true;
    }

    debugger;

    if (isLast == false) {

        var changedNum = imgId.replace(/\D/g, "");


        // this if for say witch image have been changed

        var changedInput = document.createElement('input');
        changedInput.setAttribute('type', 'hidden');
        changedInput.setAttribute('multiple', '');
        changedInput.setAttribute('name', 'ChangedImages');
        changedInput.setAttribute('value', changedNum);

        document.getElementById('edited-images').appendChild(changedInput);
    }

    if (isLast == true) {

        var imageField = document.createElement('div');
        imageField.className = 'image-field';

        var imageArea = document.createElement('div');
        imageArea.className = 'image-area';

        var changeImage = document.createElement('a');
        changeImage.className = 'change-image';
        changeImage.setAttribute('href', 'javascript:void(0)');

        var imageInput = document.createElement('input');
        imageInput.setAttribute('type', 'file');
        imageInput.setAttribute('multiple', '');
        imageInput.setAttribute('name', 'Images');
        imageInput.setAttribute('class', 'ProfileAvatarInput');

        var imageI = document.createElement('i');
        imageI.setAttribute('class', 'far fa-camera');

        var areaImage = document.createElement('img');
        areaImage.setAttribute('src', '/images/pattern/placeholder-image.png');
        areaImage.setAttribute('alt', 'تصویر کالا');

        var imagesPart = document.querySelectorAll('#images-part .col-auto');

        var imagesPartLength = parseInt(imagesPart.length) + 1;

        areaImage.setAttribute('id', 'imgProduct' + imagesPartLength);

        imageInput.onchange = function () {
            editImageOfProduct(this, 'imgProduct' + imagesPartLength);
        }

        var addImageSection = document.createElement('div');
        addImageSection.className = 'col-auto';


        changeImage.appendChild(imageInput);
        changeImage.appendChild(imageI);
        imageArea.appendChild(changeImage);
        imageArea.appendChild(areaImage);
        imageField.appendChild(imageArea);
        addImageSection.appendChild(imageField);

        var lastImage = imagesPart[imagesPart.length - 1];

        lastImage.after(addImageSection);
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