/*================================================================================
	Item Name: Materialize - Material Design Admin Template
	Version: 5.0
	Author: PIXINVENT
	Author URL: https://themeforest.net/user/pixinvent/portfolio
================================================================================

NOTE:
------
PLACE HERE YOUR OWN JS CODES AND IF NEEDED.
WE WILL RELEASE FUTURE UPDATES SO IN ORDER TO NOT OVERWRITE YOUR CUSTOM SCRIPT IT'S BETTER LIKE THIS. */

//document.addEventListener('DOMContentLoaded', function () {
//    var elems = document.querySelectorAll('.tooltipped');
//    var instances = M.Tooltip.init(elems, new object());
//});

//function DataTable()
//{
//    $(document).ready(function () {
//        $('#PullRequestTable').DataTable();
//    });
//}

function ResetDropDownList(ddl) {
    ddl.selectedIndex = 0;  
}

function DatePickerSetup()
{
    var elems = document.querySelectorAll('.datepicker');
    var instance = M.Datepicker.init(elems);
}

window.setBarChart = (chartObj) => 
{
    var ctx = document.getElementById(chartObj.id).getContext('2d');
    var myChart = new Chart(ctx, {
        type: chartObj.chartJSOrientation,
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

window.setCumulativeFlowChart = (chartObj) => {
    var ctx = document.getElementById(chartObj.id).getContext('2d');
    var myChart = new Chart(ctx, {
        type: 'line',
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
                }]
            }
        }
    });

    return true;
}   