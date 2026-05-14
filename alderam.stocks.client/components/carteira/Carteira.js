'use client';

import { useState, useEffect } from 'react';
import ApiService from '@/services/Api';

export default function Carteira() {
  const [dataLimite, setDataLimite] = useState(new Date());
  const [operacoes, setOperacoes] = useState([]);

  useEffect(() => {
    fetchData(dataLimite);
  }, [dataLimite]);

  async function fetchData(date) {
    const response = await ApiService.get('/carteira', { dataLimite: date });
    setOperacoes(response.data);
  }

  function handleDataLimiteChange(e) {
    setDataLimite(new Date(e.target.value));
  }

  function formatCurrency(value) {
    return Math.abs(value).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
  }

  let currentTipo = 0;

  return (
    <section id="section-carteira">
      <div className="w-full">
        <div className="grid grid-cols-1 xl:grid-cols-5 gap-2 pb-0">
          <div className="xl:col-span-4">
            <h3 className="pt-3 pl-3 text-3xl">Carteira</h3>
          </div>
          <div className="xl:col-span-1 text-right">
            <input
              type="date"
              className="pt-6 pr-3"
              value={dataLimite.toISOString().split('T')[0]}
              onChange={handleDataLimiteChange}
            />
          </div>
        </div>
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
                {operacoes.map((operacao) => {
                  let printSubTitle = false;
                  if (currentTipo !== operacao.tipoDeInvestimento) {
                    currentTipo = operacao.tipoDeInvestimento;
                    printSubTitle = true;
                  }
                  const subtitle =
                    operacao.tipoDeInvestimento === 1
                      ? 'Ações'
                      : operacao.tipoDeInvestimento === 2
                      ? 'Fundos Imobiliários/Agro'
                      : 'Indices';

                  return (
                    <>
                      {printSubTitle && (
                        <tr key={`header-${operacao.tipoDeInvestimento}`}>
                          <th colSpan="9" className="px-4 py-1 text-left">{subtitle}</th>
                        </tr>
                      )}
                      <tr key={operacao.codigoDoAtivo}>
                        <td className="border xl:px-4 py-1 text-left">
                          <a href={`https://br.tradingview.com/chart/?symbol=BMFBOVESPA:${operacao.codigoDoAtivo}`} target="new">
                            {operacao.codigoDoAtivo}
                          </a>
                        </td>
                        <td className="border px-4 py-1 hidden xl:table-cell text-left">
                          {operacao.nomeDoAtivo.slice(0, 20) + '...'}
                        </td>
                        <td className="border px-4 py-1 text-right hidden xl:table-cell">{operacao.quantitidade}</td>
                        <td className="border px-4 py-1 text-right hidden xl:table-cell">{formatCurrency(operacao.precoMedioCompra)}</td>
                        <td className="border px-4 py-1 text-right hidden xl:table-cell">{formatCurrency(operacao.valorDaOperacao)}</td>
                        <td className="border px-4 py-1 text-right hidden xl:table-cell">
                          {operacao.comprar && <i className="far fa-bell text-red-500 float-left" />}
                          {formatCurrency(operacao.precoAtual)}
                          <i className={`fas ml-2 ${operacao.precoAtual > operacao.precoAnterior ? 'fa-caret-down text-red-500' : 'fa-caret-up text-green-500'}`} />
                        </td>
                        <td className="border px-4 py-1 text-right">{formatCurrency(operacao.valorAtual)}</td>
                        <td className={`border px-4 py-1 text-right ${operacao.rentabilidade < 0 ? 'text-red-500' : 'text-green-500'}`}>
                          {formatCurrency(operacao.rentabilidade)}
                        </td>
                        <td className="border px-4 py-1 text-left">{operacao.nomeDoSetor.slice(0, 20) + '...'}</td>
                      </tr>
                    </>
                  );
                })}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </section>
  );
}
