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
        cache: false,
        success: function (res) {

            if (res.isValid == false) {
                ShowToast(res.errorType, res.errorText);
            } else {
                // modal body 
                $("#modal .modal-body").html(res);
                // title of modal
                $(".modal-header-custom .header-title").html(title);
                document.getElementById('modal').style.display = 'block';
            }
        }
    });
}

function GetCitiesOfState(stateId) {
    var city = document.getElementById('city');
    var cityOptionList = $('.select-dropdown-list')[1];


    function addSelectList(itemValue, itemText) {
        let opt = document.createElement('option');
        opt.value = itemValue;
        opt.innerHTML = itemText;
        city.appendChild(opt);

        var op = newEl('div',
            {
                optEl: opt
            });

        op.appendChild(newEl('label', {
            text: opt.text
        }));


        op.addEventListener('click', () => {

            op.optEl.selected = !!!op.optEl.selected;
            city.dispatchEvent(new Event('change'));

        });


        cityOptionList.appendChild(op);
    }


    var div = document.querySelectorAll('.select-dropdown')[1];

    function refresh() {
        div.querySelectorAll('span.optext, span.placeholder').forEach(t => div.removeChild(t));
        var sel = Array.from(city.selectedOptions)[0];

        console.log(sel);

        var c = newEl('span', {
            class: 'optext',
            text: sel.text
        });

        div.appendChild(c);
    }

    if (stateId != 0) {
        $.ajax({
            type: "GET",
            url: "/api/State?stateId=" + stateId,
        }).done(function (res) {


            // make empty the select
            city.querySelectorAll('*').forEach(n => n.remove());

            // select city option
            var selectCityOption = document.createElement('option');
            selectCityOption.value = 0;
            selectCityOption.innerHTML = "لطفا شهر را انتخاب کنید";
            city.appendChild(selectCityOption);

            var op = newEl('div',
                {
                    optEl: selectCityOption
                });

            op.appendChild(newEl('label', {
                text: selectCityOption.text
            }));


            op.addEventListener('click', () => {

                op.optEl.selected = !!!op.optEl.selected;
                city.dispatchEvent(new Event('change'));

            });

            cityOptionList.querySelectorAll('*').forEach(n => n.remove());

            cityOptionList.appendChild(op);


            $.each(res, function (index, value) {
                addSelectList(value.cityId, value.cityName);
            });

            refresh();

        });
    } else if (stateId == 0) {
        // make empty the select
        city.querySelectorAll('*').forEach(n => n.remove());

        // select city option
        var selectStateOption = document.createElement('option');
        selectStateOption.value = 0;
        selectStateOption.innerHTML = "لطفا استان را انتخاب کنید";
        city.appendChild(selectStateOption);

        var op = newEl('div',
            {
                optEl: selectStateOption
            });

        op.appendChild(newEl('label', {
            text: selectStateOption.text
        }));

        op.addEventListener('click', () => {

            op.optEl.selected = !!!op.optEl.selected;
            city.dispatchEvent(new Event('change'));

        });


        cityOptionList.querySelectorAll('*').forEach(n => n.remove());
        cityOptionList.appendChild(op);
        refresh();
    }
}

function SubmitFormData(form) {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            processData: false,
            contentType: false,
            success: function (res) {
                if (res.isValid) {
                    // modal body 
                    $("#modal .modal-body").html('');
                    // title of modal
                    $(".modal-header-custom .header-title").html('');
                    document.getElementById('modal').style.display = 'none';

                    if (res.returnUrl != "") {
                        ShowToast('success', 'عملیات با موفقیت انجام شد.', res.returnUrl);
                    } else {
                        ShowToast('success', 'عملیات با موفقیت انجام شد.');
                    }

                } else
                    $("#modal .modal-body").html(res.html);
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
    window.history.replaceState(null, productName, url);
}


function ColorsDetailData(where, productId, colorId) {

    $.ajax({
        type: 'GET',
        url: '/AccountManager/ShopperProductColorDetail?productId=' + productId + '&colorId=' + colorId,
        cache: true,
    }).done(function (res) {
        var place = document.getElementById(where);

        $(place).html(res);
    });
}

function ColorsDiscountDetailData(where, productId, colorId) {

    $.ajax({
        type: 'GET',
        url: '/AccountManager/ShopperProductDiscountDetail?productId=' + productId + '&colorId=' + colorId,
        cache: true
    }).done(function (res) {
        var place = document.getElementById(where);

        $(place).html(res);
    });
}
