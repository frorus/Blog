// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function fixTextareaSize(textarea) {
    textarea.style.height = 'auto'
    textarea.style.height = textarea.scrollHeight + 2 + "px"
}

~function () {
    /*---var textarea = document.querySelector('textarea')*/
    //---textarea.addEventListener('input', function (e) { fixTextareaSize(e.target) })
    //---fixTextareaSize(textarea)

    var tx = document.getElementsByTagName('textarea');

    for (var i = 0; i < tx.length; i++) {

        /*---tx[i].setAttribute('style', 'height:' + (tx[i].scrollHeight) + 'px;overflow-y:hidden;');*/

        tx[i].addEventListener("input", function (e) { fixTextareaSize(e.target) });
        fixTextareaSize(tx[i])

    }
}()

//let setHeight = (input) => {

//    input.style.overflow = 'hidden';
//    input.style.height = 0;

//    input.style.height = `${input.scrollHeight + 2}px`;
//};

$('.modal').on('shown.bs.modal', function () {
    $(this).find('textarea').each(function () {
        this.addEventListener("input", function (e) { fixTextareaSize(e.target) });
        fixTextareaSize(this)
    });
})

$('.modal').on('shown.bs.modal', function () {
    var data = $('#edit-textarea').val();
    $('#edit-textarea').focus().val('').val(data);
})

$(function () {
    var Elem = $('#EditCommentModalWindow');
    $('button[data-toggle="myModal"]').click(function (event) {

        var url = $(this).data('url');
        var decoderUrl = decodeURIComponent(url);
        $.get(decoderUrl).done(function (data) {
            Elem.html(data);
            Elem.find('.modal').modal('show');
        })
    })

    Elem.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var sendData = form.serialize();
        $.post(actionUrl, sendData).done(function (data) {
            Elem.find('.modal').modal('hide');
        })
    })
})