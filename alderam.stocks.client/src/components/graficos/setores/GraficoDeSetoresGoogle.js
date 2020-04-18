import React, { useState, useEffect } from 'react';
import { Chart } from "react-google-charts";

import Api from "../../../services/Api";

function GraficoDeSetoresGoogle() {
    const [dadosDoGrafico, setDadosDoGrafico] = useState({});

    useEffect(() => {
        const fetchData = async () => {
            const response = await Api.get('/GraficoDeSetores');

            let dadosDoGraficoFormatados = response.data.labels.map(
                function (item, index) {
                    return [item, response.data.values[index]]
                }
            );

            dadosDoGraficoFormatados.unshift(['Setor', 'Total']);
            
            setDadosDoGrafico(
                dadosDoGraficoFormatados
            );
        }

        fetchData();
    }, []);


    const chartOptions = {
        title: '',
        pieSliceText: 'percent',
        legend: {
            position: 'labeled',
            labeledValueText: 'value'
        },
        chartArea: {
            left: "0%",
            top: "0%",
            height: "100%",
            width: "100%"
        }
    };

    const chartFormatters = [
        {
            type: "NumberFormat",
            column: 1,
            options: {
                prefix: "R$ ",
                fractionDigits: 2,
                //suffix: "%",
                negativeColor: "red",
                negativeParens: true,
                groupingSymbol: '.',
                decimalSymbol: ','
            },
        }
    ]

    return (
        <div className="mt-5 w-full text-center">
            <h3 className="mb-10 text-2xl">Distribuíção de ativos por setores google</h3>
            <div className="inline-block">
                <Chart
                    width={'1000px'}
                    height={'500px'}
                    chartType="PieChart"
                    loader={<div>Aguarde...</div>}
                    data={dadosDoGrafico}
                    options={chartOptions}
                    formatters={chartFormatters}
                />
            </div>
        </div>
    )
}

export default GraficoDeSetoresGoogle;
