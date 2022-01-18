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

                    if (res.returnUrl != null) {
                        window.location.href = res.returnUrl;
                    }

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

function UpdateCart(form) {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            processData: false,
            contentType: false,
            success: function (res) {
                if (res.isValid) {

                    if (res.errorType != null && res.errorText != null) {
                        ShowToast(res.errorType, res.errorText);
                    }

                    var mainPage = document.querySelector('main');

                    mainPage.innerHTML = res.html;

                    cartCollapse();

                } else {
                    if (res.errorType != null && res.errorText != null) {
                        ShowToast(res.errorType, res.errorText);
                    } 
                }
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
                        ShowToast('success', 'محصول با موفقیت به علاقه مندی ها اضافه شد.');
                    } else if (res.resultType == "ProductReplaced") {
                        ShowToast('success', 'فروشنده محصول با موفقیت تغییر کرد.');
                    }
                } else {

                    debugger;
                    if (res.returnUrl != null) {
                        window.location.href = res.returnUrl;
                    }



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
                if (res.returnUrl != null) {
                    window.location.href = res.returnUrl;
                }

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

function SubmitFinishRequest(form) {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            processData: false,
            contentType: false,
            success: function (res) {
                if (res.isValid) {
                    if (res.returnUrl != null && res.returnUrl.toLowerCase() === 'current') {
                        var loc = window.location.href;
                        ShowToast('success', 'بازخورد شما با موفقیت ثبت شد.', loc);
                    } else if (res.returnUrl != null && res.returnUrl.toLowerCase() !== 'current') {
                        ShowToast('success', 'بازخورد شما با موفقیت ثبت شد.', res.returnUrl);
                    } else {
                        ShowToast('success', 'بازخورد شما با موفقیت ثبت شد.');
                    }
                } else

                    if (res.returnUrl != null) {
                        window.location.href = res.returnUrl;
                    }


                $("#shopper-request-finish").html(res.html);
            }
        });
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex);
    }
}

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
    var selectDropDown = document.getElementById('city_select');
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

function SubmitFormData(form, isModal) {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            processData: false,
            contentType: false,
            success: function (res) {


                var loc;
                if (res.isValid) {

                    if (isModal) {
                        // modal body 
                        $("#modal .modal-body").html('');
                        // title of modal
                        $(".modal-header-custom .header-title").html('');
                        document.getElementById('modal').style.display = 'none';
                    }

                    if (res.errorType != null && res.errorText != null) {
                        if (res.returnUrl != null && res.returnUrl.toLowerCase() === 'current') {
                            loc = window.location.href;
                            ShowToast(res.errorType, res.errorText, loc);
                        } else if (res.returnUrl != null && res.returnUrl.toLowerCase() !== 'current') {
                            loc = window.location.origin + res.returnUrl;
                            ShowToast(res.errorType, res.errorText, loc);
                        } else {
                            ShowToast(res.errorType, res.errorText);
                        }
                    } else {
                        if (res.returnUrl != null && res.returnUrl.toLowerCase() === 'current') {
                            loc = window.location.href;
                            ShowToast('success', 'عملیات با موفقیت انجام شد.', loc);
                        } else if (res.returnUrl != null && res.returnUrl.toLowerCase() !== 'current') {
                            ShowToast('success', 'عملیات با موفقیت انجام شد.', res.returnUrl);
                        } else {
                            ShowToast('success', 'عملیات با موفقیت انجام شد.');
                        }
                    }



                } else {
                    if (isModal) {
                        $("#modal .modal-body").html(res.html);
                    }

                    if (res.errorType != null && res.errorText != null) {
                        if (res.returnUrl != null && res.returnUrl.toLowerCase() === 'current') {
                            loc = window.location.href;
                            ShowToast(res.errorType, res.errorText, loc);
                        } else if (res.returnUrl != null && res.returnUrl.toLowerCase() !== 'current') {
                            ShowToast(res.errorType, res.errorText, res.returnUrl);
                        } else {
                            ShowToast(res.errorType, res.errorText);
                        }
                    } else {
                        if (res.returnUrl != null && res.returnUrl.toLowerCase() === 'current') {
                            loc = window.location.href;
                            ShowToast('warning', 'عملیات با موفقیت انجام نشد.', loc);
                        } else if (res.returnUrl != null && res.returnUrl.toLowerCase() !== 'current') {
                            ShowToast('warning', 'عملیات با موفقیت انجام نشد.', res.returnUrl);
                        } else {
                            ShowToast('warning', 'عملیات با موفقیت انجام نشد.');
                        }
                    }
                }
            }
        });
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex);
    }
}

