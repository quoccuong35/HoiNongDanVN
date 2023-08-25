$(function () {
    var ctx = document.getElementById("chartBar2");
    var myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ["HCM", "Đồng Nai", "Bình Dương", "Long An"],
            datasets: [
                {
                    label: "Cơ hữu- biên chế",
                    data: [65, 59, 80, 81],
                    borderColor: "#0e344f",
                    borderWidth: "0",
                    backgroundColor: "#0e344f"
                },
                {
                    label: "Hợp đồng dài hạn",
                    data: [28, 48, 40, 19],
                    borderColor: "#05c3fb",
                    borderWidth: "0",
                    backgroundColor: "#05c3fb"
                },
                {
                    label: "Hợp đồng khoán",
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
        labels: ['Cơ hữu- biên chế', 'Hợp đồng dài hạn', 'Hợp đồng khoán'],
        datasets: [{
            data: [285, 135, 162],
            backgroundColor: ['#0e344f', '#05c3fb', '#09ad95']
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
