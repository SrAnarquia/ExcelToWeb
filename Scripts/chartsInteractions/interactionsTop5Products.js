document.addEventListener("DOMContentLoaded", function () {
    const canvas = document.getElementById('interactionsByProductsChart');
    if (!canvas) return;

    const rawData = canvas.dataset.chart;

    let productData;
    try {
        productData = JSON.parse(rawData);
    } catch (e) {
        console.error("Error parsing product chart data:", e);
        return;
    }

    const products = productData.map(item => item.Product);
    const counts = productData.map(item => item.Count);

    const ctx = canvas.getContext('2d');

    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: products,
            datasets: [{
                label: 'Interacciones',
                data: counts,
                backgroundColor: '#2a9d8f',
                hoverBackgroundColor: '#57c4b8',
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
                            return context.parsed.y.toLocaleString() + " interacciones";
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
                        beginAtZero: true,
                        callback: function (value) {
                            return value.toLocaleString();
                        }
                    }
                }
            }
        }
    });
});
