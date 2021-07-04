

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

        case "danger":
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


function BrandFilter(input) {

    let txt, txtValue, checkbox;

    txt = $(".main-filter .filter-by-brand .brands span");
    checkbox = $(".main-filter .filter-by-brand .brands input[type=checkbox]");

    for (i = 0; i < txt.length; i++) {

        txtValue = txt[i].textContent || txt[i].innerText;
        if (txtValue.indexOf(input.value) > -1) {
            txt[i].style.display = "";
            checkbox[i].style.display = "";
        } else {
            txt[i].style.display = "none";
            checkbox[i].style.display = "none";
        }
    }

    let btn = document.getElementById("brand-remove");

    if ($.trim(input.value).length) {
        btn.setAttribute("style", "visibility: visible;opacity: 1;");
    } else {
        btn.setAttribute("style", "");
    }
}

function ResetBrandFilterValue(btn) {
    let input = document.getElementById("brand-filter");
    input.value = "";

    btn.setAttribute("style", "");

    let txt = $(".main-filter .filter-by-brand .brands span");
    let checkbox = $(".main-filter .filter-by-brand .brands input[type=checkbox]");

    for (i = 0; i < txt.length; i++) {
        txt[i].style.display = "";
        checkbox[i].style.display = "";
    }

}

$(function () {
    if (window.innerWidth > 767) {
        let collapse = document.getElementsByClassName('collapse');
        for (let i = 0; i < collapse.length; i++) {
            collapse[i].classList.add("show");
        }
    }

    window.addEventListener('resize', function () {
        let collapse = document.getElementsByClassName('collapse');

        if (window.innerWidth > 767) {

            for (let i = 0; i < collapse.length; i++) {
                collapse[i].classList.add("show");
            }
        } else {
            for (let i = 0; i < collapse.length; i++) {
                collapse[i].classList.remove("show");
            }
        }
    });
});

function SubmitForm(formId) {
    $('#' + formId).submit();
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



$(function () {
    let formFieldInputs = $(".form-field input");
    let formFieldTextAreas = $(".form-field textarea");



    for (let i = 0; i < formFieldInputs.length; i++) {
        let input = formFieldInputs[i];

        if (input.value != "") {
            $(input).addClass("active");
        }

        $(input).focusout(function () {
            if (this.value != "") {
                $(this).addClass("active");
            } else {
                $(this).removeClass("active");
            }
        });
    }

    for (let i = 0; i < formFieldTextAreas.length; i++) {
        let textarea = formFieldTextAreas[i];

        if (textarea.value != "") {
            $(textarea).addClass("active");
        }

        $(textarea).focusout(function () {
            if (this.value != "") {
                $(this).addClass("active");
            } else {
                $(this).removeClass("active");
            }
        });
    }
});


$(function () {
    function MultiselectDropdown(options) {

        function newEl(tag, attrs) {
            var e = document.createElement(tag);
            if (attrs !== undefined) Object.keys(attrs).forEach(k => {
                if (k === 'class') { Array.isArray(attrs[k]) ? attrs[k].forEach(o => o !== '' ? e.classList.add(o) : 0) : (attrs[k] !== '' ? e.classList.add(attrs[k]) : 0) }
                else if (k === 'style') {
                    Object.keys(attrs[k]).forEach(ks => {
                        e.style[ks] = attrs[k][ks];
                    });
                }
                else if (k === 'text') { attrs[k] === '' ? e.innerHTML = '&nbsp;' : e.innerText = attrs[k] }
                else e[k] = attrs[k];
            });
            return e;
        }


        document.querySelectorAll("select[multiple]").forEach((el, k) => {

            var div = newEl('div', { class: 'multiselect-dropdown' });
            el.style.display = 'none';
            el.parentNode.insertBefore(div, el.nextSibling);
            var listWrap = newEl('div', { class: 'multiselect-dropdown-list-wrapper' });
            var list = newEl('div', { class: 'multiselect-dropdown-list' });
            var search = newEl('input', { class: ['multiselect-dropdown-search'], style: { display: el.attributes['multiselect-search']?.value === 'true' ? 'block' : 'none' }, placeholder: 'جستجو' });
            listWrap.appendChild(search);
            div.appendChild(listWrap);
            listWrap.appendChild(list);

            el.loadOptions = () => {
                list.innerHTML = '';

                if (el.attributes['multiselect-select-all']?.value == 'true') {
                    var op = newEl('div', { class: 'multiselect-dropdown-all-selector' })
                    var ic = newEl('input', { type: 'checkbox' });
                    op.appendChild(ic);
                    op.appendChild(newEl('label', { text: "انتخاب همه" }));

                    op.addEventListener('click', () => {
                        op.classList.toggle('checked');
                        op.querySelector("input").checked = !op.querySelector("input").checked;

                        var ch = op.querySelector("input").checked;
                        list.querySelectorAll("input").forEach(i => i.checked = ch);
                        Array.from(el.options).map(x => x.selected = ch);

                        el.dispatchEvent(new Event('change'));
                    });
                    ic.addEventListener('click', (ev) => {
                        ic.checked = !ic.checked;
                    });

                    list.appendChild(op);
                }

                Array.from(el.options).map(o => {
                    var op = newEl('div', { class: o.selected ? 'checked' : '', optEl: o })
                    var ic = newEl('input', { type: 'checkbox', checked: o.selected });
                    op.appendChild(ic);
                    op.appendChild(newEl('label', { text: o.text }));

                    op.addEventListener('click', () => {
                        op.classList.toggle('checked');
                        op.querySelector("input").checked = !op.querySelector("input").checked;
                        op.optEl.selected = !!!op.optEl.selected;
                        el.dispatchEvent(new Event('change'));
                    });
                    ic.addEventListener('click', (ev) => {
                        ic.checked = !ic.checked;
                    });

                    list.appendChild(op);
                });
                div.listEl = listWrap;

                div.refresh = () => {
                    div.querySelectorAll('span.optext, span.placeholder').forEach(t => div.removeChild(t));
                    var sels = Array.from(el.selectedOptions);
                    if (sels.length > (el.attributes['multiselect-max-items']?.value ?? 5)) {
                        div.appendChild(newEl('span', { class: ['optext', 'maxselected'], text: sels.length + ' ' + 'انتخاب شده' }));
                    }
                    else {
                        sels.map(x => {
                            var c = newEl('span', { class: 'optext', text: x.text });
                            div.appendChild(c);
                        });
                    }
                    if (0 == el.selectedOptions.length) div.appendChild(newEl('span', { class: 'placeholder', text: el.attributes['placeholder']?.value ?? "عناوین فروشنده" }));
                };
                div.refresh();
            }
            el.loadOptions();

            search.addEventListener('input', () => {
                list.querySelectorAll("div").forEach(d => {
                    var txt = d.querySelector("label").innerText.toUpperCase();
                    d.style.display = txt.includes(search.value.toUpperCase()) ? 'block' : 'none';
                });
            });

            div.addEventListener('click', () => {
                div.listEl.style.display = 'block';
                search.focus();
                search.select();
            });

            document.addEventListener('click', function (event) {
                if (!div.contains(event.target)) {
                    listWrap.style.display = 'none';
                    div.refresh();
                }
            });
        });
    }

    window.addEventListener('load', () => {
        MultiselectDropdown(window.MultiselectDropdownOptions);
    });

});