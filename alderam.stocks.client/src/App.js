import React, { useEffect } from 'react';

import Routes from './routes';

import './styles.css'

const App = () => {
    useEffect(() => {
        document.title = 'Alderam.Stocks';
     }, []);    

    return (
        <div className="App">
            <Routes />
        </div>
    );
}

export default App; 