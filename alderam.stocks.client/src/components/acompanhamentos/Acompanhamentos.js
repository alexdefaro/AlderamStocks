import React, { useState, useEffect } from 'react';
import Modal from 'react-modal';

import ApiService from "../../services/Api";
import Toast from "../../services/Toast";

Modal.setAppElement('#root');

const customStyles = {
    overlay: {
        backgroundcolor: 'rgba(255, 255, 255, 0.2)'
    },
    content: {
        top: '50%',
        left: '50%',
        right: 'auto',
        bottom: 'auto',
        transform: 'translate(-50%, -50%)'
    }
};

function Acompanhamentos() {
    const [ativos, setAtivos] = useState([]);
    const [modalIsOpen, setModalIsOpen] = useState(false);
    const [dadosDoAtivo, setDadosDoAtivo] = useState({
        id: 0,
        codigoDoAtivo: '',
        nomeDoAtivo: '',
        precoDeCompra: ''
    });

    useEffect(() => {
        fetchData();
    }, []);

    const fetchData = async () => {
        const response = await ApiService.get('/acompanhamentos');

        setAtivos(response.data);
    }

    function formatCurrency(value) {
        value = value ?? 0;

        let result = value.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
        return result;
    }

    function handleChangeDadosDoAtivo(e) {
        setDadosDoAtivo({
            ...dadosDoAtivo,
            [e.target.name]: e.target.value
        });
    }

    async function handleRemoveClick(e, id) {
        e.preventDefault();
        try {
            await ApiService.remove('/acompanhamentos/' + id);
            fetchData();

            Toast.success('Ativo removido.')
        } catch (e) {
            Toast.error('Erro ao exclir registro.')
        }
    }

    async function handleEditClick(e, ativo) {
        e.preventDefault();

        setDadosDoAtivo(ativo);
        setModalIsOpen(true);
    }

    async function handleSaveAcompanhamentoClick(e) {
        e.preventDefault();

        try {
            const data = {
                id: dadosDoAtivo.id,
                codigoDoAtivo: dadosDoAtivo.codigoDoAtivo,
                nomeDoAtivo: dadosDoAtivo.nomeDoAtivo,
                precoDeCompra: dadosDoAtivo.precoDeCompra
            }

            if (data.id > 0) {
                await ApiService.put('/acompanhamentos/' + data.id, data);
            }
            else { 
                await ApiService.post('/acompanhamentos', data);
            }

            fetchData();

            Toast.success('Ativo adicionado.')

            setModalIsOpen(false);
        } catch (e) {
            Toast.error('Erro ao adicionar registro.')
        }
    }

    return (
        <section id="section-acompanhamentos" className="xl:col-span-2">
            <div className="bg-gray-100 border border-gray-800 rounded shadow p-0 h-full">
                <div className="flex flex-row items-center p-1">
                    <Modal
                        isOpen={modalIsOpen}
                        onRequestClose={() => setModalIsOpen(false)}
                        style={customStyles}>

                        <form onSubmit={handleSaveAcompanhamentoClick}>
                            <h2 className="mb-10 text-2xl">Incluir ativo na lista de acompanhamento</h2>
                            <label>Codigo do ativo</label>
                            <input name="codigoDoAtivo" className="border border-gray-300 rounded-lg py-2 px-4 block w-full" type="text" value={dadosDoAtivo.codigoDoAtivo} onChange={handleChangeDadosDoAtivo} />

                            <label>Nome do ativo</label>
                            <input name="nomeDoAtivo" className="border border-gray-300 rounded-lg py-2 px-4 block w-full" type="text" value={dadosDoAtivo.nomeDoAtivo} onChange={handleChangeDadosDoAtivo} />

                            <label>Preço de compra</label>
                            <input name="precoDeCompra" className="border border-gray-300 rounded-lg py-2 px-4 block w-full" type="number" value={dadosDoAtivo.precoDeCompra} onChange={handleChangeDadosDoAtivo} />

                            <div className="w-full mt-2 p-1 mt-5">
                                <div className="grid grid-cols-1 grid-cols-1 xl:grid-cols-5 gap-2 pb-0 ">
                                    <button className="rounded p-3 bg-green-600 w-20"><i className="fas fa-save fa-3x fa-fw fa-inverse"></i></button>
                                    <button className="rounded p-3 bg-orange-600 w-20" onClick={() => setModalIsOpen(false)}><i className="far fa-window-close fa-3x fa-fw fa-inverse"></i></button>
                                </div>
                            </div>
                        </form>

                    </Modal>

                    <table className="table-auto w-full">
                        <thead>
                            <tr>
                                <th className="px-2 py-1 text-left">Ativo</th>
                                <th className="px-2 py-1 text-right">Atual</th>
                                <th className="px-2 py-1 text-right">Compra</th>
                                <th className="w-20 xl:w-auto px-1 py-1 text-center"><a href="#/" onClick={() => setModalIsOpen(true)} ><i className="fas fa-search-plus mr-1" title="Adicionar ativo a lista" /></a></th>
                            </tr>
                        </thead>
                        <tbody>
                            {
                                ativos.map(ativo => (
                                    <tr key={ativo.id} >
                                        <td className="border px-2 py-1 text-left">
                                            <a href={"https://br.tradingview.com/chart/?symbol=BMFBOVESPA:" + ativo.codigoDoAtivo} target="new">{ativo.codigoDoAtivo}</a>
                                        </td>

                                        <td className="border text-right">
                                            {formatCurrency(ativo.precoAtual)}
                                            <i className={"fas ml-2 " + ((ativo.precoAtual > ativo.precoAnterior) ? "fa-caret-down text-red-500" : "fa-caret-up text-green-500")}></i>
                                        </td>
                                        <td className="border py-1 text-right">
                                            {formatCurrency(ativo.precoDeCompra)}
                                            {(ativo.comprar) && <i className="far fa-bell text-red-500 float-left" />}
                                        </td>

                                        <td className="border text-center">
                                            <a href="#/" onClick={(e) => handleEditClick(e, ativo)}><i className=" far fa-edit mr-1" title="Alterar este ativo" /></a>
                                            <a href="#/" onClick={(e) => handleRemoveClick(e, ativo.id)}><i className="far fa-trash-alt" title="Remover ativo da lista" /></a>
                                        </td>
                                    </tr>
                                ))
                            }
                        </tbody>
                    </table>
                </div>
            </div>
        </section>
    )
}

export default Acompanhamentos;
