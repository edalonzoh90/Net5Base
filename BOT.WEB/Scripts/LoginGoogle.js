var signinChanged = function (val) {
    //console.log('Signin state changed to ', val);
    //document.getElementById('signed-in-cell').innerText = val;
};

//var userChanged = function (user) {
//    console.log('User now: ', user);
//    googleUser = user;
//    console.log(googleUser);
//};


var loadGoogleApp = function () {
    gapi.load('auth2', function () {
        // Retrieve the singleton for the GoogleAuth library and set up the client.
        auth2 = gapi.auth2.init({
            client_id: _googleCliendId,
            cookiepolicy: 'single_host_origin',
            // Request scopes in addition to 'profile' and 'email'
            //scope: 'additional_scope'
        });
        //attachGoogleSignin(document.getElementById('Google'));
        auth2.isSignedIn.listen(signinChanged);
        auth2.currentUser.listen(userChanged);

    });
};

function attachGoogleSignin(element) {
    console.log("attachGoogleSignin");
    auth2.attachClickHandler(element, {},
        function (googleUser) {
            logInGoogle(googleUser);
        }, function (error) {
            alert(JSON.stringify(error, undefined, 2));
        });
};


function logInGoogle(googleUser) {
    console.log("logInGoogle");

    //var getId = googleUser.getBasicProfile().getId;
    var getName = googleUser.getBasicProfile().getName();
    var getGivenName = googleUser.getBasicProfile().getGivenName();
    var getFamilyName = googleUser.getBasicProfile().getFamilyName();
    var getImageUrl = googleUser.getBasicProfile().getImageUrl();
    var getEmail = googleUser.getBasicProfile().getEmail();
    var idToken = googleUser.getAuthResponse().id_token;

    var LoginUserGoogle = {
        Name: getName,
        GivenName: getGivenName,
        FamilyName: getFamilyName,
        ImageUrl: getImageUrl,
        Email: getEmail,
        Token: idToken
    };

    $.ajax({
        type: 'POST',
        cache: false,
        async: true,
        url: _urlGoogleLogin,
        data: {
            googleUser: LoginUserGoogle,
            returnUrl: _urlReturn
        },
        beforeSend: function () {
            //App.blockUI({
            //    target: "main",
            //    animate: !0
            //});
        },
        success: function (data) {
            console.log("success");
            //App.blockUI({
            //    target: "main",
            //    animate: !0
            //});
            //try {
            //    //Se recibe la información de la respuesta de la peticion.
            //    if (data) {
            //        switch (data.Mensaje.Estado) //Revisar clase UI.Message , esta definido un enum de los estados de los mensajes.
            //        {
            //            case 0://exitoso
            //                if (data.Url) {
            //                    window.location.href = data.Url;
            //                    //toastr.options.onHidden = function () { window.location.href = data.Url; }
            //                    //toastr["success"]("Redireccionando...");
            //                }
            //                break;
            //            case 1: case 2: case 3: //error
            //                $(".alert-danger span", $(".login-form")).html(data.Mensaje.Contenido);
            //                $(".alert-danger", $(".login-form")).show();
            //                $('.form-md-floating-label').addClass('has-error');
            //                break;
            //        }
            //    }
            //}
            //catch (err) {
            //    $(".alert-danger span", $(".login-form")).html(err);
            //    $(".alert-danger", $(".login-form")).show();
            //}
        },
        //error: function (xhr, ajaxOptions, thrownError) {
        //    App.unblockUI($("main"));

        //},
    });
}
