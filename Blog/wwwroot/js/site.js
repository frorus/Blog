// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function fixTextareaSize(textarea) {
    textarea.style.height = 'auto'
    textarea.style.height = textarea.scrollHeight + 2 + "px"
}

~function () {
    var textarea = document.querySelector('textarea')
    textarea.addEventListener('input', function (e) { fixTextareaSize(e.target) })
    fixTextareaSize(textarea)
}()
