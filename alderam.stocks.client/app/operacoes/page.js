'use client';

import { useEffect } from 'react';
import { useRouter } from 'next/navigation';

import Header from '@/components/header/Header';
import Boletas from '@/components/boletas/Boletas';
import { authenticationService } from '@/services/Auth';

export default function OperacoesPage() {
  const router = useRouter();

  useEffect(() => {
    if (!authenticationService.isAuthenticated()) {
      router.replace('/');
      return;
    }
    document.title = 'Alderam.Stocks/Operacoes';
  }, [router]);

  return (
    <div id="Operacoes">
      <Header />
      <div className="container w-full mx-auto pt-20 mt-10 xl:mt-0">
        <div className="w-full px-4 md:px-0 md:mt-8 mb-16 text-gray-800 leading-normal">
          <h3 className="p-3 text-3xl">Operações</h3>
          <Boletas />
        </div>
      </div>
    </div>
  );
}
