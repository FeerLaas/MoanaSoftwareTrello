// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    var PlaceHolderElement = $('#modalLocation');
    $('a[data-toggle="ajax-modal"]').click(function (e) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show');
        });
    });
    PlaceHolderElement.on('click', '[data-save="modal"]', function (e) {
        var form = $(this).parents('.modal').find('form');
        
        if (form[0].checkValidity()) {
            var actionUrl = form.attr('action');
            var sendData = form.serialize();
            $.post(actionUrl, sendData).done(function (d) {
                PlaceHolderElement.find('.modal').modal('hide');
            })
        }
        else {
            console.log("hiba");
        }
    })
    PlaceHolderElement.on('click', '[data-dismiss="modal"]', function (e) {
            PlaceHolderElement.find('.modal').modal('hide');
    })
})