function isNullOrEmpty(str) {
    return (!str || str.length === 0);
}

function doBlank(id) {
    $('#' + id).text("");
}

function resetFields(arrayOfId) {
    $.each(arrayOfId, function (index, value) {
        console.log(value);
        doBlank(value);
    });
}

// Function to validate a number
function isValidNumber(input) {
    return !isNaN(input) && typeof input === 'number';
}

// Function to validate a varchar (string)
function isValidVarchar(input, maxLength) {
    return typeof input === 'string' && input.length <= maxLength;
}

// Function to validate an email
function isValidEmail(input) {
    const emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    return emailPattern.test(input);
}

// Function to validate a mobile number (assuming international format)
function isValidMobileNumber(input) {
    const mobilePattern = /^\+?[1-9]\d{1,14}$/; // E.164 format
    return mobilePattern.test(input);
}

// Function to validate only characters and numbers (no special characters)
function isValidCharAndNumber(input) {
    const charAndNumberPattern = /^[a-zA-Z0-9]+$/;
    return charAndNumberPattern.test(input);
}

// Function to validate a username
function isValidUsername(input) {
    const usernamePattern = /^[a-zA-Z0-9_-]{3,20}$/;
    return usernamePattern.test(input);
}

//generic ajax function

function genericAjax(url, method, data, successCallback, errorCallback) {
    $.ajax({
        url: url,
        method: method,
        data: data,
        success: function (response) {
            if (typeof successCallback === 'function') {
                successCallback(response);
            }
        },
        error: function (xhr, status, error) {
            if (typeof errorCallback === 'function') {
                errorCallback(xhr, status, error);
            }
        }
    });
}


//modalLoader
var showLoader = function () {
//    var button = $(".button");
    //    button.button(button.button("option", "disabled") ? "enable" : "disable");
    $('#modalLoader').modal('show');
}

var hideLoader = function () {
    $('#modalLoader').modal('hide');
}


/*
// Export functions for use in other scripts
export { isValidNumber, isValidVarchar, isValidEmail, isValidMobileNumber, isValidCharAndNumber };
export { isNullOrEmpty, doBlank, resetFields };*/