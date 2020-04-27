import React, { useState, useEffect } from 'react';
import { useHistory } from "react-router-dom";
import { FiLogIn } from "react-icons/fi";

import Api from '../../services/Api';

function Main() {
    const [userKey, setUserKey] = useState('');
    const [buttonText, setbuttonText] = useState('Entrar');

    const history = useHistory();

    useEffect(() => {
        document.title = 'Alderam.Stocks/Logon';
    });

    async function handleLogon(e) {
        e.preventDefault();

        try {
            setbuttonText(' Aguarde atualizando... '); 

            await Api.post('authentication', { userKey });

            localStorage.setItem('authenticated', true);
            history.push('/dashboard');

        } catch (error) {
            localStorage.setItem('authenticated', false);
            setbuttonText('Entrar'); 

            alert('Error ao efetuar login.');
        }
    }

    return (
        <div className="container w-full mx-auto pt-20 mt-10 xl:mt-0">
            <div className="w-full mx-auto max-w-xs mt-10">
                <form className="bg-gray-100 border shadow-md rounded px-8 pt-6 pb-8 mb-2" onSubmit={handleLogon}>
                    <div className="mb-4">
                        <label className="block text-gray-700 text-sm font-bold mb-4">Chave do usuário</label>
                        <input type="text" placeholder="Chave do usuário" value={userKey} onChange={e => setUserKey(e.target.value)} type="password"
                            className="shadow appearance-none rounded w-full py-3 px-3 text-gray-700 mb-3 leading-tight focus:outline-none focus:shadow-outline" />

                    </div>
                    <div className="flex items-center justify-between">
                        <button id="btnLogin" className="bg-blue-500 w-full hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline" type="submit">
                            {buttonText}
                        </button>
                    </div>
                </form>
            </div>

            <p className="text-center text-gray-500 text-xs">
                &copy;2020 Alderam.Stocks. All rights reserved.
            </p>
        </div>
    );
}

export default Main;