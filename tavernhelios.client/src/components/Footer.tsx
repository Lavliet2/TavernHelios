import React from 'react';
import { Box, Typography } from '@mui/material';

const Footer: React.FC = () => {
  return (
    <Box sx={{ textAlign: 'center', marginTop: 4 }}>
      <Typography variant="body2" color="textSecondary">
        &copy; 2025 TavernHelios. Все права защищены.
      </Typography>
    </Box>
  );
};

export default Footer;