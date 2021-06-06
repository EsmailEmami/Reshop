

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