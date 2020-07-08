import React, { useState, useEffect } from 'react';

import Header from '../../components/header/Header';
import Resumo from '../../components/resumo/Resumo';
import Acompanhamentos from '../../components/acompanhamentos/Acompanhamentos';
import Carteira from '../../components/carteira/Carteira';
import GraficoDeSubsetoresHighcharts from '../../components/graficos/subsetores/GraficoDeSubsetoresHighcharts';

import ApiService from "../../services/Api";
import Toast from "../../services/Toast";
import Spinner from '../../components/spinner/Spinner';

function Dashboard() {
    const [spinnerVisibilityClass, setspinnerVisibilityClass] = useState('visible');

    useEffect(() => {
        document.title = 'Alderam.Stocks/Dashboard';
        setTimeout(() => setspinnerVisibilityClass('hidden'), 1000)
    }, []); 

    async function handleRefreshClick(e) {
        e.preventDefault();
        try {
            setspinnerVisibilityClass('visible')
            await ApiService.post('/ativos', {});

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

                    <h3 className="p-1 text-3xl">
                        <a href="#/" onClick={handleRefreshClick} title="Clique aqui para atualizar os dados da página">
                            <i className={"fa fa-sync mr-3"} />
                        </a>
                        Dashboard
                    </h3>

                    <Spinner visibilityValue={spinnerVisibilityClass} />

                    <div className="w-full mt-2 p-1">
                        <div className="grid grid-cols-1 grid-cols-1 xl:grid-cols-5 gap-2  pb-0  ">
                            <Resumo />
                            <Acompanhamentos />
                        </div>
                    </div>

                    <Carteira />

                    <h3 className="p-3 text-3xl">Graficos</h3>
                    <div className="">
                        <GraficoDeSubsetoresHighcharts />
                    </div>
                </div>
            </div>
        </div>
    );
} 

export default Dashboard; 