"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/signalr").build();

// -----------------------------------------------------------
// Needed Function for beautiful code
// -----------------------------------------------------------

function getKeyByValue(object, value) {
    return Object.keys(object).find(key => object[key] === value);
}

function getRowNumber(id) {

    
    var child = document.getElementById(id);
    var parent = child.parentNode;
    // The equivalent of parent.children.indexOf(child)
    return Array.prototype.indexOf.call(parent.children, child);
}
function getColumnNumber(id) {
    return document.getElementById(id).parentNode.id;
    
}
function updateRow(source) {
    //update from row
    var cards = Array.from(document.getElementById(source["from"]).children);
    console.log(cards);
    cards.forEach(function (card) {if (card.id) updateCard(card.id, getKeyByValue(columns, source["from"]), getRowNumber(card.id)) });
    if (source["from"] === source["to"]) return;
    cards = Array.from(document.getElementById(source["to"]).children);
    cards.forEach(function (card) { if (card.id) updateCard(card.id, getKeyByValue(columns, source["to"]), getRowNumber(card.id)) });
}
async function updateCard(id, status, position) {
    $.post(updateCardPosUrl + "?pos=true", {
        "id": id,
        "status": status,
        "position": position
    });
}

// -----------------------------------------------------------
// SignalR inic / open / lisen + Dragula inic
// -----------------------------------------------------------


// ----------  SignalR Lisen ----------

connection.on("ReceiveCardPos", function (id, from, to) {
    console.log(`${id} moved from ${from} to ${to} and update columns`);
    var attr = $(`#${from}`)[0]["attributes"][0].name;
    $(`#${from}`).load(loadContainerUrl + "?column=" + from + "&attr="+attr);
    
    if (from === to) return;

    $(`#${to}`).load(loadContainerUrl + "?column=" + to + "&attr=" + attr);
});

// ----------  SignalR Open ----------

connection.start().then(function () {
    console.log("start");
}).catch(function (err) {
    return console.error(err.toString());
});
// ----------  SignalR +Dragula Inic ----------

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
// -----------------------------------------------------------
// Modal Open / Close / SendData  + SignalR communication
// -----------------------------------------------------------
$(document).ready(eventLoadToModal(""));
function eventLoadToModal(obj) {
    // -----------  Modal Open  -----------
    
    var PlaceHolderElement = $('#modalLocation');
    console.log($(obj + ' a[data-toggle="ajax-modal"]'));
    $(obj + ' a[data-toggle="ajax-modal"]').click(function (e) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show');
        });
    });
    if (obj !== "") return;
    // ----------  Card SaveData ----------
    PlaceHolderElement.on('click', '[data-save="modal"]', function (e) {
        var form = $(this).parents('.modal').find('form');

        if (form[0].checkValidity()) {
            var actionUrl = form.attr('action');
            var sendData = form.serialize();
            $.post(actionUrl, sendData).done(function (d) {
                PlaceHolderElement.find('.modal').modal('hide');
                if (form.attr('id') === "modifyCard") {
                    var row = getColumnNumber($("#modifyCard #Id").val());
                }
                else {
                    var row = columns[0];
                }
                connection.invoke("UpdateCardPos", "0", row, row).catch(function (err) {
                    return console.error(err.toString());
                });
            })
        }
    })
    // ----------  Card Destroy ----------
    PlaceHolderElement.on('click', '[data-destroy="modal"]', function (e) {
        var form = $(this).parents('.modal').find('form');

        if (form[0].checkValidity()) {
            
            if (form.attr('id') === "modifyCard") {
                var row = getColumnNumber($("#modifyCard #Id").val());
            }
            else {
                var row = columns[0];
            }
            $.ajax({
                url: destroyCardUrl + "?id=" + $("#modifyCard #Id").val(),
                type: 'DELETE',
                success: function (result) {
                    PlaceHolderElement.find('.modal').modal('hide');
                    connection.invoke("UpdateCardPos", "0", row, row).catch(function (err) {
                        return console.error(err.toString());
                    });
                }
            });
            return;
            $.delete(destroyCardUrl, $("#modifyCard #Id").val()).done(function (d) {
                PlaceHolderElement.find('.modal').modal('hide');

                connection.invoke("UpdateCardPos", "0", row, row).catch(function (err) {
                    return console.error(err.toString());
                });
            })
        }
    })
    // ----------  Card Close  -----------
    PlaceHolderElement.on('click', '[data-dismiss="modal"]', function (e) {
        PlaceHolderElement.find('.modal').modal('hide');
    })
}
// -----------------------------------------------------------
// -----------------------------------------------------------
// -----------------------------------------------------------