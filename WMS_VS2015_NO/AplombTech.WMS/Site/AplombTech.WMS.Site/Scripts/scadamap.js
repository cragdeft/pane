function LoadDma(zoneId) {
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
                    text:res[key] 
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
    if (interval != null)
        clearInterval(interval);
    interval = setInterval(function() {
        $('#searchResults').html('<img src="' + $("#imageUrl").val() + '/ajax-loader.gif">').delay(50000).fadeIn(4000);

        var url = $("#showScadaUrl").val();
        var pumpStationId = $('#SelectedPumpStationId').val();
        $('#searchResults').load(url, { pumpStationId: pumpStationId });
    }, 10000);
}


