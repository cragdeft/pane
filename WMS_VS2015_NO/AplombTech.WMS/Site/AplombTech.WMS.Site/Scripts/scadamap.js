function LoadDma(zoneId) {
    if (interval != null)
        clearInterval(interval);
    if (!zoneId) return;//zoneId = 0;
    var dmaDownList = $('#SelectedDmaId');
    $.ajax({
        type: "POST",
        url: $("#getDmaUrl").val(),
        data: JSON.stringify({ zoneId: zoneId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var res = JSON.parse(data.Data);
            dmaDownList.empty();
            dmaDownList.append($('<option/>', {
                value: 0,
                text: "Select Dma"
            }));
            for (var key in res) {
                if (res.hasOwnProperty(key)) {

                    dmaDownList.append($('<option/>', {
                        value: key,
                        text: res[key]
                    }));
                }
            }
        },
        failure: function (response) {
        }
    });
}

function LoadPumpStation(dmaId) {
    if (interval != null)
        clearInterval(interval);
    if (!dmaId) return;// dmaId = 0;
    var dmaDownList = $('#SelectedPumpStationId');
    $.ajax({
        type: "POST",
        url: $("#getPumpStationUrl").val(),
        data: JSON.stringify({ dmaId: dmaId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var res = JSON.parse(data.Data);
            dmaDownList.empty();
            dmaDownList.append($('<option/>', {
                value: 0,
                text: "Select Pumpstation"
            }));
            for (var key in res) {
                if (res.hasOwnProperty(key)) {

                    dmaDownList.append($('<option/>', {
                        value: key,
                        text: res[key]
                    }));
                }
            }
        },
        failure: function (response) {
        }
    });
}
var interval;
function showScada() {


    $('#searchResults').html('<img src="' + $("#imageUrl").val() + '/ajax-loader.gif">').delay(50000).fadeIn(4000);

    var url = $("#showScadaUrl").val();
    var pumpStationId = $('#SelectedPumpStationId').val();
    $('#searchResults').load(url, { pumpStationId: pumpStationId });
    refreshScada();
}

function LoadScada() {
    if (interval != null)
        clearInterval(interval);

}

function refreshScada() {
    var pumpStationId = $('#SelectedPumpStationId').val();
    if (pumpStationId > 0) {

        if (interval != null)
            clearInterval(interval);

        interval = setInterval(function() {
            $.ajax({
                type: "POST",
                url: $("#getScadaDataUrl").val(),
                data: JSON.stringify({ pumpStationId: pumpStationId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function(data) {
                    if (data.IsSuccess == true) {
                        $('#motorswitch *').prop('disabled', false);
                        $('#connectionStatus').val('Online');
                        for (var i = 0; i < data.MotorList.length; i++) {

                            //if the name is what we are looking for return it
                            if (i % 2 == 0) {
                                $('#pmotorStatus').val(data.MotorList[i].MotorStatus);
                                $('#pmotorCommandTime').val(data.MotorList[i].LastCommandTime);
                                if (data.MotorList[i].Auto == true) {
                                    $("#pmotorAuto").text('Auto');
                                } else {
                                    $("#pmotorAuto").text('Manual');
                                    $('#pautoarea').show();
                                }

                                if ($('#motorswitchStatus').text() == 'Processing...') {
                                    if (data.MotorList[i].MotorStatus != $('#motorswitch').text()) {
                                        if (data.MotorList[i].MotorStatus == 'ON') {
                                            $("#motorswitch").removeClass("label-danger");
                                            $("#motorswitch").addClass("label-success");

                                        }

                                        if (data.MotorList[i].MotorStatus == 'OFF') {
                                            $("#motorswitch").addClass("label-danger");
                                            $("#motorswitch").removeClass("label-success");
                                        }
                                        $('#motorswitch').text(data.MotorList[i].MotorStatus);
                                        $('#motorswitchStatus').text('');
                                        $('#pmotorCommandSent').text('');
                                    }

                                    if (data.MotorList[i].MotorStatus == $('#motorswitch').text()) {
                                        var d1 = new Date($('#pmotorCommandSent').text());
                                        var d2 = new Date(d1);
                                        d2.setMinutes(d1.getMinutes() + 1);
                                        if (new Date() > d2) {
                                            if (data.MotorList[i].MotorStatus == 'ON') {
                                                $("#motorswitch").removeClass("label-danger");
                                                $("#motorswitch").addClass("label-success");
                                            }

                                            if (data.MotorList[i].MotorStatus == 'OFF') {
                                                $("#motorswitch").addClass("label-danger");
                                                $("#motorswitch").removeClass("label-success");
                                            }
                                            $('#motorswitch').text(data.MotorList[i].MotorStatus);
                                            $('#motorswitchStatus').text('');
                                            $('#pmotorCommandSent').text('');
                                        }
                                    }
                                }


                            }
                            if (i % 2 == 1) {
                                $('#cmotorStatus').val(data.MotorList[i].MotorStatus);
                                $('#cmotorCommandTime').val(data.MotorList[i].LastCommandTime);

                                if (data.MotorList[i].MotorStatus == 'ON') {
                                    $("#cmotorStatus").removeClass("label-danger");
                                    $("#cmotorStatus").addClass("label-success");

                                }

                                if (data.MotorList[i].MotorStatus == 'OFF') {
                                    $("#cmotorStatus").addClass("label-danger");
                                    $("#cmotorStatus").removeClass("label-success");
                                }

                                var d3 = new Date($('#connectionStatusTime').text());
                                var d4 = new Date(d3);

                                d4.setMinutes(d3.getMinutes() + 1);

                                if (d4 > new Date()) {
                                    $('#connectionStatus').text('Online');
                                    $("#connectionStatus").removeClass("label-danger");
                                    $("#connectionStatus").addClass("label-success");
                                } else {
                                    $('#connectionStatus').text('Offline');
                                    $("#connectionStatus").addClass("label-danger");
                                    $("#connectionStatus").removeClass("label-success");
                                }
                                $('#connectionStatusTime').text(data.LastDataRecived);

                            }

                        }
                        var res = JSON.parse(data.SensorList);
                        for (var key in res) {
                            if (res.hasOwnProperty(key)) {
                                $('#' + key).text(res[key]);


                            }
                        }


                    }
                },
                failure: function(response) {
                }
            });
        }, 5000);

    }
}

