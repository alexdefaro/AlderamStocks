import React, { Component, useEffect } from 'react';

import Routes from './routes';

import './styles.css'

const App = () => {
    useEffect(() => {
        document.title = 'Alderam.Stocks';
        sessionStorage.clear();
    }, []);    

    return (
        <div className="App">
            <Routes />
        </div>
    );
}

export default App; 