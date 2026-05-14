'use client';

import { useForm } from 'react-hook-form';

export default function AdicionarBoleta() {
  const { register, handleSubmit } = useForm();

  function onSubmit(data) {
    console.log(data);
  }

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <h2 className="mb-10 text-2xl">Adicionar Boleta</h2>
      <div>
        <label>Número da boleta</label>
        <input {...register('numero')} type="number"
          className="border border-gray-300 rounded-lg py-2 px-4 block w-full" />
      </div>
      <div>
        <label>Data da Operação</label>
        <input {...register('dataDaOperacao')} type="date"
          className="border border-gray-300 rounded-lg py-2 px-4 block w-full" />
      </div>
      <div>
        <label>Taxa de liquidação</label>
        <input {...register('taxaDeLiquidacao')} type="text"
          className="border border-gray-300 rounded-lg py-2 px-4 block w-full" />
      </div>
      <div>
        <label>Emolumentos</label>
        <input {...register('emolumentos')} type="text"
          className="border border-gray-300 rounded-lg py-2 px-4 block w-full" />
      </div>
      <div>
        <label>Corretagem</label>
        <input {...register('corretagem')} type="text"
          className="border border-gray-300 rounded-lg py-2 px-4 block w-full" />
      </div>
      <div>
        <label>ISS</label>
        <input {...register('iss')} type="text"
          className="border border-gray-300 rounded-lg py-2 px-4 block w-full" />
      </div>
      <div>
        <label>IRRF</label>
        <input {...register('irrf')} type="text"
          className="border border-gray-300 rounded-lg py-2 px-4 block w-full" />
      </div>
      <div>
        <label>Observações</label>
        <input {...register('observacoes')} type="text"
          className="border border-gray-300 rounded-lg py-2 px-4 block w-full" />
      </div>
      <div className="w-full mt-5 p-1">
        <div className="grid grid-cols-1 xl:grid-cols-5 pb-0">
          <button aria-label="Submit" type="submit"
            className="rounded p-3 bg-green-600 w-20">
            <i className="fas fa-plus fa-3x fa-fw fa-inverse"></i>
          </button>
          <button aria-label="Cancel" type="button"
            className="rounded p-3 bg-red-600 w-20">
            <i className="far fa-window-close fa-3x fa-fw fa-inverse"></i>
          </button>
        </div>
      </div>
    </form>
  );
}
