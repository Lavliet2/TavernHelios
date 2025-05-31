import React, { useEffect, useState } from 'react';
import {
  Box,
  Typography,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Button,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  FormControlLabel,
  Checkbox,
  IconButton,
  Tooltip,
} from '@mui/material';
import { Edit as EditIcon, Delete as DeleteIcon } from '@mui/icons-material';
import { useTranslation } from 'react-i18next';
import { API_BASE_URL } from '../../config';
import { USER_ROLES } from '../../config';
import axios from 'axios';

interface User {
  id: string;
  login: string;
  fullName: string;
  userRoles: Array<{
    roleId: number;
    roleName: string;
  }>;
}

const UserManagement: React.FC = () => {
  const { t } = useTranslation();
  const [users, setUsers] = useState<User[]>([]);
  const [openDialog, setOpenDialog] = useState(false);
  const [editingUser, setEditingUser] = useState<User | null>(null);
  const [formData, setFormData] = useState({
    login: '',
    fullName: '',
    password: '',
    roleIds: [] as number[],
  });

  useEffect(() => {
    fetchUsers();
  }, []);

  const fetchUsers = async () => {
    try {
      const response = await axios.get(`${API_BASE_URL}/api/users`);
      setUsers(response.data);
    } catch (error) {
      console.error('Error fetching users:', error);
    }
  };

  const handleOpenDialog = (user?: User) => {
    if (user) {
      setEditingUser(user);
      setFormData({
        login: user.login,
        fullName: user.fullName,
        password: '',
        roleIds: user.userRoles.map(role => role.roleId),
      });
    } else {
      setEditingUser(null);
      setFormData({
        login: '',
        fullName: '',
        password: '',
        roleIds: [],
      });
    }
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setEditingUser(null);
  };

  const handleRoleChange = (roleId: number) => {
    setFormData(prev => ({
      ...prev,
      roleIds: prev.roleIds.includes(roleId)
        ? prev.roleIds.filter(r => r !== roleId)
        : [...prev.roleIds, roleId],
    }));
  };

  const handleSubmit = async () => {
    try {
      const submitData = {
        fullName: formData.fullName,
        roleIds: formData.roleIds
      };

      if (editingUser) {
        await axios.put(`${API_BASE_URL}/api/users/${editingUser.id}`, submitData);
      } else {
        await axios.post(`${API_BASE_URL}/api/users`, {
          ...submitData,
          login: formData.login,
          password: formData.password
        });
      }
      fetchUsers();
      handleCloseDialog();
    } catch (error) {
      console.error('Error saving user:', error);
    }
  };

  const handleDelete = async (userId: string) => {
    if (window.confirm(t('admin.users.confirmDelete'))) {
      try {
        await axios.delete(`${API_BASE_URL}/api/users/${userId}`);
        fetchUsers();
      } catch (error) {
        console.error('Error deleting user:', error);
      }
    }
  };

  return (
    <Box sx={{ p: 3 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4">{t('admin.users.title')}</Typography>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>{t('admin.users.login')}</TableCell>
              <TableCell>{t('admin.users.fullName')}</TableCell>
              <TableCell>{t('admin.users.roles')}</TableCell>
              <TableCell>{t('admin.users.actions')}</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {users.map((user) => (
              <TableRow key={user.id}>
                <TableCell>{user.login}</TableCell>
                <TableCell>{user.fullName}</TableCell>
                <TableCell>
                  {user.userRoles.map(role => (
                    <span key={role.roleId} style={{ marginRight: '8px' }}>
                      {role.roleId === USER_ROLES.Admin ? t('admin.users.roleAdmin') : role.roleId === USER_ROLES.Manager ? t('admin.users.roleManager') : ''}
                    </span>
                  ))}
                </TableCell>
                <TableCell>
                  <Tooltip title={t('admin.users.edit')}>
                    <IconButton onClick={() => handleOpenDialog(user)}>
                      <EditIcon />
                    </IconButton>
                  </Tooltip>
                  <Tooltip title={t('admin.users.delete')}>
                    <IconButton onClick={() => handleDelete(user.id)}>
                      <DeleteIcon />
                    </IconButton>
                  </Tooltip>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <Dialog open={openDialog} onClose={handleCloseDialog}>
        <DialogTitle>
          {editingUser ? t('admin.users.editUser') : t('admin.users.addUser')}
        </DialogTitle>
        <DialogContent>
          <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, pt: 2 }}>
            <TextField
              label={t('admin.users.login')}
              value={formData.login}
              onChange={(e) => setFormData({ ...formData, login: e.target.value })}
              fullWidth
              disabled={!!editingUser}
            />
            <TextField
              label={t('admin.users.fullName')}
              value={formData.fullName}
              onChange={(e) => setFormData({ ...formData, fullName: e.target.value })}
              fullWidth
            />
            {!editingUser && (
              <TextField
                label={t('admin.users.password')}
                type="password"
                value={formData.password}
                onChange={(e) => setFormData({ ...formData, password: e.target.value })}
                fullWidth
              />
            )}
            <FormControlLabel
              control={
                <Checkbox
                  checked={formData.roleIds.includes(USER_ROLES.Admin)}
                  onChange={() => handleRoleChange(USER_ROLES.Admin)}
                />
              }
              label={t('admin.users.roleAdmin')}
            />
            <FormControlLabel
              control={
                <Checkbox
                  checked={formData.roleIds.includes(USER_ROLES.Manager)}
                  onChange={() => handleRoleChange(USER_ROLES.Manager)}
                />
              }
              label={t('admin.users.roleManager')}
            />
          </Box>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseDialog}>{t('common.cancel')}</Button>
          <Button onClick={handleSubmit} variant="contained" color="primary">
            {t('common.save')}
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
};

export default UserManagement; 