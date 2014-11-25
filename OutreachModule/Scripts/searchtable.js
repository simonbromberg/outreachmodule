$('#searchBox').bind('input', function () {
    if ($('#searchBox').val() == "") {
        $('#searchButton').click();
    }
});

var field = $('#searchBox');
field.focus();
var strLength = field.val().length * 2;
field[0].setSelectionRange(strLength, strLength);