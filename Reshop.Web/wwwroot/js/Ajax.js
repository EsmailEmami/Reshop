function AddProductToCart(productId, shopperUserId) {

    if (shopperUserId == null) {
        shopperUserId = $("#selected-shopper").val();
    }


    $.ajax({
        type: 'POST',
        url: '/Cart/AddToCart?productId=' + productId + '&shopperUserId=' + shopperUserId,
        dataType: "json",
        contentType: "application/json",
        success: function (res) {
            if (res.success) {
                ShowToast('success', 'محصول با موفقیت به سبد خرید اضافه شد.');
            } else {
                ShowToast('warning', 'محصول با موفقیت به سبد خرید اضافه نشد.');
            }
        }
    });

}


function AddProductToFavorites(productId, shopperUserId) {

    if (shopperUserId == null) {
        shopperUserId = $("#selected-shopper").val();
    }


    $.ajax({
        type: 'POST',
        url: '/Product/AddToFavoriteProduct?productId=' + productId + '&shopperUserId=' + shopperUserId,
        dataType: "json",
        contentType: "application/json",
        success: function (res) {
            if (res.success) {
                ShowToast('success', 'محصول با موفقیت به علاقه مندی ها اضافه شد');
            } else {
                ShowToast('warning', 'محصول با موفقیت به علاقه مندی ها اضافه نشد.');
            }
        }
    });

}
