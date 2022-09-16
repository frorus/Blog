//Automatically increase textarea field during typing the text
function fixTextareaSize(textarea) {
    textarea.style.height = 'auto'
    textarea.style.height = textarea.scrollHeight + 2 + "px"
}

//Resize textarea by content
~function () {
    let tx = document.getElementsByTagName('textarea');

    for (let i = 0; i < tx.length; i++) {
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
    let data = $('#edit-textarea').val();
    $('#edit-textarea').focus().val('').val(data);
})

//Edit modal windows
$(function () {
    let Elem = $('#CommentModalWindow');
    $('button[data-toggle="myModal"]').click(function (event) {

        let url = $(this).data('url');
        let decoderUrl = decodeURIComponent(url);
        $.get(decoderUrl).done(function (data) {
            Elem.html(data);
            Elem.find('.modal').modal('show');
        })
    })

    Elem.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();
        let form = $(this).parents('.modal').find('form');
        let actionUrl = form.attr('action');
        let sendData = form.serialize();
        $.post(actionUrl, sendData).done(function (data) {
            Elem.find('.modal').modal('hide');
        })
    })
})

//Add like to article
$(function () {
    $(document).on("click", ".like", function (event) { 
        event.preventDefault();
        let id = $(this).val();
        $.ajax({
            type: "POST",
            url: "/Like/AddLikeToArticle",
            data: { "id": id },
            context: this,
            success: function () {
                $(this).removeClass("like");
                $(this).addClass("unlike");
                $(this).addClass("user-activated");
                $(this).children("span.blog-reaction__count")[0].textContent++;
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
        let id = $(this).val();
        $.ajax({
            type: "POST",
            url: "/Like/DeleteLikeFromArticle",
            data: { "id": id },
            context: this,
            success: function () {
                $(this).removeClass("unlike");
                $(this).addClass("like");
                $(this).removeClass("user-activated");
                $(this).children("span.blog-reaction__count")[0].textContent--;
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
})

//Add like to comment
$(function () {
    $(document).on("click", ".like-comment", function (event) {
        event.preventDefault();
        let id = $(this).val();
        $.ajax({
            type: "POST",
            url: "/Like/AddLikeToComment",
            data: { "id": id },
            context: this,
            success: function () {
                $(this).removeClass("like-comment");
                $(this).addClass("unlike-comment");
                $(this).addClass("reacted");
                $(this).children("span.reactions-count")[0].textContent++;
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
})

//Remove like from comment
$(function () {
    $(document).on("click", ".unlike-comment", function (event) {
        event.preventDefault();
        let id = $(this).val();
        $.ajax({
            type: "POST",
            url: "/Like/DeleteLikeFromComment",
            data: { "id": id },
            context: this,
            success: function () {
                $(this).removeClass("unlike-comment");
                $(this).addClass("like-comment");
                $(this).removeClass("reacted");
                $(this).children("span.reactions-count")[0].textContent--;
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
})

//Add article to favourite
$(function () {
    $(document).on("click", ".favourite", function (event) {
        event.preventDefault();
        let id = $(this).val();
        $.ajax({
            type: "POST",
            url: "/Favourite/Add",
            data: { "id": id },
            context: this,
            success: function () {
                $(this).removeClass("favourite");
                $(this).addClass("unfavourite");
                $(this).addClass("user-activated");
                $(this).children("span.blog-reaction__count")[0].textContent++;
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
})

//Remove article from favourite
$(function () {
    $(document).on("click", ".unfavourite", function (event) {
        event.preventDefault();
        let id = $(this).val();
        $.ajax({
            type: "POST",
            url: "/Favourite/Delete",
            data: { "id": id },
            context: this,
            success: function () {
                $(this).removeClass("unfavourite");
                $(this).addClass("favourite");
                $(this).removeClass("user-activated");
                $(this).children("span.blog-reaction__count")[0].textContent--;
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
})

$(document).ready(function () {
    favArticlesCounter();
});

//Add/remove article to/from favourite from index page
$(function () {
    $(document).on("click", ".reading-list", function (event) {
        event.preventDefault();
        let id = $(this).val();
        let params = {
            type: "POST",
            data: { "id": id },
            context: this,
            error: function (error) {
                console.log(error);
            }
        };
        if ($(this).attr("aria-pressed") == "false") {
            params.url = "/Favourite/Add";
            params.success = function () {
                $(this).attr("aria-pressed", "true");
                $(this).find("path").attr("d", "M6.75 4.5h10.5a.75.75 0 0 1 .75.75v14.357a.375.375 0 0 1-.575.318L12 16.523l-5.426 3.401A.375.375 0 0 1 6 19.607V5.25a.75.75 0 0 1 .75-.75z");
                favArticlesCounter();
            }
        } else {
            params.url = "/Favourite/Delete";
            params.success = function () {
                $(this).attr("aria-pressed", "false");
                $(this).find("path").attr("d", "M6.75 4.5h10.5a.75.75 0 0 1 .75.75v14.357a.375.375 0 0 1-.575.318L12 16.523l-5.426 3.401A.375.375 0 0 1 6 19.607V5.25a.75.75 0 0 1 .75-.75zM16.5 6h-9v11.574l4.5-2.82 4.5 2.82V6z");
                favArticlesCounter();
            }
        }
        $.ajax(params);
    });
})

//Get count of favourite articles
function favArticlesCounter() {
    let element = $(document).find("span.c-indicator");
    if (element.length != 0) {
        let id = $(document).find("span.c-indicator").data("id");
        $.ajax({
            type: "GET",
            url: "/User/GetUserReadingListCount",
            data: { "id": id },
            success: function (data) {
                element[0].textContent = data
                if (element[0].textContent != 0) {
                    element.removeClass("hidden");
                }
                else {
                    element.addClass("hidden");
                }
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
}


//Errors handler
$(document).ajaxError(function (event, request, settings) {
    let status = request.status;
    if (status == 401)
        location.replace("/Login/Login");
});

//Characters counter for Settings - Profile page
function countChar(element) {
    let countOfChars = element.value.length;
    switch (element.id) {
        case 'Website':
            $('#url-characters').text(countOfChars);
            break;
        case 'Location':
            $('#location-characters').text(countOfChars);
            break;
        case 'Bio':
            $('#summary-characters').text(countOfChars);
            break;
        case 'Learning':
            $('#currently_learning-characters').text(countOfChars);
            break;
        case 'Skills':
            $('#skills_languages-characters').text(countOfChars);
            break;
        case 'Work':
            $('#work-characters').text(countOfChars);
            break;
        case 'Education':
            $('#education-characters').text(countOfChars);
            break;
    }
};

//Filter reading list by tag
$(function () {
    $(document).on("click", ".tag", function (event) {
        event.preventDefault();
        let userId = $(this).data("id");
        let tag = $(this).data("tag");
        let element = $(document).find("section");
        let prevCurrent = $(document).find("a.blog-link--current");
        $.ajax({
            type: "GET",
            url: "/User/GetUserReadingListByTag",
            data: { "id": userId, "tag": tag },
            context: this,
            success: function (data) {
                prevCurrent.removeClass("blog-link--current");
                $(this).addClass("blog-link--current");
                element.replaceWith(data);
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
})