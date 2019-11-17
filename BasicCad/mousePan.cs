using System.Drawing;
using CAD;

namespace BasicCad
{
    public class mousePan
    {
        public PointF start;
        private PointF delta;
        public PointF end
        {
            set
            {
                delta =
                    new PointF(value.X - start.X, value.Y - start.Y);
                ShapeSystem.gridSystem.MoveOriginByDelta_Real(delta);
            }
        }
        //GridSystem grid;
        public mousePan(PointF start)
        {
            this.start = start;
            //this.grid = grid;
        }
        public static mousePan mousePanGrid(PointF start)
        {
            return new mousePan(start);
        }
    }
}