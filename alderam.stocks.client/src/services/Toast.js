import { toast } from 'react-toastify'

toast.configure({
    autoClose: 1500,
    closeButton: false,
    newestOnTop: true,
    hideProgressBar: true
});

const Toast = toast;

export default Toast;