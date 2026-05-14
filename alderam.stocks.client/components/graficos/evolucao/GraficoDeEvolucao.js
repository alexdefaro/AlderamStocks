'use client';

import { useState, useEffect } from 'react';
import Highcharts from 'highcharts';
import HighchartsReact from 'highcharts-react-official';
import ApiService from '@/services/Api';

export default function GraficoDeEvolucao() {
  const [chartData, setChartData] = useState([]);

  useEffect(() => {
    ApiService.get('/graficodesetores').then(r => {
      setChartData(
        r.data.labels.map((label, i) => [label, r.data.values[i]])
      );
    });
  }, []);

  function formatCurrency(value) {
    return value.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
  }

  const chartOptions = {
    chart: { plotBackgroundColor: null, plotBorderWidth: null, plotShadow: false, type: 'pie', height: 700, width: 1000 },
    credits: { enabled: false },
    title: { text: 'Distribuíção de ativos por setores' },
    tooltip: {
      pointFormatter() {
        return `<span><b> ${formatCurrency(this.y)} </b></span>`;
      },
    },
    accessibility: { point: { valueSuffix: '%' } },
    plotOptions: {
      pie: {
        allowPointSelect: true,
        cursor: 'pointer',
        dataLabels: {
          enabled: true,
          distance: '50%',
          useHTML: true,
          formatter() {
            return `${this.key}<br> ${this.percentage.toFixed(2)}%<br> ${formatCurrency(this.y)}`;
          },
        },
      },
    },
    series: [{ name: 'Setores', colorByPoint: true, data: chartData }],
  };

  return (
    <div className="mt-10 w-full text-center">
      <div className="inline-block">
        <HighchartsReact highcharts={Highcharts} options={chartOptions} />
      </div>
    </div>
  );
}
