$(document).ready(function () {



});

var debugMessage = document.getElementById('debugMessage');

var methodDropdown = document.getElementById('method-menu');
var requestMethod = document.getElementById('Method');

var responseHeadersTab = document.getElementById('responseHeaders-tab');
var responseBodyTab = document.getElementById('responseBody-tab');
var responseCookiesTab = document.getElementById('responseCookies-tab');

var responseView = document.getElementById('ResponseView');

function Start() {
    if (responseView.value == "Headers") responseHeadersTab.click();
    else if (responseView.value == "Body") responseBodyTab.click();
    else if (responseView.value == "Cookies") responseCookiesTab.click();
}

function SetMethod(name, value) {
    methodDropdown.textContent = name.textContent;
    requestMethod.value = value;
}

function SetResponseView(value) {
    responseView.value = value;
}

document.addEventListener("DOMContentLoaded", Start);
