
$(function () {
    $('#inputPanel').hide();
});
$("#show").click(function (e) {
    e.preventDefault();
    
    var model = setModel();
    $.ajax({
        type: "POST",
        url: '/Report/GetReportModel',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ model: model }),
        dataType: "json",
        success:
            function (data) {
                if (data.IsSuccess) {
                    if ($('#ReportType').val() == 5) {
                        showRealChart(data);
                    }
                    else
                        showGraph(data);
                }

            },
        error: function () { }
    });

});
function showRealChart(data2) {
    var xd = [];
    
    for (var k = 0; k < data2.Data.Series.length; k++) {
        
        xd.push({
            name: 's' + k, data: (function () {
                // generate an array of random data
                var datax = [], time = (new Date()).getTime(), i;

                for (i = -3; i <= 0; i += 1) {
                    datax.push([
                        time + i * 1000,
                        Math.round(Math.random() * 100)
                    ]);

                    
                }
                return datax;
            }())
        });
    }


    $('#chart_div').highcharts('StockChart', {
        chart: {
            events: {
                load: function () {
                    // set up the updating of the chart each second
                    var series = this.series;
                    setInterval(function () {
                        for (var i = 0; i < series.length-1; i++) {
                            var seriea = series[i];

                            var x = (new Date()).getTime(), // current time
                                y = Math.round(Math.random() * 100);
                            
                            series[i].addPoint([x, y], true, true);
                        }
                    }, 5000);

                }
            }
        },

        rangeSelector: {
            buttons: [{
                count: 1,
                type: 'minute',
                text: '1M'
            }, {
                count: 5,
                type: 'minute',
                text: '5M'
            }, {
                type: 'all',
                text: 'All'
            }],
            inputEnabled: false,
            selected: 0
        },

        title: {
            text: 'Live data'
        },

        exporting: {
            enabled: false
        },

        series: xd

    });
}
$("#ReportType").change(function () {
    var reportType = $('#ReportType').val();
    $('#inputPanel').show();
    if (reportType == 1) {
        $('#wk').hide();
        $('#yr').show();
        $('#mth').show();
        $('#dy').show();
        $('#hr').show();
    }
    if (reportType == 2) {
        $('#wk').hide();
        $('#hr').hide();
        $('#yr').show();
        $('#mth').show();
        $('#dy').show();
    }
    if (reportType == 3) {
        $('#hr').hide();
        $('#mth').hide();
        $('#dy').hide();
        $('#yr').show();
        $('#wk').show();
    }
    if (reportType == 4) {
        $('#wk').hide();
        $('#dy').hide();
        $('#hr').hide();
        $('#yr').show();
        $('#mth').show();
        $('#dy').hide();
    }

    if (reportType == 5) {
        $('#mth').hide();
        $('#dy').hide();
        $('#hr').hide();
        $('#yr').show();
        $('#mth').show();
        $('#dy').hide();
        $('#inputPanel').hide();
    }
});
$(function () {
    $('.datetimepicker').datetimepicker({
        viewMode: 'months',
        showClose: true,
        showClear: true,
        toolbarPlacement: 'top'
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
        PumpStation: { PumpStationId: $('#PumpStation_PumpStationId').val() },
        SensorType: $('#SensorType').val()
    }

    return model;
}

function GetHourValue(name) {
    if (name == '12:00 AM') {
        return 0;
    }

    if (name == '1:00 AM') {
        return 1;
    }

    if (name == '2:00 AM') {
        return 2;
    }

    if (name == '3:00 AM') {
        return 3;
    }

    if (name == '4:00 AM') {
        return 4;
    }
    if (name == '5:00 AM') {
        return 5;
    }

    if (name == '6:00 AM') {
        return 6;
    }

    if (name == '7:00 AM') {
        return 7;
    }

    if (name == '8:00 AM') {
        return 8;
    }

    if (name == '9:00 AM') {
        return 9;
    }

    if (name == '10:00 AM') {
        return 10;
    }

    if (name == '11:00 AM') {
        return 11;
    }

    if (name == '12:00 PM') {
        return 12;
    }

    if (name == '1:00 PM') {
        return 13;
    }

    if (name == '2:00 PM') {
        return 14;
    }

    if (name == '3:00 PM') {
        return 15;
    }

    if (name == '4:00 PM') {
        return 16;
    }

    if (name == '5:00 PM') {
        return 17;
    }

    if (name == '6:00 PM') {
        return 18;
    }

    if (name == '7:00 PM') {
        return 19;
    }

    if (name == '8:00 PM') {
        return 20;
    }

    if (name == '9:00 PM') {
        return 21;
    }

    if (name == '10:00 PM') {
        return 22;
    }

    if (name == '11:00 PM') {
        return 23;
    }

    return 0;
}
function getDateOfWeek(w, y) {
    var d = (1 + (w - 1) * 7); // 1st of January + 7 days for each week

    return new Date(y, 0, d);
}
function showGraph(data) {
    var categories = data.Data.XaxisCategory;
    $('#chart_div').highcharts({
        title: {
            text: data.Data.GraphTitle,
            x: -20 //center
        },
        subtitle: {
            text: data.Data.GraphSubTitle,
            x: -20
        },
        xAxis: {
            categories: categories,
            gridLineWidth: 1
        },
        plotOptions: {
            series: {
                cursor: 'pointer',
                point: {
                    events: {
                        click: function () {

                            if ($('#ReportType').val() == 2) {
                                $('#ReportType').val('1').change();
                                $("#Hour").val(GetHourValue(this.category) + 1);
                            }

                            if ($('#ReportType').val() == 3) {
                                $('#ReportType').val('2').change();
                                var date = getDateOfWeek($('#Week').val(), $('#Year').val());
                                $('#Month').val(date.getMonth() + 1);
                                $("#Day").val(this.category);
                            }

                            if ($('#ReportType').val() == 4) {
                                $('#ReportType').val('2').change();
                                $("#Day").val(this.category);
                            }

                            var model = setModel();
                            $.ajax({
                                type: "POST",
                                url: '/Report/GetReportModel',
                                contentType: "application/json; charset=utf-8",
                                data: JSON.stringify({ model: model }),
                                dataType: "json",
                                success:
                                    function (data) {
                                        if (data.IsSuccess) {
                                            showGraph(data);
                                        }

                                    },
                                error: function () { }
                            });
                        }
                    }
                }
            }
        },
        yAxis: {
            title: {
                text: data.Data.Unit
            },
            lineWidth: 0,
            gridLineWidth: 0,
            lineColor: 'transparent'
        },
        tooltip: {
            valueSuffix: data.Data.Unit
        },
        legend: {
            layout: 'horizontal',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0
        },
        series: data.Data.Series
    });

}