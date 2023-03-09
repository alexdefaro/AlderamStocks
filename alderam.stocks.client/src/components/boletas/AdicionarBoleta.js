import React from "react";

import { useForm } from "react-hook-form";

/*
 * 
 * https://codesandbox.io/s/react-hook-form-master-detail-cmklr?file=/src/App.tsx:327-333
   https://www.carlrippon.com/master-detail-forms-with-react-form-hook/
{
    "numero": "40867",
    "dataDaOperacao": "2022-08-08",
    "taxaDeLiquidacao": "4.09",
    "emolumentos": "0.81",
    "corretagem": "",
    "iss": "0.00",
    "irrf": "0.00",
    "observacoes": "HCTR11"
}

 {
    "numero": "40867",
    "dataDaOperacao": "2022-08-08T00:00:00",
    "taxaDeLiquidacao": 4.09,
    "emolumentos": 0.81,
    "corretagem": 0.00,
    "iss": 0.00,
    "irrf": 0,
    "observacoes": "HCTR11",

    "operacoes": [
                                { 
                                    "codigoDoAtivo": "HCTR11",
                                    "nomeDoAtivo": "FII HECTARE CI ER",
                                    "tipoDeOperacao": "C",
                                  "tipoDeInvestimento": 2,
                                    "quantitidade": 100,
                                    "precoUnitario": 109.50,
                                    "dataDaOperacao": "2022-08-08T00:00:00"
                                },
                                { 
                                    "codigoDoAtivo": "HCTR11",
                                    "nomeDoAtivo": "FII HECTARE CI ER",
                                    "tipoDeOperacao": "C",
                                  "tipoDeInvestimento": 2,
                                    "quantitidade": 50,
                                    "precoUnitario": 108.50,
                                    "dataDaOperacao": "2022-08-08T00:00:00"
                                }								
    ]
}
 */

function AdicionarBoleta() {
    const { register, handleSubmit, watch, formState: { errors } } = useForm();

    function onSubmit(data) {
        console.log(data);
    };

    return (
        <>
            <form onSubmit={handleSubmit(onSubmit)}>
                <h2 className="mb-10 text-2xl">Adicionar Boleta</h2>
                <div>
                    <label>Número da boleta</label>
                    <input {...register("numero")} type="number"
                        className="border border-gray-300 rounded-lg py-2 px-4 block w-full" />
                </div>
                 <div>
                    <label>Data da Operação</label>
                    <input {...register("dataDaOperacao")} type="date"
                        className="border border-gray-300 rounded-lg py-2 px-4 block w-full"  />
                </div>
                <div>
                    <label>Taxa de liquidação</label>
                    <input {...register("taxaDeLiquidacao")} type="text" 
                        className="border border-gray-300 rounded-lg py-2 px-4 block w-full" />

                </div>
                <div>
                    <label>Emolumentos</label>
                    <input {...register("emolumentos")} type="text"
                        className="border border-gray-300 rounded-lg py-2 px-4 block w-full" />
                </div>
                <div>
                    <label>Corretagem</label>
                    <input {...register("corretagem")} type="text"
                        className="border border-gray-300 rounded-lg py-2 px-4 block w-full" />
                </div>
                <div>
                    <label>ISS</label>
                    <input {...register("iss")} type="text"
                        className="border border-gray-300 rounded-lg py-2 px-4 block w-full" />
                </div>
                <div>
                    <label>IRRF</label>
                    <input {...register("irrf")} type="text"
                        className="border border-gray-300 rounded-lg py-2 px-4 block w-full" />
                </div>
                <div>
                    <label>Observações</label>
                    <input {...register("observacoes")} type="text"
                        className="border border-gray-300 rounded-lg py-2 px-4 block w-full" />
                </div>
                <div className="w-full mt-2 p-1 mt-5">
                    <div className="grid grid-cols-1 grid-cols-1 xl:grid-cols-5 pb-0">
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
        </>
    )
}


export default AdicionarBoleta;