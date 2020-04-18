import React from 'react';

export default function Boleta({ boleta }) {
    function formatCurrency(value) {
        let result = value.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
        return result;
    }

    return (
        <div className="mt-5 w-full bg-gray-100 border border-gray-900 rounded shadow">
            <div className="w-full border-0 rounded ">
                <div class="grid grid-cols-1 grid-cols-1 xl:grid-cols-3 gap-0 xl:gap-2  mr-0  pb-0  ">
                    <div class="xl:col-span-2 mx-2 pb-2">
                        <div class="p-3">
                            <h2 class="text-3xl font-semibold">{boleta.numero}</h2>
                            <p><span class="font-semibold">Data da Operação: </span>{Intl.DateTimeFormat('pt-BR').format(new Date(boleta.dataDaOperacao))}</p>
                        </div>
                        <div class="hidden sm:hidden xl:grid grid-cols-6 gap-2 px-3 font-semibold">
                            <div class="text-left xl:col-span-1">Codigo do Ativo</div>
                            <div class="text-left xl:col-span-2">Nome do Ativo</div>
                            <div class="text-right xl:col-span-1">Quantidade </div>
                            <div class="text-right xl:col-span-1">Preço</div>
                            <div class="text-right xl:col-span-1">Total</div>
                        </div>
                        {
                            boleta.operacoes.map(operacao => (
                                <div key={operacao.id} class="grid grid-cols-1 sm:grid-cols-1 xl:grid-cols-6 gap-2 px-3 py-3 my-3 border sm:py-3 sm:my-3 sm:border rounded xl:border-0 xl:py-0 xl:my-0  ">
                                    <div class="text-right sm:text-right xl:text-left xl:col-span-1">
                                        <span class="float-left block sm:block xl:hidden font-semibold">Codigo do Ativo</span>
                                        {operacao.ativo.codigo}
                                    </div>
                                    <div class="text-right sm:text-right xl:text-left xl:col-span-2">
                                        <span class="float-left block sm:block xl:hidden font-semibold">Nome do Ativo</span>
                                        {operacao.ativo.nome}
                                    </div>
                                    <div class="text-right xl:col-span-1">
                                        <span class="float-left block sm:block xl:hidden font-semibold">Quantidade </span>
                                        {formatCurrency(operacao.quantitidade)}
                                    </div>
                                    <div class="text-right xl:col-span-1">
                                        <span class="float-left block sm:block xl:hidden font-semibold">Preço de Compra</span>
                                        {formatCurrency(operacao.precoDeCompra)}
                                    </div>
                                    <div class="text-right xl:col-span-1">
                                        <span class="float-left block sm:block xl:hidden font-semibold">Valor da Operação</span>
                                        {formatCurrency(operacao.valorDaOperacao)}
                                    </div>
                                </div>
                            ))
                        }
                    </div>


                    <div class="xl:col-span-1">
                        <div class="bg-blue-100 text-black grid grid-cols-1 gap-1  px-3 py-3 mb-0 mt-0 h-full border-0 border-l rounded-b xl:rounded-r ">
                            <div class="text-right font-medium ">
                                <span class="float-left inline-block font-semibold mx-1">Total Compra: </span>
                                {formatCurrency(boleta.valorDaCompra)}
                                </div>
                            <div class="text-right font-medium">
                                <span class="float-left inline-block font-semibold mx-1">Taxa de liquidação: </span>
                                {formatCurrency(boleta.taxaDeLiquidacao)}
                            </div>
                            <div class="text-right font-medium">
                                <span class="float-left inline-block font-semibold mx-1">Emolumentos: </span>
                                {formatCurrency(boleta.emolumentos)}
                            </div>
                            <div class="text-right font-medium">
                                <span class="float-left inline-block font-semibold mx-1">Corretagem: </span>
                                {formatCurrency(boleta.corretagem)}
                            </div>
                            <div class="text-right font-medium">
                                <span class="float-left inline-block font-semibold mx-1">ISS: </span>
                                {formatCurrency(boleta.iss)}
                            </div>
                            <div class="text-right font-medium">
                                <span class="float-left inline-block font-semibold mx-1">Total Operação: </span>
                                {formatCurrency(boleta.valorDaOperacao)}
                                </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}