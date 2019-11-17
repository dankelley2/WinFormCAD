using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CAD
{
    public class GridSystem
    {
        public float gridIncrements { get; set; }
        public float gridScale { get; set; }
        public PointF containerOrigin { get; set; }
        public PointF gridOrigin { get; set; }
        public PointF cursorPosition { get; set; }
        public bool showGrid   = true;
        public bool showOrigin = true;
        public bool showSnaps = true;
        public bool showActiveSnaps = false;
        public bool showDims = true;
        public bool relativePositioning = false;
        public RectangleF gridBounds;
        public SizeF containerSize;
        private Pen gridPen = new Pen(Color.FromArgb(255,240,240,240));
        private Pen cursPen = new Pen(Color.FromArgb(100, Color.Black), 3);
        private Pen origPen = new Pen(Color.FromArgb(100, Color.Green), 3);
        public int DPI;
        //all sizes currently in inches
        
        public GridSystem (PointF containerOrigin, SizeF containerSize, float gridIncrements, int dpi)
        {
            this.gridIncrements = gridIncrements;
            this.containerOrigin = containerOrigin;
            this.containerSize = containerSize;
            this.cursorPosition = new PointF(0, 0);
            this.gridBounds = new RectangleF(containerOrigin, containerSize);
            this.DPI = dpi;
            this.gridScale = 1;
            this.gridOrigin = containerOrigin;
        }

        public static double GetDistanceP2P(PointF P1, PointF P2)
        {
            return Math.Abs(Math.Sqrt(Math.Pow((P2.X - P1.X), 2) +
                                        Math.Pow((P2.Y - P1.Y), 2)));
        }
        public bool toggleGrid()
        {
            showGrid = !(showGrid);
            return showGrid;
        }
        public bool toggleSnaps()
        {
            showSnaps = !(showSnaps);
            return showSnaps;
        }
        public bool toggleActiveSnaps()
        {
            showActiveSnaps = !(showActiveSnaps);
            return showActiveSnaps;
        }
        public bool toggleOrigin()
        {
            showOrigin = !(showOrigin);
            return showOrigin;
        }
        public bool toggleDims()
        {
            showDims = !(showDims);
            return showDims;
        }

        public float getGridIncrements()
        {
            return gridIncrements;
        }

        public bool setGridIncrements(float newSize)
        {
            if (newSize > 0)
            {
                gridIncrements = newSize;
                return true;
            }
            else
            {
                return false;
            }
        }

        public float getZoomScale()
        {
            return gridScale;
        }

        public PointF realizePoint(PointF P)
        {
            P.X = (P.X * (DPI * gridScale)) + gridOrigin.X;
            P.Y = (P.Y * (DPI * gridScale)) + gridOrigin.Y;
            return P;
        }

        public SizeF realizeSize(SizeF S)
        {
            S.Width = (S.Width * (DPI * gridScale));
            S.Height = (S.Height * (DPI * gridScale));
            return S;
        }

        public PointF theorizePoint(PointF P)
        {
            P.X = (P.X - gridOrigin.X) / (DPI * gridScale);
            P.Y = (P.Y - gridOrigin.Y) / (DPI * gridScale);
            return P;
        }

        public PointF GetCursorReal()
        {
            return realizePoint(cursorPosition);
        }

        public bool IsWithinCurrentBounds(PointF p1)
        {
            return gridBounds.Contains(p1);
        }

        public PointF GetNearestSnapPoint(PointF p1)
        {
            float x = 0;
            float y = 0;
            float gridSpacing = (gridIncrements * (DPI * gridScale));
            float snapDistance = (gridSpacing / 2) + 1;
            PointF theoPoint = theorizePoint(p1);
            if (!(showSnaps) && !(showGrid)) { return theoPoint; }

            if (showSnaps)
            {
                foreach (PointF P in ShapeSystem.SnapPoints)
                {
                    if (Math.Abs(theoPoint.X - P.X) < .0625F && Math.Abs(theoPoint.Y - P.Y) < .0625F)
                    {
                        x = P.X;
                        y = P.Y;
                        return P;
                    }
                }
            }
            if (showGrid)
            {
                //Forwards X
                for (float i = (int)gridOrigin.X; i < (int)gridBounds.Right; i += gridSpacing)
                {
                    if (Math.Abs(p1.X - i) < snapDistance)
                    {
                        x = i;
                        break;
                    }
                }
                //Backwards X
                for (float i = (int)gridOrigin.X; i > (int)gridBounds.Left; i -= gridSpacing)
                {
                    if (Math.Abs(p1.X - i) < snapDistance)
                    {
                        x = i;
                        break;
                    }
                }
                //Forwards Y
                for (float i = (int)gridOrigin.Y; i < (int)gridBounds.Bottom; i += gridSpacing)
                {
                    if (Math.Abs(p1.Y - i) < snapDistance)
                    {
                        y = i;
                        break;
                    }
                }
                //Backwards Y
                for (float i = (int)gridOrigin.Y; i > (int)gridBounds.Top; i -= gridSpacing)
                {
                    if (Math.Abs(p1.Y - i) < snapDistance)
                    {
                        y = i;
                        break;
                    }
                }
            }
            return theorizePoint(new PointF(x, y));
        }

        public PointF GetNearestSnapPoint_Real(PointF p1)
        {
            float x = 0;
            float y = 0;
            float gridSpacing = ((gridIncrements * DPI) / 4);
            float snapDistance = (gridSpacing / 2) + 1;

            //Forwards X
            for (float i = (int)gridOrigin.X; i < (int)gridBounds.Right; i += gridSpacing)
            {
                if (Math.Abs(p1.X - i) < snapDistance)
                {
                    x = i;
                    break;
                }
            }
            //Backwards X
            for (float i = (int)gridOrigin.X; i > (int)gridBounds.Left; i -= gridSpacing)
            {
                if (Math.Abs(p1.X - i) < snapDistance)
                {
                    x = i;
                    break;
                }
            }
            //Forwards Y
            for (float i = (int)gridOrigin.Y; i < (int)gridBounds.Bottom; i += gridSpacing)
            {
                if (Math.Abs(p1.Y - i) < snapDistance)
                {
                    y = i;
                    break;
                }
            }
            //Backwards Y
            for (float i = (int)gridOrigin.Y; i > (int)gridBounds.Top; i -= gridSpacing)
            {
                if (Math.Abs(p1.Y - i) < snapDistance)
                {
                    y = i;
                    break;
                }
            }
            return new PointF(x, y);
        }

        public void TogglePositioning()
        {
            relativePositioning = (!(relativePositioning));
            if (relativePositioning)
            {
                Console.WriteLine("Relative positioning is now active.");
            }
            else
            {
                Console.WriteLine("Global positioning is now active.");
            }
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
                    cursorPosition = new PointF(x1, y1);
                    string[] newpos = new string[2] { position[0].ToString(), position[1].ToString() };
                    Console.WriteLine("Cursor set to {0}:{1}", newpos);
                }
            }
            else
            {
                Console.WriteLine("Invalid cursor point.");
            }
        }

        public void resizeGrid(SizeF newContainerSize)
        {
            this.gridBounds = new RectangleF(containerOrigin, newContainerSize);
        }

        public void ZoomIn()
        {
            if (gridScale < 15)
                gridScale += .125F;
        }

        public void ZoomOut()
        {
            if (gridScale > .25F)
                gridScale -= .125F;
        }

        public void SnapCursorToPoint(PointF p1)
        {
            cursorPosition = GetNearestSnapPoint(p1);
        }

        public void SnapOriginToPoint(PointF p1)
        {
            gridOrigin = GetNearestSnapPoint_Real(p1);
        }

        public void MoveOriginByDelta_Real(PointF delta)
        {
            gridOrigin = new PointF(gridOrigin.X+delta.X,gridOrigin.Y+delta.Y);
        }

        public void DrawGrid(Graphics g)
        {
            if (!(showGrid)) { return; }
            g.Clip = new Region(gridBounds);

            float gridSpacing = (gridIncrements * (DPI * gridScale));
            //X+ Lines
            for (float i = (int)gridOrigin.X; i < (int)gridBounds.Right; i += gridSpacing)
            {
                g.DrawLine(gridPen, new PointF(i, gridBounds.Y), new PointF(i, gridBounds.Height));
            }
            //Y+ Lines
            for (float i = (int)gridOrigin.Y; i < (int)gridBounds.Bottom; i += gridSpacing)
            {
                g.DrawLine(gridPen, new PointF(gridBounds.X, i), new PointF(gridBounds.Width, i));
            }
            //X- Lines
            for (float i = (int)gridOrigin.X; i > (int)gridBounds.Left; i -= gridSpacing)
            {
                g.DrawLine(gridPen, new PointF(i, gridBounds.Y), new PointF(i, gridBounds.Height));
            }
            //Y- Lines
            for (float i = (int)gridOrigin.Y; i > (int)gridBounds.Top; i -= gridSpacing)
            {
                g.DrawLine(gridPen, new PointF(gridBounds.X, i), new PointF(gridBounds.Width, i));
            }
        }
        public void DrawGrid(Graphics g, PointF P)
        {
            if (!(showGrid)) { return; }
            g.Clip = new Region(gridBounds);

            float gridSpacing = (gridIncrements * (DPI * gridScale));
            //X+ Lines
            for (float i = (int)gridOrigin.X + P.X; i < (int)gridBounds.Right; i += gridSpacing)
            {
                g.DrawLine(gridPen, new PointF(i, gridBounds.Y), new PointF(i, gridBounds.Height));
            }
            //Y+ Lines
            for (float i = (int)gridOrigin.Y + P.Y; i < (int)gridBounds.Bottom; i += gridSpacing)
            {
                g.DrawLine(gridPen, new PointF(gridBounds.X, i), new PointF(gridBounds.Width, i));
            }
            //X- Lines
            for (float i = (int)gridOrigin.X + P.X; i > (int)gridBounds.Left; i -= gridSpacing)
            {
                g.DrawLine(gridPen, new PointF(i, gridBounds.Y), new PointF(i, gridBounds.Height));
            }
            //Y- Lines
            for (float i = (int)gridOrigin.Y + P.Y; i > (int)gridBounds.Top; i -= gridSpacing)
            {
                g.DrawLine(gridPen, new PointF(gridBounds.X, i), new PointF(gridBounds.Width, i));
            }
        }

        public void DrawOrigin(Graphics g)
        {
            if (!(showOrigin)) { return; }
            g.Clip = new Region(gridBounds);

            //Draw Origin
            PointF Gy1 = new PointF(gridOrigin.X, gridOrigin.Y - 50);
            PointF Gy2 = new PointF(gridOrigin.X, gridOrigin.Y + 50);
            PointF Gx1 = new PointF(gridOrigin.X - 50, gridOrigin.Y);
            PointF Gx2 = new PointF(gridOrigin.X + 50, gridOrigin.Y);

            g.DrawLine(origPen, Gy1, Gy2);
            g.DrawLine(origPen, Gx1, Gx2);
        }
        public void DrawOrigin(Graphics g, PointF P)
        {
            if (!(showOrigin)) { return; }
            g.Clip = new Region(gridBounds);

            //Draw Origin
            PointF Gy1 = new PointF(gridOrigin.X + P.X, gridOrigin.Y - 50 + P.Y);
            PointF Gy2 = new PointF(gridOrigin.X + P.X, gridOrigin.Y + 50 + P.Y);
            PointF Gx1 = new PointF(gridOrigin.X - 50 + P.X, gridOrigin.Y + P.Y);
            PointF Gx2 = new PointF(gridOrigin.X + 50 + P.X, gridOrigin.Y + P.Y);

            g.DrawLine(origPen, Gy1, Gy2);
            g.DrawLine(origPen, Gx1, Gx2);
        }

        public void DrawCurs(Graphics g)
        {
            g.Clip = new Region(gridBounds);
            //Cursor
            PointF C = realizePoint(new PointF(cursorPosition.X, cursorPosition.Y));

            g.DrawLine(cursPen, new PointF(C.X - 25, C.Y), new PointF(C.X + 25, C.Y));
            g.DrawLine(cursPen, new PointF(C.X, C.Y - 25), new PointF(C.X, C.Y + 25));
        }
        public void DrawCurs(Graphics g, PointF P)
        {
            g.Clip = new Region(gridBounds);
            //Cursor
            PointF C = realizePoint(new PointF(cursorPosition.X, cursorPosition.Y));

            g.DrawLine(cursPen, new PointF(C.X + P.X - 25, C.Y + P.Y), new PointF(C.X + P.X + 25, C.Y + P.Y));
            g.DrawLine(cursPen, new PointF(C.X + P.X, C.Y + P.Y - 25), new PointF(C.X + P.X, C.Y + P.Y + 25));
        }

        public void DrawSnaps(Graphics g)
        {
            if (!(showSnaps)) { return; }
            g.Clip = new Region(gridBounds);
            //Cursor
            if (showActiveSnaps)
            {
                foreach (Shape S in ShapeSystem.ShapeList.Where(s => s.isActiveShape == true && s is iSnappable))
                {
                    foreach (PointF sp in ((iSnappable)S).GetSnapPoints())
                    {
                        PointF P = realizePoint(sp);
                        g.DrawLine(new Pen(Color.Orange), new PointF(P.X - 10, P.Y), new PointF(P.X + 10, P.Y));
                        g.DrawLine(new Pen(Color.Orange), new PointF(P.X, P.Y - 10), new PointF(P.X, P.Y + 10));
                    }
                }
            }
            else
            {
                foreach (PointF S in ShapeSystem.GetSnapPoints())
                {
                    PointF P = realizePoint(S);
                    g.DrawLine(new Pen(Color.Orange), new PointF(P.X - 10, P.Y), new PointF(P.X + 10, P.Y));
                    g.DrawLine(new Pen(Color.Orange), new PointF(P.X, P.Y - 10), new PointF(P.X, P.Y + 10));
                }
            }
        }

        public void DrawLineToCursor(Graphics g,PointF start, PointF end)
        {
            g.DrawLine(cursPen, start, end);
        }


    }
}
