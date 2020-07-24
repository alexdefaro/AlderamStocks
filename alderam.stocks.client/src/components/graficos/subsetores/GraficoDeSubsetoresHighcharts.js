// https://www.highcharts.com/demo

import React, { useState, useEffect } from 'react';

import { default as Drilldown } from 'highcharts/modules/drilldown';
import Highcharts from 'highcharts';
import HighchartsReact from 'highcharts-react-official';
import Api from "../../../services/Api";

Drilldown(Highcharts);

function GraficoDeSubsetoresHighcharts(props) {
    const [dadosDoGrafico, setDadosDoGrafico] = useState({});
    const tipoDeInvestimento = props.tipoDeInvestimento;

    function formatCurrency(value) {
        let result = value.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
        return result;
    }

    useEffect(() => {
        const fetchData = async () => {
            const response = await Api.get('/graficodesetores?tipoDeInvestimento=' + tipoDeInvestimento, {
            });

            let dadosDoGraficoFormatados = response.data.labels.map(
                function (item, index) {
                    return [item, response.data.values[index]]
                }
            );

            setDadosDoGrafico(
                dadosDoGraficoFormatados
            );
        }

        fetchData();
    }, [tipoDeInvestimento]);

    const chartOptions = {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie',

        },
        credits: {
            enabled: false
        },
        title: {
            text: 'Distribuíção de ativos por subsetores (' + ((tipoDeInvestimento === 1) ? 'Açoes' : 'Fundos Imobiliários') +')'
        },
        tooltip: {
            pointFormatter: function () {
                let value = formatCurrency(this.y);
                return '<span><b> ' + value + ' </b></span>'
            }

        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    useHTML: true,
                    formatter: function () {
                        let value = formatCurrency(this.y);
                        return this.key
                            + '<br> '
                            + this.percentage.toFixed(2) + '%'
                            + '<br> '
                            + value;
                    }
                }
            }
        },
        series: [
            {
                name: "Setores",
                colorByPoint: true,
                data: dadosDoGrafico
            }
        ]
    };

    return (
        <div className="mt-10 w-full text-center">
            <div className="inline-block">
                <HighchartsReact
                    highcharts={Highcharts}
                    options={chartOptions}
                />
            </div>
        </div>
    )
}

export default GraficoDeSubsetoresHighcharts;
