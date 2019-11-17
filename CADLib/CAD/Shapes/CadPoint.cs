using System;
using System.Collections.Generic;
using System.Drawing;

namespace CAD
{

    [Serializable]
    public class cadPoint : Shape, iSnappable, iClickable
    {
        public PointF P1;

        public cadPoint()
        {

        }
        public cadPoint(PointF p1)
        {
            this.P1 = p1;
            this.ParentId = -1;
            this.MetaName = "Point";
            object[] MetaDescArray = { this.P1.X, this.P1.Y };
            this.MetaDesc = string.Format("({0},{1})", MetaDescArray);
            ShapeSystem.AddShapeToDataSet(this);
        }
        public static void AddPoint(List<float> Input)
        {
            if (Input.Count == 0)
            {
                PointF p1 = ShapeSystem.gridSystem.cursorPosition;
                cadPoint newPoint = new cadPoint(p1);
                Console.WriteLine("Point " + newPoint.IdShape.ToString() + " created as {0}", newPoint.MetaDesc);
                ShapeSystem.UpdateSnapPoints();
                return;
            }
            else if (Input.Count == 2)
            {
                float x1 = (Input[0]);
                float y1 = (Input[1]);
                PointF p1 = new PointF(x1, y1);
                cadPoint newPoint = new cadPoint(p1);
                Console.WriteLine("Point " + newPoint.IdShape.ToString() + " created as {0}", newPoint.MetaDesc);
                ShapeSystem.UpdateSnapPoints();
                return;
            }
            else
            {
                Console.WriteLine("Invalid input.");
                return;
            }
        }

        public override void Dimension()
        {
            //new LineDimension(this, .5F);
        }

        public List<PointF> GetSnapPoints()
        {
            return new List<PointF>() { P1 };
        }

        public bool IntersectsWithCircle(PointF CPoint, float CRad)
        {
            //Distance between two points
            var dist = Math.Sqrt(Math.Pow((CPoint.X - P1.X), 2) +
                                    Math.Pow((CPoint.Y - P1.Y), 2));
            if (dist <= CRad + .0625)
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
            return (double)(Math.Sqrt(Math.Pow((P.X - P1.X), 2) +
                                    Math.Pow((P.Y - P1.Y), 2)));
        }

        public override void Draw(Graphics g)
        {
            if (isActiveShape)
            {
                //Cursor
                PointF C = ShapeSystem.gridSystem.realizePoint(new PointF(P1.X, P1.Y));
                g.DrawEllipse(new Pen(ShapeSystem.penActive.Color), new RectangleF(new PointF(C.X - 5, C.Y - 5), new SizeF(10, 10)));
            }
            else
            {
                //Cursor
                PointF C = ShapeSystem.gridSystem.realizePoint(new PointF(P1.X, P1.Y));
                g.DrawEllipse(ShapeSystem.penBasic, new RectangleF(new PointF(C.X - 5, C.Y - 5), new SizeF(10, 10)));
            }

        }
    }
}


