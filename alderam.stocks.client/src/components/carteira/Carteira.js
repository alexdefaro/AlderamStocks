import React, { useState, useEffect } from 'react';

import ApiService from "../../services/Api";

function Carteira() {
    const [dataLimite, setDataLimite] = useState(new Date());
    const [operacoes, setOperacoes] = useState([]);

    var tipoDeInvestimento = 0;

    useEffect(() => {
        fetchData(dataLimite);
    }, []);

    useEffect(() => {
        fetchData(dataLimite);
    }, [dataLimite]);

    async function fetchData(dataLimite) {
        let params = {
            dataLimite: dataLimite
        }
        const response = await ApiService.get('/carteira', params);
        setOperacoes(response.data);
    }

    function handleDataLimiteChange(e) {
        setDataLimite(new Date(e.target.value));
    }

    function formatCurrency(value) {
        let result = Math.abs(value).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
        return result;
    }

    return (
        <React.Fragment> 
            <div className="inline-flex">
                <h3 className="flex-1 p-3 text-3xl">Carteira</h3>
                <input id="InputDataLimite" type="date" className="flex-1" value={dataLimite.toISOString().split('T')[0]} onChange={handleDataLimiteChange} ></input>
            </div>

            <div className="w-full mt-2 p-1">
                <div className="bg-gray-100 border border-gray-800 rounded shadow p-0">
                    <div className="flex flex-row items-center p-1">
                        <table className="table-auto w-full">
                            <thead>
                                <tr>
                                    <th className="xl:px-4 py-1 text-left">Codigo</th>
                                    <th className="px-4 py-1 hidden xl:table-cell text-left">Nome</th>
                                    <th className="px-4 py-1 hidden xl:table-cell text-right">Quantidade</th>
                                    <th className="px-4 py-1 hidden xl:table-cell text-right">Preço Médio</th>
                                    <th className="px-4 py-1 hidden xl:table-cell text-right">Valor Médio</th>
                                    <th className="px-4 py-1 hidden xl:table-cell text-right">Preço Atual</th>
                                    <th className="px-4 py-1 text-right">Valor Atual</th>
                                    <th className="px-4 py-1 text-right">Rentabilidade</th>
                                    <th className="px-4 py-1 text-left">Subsetor</th>
                                </tr>
                            </thead>
                            <tbody>
                                {
                                    operacoes.map((operacao) => {
                                        var printSubTitle = false; 

                                        if (tipoDeInvestimento !== operacao.tipoDeInvestimento) {
                                            tipoDeInvestimento = operacao.tipoDeInvestimento;
                                            printSubTitle = true;
                                        }

                                        return (
                                            <React.Fragment key={operacao.codigoDoAtivo}>
                                                {
                                                    (printSubTitle) &&
                                                        <tr>
                                                            <th colSpan="9" className="px-4 py-1 xl:table-cell text-left">{((operacao.tipoDeInvestimento) === 1 ? "Ações" : "Fundos Imobiliários")}</th>
                                                        </tr>
                                                }

                                                <tr>
                                                    <td className="border xl:px-4 py-1 text-left">
                                                        <a href={"https://br.tradingview.com/chart/?symbol=BMFBOVESPA:" + operacao.codigoDoAtivo} target="new">{operacao.codigoDoAtivo}</a>
                                                    </td>
                                                    <td className="border px-4 py-1 hidden xl:table-cell text-left">{operacao.nomeDoAtivo.slice(0, 20) + "..."}</td>
                                                    <td className="border px-4 py-1 text-right hidden xl:table-cell">{operacao.quantitidade}</td>
                                                    <td className="border px-4 py-1 text-right hidden xl:table-cell">{formatCurrency(operacao.precoMedioCompra)}</td>
                                                    <td className="border px-4 py-1 text-right hidden xl:table-cell">{formatCurrency(operacao.valorDaOperacao)}</td>
                                                    <td className="border px-4 py-1 text-right hidden xl:table-cell">
                                                        {(operacao.comprar) && <i className="far fa-bell text-red-500 float-left" />}
                                                        {formatCurrency(operacao.precoAtual)}
                                                        <i className={"fas ml-2 " + ((operacao.precoAtual > operacao.precoAnterior) ? "fa-caret-down text-red-500" : "fa-caret-up text-green-500")}></i>
                                                    </td>
                                                    <td className="border px-4 py-1 text-right">{formatCurrency(operacao.valorAtual)}</td>
                                                    <td className={"border px-4 py-1 text-right " + (operacao.rentabilidade < 0 ? "text-red-500" : "text-green-500")}>{formatCurrency(operacao.rentabilidade)}</td>
                                                    <td className="border px-4 py-1 text-left">{operacao.nomeDoSetor.slice(0, 20) + "..."}</td>
                                                </tr>
                                            </React.Fragment>
                                        )
                                    }
                                    )
                                }
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </React.Fragment>
    )
}

export default Carteira;
