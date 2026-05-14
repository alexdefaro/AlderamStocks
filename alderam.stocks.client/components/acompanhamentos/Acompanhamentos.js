'use client';

import { useState, useEffect } from 'react';
import Modal from 'react-modal';
import ApiService from '@/services/Api';
import Toast from '@/services/Toast';

const modalStyles = {
  overlay: { backgroundColor: 'rgba(0, 0, 0, 0.5)' },
  content: {
    top: '50%',
    left: '50%',
    right: 'auto',
    bottom: 'auto',
    transform: 'translate(-50%, -50%)',
  },
};

const emptyAtivo = { id: 0, codigoDoAtivo: '', nomeDoAtivo: '', precoDeCompra: '' };

export default function Acompanhamentos() {
  const [ativos, setAtivos] = useState([]);
  const [modalIsOpen, setModalIsOpen] = useState(false);
  const [dadosDoAtivo, setDadosDoAtivo] = useState(emptyAtivo);

  useEffect(() => {
    if (typeof window !== 'undefined') {
      Modal.setAppElement('body');
    }
    fetchData();
  }, []);

  async function fetchData() {
    const response = await ApiService.get('/acompanhamentos');
    setAtivos(response.data);
  }

  function formatCurrency(value) {
    return (value ?? 0).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
  }

  function handleChange(e) {
    setDadosDoAtivo(prev => ({ ...prev, [e.target.name]: e.target.value }));
  }

  async function handleRemove(e, id) {
    e.preventDefault();
    try {
      await ApiService.remove('/acompanhamentos/' + id);
      fetchData();
      Toast.success('Ativo removido.');
    } catch {
      Toast.error('Erro ao excluir registro.');
    }
  }

  function handleEdit(e, ativo) {
    e.preventDefault();
    setDadosDoAtivo(ativo);
    setModalIsOpen(true);
  }

  async function handleSave(e) {
    e.preventDefault();
    try {
      if (dadosDoAtivo.id > 0) {
        await ApiService.put('/acompanhamentos/' + dadosDoAtivo.id, dadosDoAtivo);
      } else {
        await ApiService.post('/acompanhamentos', dadosDoAtivo);
      }
      fetchData();
      Toast.success('Ativo salvo.');
      setModalIsOpen(false);
      setDadosDoAtivo(emptyAtivo);
    } catch {
      Toast.error('Erro ao salvar registro.');
    }
  }

  function openNew() {
    setDadosDoAtivo(emptyAtivo);
    setModalIsOpen(true);
  }

  return (
    <section id="section-acompanhamentos" className="xl:col-span-2">
      <div className="bg-gray-100 border border-gray-800 rounded shadow p-0 h-full">
        <div className="flex flex-row items-center p-1">
          <Modal isOpen={modalIsOpen} onRequestClose={() => setModalIsOpen(false)} style={modalStyles}>
            <form onSubmit={handleSave}>
              <h2 className="mb-10 text-2xl">Incluir ativo na lista de acompanhamento</h2>

              <label>Codigo do ativo</label>
              <input name="codigoDoAtivo" className="border border-gray-300 rounded-lg py-2 px-4 block w-full"
                type="text" value={dadosDoAtivo.codigoDoAtivo} onChange={handleChange} />

              <label>Nome do ativo</label>
              <input name="nomeDoAtivo" className="border border-gray-300 rounded-lg py-2 px-4 block w-full"
                type="text" value={dadosDoAtivo.nomeDoAtivo} onChange={handleChange} />

              <label>Preço de compra</label>
              <input name="precoDeCompra" className="border border-gray-300 rounded-lg py-2 px-4 block w-full"
                type="number" value={dadosDoAtivo.precoDeCompra} onChange={handleChange} />

              <div className="w-full mt-5 p-1">
                <div className="grid grid-cols-1 xl:grid-cols-5 gap-2 pb-0">
                  <button type="submit" className="rounded p-3 bg-green-600 w-20">
                    <i className="fas fa-save fa-3x fa-fw fa-inverse"></i>
                  </button>
                  <button type="button" className="rounded p-3 bg-orange-600 w-20" onClick={() => setModalIsOpen(false)}>
                    <i className="far fa-window-close fa-3x fa-fw fa-inverse"></i>
                  </button>
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
                <th className="w-20 xl:w-auto px-1 py-1 text-center">
                  <a href="#/" onClick={e => { e.preventDefault(); openNew(); }}>
                    <i className="fas fa-search-plus mr-1" title="Adicionar ativo a lista" />
                  </a>
                </th>
              </tr>
            </thead>
            <tbody>
              {ativos.map(ativo => (
                <tr key={ativo.id}>
                  <td className="border px-2 py-1 text-left">
                    <a href={`https://br.tradingview.com/chart/?symbol=BMFBOVESPA:${ativo.codigoDoAtivo}`} target="new">
                      {ativo.codigoDoAtivo}
                    </a>
                  </td>
                  <td className="border text-right">
                    {formatCurrency(ativo.precoAtual)}
                    <i className={`fas ml-2 ${ativo.precoAtual > ativo.precoAnterior ? 'fa-caret-down text-red-500' : 'fa-caret-up text-green-500'}`} />
                  </td>
                  <td className="border py-1 text-right">
                    {formatCurrency(ativo.precoDeCompra)}
                    {ativo.comprar && <i className="far fa-bell text-red-500 float-left" />}
                  </td>
                  <td className="border text-center">
                    <a href="#/" onClick={e => handleEdit(e, ativo)}>
                      <i className="far fa-edit mr-1" title="Alterar este ativo" />
                    </a>
                    <a href="#/" onClick={e => handleRemove(e, ativo.id)}>
                      <i className="far fa-trash-alt" title="Remover ativo da lista" />
                    </a>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </section>
  );
}
