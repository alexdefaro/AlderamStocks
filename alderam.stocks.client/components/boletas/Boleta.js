'use client';

export default function Boleta({ boleta }) {
  function formatCurrency(value) {
    return value.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
  }

  return (
    <div className="mt-5 w-full bg-gray-100 border border-gray-900 rounded shadow">
      <div className="w-full border-0 rounded">
        <div className="grid grid-cols-1 xl:grid-cols-4 gap-0 xl:gap-2 pb-0">
          <div className="xl:col-span-3 mx-2 pb-2">
            <div className="p-3">
              <h2 className="text-3xl font-semibold">{boleta.numero}</h2>
              <p>
                <span className="font-semibold">Data da Operação: </span>
                {Intl.DateTimeFormat('pt-BR').format(new Date(boleta.dataDaOperacao))}
              </p>
            </div>
            <div className="hidden xl:grid grid-cols-7 gap-2 px-3 font-semibold">
              <div className="text-left xl:col-span-1">Tipo</div>
              <div className="text-left xl:col-span-1">Codigo do Ativo</div>
              <div className="text-left xl:col-span-2">Nome do Ativo</div>
              <div className="text-right xl:col-span-1">Quantidade</div>
              <div className="text-right xl:col-span-1">Preço</div>
              <div className="text-right xl:col-span-1">Total</div>
            </div>
            {boleta.operacoes.map(operacao => (
              <div key={operacao.id} className="grid grid-cols-1 xl:grid-cols-7 gap-2 px-3 py-3 my-3 border xl:border-0 xl:py-0 xl:my-0 rounded xl:rounded-none">
                <div className="text-right xl:text-left xl:col-span-1">
                  <span className="float-left xl:hidden font-semibold">Tipo</span>
                  {operacao.tipoDeOperacao === 67 ? 'Compra' : operacao.tipoDeOperacao === 66 ? 'Bonificação' : 'Venda'}
                </div>
                <div className="text-right xl:text-left xl:col-span-1">
                  <span className="float-left xl:hidden font-semibold">Codigo do Ativo</span>
                  {operacao.ativo.codigo}
                </div>
                <div className="text-right xl:text-left xl:col-span-2">
                  <span className="float-left xl:hidden font-semibold">Nome do Ativo</span>
                  {operacao.ativo.nome}
                </div>
                <div className="text-right xl:col-span-1">
                  <span className="float-left xl:hidden font-semibold">Quantidade</span>
                  {operacao.quantitidade}
                </div>
                <div className="text-right xl:col-span-1">
                  <span className="float-left xl:hidden font-semibold">Preço de Compra</span>
                  {formatCurrency(operacao.precoUnitario)}
                </div>
                <div className="text-right xl:col-span-1">
                  <span className="float-left xl:hidden font-semibold">Valor da Operação</span>
                  {formatCurrency(operacao.valorDaOperacao)}
                </div>
              </div>
            ))}
          </div>

          <div className="xl:col-span-1">
            <div className="bg-blue-100 text-black grid grid-cols-1 gap-1 px-3 py-3 h-full border-l rounded-b xl:rounded-r">
              <div className="text-right font-medium">
                <span className="float-left font-semibold mx-1">Total: </span>
                {formatCurrency(boleta.valorDaCompra)}
              </div>
              <div className="text-right font-medium">
                <span className="float-left font-semibold mx-1">Taxa de liquidação: </span>
                {formatCurrency(boleta.taxaDeLiquidacao)}
              </div>
              <div className="text-right font-medium">
                <span className="float-left font-semibold mx-1">Emolumentos: </span>
                {formatCurrency(boleta.emolumentos)}
              </div>
              <div className="text-right font-medium">
                <span className="float-left font-semibold mx-1">Corretagem: </span>
                {formatCurrency(boleta.corretagem)}
              </div>
              <div className="text-right font-medium">
                <span className="float-left font-semibold mx-1">ISS: </span>
                {formatCurrency(boleta.iss)}
              </div>
              {boleta.irrf != null && (
                <div className="text-right font-medium">
                  <span className="float-left font-semibold mx-1">IRRF: </span>
                  {formatCurrency(boleta.irrf ?? 0)}
                </div>
              )}
              <div className="text-right font-medium">
                <span className="float-left font-semibold mx-1">Total Operação: </span>
                {formatCurrency(boleta.valorDaOperacao)}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
