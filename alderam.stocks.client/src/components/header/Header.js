import React from 'react';
import { Link } from "react-router-dom";
import { authenticationService } from '../../services/Auth';

function Header() {
    function handlePowerOffClick() {
        authenticationService.logout();
    }

    return (
        <div>
            <nav id="header" className="bg-gray-100 fixed w-full z-10 top-0 shadow px-3 xl:px-0">
                <div className="w-full container mx-auto flex flex-wrap items-center mt-0 pt-3 pb-3 md:pb-0">
                    <div className="w-full block">
                        <a className="text-gray-900 text-base xl:text-xl no-underline hover:no-underline font-semibold pl-1" href="#/">
                            <i className="fas fa-house-damage text-blue-800 pr-3"></i> Alderam.Stocks
				        </a>
                    </div>
                    <div className="w-full flex items-center w-auto block mt-2 mt-0 bg-gray-100 z-20" id="nav-content">
                        <ul className="list-reset flex flex-1 items-center ">
                            <li className="mr-6 my-2 md:my-0 text-blue-900 hover:text-blue-400 ">
                                <Link to={`/Dashboard`} className="block py-1 md:py-3 pl-1  no-underline">
                                    <i className="fas fa-tachometer-alt fa-fw mr-3 "></i><span className="pb-1 md:pb-0 text-sm">Dashboard</span>
                                </Link>
                            </li>
                            <li className="mr-6 my-2 md:my-0 text-blue-900 hover:text-blue-400 ">
                                <Link to={`/Operacoes`} className="block py-1 md:py-3 pl-1  no-underline">
                                    <i className="fas fa-tasks fa-fw mr-3"></i><span className="pb-1 md:pb-0 text-sm">Operacoes</span>
                                </Link>
                            </li>
                            <li className="mr-6 my-2 md:my-0 text-blue-900 hover:text-blue-400 float-right">
                                <a href="#/" className="block py-1 md:py-3 pl-1 no-underline" onClick={handlePowerOffClick}>
                                    <i className="fas fa-power-off fa-fw mr-3"></i><span className="pb-1 md:pb-0 text-sm">Logout</span>
                                </a>
                            </li>
                        </ul>
                    </div>
                </div> 
            </nav>
        </div>
    );
};

export default Header;
