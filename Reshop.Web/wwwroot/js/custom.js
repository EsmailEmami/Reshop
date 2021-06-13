

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




let megaDropDowns = $(".main-nav .mega-dropdown");

for (let i = 0; i < megaDropDowns.length; i++) {
    let megaDropDown = megaDropDowns[i];

    let childs = megaDropDown.childNodes;

    let ulCount = 0;

    for (let child = 0; child < childs.length; child++) {
        if (childs[child].localName == "ul") {
            ulCount++;
        }
    }

    let width = (ulCount * 150) + ulCount;



    if (ulCount > 6) {
        megaDropDown.style.width = "100%";
    }

    if (ulCount < 3) {
        megaDropDown.parentElement.style.position = "relative";
        megaDropDown.style.right = "0";
    }

    megaDropDown.style.width = width + "px";
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

$('.carousel').carousel({
    interval: 10000
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

        case "wrong":
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

    let txt, txtValue, checkbox;

    txt = $(".main-filter .filter-by-brand .brands span");
    checkbox = $(".main-filter .filter-by-brand .brands input[type=checkbox]");

    for (i = 0; i < txt.length; i++) {

        txtValue = txt[i].textContent || txt[i].innerText;
        if (txtValue.indexOf(input.value) > -1) {
            txt[i].style.display = "";
            checkbox[i].style.display = "";
        } else {
            txt[i].style.display = "none";
            checkbox[i].style.display = "none";
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

    let txt = $(".main-filter .filter-by-brand .brands span");
    let checkbox = $(".main-filter .filter-by-brand .brands input[type=checkbox]");

    for (i = 0; i < txt.length; i++) {
        txt[i].style.display = "";
        checkbox[i].style.display = "";
    }

}

$(function () {
    if (window.innerWidth > 767) {
        let collapse = document.getElementsByClassName('collapse');
        for (let i = 0; i < collapse.length; i++) {
            collapse[i].classList.add("show");
        }
    }

    window.addEventListener('resize', function () {
        let collapse = document.getElementsByClassName('collapse');

        if (window.innerWidth > 767) {

            for (let i = 0; i < collapse.length; i++) {
                collapse[i].classList.add("show");
            }
        } else {
            for (let i = 0; i < collapse.length; i++) {
                collapse[i].classList.remove("show");
            }
        }
    });
});

function SubmitForm(formId) {
    document.getElementById(formId).Submit();
}