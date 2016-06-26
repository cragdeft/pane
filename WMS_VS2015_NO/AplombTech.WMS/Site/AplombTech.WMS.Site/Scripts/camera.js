
    var flashVars = {
        streamer: 'rtmp://192.169.244.37/myapp',
        file:'test'
    };
var params = {};
params.allowfullscreen = "true";
var attributes = {};
swfobject.embedSWF($('swfUrl').val(), "rtmp-publisher", "200", "150", "9.0.0", null, flashVars, params, attributes);

