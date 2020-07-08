import React, { useState, useEffect } from 'react';

import ApiService from "../../services/Api";
import Boleta from "../boletas/Boleta";

function Boletas() {
    const [boletas, setBoletas] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            const response = await ApiService.get('/boletas');
            setBoletas(response.data);
        }

        fetchData();
    }, []);

    return (
        boletas.map(boleta => (
            <Boleta key={boleta.id} boleta={boleta} />
        ))
    )
}

export default Boletas;
