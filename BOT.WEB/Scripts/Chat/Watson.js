﻿
var _urlGoogleLogin = '@Url.Action("LoginGoogle", "Account", new { area = string.Empty })';
var _googleCliendId = '@googleAuth.ClientId';
var _urlReturn = '#';
const element = document.querySelector('.chatElement');
var wats;
var userw = {
    Ficha: null,
    EmailPersonal: null,
    Name: null,
    PhoneNumber: null,
    Email: null
};
var firstLoad = true;
var initialIntent = null;
var context = null;

loadGoogleApp();

window.watsonAssistantChatOptions = {
    //integrationID: "c12019ba-78c9-42ec-ae01-f1c5f654a157", // The ID of this integration.
    //region: "us-south", // The region your integration is hosted in.
    //serviceInstanceID: "91882f7b-e585-4560-8b9b-f6f597ac7fbb",

    integrationID: "155b179e-7aa9-44ee-aca2-a1b6d1b9599a", // The ID of this integration.
    region: "us-south", // The region your integration is hosted in.
    serviceInstanceID: "ac9d0cbc-929d-4585-a2f6-fad717a31eff", // The ID of your service instance.

    element: element,
    hideCloseButton: true,
    showLauncher: false,
    openChatByDefault: true,
    carbonTheme: 'g10',
    onLoad: function (instance) {
        wats = instance;
        $(".cssload-container").hide("slow");
        $(".onload").removeClass("hidden");

        instance.on({ type: 'pre:receive', handler: TEMPORARY_convertToUserDefinedResponse });
        //instance.on({ type: 'pre:send', handler: PreSendHandler });
        instance.on({ type: 'customResponse', handler: customResponseHandler });

        instance.render();
    }
};

setTimeout(function () {
    const t = document.createElement('script');
    t.src = 'https://web-chat.global.assistant.watson.appdomain.cloud/loadWatsonAssistantChat.js';
    document.head.appendChild(t);
});

function TEMPORARY_convertToUserDefinedResponse(event) {
    if (event.data.output.intents.length > 0) {
        let intent = event.data.output.intents[0].intent;
        if (intent === "Identificame") {
            initialIntent = intent;
        }
        else if (intent === "ResetearContraseñaDelEquipo") {
            initialIntent = intent;
        }

    }
    event.data.output.generic = event.data.output.generic.map(function (item) {

        let entidades = event.data.output.entities;
        let fichas = entidades.map(function (task) { return task.value });

        if (fichas[0] == "Ficha") {
            userw.Ficha = fichas[1];
        }

        if (item.response_type === 'option' && (
            item.description === "#ship_list#"
            || item.description === "#evaluate#"
            || item.description === "#ask_email#"
            || item.description === "#identify#"
            || item.description === "#sms_sended#"
        )) {
            item.response_type = 'user_defined';
            item.user_defined = {
                "template_name": 'option', //item.text
                "handler": item.description.substring(1, item.description.length - 1) + "HandlerEvent"
            }
        }
        //Identificar que es un KeyCode
        else if (item.response_type === 'text' && (
            item.text === '#user_link_printer#'
            || item.text === '#quiz_evaluate#'
            || item.text === '#contact_support#'
            || item.text === '#login_google#'
            || item.text === '#get_personal_email#'
            || item.text === '#send_code_personal_email#'
            || item.text === '#welcome#'
            || item.text === '#sms#'
            || item.text === '#send_sms#'
        )) {
            item.response_type = 'user_defined';
            item.user_defined = {
                "template_name": item.text
            }
        }

        else if (item.response_type === 'text') {
            item.response_type = 'user_defined';
            item.user_defined = {
                "template_name": "text"
            }
        }

        return item;
    });
}

//function PreSendHandler(event) {
//    console.log(event);

//}

//Se agrega el keycode y se redirecciona al handler correspondiente
function customResponseHandler(event) {
    const { message } = event.data;

    //console.log(message);

    switch (message.user_defined.template_name) {
        case 'text':
            handleText(event);
            break;
        case 'option':
            handleOption(event);
            break;
        case '#user_link_printer#':
            user_link_printerHandler(event);
            break;
        case '#quiz_evaluate#':
            quiz_evaluateHandler(event);
            break;
        case '#contact_support#':
            contact_supportHandler(event);
            break;
        case '#login_google#':
            login_googleHandler(event);
            break;
        case '#get_personal_email#':
            get_personal_EmailHandler(event);
            break;
        case '#send_code_personal_email#':
            send_mailHandler(event);
            break;
        case '#welcome#':
            welcomeHandler(event);
            break;
        case '#sms#':
            smsHandler(event);
            break;
        case '#send_sms#':
            send_smsHandler(event);
            break;
            sms_sended
        default:
            handleText(event);
    }
}



function handleText(event) {
    const element = document.createElement('div');
    element.setAttribute('class', 'custom_received');
    element.innerHTML = event.data.message.text;

    event.data.element.appendChild(element);
}

function handleOption(event) {
    const element = document.createElement('div');
    element.setAttribute('class', 'custom_received');
    element.innerHTML = event.data.message.title;
    event.data.element.appendChild(element);

    for (var x = 0; x < event.data.message.options.length; x++) {
        let opt = event.data.message.options[x];

        $("#tmp_option").html(opt.label);
        let $opt = $("#tmp_option");

        $opt.clone().appendTo(event.data.element).removeAttr("id").addClass("remove").attr("value", opt.value.input.text).click(function () {
            let f = new Function(event.data.message.user_defined.handler + "(this)");
            f.call(this);
        });
    }
}

function identifyHandlerEvent(input) {
    $(".remove").remove();
    SendMessage(input.value);
    //get_personal_EmailHandler();
}

function ship_listHandlerEvent(input) {
    DefaultOptionResponseEventHandler(input);
}

function evaluateHandlerEvent(input) {
    DefaultOptionResponseEventHandler(input);
}

function quiz_evaluateHandler(event) {
    const element = document.createElement('div');
    element.setAttribute('class', 'custom_received');
    element.innerHTML = $("#quiz_evaluate").html();
    event.data.element.appendChild(element);
}

function quiz_evaluateEventHandler(eval) {
    SendMessage(eval.toString(), true);
    for (var x = 1; x <= eval; x++) {
        $(".btn" + x).addClass("btn-grey-selected");
    }
    $(".btn-rating").addClass("disabled");

}

function login_googleHandler(event) {
    auth2.signIn();
}

var userChanged = function (user) {
    if (firstLoad) {
        firstLoad = false;
        return;
    }

    userw.Name = user.getBasicProfile().getGivenName();
    userw.Email = user.getBasicProfile().getEmail();

    SendMessage("authorized ", true);
};

