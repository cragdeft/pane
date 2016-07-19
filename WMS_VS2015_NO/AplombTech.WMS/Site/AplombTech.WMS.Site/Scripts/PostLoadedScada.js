var graphInterval;
var unit = parseInt($('#ltvalue').text());
var height = parseInt($('#waterlevel').css('height'));
var margin = parseInt($('#waterlevel').css('margin-top'));
$("#waterlevel").css("background-color", "#2fcff4");
$("#waterlevel").css("height", height - (100 - (1.35 * unit)) + "px");
$("#waterlevel").css("margin-top", margin + (100 - (1.35 * unit)) + "px");


$('#motorswitch').on('click', function () {
    if ($('#pmotorAuto').text() == "Manual") {
        return;
    }
    if ($('#motorswitchStatus').text() == "Processing...") {
        return;
    }
    var state;
    if ($('#motorswitch').text() == 'ON') {
        state = "OFF";
    } else
        state = "ON";
    $('#motorswitchStatus').text('Processing...');
    $('#pmotorCommandSent').text(new Date().toLocaleString());

    $.ajax({
        type: "POST",
        url: $("#publishCommandUrl").val(),
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ state: state, pumpStationId: $('#SelectedPumpStationId').val() }),
        dataType: "json",
        success:
            function (data) {
                if (data.IsSuccess) {
                    if (data.Data == 'Success') {

                    } else {
                    }

                } else {

                }
            },
        error: function (e) {
        }
    });

});
$('span').on('click', function () {

    var sensorId = $.trim(this.id.split("_")[1]);
    if (graphInterval != null)
        clearInterval(graphInterval);
    $.ajax({
        type: "POST",
        url: $("#getScadaSensorDataUrl").val(),
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ sensorId: sensorId }),
        dataType: "json",
        success:
            function (data) {
                if (data.IsSuccess) {

                    showRealChartScada(data, sensorId);

                }

            },
        error: function () { }
    });

});