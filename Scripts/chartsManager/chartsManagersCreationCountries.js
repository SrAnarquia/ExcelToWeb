document.addEventListener("DOMContentLoaded", function () {
    const canvas = document.getElementById('topCountriesChart');
    const rawData = canvas.dataset.chart;

    let countrySalesData;
    try {
        countrySalesData = JSON.parse(rawData);
    } catch (e) {
        console.error("Error parsing chart data:", e);
        return;
    }

    const countries = countrySalesData.map(item => item.Country);
    const sales = countrySalesData.map(item => item.Sales);

    const ctx = canvas.getContext('2d');

    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: countries,
            datasets: [{
                label: 'Ventas ($)',
                data: sales,
                backgroundColor: '#1d3557',
                hoverBackgroundColor: '#457b9d',
                borderRadius: 5,
                barPercentage: 0.6
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { display: false },
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            return '$' + context.parsed.y.toLocaleString();
                        }
                    }
                }
            },
            scales: {
                x: {
                    ticks: {
                        color: '#343a40',
                        font: { weight: '500' }
                    }
                },
                y: {
                    ticks: {
                        color: '#495057',
                        callback: function (value) {
                            return '$' + value;
                        }
                    }
                }
            }
        }
    });
});
