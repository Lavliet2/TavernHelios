import { createTheme } from '@mui/material/styles';

const theme = createTheme({
  palette: {
    primary: {
      main: '#8B5E3C',
    },
    secondary: {
      main: '#FFD700',
    },
    background: {
      default: '#2C1A2A',
      paper: '#3E3E3E',
    },
    text: {
      primary: '#F4E1A1',
    },
  },
  typography: {
    fontFamily: 'Montserrat, sans-serif',
    h6: {
      fontFamily: 'Montserrat, sans-serif',
      fontSize: '1.8rem',
      fontWeight: 'bold',
      color: '#FFD700',
    },
  },
  components: {
    MuiButton: {
      styleOverrides: {
        root: {
          '&:hover': {
            backgroundColor: '#FFD700',
            color: '#2C1A2A',
          },
          transition: 'background-color 0.3s ease, color 0.3s ease',
        },
      },
    },
    MuiMenuItem: {
      styleOverrides: {
        root: {
          '&:hover': {
            backgroundColor: '#FFD700',
            color: '#2C1A2A',
          },
          transition: 'background-color 0.3s ease, color 0.3s ease',
        },
      },
    },
    MuiAppBar: {
      styleOverrides: {
        root: {
          background: 'linear-gradient(45deg, #8B5E3C, #FFD700)', 
          boxShadow: '3',
        },
      },
    },
  },
});

export default theme;
