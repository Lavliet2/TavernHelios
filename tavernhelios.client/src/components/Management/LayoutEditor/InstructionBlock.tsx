import React, { useState } from "react";
import { Box, Typography, Collapse, Button } from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import ExpandLessIcon from "@mui/icons-material/ExpandLess";

export const InstructionBlock: React.FC = () => {
  const [open, setOpen] = useState(false);

  return (
    <Box sx={{ mt: 3 }}>
      <Button
        variant="text"
        onClick={() => setOpen(!open)}
        endIcon={open ? <ExpandLessIcon /> : <ExpandMoreIcon />}
        fullWidth
        sx={{ justifyContent: "flex-start", textTransform: "none" }}
      >
        {open ? "Скрыть инструкцию" : "Показать инструкцию"}
      </Button>

      <Collapse in={open}>
        <Box sx={{ p: 2, bgcolor: "#eee", borderRadius: 1 }}>
          {/* <Typography variant="subtitle2" gutterBottom>Инструкция по использованию</Typography> */}

          <Typography variant="body2" gutterBottom>
            <strong>Редактировать</strong> — включает режим изменения схемы. Только в этом режиме можно добавлять, перемещать и удалять объекты.
          </Typography>

          <Typography variant="body2" gutterBottom>
            <strong>Сохранить</strong> — сохраняет текущие изменения в выбранной схеме зала.
          </Typography>

          <Typography variant="body2" gutterBottom>
            🔲 Чтобы добавить <strong>стол</strong>:
          </Typography>
            <ul style={{ marginTop: 4, marginBottom: 4 }}>
              <li>Введите уникальное <strong>имя стола</strong>.</li>
              <li>Укажите ширину, высоту и количество мест.</li>
              <li>Если имя не задано или уже существует — стол <strong>не появится</strong> в панели и его нельзя будет добавить.</li>
              <li>После ввода перетащите стол на схему.</li>
            </ul>

          <Typography variant="body2" gutterBottom>
            🪑 Чтобы добавить <strong>стул</strong>:
          </Typography>
            <ul style={{ marginTop: 4, marginBottom: 4 }}>
              <li>Имя стола должно быть задано.</li>
              <li>Вы можете добавить не более указанного количества мест.</li>
              <li>Если все места уже добавлены — стул <strong>не появится</strong> в панели.</li>
              <li>Радиус стул не может быть меньше 5.</li>
              <li>Перетащите стул на нужное место около стола.</li>
            </ul>

          <Typography variant="body2" gutterBottom>
            ❌ <strong>Удаление объектов</strong>:
          </Typography>
            <ul style={{ marginTop: 4, marginBottom: 4 }}>
              <li>Нажмите <strong>правой кнопкой мыши</strong> по столу или стулу на схеме.</li>
              <li>Выберите <strong>"Удалить"</strong> в контекстном меню.</li>
            </ul>

          <Typography variant="body2">
            🖱️ Вы можете <strong>перетаскивать</strong> объекты по схеме в режиме редактирования.
          </Typography>

          <Typography variant="body2">
            ⚠️ Не забудьте сохранить схему после внесения изменений! Для отмены перезагрузите страницу.
          </Typography>
        </Box>
      </Collapse>
    </Box>
  );
};
