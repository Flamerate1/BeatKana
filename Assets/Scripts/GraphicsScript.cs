using UnityEngine;

public class GraphicsScript : MonoBehaviour
{
    [SerializeField] LR[] lines;
    
    enum CanvasRef { LeftSide, RightSide, BothHorizontal }

    [System.Serializable] struct LR
    {
        public LineRenderer lineRenderer;
        public CanvasRef canvasRef;
    }

    void UpdateGraphics()
    {
        var x1 = GameManager.camWorldCorners[0].x;
        var x2 = GameManager.camWorldCorners[2].x;

        foreach (LR line in lines)
        {
            switch (line.canvasRef)
            {
                case CanvasRef.LeftSide:
                    LRChangeX(line.lineRenderer, 0, x1);
                    break;
                case CanvasRef.RightSide:
                    LRChangeX(line.lineRenderer, 1, x2);
                    break;
                case CanvasRef.BothHorizontal:
                    LRChangeX(line.lineRenderer, 0, x1);
                    LRChangeX(line.lineRenderer, 1, x2);
                    break;
            }
        }
    }

    void LRChangeX(LineRenderer line, int index, float x)
    {
        Vector3 pos = line.GetPosition(index);
        pos.x = x; line.SetPosition(index, pos);
    }

    private void OnEnable()
    {
        GameManager.OnResolutionChanged += UpdateGraphics;
    }

    private void OnDisable()
    {
        GameManager.OnResolutionChanged -= UpdateGraphics;
    }
}
