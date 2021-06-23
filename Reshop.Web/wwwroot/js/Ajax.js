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
                if (res.resultType == "Successful") {
                    ShowToast('success', 'محصول با موفقیت به سبد خرید اضافه شد.');
                } else if (res.resultType == "ProductReplaced") {
                    ShowToast('success', 'فروشنده محصول با موفقیت تغییر کرد.');
                }
            } else {
                if (res.resultType == "NotFound") {
                    ShowToast('wrong', 'متاسفانه خطایی غیر منتظره رخ داده است.');
                } else if (res.resultType == "Available") {
                    ShowToast('warning', 'محصول در علاقه مندی های شما موجود است.');
                }
            }
        }
    });
}
