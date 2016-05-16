function LoadDma(zoneId) {
    if (!zoneId) zoneId = 0;
    var dmaDownList = $('#SelectedDmaId');
    $.ajax({
        type: "POST",
        url: window.location.origin+"/ScadaMap/GetDmaDropdownData",
        data: JSON.stringify({ zoneId: zoneId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
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
    if (!dmaId) dmaId = 0;
    var dmaDownList = $('#SelectedPumpStationId');
    $.ajax({
        type: "POST",
        url: window.location.origin+"/ScadaMap/GetPumpStationDropdownData",
        data: JSON.stringify({ dmaId: dmaId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
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

function showScada() {

    $('#searchResults').html('<img src="../../Images/ajax-loader.gif">').delay(50000).fadeIn(4000);
    var url = window.location.origin + "/ScadaMap/ShowScada";
    var pumpStationId = $('#SelectedPumpStationId').val();
    $('#searchResults').load(url, { pumpStationId: pumpStationId });
}
