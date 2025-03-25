import React, { ReactNode, useEffect } from 'react';
import { Outlet, useNavigate } from 'react-router-dom';
import NavigationBar from '../components/NavigationBar';
import Footer from '../components/Footer';
import axios from 'axios';

interface MainLayoutProps {
  children?: ReactNode;
}

const MainLayout: React.FC<MainLayoutProps> = ({ children }) => {
  const navigate = useNavigate();

  useEffect(() => {
    axios.interceptors.response.use(
      null,
      (data) => {
        if (data.response.status === 401) {
          navigate('/login');
        }
        return Promise.reject(data);
      }
    );

  }, [])
  
  return (
    <div>
      <NavigationBar />
      <main>
        {children || <Outlet />}
      </main>
      <Footer />
    </div>
  );
};

export default MainLayout;
