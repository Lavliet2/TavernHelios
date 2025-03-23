import React, { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { TextField, Button, Box, Typography, Paper } from "@mui/material";
import { useNavigate } from "react-router-dom";
// import { useAuth } from "../contexts/AuthContext";
import { useAuth } from "../hooks/useAuth";
import Logo from "../assets/logo_login_bg.webp";
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
  console.log(`Login form redirect_uri: ${API_BASE_URL}/yandexAuth/login/` )

  useEffect(() => {
    // @ts-ignore
    window.YaAuthSuggest.init(
      {
        client_id: "5c9531b9a1cb48628ffb70ea2582afc8",
        response_type: "token",
        redirect_uri: `${API_BASE_URL}/yandexAuth/login/`
      },
      `${API_BASE_URL}`, // ???
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
    // @ts-ignore
      .then(({ handler }) => handler())}, [])

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    // Заглушка для авторизации
    if (username === "test" && password === "test") {
      localStorage.setItem("username", username);
      login(); // Устанавливаем состояние аутентификации
      navigate("/"); // Переход на главную страницу
    } else {
      setError("Неверные данные");
    }
  };

  //Когда будет API
//   const handleSubmit = async (e: React.FormEvent) => {
//     e.preventDefault();
  
//     try {
//       const response = await fetch("/api/login", {
//         method: "POST",
//         headers: { "Content-Type": "application/json" },
//         body: JSON.stringify({ username, password }),
//       });
  
//       if (response.ok) {
//         login();
//         navigate("/");
//       } else {
//         setError("Неверные данные");
//       }
//     } catch (err) {
//       setError("Ошибка сервера. Попробуйте позже.");
//     }
//   };

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
                input: {
                color: 'text.primary',
                },
                '& .MuiInputLabel-root': {
                color: 'text.primary',
                zIndex: 1, 
                },
                '& .MuiOutlinedInput-root': {
                '& fieldset': {
                    borderColor: 'primary.main', 
                },
                '&:hover fieldset': {
                    borderColor: 'primary.main', 
                },
                '&.Mui-focused fieldset': {
                    borderColor: 'primary.main', 
                },
                },

                '& input:-webkit-autofill': {
                backgroundColor: 'transparent !important',
                color: 'text.primary !important',
                transition: 'background-color 5000s ease-in-out 0s, color 5000s ease-in-out 0s',
                zIndex: 0, 
                },
                '& input:-moz-autofill': {
                backgroundColor: 'transparent !important',
                color: 'text.primary !important',
                transition: 'background-color 5000s ease-in-out 0s, color 5000s ease-in-out 0s', 
                zIndex: 0, 
                },
                '& input:-webkit-autofill:focus': {
                backgroundColor: 'transparent !important',
                color: 'text.primary !important',
                },
                '& input:-moz-autofill:focus': {
                backgroundColor: 'transparent !important',
                color: 'text.primary !important',
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
                input: {
                color: 'text.primary', 
                },
                '& .MuiInputLabel-root': {
                color: 'text.primary', 
                zIndex: 1, 
                },
                '& .MuiOutlinedInput-root': {
                '& fieldset': {
                    borderColor: 'primary.main', 
                },
                '&:hover fieldset': {
                    borderColor: 'primary.main', 
                },
                '&.Mui-focused fieldset': {
                    borderColor: 'primary.main', 
                },
                },
                
                '& input:-webkit-autofill': {
                backgroundColor: 'transparent !important',
                color: 'text.primary !important',
                transition: 'background-color 5000s ease-in-out 0s, color 5000s ease-in-out 0s',
                zIndex: 0, 
                },
                '& input:-moz-autofill': {
                backgroundColor: 'transparent !important',
                color: 'text.primary !important',
                transition: 'background-color 5000s ease-in-out 0s, color 5000s ease-in-out 0s',
                zIndex: 0, 
                },
                '& input:-webkit-autofill:focus': {
                backgroundColor: 'transparent !important',
                color: 'text.primary !important',
                },
                '& input:-moz-autofill:focus': {
                backgroundColor: 'transparent !important',
                color: 'text.primary !important',
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

            <div style={{marginTop: "10px"}} id='yandexAuth'></div>
          </form>
        </Paper>
      </Box>
    </ThemeProvider>
  );
};

export default LoginForm;
