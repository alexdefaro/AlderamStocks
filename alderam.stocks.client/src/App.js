import React, { useEffect } from 'react';

import RoutingConfiguration from './routes';

import './styles.css'

const App = () => {
    useEffect(() => {
        document.title = 'Alderam.Stocks';
     }, []);    

    return (
        <div className="App">
            <RoutingConfiguration />
        </div>
    );
}

export default App; 