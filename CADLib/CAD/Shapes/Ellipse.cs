using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace CAD
{
    [Serializable]
    public class Ellipse : Shape, iClickable, iSnappable
    {
        public PointF P1;
        public float R;
        public Ellipse()
        {

        }
        public Ellipse(PointF P1, float R)
        {
            this.P1 = P1;
            this.R = R;
            this.MetaName = "Ellipse";
            this.MetaDesc = P1.X.ToString() + "," + P1.Y.ToString() + "; R: " + R.ToString();
            ShapeSystem.AddShapeToDataSet(this);
        }
        public Ellipse(PointF P1, float R, Shape Parent)
        {
            this.P1 = P1;
            this.R = R;
            this.ParentId = Parent.IdShape;
            this.MetaName = "Ellipse";
            this.MetaDesc = P1.X.ToString() + "," + P1.Y.ToString() + "; R: " + R.ToString();
            ShapeSystem.AddShapeToDataSet(this);
        }
        public static void AddEllipse(List<float> Input)
        {
            if (Input.Count() == 1)
            {
                PointF P1 = ShapeSystem.gridSystem.cursorPosition;
                float R = Input[0];
                Ellipse retCircle = new Ellipse(P1, R);
                Console.WriteLine("Ellipse " + retCircle.IdShape.ToString() + " created as {0}", retCircle.MetaDesc);
                ShapeSystem.UpdateSnapPoints();
                return;
            }
            else if (Input.Count() == 3)
            {
                PointF P1 = new PointF(Input[0], Input[1]);
                float R = Input[2];
                Ellipse retCircle = new Ellipse(P1, R);
                Console.WriteLine("Ellipse " + retCircle.IdShape.ToString() + " created as {0}", retCircle.MetaDesc);
                ShapeSystem.UpdateSnapPoints();
                return;
            }
            else
            {
                Console.WriteLine("Invalid input for Ellipse.");
            }
        }
        public override void Dimension()
        {
            throw new NotImplementedException();
        }
        public bool IntersectsWithCircle(PointF CPoint, float CRad)
        {
            //Distance between two points
            var dist = Math.Abs(Math.Sqrt(Math.Pow((CPoint.X - P1.X), 2) +
                                    Math.Pow((CPoint.Y - P1.Y), 2)) - R);
            if (dist <= CRad + .03)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public double GetDistanceFromPoint(PointF P)
        {
            //Check distance from nearest point
            return (double)Math.Abs(Math.Sqrt(Math.Pow((P.X - P1.X), 2) +
                                    Math.Pow((P.Y - P1.Y), 2)) - R);
        }
        public List<PointF> GetSnapPoints()
        {

            List<PointF> snaps = new List<PointF>();
            snaps.Add(new PointF(P1.X, P1.Y - R));
            snaps.Add(new PointF(P1.X, P1.Y + R));
            snaps.Add(new PointF(P1.X - R, P1.Y));
            snaps.Add(new PointF(P1.X + R, P1.Y));
            snaps.Add(P1);
            foreach (Shape S in ShapeSystem.ShapeList.Where(s => s is Line))
            {
                Line L = (Line)S;
                PointF potential1 = new PointF();
                PointF potential2 = new PointF();
                int result = ShapeSystem.FindLineCircleIntersections(P1, R, L.P1, L.P2, out potential1, out potential2);
                if (result == 2)
                {
                    snaps.Add(potential1);
                    snaps.Add(potential2);
                }
                else if (result == 1)
                {
                    snaps.Add(potential1);
                }
            }
            return snaps;
        }
        public override void Draw(Graphics g)
        {
            PointF origin = ShapeSystem.gridSystem.realizePoint(new PointF(P1.X - R, P1.Y - R));
            SizeF size = ShapeSystem.gridSystem.realizeSize(new SizeF(R * 2, R * 2));
            if (isActiveShape)
            {
                g.DrawEllipse(ShapeSystem.penActive, new RectangleF(origin, size));
            }
            else
            {
                g.DrawEllipse(ShapeSystem.penBasic, new RectangleF(origin, size));
            }
        }
    }
}
