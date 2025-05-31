import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';
import { useUser } from '../contexts/UserContext';

interface RoleProtectedRouteProps {
  requiredRoles: number[];
}

export const RoleProtectedRoute: React.FC<RoleProtectedRouteProps> = ({ requiredRoles }) => {
  const { hasRole } = useUser();

  const hasAnyRequiredRole = requiredRoles.some(role => hasRole(role));

  if (!hasAnyRequiredRole) {
    return <Navigate to="/" replace />;
  }

  return <Outlet />;
}; 