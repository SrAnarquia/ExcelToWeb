document.addEventListener("DOMContentLoaded", function () {
    const canvas = document.getElementById('interactionsByCategoryChart');
    if (!canvas) return;

    const rawData = canvas.dataset.chart;

    let categoryData;
    try {
        categoryData = JSON.parse(rawData);
    } catch (e) {
        console.error("Error parsing category chart data:", e);
        return;
    }

    const categories = categoryData.map(item => item.Category);
    const counts = categoryData.map(item => item.Count);

    const ctx = canvas.getContext('2d');

    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: categories,
            datasets: [{
                label: 'Interacciones',
                data: counts,
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
