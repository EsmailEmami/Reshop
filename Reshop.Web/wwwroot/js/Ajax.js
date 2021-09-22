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

// this function is for add select option
function addSelectList(select, selectDropdown, itemValue, itemText) {

    // create new option for select
    let opt = document.createElement('option');
    opt.value = itemValue;
    opt.innerHTML = itemText;
    select.appendChild(opt);

    // create new option for select dropDown
    var op = newEl('div',
        {
            optEl: opt
        });

    op.appendChild(newEl('label', {
        text: opt.text
    }));


    op.addEventListener('click', () => {

        op.optEl.selected = !!!op.optEl.selected;
        select.dispatchEvent(new Event('change'));

    });


    selectDropdown.appendChild(op);
}

// refresh dropdown 
function selectRefresh(select, dropDown) {
    var optext = dropDown.querySelector('span.optext');

    dropDown.removeChild(optext);

    var sel = Array.from(select.selectedOptions)[0];

    var c = newEl('span', {
        class: 'optext',
        text: sel.text
    });

    dropDown.appendChild(c);
}


function GetCitiesOfState(stateId) {

    var select = document.getElementById('city');
    // dropDown 
    var selectDropDown = document.getElementById('city-select');
    // dropDown Options
    var selectDropDownList = selectDropDown.querySelector('.select-dropdown-list');

    if (stateId != '') {
        $.ajax({
            type: "GET",
            url: "/api/State?stateId=" + stateId,
        }).done(function (res) {

            // make empty the select
            select.querySelectorAll('*').forEach(n => n.remove());
            selectDropDownList.querySelectorAll('*').forEach(n => n.remove());

            addSelectList(select, selectDropDownList, '', 'لطفا شهر را انتخاب کنید');


            $.each(res, function (index, value) {
                addSelectList(select, selectDropDownList, value.cityId, value.cityName);
            });

            selectRefresh(select, selectDropDown);

        });
    } else
        // make empty the select
        select.querySelectorAll('*').forEach(n => n.remove());
    selectDropDownList.querySelectorAll('*').forEach(n => n.remove());

    addSelectList(select, selectDropDownList, '', 'لطفا استان را انتخاب کنید');

    selectRefresh(select, selectDropDown);
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

                    if (res.returnUrl !== '' && res.returnUrl.toLowerCase() === 'current') {
                        var loc = window.location.href;
                        ShowToast('success', 'عملیات با موفقیت انجام شد.', loc);
                    } else if (res.returnUrl !== '' && res.returnUrl.toLowerCase() !== 'current') {
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

function UpdateProductDetailShopper(productName, sellerId) {
    $("#changeShopper").load('/Product/ChangeProductShopper?seller=' + sellerId);
    let url = window.location.origin + "/Product/" + productName + '/' + sellerId;
    window.history.replaceState(null, productName, url);
}

function ColorCollapsibleOfManager(btn, where, url) {

    var res = ColorsDetailData(where, url);

    if (res === true) {

        var content = document.getElementById(where);

        if (content.scrollHeight !== 0) {

            btn.classList.toggle("active");

            if (!content.style.maxHeight) {
                if (content.scrollHeight >= 700) {
                    content.style.maxHeight = 700 + "px";
                } else {
                    content.style.maxHeight = content.scrollHeight + "px";
                }

            } else {
                if (content.style.maxHeight != "0px") {
                    content.style.maxHeight = "0px";
                } else {
                    if (content.scrollHeight >= 700) {
                        content.style.maxHeight = 700 + "px";
                    } else {
                        content.style.maxHeight = content.scrollHeight + "px";
                    }
                }
            }
        }
    }
}

function ColorsDetailData(where, url) {

    var result = true;

    $.ajax({
        type: 'GET',
        url: url,
    }).done(function (res) {
        if (res.isValid == false) {
            ShowToast(res.errorType, res.errorText);

            result = false;
        } else {
            var content = document.getElementById(where);
            $(content).html(res);

            result = true;
        }
    });

    return result;
}

function ColorsDiscountDetailData(where, url) {
    $.ajax({
        type: 'GET',
        url: url
    }).done(function (res) {
        var place = document.getElementById(where);
        $(place).html(res);
    });
}


// brand 

function GetBrandsOfStoreTitle(storeTitleId) {

    GetOfficialProductsOfBrand('');
    GetProductsOfOfficialProduct('');

    var select = document.getElementById('brand');
    // dropDown 
    var selectDropDown = document.getElementById('brand-select');
    // dropDown Options
    var selectDropDownList = selectDropDown.querySelector('.select-dropdown-list');

    if (storeTitleId != '') {
        $.ajax({
            type: "GET",
            url: "/api/Product/GetBrandsOfStoreTitle/" + storeTitleId,
        }).done(function (res) {

            // make empty the select
            select.querySelectorAll('*').forEach(n => n.remove());
            selectDropDownList.querySelectorAll('*').forEach(n => n.remove());

            addSelectList(select, selectDropDownList, '', 'لطفا برند را انتخاب کنید');


            $.each(res, function (index, value) {
                addSelectList(select, selectDropDownList, value.item1, value.item2);
            });

            selectRefresh(select, selectDropDown);

        });
    } else
        // make empty the select
        select.querySelectorAll('*').forEach(n => n.remove());
    selectDropDownList.querySelectorAll('*').forEach(n => n.remove());

    addSelectList(select, selectDropDownList, '', 'لطفا عنوان کالا را انتخاب کنید');

    selectRefresh(select, selectDropDown);
}

function GetOfficialProductsOfBrand(brandId) {

    GetProductsOfOfficialProduct('');

    var select = document.getElementById('officialProduct');
    // dropDown 
    var selectDropDown = document.getElementById('officialProduct-select');
    // dropDown Options
    var selectDropDownList = selectDropDown.querySelector('.select-dropdown-list');

    if (brandId !== '') {
        $.ajax({
            type: "GET",
            url: "/api/Product/GetBrandOfficialProducts/" + brandId,
        }).done(function (res) {

            // make empty the select
            select.querySelectorAll('*').forEach(n => n.remove());
            selectDropDownList.querySelectorAll('*').forEach(n => n.remove());

            addSelectList(select, selectDropDownList, '', 'لطفا نام اختصاصی کالا را انتخاب کنید');


            $.each(res, function (index, value) {
                addSelectList(select, selectDropDownList, value.item1, value.item2);
            });

            selectRefresh(select, selectDropDown);

        });
    } else
        // make empty the select
        select.querySelectorAll('*').forEach(n => n.remove());
    selectDropDownList.querySelectorAll('*').forEach(n => n.remove());

    addSelectList(select, selectDropDownList, '', 'لطفا برند را انتخاب کنید');

    selectRefresh(select, selectDropDown);
}

function GetProductsOfOfficialProduct(officialProductId) {

    var select = document.getElementById('product');
    // dropDown 
    var selectDropDown = document.getElementById('product-select');
    // dropDown Options
    var selectDropDownList = selectDropDown.querySelector('.select-dropdown-list');

    if (officialProductId !== '') {
        $.ajax({
            type: "GET",
            url: "/api/Product/GetProductsOfOfficialProduct/" + officialProductId,
        }).done(function (res) {

            // make empty the select
            select.querySelectorAll('*').forEach(n => n.remove());
            selectDropDownList.querySelectorAll('*').forEach(n => n.remove());

            addSelectList(select, selectDropDownList, '', 'لطفا کالا را انتخاب کنید');


            $.each(res, function (index, value) {
                addSelectList(select, selectDropDownList, value.item1, value.item2);
            });

            selectRefresh(select, selectDropDown);

        });
    } else
        // make empty the select
        select.querySelectorAll('*').forEach(n => n.remove());
    selectDropDownList.querySelectorAll('*').forEach(n => n.remove());

    addSelectList(select, selectDropDownList, '', 'لطفا نام اختصاصی کالا را انتخاب کنید');

    selectRefresh(select, selectDropDown);
}