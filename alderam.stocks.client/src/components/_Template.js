import React, { useState, useEffect } from 'react';
import { Link } from "react-router-dom";

import Api from '../services/Api';

function Template() {
    const [boletas, setBoletas] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            const response = await Api.get('/boletas');
            setBoletas(response.data);
        }

        fetchData();
    }, []);

    return (
        <div>
            <Link to={`/blocked`}>Access blocked URL</Link>
        </div>
    )
}

export default Template;
