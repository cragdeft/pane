

$(function () { $('#legend').jstree(); });
var map;
var myLatLng = { lat: 23.751284, lng: 90.393570 };
var marker;
var z2marker;
var z3marker;
var z4marker;
var zone1;
var node, infoWindow, barchart;

var zone1PolyGonCoords = [
    { lat: 23.784850, lng: 90.389999 },
    { lat: 23.784676, lng: 90.393992 },
    { lat: 23.784421, lng: 90.394196 },
    { lat: 23.785295, lng: 90.397071 },
    { lat: 23.785108, lng: 90.398809 },
    { lat: 23.785108, lng: 90.398809 },
    { lat: 23.775901, lng: 90.393132 },
    { lat: 23.775371, lng: 90.389860 },
    { lat: 23.781862, lng: 90.389291 },
    { lat: 23.784850, lng: 90.389999 }
];

var infowindow;
google.load('visualization', '1.0', { 'packages': ['corechart'] });

function initMap() {
    map = new window.google.maps.Map(document.getElementById('map'), {
        center: myLatLng,
        zoom: 13
    });


    marker = new window.google.maps.Marker({
        position: { lat: 23.781089, lng: 90.391704 },
        map: map,
        label: "Pump1",
        title: 'Pump 1'
    });

    z2marker = new window.google.maps.Marker({
        position: { lat: 23.782989, lng: 90.391704 },
        map: map,
        label: "Pump2",
        title: 'Pump 2'
    });

    z3marker = new window.google.maps.Marker({
        position: { lat: 23.783589, lng: 90.391704 },
        map: map,
        label: "Pump3",
        title: 'Pump 3'
    });

    z4marker = new window.google.maps.Marker({
        position: { lat: 23.784689, lng: 90.391704 },
        map: map,
        label: "Pump4",
        title: 'Pump 4'
    });

    marker.addListener('mouseover', function () {
        drawChart(this);
    });

    z2marker.addListener('mouseover', function () {
        drawChart(this);
    });

    z3marker.addListener('mouseover', function () {
        drawChart(this);
    });

    z4marker.addListener('mouseover', function () {
        drawChart(this);
    });


    // Construct the polygon.
    zone1 = new window.google.maps.Polygon({
        paths: zone1PolyGonCoords,
        strokeColor: 'green',
        strokeOpacity: 0.8,
        strokeWeight: 2,
        fillColor: 'green',
        fillOpacity: 0.35
    });
    zone1.setMap(map);
    zone1.addListener('mouseover', function () {
        highlightPolyGon(this);
    });

    zone1.addListener('mouseout', function () {
        this.setOptions({ strokeColor: 'green' });;
    });

    var legend = document.getElementById('legend');


    map.controls[window.google.maps.ControlPosition.RIGHT_BOTTOM].push(legend);
}


$('#legend').on("changed.jstree", function (e, data) {

    zone1.setOptions({ strokeColor: 'green' });

    if ($('.jstree-clicked').text().trim() === 'Pump 1') {

        drawChart(marker);
    }

    if ($('.jstree-clicked').text().trim() === 'Pump 2') {

        drawChart(z2marker);
    }

    if ($('.jstree-clicked').text().trim() === 'Pump 3') {

        drawChart(z3marker);
    }

    if ($('.jstree-clicked').text().trim() === 'Pump 4') {

        drawChart(z4marker);
    }

    if ($('.jstree-clicked').text().trim() === 'Zone 1' || $('.jstree-clicked').text().trim() === 'DMA 1' || $('.jstree-clicked').text().trim() === 'Pump 1') {

        highlightPolyGon(zone1);
    }
});

function drawChart(marker) {

    // Create the data table.
    var data = new google.visualization.DataTable();
    data.addColumn('string', 'Topping');
    data.addColumn('number', 'Slices');
    data.addRows([
        ['Production', 3],
        ['Energy', 1],
        ['Pressure', 1],
        ['WaterLevel', 1],
        ['Clorination', 2]
    ]);

    // Set chart options
    var options = {
        'title': marker.title + ' ' +
            marker.getPosition().toString(),
        'width': 400,
        'height': 150
    };

    node = document.createElement('div');
    infoWindow = new google.maps.InfoWindow();
    barchart = new google.visualization.BarChart(node);

    barchart.draw(data, options);
    infoWindow.setContent(node);
    infoWindow.open(marker.getMap(), marker);
}

function highlightPolyGon(zone1) {
    zone1.setOptions({ strokeColor: 'black' });
}
