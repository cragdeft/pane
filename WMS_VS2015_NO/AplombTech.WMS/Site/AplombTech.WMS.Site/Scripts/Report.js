var interval;
var chart;

$(function () {
    $('#inputPanel').hide();
    
});
$("#show").click(function (e) {
    
    e.preventDefault();
    if (!validate())
        return;
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
        error: function(e) {
            alert(e);
        }
    });

});

$("#Month").change(function () {
    validate();
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

$("#SelectedPumpStationId").change(function () {
    validate();
}); 
$("#TransmeType").change(function () {
    validate();
});

$("#Year").change(function () {
    validate();
});

$("#Week").change(function () {
    validate();
});

$("#Day").change(function () {
    validate();
});

$("#Hour").change(function () {
    validate();
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
    validate();
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
        $("#TransmeType option[value='6']").show();
        $("#TransmeType option[value='7']").show();
        $('#exp').hide();
    }

    if (reportType != 5 && reportType != 1) {
        $("#TransmeType option[value='1']").hide();
        $("#TransmeType option[value='2']").hide();
        $("#TransmeType option[value='5']").hide();
        $("#TransmeType option[value='6']").hide();
        $("#TransmeType option[value='7']").hide();
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
                                
                                $("#Hour").val(GetHourValue(this.category));
                                $('#ReportType').val('1').change();
                            }

                            if ($('#ReportType').val() == 3) {
                                
                                var date = getDateOfWeek($('#Week').val(), $('#Year').val());
                                $('#Month').val(date.getMonth() + 1);

                                $("#Day").val(date.getDate());
                                $('#ReportType').val('2').change();

                            }

                            if ($('#ReportType').val() == 4) {
                                
                                var d = new Date(this.category);
                                console.log(this.category);
                                var n = d.getDate();
                                $("#Day").val(n);
                                $('#ReportType').val('2').change();
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

function validate() {
    var valid = true;
    if ($('#SelectedPumpStationId').val() <= 0) {
        $("#SelectedPumpStationId").addClass("input-validation-error");
        $("#pumpstationvalid").text("Pumpstation required");
        valid = false;

    } else {
        $("#SelectedPumpStationId").addClass("valid");
        $("#pumpstationvalid").text("");
    }

    if ($('#ReportType').val() <= 0) {
        $("#ReportType").addClass("input-validation-error");
        $("#reporttypevalid").text("Report Type required");
        valid = false;

    } else {
        $("#ReportType").addClass("valid");
        $("#reporttypevalid").text("");
    }

    if ($('#TransmeType').val() <= 0) {
        $("#TransmeType").addClass("input-validation-error");
        $("#sensorvalid").text("Sensor Type required");
        valid = false;

    } else {
        $("#TransmeType").addClass("valid");
        $("#sensorvalid").text("");
    }

    if ($('#Year').val() < 2016 && $('#ReportType').val() != 5) {
        $("#Year").addClass("input-validation-error");
        $("#yearvalid").text("year required");
        valid = false;

    } else {
        $("#Year").addClass("valid");
        $("#yearvalid").text("");
    }

    if (($('#ReportType').val() == 1 || $('#ReportType').val() == 2 || $('#ReportType').val() == 4) && $('#Month').val() <= 0) {
        $("#Month").addClass("input-validation-error");
        $("#monthvalid").text("month required");
        valid = false;

    } else {
        $("#Month").addClass("valid");
        $("#monthvalid").text("");
    }

    if ($('#ReportType').val() == 3 && $('#Week').val() < 1) {
        $("#Week").addClass("input-validation-error");
        $("#weekvalid").text("Week required");
        valid = false;

    } else {
        $("#Week").addClass("valid");
        $("#weekvalid").text("");
    }

    if (($('#ReportType').val() == 1 || $('#ReportType').val() == 2 ) && $('#Day').val() < 1) {
        $("#Day").addClass("input-validation-error");
        $("#dayvalid").text("day required");
        valid = false;

    } else {
        $("#Day").addClass("valid");
        $("#dayvalid").text("");
    }

    if ($('#ReportType').val() == 1 && $('#Hour').val() < 1) {
        $("#Hour").addClass("input-validation-error");
        $("#hourvalid").text("Hour required");
        valid = false;

    } else {
        $("#Hour").addClass("valid");
        $("#hourvalid").text("");
    }
    return valid;
}

$('#exp').click(function (e) {
    if (!validate())
        e.preventDefault();
});