window.showBootstrapModal = (elementId) => {
    var modal = new bootstrap.Modal(document.getElementById(elementId));
    modal.show();
};

window.hideBootstrapModal = (elementId) => {
    var modal = bootstrap.Modal.getInstance(document.getElementById(elementId));
    modal.hide();
};
