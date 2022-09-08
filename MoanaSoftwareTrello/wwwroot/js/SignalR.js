"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/signalr").build();

function getKeyByValue(object, value) {
    return Object.keys(object).find(key => object[key] === value);
}

function getRowNumber(id) {
    var child = document.getElementById(id);
    var parent = child.parentNode;
    // The equivalent of parent.children.indexOf(child)
    return Array.prototype.indexOf.call(parent.children, child);
}
function updateRow(source) {
    //update from row
    var cards = Array.from(document.getElementById(source["from"]).children);
    cards.forEach(function (card) { updateCard(card.id, getKeyByValue(columns, source["from"]), getRowNumber(card.id)) });
    if (source["from"] === source["to"]) return;
    cards = Array.from(document.getElementById(source["to"]).children);
    cards.forEach(function (card) { updateCard(card.id, getKeyByValue(columns, source["to"]), getRowNumber(card.id)) });
}
async function updateCard(id, status, position) {
    $.post(updateCardPosUrl + "?pos=true", {
        "id": id,
        "status": status,
        "position": position
    });
}






connection.on("ReceiveCardPos", function (id, from, to) {
    console.log(`${id} moved from ${from} to ${to} and update columns`);
    console.log($(`#${from}`));
    $(`#${from}`).load(loadContainerUrl + "?column=" + from);
    if (from === to) return;
    $(`#${to}`).load(loadContainerUrl + "?column=" + to);
});


connection.start().then(function () {
    console.log("start");
}).catch(function (err) {
    return console.error(err.toString());
});

$(document).ready(function () {

    var movingObj = {};
    dragula(dragulaColumns).on('drag', function (el, source) {

        movingObj = {};
        movingObj["obj"] = el.id;
        movingObj["from"] = source.id;

    }).on('drop', function (el, target) {
        movingObj["to"] = target.id;
        updateRow(movingObj);
        console.log("updated data");
        connection.invoke("UpdateCardPos", movingObj["obj"], movingObj["from"], movingObj["to"]).catch(function (err) {
            return console.error(err.toString());
        });

    });
});