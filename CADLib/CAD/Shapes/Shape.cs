using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace CAD
{
    [Serializable]
    public abstract class Shape
    {
        public abstract void Draw(Graphics g);
        public abstract void Dimension();
        public List<PointF> Points = new List<PointF>();
        public bool isActiveShape { get; set; }
        public int ParentId;
        public string MetaName;
        public string MetaDesc;
        public bool needsRedraw;
        public int IdShape;
        public Shape()
        {
            ShapeSystem.ShapeList.Add(this);
            IdShape = ShapeSystem.ShapeList.Max(m => m.IdShape) + 1;
            ShapeSystem.MakeActiveShape(this);
        }
    }
}
