using System;
using System.Collections.Generic;

namespace CAD
{
    [Serializable]
    public class Snapshot
    {
        public List<Line> list_Line = new List<Line>();
        public List<LinearDimension> list_Dim = new List<LinearDimension>();
        public List<cadPoint> list_cadPoint = new List<cadPoint>();
        public List<Rect> list_Rect = new List<Rect>();
        public List<Ellipse> list_Ellipse = new List<Ellipse>();
        public Snapshot()
        {
            foreach (Shape S in ShapeSystem.ShapeList)
            {
                if (S is Line)
                {
                    list_Line.Add((Line)S);
                }
                else if (S is LinearDimension)
                {
                    list_Dim.Add((LinearDimension)S);
                }
                else if (S is cadPoint)
                {
                    list_cadPoint.Add((cadPoint)S);
                }
                else if (S is Rect)
                {
                    list_Rect.Add((Rect)S);
                }
                else if (S is Ellipse)
                {
                    list_Ellipse.Add((Ellipse)S);
                }
            }
        }

        public void Load()
        {
            foreach (Line L in list_Line)
            {
                ShapeSystem.ShapeList.Add(L);
                ShapeSystem.AddShapeToDataSet(L);
            }
            foreach (LinearDimension L in list_Dim)
            {
                ShapeSystem.ShapeList.Add(L);
                ShapeSystem.AddShapeToDataSet(L);
            }
            foreach (cadPoint L in list_cadPoint)
            {
                ShapeSystem.ShapeList.Add(L);
                ShapeSystem.AddShapeToDataSet(L);
            }
            foreach (Rect L in list_Rect)
            {
                ShapeSystem.ShapeList.Add(L);
                ShapeSystem.AddShapeToDataSet(L);
            }
            foreach (Ellipse L in list_Ellipse)
            {
                ShapeSystem.ShapeList.Add(L);
                ShapeSystem.AddShapeToDataSet(L);
            }
        }
    }
    
}
