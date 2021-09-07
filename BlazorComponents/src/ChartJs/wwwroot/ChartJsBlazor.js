// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

export function setBarChart(chartObj) {
    var ctx = document.getElementById(chartObj.id).getContext('2d');
    var myChart = new Chart(ctx, {
        type: chartObj.chartJsType,
        data: {
            labels: chartObj.labels,
            datasets: chartObj.datasets
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    },
                    stacked: chartObj.isStacked
                }],
                xAxes: [{
                    stacked: chartObj.isStacked
                }]
            }
        }
    });

    return true;
}    