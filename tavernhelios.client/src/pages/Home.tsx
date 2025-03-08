import { Typography, Box, Grid2, Container } from '@mui/material';
import axios from 'axios';
import { useEffect } from 'react';
import { API_BASE_URL } from '../config';
import { useUser } from '../contexts/UserContext';

function Home() {
    const userContext = useUser();

    useEffect(() => {
        axios.get(`${API_BASE_URL}/api/auth/userInfo`)
            .then(response => userContext?.login(response.data));
    }, []) 

  return (
    <Box sx={{ padding: 2 }}>
        <Container maxWidth="lg" sx={{ marginTop: 4 }}>
            <Typography variant="h4" color="primary" align="center" sx={{ marginBottom: 2 }}>
                Привет, команда Helios!Test
            </Typography>
            <Grid2 container spacing={2} justifyContent="center">
                <Grid2>
                    <Typography variant="body1" align="center">
                        Приложение успешно задеплоено на сервер и работает корректно.
                    </Typography>
                    <Typography variant="body2" color="textSecondary" align="center">
                        При мердже или пуше в ветку main обновления будут автоматически деплоиться сюда.
                    </Typography>
                </Grid2>
            </Grid2>
        </Container>
    </Box>
  );
}

export default Home;
