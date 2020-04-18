import React, { useState, useEffect } from 'react';

import Api from "../../services/Api";
import Toast from "../../services/Toast";

function Acompanhamentos() {
    const [ativos, setAtivos] = useState([]);

    useEffect(() => {
        fetchData();
    }, []);

    const fetchData = async () => {
        const response = await Api.get('/Acompanhamentos');
        setAtivos(response.data);
    }

    function formatCurrency(value) {
        value = value ?? 0;

        let result = value.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
        return result;
    }

    async function handleNewClick(e) {
        e.preventDefault();
        try {
            const data = {
                codigoDoAtivo: 'DELME',
                nomeDoAtivo: 'DELME',
                precoDeCompra: 9.00
            }

            await Api.post('/Acompanhamentos', data);
            fetchData();

            Toast.success('Ativo adicionado.')
        } catch (e) {
            Toast.error('Erro ao adicionar registro.')
        }
    }

    async function handleRemoveClick(e, id) {
        e.preventDefault();
        try {
            await Api.delete('/Acompanhamentos/' + id);
            fetchData();

            Toast.success('Ativo removido.')
        } catch (e) {
            Toast.error('Erro ao exclir registro.')
        }
    }

    async function handleEditClick(e, ativo) {
        e.preventDefault();
        try {
            const data = {
                id: ativo.id,
                codigoDoAtivo: ativo.codigoDoAtivo,
                nomeDoAtivo: ativo.nomeDoAtivo,
                precoDeCompra: ativo.precoDeCompra
            }

            await Api.put('/Acompanhamentos/' + ativo.id, data);
            fetchData();

            Toast.success('Ativo alterado.')
        } catch (e) {
            Toast.error('Erro ao alterar registro.')
        }
    }

    return (
        <div className="xl:col-span-2">
            <div className="bg-gray-100 border border-gray-800 rounded shadow p-0">
                <div className="flex flex-row items-center p-1">
                    <table className="table-auto w-full">
                        <thead>
                            <tr>
                                <th className="px-4 py-1 text-left">Ativo</th>
                                <th className="px-4 py-1 text-right">Atual</th>
                                <th className="px-4 py-1 text-right">Compra</th>
                                <th className="px-1 py-1 text-center"><a href="#" onClick={handleNewClick} ><i className="fas fa-search-plus mr-1" title="Adicionar ativo a lista" /></a></th>
                            </tr>
                        </thead>
                        <tbody>
                            {
                                ativos.map(ativo => (
                                    <tr key={ativo.id} >
                                        <td className="border px-4 py-1 text-left">{ativo.codigoDoAtivo}</td>
                                        <td className="border px-4 py-1 text-right">
                                            {formatCurrency(ativo.precoAtual)}
                                            <i className={"fas ml-2 " + ((ativo.precoAtual > ativo.precoAnterior) ? "fa-caret-down text-red-500" : "fa-caret-up text-green-500")}></i>
                                        </td>
                                        <td className="border px-4 py-1 text-right">
                                            {formatCurrency(ativo.precoDeCompra)}
                                            {(ativo.comprar) && <i className="far fa-bell text-red-500 float-left" />}
                                        </td>
                                        <td className="border text-center">
                                            <a href="#" onClick={(e) => handleEditClick(e, ativo)}><i className=" far fa-edit mr-1" title="Alterar este ativo" /></a>
                                            <a href="#" onClick={(e) => handleRemoveClick(e, ativo.id)}><i className="far fa-trash-alt" title="Remover ativo da lista" /></a>
                                        </td>
                                    </tr>
                                ))
                            }
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    )
}

export default Acompanhamentos;
