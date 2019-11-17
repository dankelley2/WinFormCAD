using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CAD
{

    [Serializable]
    public class LinearDimension : Shape, iClickable
    {
        public PointF P1 { get; set; }
        public PointF P2 { get; set; }
        public int direction = 1;
        public double slope
        {
            get
            {
                return (P2.Y - P1.Y) / (P2.X - P1.X);
            }
        }
        public float distanceFromLine { get; set; }
        private float leadingLineLength { get; set; }
        public float _leadingLineLength
        {
            get
            {
                return leadingLineLength;
            }
            set
            {
                leadingLineLength = Math.Max(.25F, value);
            }
        }
        public float dimInsetFromLeadingLine { get; set; }
        public float dimLength { get; set; }
        public string TxDisplay { get; set; }
        public Font TxFont { get; set; }


        public LinearDimension()
        {

        }
        public LinearDimension(Line L, float dist)
        {
            this.P1 = L.P1;
            this.P2 = L.P2;
            this.ParentId = L.IdShape;
            float x1 = this.P1.X;
            float y1 = this.P1.Y;
            float x2 = this.P2.X;
            float y2 = this.P2.Y;
            this.TxFont = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular);
            this.dimLength = (float)Math.Sqrt((Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
            this.MetaName = "Dim";
            object[] MetaDescArray = { L.MetaName, L.IdShape };
            this.MetaDesc = string.Format("{0} {1}", MetaDescArray);
            this.distanceFromLine = .0625F;
            this.leadingLineLength = dist;
            this.dimInsetFromLeadingLine = .125F;
            ShapeSystem.AddShapeToDataSet(this);

        }
        public LinearDimension(PointF P1, PointF P2)
        {
            this.P1 = P1;
            this.P2 = P2;
            this.ParentId = -1;
            float x1 = this.P1.X;
            float y1 = this.P1.Y;
            float x2 = this.P2.X;
            float y2 = this.P2.Y;
            this.TxFont = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular);
            this.dimLength = (float)Math.Sqrt((Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
            this.MetaName = "Dim";
            object[] MetaDescArray = { P1.X, P1.Y, P2.X, P2.Y };
            this.MetaDesc = string.Format("({0},{1}):({2},{3})", MetaDescArray);
            this.distanceFromLine = .0625F;
            this.leadingLineLength = .5F;
            this.dimInsetFromLeadingLine = .125F;
            ShapeSystem.AddShapeToDataSet(this);

        }
        public LinearDimension(List<float> Input)
        {
            float x1 = Input[0];
            float y1 = Input[1];
            float x2 = Input[2];
            float y2 = Input[3];
            this.P1 = new PointF(x1, y1);
            this.P2 = new PointF(x2, y2);
            this.ParentId = -1;
            this.TxFont = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular);
            this.dimLength = (float)Math.Sqrt((Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
            this.MetaName = "Dim";
            object[] MetaDescArray = { P1.X, P1.Y, P2.X, P2.Y };
            this.MetaDesc = string.Format("({0},{1}):({2},{3})", MetaDescArray);
            this.distanceFromLine = .0625F;
            this.leadingLineLength = .5F;
            this.dimInsetFromLeadingLine = .125F;
            ShapeSystem.AddShapeToDataSet(this);

        }
        public static void AddNewDim(List<float> Input)
        {
            LinearDimension retDim = new LinearDimension(Input);
        }
        public override void Dimension()
        {
            throw new NotImplementedException();
        }

        public static void AdjDim(List<float> Input)
        {
            Shape S = ShapeSystem.ShapeList.Where(s => s.IdShape == (int)Input[0]).FirstOrDefault();

            if (S is LinearDimension)
            {
                ((LinearDimension)S)._leadingLineLength = Input[1];
            }
        }

        public List<PointF> CalculateSidelines(PointF source, double slope, float dist, float len)
        {
            {
                // m is the slope of line, and the 
                // required Point lies distance l 
                // away from the source Point

                PointF P1 = new PointF();
                PointF P2 = new PointF();
                PointF P3 = new PointF();

                float insetfromlen = len - dimInsetFromLeadingLine;

                // slope is 0
                if (slope == 0)
                {
                    P1.X = source.X + (dist * direction);
                    P1.Y = source.Y;

                    P2.X = source.X + (len * direction);
                    P2.Y = source.Y;

                    P3.X = source.X + (len * direction);
                    P3.Y = source.Y;
                }

                // if slope is infinte
                else if (Double.IsInfinity((double)slope))
                {
                    P1.X = source.X;
                    P1.Y = source.Y + (dist * direction);

                    P2.X = source.X;
                    P2.Y = source.Y + (len * direction);

                    P3.X = source.X;
                    P3.Y = source.Y + (len * direction);
                }
                else
                {
                    float distx = (float)(dist / Math.Sqrt(1 + (slope * slope)));
                    float disty = (float)slope * distx;
                    float lenx = (float)(len / Math.Sqrt(1 + (slope * slope)));
                    float leny = (float)slope * lenx;
                    float insx = (float)(.1F / Math.Sqrt(1 + (slope * slope)));
                    float insy = (float)slope * .1F;
                    P1.X = source.X + (distx * direction);
                    P1.Y = source.Y + (disty * direction);
                    P2.X = source.X + (lenx * direction);
                    P2.Y = source.Y + (leny * direction);
                    P3.X = source.X + (lenx * direction);
                    P3.Y = source.Y + (leny * direction);
                }

                //return new List<PointF>() { P1, P2, GetFractionOfLine(P1, P2, .75F) };
                return new List<PointF>() { P1, P2, P3 };
            }
        }

        public bool IntersectsWithCircle(PointF CPoint, float CRad)
        {
            double dimSlope = (1 / this.slope) * -1;
            PointF A = CalculateSidelines(this.P1, dimSlope, distanceFromLine, leadingLineLength)[2];
            PointF B = CalculateSidelines(this.P2, dimSlope, distanceFromLine, leadingLineLength)[2];
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

        public double GetDistanceFromPoint(PointF P)
        {
            double dimSlope = (1 / this.slope) * -1;
            PointF A = CalculateSidelines(this.P1, dimSlope, distanceFromLine, leadingLineLength)[2];
            PointF B = CalculateSidelines(this.P2, dimSlope, distanceFromLine, leadingLineLength)[2];
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
            double dimSlope = (1 / this.slope) * -1;

            List<PointF> leaderLine1 = CalculateSidelines(P1, dimSlope, distanceFromLine, leadingLineLength);
            List<PointF> leaderLine2 = CalculateSidelines(P2, dimSlope, distanceFromLine, leadingLineLength);

            if (isActiveShape)
            {
                Pen AArrowPen = new Pen(ShapeSystem.penActive.Color);
                AArrowPen.CustomStartCap = ShapeSystem.penDimArrow.CustomStartCap;
                AArrowPen.CustomEndCap = ShapeSystem.penDimArrow.CustomEndCap;

                g.DrawLine(new Pen(ShapeSystem.penActive.Color), ShapeSystem.gridSystem.realizePoint(leaderLine1[0]), ShapeSystem.gridSystem.realizePoint(leaderLine1[1]));
                g.DrawLine(new Pen(ShapeSystem.penActive.Color), ShapeSystem.gridSystem.realizePoint(leaderLine2[0]), ShapeSystem.gridSystem.realizePoint(leaderLine2[1]));
                g.DrawLine(AArrowPen, ShapeSystem.gridSystem.realizePoint(leaderLine1[2]), ShapeSystem.gridSystem.realizePoint(leaderLine2[2]));

            }
            else
            {
                g.DrawLine(ShapeSystem.penDimension, ShapeSystem.gridSystem.realizePoint(leaderLine1[0]), ShapeSystem.gridSystem.realizePoint(leaderLine1[1]));
                g.DrawLine(ShapeSystem.penDimension, ShapeSystem.gridSystem.realizePoint(leaderLine2[0]), ShapeSystem.gridSystem.realizePoint(leaderLine2[1]));
                g.DrawLine(ShapeSystem.penDimArrow, ShapeSystem.gridSystem.realizePoint(leaderLine1[2]), ShapeSystem.gridSystem.realizePoint(leaderLine2[2]));
            }
            //dimLength string
            string dimLength = Math.Round((decimal)this.dimLength, 4).ToString() + "\"";
            SizeF dimLength_Size = g.MeasureString(dimLength, this.TxFont);
            PointF drawingPoint = Line.GetFractionOfLine(ShapeSystem.gridSystem.realizePoint(leaderLine1[2]), ShapeSystem.gridSystem.realizePoint(leaderLine2[2]), .5F);
            drawingPoint.X = drawingPoint.X - (dimLength_Size.Width / 2);
            drawingPoint.Y = drawingPoint.Y - (dimLength_Size.Height / 2);
            g.FillRectangle(new SolidBrush(Color.White), new RectangleF(drawingPoint.X - 2, drawingPoint.Y - 2, dimLength_Size.Width + 4, dimLength_Size.Height + 4));
            g.DrawString(dimLength, this.TxFont, new SolidBrush(Color.Black), drawingPoint);
        }
    }
}
