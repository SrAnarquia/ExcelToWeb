document.addEventListener("DOMContentLoaded", function () {
    const canvas = document.getElementById('topProductsChart');
    const rawData = canvas.dataset.chart;

    let CategorySalesData;
    try {
        CategorySalesData = JSON.parse(rawData);
    } catch (e) {
        console.error("Error parsing chart data:", e);
        return;
    }

    const category = CategorySalesData.map(item => item.Category);
    const sales = CategorySalesData.map(item => item.Sales);

    const ctx = canvas.getContext('2d');

    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: category,
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
