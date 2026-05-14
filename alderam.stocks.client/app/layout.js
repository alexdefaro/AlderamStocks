import '@/styles/globals.css';
import 'react-toastify/dist/ReactToastify.css';
import ToastProvider from '@/components/providers/ToastProvider';

export const metadata = {
  title: 'Alderam.Stocks',
  description: 'Gerenciador de carteira de ações',
};

export default function RootLayout({ children }) {
  return (
    <html lang="pt-BR">
      <head>
        <link
          rel="stylesheet"
          href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.13.0/css/all.min.css"
          integrity="sha256-h20CPZ0QyXlBuAw7A+KluUYx/3pK+c7lYEpqLTlxjYQ="
          crossOrigin="anonymous"
        />
      </head>
      <body className="bg-white font-sans leading-normal tracking-normal">
        {children}
        <ToastProvider />
      </body>
    </html>
  );
}
