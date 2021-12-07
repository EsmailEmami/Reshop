const navExpand = [].slice.call(document.querySelectorAll('.nav-expand'));

const navSlider = document.querySelector('#nav-slider');

const backLink = `
<li class="nav-item">
	<a class="nav-link nav-back-link" href="javascript:;">
		بازگشت
	</a>
</li>`;

navExpand.forEach(item => {

    var nextPageItems = item.querySelector('.nav-expand-content');

    // append back link to top of the items
    nextPageItems.insertAdjacentHTML('afterbegin', backLink);

    // nav next on click 
    item.querySelector('.nav-next').addEventListener('click', () => {
        item.classList.add('active');
        nextPageItems.style.height = (navSlider.clientHeight + 75) + 'px';
    });

    // nav back on click
    item.querySelector('.nav-back-link').addEventListener('click', () => {
        item.classList.remove('active');
    });
}); // ---------------------------------------
// not-so-important stuff starts here

const ham = document.getElementById('ham');
const navOverly = document.querySelector('.mobile-nav-overly');
const closeNav = document.querySelector('.close-nav-menu');

ham.addEventListener('click', function () {
    var nav = document.querySelector('.nav-drill');

    navOverly.style.display = "block";

    if (document.body.classList.contains('nav-is-toggled')) {
        navOverly.removeAttribute("style");
        document.body.classList.remove('nav-is-toggled');
        navExpand.forEach(item => {
            item.classList.remove('active');
        });
    } else {

        document.body.classList.add('nav-is-toggled');
    }
});



// close nav
closeNav.onclick = function () {
    navOverly.removeAttribute("style");
    document.body.classList.remove('nav-is-toggled');
    navExpand.forEach(item => {
        item.classList.remove('active');
    });
}

// close nav on click overly
document.body.onclick = function (event) {
    if (event.target == navOverly) {
        navOverly.removeAttribute("style");

        document.body.classList.remove('nav-is-toggled');

        navExpand.forEach(item => {
            item.classList.remove('active');
        });
    }
}

function imageZoom(imgID, resultID) {
    var img, lens, result, cx, cy;
    img = document.getElementById(imgID);
    result = document.getElementById(resultID);
    /*create lens:*/

    lens = document.querySelector('.img-zoom-lens');


    if (lens == null) {
        lens = document.createElement("DIV");
        lens.setAttribute("class", "img-zoom-lens");
        /*insert lens:*/
        img.parentElement.insertBefore(lens, img);
    }


    lens.style.display = "block";



    pos = getCursorPos();
    /*calculate the position of the lens:*/
    var x = pos.x - (lens.offsetWidth / 2);
    var y = pos.y - (lens.offsetHeight / 2);
    /*prevent the lens from being positioned outside the image:*/
    if (x > img.width - lens.offsetWidth) {
        x = img.width - lens.offsetWidth;
    }
    if (x < 0) {
        x = 0;
    }
    if (y > img.height - lens.offsetHeight) {
        y = img.height - lens.offsetHeight;
    }
    if (y < 0) {
        y = 0;
    }
    /*set the position of the lens:*/
    lens.style.left = x + "px";
    lens.style.top = y + "px";



    /*calculate the ratio between result DIV and lens:*/
    cx = result.offsetWidth / lens.offsetWidth;
    cy = result.offsetHeight / lens.offsetHeight;
    /*set background properties for the result DIV:*/
    result.style.backgroundImage = "url('" + img.src + "')";
    result.style.backgroundSize = (img.width * cx) + "px " + (img.height * cy) + "px";
    /*execute a function when someone moves the cursor over the image, or the lens:*/
    lens.addEventListener("mousemove", moveLens);
    img.addEventListener("mousemove", moveLens);


    lens.addEventListener("mouseout", mouseOut);


    /*and also for touch screens:*/
    lens.addEventListener("touchmove", moveLens);
    img.addEventListener("touchmove", moveLens);

    function moveLens(e) {

        if (lens.style.display != "block") {
            return;
        }



        result.style.visibility = "visible";
        result.style.opacity = 1;

        var pos, x, y;
        /*prevent any other actions that may occur when moving over the image:*/
        e.preventDefault();
        /*get the cursor's x and y positions:*/
        pos = getCursorPos(e);
        /*calculate the position of the lens:*/
        x = pos.x - (lens.offsetWidth / 2);
        y = pos.y - (lens.offsetHeight / 2);
        /*prevent the lens from being positioned outside the image:*/
        if (x > img.width - lens.offsetWidth) {
            x = img.width - lens.offsetWidth;
        }
        if (x < 0) {
            x = 0;
        }
        if (y > img.height - lens.offsetHeight) {
            y = img.height - lens.offsetHeight;
        }
        if (y < 0) {
            y = 0;
        }
        /*set the position of the lens:*/
        lens.style.left = x + "px";
        lens.style.top = y + "px";
        /*display what the lens "sees":*/
        result.style.backgroundPosition = "-" + (x * cx) + "px -" + (y * cy) + "px";
    }

    function getCursorPos(e) {
        var a, x = 0,
            y = 0;
        e = e || window.event;
        /*get the x and y positions of the image:*/
        a = img.getBoundingClientRect();
        /*calculate the cursor's x and y coordinates, relative to the image:*/
        x = e.pageX - a.left;
        y = e.pageY - a.top;
        /*consider any page scrolling:*/
        x = x - window.pageXOffset;
        y = y - window.pageYOffset;
        return {
            x: x,
            y: y
        };
    }

    function mouseOut() {
        result.style.visibility = "hidden";
        result.style.opacity = 0;
        lens.style.display = "none";
    }
}

