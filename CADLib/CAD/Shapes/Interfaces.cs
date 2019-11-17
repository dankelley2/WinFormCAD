using System.Collections.Generic;
using System.Drawing;

namespace CAD
{

    public interface iSnappable
    {
        List<PointF> GetSnapPoints();
    }

    public interface iClickable
    {
        double GetDistanceFromPoint(PointF P);
        bool IntersectsWithCircle(PointF CPoint, float CRad);
    }

    public interface iFillable
    {
        Color FillColor { get; set; }
        void DrawFill(Graphics g);
    }
}
