

$("#showUT").click(function () {
    var model = setModel();
        $.ajax({
            type: 'POST',
            url: "/UnderThresold/GetUnderThresoldReportModel",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ model: model }),
            dataType: "html",
            success: function (data) {
                    $('#results').html(data);
            }
        });
    });

function setModel() {
        var model = {
            ReportType: $('#ReportType').val(),
            Month: $('#Month').val(),
            Year: $('#Year').val(),
            Week: $('#Week').val(),
            Day: $('#Day').val(),
            Hour: $('#Hour').val(),
            SelectedPumpStationId: $('#SelectedPumpStationId').val(),
            TransmeType: $('#TransmeType').val()
        }

        return model;
    }

$(function () {
        $('#inputPanel').hide();
    });

$("#Month").change(function() {
        var monthValue = $('#Month').val();
        if (monthValue == 4 || monthValue == 6 || monthValue == 9 || monthValue == 11) {
            $("#Day option[value='31']").hide();
        } else if (monthValue == 2) {
            $("#Day option[value='30']").hide();
            $("#Day option[value='31']").hide();

            if ($('#Year').val() > 0) {
                if (!leapYear($('#Year').val()))
                    $("#Day option[value='29']").hide();
                else {
                    $("#Day option[value='29']").show();
                }
            }
        } else {
            $("#Day option[value='29']").show();
            $("#Day option[value='30']").show();
            $("#Day option[value='31']").show();
        }
    });

function leapYear(year) {
        return new Date(year, 1, 29).getMonth() == 1;
    }

$("#ReportType").change(function () {
        var reportType = $('#ReportType').val();
        $('#inputPanel').show();
        if (reportType == 1) {
            $('#Week').hide();
            $('#Year').show();
            $('#Month').show();
            $('#Day').show();
            $('#Hour').show();
            $("#TransmeType option[value='1']").show();
            $("#TransmeType option[value='2']").show();
            $("#TransmeType option[value='5']").show();
        }
        if (reportType == 2) {
            $('#Week').hide();
            $('#Hour').hide();
            $('#Year').show();
            $('#Month').show();
            $('#Day').show();
        }
        if (reportType == 3) {
            $('#Hour').hide();
            $('#Month').hide();
            $('#Day').hide();
            $('#Year').show();
            $('#Week').show();
        }
        if (reportType == 4) {
            $('#Week').hide();
            $('#Day').hide();
            $('#Hour').hide();
            $('#Year').show();
            $('#Month').show();
            $('#Day').hide();
        }
        $('#exp').show();
        if (reportType == 5) {
            $('#Month').hide();
            $('#Day').hide();
            $('#Hour').hide();
            $('#Year').show();
            $('#Month').show();
            $('#Day').hide();
            //$('#inputPanel').hide();
            //$("#TransmeType option[value='1']").show();
            //$("#TransmeType option[value='2']").show();
            //$("#TransmeType option[value='5']").show();
            $('#exp').hide();
        }

        if (reportType != 5 && reportType != 1) {
            //$("#TransmeType option[value='1']").hide();
            //$("#TransmeType option[value='2']").hide();
            //$("#TransmeType option[value='5']").hide();
        }
});

$(function () {
    $("form").validate();
});