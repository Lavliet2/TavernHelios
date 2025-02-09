import { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

// @ts-ignore
const ErrorHandler = ({ children }) => {
    const [isSet, setIsSet] = useState(false)

    const navigate = useNavigate();

    useEffect(() => {
        const interceptor = axios.interceptors.response.use(
            null,
            (data) => {
                console.log(data);
                if (data.response?.status === 401) {
                    navigate('/login');
                    return Promise.reject(data);
                }
                return Promise.reject(data);
            }
        );
        setIsSet(true);

        return () => axios.interceptors.response.eject(interceptor);
    }, [navigate]);

    return isSet && children;
}

export default ErrorHandler;
