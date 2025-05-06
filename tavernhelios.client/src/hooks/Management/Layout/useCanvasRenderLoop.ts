import { useEffect, useRef } from "react";
import { DroppedObject } from "../../../types/DroppedObject";

interface UseCanvasRenderLoopParams {
  canvasRef: React.RefObject<HTMLCanvasElement | null>;
  backgroundImgRef: React.RefObject<HTMLImageElement | null>;
  objects: DroppedObject[];
  drawCanvas: (
    canvas: HTMLCanvasElement,
    image: HTMLImageElement | null,
    objects: DroppedObject[],
    reservedSeats?: { seatNumber: number; tableName: string; personId: string }[] 
  ) => void;
  reservedSeats: { seatNumber: number; tableName: string; personId: string }[]
  // reservedSeats?: { seatNumber: number; tableName: string }[];
}


export const useCanvasRenderLoop = ({
  canvasRef,
  backgroundImgRef,
  objects,
  drawCanvas,
  reservedSeats, // ← добавь сюда
}: UseCanvasRenderLoopParams) => {
  const animationRef = useRef<number | undefined>(undefined);

  useEffect(() => {
    const render = () => {
      if (canvasRef.current) {
        drawCanvas(canvasRef.current, backgroundImgRef.current, objects, reservedSeats);
      }
      animationRef.current = requestAnimationFrame(render);
    };

    animationRef.current = requestAnimationFrame(render);
    return () => cancelAnimationFrame(animationRef.current!);
  }, [canvasRef, backgroundImgRef, objects, drawCanvas, reservedSeats]);
};

