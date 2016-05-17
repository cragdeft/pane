

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