'use client';

import { ToastContainer } from 'react-toastify';

export default function ToastProvider() {
  return (
    <ToastContainer
      autoClose={1500}
      closeButton={false}
      newestOnTop
      hideProgressBar
    />
  );
}
