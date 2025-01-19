import React from 'react';
import { Box, Typography } from '@mui/material';
import { useTranslation } from 'react-i18next';

const Footer: React.FC = () => {
const { t } = useTranslation();
  return (
    <Box sx={{ textAlign: 'center', marginTop: 4 }}>
      <Typography variant="body2" color="textSecondary">
        {t('copy')}
      </Typography>
    </Box>
  );
};

export default Footer;