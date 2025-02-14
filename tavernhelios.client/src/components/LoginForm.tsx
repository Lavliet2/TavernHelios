import React, { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { TextField, Button, Box, Typography, Paper } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../contexts/AuthContext";
import Logo from "../assets/logo_login_bg.png";
import { ThemeProvider } from "@mui/material/styles";
import Theme from "../styles/theme";
import { API_BASE_URL } from "../config";

const LoginForm: React.FC = () => {
  const { t } = useTranslation();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();
  const { login } = useAuth();

  // ❌ Комментируем/удаляем YaAuthSuggest, чтобы не использовать Implicit Flow
  /*
  useEffect(() => {
    window.YaAuthSuggest.init(
      {
        client_id: "5c9531b9a1cb48628ffb70ea2582afc8",
        response_type: "token",
        redirect_uri: `${API_BASE_URL}/yandexAuth/login`
      },
      `${API_BASE_URL}`,
      {
        view: "button",
        parentId: "yandexAuth",
        buttonSize: 'm',
        buttonView: 'main',
        buttonTheme: 'light',
        buttonBorderRadius: "8",
        buttonIcon: 'ya',
      }
    ).then(({ handler }: any) => handler())
  }, []);
  */

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    // Заглушка для локального логина/пароля
    if (username === "test" && password === "test") {
      localStorage.setItem("username", username);
      login(); 
      navigate("/");
    } else {
      setError("Неверные данные");
    }
  };

  return (
    <ThemeProvider theme={Theme}>
      <Box
        sx={{
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          height: "100vh",
        }}
      >
        <Paper
          sx={{
            padding: 4,
            maxWidth: 400,
            width: "100%",
            borderRadius: 2,
            boxShadow: 6,
            backgroundColor: "background.paper",
          }}
        >
          <img
            src={Logo}
            alt="Taverna Helios Logo"
            style={{
              height: "150px",
              display: "block",
              margin: "0 auto 20px",
            }}
          />
          <Typography
            variant="h5"
            align="center"
            sx={{ color: "secondary.main", marginBottom: 3 }}
          >
            Tavern Helios
          </Typography>

          {error && (
            <Typography color="error" align="center" sx={{ marginBottom: 2 }}>
              {error}
            </Typography>
          )}

          <form onSubmit={handleSubmit}>
            <TextField
              label={t("login")}
              variant="outlined"
              fullWidth
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              margin="normal"
              sx={{
                input: { color: 'text.primary' },
                '& .MuiInputLabel-root': { color: 'text.primary', zIndex: 1 },
                '& .MuiOutlinedInput-root': {
                  '& fieldset': {
                    borderColor: 'primary.main',
                  },
                  '&:hover fieldset': { borderColor: 'primary.main' },
                  '&.Mui-focused fieldset': { borderColor: 'primary.main' },
                },
              }}
            />
            <TextField
              label={t("password")}
              type="password"
              variant="outlined"
              fullWidth
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              margin="normal"
              sx={{
                input: { color: 'text.primary' },
                '& .MuiInputLabel-root': { color: 'text.primary', zIndex: 1 },
                '& .MuiOutlinedInput-root': {
                  '& fieldset': {
                    borderColor: 'primary.main',
                  },
                  '&:hover fieldset': { borderColor: 'primary.main' },
                  '&.Mui-focused fieldset': { borderColor: 'primary.main' },
                },
              }}
            />
            <Button
              type="submit"
              variant="contained"
              color="primary"
              fullWidth
              sx={{ marginTop: 2 }}
            >
              {t("signIn")}
            </Button>
          </form>

          {/* Вот кнопка, которая ведёт на серверный /yandexAuth/login */}
          <Button
            variant="contained"
            color="secondary"
            fullWidth
            sx={{ marginTop: 2 }}
            onClick={() => {
              window.location.href = `${API_BASE_URL}/yandexAuth/login`;
            }}
          >
            {t("signInWithYandex")}
          </Button>

          {/* Убираем/комментируем div с id='yandexAuth' */}
          {/* <div style={{marginTop: "10px"}} id='yandexAuth'></div> */}
        </Paper>
      </Box>
    </ThemeProvider>
  );
};

export default LoginForm;
