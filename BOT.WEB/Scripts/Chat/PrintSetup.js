function user_link_printerHandler(event) {
    const element = document.createElement('div');
    element.setAttribute('class', 'custom_received');
    element.innerHTML = $("#user_link_printer").html();
    event.data.element.appendChild(element);
}
