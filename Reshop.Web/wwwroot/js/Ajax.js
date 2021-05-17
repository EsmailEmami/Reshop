function AddTypeForm(form) {
    $.ajax({
        type: "POST",
        url: form.action,
        data: new FormData(form),
        contentType: false,
        processData: false,
        success: function (res) {
            if (res.Successful) {
                swal("با موفقیت انجام شد", "You clicked the button!", "success");
            } else {
                swal("با موفقیت انجام شد", "You clicked the button!", "error");
            }
        }
    });
}