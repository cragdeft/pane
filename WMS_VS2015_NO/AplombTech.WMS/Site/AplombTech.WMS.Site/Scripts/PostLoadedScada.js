var graphInterval;
var unit = parseInt($('#ltvalue').text());
var height = parseInt($('#waterlevel').css('height'));
var margin = parseInt($('#waterlevel').css('margin-top'));
$("#waterlevel").css("background-color", "#2fcff4");
$("#waterlevel").css("height", height - (100 - (1.35 * unit)) + "px");
$("#waterlevel").css("margin-top", margin + (100 - (1.35 * unit)) + "px");


$('#motorswitch').on('click', scadModule.MotorSwitchFunction());
$('span').on('click', scadModule.ScadaSpanClick());