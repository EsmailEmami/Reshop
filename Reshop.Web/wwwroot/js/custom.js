if ($('.main-nav').length) {
    var $mobile_nav = $('.main-nav').clone().prop({
        class: 'mobile-nav d-lg-none'
    });
    $('body').append($mobile_nav);
    $('body').prepend('<button type="button" class="mobile-nav-toggle d-lg-none"><i class="fa fa-bars"></i></button>');
    $('body').append('<div class="mobile-nav-overly"></div>');

    $(document).on('click', '.mobile-nav-toggle', function (e) {
        $('body').toggleClass('mobile-nav-active');
        $('.mobile-nav-toggle i').toggleClass('fa-times fa-bars');
        $('.mobile-nav-overly').toggle();
    });

    $(document).on('click', '.mobile-nav li.drop-down > a', function (e) {
        e.preventDefault();
        $(this).parent().toggleClass('active');
    });

    $(document).click(function (e) {
        var container = $(".mobile-nav, .mobile-nav-toggle");
        if (!container.is(e.target) && container.has(e.target).length === 0) {
            if ($('body').hasClass('mobile-nav-active')) {
                $('body').removeClass('mobile-nav-active');
                $('.mobile-nav-toggle i').toggleClass('fa-times fa-bars');
                $('.mobile-nav-overly').fadeOut();
            }
        }
    });
} else if ($(".mobile-nav, .mobile-nav-toggle").length) {
    $(".mobile-nav, .mobile-nav-toggle").hide();
    $("body").prepend($mobile_nav);
    $('body').prepend('<div class="mobile-nav-overly"></div>');
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