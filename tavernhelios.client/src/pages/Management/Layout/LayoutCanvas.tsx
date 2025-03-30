// src/components/Management/LayoutEditor/LayoutCanvas.tsx
import React from "react";
import { CANVAS_BORDER } from "../../../constants/layoutDefaults";

interface LayoutCanvasProps {
  canvasRef: React.RefObject<HTMLCanvasElement | null>;
  canvasSize: { width: number; height: number };
  dropRef: (element: HTMLElement | null) => void;
  onMouseDown: (e: React.MouseEvent<HTMLCanvasElement>) => void;
  onMouseMove: (e: React.MouseEvent<HTMLCanvasElement>) => void;
  onMouseUp: () => void;
  onMouseLeave: () => void;
  onContextMenu: (e: React.MouseEvent<HTMLDivElement>) => void;
}

const LayoutCanvas: React.FC<LayoutCanvasProps> = ({
  canvasRef,
  canvasSize,
  dropRef,
  onMouseDown,
  onMouseMove,
  onMouseUp,
  onMouseLeave,
  onContextMenu,
}) => {
  return (
    <div
      ref={dropRef}
      style={{
        flexGrow: 1,
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
      }}
      onContextMenu={onContextMenu}
    >
      <canvas
        ref={canvasRef}
        width={canvasSize.width}
        height={canvasSize.height}
        style={{ border: CANVAS_BORDER }}
        onMouseDown={onMouseDown}
        onMouseMove={onMouseMove}
        onMouseUp={onMouseUp}
        onMouseLeave={onMouseLeave}
      />
    </div>
  );
};

export default React.memo(LayoutCanvas);
