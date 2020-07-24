import React, { useState, useEffect } from 'react';

import ApiService from "../../services/Api";

function Resumo() {
    const [resumo, setResumo] = useState({});

    useEffect(() => {
        const fetchData = async () => {
            const response = await ApiService.get('/resumos');

            setResumo(response.data);
        }

        fetchData();
    }, []);

    function formatCurrency(value) {
        if (value === null)
            return 0;

        let result = value.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
        return result;
    }

    function formatPercent(value) {
        if (value === null)
            return 0;

        let result = value.toFixed(2) + '%';
        return result;
    }

    return (
        <div className="xl:col-span-3">
            <div className="bg-gray-100 border border-gray-800 rounded shadow p-2 mb-3 text-center">
                <h3 className="font-bold text-2xl text-gray-800">Data da última atualização {resumo.dataDaUltimaAtualizacao}</h3>
            </div>

            <div className="grid grid-cols-1 grid-cols-1 xl:grid-cols-2 gap-1 pb-0 ">
                <div className="bg-gray-100 border border-gray-800 rounded shadow p-2">
                    <div className="flex flex-row items-center">
                        <div className="flex-shrink pr-4">
                            <div className="rounded p-3 bg-green-600"><i className="fas fa-money-check-alt fa-3x fa-fw fa-inverse"></i></div>
                        </div>
                        <div className="flex-1 text-right md:text-center">
                            <h5 className="font-bold uppercase text-blue-800 text-right">Total Investido</h5>
                            <h3 className="font-bold text-2xl xl:text-4xl text-gray-800 text-right">{formatCurrency(resumo.valorTotalInvestido)}</h3>
                        </div>
                    </div>
                </div>

                <div className="bg-gray-100 border border-gray-800 rounded shadow p-2">
                    <div className="flex flex-row items-center">
                        <div className="flex-shrink pr-4">
                            <div className="rounded p-3 bg-orange-600"><i className="far fa-money-bill-alt fa-3x fa-fw fa-inverse"></i></div>
                        </div>
                        <div className="flex-1 text-right md:text-center">
                            <h5 className="font-bold uppercase text-blue-800 text-right">Valor Atual</h5>
                            <h3 className={"font-bold text-2xl xl:text-4xl text-right " + ((resumo.valorTotalInvestido > resumo.valorAtualDaCarteiral) ? "text-red-500" : "text-green-500")}>
                                {formatCurrency(resumo.valorAtualDaCarteiral)}
                            </h3>
                        </div>
                    </div>
                </div>

                <div className="bg-gray-100 border border-gray-800 rounded shadow p-2">
                    <div className="flex flex-row items-center">
                        <div className="flex-shrink pr-4">
                            <div className="rounded p-3 bg-yellow-600"><i className="fas fa-piggy-bank fa-3x fa-fw fa-inverse"></i></div>
                        </div>
                        <div className="flex-1 text-right md:text-center">
                            <h5 className="font-bold uppercase text-blue-800 text-right">Saldo ({formatPercent(resumo.saldoAtualDaCarteiralEmPercentual)})</h5>
                            <h3 className={"font-bold text-2xl xl:text-4xl text-right " + ((resumo.saldoAtualDaCarteiral < 0) ? "text-red-500" : "text-green-500")}>
                                {formatCurrency(resumo.saldoAtualDaCarteiral)}
                            </h3>
                        </div>
                    </div>
                </div>

                <div className="bg-gray-100 border border-gray-800 rounded shadow p-2">
                    <div className="flex flex-row items-center">
                        <div className="flex-shrink pr-4">
                            <div className="rounded p-3 bg-blue-600"><i className="fas fa-coins fa-3x fa-fw fa-inverse"></i></div>
                        </div>
                        <div className="flex-1 text-right md:text-center">
                            <h5 className="font-bold uppercase text-blue-800 text-right">Saldo Liquido ({formatPercent(resumo.liquidezAtualDaCarteiralEmPercentual)})</h5> 
                            <h3 className={"font-bold text-2xl xl:text-4xl text-right " + ((resumo.liquidezAtualDaCarteiral < 0) ? "text-red-500" : "text-green-500")}>
                                {formatCurrency(resumo.liquidezAtualDaCarteiral)}
                            </h3>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    )
}

export default Resumo;
