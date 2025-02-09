import axios from 'axios';
import React, { createContext, useContext, useState, ReactNode } from 'react';
import { API_BASE_URL } from '../config';

interface UserContextType {
    userInfo: any;
    login: () => Promise<any>;
    logout: () => void;
}

const UserContext = createContext<UserContextType | undefined>(undefined);

export const useUserContext = () => {
    const context = useContext(UserContext);
    if (!context) {
        throw new Error('useUserContext must be used within an UserProvider');
    }
    return context;
};

export const UserProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [userInfo, setUserInfo] = useState(null);

    const login = () => {
        return axios.get(`${API_BASE_URL}/users/info`)
            .then((response) => {console.log(response.data); return setUserInfo(response.data)});
    };

    const logout = () => {
        setUserInfo(null);
        // TODO signout
        localStorage.removeItem('isAuthenticated');
    };

    return (
        <UserContext.Provider value={{ userInfo, login, logout }}>
            {children}
        </UserContext.Provider>
    );
};
