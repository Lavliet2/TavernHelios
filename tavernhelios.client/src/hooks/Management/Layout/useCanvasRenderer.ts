import { useEffect } from "react";
import { DroppedObject } from "../../../types/DroppedObject";

interface UseCanvasRendererParams {
    canvasRef: React.RefObject<HTMLCanvasElement | null>; 
    backgroundImgRef: React.RefObject<HTMLImageElement | null>; 
    objects: DroppedObject[];
    drawCanvas: (
      canvas: HTMLCanvasElement,
      image: HTMLImageElement | null,
      objects: DroppedObject[]
    ) => void;
  }
  

export const useCanvasRenderer = ({
  canvasRef,
  backgroundImgRef,
  objects,
  drawCanvas,
}: UseCanvasRendererParams) => {
  useEffect(() => {
    if (canvasRef.current) {
      drawCanvas(canvasRef.current, backgroundImgRef.current, objects);
    }
  }, [objects, canvasRef, backgroundImgRef, drawCanvas]);
};