$(window).scroll(function () {
    if ($(this).scrollTop() > 80 && window.innerWidth > 991) {
        $('.main-nav').addClass('menu-fixed');
    } else {
        $('.main-nav').removeClass('menu-fixed');
    }

    if ($(this).scrollTop() > 80 && window.innerWidth < 991) {
        $('.top-menu').addClass('menu-fixed w-100 pb-0');
        $(".search-header").addClass("d-none");
    } else {
        $('.top-menu').removeClass('menu-fixed w-100 pb-0');
        $(".search-header").removeClass("d-none");
    }
});

function CopyToClipboard(a) {
    var text = a.attributes['data-clipboard-text'].value;

    navigator.clipboard.writeText(text);
    ShowToast('success', 'لینک با موفقیت کپی شد.');
}

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

function BrandFilter(input) {

    let div = $(".main-filter .filter-by-brand .brands div");
    let persian = $(".main-filter .filter-by-brand .brands span span:nth-child(1)");
    let latin = $(".main-filter .filter-by-brand .brands span span:nth-child(2)");


    for (let i = 0; i < persian.length; i++) {

        let txtValue = (persian[i].textContent || persian[i].innerText) + " " + (latin[i].textContent || latin[i].innerText);
        if (txtValue.indexOf(input.value) > -1) {
            div[i].style.display = "";
        } else {
            div[i].style.display = "none";
        }
    }

    let btn = document.getElementById("brand-remove");

    if ($.trim(input.value).length) {
        btn.setAttribute("style", "visibility: visible;opacity: 1;");
    } else {
        btn.setAttribute("style", "");
    }
}

