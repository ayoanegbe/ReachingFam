
var prd = document.getElementById("language").value;
let graphData = document.getElementById("graphData").value;;

gData = JSON.parse(graphData);
console.log('families1', gData);
console.log('families2', gData.familyHampers);
console.log('partners', gData.partnerHampers);
console.log('donors', gData.donors);
console.log('categories', gData.categories);

//$.ajax({
//    url: '/home/GetGraphData/', 
//    data: { period: prd },
//    type: 'POSt',
//    async: true,
//    dataType: 'json',
//    success: function (response) {
//        graphData = response;
//        console.log('graphData', graphData);
        
//        families = response.familyHampers;
//        console.log('families1', response.familyHampers);
//        console.log('families2', families);
//        console.log('partners', graphData.partnerHampers);
//        console.log('donors', graphData.donors);
//        console.log('categories', graphData.categories);
//    }
//});

var options = {
    
    series: [{
        name: 'Families',
        type: 'column',
        data: gData.families
    }, {
        name: 'Partners',
        type: 'column',
        data: gData.partners
    }, {
        name: 'Donors',
        type: 'line',
        data: gData.donors
    }],
    chart: {
        height: 350,
        type: 'line',
        stacked: false,
        zoom: {
            enabled: false
        },
        toolbar: {
            show: true,
            tools: {
                download: false // <== line to add
            }
        }
    },
    dataLabels: {
        enabled: false
    },
    stroke: {
        width: [1, 1, 4]
    },
    xaxis: {
        categories: gData.categories,
    },
    yaxis: [
        {
            axisTicks: {
                show: true,
            },
            axisBorder: {
                show: true,
                color: '#008FFB'
            },
            labels: {
                style: {
                    colors: '#008FFB',
                }
            },
            title: {
                text: "Families",
                style: {
                    color: '#008FFB',
                }
            },
            tooltip: {
                enabled: true
            }
        },
        {
            seriesName: 'Partners',
            opposite: true,
            axisTicks: {
                show: true,
            },
            axisBorder: {
                show: true,
                color: '#00E396'
            },
            labels: {
                style: {
                    colors: '#00E396',
                }
            },
            title: {
                text: "Partners",
                style: {
                    color: '#00E396',
                }
            },
        },
        {
            seriesName: 'Donors',
            opposite: true,
            axisTicks: {
                show: true,
            },
            axisBorder: {
                show: true,
                color: '#FEB019'
            },
            labels: {
                style: {
                    colors: '#FEB019',
                },
            },
            title: {
                text: "Donors",
                style: {
                    color: '#FEB019',
                }
            }
        },
    ],
    tooltip: {
        fixed: {
            enabled: true,
            position: 'topLeft', // topRight, topLeft, bottomRight, bottomLeft
            offsetY: 30,
            offsetX: 60
        },
    },
    legend: {
        horizontalAlign: 'left',
        offsetX: 40
    }
};

var chart = new ApexCharts(document.querySelector("#chart"), options);

onMounted(() => {
    nextTick(() => {       
        chart.render();
    });
});

