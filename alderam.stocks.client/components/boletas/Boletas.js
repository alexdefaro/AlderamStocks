'use client';

import { useState, useEffect } from 'react';
import ApiService from '@/services/Api';
import Boleta from './Boleta';

export default function Boletas() {
  const [boletas, setBoletas] = useState([]);

  useEffect(() => {
    ApiService.get('/boletas').then(r => setBoletas(r.data));
  }, []);

  return boletas.map(boleta => <Boleta key={boleta.id} boleta={boleta} />);
}
