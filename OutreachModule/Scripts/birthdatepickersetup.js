    $('.datepicker').datepicker({
        format: 'd MM yyyy',
        startView: "decade",
        startDate: "-200y",
        endDate: "0d"
    })
    .on('hide', function (e) {
        if (e.date != null) {
            $('#agefield').val(_calculateAge(e.date));
        }
    });
    function _calculateAge(birthday) { // birthday is a date
        var ageDifMs = Date.now() - birthday.getTime();
        var ageDate = new Date(ageDifMs); // miliseconds from epoch
        return Math.abs(ageDate.getUTCFullYear() - 1970);
    }
    $('#agefield').keyup(function () {
        $('.datepicker').val("");
    });

    $('#agefield').bind('keyup mouseup', function () {
        $('.datepicker').val("");
    });

    $('.datepicker').keypress(function (event) {
        console.log("hi");
        console.log(event.keyCode);
        if (event.keyCode == 13) {
            
            $(this).val = $(this).val;
        }
    });