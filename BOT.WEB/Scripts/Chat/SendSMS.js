function TryMe() {
    $.ajax({
        url: "SendCodeSMS",
        async: true,
        type: 'POST',
        data: { to: "9381044748", msg: "prueba" },
        success: function (data) {
            console.log("success");
            console.log(data);

        },
        error: function (jqXHR, textStatus, error) {
            var err = textStatus + ", " + error;
            console.log(err);
        }
    });
}