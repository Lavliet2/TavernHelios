import { useEffect, useRef } from "react";
import { DroppedObject } from "../../types/DroppedObject";

interface UseCanvasRenderLoopParams {
  canvasRef: React.RefObject<HTMLCanvasElement | null>;
  backgroundImgRef: React.RefObject<HTMLImageElement | null>;
  objects: DroppedObject[];
  drawCanvas: (
    canvas: HTMLCanvasElement,
    image: HTMLImageElement | null,
    objects: DroppedObject[]
  ) => void;
}

export const useCanvasRenderLoop = ({
  canvasRef,
  backgroundImgRef,
  objects,
  drawCanvas,
}: UseCanvasRenderLoopParams) => {
  const animationRef = useRef<number | undefined>(undefined);


  useEffect(() => {
    const render = () => {
      if (canvasRef.current) {
        drawCanvas(canvasRef.current, backgroundImgRef.current, objects);
      }
      animationRef.current = requestAnimationFrame(render);
    };

    animationRef.current = requestAnimationFrame(render);
    return () => cancelAnimationFrame(animationRef.current!);
  }, [canvasRef, backgroundImgRef, objects, drawCanvas]);
};
