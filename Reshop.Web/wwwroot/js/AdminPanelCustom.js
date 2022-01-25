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


function addImageToProduct(input, partitionId) {
    if (input.files && input.files[0]) {
        var partitionImg = document.getElementById(partitionId).querySelector('img');

        var reader = new FileReader();

        reader.onload = function (e) {
            partitionImg.setAttribute('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }

    var partitionNum = parseInt(partitionId.replace(/\D/g, ""));

    // get all partitions
    var partitions = document.querySelectorAll('#images-part .col-auto');
    var lastPartitionId = partitions[partitions.length - 1].getAttribute('id');
    var lastPartitionNum = parseInt(lastPartitionId.replace(/\D/g, ""));

    var isLast = false;

    if (partitionNum === lastPartitionNum) {
        isLast = true;
    }


    if (isLast === true) {

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

        imageInput.onchange = function () {
            editImageOfProduct(this, 'imgProduct' + (lastPartitionNum + 1));
        }

        var options = document.createElement('div');
        options.className = 'image-field-options';

        var optionA = document.createElement('a');

        optionA.onclick = function () {
            removeImageOnAdd('imgProduct' + (lastPartitionNum + 1));
        }

        var optionI = document.createElement('i');
        optionI.setAttribute('class', 'far fa-trash');

        var newImagePartition = document.createElement('div');
        newImagePartition.className = 'col-auto';

        newImagePartition.setAttribute('id', 'imgProduct' + (lastPartitionNum + 1));

        changeImage.appendChild(imageInput);
        changeImage.appendChild(imageI);
        imageArea.appendChild(changeImage);
        imageArea.appendChild(areaImage);
        imageField.appendChild(imageArea);
        optionA.appendChild(optionI);
        options.appendChild(optionA);
        imageField.appendChild(options);
        newImagePartition.appendChild(imageField);

        partitions[partitions.length - 1].after(newImagePartition);
    }
}

function editImageOfProduct(input, partitionId) {

    if (input.files && input.files[0]) {
        var partitionImg = document.getElementById(partitionId).querySelector('img');

        var reader = new FileReader();

        reader.onload = function (e) {
            partitionImg.setAttribute('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }

    var partitionNum = parseInt(partitionId.replace(/\D/g, ""));

    // get all partitions
    var partitions = document.querySelectorAll('#images-part .col-auto');
    var lastPartitionId = partitions[partitions.length - 1].getAttribute('id');
    var lastPartitionNum = parseInt(lastPartitionId.replace(/\D/g, ""));

    var isLast = false;

    if (partitionNum === lastPartitionNum) {
        isLast = true;
    }

    if (isLast === false) {

        // this if for say witch image have been changed

        var changedInput = document.createElement('input');
        changedInput.setAttribute('type', 'hidden');
        changedInput.setAttribute('multiple', '');
        changedInput.setAttribute('name', 'ChangedImages');
        changedInput.setAttribute('value', partitionNum);

        document.getElementById('edited-images').appendChild(changedInput);
    }

    if (isLast === true) {

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

        imageInput.onchange = function () {
            editImageOfProduct(this, 'imgProduct' + (lastPartitionNum + 1));
        }

        var options = document.createElement('div');
        options.className = 'image-field-options';

        var optionA = document.createElement('a');

        optionA.onclick = function () {
            removeImageOnEdit('imgProduct' + (lastPartitionNum + 1));
        }

        var optionI = document.createElement('i');
        optionI.setAttribute('class', 'far fa-trash');

        var newImagePartition = document.createElement('div');
        newImagePartition.className = 'col-auto';

        newImagePartition.setAttribute('id', 'imgProduct' + (lastPartitionNum + 1));

        changeImage.appendChild(imageInput);
        changeImage.appendChild(imageI);
        imageArea.appendChild(changeImage);
        imageArea.appendChild(areaImage);
        imageField.appendChild(imageArea);
        optionA.appendChild(optionI);
        options.appendChild(optionA);
        imageField.appendChild(options);
        newImagePartition.appendChild(imageField);

        partitions[partitions.length - 1].after(newImagePartition);
    }
}

function addImageToCategory(input, partitionId) {
    if (input.files && input.files[0]) {
        var partitionImg = document.getElementById(partitionId).querySelector('img');

        var reader = new FileReader();

        reader.onload = function (e) {
            partitionImg.setAttribute('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }

    var partitionNum = parseInt(partitionId.replace(/\D/g, ""));

    // get all partitions
    var partitions = document.querySelectorAll('#images-part .col-auto');
    var lastPartitionId = partitions[partitions.length - 1].getAttribute('id');
    var lastPartitionNum = parseInt(lastPartitionId.replace(/\D/g, ""));

    var isLast = false;

    if (partitionNum === lastPartitionNum) {
        isLast = true;
    }


    if (isLast === true) {

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

        imageInput.onchange = function () {
            addImageToCategory(this, 'imgProduct' + (lastPartitionNum + 1));
        }

        var options = document.createElement('div');
        options.className = 'image-field-options';

        var optionA = document.createElement('a');

        optionA.onclick = function () {
            removeImageOnAdd('imgProduct' + (lastPartitionNum + 1));
        }

        var optionI = document.createElement('i');
        optionI.setAttribute('class', 'far fa-trash');

        var newImagePartition = document.createElement('div');
        newImagePartition.className = 'col-auto';

        newImagePartition.setAttribute('id', 'imgProduct' + (lastPartitionNum + 1));

        var formField = document.createElement('div');
        formField.className = 'form-field';

        var formFieldInput = document.createElement('input');
        formFieldInput.setAttribute('type', 'text');
        formFieldInput.setAttribute('multiple', '');
        formFieldInput.setAttribute('name', 'Urls');

        var formFieldLabel = document.createElement('label');
        formFieldLabel.innerHTML = "آدرس عکس";


        changeImage.appendChild(imageInput);
        changeImage.appendChild(imageI);
        imageArea.appendChild(changeImage);
        imageArea.appendChild(areaImage);
        imageField.appendChild(imageArea);
        optionA.appendChild(optionI);
        options.appendChild(optionA);
        formField.appendChild(formFieldInput);
        formField.appendChild(formFieldLabel);
        imageField.appendChild(options);
        imageField.appendChild(formField);
        newImagePartition.appendChild(imageField);

        partitions[partitions.length - 1].after(newImagePartition);
    }
}

function editImageOfCategory(input, partitionId) {

    if (input.files && input.files[0]) {
        var partitionImg = document.getElementById(partitionId).querySelector('img');

        var reader = new FileReader();

        reader.onload = function (e) {
            partitionImg.setAttribute('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }

    var partitionNum = parseInt(partitionId.replace(/\D/g, ""));

    // get all partitions
    var partitions = document.querySelectorAll('#images-part .col-auto');
    var lastPartitionId = partitions[partitions.length - 1].getAttribute('id');
    var lastPartitionNum = parseInt(lastPartitionId.replace(/\D/g, ""));

    var isLast = false;

    if (partitionNum === lastPartitionNum) {
        isLast = true;
    }

    if (isLast === false) {

        // this if for say witch image have been changed

        var changedInput = document.createElement('input');
        changedInput.setAttribute('type', 'hidden');
        changedInput.setAttribute('multiple', '');
        changedInput.setAttribute('name', 'ChangedImages');
        changedInput.setAttribute('value', partitionNum);

        document.getElementById('edited-images').appendChild(changedInput);
    }

    if (isLast === true) {

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

        imageInput.onchange = function () {
            editImageOfCategory(this, 'imgProduct' + (lastPartitionNum + 1));
        }

        var options = document.createElement('div');
        options.className = 'image-field-options';

        var optionA = document.createElement('a');

        optionA.onclick = function () {
            removeImageOnEdit('imgProduct' + (lastPartitionNum + 1));
        }

        var optionI = document.createElement('i');
        optionI.setAttribute('class', 'far fa-trash');

        var newImagePartition = document.createElement('div');
        newImagePartition.className = 'col-auto';

        newImagePartition.setAttribute('id', 'imgProduct' + (lastPartitionNum + 1));

        var formField = document.createElement('div');
        formField.className = 'form-field';

        var formFieldInput = document.createElement('input');
        formFieldInput.setAttribute('type', 'text');
        formFieldInput.setAttribute('multiple', '');
        formFieldInput.setAttribute('name', 'Urls');

        formFieldInput.addEventListener('change', function () {
            setChangeUrl('imgProduct' + (lastPartitionNum + 1));
        });

        var formFieldLabel = document.createElement('label');
        formFieldLabel.innerHTML = "آدرس عکس";


        changeImage.appendChild(imageInput);
        changeImage.appendChild(imageI);
        imageArea.appendChild(changeImage);
        imageArea.appendChild(areaImage);
        imageField.appendChild(imageArea);
        optionA.appendChild(optionI);
        options.appendChild(optionA);
        formField.appendChild(formFieldInput);
        formField.appendChild(formFieldLabel);
        imageField.appendChild(options);
        imageField.appendChild(formField);
        newImagePartition.appendChild(imageField);

        partitions[partitions.length - 1].after(newImagePartition);
    }
}



function removeImageOnAdd(partitionId) {
    var partitionsParent = document.querySelector('#images-part');
    var partitions = partitionsParent.querySelectorAll('.col-auto');
    var lastPartitionId = partitions[partitions.length - 1].getAttribute('id');
    var lastPartitionNum = parseInt(lastPartitionId.replace(/\D/g, ""));

    var partitionNum = parseInt(partitionId.replace(/\D/g, ""));

    var isLast = false;

    if (partitionNum === lastPartitionNum) {
        isLast = true;
    }

    if (isLast === false) {

        var selectedPartition = document.getElementById(partitionId);

        partitionsParent.removeChild(selectedPartition);

    }
}

function removeImageOnEdit(partitionId) {
    var partitionsParent = document.querySelector('#images-part');
    var partitions = partitionsParent.querySelectorAll('.col-auto');
    var lastPartitionId = partitions[partitions.length - 1].getAttribute('id');
    var lastPartitionNum = parseInt(lastPartitionId.replace(/\D/g, ""));

    var partitionNum = parseInt(partitionId.replace(/\D/g, ""));

    var isLast = false;

    if (partitionNum === lastPartitionNum) {
        isLast = true;
    }



    if (isLast === false) {

        var selectedPartition = document.getElementById(partitionId);

        partitionsParent.removeChild(selectedPartition);

        // create new input for deleted Items

        var beforeAddedImagesLength = document.querySelectorAll('#selected-images input').length;

        if (partitionNum <= beforeAddedImagesLength) {
            var changedInput = document.createElement('input');
            changedInput.setAttribute('type', 'hidden');
            changedInput.setAttribute('multiple', '');
            changedInput.setAttribute('name', 'DeletedImages');
            changedInput.setAttribute('value', partitionNum);

            document.getElementById('deleted-images').appendChild(changedInput);
        }
    }
}

function setChangeUrl(partitionId) {
    var partitionsParent = document.querySelector('#images-part');
    var partitions = partitionsParent.querySelectorAll('.col-auto');
    var lastPartitionId = partitions[partitions.length - 1].getAttribute('id');
    var lastPartitionNum = parseInt(lastPartitionId.replace(/\D/g, ""));

    var partitionNum = parseInt(partitionId.replace(/\D/g, ""));

    var isLast = false;

    if (partitionNum === lastPartitionNum) {
        isLast = true;
    }



    if (isLast === false) {

        // create new input for deleted Items

        var beforeAddedImagesLength = document.querySelectorAll('#selected-images input').length;

        if (partitionNum <= beforeAddedImagesLength) {
            var changedInput = document.createElement('input');
            changedInput.setAttribute('type', 'hidden');
            changedInput.setAttribute('multiple', '');
            changedInput.setAttribute('name', 'ChangedUrls');
            changedInput.setAttribute('value', partitionNum);

            document.getElementById('edited-urls').appendChild(changedInput);
        }
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