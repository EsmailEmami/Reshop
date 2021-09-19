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
        },
        1500: {
            items: 5
        },
        2000: {
            items: 6
        },
        2300: {
            items: 7
        },
        2600: {
            items: 8
        },
        3000: {
            items: 9
        }
    }
});


$("#mini-pic-slider").owlCarousel({
    rtl: true,
    loop: false,
    nav: false,
    dots: false,
    items: 4,
    stagePadding: 0,
    responsive: {
        0: {
            items: 4
        },
        600: {
            items: 3
        }
    },
});