
function SendMessage(msg, silent) {
    silent = silent === undefined ? false : silent;
    const sendObject = {
        "input": {
            "message_type": "text",
            "text": msg
        }
    };
    const sendOptions = {
        "silent": silent
    }
    wats.send(sendObject, sendOptions).catch(function (error) {
        console.error('This message did not send!');
    });

}

function DefaultOptionResponseEventHandler(input) {
    $(".remove").remove();
    if (initialIntent === "ResetearContraseñaDelEquipo") {

        if (input.value == "si")
            context = "email";
        if (input.value == "SMS")
            context = "sms";
        if (input.value == "Envío de código a la cuenta de correo personal")
            context = "emailpersonal";

    }
    SendMessage(input.value);
}

function get_personal_EmailHandler(event) {
    let infoEmployee;
    const element = document.createElement('div');

    //Crear petición ajax al chat controller
    $.ajax({
        url: "GetInfoEmployee",
        async: true,
        type: 'POST',
        data: { ficha: userw.Ficha },
        success: function (data) {
            debugger;
            let email = data.Data.EmailPersonal;
            userw.Name = data.Data.Nombre;
            userw.EmailEmpresa = data.Data.EMAILTRABAJO;
            userw.EmailPersonal = email;

            if (email !== "" && email !== undefined) {

                let posicionArroba = email.indexOf("@@");
                //posicionArroba = posicionArroba + 1;
                let arroba = email.substring(posicionArroba, posicionArroba + 1); //@@

                let posicionSig = posicionArroba + 1;
                let inicioCorreo = email.substring(posicionSig, posicionSig + 1); //@@

                let crypt2 = userw.EmailPersonal.substring(0, 1)
                    + "...." + arroba + inicioCorreo + "..."
                    + userw.EmailPersonal.substring(userw.EmailPersonal.length - 5);

                const element = document.createElement('div');
                element.setAttribute('class', 'custom_received');
                element.innerHTML = "Se enviará un código de verificación al correo electronico " + "<b>" + crypt2 + "</b>"
                    + " ¿Estas de acuerdo?";

                event.data.element.appendChild(element);

                let options = [];
                options[0] = "Si";
                options[1] = "No";

                for (var x = 0; x < options.length; x++) {
                    let opt = options[x];

                    $("#tmp_option").html(opt);
                    let $opt = $("#tmp_option");

                    $opt.clone().appendTo(event.data.element).removeAttr("id").addClass("remove").attr("value", opt).click(function () {
                        let f = new Function("DefaultOptionResponseEventHandler(this)");
                        f.call(this);
                    });
                }
            }

        },

        error: function (jqXHR, textStatus, error) {
            var err = textStatus + ", " + error;
            console.log(err);
        }
    });


}

function send_mailHandler(event) {

    const element = document.createElement('div');
    element.setAttribute('class', 'custom_received');
    element.innerHTML = "Enviando código de verificación...";
    event.data.element.appendChild(element);

    $.ajax({
        url: "SendMail",
        async: true,
        type: 'POST',
        data: { destinatario: userw.EmailPersonal, msg: "El código de verificación de -Asistente 400- es: 1234" },
        success: function (data) {

            console.log(data);
            element.innerHTML = "Se ha enviado el código. Por favor, ingreselo a continuación";

            event.data.element.appendChild(element);

            //#####


            let options = [];
            options[0] = "Volver a enviar código";
            options[1] = "Intentar por otro medio";

            for (var x = 0; x < options.length; x++) {
                let opt = options[x];

                $("#tmp_option").html(opt);
                let $opt = $("#tmp_option");

                $opt.clone().appendTo(event.data.element).removeAttr("id").addClass("remove").attr("value", opt).click(function () {
                    let f = new Function("DefaultOptionResponseEventHandler(this)");
                    f.call(this);
                });
            }

        },
        error: function (jqXHR, textStatus, error) {
            var err = textStatus + ", " + error;
            console.log(err);
        }
    });



}

function ask_emailHandlerEvent(input) {
    DefaultOptionResponseEventHandler(input);
}

function welcomeHandler(event) {

    const element = document.createElement('div');
    element.setAttribute('class', 'custom_received');

    var html = "Bienvenido " + userw.Name + "!";
    //if (initialIntent === "Identificame") {
    //}
    if (initialIntent === "ResetearContraseñaDelEquipo") {

        html += "<br /><br />Haz click en la siguiente <a target='_blank' href='changepassword?email=" + (userw.EmailEmpresa === undefined ? userw.Email : userw.EmailEmpresa) + "'>liga</a>  para ingresar tu nueva contraseña.";
        //ajax to IDM
        element.innerHTML = html;

        //SendMessage("evaluame", true);
    }

    event.data.element.appendChild(element);
}

function smsHandler(event) {
    $.ajax({
        url: "GetInfoEmployee",
        async: true,
        type: 'POST',
        data: { ficha: userw.Ficha },
        success: function (data) {
            userw.Name = data.Data.Nombre;
            userw.Email = data.Data.EmailPersonal;
            userw.PhoneNumber = data.Data.TelefonoCelular;
            userw.EmailEmpresa = data.Data.EMAILTRABAJO;

            let crypt = userw.PhoneNumber.substring(0, 1)
                + "xxxxxxx"
                + userw.PhoneNumber.substring(userw.PhoneNumber.length - 2)

            const element = document.createElement('div');
            element.setAttribute('class', 'custom_received');
            element.innerHTML = "Se enviará un código de verificación al número " + crypt
                + " ¿Estas de acuerdo?";

            event.data.element.appendChild(element);

            let options = [];
            options[0] = "Si";
            options[1] = "No";

            for (var x = 0; x < options.length; x++) {
                let opt = options[x];

                $("#tmp_option").html(opt);
                let $opt = $("#tmp_option");

                $opt.clone().appendTo(event.data.element).removeAttr("id").addClass("remove").attr("value", opt).click(function () {
                    let f = new Function("DefaultOptionResponseEventHandler(this)");
                    f.call(this);
                });
            }
        },
        error: function (jqXHR, textStatus, error) {
            var err = textStatus + ", " + error;
            console.log(err);
        }
    });

}

function send_smsHandler(event) {
    const element = document.createElement('div');
    element.setAttribute('class', 'custom_received');
    element.innerHTML = "Enviando código de verificación...";

    event.data.element.appendChild(element);

    $.ajax({
        url: "SendCodeSMS",
        async: true,
        type: 'POST',
        data: { to: userw.PhoneNumber, msg: "El código de verificación de -Asistente 400- es: 1234" },
        success: function (data) {

            console.log(data);
            element.innerHTML = "Se ha enviado el código. Por favor, ingreselo a continuación";

            event.data.element.appendChild(element);

            //#####


            let options = [];
            options[0] = "Volver a enviar código";
            options[1] = "Intentar por otro medio";

            for (var x = 0; x < options.length; x++) {
                let opt = options[x];

                $("#tmp_option").html(opt);
                let $opt = $("#tmp_option");

                $opt.clone().appendTo(event.data.element).removeAttr("id").addClass("remove").attr("value", opt).click(function () {
                    let f = new Function("DefaultOptionResponseEventHandler(this)");
                    f.call(this);
                });
            }

        },
        error: function (jqXHR, textStatus, error) {
            var err = textStatus + ", " + error;
            console.log(err);
        }
    });
}