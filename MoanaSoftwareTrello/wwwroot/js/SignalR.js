"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/signalr").build();

connection.on("ReceiveCardPos", function (id, from, to) {
    console.log(`${id} moved from ${from} to ${to}`);
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

        connection.invoke("UpdateCardPos", movingObj["obj"], movingObj["from"], movingObj["to"]).catch(function (err) {
            return console.error(err.toString());
        });
    });
});