function AddComment(form) {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            processData: false,
            contentType: false,
            success: function (res) {
                if (res.isValid) {
                    if (res.returnUrl != null && res.returnUrl.toLowerCase() === 'current') {
                        var loc = window.location.href;
                        ShowToast('success', 'بازخورد شما با موفقیت ثبت شد.', loc);
                    } else if (res.returnUrl != null && res.returnUrl.toLowerCase() !== 'current') {
                        ShowToast('success', 'بازخورد شما با موفقیت ثبت شد.', res.returnUrl);
                    } else {
                        ShowToast('success', 'بازخورد شما با موفقیت ثبت شد.');
                    }
                } else

                    if (res.returnUrl != null) {
                        window.location.href = res.returnUrl;
                    }


                $("#newComment").html(res.html);
            }
        });
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex);
    }
}

function AddQuestion(form) {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            processData: false,
            contentType: false,
            success: function (res) {
                if (res.isValid) {
                    if (res.returnUrl != null && res.returnUrl.toLowerCase() === 'current') {
                        var loc = window.location.href;
                        ShowToast('success', 'پرسش شما با موفقیت ثبت شد.', loc);
                    } else if (res.returnUrl != null && res.returnUrl.toLowerCase() !== 'current') {
                        ShowToast('success', 'پرسش شما با موفقیت ثبت شد.', res.returnUrl);
                    } else {
                        ShowToast('success', 'پرسش شما با موفقیت ثبت شد.');
                    }
                } else {
                    if (res.returnUrl != null) {
                        window.location.href = res.returnUrl;
                    }

                    $("#newQuestion").html(res.html);
                }
            }
        });
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex);
    }
}

function ReportComment(form, name, token) {
    var commentId = form[0].defaultValue;
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            processData: false,
            contentType: false,
            success: function (res) {
                var reportBtn = 'report-btn' + commentId;

                if (res.isValid) {
                    // modal body 
                    $("#modal .modal-body").html('');
                    // title of modal
                    $(".modal-header-custom .header-title").html('');
                    document.getElementById('modal').style.display = 'none';

                    ShowToast('success', 'گزارش شما با موفقیت ثبت شد');


                    var tag = document.createElement('form');

                    tag.setAttribute('id', reportBtn);
                    tag.setAttribute('action', '/Product/RemoveCommentFromReport');
                    tag.setAttribute('method', 'post');
                    tag.setAttribute('onsubmit', 'return RemoveReportComment(this)');

                    var input = document.createElement('input');
                    input.setAttribute('type', 'hidden');
                    input.setAttribute('name', 'commentId');
                    input.value = commentId;

                    var button = document.createElement('button');
                    button.setAttribute('type', 'submit');
                    button.classList.add('badge');
                    button.innerHTML = "گزارش شده";

                    var validation = document.createElement('input');
                    validation.setAttribute('name', name);
                    validation.setAttribute('type', 'hidden');
                    validation.value = token;


                    tag.appendChild(input);
                    tag.appendChild(button);
                    tag.appendChild(validation);


                    document.getElementById(reportBtn).replaceWith(tag);

                } else
                    if (res.returnUrl != null) {
                        window.location.href = res.returnUrl;
                    }

                $("#modal .modal-body").html(res.html);
            }
        });
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex);
    }
}

function ReportQuestion(form, name, token) {
    var questionId = form[0].defaultValue;
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

                    ShowToast('success', 'گزارش شما با موفقیت ثبت شد');

                    var reportBtn = 'report-question-btn' + questionId;

                    var tag = document.createElement('form');

                    tag.setAttribute('id', reportBtn);
                    tag.setAttribute('action', '/Question/RemoveReportQuestion');
                    tag.setAttribute('method', 'post');
                    tag.setAttribute('onsubmit', 'return RemoveReportQuestion(this)');

                    var input = document.createElement('input');
                    input.setAttribute('type', 'hidden');
                    input.setAttribute('name', 'questionId');
                    input.value = questionId;

                    var button = document.createElement('button');
                    button.setAttribute('type', 'submit');
                    button.classList.add('badge');
                    button.innerHTML = "گزارش شده";

                    var validation = document.createElement('input');
                    validation.setAttribute('name', name);
                    validation.setAttribute('type', 'hidden');
                    validation.value = token;


                    tag.appendChild(input);
                    tag.appendChild(button);
                    tag.appendChild(validation);


                    document.getElementById(reportBtn).replaceWith(tag);

                } else
                    if (res.returnUrl != null) {
                        window.location.href = res.returnUrl;
                    }

                $("#modal .modal-body").html(res.html);
            }
        });
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex);
    }
}

