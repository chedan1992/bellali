//$(function () {
//    var d1 = [];
//    for (var i = 1; i <= 12; i += 1) {
//        d1.push([i, parseInt((Math.floor(Math.random() * (1 + 20 - 10))) + 10)]);
//        //        d1.push([i,i]);//xÖá,yÖá
//    }
//    $("#flot-1ine").length && $.plot($("#flot-1ine"), [{
//        data: d1
//    }],
//      {
//          series: {
//              lines: {
//                  show: true,
//                  lineWidth: 2,
//                  fill: true,
//                  fillColor: {
//                      colors: [{
//                          opacity: 0.0
//                      }, {
//                          opacity: 0.2
//                      }]
//                  }
//              },
//              points: {
//                  radius: 5,
//                  show: true
//              },
//              grow: {
//                  active: true,
//                  steps: 50
//              },
//              shadowSize: 2
//          },
//          grid: {
//              hoverable: true,
//              clickable: true,
//              tickColor: "#f0f0f0",
//              borderWidth: 1,
//              color: '#f0f0f0'
//          },
//          colors: ["#65bd77"],
//          xaxis: {

//          },
//          yaxis: {
//              ticks: 5
//          },
//          tooltip: true,
//          tooltipOpts: {
//              content: "chart: %x.1 is %y.4",
//              defaultTheme: false,
//              shifts: {
//                  x: 0,
//                  y: 20
//              }
//          }
//      }



//  );

//    $(".flot-x-axis").children().each(function () {
//        // $(this).html(parseFloat($(this).html()) * 10);
//        $(this).html(parseFloat($(this).html()));

//    });
//});