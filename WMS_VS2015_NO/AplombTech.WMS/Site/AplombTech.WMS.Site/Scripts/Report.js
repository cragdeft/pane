﻿var interval;
var chart;

$(function () {
    $('#inputPanel').hide();
});
$("#show").click(function (e) {
    e.preventDefault();
    if (interval != null)
        clearInterval(interval);
    var model = setModel();
    $.ajax({
        type: "POST",
        url: '/DrillDown/GetReportModel',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ model: model }),
        dataType: "json",
        success:
            function (data) {
                if (data.IsSuccess) {
                    if ($('#ReportType').val() == 5) {
                        $('#chart_div').empty();
                        showRealChart(data);

                    } else {
                        $('#chartContainer').empty();
                        showGraph(data);

                    }

                }

            },
        error: function () { }
    });

});

$("#Month").change(function () {
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

function showRealChart(data2) {
    Highcharts.setOptions({
        global: {
            useUTC: false
        }
    });
    chart = new Highcharts.Chart({
        chart: {
            renderTo: 'chartContainer',
            defaultSeriesType: 'spline',
            events: {
                load: function () {
                    var series = this.series[0];

                    interval = setInterval(function () {
                        //var shift = series.data.length > 20; // shift if the series is longer than 20

                        $.ajax({
                            type: "POST",
                            url: '/DrillDown/GetReportModel',
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({ model: setModel() }),
                            dataType: "json",
                            success:
                                function (data) {
                                    if (data.IsSuccess) {
                                        if ($('#ReportType').val() == 5) {
                                            var x = (new Date()).getTime(), // current time
                                                y = data.Data.Series[0].data[0];
                                            series.addPoint([x, y], true, series.data.length > 20);
                                        }
                                    }

                                },
                            error: function () { }
                        });


                    }, 1000);
                }
            }
        },
        global: {
            useUTC: false
        },
        title: {
            text: data2.Data.GraphTitle
        },
        xAxis: {
            type: 'datetime',
            tickPixelInterval: 150,
            maxZoom: 20 * 1000,
            gridLineWidth: 1
        },
        yAxis: {
            minPadding: 0.2,
            maxPadding: 0.2,
            title: {
                text: data2.Data.Unit,
                margin: 80
            },
            lineWidth: 0,
            gridLineWidth: 0,
            lineColor: 'transparent'
        },
        credits: {
            text: 'Aplombtech BD',
            href: 'http://www.example.com'
        },
        tooltip: {
            valueSuffix: ' ' + data2.Data.Unit
        },
        series: [{
            name: data2.Data.Series[0].name,
            data: []
        }],
        exporting: {
            enabled: true
        }
    });

    chart.setOptions({
        global: {
            useUTC: false
        }
    });
}
$("#ReportType").change(function () {
    if (interval != null)
        clearInterval(interval);
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
        $('#inputPanel').hide();
        $("#TransmeType option[value='1']").show();
        $("#TransmeType option[value='2']").show();
        $("#TransmeType option[value='5']").show();
        $('#exp').hide();
    }

    if (reportType != 5 && reportType != 1) {
        $("#TransmeType option[value='1']").hide();
        $("#TransmeType option[value='2']").hide();
        $("#TransmeType option[value='5']").hide();
    }
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

function GetHourValue(name) {
    var d = new Date(name);

    return d.getHours();

}
function getDateOfWeek(w, y) {
    var d = (1 + (w - 1) * 7); // 1st of January + 7 days for each week

    return new Date(y, 0, d);
}
function showGraph(data) {
    var plotline;
    if ($('#ReportType').val() == 1) {
        plotline = [
            {
                color: '#FF0000',
                dashStyle: 'ShortDash',
                width: 2,
                value: data.Data.SelectedSensor.MinimumValue,
                zIndex: 0,
                label: {
                    text: 'Minimum value'
                }
            }
        ];
    } else {
        plotline = [{}];
    }
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

        credits: {
            text: 'Aplombtech BD',
            href: 'http://www.example.com'
        },
        plotOptions: {
            series: {
                cursor: 'pointer',
                point: {
                    events: {
                        click: function () {

                            if ($('#ReportType').val() == 2) {
                                $('#ReportType').val('1').change();
                                $("#Hour").val(GetHourValue(this.category));
                            }

                            if ($('#ReportType').val() == 3) {
                                $('#ReportType').val('2').change();
                                var date = getDateOfWeek($('#Week').val(), $('#Year').val());
                                $('#Month').val(date.getMonth() + 1);

                                $("#Day").val(date.getDate());

                            }

                            if ($('#ReportType').val() == 4) {
                                $('#ReportType').val('2').change();
                                var d = new Date(this.category);
                                console.log(this.category);
                                var n = d.getDate();
                                $("#Day").val(n);
                            }

                            var model = setModel();
                            $.ajax({
                                type: "POST",
                                url: '/DrillDown/GetReportModel',
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
            lineColor: 'transparent',
            plotLines: plotline
        },
        tooltip: {
            valueSuffix: ' ' + data.Data.Unit
        },
        legend: {
            layout: 'horizontal',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0
        },
        series: data.Data.Series,
        exporting: {
            enabled: true
        }

    });

}