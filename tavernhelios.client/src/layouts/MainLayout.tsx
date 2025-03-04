import React, { ReactNode } from 'react';
import { Outlet } from 'react-router-dom';
import NavigationBar from '../components/NavigationBar';
import Footer from '../components/Footer';

interface MainLayoutProps {
  children?: ReactNode;
}

const MainLayout: React.FC<MainLayoutProps> = ({ children }) => {
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