function RemoveReportQuestion(form) {
    var questionId = form[0].defaultValue;
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            processData: false,
            contentType: false,
            success: function (res) {
                var reportBtn = 'report-question-btn' + questionId;

                if (res.isValid) {
                    ShowToast('success', 'گزارش شما با موفقیت حذف شد.');

                    var tag = document.createElement('a');

                    tag.setAttribute('id', reportBtn);
                    tag.innerHTML = "گزارش";
                    tag.classList.add('badge');

                    var origin = window.location.origin;

                    var url = origin + '/Question/ReportQuestion?questionId=' + questionId;

                    tag.onclick = function () {
                        ShowModal(url, 'گزارش بازخورد');
                    }



                    document.getElementById(reportBtn).replaceWith(tag);

                } else {
                    if (res.returnUrl != null) {
                        window.location.href = res.returnUrl;
                    }

                    ShowToast(res.errorType, res.errorText);
                }
            }
        });
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex);
    }
}

function RemoveReportComment(form) {
    var commentId = form[0].defaultValue;
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            processData: false,
            contentType: false,
            success: function (res) {
                var reportBtn = 'report-btn' + commentId;

                if (res.isValid) {
                    ShowToast('success', 'گزارش شما با موفقیت حذف شد.');

                    var tag = document.createElement('a');

                    tag.setAttribute('id', reportBtn);
                    tag.innerHTML = "گزارش";
                    tag.classList.add('badge');

                    var origin = window.location.origin;

                    var url = origin + '/Comment/ReportComment?commentId=' + commentId;

                    tag.onclick = function () {
                        ShowModal(url, 'گزارش بازخورد');
                    }



                    document.getElementById(reportBtn).replaceWith(tag);

                } else {
                    if (res.returnUrl != null) {
                        window.location.href = res.returnUrl;
                    }

                    ShowToast(res.errorType, res.errorText);
                }
            }
        });
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex);
    }
}

function LikeOrDisLikeComment(form) {
    var commentId = form[0].defaultValue;
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            processData: false,
            contentType: false,
            success: function (res) {

                if (res.isValid) {
                    var likeBtnId = 'like-btn' + commentId;
                    var disLikeBtnId = 'dislike-btn' + commentId;

                    var btnLike = document.getElementById(likeBtnId);
                    var spanLike = btnLike.querySelector('span');
                    var likeCount = parseInt(spanLike.innerHTML);

                    var btnDislike = document.getElementById(disLikeBtnId);
                    var spanDislike = btnDislike.querySelector('span');
                    var dislikeCount = parseInt(spanDislike.innerHTML);

                    switch (res.where) {
                        case "LikeAdded":
                            {
                                btnLike.classList.add('active');
                                spanLike.innerHTML = likeCount + 1;

                                break;
                            }
                        case "DislikeAdded":
                            {
                                btnDislike.classList.add('active');
                                spanDislike.innerHTML = dislikeCount + 1;

                                break;
                            }
                        case "LikeRemoved":
                            {
                                btnLike.classList.remove('active');
                                spanLike.innerHTML = likeCount - 1;

                                break;
                            }
                        case "DislikeRemoved":
                            {
                                btnDislike.classList.remove('active');
                                spanDislike.innerHTML = dislikeCount - 1;

                                break;
                            }
                        case "LikeEdited":
                            {
                                btnLike.classList.add('active');
                                btnDislike.classList.remove('active');

                                spanLike.innerHTML = likeCount + 1;
                                spanDislike.innerHTML = dislikeCount - 1;
                                break;
                            }
                        case "DislikeEdited":
                            {
                                btnDislike.classList.add('active');
                                btnLike.classList.remove('active');

                                spanDislike.innerHTML = dislikeCount + 1;
                                spanLike.innerHTML = likeCount - 1;
                                break;
                            }
                    }

                    ShowToast(res.errorType, res.errorText);

                } else {
                    if (res.returnUrl != null) {
                        window.location.href = res.returnUrl;
                    }

                    ShowToast(res.errorType, res.errorText);
                }
            }
        });
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex);
    }
}

