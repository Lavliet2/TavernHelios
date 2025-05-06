import React from 'react';
import { Dialog, DialogTitle, DialogContent, IconButton } from '@mui/material';
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
      <DialogContent>
        <LayoutEditor
          selectionMode={true}
          selectedTime={selectedTime}
          onSelectSeat={(seatNumber, tableName, layoutId) => {
            onSelectSeat(seatNumber, tableName, layoutId);
            onClose();
          }}
        />
      </DialogContent>
    </Dialog>
  );
};

export default TableLayoutModal;
