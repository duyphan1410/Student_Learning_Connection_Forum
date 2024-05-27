
    $(function () {
        "use strict";
    // Dashboard 1 Morris-chart
    Morris.Area({
        element: 'morris-area-chart',
            data: [{
        period: '2010',
        Shoe: 0,
        Total: 0,
        itouch: 0
            }, {
        period: '2021',
        Shoe: 130,
        Total: 100,
        itouch: 80
            }, {
        period: '2012',
        Shoe: 80,
        Total: 60,
        itouch: 70
            }, {
        period: '2013',
        Shoe: 70,
        Total: 200,
        itouch: 140
            }, {
        period: '2014',
        Shoe: 180,
        Total: 150,
        itouch: 140
            }, {
        period: '2015',
        Shoe: 105,
        Total: 100,
        itouch: 80
            }, {
        period: '2016',
        Shoe: 250,
        Total: 150,
        itouch: 200
    }],
    xkey: 'period',
    ykeys: ['Shoe', 'Total'],
    labels: ['Shoe', 'Total'],
    pointSize: 0,
    fillOpacity: 0,
    pointStrokeColors: ['#f62d51', '#7460ee', '#009efb'],
    behaveLikeLine: true,
    gridLineColor: '#f6f6f6',
    lineWidth: 1,
    hideHover: 'auto',
    lineColors: ['#009efb', '#7460ee', '#009efb'],
    resize: true
    });
});
