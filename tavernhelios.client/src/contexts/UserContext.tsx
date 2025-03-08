import React, { createContext, useState, useContext, ReactNode } from 'react';

interface UserContextType {
  login: (data: any) => void;
  logout: () => void;
  user: any;
}
const UserContext = createContext<UserContextType | undefined>(undefined);
interface UserProviderProps {
  children: ReactNode;
}
export const UserContextProvider : React.FC<UserProviderProps> = ({ children }) => {
  const [user, setUser] = useState(null);

  const login = (userData: any) => {
    setUser(userData);
  };

  const logout = () => {
    setUser(null);
  };

  const value = {
    user,
    login,
    logout,
  };

  return <UserContext.Provider value={value}>{children}</UserContext.Provider>;
};

export const useUser = () => {
  return useContext(UserContext);
};