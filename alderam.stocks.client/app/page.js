'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import ApiService from '@/services/Api';

const initialValues = { userKey: '' };

const validationSchema = Yup.object({
  userKey: Yup.string().required('Chave do usuário é obrigatória.'),
});

export default function LoginPage() {
  const [buttonText, setButtonText] = useState('Entrar');
  const router = useRouter();

  async function onSubmit(values) {
    try {
      setButtonText(' Aguarde atualizando... ');
      const response = await ApiService.post('authentication', { userKey: values.userKey });
      sessionStorage.setItem('AUTH_TOKEN', response.data.token);
      router.push('/dashboard');
    } catch {
      sessionStorage.clear();
      setButtonText('Entrar');
      alert('Error ao efetuar login.');
    }
  }

  return (
    <div className="container w-full mx-auto pt-20 mt-10 xl:mt-0">
      <div className="w-full mx-auto max-w-xs mt-10">
        <Formik initialValues={initialValues} onSubmit={onSubmit} validationSchema={validationSchema}>
          <Form className="bg-gray-100 border shadow-md rounded px-8 pt-6 pb-8 mb-2">
            <div className="mb-4">
              <label className="block text-gray-700 text-sm font-bold mb-4">
                Informe a chave do usuário
              </label>
              <Field
                name="userKey"
                placeholder="Chave do usuário"
                type="password"
                autoFocus
                className="shadow appearance-none rounded w-full py-3 px-3 text-gray-700 mb-3 leading-tight focus:outline-none focus:shadow-outline"
              />
              <ErrorMessage name="userKey" />
            </div>
            <div className="flex items-center justify-between">
              <button
                type="submit"
                className="bg-blue-500 w-full hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
              >
                {buttonText}
              </button>
            </div>
          </Form>
        </Formik>
      </div>
      <p className="text-center text-gray-500 text-xs">
        &copy;2020 Alderam.Stocks. All rights reserved.
      </p>
    </div>
  );
}
