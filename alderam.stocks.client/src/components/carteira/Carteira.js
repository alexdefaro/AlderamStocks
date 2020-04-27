import React, { useState, useEffect } from 'react';

import Api from "../../services/Api";

function Carteira() {
    const [operacoes, setOperacoes] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            const response = await Api.get('/carteira');
            setOperacoes(response.data);
        }

        fetchData();
    }, []);

    function formatCurrency(value) {
        let result = Math.abs(value).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
        return result;
    }

    return (
        <React.Fragment>
            <h3 className="p-3 text-3xl">Carteira</h3>

            <div className=" w-full mt-2 p-1">
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
                                </tr>
                            </thead>
                            <tbody>
                                {
                                    operacoes.map(operacao => (
                                        <tr key={operacao.codigoDoAtivo} >
                                            <td className="border xl:px-4 py-1 text-left">{operacao.codigoDoAtivo}</td>
                                            <td className="border px-4 py-1 hidden xl:table-cell text-left">{operacao.nomeDoAtivo}</td>
                                            <td className="border px-4 py-1 text-right hidden xl:table-cell">{operacao.quantitidade}</td>
                                            <td className="border px-4 py-1 text-right hidden xl:table-cell">{formatCurrency(operacao.precoMedioCompra)}</td>
                                            <td className="border px-4 py-1 text-right hidden xl:table-cell">{formatCurrency(operacao.valorDaOperacao)}</td>
                                            <td className="border px-4 py-1 text-right hidden xl:table-cell">{formatCurrency(operacao.precoAtual)}</td>
                                            <td className="border px-4 py-1 text-right">{formatCurrency(operacao.valorAtual)}</td>
                                            <td className={"border px-4 py-1 text-right " + (operacao.rentabilidade < 0 ? "text-red-500" : "text-green-500")}>{formatCurrency(operacao.rentabilidade)}</td>
                                        </tr>
                                    ))
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
