import React, { createContext, useState, useContext, ReactNode } from 'react';

interface User {
  id: string;
  login: string;
  fullName: string;
  roles: number[];
}

interface UserContextType {
  login: (data: User) => void;
  logout: () => void;
  user: User | null;
  hasRole: (role: number) => boolean;
}

const UserContext = createContext<UserContextType | undefined>(undefined);

interface UserProviderProps {
  children: ReactNode;
}

export const UserContextProvider: React.FC<UserProviderProps> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);

  const login = (userData: User) => {
    setUser(userData);
  };

  const logout = () => {
    setUser(null);
  };

  const hasRole = (role: number) => {
    return user?.roles.includes(role) ?? false;
  };

  const value = {
    user,
    login,
    logout,
    hasRole,
  };

  return <UserContext.Provider value={value}>{children}</UserContext.Provider>;
};

export const useUser = () => {
  const context = useContext(UserContext);
  if (context === undefined) {
    throw new Error('useUser must be used within a UserProvider');
  }
  return context;
};