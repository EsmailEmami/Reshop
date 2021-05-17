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
    responsive: {
        0: {
            items: 1
        },
        600: {
            items: 2
        },

        800: {
            items: 3
        },
        1000: {
            items: 4
        }
    }
});