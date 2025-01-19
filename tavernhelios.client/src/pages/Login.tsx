import { Typography, Box, TextField, Button } from '@mui/material';
import { useEffect } from 'react';

function Login() {
  useEffect(() => {
    window.YaAuthSuggest.init(
      {
        client_id: "5c9531b9a1cb48628ffb70ea2582afc8",
        response_type: "token",
        redirect_uri: "https://localhost:63049"
      },
      "https://localhost",
      {
        view: "button",
        parentId: "yandexAuth",
        buttonSize: 'm',
        buttonView: 'main',
        buttonTheme: 'light',
        buttonBorderRadius: "8",
        buttonIcon: 'ya',
      }
    )
      .then(({ handler }) => handler())
      .then(data => console.log('Сообщение с токеном', data))
      .catch(error => console.log('Обработка ошибки', error))
  }, [])

  return (
    <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
      <Box sx={{ maxWidth: 400, width: '100%' }}>
        <Typography variant="h4" align="center">Вход</Typography>
        <TextField label="Логин" fullWidth sx={{ marginBottom: 2 }} />
        <TextField label="Пароль" type="password" fullWidth sx={{ marginBottom: 2 }} />
        <Button variant="contained" color="primary" fullWidth>Войти</Button>
        <div style={{marginTop: "10px"}} id='yandexAuth'></div>
      </Box>
    </Box>
  );
}

// export default Login;
