document.addEventListener("DOMContentLoaded", function () {
    const canvas = document.getElementById('monthlyProfitChart');
    const rawData = canvas.dataset.chart;

    let monthlyData;
    try {
        monthlyData = JSON.parse(rawData);
    } catch (e) {
        console.error("Error parsing monthly profit data:", e);
        return;
    }

    const labels = monthlyData.map(item => item.MonthYear);
    const profits = monthlyData.map(item => item.Profit);

    const ctx = canvas.getContext('2d');
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Ganancia ($)',
                data: profits,
                backgroundColor: 'rgba(75, 192, 192, 0.4)',
                borderColor: '#0077b6',
                tension: 0.3,
                fill: true,
                borderWidth: 2,
                pointRadius: 4
            }]
        },
        options: {
            responsive: true,
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
                y: {
                    ticks: {
                        callback: function (value) {
                            return '$' + value;
                        }
                    }
                }
            }
        }
    });
});
