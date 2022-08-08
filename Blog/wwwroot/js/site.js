//Automatically increase textarea field during typing the text
function fixTextareaSize(textarea) {
    textarea.style.height = 'auto'
    textarea.style.height = textarea.scrollHeight + 2 + "px"
}

//Resize textarea by content
~function () {
    var tx = document.getElementsByTagName('textarea');

    for (var i = 0; i < tx.length; i++) {
        tx[i].addEventListener("input", function (e) { fixTextareaSize(e.target) });
        fixTextareaSize(tx[i])
    }
}()

//Resize textarea for modal windows
$('.modal').on('shown.bs.modal', function () {
    $(this).find('textarea').each(function () {
        this.addEventListener("input", function (e) { fixTextareaSize(e.target) });
        fixTextareaSize(this)
    });
})

//Move the cursor to the end of the text in modal windows
$('.modal').on('shown.bs.modal', function () {
    var data = $('#edit-textarea').val();
    $('#edit-textarea').focus().val('').val(data);
})

//Edit modal windows
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

//Add like to article
$(function () {
    $(document).on("click", ".like", function (event) { 
        event.preventDefault();
        var id = $(this).val();
        $.ajax({
            type: "POST",
            url: "/Like/AddLike",
            data: { "id": id },
            context: this,
            success: function () {
                $(this).removeClass("like");
                $(this).addClass("unlike");
                $(this).addClass("user-activated");
                incrementCounter();
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
})

//Remove like from article
$(function () {
    $(document).on("click", ".unlike", function (event) {
        event.preventDefault();
        var id = $(this).val();
        $.ajax({
            type: "POST",
            url: "/Like/Delete",
            data: { "id": id },
            context: this,
            success: function () {
                $(this).removeClass("unlike");
                $(this).addClass("like");
                $(this).removeClass("user-activated");
                decrementCounter();
            },
            error: function (error) {
                //if (error.response.status == 401) {
                //    //throw new Error(error);
                //    console.log("Error!");
                //}
                console.log(error);
                //console.log(error);
            }
        });
    });
})

function incrementCounter() {
    document.getElementById('reaction-number-like').textContent++;
}

function decrementCounter() {
    document.getElementById('reaction-number-like').textContent--;
}

//Errors handler
$(document).ajaxError(function (event, request, settings) {
    let status = request.status;
    if (status == 401)
        location.replace("/Account/Login");
});