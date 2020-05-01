import React, { useState, useEffect } from 'react';

import Header from '../../components/header/Header';
import Resumo from '../../components/resumo/Resumo';
import Acompanhamentos from '../../components/acompanhamentos/Acompanhamentos';
import Carteira from '../../components/carteira/Carteira';
import GraficoDeSetoresGoogle from '../../components/graficos/setores/GraficoDeSetoresGoogle';
import GraficoDeSetoresHighcharts from '../../components/graficos/setores/GraficoDeSetoresHighcharts';

import Api from "../../services/Api";
import Toast from "../../services/Toast";



function Dashboard() {
    useEffect(() => {
        document.title = 'Alderam.Stocks/Dashboard';
    }, []);

    async function handleRefreshClick(e) {
        e.preventDefault();
        try {
            Toast.success('Aguarde atualizando resitros...', { autoClose: false })

            await Api.post('/ativos', {}, {
                headers: {
                    Authorization: 'Bearer ' + sessionStorage.getItem('AUTH_TOKEN')
                }
            });

            window.location.reload(false);
        } catch (e) {
            Toast.error('Erro ao atualizar registros.')
        }
    }

    return (
        <div id="Dashboard">
            <Header />

            <div className="container w-full mx-auto pt-20 mt-10 xl:mt-0 ">
                <div className="w-full  px-4 md:px-0 md:mt-8 mb-16 text-gray-800 leading-normal">

                    <h3 className="p-3 text-3xl">
                        <a href="#" onClick={handleRefreshClick} title="Clique aqui para atualizar os dados da página">
                            <i className="fa fa-sync mr-3" />
                        </a>
                        Dashboard
                    </h3>

                    <div className="w-full mt-2 p-1">
                        <div className="grid grid-cols-1 grid-cols-1 xl:grid-cols-5 gap-2  pb-0  ">
                            <Resumo />
                            <Acompanhamentos />
                        </div>
                    </div>

                    <Carteira />

                    <h3 className="p-3 text-3xl">Graficos</h3>
                    <div className="">
                        <GraficoDeSetoresHighcharts />
                    </div>
                </div>
            </div>
        </div>
    );
}

export default Dashboard; 