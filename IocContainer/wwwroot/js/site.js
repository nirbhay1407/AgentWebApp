function returnType(type) {
    var returnvalue;
    switch (type) {
        case type == "Primary":
            returnvalue = "bg-secondary";
            break;
        case type == "Success":
            returnvalue = "bg-success";
            break;
        default:
            returnvalue = "bg-info";
    }
    return returnvalue;
}

/* <option value="bg-primary" selected>Primary</option>
                        <option value="bg-secondary">Secondary</option>
                        <option value="bg-success">Success</option>
                        <option value="bg-danger">Danger</option>
                        <option value="bg-warning">Warning</option>
                        <option value="bg-info">Info</option>
                        <option value="bg-dark">Dark</option>*/


function ToDesign(type, message) {
    const toastPlacementExample = document.querySelector('.toast-placement-ex');
    let toastPlacement;
    var selectedType = returnType(type);
    if (toastPlacement) {
        toastDispose(toastPlacement);
    }

    toastPlacementExample.classList.add(selectedType);
    DOMTokenList.prototype.add.apply(toastPlacementExample.classList, ["top-0", "end-0"]);
    toastPlacement = new bootstrap.Toast(toastPlacementExample);
    $('.toast-body').text(message);
    toastPlacement.show();
}