import React, { useState, useEffect } from 'react';

import Header from '../../components/header/Header';
import Boletas from '../../components/boletas/Boletas';

document.title = 'Alderam.Stocks/Operacoes'; 

function Operacoes() {
    useEffect(() => {
        document.title = 'Alderam.Stocks/Operacoes'; 
    }, []);

    return (
        <div id="Dashboard">
            <Header />

            <div className="container w-full mx-auto pt-20 mt-10 xl:mt-0">
                <div className="w-full px-4 md:px-0 md:mt-8 mb-16 text-gray-800 leading-normal">
                    <h3 className="p-3 text-3xl">Operações</h3>
                    <Boletas />
                </div>
            </div>
        </div >
    );
}

export default Operacoes; 