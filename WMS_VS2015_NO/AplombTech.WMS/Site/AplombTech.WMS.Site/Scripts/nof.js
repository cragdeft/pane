
        function LoadMapRelatedScript() {
            $(function () { $('#legend').jstree({ "themes": { "stripes": true }}); });
            $('#legend').on("changed.jstree", function (e, data) {

                //zone1.setOptions({ strokeColor: 'green' });

                if ($('.jstree-clicked').text().trim()) {

                    var stationName = $('.jstree-clicked').text();
                    var depth = data.node.parents.length;
                    var node_id = data.node.id;
                    if (depth == 1) {
                        drawDmaAndPumpStation(node_id.split("_")[1]);
                    }
                    if (depth == 3) {
                        node_id = node_id.split("_")[1];

                        $.ajax({
                            type: "POST",
                            url: '/ZoneGoogleMap/GetOverViewDataOfPumpStation',
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({ pumpStationId: node_id }),
                            dataType: "json",
                            success: function (model) {
                                if (model.IsSuccess) {
                                    var contentString = '<h2>Overview</h2><hr/> <div >';
                                    //drawChart(z2marker, model.data, stationName);
                                    for (var key in model.Data) {
                                        if (model.Data.hasOwnProperty(key)) {
                                            var val = model.Data[key];
                                            if (val !== null) {
                                                contentString += '<h4>' + key + '</h4>' + ' <strong>' + model.Data[key] + ' </strong>';
                                            }

                                        }
                                    }
                                    drawChart(markers['marker_' + node_id], contentString + '</div>');
                                }

                            },
                            error: function () { }
                        });
                    }

                    if (depth == 4) {
                        node_id = node_id.split("_")[1];


                        $.ajax({
                            type: "POST",
                            url: '/ZoneGoogleMap/GetSingleSensorStatus',
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({ sensorId: node_id }),
                            dataType: "json",
                            success: function (model) {
                                if (model.IsSuccess) {

                                    //drawChart(z2marker, model.data, stationName);
                                    

                                    if (stationName.includes("CT")) {
                                        var contentString;
                                        if (model.Value > 0)
                                            contentString = '<h2>Cholorination On</h2>';
                                        else {
                                            contentString = '<h2>Cholorination Off</h2>';
                                        }
                                        drawChart(markers['marker_' + model.PumpStationId], contentString);
                                    } else {
                                        contentString = '<div><h2>' + stationName.trim() + '</h2><hr>' + '<p>Current value = ' + model.Value + ' <strong>' + model.Unit + '</strong></p></div>';
                                        //drawSensorData(z2marker, stationName.trim(), model.Value, model.Unit);
                                        drawChart(markers['marker_' + model.PumpStationId], contentString);
                                    }
                                }

                            },
                            error: function () { }
                        });
                    }

                }
            });

            function drawChart(marker, content) {

                var infowindow2 = new google.maps.InfoWindow({
                    content: content
                });

                infowindow2.open(marker.getMap(), marker);
            }
        }