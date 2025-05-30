import React from 'react';
import {useState} from 'react';
import { Dialog, DialogTitle, DialogContent, IconButton, Box, Button, Typography } from '@mui/material';
import CloseIcon from '@mui/icons-material/Close';
import LayoutEditor from '../../../pages/Management/Layout/EditLayout';

interface TableLayoutModalProps {
  open: boolean;
  onClose: () => void;
  selectedTime: string;
  onSelectSeat: (seatNumber: number, tableName: string, layoutId: string) => void;
}

const TableLayoutModal: React.FC<TableLayoutModalProps> = ({
  open, onClose, selectedTime, onSelectSeat
}) => {
  const [tempSeat, setTempSeat] = useState<{
    seatNumber: number;
    tableName: string;
    layoutId: string;
  } | null>(null);
  return (
    <Dialog open={open} onClose={onClose} fullScreen>
      <DialogTitle>
        Выбор места
        <IconButton
          aria-label="close"
          onClick={onClose}
          sx={{ position: 'absolute', right: 8, top: 8 }}
        >
          <CloseIcon />
        </IconButton>
      </DialogTitle>
      <DialogContent sx={{ paddingBottom: '100px', position: 'relative' }}>
        <LayoutEditor
          selectionMode={true}
          selectedTime={selectedTime}
          onSelectSeat={(seatNumber, tableName, layoutId) => {
            setTempSeat({ seatNumber, tableName, layoutId });
          }}
        />
        <Box
          sx={{
            position: 'fixed',
            bottom: 0,
            left: 0,
            width: '100%',
            backgroundColor: 'rgba(255, 255, 255, 0.95)',
            borderTop: '1px solid #ccc',
            p: 2,
            textAlign: 'center',
            zIndex: 10,
          }}
        >
          <Typography variant="body2" color="text.secondary" gutterBottom>
            Дата: <strong>{new Date().toLocaleDateString()}</strong>, время: <strong>{selectedTime}</strong>
          </Typography>
          {tempSeat ? (
            <>
              <Typography variant="body1" gutterBottom>
                Вы выбрали: стол <strong>{tempSeat.tableName}</strong>, место <strong>{tempSeat.seatNumber}</strong>
              </Typography>
            </>
          ) : (
            <Typography variant="body2" gutterBottom color="text.secondary">
              Выберите свободное место, чтобы продолжить
            </Typography>
          )}

          <Button
            variant="contained"
            color="primary"
            disabled={!tempSeat}
            onClick={() => {
              if (tempSeat) {
                onSelectSeat(tempSeat.seatNumber, tempSeat.tableName, tempSeat.layoutId);
                onClose();
              }
            }}
          >
            Подтвердить выбор
          </Button>
        </Box>
      </DialogContent>
    </Dialog>
  );
};

export default TableLayoutModal;

