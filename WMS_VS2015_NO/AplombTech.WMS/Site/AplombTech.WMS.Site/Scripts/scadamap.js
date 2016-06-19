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
            alert(response);
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
            alert(response);
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
    if (interval != null)
        clearInterval(interval);

    interval = setInterval(function () {
        $.ajax({
            type: "POST",
            url: $("#getScadaDataUrl").val(),
            data: JSON.stringify({ pumpStationId: $('#SelectedPumpStationId').val() }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.IsSuccess == true) {
                    $('#motorswitch *').prop('disabled', false);
                    for (var i = 0; i < data.MotorList.length; i++) {
                        //if the name is what we are looking for return it
                        if (i % 2 == 0) {
                            $('#pmotorStatus').val(data.MotorList[i].MotorStatus);
                            $('#pmotorCommandTime').val(data.MotorList[i].LastCommandTime);
                        }
                        if (i % 2 == 1) {
                            $('#cmotorStatus').val(data.MotorList[i].MotorStatus);
                            $('#cmotorCommandTime').val(data.MotorList[i].LastCommandTime);
                        }
                        if (data.MotorList[i].Auto == true);
                        {
                            if ($("#motorswitch").length !== 0) {
                                if (data.MotorList[i].MotorStatus == "ON")
                                    $("#toggle-one").toggle(true);
                                else {
                                    $("#toggle-one").toggle(false);
                                }
                            }
                        }
                    }
                    var res = JSON.parse(data.SensorList);
                    for (var key in res) {
                        if (res.hasOwnProperty(key)) {
                                $('#' + key).val(res[key]);

                                if (res[key].indexOf("CPD") != -1) {
                                    if (parseFloat(res[key]) > 0) {
                                        $('#' + key).val("On");
                                    } else {
                                        $('#' + key).val("Off");
                                    }
                                    
                            }

                            else if (res[key].indexOf("ACP") != -1) {
                                if (parseFloat(res[key]) > 0) {
                                    $('#' + key).val("On");
                                } else {
                                    $('#' + key).val("Off");
                                }
                            }

                            else  {
                                $('#' + key).val(res[key]);
                            }
                                
                        }
                    }

                    
                }
            },
            failure: function (response) {
                alert(response);
            }
        });
    }, 10000);
}

