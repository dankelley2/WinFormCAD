using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicCad
{
    public class GridSystem
    {
        public float gridScale { get; set; }
        public PointF gridOrigin { get; set; }
        public PointF cursorPosition { get; set; }
        public bool relativePositioning = false;
        public RectangleF gridBounds;
        public Form container;
        private Pen gridPen = new Pen(Color.LightGray);
        private Pen cursPen = new Pen(Color.FromArgb(50,Color.Black),3);
        public int DPI;
        public GridSystem(Form cont,float scale, PointF origin, int dpi)
        {
            this.gridScale = scale;
            this.gridOrigin = origin;
            this.container = cont;
            this.cursorPosition = new PointF(0,0);
            this.gridBounds = new RectangleF(origin, new SizeF(cont.Width, cont.Height));
            this.DPI = dpi;
        }

        public void resizeGrid()
        {
            this.gridBounds = new RectangleF(gridOrigin, new SizeF(container.Width, container.Height));
        }

        public void ZoomIn()
        {
            gridScale += .25F;
        }

        public void ZoomOut()
        {
            gridScale -= .25F;
        }

        public PointF realizePoint(PointF P)
        {
            P.X = (P.X * (DPI * gridScale)) + gridOrigin.X;
            P.Y = (P.Y * (DPI * gridScale)) + gridOrigin.Y;
            return P;
        }
        public PointF theorizePoint(PointF P)
        {
            P.X = (P.X - gridOrigin.X) / (DPI * gridScale);
            P.Y = (P.Y - gridOrigin.Y) / (DPI * gridScale);
            return P;
        }
        public void SetCursor(List<float> position)
        {
            if (position.Count == 2)
            {
                if (relativePositioning)
                {
                    float x1 = position[0] + this.cursorPosition.X;
                    float y1 = position[1] + this.cursorPosition.Y;
                    cursorPosition = new PointF(x1, y1);
                    string[] newpos = new string[2] { position[0].ToString(), position[1].ToString() };
                    Console.WriteLine("Cursor moved with a delta of {0}:{1}", newpos);
                }
                else
                {
                    float x1 = position[0];
                    float y1 = position[1];
                    cursorPosition = new PointF(x1,y1);
                    string[] newpos = new string[2] { position[0].ToString(), position[1].ToString() };
                    Console.WriteLine("Cursor set to {0}:{1}",newpos);
                }
            }
            else
            {
                Console.WriteLine("Invalid cursor point.");
            }
        }
        public void SnapCursorToPoint(PointF p1)
        {
            cursorPosition = GetNearestSnapPoint(p1);
        }
        public bool IsWithinCurrentBounds(PointF p1)
        {
            return gridBounds.Contains(p1);
        }
        public PointF GetNearestSnapPoint(PointF p1)
        {
            float x = 0;
            float y = 0;
            int gridSpacing = (int)((gridScale * DPI) / 4);
            int snapDistance = (gridSpacing / 2) + 1;

            for (float i = (int)gridBounds.X; i < (int)gridBounds.Width; i += gridSpacing)
            {
                if (Math.Abs(p1.X - i) < snapDistance)
                {
                    x = i;
                    break;
                }
            }
            for (float i = (int)gridBounds.Y; i < (int)gridBounds.Height; i += gridSpacing)
            {
                if (Math.Abs(p1.Y - i) < snapDistance)
                {
                    y = i;
                    break;
                }
            }
            return theorizePoint(new PointF(x, y));
        }
        public void Draw(Graphics g)
        {
            int gridSpacing = (int)((gridScale * DPI) / 4);
            //X Lines
            for (float i = (int)gridBounds.X; i < (int)gridBounds.Width; i += gridSpacing)
            {
                g.DrawLine(gridPen, new PointF(i, gridBounds.Y), new PointF(i, gridBounds.Height));
            }
            //Y Lines
            for (float i = (int)gridBounds.Y; i < (int)gridBounds.Height; i += gridSpacing)
            {
                g.DrawLine(gridPen, new PointF(gridBounds.X, i), new PointF(gridBounds.Width, i));
            }
        }
        public void DrawCurs(Graphics g)
        {
            //Cursor
            PointF Gy1 = realizePoint(new PointF(cursorPosition.X, cursorPosition.Y - .5F));
            PointF Gy2 = realizePoint(new PointF(cursorPosition.X, cursorPosition.Y + .5F));
            PointF Gx1 = realizePoint(new PointF(cursorPosition.X - .5F, cursorPosition.Y));
            PointF Gx2 = realizePoint(new PointF(cursorPosition.X + .5F, cursorPosition.Y)); 

            g.DrawLine(cursPen, Gy1, Gy2);
            g.DrawLine(cursPen, Gx1, Gx2);
        }


    }
    [Serializable]
    public abstract class Shape
    {
        public static List<Shape> ShapeList = new List<Shape>();
        public static GridSystem gridSystem { get; set; }
        public static Pen basicPen { get; set; }
        public static Pen activePen { get; set; }
        public abstract void Draw(Graphics g);
        public static Shape GetShapeById(int Id)
        {
            Shape S = ShapeList.Where(s => s.IdShape == Id).FirstOrDefault();
            return S;
        }
        public static Shape GetActiveShape()
        {
            Shape S = ShapeList.Where(s => s.isActiveShape == true).FirstOrDefault();
            return S;
        }
        public static void MakeActiveShape(Shape S)
        {
            foreach (Shape shape in ShapeList)
            {
                shape.isActiveShape = false;
            }
            S.isActiveShape = true;
        }
        public bool isActiveShape { get; set; }
        public string MetaName;
        public string MetaDesc;
        public bool needsRedraw;
        public int IdShape;
        public Shape()
        {
            ShapeList.Add(this);
            IdShape = ShapeList.Count();
            MakeActiveShape(this);
        }
        public static void RefreshAll(Graphics g)
        {
            foreach (Shape S in ShapeList)
            {
                S.Draw(g);
            }
        }
    }

    [Serializable]
    public class Line : Shape
    {
        internal PointF P1;
        internal PointF P2;
        public Line(PointF p1, PointF p2)
        {
            this.P1 = p1;
            this.P2 = p2;
            this.MetaName = "Line";
            object[] MetaDescArray = { this.P1.X, this.P1.Y, this.P2.X, this.P2.Y };
            this.MetaDesc = string.Format("{0},{1}:{2},{3}", MetaDescArray);
        }
        
        public static void AddLine(List<float> Input)
        {
            if (gridSystem.relativePositioning && Input.Count == 2)
            {
                PointF p1 = new PointF(gridSystem.cursorPosition.X, gridSystem.cursorPosition.Y);

                float x2 = (Input[0] + p1.X);
                float y2 = (Input[1] + p1.Y);
                PointF p2 = new PointF(x2, y2);
                Line RetLine = new Line(p1, p2);
                gridSystem.cursorPosition = p2;
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
                gridSystem.cursorPosition = p2;
                return;
            }
        }

        public override void Draw(Graphics g)
        {
            if (isActiveShape)
                g.DrawLine(activePen, gridSystem.realizePoint(P1), gridSystem.realizePoint(P2));
            else
                g.DrawLine(basicPen, gridSystem.realizePoint(P1), gridSystem.realizePoint(P2));

        }
    }

    [Serializable]
    public class Path : Shape
    {
        List<PointF> points = new List<PointF>();
        GraphicsPath path = new GraphicsPath();
        public Path(List<float> start)
        {
            this.points.Add(new PointF(start[0],start[1]));
            this.MetaName = "Path";
            this.MetaDesc = string.Format("Id:{0}", IdShape);
            this.path.StartFigure();
        }
        public static void AddPath(List<float> start)
        {
            if (start.Count != 2)
            {
                Console.WriteLine("Invalid Coordinates");
                return;
            }
            Path p = new Path(start);
        }

        //public static Path AddToActivePath(List<float> pointList)
        //{
        //    if (pointList.Count != 2)
        //    {
        //        Console.WriteLine("Invalid Coordinates");
        //        return null;
        //    }
        //    else if (GetActiveShape().MetaName != "Path")
        //    {
        //        Console.WriteLine("ActiveObject is not a Path");
        //        return null;
        //    }
        //    Path ActivePath;
        //    PointF P = new PointF(pointList[0], pointList[1]);
        //    points.Add(P);
        //    PointF P1 = points[points.Count - 2];
        //    PointF P2 = points[points.Count - 2];
        //    path.AddLine(P1, P2);
        //    Console.WriteLine("Point Added.");
        //    return;
        //}
        //public static void Create(List<float> pointList)
        //{
        //    if (pointList.Count != 2)
        //    {
        //        Console.WriteLine("Invalid Coordinates");
        //        return;
        //    }
        //    points.Add(new PointF(pointList[0],pointList[1]));
        //    PointF P1 = points[points.Count - 2];
        //    PointF P2 = points[points.Count - 2];
        //    path.AddLine(P1, P2);
        //    Console.WriteLine("Point Added.");
        //}
        
        public override void Draw(Graphics g)
        {
            if (points.Count < 2)
            {
                return;
            }
            g.DrawPath(basicPen, path);
            
        }
    }


}
