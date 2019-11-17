using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace CAD
{
    public partial class ShapeSystem
    {
        public static List<Shape> ShapeList = new List<Shape>();
        public static DataTable DT_ShapeList = new DataTable("DT_ShapeList");
        public static List<PointF> SnapPoints = new List<PointF>();
        public AdjustableArrowCap penDisplayArrowCap = new AdjustableArrowCap(5, 5);
        public static Pen penBasic { get; set; }
        public static Pen penActive { get; set; }
        public static Pen penDimension { get; set; }
        public static Pen penDimArrow { get; set; }
        public static Pen penCTORLine { get; set; }
        public static GridSystem gridSystem { get; set; }

        public static List<PointF> GetSnapPoints()
        {
            return SnapPoints;
        }

        public static int FindLineCircleIntersections(PointF P1, float radius, PointF point1, PointF point2, out PointF intersection1, out PointF intersection2)
        {
            float cx = P1.X;
            float cy = P1.Y;
            float dx, dy, A, B, C, det, t;

            dx = point2.X - point1.X;
            dy = point2.Y - point1.Y;

            A = dx * dx + dy * dy;
            B = 2 * (dx * (point1.X - cx) + dy * (point1.Y - cy));
            C = (point1.X - cx) * (point1.X - cx) +
                (point1.Y - cy) * (point1.Y - cy) -
                radius * radius;

            det = B * B - 4 * A * C;
            if ((A <= 0.0000001) || (det < 0))
            {
                // No real solutions.
                intersection1 = new PointF(float.NaN, float.NaN);
                intersection2 = new PointF(float.NaN, float.NaN);
                return 0;
            }
            else if (det == 0)
            {
                // One solution.
                t = -B / (2 * A);
                intersection1 =
                    new PointF(point1.X + t * dx, point1.Y + t * dy);
                intersection2 = new PointF(float.NaN, float.NaN);
                return 1;
            }
            else
            {
                // Two solutions.
                t = (float)((-B + Math.Sqrt(det)) / (2 * A));
                intersection1 =
                    new PointF(point1.X + t * dx, point1.Y + t * dy);
                t = (float)((-B - Math.Sqrt(det)) / (2 * A));
                intersection2 =
                    new PointF(point1.X + t * dx, point1.Y + t * dy);
                return 2;
            }
        }

        public static double GetDistancePointToLine(PointF P, PointF A, PointF B)
        {
            //get Length of |AB|
            var d = Math.Sqrt(Math.Pow((B.X - A.X), 2) +
                                    Math.Pow((B.Y - A.Y), 2));
            //Get nearest point to P on |AB|
            var a = (1 / Math.Pow(d, 2)) *
                    (
                        ((B.X - A.X) * (P.X - A.X)) +
                        ((B.Y - A.Y) * (P.Y - A.Y))
                    );
            PointF nearest = new PointF();
            nearest.X = (float)(A.X + (B.X - A.X) * a);
            nearest.Y = (float)(A.Y + (B.Y - A.Y) * a);
            
            return (double)Math.Sqrt(Math.Pow((P.X - nearest.X), 2) +
                                    Math.Pow((P.Y - nearest.Y), 2));

        }

        public static void MakeActiveShape(Shape S)
        {
            foreach (Shape shape in ShapeList)
            {
                shape.isActiveShape = false;
            }
            S.isActiveShape = true;
        }

        public static void AddActiveShape(Shape S)
        {
            S.isActiveShape = true;
        }

        public static void SetShapeFillColor(List<float> Input)
        {
            Shape S = ShapeList.Where(s => s.IdShape == (int)Input[0]).FirstOrDefault();
            if (S is iFillable)
            {
                if (Input.Count == 4)
                {
                    ((iFillable)S).FillColor = Color.FromArgb(255, (int)Input[1], (int)Input[2], (int)Input[3]);
                }
                else if (Input.Count == 5)
                {
                    ((iFillable)S).FillColor = Color.FromArgb((int)Input[1], (int)Input[2], (int)Input[3], (int)Input[4]);
                }
            }
        }

        public static void AddShapeToDataSet(Shape S)
        {
            DataRow row = DT_ShapeList.NewRow();
            row["IdShape"] = S.IdShape;
            row["IdParentShape"] = S.ParentId;
            row["MetaName"] = S.MetaName;
            row["MetaDesc"] = S.MetaDesc;
            row["DisplayString"] = S.MetaName + " " + S.IdShape.ToString() + " : " + S.MetaDesc;
            DT_ShapeList.Rows.Add(row);
        }

        public static void Draw2PTDim(PointF P1, PointF P2)
        {
            LinearDimension newDim = new LinearDimension(P1, P2);
        }

        public static void FixParentAbandonmentIssues(int IdShape)
        {
            foreach (DataRow row in DT_ShapeList.Rows)
            {
                if (row["IdShape"].ToString() == IdShape.ToString())
                {
                    row["IdParentShape"] = -1;
                }
            }
        }

        public static void UpdateSnapPoints()
        {
            SnapPoints.Clear();
            foreach (Shape S in ShapeList)
            {
                if (S is iSnappable)
                {
                    iSnappable snapObj = (iSnappable)S;
                    SnapPoints = SnapPoints.Union((snapObj.GetSnapPoints()).OrderByDescending(p => p.X)).ToList();
                }
            }
        }

        public static void AdjustDimByRealCursor(PointF RealCursor, int IdDim)
        {
            PointF cursor = ShapeSystem.gridSystem.theorizePoint(RealCursor);
            LinearDimension S = (LinearDimension)GetShapeById(IdDim);
            double dist = GetDistancePointToLine(cursor, S.P1, S.P2);
            S._leadingLineLength = (float)dist;
            if (cursor.X < Line.GetFractionOfLine(S.P1, S.P2, .5F).X)
                S.direction = -1;
            else
                S.direction = 1;
        }

        public static Shape GetActiveShape()
        {
            Shape S = ShapeList.Where(s => s.isActiveShape == true).FirstOrDefault();
            return S;
        }

        public static List<Shape> GetShapes()
        {
            return ShapeList;
        }

        public static void SetGrid(GridSystem grid)
        {
            gridSystem = grid;
        }

        public static Shape GetShapeById(int Id)
        {
            Shape S = ShapeSystem.ShapeList.Where(s => s.IdShape == Id).FirstOrDefault();
            return S;
        }

        public static bool ActivateShapeUnderPoint(PointF P)
        {
            P = ShapeSystem.gridSystem.theorizePoint(P);
            double distanceToPoint = 999;
            Shape newActive = null;
            foreach (Shape S in GetShapes())
            {
                if (!(S is iClickable)) // if it's not clickable continue
                {
                    continue;
                }

                iClickable C = (iClickable)S;

                if (C.IntersectsWithCircle(P, .1F))
                {
                    double D = C.GetDistanceFromPoint(P); //ITS LIKE A BAD ITCH EDDY, GET IT OFF
                    if (D < distanceToPoint)
                    {
                        distanceToPoint = D;
                        newActive = (Shape)C;
                    }
                }
            }
            if (newActive != null)
            {
                MakeActiveShape(GetShapeById(newActive.IdShape));
                return true;
            }
            return false;
        }

        public static bool ActivateShapeUnderPoint<T>(PointF P) where T : iClickable
        {
            P = ShapeSystem.gridSystem.theorizePoint(P);
            double distanceToPoint = 999;
            Shape newActive = null;
            foreach (Shape S in GetShapes())
            {
                if (!(S is T)) // if it's not clickable continue
                {
                    continue;
                }

                iClickable C = (iClickable)S;

                if (C.IntersectsWithCircle(P, .1F))
                {
                    double D = C.GetDistanceFromPoint(P); //ITS LIKE A BAD ITCH EDDY, GET IT OFF
                    if (D < distanceToPoint)
                    {
                        distanceToPoint = D;
                        newActive = (Shape)C;
                    }
                }
            }
            if (newActive != null)
            {
                MakeActiveShape(GetShapeById(newActive.IdShape));
                return true;
            }
            return false;
        }

        public static bool RemoveShapeById(int Id)
        {
            Shape S = ShapeSystem.ShapeList.Where(s => s.IdShape == Id).FirstOrDefault();
            if (S != null)
            {
                ShapeSystem.ShapeList.Remove(S);
                DT_ShapeList.Rows.Remove(DT_ShapeList.Rows.Find(S.IdShape));
                ShapeSystem.UpdateSnapPoints();
                return true;
            }
            return false;
        }

        public static bool RemoveActiveShape()
        {
            Shape S = ShapeSystem.ShapeList.Where(s => s.isActiveShape == true).FirstOrDefault();
            if (S != null)
            {
                ShapeSystem.ShapeList.Remove(S);
                DT_ShapeList.Rows.Remove(DT_ShapeList.Rows.Find(S.IdShape));
                ShapeSystem.UpdateSnapPoints();
                return true;
            }
            return false;
        }

        public static void DeselectActiveShapes()
        {
            foreach (Shape S in ShapeSystem.ShapeList)
            {
                S.isActiveShape = false;
            }
        }

        public static void DimensionActiveLine()
        {
            Shape S = GetActiveShape();
            if (S == null)
            {
                Console.WriteLine("No object selected.");
                return;
            }
            if (S.MetaName == "Line")
            {
                S.Dimension();
                Console.WriteLine(S.MetaName + " " + S.IdShape.ToString() + " dimensioned.");
            }
            else
            {
                Console.WriteLine("Active object is not dimension-able.");
            }
        }

        public static void RefreshAll(Graphics g)
        {
            foreach (Shape S in ShapeSystem.ShapeList)
            {
                if (S is iFillable)
                {
                    ((iFillable)S).DrawFill(g);
                }
                S.Draw(g);
            }
        }

        public static void setBaseColors(Pen basic, Pen active)
        {
            ShapeSystem.penBasic = basic;
            ShapeSystem.penActive = active;
            ShapeSystem.penDimension = new Pen(Color.CornflowerBlue, 1);
            ShapeSystem.penDimArrow = new Pen(penDimension.Color, 1);
            ShapeSystem.penCTORLine = new Pen(Color.FromArgb(150, Color.Orange),1);
            ShapeSystem.penCTORLine.DashStyle = DashStyle.Dash;
            AdjustableArrowCap dimPenCap = new AdjustableArrowCap(4, 5);
            penDimArrow.CustomStartCap = dimPenCap;
            penDimArrow.CustomEndCap = dimPenCap;
        }

        public static void ClearData()
        {
            List<int> ShapeIds = new List<int>();
            foreach (Shape S in ShapeSystem.ShapeList)
            {
                ShapeIds.Add(S.IdShape);
            }
            foreach (int I in ShapeIds)
            {
                RemoveShapeById(I);
            }
            ShapeSystem.UpdateSnapPoints();
        }

        public static void InitSystem()
        {
            DataColumn IdShape = new DataColumn("IdShape");
            IdShape.DataType = System.Type.GetType("System.Int32");
            IdShape.Unique = true;
            DT_ShapeList.Columns.Add(IdShape);
            DT_ShapeList.PrimaryKey = new DataColumn[] { DT_ShapeList.Columns["IdShape"] };

            DataColumn IdParentShape = new DataColumn("IdParentShape");
            IdShape.DataType = System.Type.GetType("System.Int32");
            DT_ShapeList.Columns.Add(IdParentShape);

            DataColumn MetaName = new DataColumn("MetaName");
            MetaName.DataType = System.Type.GetType("System.String");
            DT_ShapeList.Columns.Add(MetaName);

            DataColumn MetaDesc = new DataColumn("MetaDesc");
            MetaDesc.DataType = System.Type.GetType("System.String");
            DT_ShapeList.Columns.Add(MetaDesc);

            DataColumn DisplayString = new DataColumn("DisplayString");
            MetaDesc.DataType = System.Type.GetType("System.String");
            DT_ShapeList.Columns.Add(DisplayString);

        }

    }

}
