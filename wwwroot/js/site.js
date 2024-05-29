"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});



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
