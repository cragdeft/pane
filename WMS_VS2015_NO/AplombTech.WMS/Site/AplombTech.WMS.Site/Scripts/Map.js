

$(function () { $('#legend').jstree(); });
//var map;
//var myLatLng = { lat: 23.751284, lng: 90.393570 };
//var marker;
//var z2marker;
//var z3marker;
//var z4marker;
//var zone1;
//var markers = {};
//var pumpStations = $('*[id^="pump_"]');

//var zone1PolyGonCoords = [
//    { lat: 23.784850, lng: 90.389999 },
//    { lat: 23.784676, lng: 90.393992 },
//    { lat: 23.784421, lng: 90.394196 },
//    { lat: 23.785295, lng: 90.397071 },
//    { lat: 23.785108, lng: 90.398809 },
//    { lat: 23.785108, lng: 90.398809 },
//    { lat: 23.775901, lng: 90.393132 },
//    { lat: 23.775371, lng: 90.389860 },
//    { lat: 23.781862, lng: 90.389291 },
//    { lat: 23.784850, lng: 90.389999 }
//];

//var infowindow;
//google.load('visualization', '1.0', { 'packages': ['corechart'] });

//function initMap() {
//    map = new window.google.maps.Map(document.getElementById('map'), {
//        center: myLatLng,
//        zoom: 13
//    });



//    marker = new window.google.maps.Marker({
//        position: { lat: 23.781089, lng: 90.391704 },
//        map: map,
//        label: "Pump1",
//        title: 'Pump 1'
//    });

//    z2marker = new window.google.maps.Marker({
//        position: { lat: 23.782989, lng: 90.391704 },
//        map: map,
//        label: "Sensor values",
//        title: 'Over view'
//    });

//    z3marker = new window.google.maps.Marker({
//        position: { lat: 23.783589, lng: 90.391704 },
//        map: map,
//        label: "Pump3",
//        title: 'Pump 3'
//    });

//    z4marker = new window.google.maps.Marker({
//        position: { lat: 23.784689, lng: 90.391704 },
//        map: map,
//        label: "Pump4",
//        title: 'Pump 4'
//    });

//    marker.addListener('mouseover', function () {
//        //drawChart(this);
//    });

//    z2marker.addListener('mouseover', function () {
//        //drawChart(this);
//    });

//    z3marker.addListener('mouseover', function () {
//        //drawChart(this);
//    });

//    z4marker.addListener('mouseover', function () {
//        //drawChart(this);
//    });


//    // Construct the polygon.
//    zone1 = new window.google.maps.Polygon({
//        paths: zone1PolyGonCoords,
//        strokeColor: 'green',
//        strokeOpacity: 0.8,
//        strokeWeight: 2,
//        fillColor: 'green',
//        fillOpacity: 0.35
//    });
//    zone1.setMap(map);
//    zone1.addListener('mouseover', function () {
//        highlightPolyGon(this);
//    });

//    zone1.addListener('mouseout', function () {
//        this.setOptions({ strokeColor: 'green' });;
//    });

//    var legend = document.getElementById('legend');


//    map.controls[window.google.maps.ControlPosition.RIGHT_BOTTOM].push(legend);
//}

//$('#legend').on("changed.jstree", function (e, data) {

//    zone1.setOptions({ strokeColor: 'green' });

//    if ($('.jstree-clicked').text().trim()) {

//        var stationName = $('.jstree-clicked').text();
//        var depth = data.node.parents.length;
//        var node_id = data.node.id;

//        if (depth == 3) {
//            node_id = node_id.split("_")[1];

//            $.ajax({
//                type: "POST",
//                url: '/ZoneMap/GetOverViewDataOfPumpStation',
//                contentType: "application/json; charset=utf-8",
//                data: JSON.stringify({ pumpStationId: node_id }),
//                dataType: "json",
//                success: function (model) {
//                    if (model.IsSuccess) {
//                        var contentString = '<h2>Overview</h2><hr/> <div >';
//                        //drawChart(z2marker, model.data, stationName);
//                        for (var key in model.Data) {
//                            if (model.Data.hasOwnProperty(key)) {
//                                var val = model.Data[key];
//                                if (val !== null) {
//                                    contentString += '<h4>' + key + '</h4>' + ' <strong>' + model.Data[key] + ' </strong>';
//                                }

//                            }
//                        }
//                        drawChart(z2marker, contentString + '</div>');
//                    }

//                },
//                error: function () { }
//            });
//        }

//        if (depth == 4) {
//            node_id = node_id.split("_")[1];


//            $.ajax({
//                type: "POST",
//                url: '/ZoneMap/GetSingleSensorStatus',
//                contentType: "application/json; charset=utf-8",
//                data: JSON.stringify({ sensorId: node_id }),
//                dataType: "json",
//                success: function (model) {
//                    if (model.IsSuccess) {

//                        //drawChart(z2marker, model.data, stationName);


//                        if (stationName.includes("CT")) {
//                            var contentString;
//                            if (model.Value > 0 )
//                                contentString = '<h2>Cholorination On</h2>';
//                            else {
//                                contentString = '<h2>Cholorination Off</h2>';
//                            }
//                            drawChart(z2marker, contentString);
//                        } else {
//                            drawSensorData(z2marker, stationName.trim(), model.Value, model.Unit);
//                        }
//                    }

//                },
//                error: function () { }
//            });
//        }

//    }
//});

//function drawChart(marker, content) {

//    var infowindow2 = new google.maps.InfoWindow({
//        content: content
//    });

//    infowindow2.open(marker.getMap(), marker);
//}

//function drawSensorData(marker, sensorName, sensorValue, unit) {

//    // Create the data table.
//    var data = new google.visualization.DataTable();
//    data.addColumn('string', 'Topping');
//    data.addColumn('number', unit);
//    data.addRows([
//        [unit, parseFloat(sensorValue)]
//    ]);

//    // Set chart options
//    var options = {
//        'title': sensorName + ' ' +
//            marker.getPosition().toString(),
//        'width': 600,
//        'height': 150
//    };

//    node = document.createElement('div');
//    infoWindow = new google.maps.InfoWindow();
//    barchart = new google.visualization.BarChart(node);

//    barchart.draw(data, options);
//    infoWindow.setContent(node);
//    infoWindow.open(marker.getMap(), marker);
//}

//function highlightPolyGon(zone1) {
//    zone1.setOptions({ strokeColor: 'black' });
//}
