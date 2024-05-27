document.addEventListener('DOMContentLoaded', function () {
    var itemModal = document.getElementById('deleteModal');
    var itemForm = document.getElementById('itemForm');
    var modalTitle = document.querySelector('.modal-title');
    var itemIdInput = document.getElementById('itemId');
    var itemNameInput = document.getElementById('itemName');

    itemModal.addEventListener('show.bs.modal', function (event) {
        var button = event.relatedTarget;
        var id = button.getAttribute('data-id');
        var name = button.getAttribute('data-name');
        var actionUrl = button.getAttribute('data-action-url');

        modalTitle.textContent = 'Sei sicuro di voler cancellare ' + name + '?';
        itemIdInput.value = id;
        itemNameInput.value = name;
        itemForm.action = actionUrl;
    });
});
