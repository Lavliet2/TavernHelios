import { useEffect, useRef } from "react";
import { DroppedObject } from "../../../types/DroppedObject";

interface ReservedSeat {
  seatNumber: number;
  tableName: string;
  personId: string;
}

interface UseCanvasRenderLoopParams {
  canvasRef: React.RefObject<HTMLCanvasElement | null>;
  backgroundImgRef: React.RefObject<HTMLImageElement | null>;
  objects: DroppedObject[];
  reservedSeats: ReservedSeat[];
  drawCanvas: (
    canvas: HTMLCanvasElement,
    image: HTMLImageElement | null,
    objects: DroppedObject[],
    reservedSeats: ReservedSeat[]
  ) => void;
}

export const useCanvasRenderLoop = ({
  canvasRef,
  backgroundImgRef,
  objects,
  reservedSeats,
  drawCanvas,
}: UseCanvasRenderLoopParams) => {
  const animationRef = useRef<number>(0);

  useEffect(() => {
    const render = () => {
      const canvas = canvasRef.current;
      const img = backgroundImgRef.current;
      if (canvas) {
        drawCanvas(canvas, img, objects, reservedSeats);
      }
      animationRef.current = requestAnimationFrame(render);
    };

    render();

    return () => {
      if (animationRef.current) {
        cancelAnimationFrame(animationRef.current);
      }
    };
  }, [drawCanvas, objects, reservedSeats]);
};