function LikeOrDisLikeQuestionAnswer(form) {
    var questionAnswerId = form[0].defaultValue;
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            processData: false,
            contentType: false,
            success: function (res) {

                if (res.isValid) {
                    var btnId = 'like-question-answer-btn' + questionAnswerId;

                    debugger;

                    var btn = document.getElementById(btnId);

                    var spanLike = btn.querySelector('span');
                    var likeCount = parseInt(spanLike.innerHTML);

                    switch (res.where) {
                        case "Added":
                            {
                                btn.classList.add('active');
                                spanLike.innerHTML = likeCount + 1;

                                break;
                            }
                        case "Deleted":
                            {
                                btn.classList.remove('active');
                                spanLike.innerHTML = likeCount - 1;

                                break;
                            }
                    }

                    ShowToast(res.errorType, res.errorText);

                } else {
                    if (res.returnUrl != null) {
                        window.location.href = res.returnUrl;
                    }

                    ShowToast(res.errorType, res.errorText);
                }
            }
        });
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex);
    }
}

function ReportQuestionAnswer(form, name, token) {
    var questionAnswerId = form[0].defaultValue;
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

                    ShowToast('success', 'گزارش شما با موفقیت ثبت شد');

                    var reportBtn = 'report-question-answer-btn' + questionAnswerId;

                    var tag = document.createElement('form');

                    tag.setAttribute('id', reportBtn);
                    tag.setAttribute('action', '/Question/RemoveReportQuestionAnswer');
                    tag.setAttribute('method', 'post');
                    tag.setAttribute('onsubmit', 'return RemoveReportQuestionAnswer(this)');

                    var input = document.createElement('input');
                    input.setAttribute('type', 'hidden');
                    input.setAttribute('name', 'questionAnswerId');
                    input.value = questionAnswerId;

                    var button = document.createElement('button');
                    button.setAttribute('type', 'submit');
                    button.classList.add('report-answer', 'active');

                    var icon = document.createElement('i');
                    icon.classList.add('far', 'fa-flag');

                    button.appendChild(icon);

                    var validation = document.createElement('input');
                    validation.setAttribute('name', name);
                    validation.setAttribute('type', 'hidden');
                    validation.value = token;


                    tag.appendChild(input);
                    tag.appendChild(button);
                    tag.appendChild(validation);


                    document.getElementById(reportBtn).replaceWith(tag);

                } else
                    if (res.returnUrl != null) {
                        window.location.href = res.returnUrl;
                    }

                $("#modal .modal-body").html(res.html);
            }
        });
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex);
    }
}

function RemoveReportQuestionAnswer(form) {
    var questionAnswerId = form[0].defaultValue;
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            processData: false,
            contentType: false,
            success: function (res) {
                var reportBtn = 'report-question-answer-btn' + questionAnswerId;

                if (res.isValid) {
                    ShowToast('success', 'گزارش شما با موفقیت حذف شد.');

                    var tag = document.createElement('a');

                    tag.setAttribute('id', reportBtn);
                    tag.classList.add('report-answer');

                    var icon = document.createElement('i');
                    icon.classList.add('far', 'fa-flag');

                    tag.appendChild(icon);


                    var origin = window.location.origin;

                    var url = origin + '/Question/RemoveReportQuestionAnswer?questionAnswerId=' + questionAnswerId;

                    tag.onclick = function () {
                        ShowModal(url, 'گزارش پاسخ پرسش');
                    }


                    document.getElementById(reportBtn).replaceWith(tag);
                } else {
                    if (res.returnUrl != null) {
                        window.location.href = res.returnUrl;
                    }

                    ShowToast(res.errorType, res.errorText);
                }
            }
        });
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex);
    }
}

