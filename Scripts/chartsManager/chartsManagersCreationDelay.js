document.addEventListener("DOMContentLoaded", function () {
    const canvas = document.getElementById('monthlyDelaysChart');
    const rawData = canvas.dataset.chart;

    let monthlyData;
    try {
        monthlyData = JSON.parse(rawData);
    } catch (e) {
        console.error("Error parsing monthly delay data:", e);
        return;
    }

    const labels = monthlyData.map(item => item.MonthYear);
    const delays = monthlyData.map(item => item.Delay);

    const ctx = canvas.getContext('2d');
    new Chart(ctx, {
        type: 'bar', // Puedes cambiar a 'line' si prefieres líneas
        data: {
            labels: labels,
            datasets: [{
                label: 'Entregas con retraso',
                data: delays,
                backgroundColor: '#e63946',
                borderColor: '#c1121f',
                borderWidth: 1,
                borderRadius: 4,
                barPercentage: 0.6
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: { display: false },
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            return context.parsed.y + ' entregas con retraso';
                        }
                    }
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        precision: 0 // Evita decimales si no hay
                    }
                }
            }
        }
    });
});
