using System;
using System.Collections.Generic;
using System.Drawing;

namespace CAD
{
    [Serializable]
    public class Rect : Shape, iSnappable, iFillable
    {
        public Line AB;
        public Line BC;
        public Line CD;
        public Line DA;
        public Color FillColor { get; set; }

        public Rect()
        {

        }
        public Rect(PointF p1, PointF p2, PointF p3, PointF p4)
        {
            this.AB = new Line(p1, p2, (Shape)this);
            this.BC = new Line(p2, p3, (Shape)this);
            this.CD = new Line(p3, p4, (Shape)this);
            this.DA = new Line(p4, p1, (Shape)this);

            this.ParentId = -1;
            this.MetaName = "Rect";
            object[] MetaDescArray = { AB.IdShape, BC.IdShape, CD.IdShape, DA.IdShape };
            this.MetaDesc = string.Format("L{0}, L{1}, L{2}, L{3}", MetaDescArray);
            ShapeSystem.AddShapeToDataSet(this);
        }

        public override void Dimension()
        {
            throw new NotImplementedException();
        }

        public static void AddRect(List<float> Input)
        {
            //Width and height from cursor
            if (ShapeSystem.gridSystem.relativePositioning && Input.Count == 2)
            {
                PointF cursor = new PointF(ShapeSystem.gridSystem.cursorPosition.X, ShapeSystem.gridSystem.cursorPosition.Y);
                PointF p1 = new PointF(Math.Min(cursor.X, cursor.X + Input[0]), Math.Min(cursor.Y, cursor.Y + Input[1]));
                PointF p2 = new PointF(Math.Max(cursor.X, cursor.X + Input[0]), Math.Min(cursor.Y, cursor.Y + Input[1]));
                PointF p3 = new PointF(Math.Max(cursor.X, cursor.X + Input[0]), Math.Max(cursor.Y, cursor.Y + Input[1]));
                PointF p4 = new PointF(Math.Min(cursor.X, cursor.X + Input[0]), Math.Max(cursor.Y, cursor.Y + Input[1]));
                Rect RetRect = new Rect(p1, p2, p3, p4);
                ShapeSystem.gridSystem.cursorPosition = cursor;
                Console.WriteLine("Rectangle " + RetRect.IdShape.ToString() + " created as {0}", RetRect.MetaDesc);
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
                PointF pRef = new PointF(Input[0], Input[1]);
                PointF p1 = new PointF(Math.Min(pRef.X, pRef.X + Input[2]), Math.Min(pRef.Y, pRef.Y + Input[3]));
                PointF p2 = new PointF(Math.Max(pRef.X, pRef.X + Input[2]), Math.Min(pRef.Y, pRef.Y + Input[3]));
                PointF p3 = new PointF(Math.Max(pRef.X, pRef.X + Input[2]), Math.Max(pRef.Y, pRef.Y + Input[3]));
                PointF p4 = new PointF(Math.Min(pRef.X, pRef.X + Input[2]), Math.Max(pRef.Y, pRef.Y + Input[3]));

                Rect RetRect = new Rect(p1, p2, p3, p4);
                ShapeSystem.gridSystem.cursorPosition = pRef;
                Console.WriteLine("Rectangle " + RetRect.IdShape.ToString() + " created as {0}", RetRect.MetaDesc);
                ShapeSystem.UpdateSnapPoints();
                return;
            }
        }

        public List<PointF> GetSnapPoints()
        {
            PointF Middle = Line.GetFractionOfLine(AB.P1, BC.P2, .5F);
            return new List<PointF>() { Middle };
        }

        public void DrawFill(Graphics g)
        {
            if (this.FillColor == null) { return; }
            PointF p1 = ShapeSystem.gridSystem.realizePoint(AB.P1);
            PointF p2 = ShapeSystem.gridSystem.realizePoint(AB.P2);
            PointF p3 = ShapeSystem.gridSystem.realizePoint(CD.P2);
            RectangleF rect = new RectangleF(
                  p1.X
                , p1.Y
                , p2.X - p1.X
                , p3.Y - p1.Y
                );
            //path.CloseFigure();
            g.FillRectangle(new SolidBrush(FillColor), rect);
        }

        public override void Draw(Graphics g)
        {
            ;
        }
    }
}
