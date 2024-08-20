
$(function () {
    LoadBienDongHV();
})
function LoadBienDongHV() {
    let url = window.location.href;
    var maQuanHuyen = HoiNongDan.GetQueryString("maQuanHuyen", url); 
    $.get("/HoiVien/Dashboard/TongSoHoiVien", { maQuanHuyen: maQuanHuyen })
        .done(function (rs) {
            LoadChart(rs);
        }).fail(function () {
            alert("error");
        });
}
function LoadChart(data) {
    var colors = { 'sl': '#09ad95', 'themMoi': '#05c3fb', 'giam': '#6c5ffc', 'choDuyet': '#1170e4' };
    var names = { 'sl': 'Số HV', 'themMoi': 'Thêm mới', 'giam': 'Giảm',  'choDuyet': 'Chưa duyệt' }
    var categories = [], sl = ['sl'], giam = ['giam'], themMoi = ['themMoi'], choDuyet = ['choDuyet'];
    var tSL = 0, tTM = 0, tGiam = 0, tTM = 0, tCD = 0;
    let width = data.length > 20 ? 15 : data.length > 10 ? 20 : 30;
    for (var i = 0; i < data.length; i++) {
        let id 
        categories.push(data[i].ten);
        sl.push(data[i].sl);
        giam.push(data[i].giam);
        themMoi.push(data[i].themMoi);
        choDuyet.push(data[i].choDuyet);

        tSL = tSL + data[i].sl;
        tGiam = tGiam + data[i].giam;
        tTM = tTM + data[i].themMoi;
        tCD = tCD + data[i].choDuyet;
    }
    var chart = c3.generate({
        bindto: '#chart-bar', // id of chart wrapper
        data: {
            columns: [
                sl,
                themMoi,
                giam,
                choDuyet
            ],
            onclick: function (d, i) {
                let label = chart.categories()[d.index];
                console.log(label);
                let url = window.location.href;
                var maQuanHuyen = HoiNongDan.GetQueryString("maQuanHuyen", url);
                window.open("/HoiVien/HoiVien/LoadHoiVienOrg?MaQuanHuyen=" + maQuanHuyen + '&TenPhuongXa=' + label,"_blank");
                
               
            },
            type: 'bar', // default type of chart
            groups: [
                ['sl', 'themMoi','giam','choDuyet']
            ],
            colors: colors,
            names: names
        },
        axis: {
            x: {
                type: 'category',
                categories: categories,
            },
        },
        bar: {
            width: width
        },
        legend: {
            show: false, //hide legend
        },
        padding: {
            bottom: 0,
            top: 0
        }
    });
    var datapie = {
        labels: ['Số HV','Thêm mới', 'Giảm','Chưa duyệt'],
        datasets: [{
            data: [tSL, tGiam, tTM, tCD],
            backgroundColor: ['#09ad95', '#05c3fb', '#6c5ffc','#1170e4']
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
}