//Press Enter in INPUT moves cursor to next INPUT
$('form').keydown(function (e) {
    if (e.which == 13) // Enter key = keycode 13
    {
        e.preventDefault();
    }
});