function LikeOrDisLikeQuestion(form) {
    var questionId = form[0].defaultValue;
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            processData: false,
            contentType: false,
            success: function (res) {

                if (res.isValid) {
                    var btnId = 'like-question-btn' + questionId;

                    var btn = document.getElementById(btnId);

                    var spanLike = btn.querySelector('span');
                    var likeCount = parseInt(spanLike.innerHTML);

                    switch (res.where) {
                        case "Added":
                            {
                                btn.classList.add('active');
                                spanLike.innerHTML = likeCount + 1;

                                break;
                            }
                        case "Deleted":
                            {
                                btn.classList.remove('active');
                                spanLike.innerHTML = likeCount - 1;

                                break;
                            }
                    }

                    ShowToast(res.errorType, res.errorText);

                } else {
                    if (res.returnUrl != null) {
                        window.location.href = res.returnUrl;
                    }

                    ShowToast(res.errorType, res.errorText);
                }
            }
        });
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex);
    }
}

function UpdateProductDetailShopper(productName, sellerId) {

    $.ajax({
        type: 'GET',
        url: '/Product/ChangeProductShopper?seller=' + sellerId,
        processData: false,
        contentType: false,
        success: function (res) {
            var htmlObject = document.createElement('div');
            htmlObject.innerHTML = res;


            /*change like btn*/
            var realLike = document.getElementById('add-favorite');
            var resLike = htmlObject.querySelector('#add-favorite');

            realLike.replaceWith(resLike);


            /*change shortKey*/
            var realShortKey = document.getElementById('short-key');
            var resShortKey = htmlObject.querySelector('#short-key-part');

            realShortKey.innerHTML = resShortKey.innerHTML;

            /*change shopper*/
            var realShopperSection = document.getElementById('shopper');
            var resShopperSection = htmlObject.querySelector('#shopper-part');
            realShopperSection.innerHTML = resShopperSection.innerHTML;

            /*change product data*/
            var realProductData = document.getElementById('product-data');
            var resProductData = htmlObject.querySelector('#product-data-part');

            realProductData.innerHTML = resProductData.innerHTML;

            callSelect();

          /* change url */
            var url = window.location.origin + "/Product/" + productName + '/' + sellerId;
            window.history.replaceState(null, productName, url);
        }
    });
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
    GetChildCategoriesOfBrand('');

    var select = document.getElementById('brand');
    if (select == null) {
        return null;
    }

    var selectDropDown = document.getElementById('brand_select');
    if (selectDropDown == null) {
        return null;
    }

    // dropDown Options
    var selectDropDownList = selectDropDown.querySelector('.select-dropdown-list');
    if (selectDropDownList == null) {
        return null;
    }

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
    if (select == null) {
        return null;
    }

    // dropDown 
    var selectDropDown = document.getElementById('officialProduct_select');
    if (selectDropDown == null) {
        return null;
    }

    // dropDown Options
    var selectDropDownList = selectDropDown.querySelector('.select-dropdown-list');
    if (selectDropDownList == null) {
        return null;
    }


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

    if (select == null) {
        return null;
    }

    // dropDown 
    var selectDropDown = document.getElementById('product_select');

    if (selectDropDown == null) {
        return null;
    }
    // dropDown Options
    var selectDropDownList = selectDropDown.querySelector('.select-dropdown-list');

    if (selectDropDownList == null) {
        return null;
    }

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

function GetChildCategoriesOfBrand(brandId) {
    var select = document.getElementById('childCategory');
    if (select == null) {
        return null;
    }

    var selectDropDown = document.getElementById('childCategory_select');
    if (selectDropDown == null) {
        return null;
    }

    // dropDown Options
    var selectDropDownList = selectDropDown.querySelector('.select-dropdown-list');
    if (selectDropDownList == null) {
        return null;
    }

    if (brandId != '') {
        $.ajax({
            type: "GET",
            url: "/api/Product/GetChildCategoriesOfBrand/" + brandId,
        }).done(function (res) {

            // make empty the select
            select.querySelectorAll('*').forEach(n => n.remove());
            selectDropDownList.querySelectorAll('*').forEach(n => n.remove());

            addSelectList(select, selectDropDownList, '', 'لطفا زیر گروه را انتخاب کنید');


            $.each(res,
                function (index, value) {

                    addSelectList(select, selectDropDownList, value.item1, value.item2);
                });

            selectRefresh(select, selectDropDown);

        });
    } else {
        // make empty the select
        select.querySelectorAll('*').forEach(n => n.remove());
        selectDropDownList.querySelectorAll('*').forEach(n => n.remove());

        addSelectList(select, selectDropDownList, '', 'لطفا برند را انتخاب کنید');

        selectRefresh(select, selectDropDown);
    }
}


function GetCpusOfChipset(chipsetId) {

    
    var select = document.getElementById('cpu');
    if (select == null) {
        return null;
    }

    // dropDown 
    var selectDropDown = document.getElementById('cpu_select');
    if (selectDropDown == null) {
        return null;
    }

    // dropDown Options
    var selectDropDownList = selectDropDown.querySelector('.select-dropdown-list');
    if (selectDropDownList == null) {
        return null;
    }


    if (chipsetId !== '') {
        $.ajax({
            type: "GET",
            url: "/api/Product/GetCpusOfChipset/" + chipsetId,
        }).done(function (res) {

            // make empty the select
            select.querySelectorAll('*').forEach(n => n.remove());
            selectDropDownList.querySelectorAll('*').forEach(n => n.remove());

            addSelectList(select, selectDropDownList, '', 'لطفا پردازنده مرکزی را انتخاب کنید');


            $.each(res, function (index, value) {
                addSelectList(select, selectDropDownList, value.item1, value.item2);
            });

            selectRefresh(select, selectDropDown);

        });
    } else
    // make empty the select
        select.querySelectorAll('*').forEach(n => n.remove());
    selectDropDownList.querySelectorAll('*').forEach(n => n.remove());

    addSelectList(select, selectDropDownList, '', 'لطفا تراشه را انتخاب کنید');

    selectRefresh(select, selectDropDown);
}

function GetGpusOfChipset(chipsetId) {


    var select = document.getElementById('gpu');
    if (select == null) {
        return null;
    }

    // dropDown 
    var selectDropDown = document.getElementById('gpu_select');
    if (selectDropDown == null) {
        return null;
    }

    // dropDown Options
    var selectDropDownList = selectDropDown.querySelector('.select-dropdown-list');
    if (selectDropDownList == null) {
        return null;
    }


    if (chipsetId !== '') {
        $.ajax({
            type: "GET",
            url: "/api/Product/GetGpusOfChipset/" + chipsetId,
        }).done(function (res) {

            // make empty the select
            select.querySelectorAll('*').forEach(n => n.remove());
            selectDropDownList.querySelectorAll('*').forEach(n => n.remove());

            addSelectList(select, selectDropDownList, '', 'لطفا پردازنده گرافیکی را انتخاب کنید');


            $.each(res, function (index, value) {
                addSelectList(select, selectDropDownList, value.item1, value.item2);
            });

            selectRefresh(select, selectDropDown);

        });
    } else
    // make empty the select
        select.querySelectorAll('*').forEach(n => n.remove());
    selectDropDownList.querySelectorAll('*').forEach(n => n.remove());

    addSelectList(select, selectDropDownList, '', 'لطفا تراشه را انتخاب کنید');

    selectRefresh(select, selectDropDown);
}

function GetOperatingSystemVersionsOfOperatingSystem(operatingSystemId) {


    var select = document.getElementById('operatingSystemVersion');
    if (select == null) {
        return null;
    }

    // dropDown 
    var selectDropDown = document.getElementById('operatingSystemVersion_select');
    if (selectDropDown == null) {
        return null;
    }

    // dropDown Options
    var selectDropDownList = selectDropDown.querySelector('.select-dropdown-list');
    if (selectDropDownList == null) {
        return null;
    }


    if (operatingSystemId !== '') {
        $.ajax({
            type: "GET",
            url: "/api/Product/GetOperatingSystemVersionsOfOperatingSystem/" + operatingSystemId,
        }).done(function (res) {

            // make empty the select
            select.querySelectorAll('*').forEach(n => n.remove());
            selectDropDownList.querySelectorAll('*').forEach(n => n.remove());

            addSelectList(select, selectDropDownList, '', 'لطفا نسخه سیستم عامل را انتخاب کنید');


            $.each(res, function (index, value) {
                addSelectList(select, selectDropDownList, value.item1, value.item2);
            });

            selectRefresh(select, selectDropDown);

        });
    } else
    // make empty the select
        select.querySelectorAll('*').forEach(n => n.remove());
    selectDropDownList.querySelectorAll('*').forEach(n => n.remove());

    addSelectList(select, selectDropDownList, '', 'لطفا سیستم عامل را انتخاب کنید');

    selectRefresh(select, selectDropDown);
}
