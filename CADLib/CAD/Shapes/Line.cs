using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CAD
{
    [Serializable]
    public class Line : Shape, iSnappable, iClickable
    {
        public double slope;
        public PointF P1
        {
            get
            {
                return Points[0];
            }
        }
        public PointF P2
        {
            get
            {
                return Points[1];
            }
        }
        public Line()
        {

        }
        public Line(PointF p1, PointF p2)
        {
            this.Points.Add(p1);
            this.Points.Add(p2);
            this.ParentId = -1;
            this.MetaName = "Line";
            object[] MetaDescArray = { this.Points[0].X, this.Points[0].Y, this.Points[1].X, this.Points[1].Y };
            this.MetaDesc = string.Format("({0},{1}):({2},{3})", MetaDescArray);
            ShapeSystem.AddShapeToDataSet(this);
            this.slope = (Points[1].Y - Points[0].Y) / (Points[1].X - Points[0].X);
        }
        //Line with Parent
        public Line(PointF p1, PointF p2, Shape parent)
        {
            this.Points.Add(p1);
            this.Points.Add(p2);
            this.ParentId = parent.IdShape;
            this.MetaName = "Line";
            object[] MetaDescArray = { this.Points[0].X, this.Points[0].Y, this.Points[1].X, this.Points[1].Y };
            this.MetaDesc = string.Format("({0},{1}):({2},{3})", MetaDescArray);
            ShapeSystem.AddShapeToDataSet(this);
            this.slope = (Points[1].Y - Points[0].Y) / (Points[1].X - Points[0].X);
        }

        public static void AddLine(List<float> Input)
        {
            if (ShapeSystem.gridSystem.relativePositioning && Input.Count == 2)
            {
                PointF p1 = new PointF(ShapeSystem.gridSystem.cursorPosition.X, ShapeSystem.gridSystem.cursorPosition.Y);

                float x2 = (Input[0] + p1.X);
                float y2 = (Input[1] + p1.Y);
                PointF p2 = new PointF(x2, y2);
                Line RetLine = new Line(p1, p2);
                ShapeSystem.gridSystem.cursorPosition = p2;
                Console.WriteLine("Line " + RetLine.IdShape.ToString() + " created as {0}", RetLine.MetaDesc);
                ShapeSystem.UpdateSnapPoints();
                return;
            }
            if (Input.Count != 4)
            {
                Console.WriteLine("Invalid input.");
                return;
            }
            else
            {
                float x1 = (Input[0]);
                float y1 = (Input[1]);
                float x2 = (Input[2]);
                float y2 = (Input[3]);
                PointF p1 = new PointF(x1, y1);
                PointF p2 = new PointF(x2, y2);
                Line RetLine = new Line(p1, p2);
                Console.WriteLine("Line " + RetLine.IdShape.ToString() + " created as {0}", RetLine.MetaDesc);
                ShapeSystem.gridSystem.cursorPosition = p2;
                ShapeSystem.UpdateSnapPoints();
                return;
            }
        }

        public override void Dimension()
        {
            new LinearDimension(this, .5F);
        }

        public List<PointF> GetSnapPoints()
        {
            PointF P3 = GetFractionOfLine(Points[0], Points[1], .5F);
            return new List<PointF>() { Points[0], Points[1], P3 };
        }

        public bool IntersectsWithCircle(PointF CPoint, float CRad)
        {
            PointF A = Points[0];
            PointF B = Points[1];
            //Distance between two points
            var d = Math.Sqrt(Math.Pow((B.X - A.X), 2) +
                                    Math.Pow((B.Y - A.Y), 2));
            //Get nearest of C
            var a = (1 / Math.Pow(d, 2)) *
                    (
                        ((B.X - A.X) * (CPoint.X - A.X)) +
                        ((B.Y - A.Y) * (CPoint.Y - A.Y))
                    );
            PointF nearest = new PointF();
            nearest.X = (float)(A.X + (B.X - A.X) * a);
            nearest.Y = (float)(A.Y + (B.Y - A.Y) * a);

            //Check if point within circle
            var distToPoint = Math.Sqrt(Math.Pow((CPoint.X - nearest.X), 2) +
                                    Math.Pow((CPoint.Y - nearest.Y), 2));
            if (distToPoint <= CRad)
            {

                //line intersects, but check in line segment
                var ma = Math.Sqrt(Math.Pow((A.X - nearest.X), 2) +
                                        Math.Pow((A.Y - nearest.Y), 2));
                var mb = Math.Sqrt(Math.Pow((B.X - nearest.X), 2) +
                                        Math.Pow((B.Y - nearest.Y), 2));

                //line intersects, but check in line segment
                var ac = Math.Sqrt(Math.Pow((CPoint.X - nearest.X), 2) +
                                        Math.Pow((CPoint.Y - A.Y), 2));
                var bc = Math.Sqrt(Math.Pow((CPoint.X - nearest.X), 2) +
                                        Math.Pow((CPoint.Y - nearest.Y), 2));
                if (ma <= d && mb <= d)
                {
                    if (ac <= CRad || bc <= CRad)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static PointF GetFractionOfLine(PointF p1, PointF p2, float frac)
        {
            return new PointF(p1.X + frac * (p2.X - p1.X),
                               p1.Y + frac * (p2.Y - p1.Y));
        }

        public double GetDistanceFromPoint(PointF P)
        {
            PointF A = Points[0];
            PointF B = Points[1];
            //Distance between two points
            var d = Math.Sqrt(Math.Pow((B.X - A.X), 2) +
                                    Math.Pow((B.Y - A.Y), 2));
            //Get nearest of |AB|
            var a = (1 / Math.Pow(d, 2)) *
                    (
                        ((B.X - A.X) * (P.X - A.X)) +
                        ((B.Y - A.Y) * (P.Y - A.Y))
                    );
            PointF nearest = new PointF();
            nearest.X = (float)(A.X + (B.X - A.X) * a);
            nearest.Y = (float)(A.Y + (B.Y - A.Y) * a);

            //Check distance from nearest point
            return (double)(Math.Sqrt(Math.Pow((P.X - nearest.X), 2) +
                                    Math.Pow((P.Y - nearest.Y), 2)));

        }

        public override void Draw(Graphics g)
        {
            if (isActiveShape)
            {
                AdjustableArrowCap bigArrow = new AdjustableArrowCap(5, 5);
                ShapeSystem.penActive.StartCap = LineCap.RoundAnchor;
                ShapeSystem.penActive.CustomEndCap = bigArrow;
                g.DrawLine(ShapeSystem.penActive, ShapeSystem.gridSystem.realizePoint(Points[0]), ShapeSystem.gridSystem.realizePoint(Points[1]));
            }
            else
                g.DrawLine(ShapeSystem.penBasic, ShapeSystem.gridSystem.realizePoint(Points[0]), ShapeSystem.gridSystem.realizePoint(Points[1]));

        }
    }
}
