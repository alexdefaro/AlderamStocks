import React, { useState, useEffect } from 'react';

import Api from "../../services/Api";
import Boleta from "../boletas/Boleta";

function Boletas() {
    const [boletas, setBoletas] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            const response = await Api.get('/boletas');
            setBoletas(response.data);
        }

        fetchData();
    }, []);

    function formatCurrency(value) {
        let result = value.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
        return result.replace('R$', '');
    }

    return (
        boletas.map(boleta => (
            <Boleta key={boleta.id} boleta={boleta} />
        ))
    )
}

export default Boletas;
