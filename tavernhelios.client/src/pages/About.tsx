import { Typography, Box, Container } from '@mui/material';

function About() {
  return (
    <Box sx={{ padding: 2 }}>
        <Container maxWidth="lg" sx={{ marginTop: 4 }}>
            <Typography variant="h5" color="primary" align="center" sx={{ marginBottom: 2 }}>
                Что-то о компании
            </Typography>
        </Container>
    </Box>
 
  );
}

export default About;
