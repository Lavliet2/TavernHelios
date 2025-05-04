import React, { useContext } from 'react';
import { AppBar, Toolbar, Typography, Button, IconButton, Menu, MenuItem, Box } from '@mui/material';
import { AccountCircle } from '@mui/icons-material';
import { useTranslation } from 'react-i18next';
import { ThemeProvider } from '@mui/material/styles';
import { Link, useNavigate } from 'react-router-dom';
import Logo from "../assets/logo_login_bg.webp";
import Theme from '../styles/theme';
import { LanguageContext } from '../contexts/LanguageContext';
// import WorldFlag from 'react-world-flags';
import ruFlag from "@/assets/flags/ru.svg";
import usFlag from "@/assets/flags/us.svg";
import axios from 'axios';
import { API_BASE_URL } from '../config';
import { useUser } from '../contexts/UserContext';


const NavigationBar: React.FC = () => {
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
  const [languageMenuAnchor, setLanguageMenuAnchor] = React.useState<null | HTMLElement>(null);
  const { t, i18n } = useTranslation();
  const { changeLanguage } = useContext(LanguageContext) || {};
  const navigate = useNavigate();
  const appVersion = import.meta.env.VITE_APP_VERSION || 'Unknown Version';
  const userContext = useUser();
  console.log("App Version:", import.meta.env.VITE_APP_VERSION);
  const flags = {
    ru: ruFlag,
    us: usFlag,
  };



  const handleMenuClick = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMenuLogout = () => {
    setAnchorEl(null);
    axios.post(`${API_BASE_URL}/api/auth/logout`)
      .then(() => userContext?.logout())
      .then(() => navigate("/login"));
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleLanguageMenuClick = (event: React.MouseEvent<HTMLElement>) => {
    setLanguageMenuAnchor(event.currentTarget);
  };

  const handleLanguageMenuClose = () => {
    setLanguageMenuAnchor(null);
  };

  const handleLanguageChange = (lang: string) => {
    if (changeLanguage) {
      changeLanguage(lang);
    }
    localStorage.setItem('language', lang);
    handleLanguageMenuClose();
  };

  return (
    <ThemeProvider theme={Theme}>
      <AppBar position="sticky">
        <Toolbar sx={{ display: 'flex', justifyContent: 'space-between', paddingX: 3 }}>
          <Box sx={{ display: 'flex', alignItems: 'center' }}>
            <img src={Logo} alt="Tavern Helios Logo" style={{ height: '40px', marginRight: '10px' }} />
            <Typography variant="h6">Tavern Helios</Typography>
          </Box>
          <Box sx={{ display: 'flex', ml: 2 }}>
            <Button color="inherit" component={Link} to="/">{t('home')}</Button>
            <Button color="inherit" component={Link} to="/menu">{t('menu')}</Button>
            <Button color="inherit" component={Link} to="/forecast">{t('forecast')}</Button>
            {userContext?.user?.isAdmin &&
              <Button color="inherit" component={Link} to="/management">{t('managementMenu')}</Button>
            }
            <Button color="inherit" component={Link} to="/about">{t('about')}</Button>
          </Box>
          <Typography variant="body2" color="inherit" sx={{ marginRight: '10px' }}>
            Version: {appVersion}
          </Typography>
          <Box sx={{ ml: 'auto', display: 'flex', alignItems: 'center' }}>
            {/* <Button color="inherit" onClick={handleLanguageMenuClick}> */}
            {/* Отображаем флаг текущего языка */}
            {/* <WorldFlag code={i18n.language === 'ru' ? 'RU' : 'US'} style={{ width: '20px', marginRight: '5px' }} /> */}
            {/* {i18n.language === 'ru' ? 'RU' : 'EN'} */}
            {/* </Button> */}
            <Menu
              anchorEl={languageMenuAnchor}
              open={Boolean(languageMenuAnchor)}
              onClose={handleLanguageMenuClose}
              anchorOrigin={{
                vertical: 'top',
                horizontal: 'right',
              }}
              transformOrigin={{
                vertical: 'top',
                horizontal: 'right',
              }}
            >
              <MenuItem onClick={() => handleLanguageChange('ru')}>
                <img src={flags.ru} alt="Russian Flag" style={{ width: "20px", marginRight: "10px" }} />
                RU
              </MenuItem>
              <MenuItem onClick={() => handleLanguageChange('en')}>
                <img src={flags.us} alt="US Flag" style={{ width: "20px", marginRight: "10px" }} />
                EN
              </MenuItem>
            </Menu>
            <span style={{ fontSize: "20px" }}>{userContext?.user?.fullName}</span>

            <IconButton
              size="large"
              edge="end"
              color="inherit"
              aria-label="account of current user"
              aria-controls="menu-appbar"
              aria-haspopup="true"
              onClick={handleMenuClick}
            >
              <AccountCircle />
            </IconButton>
          </Box>
          <Menu
            id="menu-appbar"
            anchorEl={anchorEl}
            anchorOrigin={{
              vertical: 'top',
              horizontal: 'right',
            }}
            keepMounted
            transformOrigin={{
              vertical: 'top',
              horizontal: 'right',
            }}
            open={Boolean(anchorEl)}
            onClose={handleMenuClose}
          >
            <Button color="inherit" onClick={handleLanguageMenuClick}>
              <img src={i18n.language === "ru" ? flags.ru : flags.us}
                alt={i18n.language === "ru" ? "Russian Flag" : "US Flag"}
                style={{ width: "20px", marginRight: "5px" }} />
              {i18n.language === "ru" ? "RU" : "EN"}
            </Button>
            <MenuItem onClick={handleMenuClose}>{t('profile')}</MenuItem>
            <MenuItem onClick={handleMenuLogout}>{t('logout')}</MenuItem>
          </Menu>
          {/* <h1>Application Version: {appVersion}</h1> */}
        </Toolbar>
      </AppBar>
    </ThemeProvider>
  );
};

export default NavigationBar;