function ResetBrandFilterValue(btn) {
    let input = document.getElementById("brand-filter");
    input.value = "";

    btn.setAttribute("style", "");

    let div = $(".main-filter .filter-by-brand .brands div");


    for (let i = 0; i < div.length; i++) {
        div[i].style.display = "";
    }
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

function SubmitForm(formId) {
    $('#' + formId).submit();
}


var headerFilter = document.getElementById('header-filter');
var headerFilterForm = document.getElementById('header-filter-form');

headerFilter.onkeydown = function (e) {
    if (e.keyCode == 13) {
        headerFilterForm.submit();
    }
}

headerFilter.oninput = function () {
    $.ajax({
        type: 'GET',
        url: '/api/product/SearchFilterMenu?filter=' + headerFilter.value,
        success: function (res) {
            AutoComplete(headerFilter, res, headerFilterForm);
        }
    });
}

function AutoComplete(input, itemsList, form) {

    form.classList.add('active');



    if (itemsList == null) {
        closeAllLists();
        return false;
    }


    if (itemsList.length <= 0) {
        closeAllLists();
        return false;
    }


    var currentFocus, autoCompleteList;
    var inputValue = input.value;

    closeAllLists();

    if (!inputValue) {
        return false;
    }


    currentFocus = -1;
    /*create a DIV element that will contain the items*/
    autoCompleteList = document.createElement("DIV");
    autoCompleteList.setAttribute("id", input.id + "autocomplete-list");
    autoCompleteList.setAttribute("class", "autocomplete-items");


    /*append the DIV element as a child of the autocomplete container:*/
    input.parentNode.appendChild(autoCompleteList);

    /*for each item in the array...*/
    for (var i = 0; i < itemsList.length; i++) {

        // create tag a
        var itemValue = document.createElement("A");
        itemValue.setAttribute('href', itemsList[i].url);

        // searched value
        var span = document.createElement("span");
        span.innerHTML = inputValue + " در دسته ";



        // create tag strong
        var itemValueStrong = document.createElement("strong");
        itemValueStrong.innerHTML = itemsList[i].title;

        itemValue.appendChild(span);
        itemValue.appendChild(itemValueStrong);


        itemValue.addEventListener("click", function (e) {

            window.location.href = itemValue.href;

            closeAllLists();
        });

        autoCompleteList.appendChild(itemValue);

    }

    /*execute a function presses a key on the keyboard:*/
    input.addEventListener("keydown", function (e) {
        var x = document.getElementById(this.id + "autocomplete-list");
        if (x) x = x.getElementsByTagName("a");
        if (e.keyCode == 40) {
            /*If the arrow DOWN key is pressed,
            increase the currentFocus variable:*/
            currentFocus++;
            /*and and make the current item more visible:*/
            addActive(x);
        } else if (e.keyCode == 38) { //up
            /*If the arrow UP key is pressed,
            decrease the currentFocus variable:*/
            currentFocus--;
            /*and and make the current item more visible:*/
            addActive(x);
        } else if (e.keyCode == 13) {
            /*If the ENTER key is pressed, prevent the form from being submitted,*/
            e.preventDefault();
            if (currentFocus > -1) {
                /*and simulate a click on the "active" item:*/
                if (x) x[currentFocus].click();
            }
        }
    });

    function addActive(x) {
        /*a function to classify an item as "active":*/
        if (!x) return false;
        /*start by removing the "active" class on all items:*/
        removeActive(x);
        if (currentFocus >= x.length) currentFocus = 0;
        if (currentFocus < 0) currentFocus = (x.length - 1);
        /*add class "autocomplete-active":*/
        x[currentFocus].classList.add("autocomplete-active");
    }

    function removeActive(x) {
        /*a function to remove the "active" class from all autocomplete items:*/
        for (var i = 0; i < x.length; i++) {
            x[i].classList.remove("autocomplete-active");
        }
    }

    function closeAllLists(elements) {
        if (itemsList == null || itemsList.length <= 0) {
            form.classList.remove('active');
        }
        var autoCompleteList = document.getElementsByClassName("autocomplete-items");
        for (var i = 0; i < autoCompleteList.length; i++) {
            if (elements != autoCompleteList[i] && elements != input) {
                autoCompleteList[i].parentNode.removeChild(autoCompleteList[i]);
            }
        }
    }
    /*execute a function when someone clicks in the document:*/
    document.addEventListener("click", function (e) {
        closeAllLists(e.target);
    });
}

const sliderDropdown = new Swiper('.slider-dropdown-menu', {
    effect: 'creative',
    creativeEffect: {
        prev: {
            shadow: true,
            translate: [0, 0, -400],
        },
        next: {
            translate: ['100%', 0, 0]
        }
    },
    spaceBetween: 30,
    centeredSlides: true,
    loop: true,
    autoplay: {
        delay: 2500,
        disableOnInteraction: false,
    },
    pagination: {
        el: '.swiper-pagination',
        clickable: true,
        dynamicBullets: true,
    },
    navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
    },
});

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

function changeDetailImg(imgSrc) { document.getElementById("zoom").src = imgSrc; }

/*collapsible*/
//var coll = document.getElementsByClassName("collapsible");

//for (var i = 0; i < coll.length; i++) {
//    coll[i].addEventListener("click", function () {
//        this.classList.toggle("active");
//        var content = this.nextElementSibling;
//        if (content.style.maxHeight) {
//            content.style.maxHeight = null;
//        } else {

//            if (content.scrollHeight >= 700) {
//                content.style.maxHeight = 700 + "px";
//                content.style.overflowY = "scroll";
//            } else {
//                content.style.maxHeight = content.scrollHeight + "px";
//            }
//        }
//    });
//}