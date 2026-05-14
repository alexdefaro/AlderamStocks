'use client';

import Link from 'next/link';
import { useRouter } from 'next/navigation';
import { authenticationService } from '@/services/Auth';

export default function Header() {
  const router = useRouter();

  function handleLogout() {
    authenticationService.logout();
    router.push('/');
  }

  return (
    <nav id="header" className="bg-gray-100 fixed w-full z-10 top-0 shadow px-3 xl:px-0">
      <div className="w-full container mx-auto flex flex-wrap items-center mt-0 pt-3 pb-3 md:pb-0">
        <div className="w-full block">
          <a className="text-gray-900 text-base xl:text-xl no-underline hover:no-underline font-semibold pl-1" href="#/">
            <i className="fas fa-house-damage text-blue-800 pr-3"></i> Alderam.Stocks
          </a>
        </div>
        <div className="w-full flex items-center w-auto block mt-2 mt-0 bg-gray-100 z-20" id="nav-content">
          <ul className="list-reset flex flex-1 items-center">
            <li className="mr-6 my-2 md:my-0 text-blue-900 hover:text-blue-400">
              <Link href="/dashboard" className="block py-1 md:py-3 pl-1 no-underline">
                <i className="fas fa-tachometer-alt fa-fw mr-3"></i>
                <span className="pb-1 md:pb-0 text-sm">Dashboard</span>
              </Link>
            </li>
            <li className="mr-6 my-2 md:my-0 text-blue-900 hover:text-blue-400">
              <Link href="/operacoes" className="block py-1 md:py-3 pl-1 no-underline">
                <i className="fas fa-tasks fa-fw mr-3"></i>
                <span className="pb-1 md:pb-0 text-sm">Operacoes</span>
              </Link>
            </li>
            <li className="mr-6 my-2 md:my-0 text-blue-900 hover:text-blue-400">
              <button onClick={handleLogout} className="block py-1 md:py-3 pl-1 no-underline bg-transparent border-0 cursor-pointer">
                <i className="fas fa-power-off fa-fw mr-3"></i>
                <span className="pb-1 md:pb-0 text-sm">Logout</span>
              </button>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  );
}
