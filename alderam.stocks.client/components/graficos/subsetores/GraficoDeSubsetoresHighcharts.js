'use client';

import { useState, useEffect } from 'react';
import Highcharts from 'highcharts';
import HighchartsReact from 'highcharts-react-official';
import ApiService from '@/services/Api';

export default function GraficoDeSubsetoresHighcharts({ tipoDeInvestimento }) {
  const [chartData, setChartData] = useState([]);

  useEffect(() => {
    ApiService.get('/graficodesetores?tipoDeInvestimento=' + tipoDeInvestimento).then(r => {
      setChartData(
        r.data.labels.map((label, i) => [label, r.data.values[i]])
      );
    });
  }, [tipoDeInvestimento]);

  function formatCurrency(value) {
    return value.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
  }

  const chartOptions = {
    chart: { plotBackgroundColor: null, plotBorderWidth: null, plotShadow: false, type: 'pie' },
    credits: { enabled: false },
    title: {
      text: `Distribuíção de ativos por subsetores (${tipoDeInvestimento === 1 ? 'Ações' : 'Fundos Imobiliários/Agro'})`,
    },
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
