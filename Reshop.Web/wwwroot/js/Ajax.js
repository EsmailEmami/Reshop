function AddProductToCart(form) {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            cache: false,
            processData: false,
            contentType: false,
            success: function (res) {
                if (res.success) {
                    ShowToast('success', 'محصول با موفقیت به سبد خرید اضافه شد.');
                } else {
                    ShowToast('warning', 'محصول با موفقیت به سبد خرید اضافه نشد.');
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex);
    }
}

function AddProductToFavorites(form) {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            cache: false,
            processData: false,
            contentType: false,
            success: function (res) {
                if (res.success) {
                    if (res.resultType == "Successful") {
                        ShowToast('success', 'محصول با موفقیت به سبد خرید اضافه شد.');
                    } else if (res.resultType == "ProductReplaced") {
                        ShowToast('success', 'فروشنده محصول با موفقیت تغییر کرد.');
                    }
                } else {
                    if (res.resultType == "NotFound") {
                        ShowToast('danger', 'متاسفانه خطایی غیر منتظره رخ داده است.');
                    } else if (res.resultType == "Available") {
                        ShowToast('warning', 'محصول در علاقه مندی های شما موجود است.');
                    }
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex);
    }
}

function ShowModal(url, title) {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            // modal body 
            $("#modal .modal-body").html(res);
            // title of modal
            $(".modal-header-custom .header-title").html(title);
            document.getElementById('modal').style.display = 'block';
        }
    });
}

function GetCitiesOfState(stateId) {
    if (stateId != 0) {
        $.ajax({
            type: "GET",
            url: "/api/State?stateId=" + stateId,
        }).done(function (res) {
            $("#city").empty();
            $("#city").append('  <option value="0">لطفا شهر را انتخاب کنید</option>');
            $.each(res, function (index, value) {
                $("#city").append('<option value="' + value.cityId + '">' + value.cityName + '</option>');
            });
        });
    } else if (stateId == 0) {
        $("#city").empty();
        $("#city").append('  <option value="0">لطفا استان را انتخاب کنید</option>');
    }
}

function SubmitFormData(form) {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            cache: false,
            processData: false,
            contentType: false,
            success: function (res) {
                if (res.isValid) {
                    // modal body 
                    $("#modal .modal-body").html('');
                    // title of modal
                    $(".modal-header-custom .header-title").html('');
                    document.getElementById('modal').style.display = 'none';
                    location.reload();
                } else
                    $("#modal .modal-body").html(res.html);
            },
            error: function (err) {
                console.log(err);
            }
        });
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex);
    }
}

function SetCartAddress(form, returnUrl) {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            cache: false,
            processData: false,
            contentType: false,
            success: function (res) {
                if (res.isValid) {
                    ShowToast('success', 'ادرس شما با موفقیت انتخاب شد.', returnUrl);
                } else if (!res.isValid && !res.isNull) {
                    ShowToast('danger', 'متاسفانه هنگام ثبت نشانی مقصد شما به مشکلی غیر منتطره برخوردیم.');
                } else if (!res.isValid && res.isNull) {
                    ShowToast('warning', 'لطفا آدرس خود را انتخاب کنید.');
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex);
    }
}

function UpdateProductDetailPage(productId, productName, sellerId) {
    $("#productDetail").load('/UpdateProductDetail/' + productId + '/' + productName + '/' + sellerId);
    let url = window.location.origin + "/Product/" + productId + '/' + productName + '/' + sellerId;
    console.log(url);
    window.history.replaceState(null, productName, url);
}