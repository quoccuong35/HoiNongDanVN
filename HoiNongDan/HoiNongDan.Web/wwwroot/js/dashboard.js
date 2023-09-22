$(function () {
    var ctx = document.getElementById("chartBar2");
    var myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ["Hoocmon", "Quận 12", "Củ Chi", "TP.Thủ Đức"],
            datasets: [
                {
                    label: "Nam",
                    data: [28, 48, 40, 19],
                    borderColor: "#05c3fb",
                    borderWidth: "0",
                    backgroundColor: "#05c3fb"
                },
                {
                    label: "Nữ",
                    data: [44, 22, 60, 36],
                    borderColor: "#09ad95",
                    borderWidth: "0",
                    backgroundColor: "#09ad95"
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                xAxes: [{
                    barPercentage: 0.4,
                    barValueSpacing: 0,
                    barDatasetSpacing: 0,
                    barRadius: 0,
                    ticks: {
                        fontColor: "#9ba6b5",
                    },
                    gridLines: {
                        color: 'rgba(119, 119, 142, 0.2)'
                    }
                }],
                yAxes: [{
                    ticks: {
                        beginAtZero: true,
                        fontColor: "#9ba6b5",
                    },
                    gridLines: {
                        color: 'rgba(119, 119, 142, 0.2)'
                    },
                }]
            },
            legend: {
                labels: {
                    fontColor: "#9ba6b5"
                },
            },
        }
    });
    /* Pie Chart*/
    /* Pie Chart*/
    var datapie = {
        labels: ['Nam', 'Nữ'],
        datasets: [{
            data: [39099, 19018],
            backgroundColor: ['#05c3fb', '#09ad95']
        }]
    };
    var optionpie = {
        maintainAspectRatio: false,
        responsive: true,
        legend: {
            display: false,
        },
        animation: {
            animateScale: true,
            animateRotate: true
        }
    };
    var ctx7 = document.getElementById('chartDonut');
    var myPieChart7 = new Chart(ctx7, {
        type: 'pie',
        data: datapie,
        options: optionpie
    });

    //
    var table = $('#file-datatable').DataTable({
        responsive: false,
        autoWidth: true,
        autoHeight: false,
        scrollCollapse: false,
        scrollX: true,
        scrollY: 300,
        buttons: ['excel', 'pdf'],
        language: {
            searchPlaceholder: 'Search...',
            scrollX: "100%",
            sSearch: '',
        }
    });
    table.buttons().container()
        .appendTo('#file-datatable_wrapper .col-md-6:eq(0)');


})
