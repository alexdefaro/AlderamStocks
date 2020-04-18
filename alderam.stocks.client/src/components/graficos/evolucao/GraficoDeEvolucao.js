// https://www.highcharts.com/demo

import React, { useState, useEffect } from 'react';

import { default as Drilldown } from 'highcharts/modules/drilldown';
import Highcharts from 'highcharts';
import HighchartsReact from 'highcharts-react-official';
import Api from "../../../services/Api";

Drilldown(Highcharts);

function GraficoDeEvolucao() {
    const [dadosDoGrafico, setDadosDoGrafico] = useState({});

    function formatCurrency(value) {
        let result = value.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
        return result;
    }

    useEffect(() => {
        const fetchData = async () => {
            const response = await Api.get('/GraficoDeSetores');

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
    }, []);


    const gradientColors = Highcharts.map(Highcharts.getOptions().colors, function (color) {
        return {
            radialGradient: {
                cx: 0.5,
                cy: 0.3,
                r: 0.7
            },
            stops: [
                [0, color],
                [1, Highcharts.color(color).brighten(-0.3).get('rgb')] // darken
            ]
        };
    });

    const chartOptions = {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie',
            height: 700,
            width: 1000
        },
        title: {
            text: 'Distribuíção de ativos por setores'
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
        //colors:
        //    gradientColors,
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    distance: '50%',
                    useHTML: true,
                    formatter: function () {
                        console.log(this);
                        let point = this.point;
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

export default GraficoDeEvolucao;
