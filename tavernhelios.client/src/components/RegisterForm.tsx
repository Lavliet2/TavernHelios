import React, { useState } from "react";
import { TextField, Button, Box, Typography, Paper } from "@mui/material";
import Theme from "../styles/theme";
import { ThemeProvider } from "@mui/material/styles";
import axios from "axios";
import { API_BASE_URL } from "../config";
import { useNavigate } from "react-router-dom";
import { useUser } from "../contexts/UserContext";

const RegisterForm: React.FC = () => {
    const [fullName, setFullName] = useState("");
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [error, setError] = useState("");
    const navigate = useNavigate();
    const userContext = useUser();

    const handleSubmit = () => {
        const formIsValid = validateForm();
        if (formIsValid) {
            axios.post(`${API_BASE_URL}/api/auth/register`, { fullName, login, password, confirmPassword })
                .then(() => axios.post(`${API_BASE_URL}/api/auth/login`, { login, password }))
                .then((response) => {
                    userContext?.login(response.data);
                    navigate("/");
                });
        }
    };

    const validateForm = () => {
        if (!fullName) {
            setError("Заполните имя");
            return false;
        }
        if (!login) {
            setError("Заполните логин");
            return false;
        }
        if (!password || !confirmPassword) {
            setError("Заполните пароль");
            return false;
        }
        if (password != confirmPassword) {
            setError("Пароли не совпадают");
            return false;
        }
        return true;
    }

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

                    <Typography
                        variant="h5"
                        align="center"
                        sx={{ color: "secondary.main", marginBottom: 3 }}
                    >
                        Регистрация
                    </Typography>


                    <TextField
                        label={"Имя"}
                        variant="outlined"
                        fullWidth
                        value={fullName}
                        onChange={(e) => setFullName(e.target.value)}
                        margin="normal"
                    />
                    <TextField
                        label={"Логин"}
                        variant="outlined"
                        fullWidth
                        value={login}
                        onChange={(e) => setLogin(e.target.value)}
                        margin="normal"
                    />
                    <TextField
                        label={"Пароль"}
                        type="password"
                        variant="outlined"
                        fullWidth
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        margin="normal"
                    />
                    <TextField
                        label={"Подтвердите пароль"}
                        type="password"
                        variant="outlined"
                        fullWidth
                        value={confirmPassword}
                        onChange={(e) => setConfirmPassword(e.target.value)}
                        margin="normal"
                    />
                    <Button
                        type="submit"
                        variant="contained"
                        color="primary"
                        fullWidth
                        sx={{ marginTop: 2 }}
                        onClick={handleSubmit}
                    >
                        Зарегистрироваться
                    </Button>

                    {error && (
                        <Typography color="error" align="center" sx={{ marginTop: 2 }}>
                            {error}
                        </Typography>
                    )}

                </Paper>
            </Box>
        </ThemeProvider>
    );

};

export default RegisterForm;