$(document).ready(function () {
    $(".img-thumbnail").click(function () {
        $.post("ShowPartialView", function (data) {
            if (data) {
                $('#loadView').html(data);
            }
        });
    });

    $(".close,#btn-close").click(function () {
        test.stop();
    });
});