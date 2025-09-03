$(document).ready(function () {

    // Submit to ANM button
    $("#formReceiveUMA").on('submit', function (e) {
        e.preventDefault();

        var formData = new FormData(this);

        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (data) {
                if (data.response.statusCode == 200) {
                    ShowNoty("success", data.message);
                    setTimeout(function () {
                        window.location.reload();
                    }, 2000);
                } else {
                    ShowNoty("error", data.message);
                }
            },
            error: function () {
                ShowNoty("error", "An error occurred while processing the request.");
            }
        });
    });

    // Date picker for ANM submission
    $("#SubmittedToANMDate").on('change', function () {
        var selectedDate = $(this).val();
        if (selectedDate) {
            $(this).removeClass('is-invalid').addClass('is-valid');
        }
    });
});