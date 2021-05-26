$(document).ready(function () {
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