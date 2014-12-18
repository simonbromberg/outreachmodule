$(document).ready(function ($) {
    $('.add-other').on('click', function () {
        var self = $(this);
        $.get("./Examination/AddOther?name=" + self.attr('id')).done(function (html) {
            var list = self.next(".other-list");
            list.append(html);
            $(".remove-other").on("click", function () {
                var par = $(this).parent().parent();
                par.remove();
            });
        });
    });
});