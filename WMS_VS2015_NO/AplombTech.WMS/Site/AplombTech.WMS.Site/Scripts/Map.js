


var map;
var myLatLng = { lat: 23.751284, lng: 90.393570 };
var marker;
var z2marker;
var z3marker;
var z4marker;
var zone1;
var markers = {};
var pumpStations = $('*[id^="pump_"]');

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

function initMap() {
    map = new window.google.maps.Map(document.getElementById('map'), {
        center: myLatLng,
        zoom: 13
    });

     var ctaLayer = new google.maps.KmlLayer({
                url: 'https://encodable.com/uploaddemo/files/802_DMA_Boundary.kml',
                map: map
            });

            var ctaLayer1 = new google.maps.KmlLayer({
                url: 'https://encodable.com/uploaddemo/files/Boundary_DMA_808.kml',
                map: map
            });

            var ctaLayer2 = new google.maps.KmlLayer({
                url: 'https://encodable.com/uploaddemo/files/DMA_BND_809.kml',
                map: map
            });

            var ctaLayer3 = new google.maps.KmlLayer({
                url: 'https://encodable.com/uploaddemo/files/DMA_Boundary_801.kml',
                map: map
            });

            var ctaLayer4 = new google.maps.KmlLayer({
                url: 'https://encodable.com/uploaddemo/files/DMA_Boundary_803.kml',
                map: map
            });

            var ctaLayer5 = new google.maps.KmlLayer({
                url: 'https://encodable.com/uploaddemo/files/DMA_Boundary_804.kml',
                map: map
            });

            var ctaLayer6 = new google.maps.KmlLayer({
                url: 'https://encodable.com/uploaddemo/files/DMA_Boundary_805.kml',
                map: map
            });

            var ctaLayer7 = new google.maps.KmlLayer({
                url: 'https://encodable.com/uploaddemo/files/DMA_Boundary_807.kml',
                map: map
            });

            var ctaLayer9 = new google.maps.KmlLayer({
                url: 'https://encodable.com/uploaddemo/files/DMA_Boundary_811.kml',
                map: map
            });

            var ctaLayer8 = new google.maps.KmlLayer({
                url: 'https://encodable.com/uploaddemo/files/DMA_Boundary_NEW_806.kml',
                map: map
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
        label: "Sensor values",
        title: 'Over view'
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
        //drawChart(this);
    });

    z2marker.addListener('mouseover', function () {
        //drawChart(this);
    });

    z3marker.addListener('mouseover', function () {
        //drawChart(this);
    });

    z4marker.addListener('mouseover', function () {
        //drawChart(this);
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


    map.controls[window.google.maps.ControlPosition.RIGHT_CENTER].push(legend);
}





function highlightPolyGon(zone1) {
    zone1.setOptions({ strokeColor: 'black' });
}

