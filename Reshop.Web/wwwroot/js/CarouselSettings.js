$('.carousel-product').owlCarousel({
    rtl: true,
    loop: true,
    dots: false,
    nav: true,
    margin: 10,
    autoplay: true,
    autoplayTimeout: 3000,
    autoplayHoverPause: true,
    animateOut: 'slideOutDown',
    animateIn: 'flipInX',
    smartSpeed: 1000,
});


$("#mini-pic-slider").owlCarousel({
    rtl: true,
    loop: false,
    nav: false,
    dots: false,
    stagePadding: 0,
});