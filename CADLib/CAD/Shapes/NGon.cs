using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD
{
    [Serializable]
    public class NGon : Shape
    {
        public NGon()
        {

        }
        public NGon(List<PointF> points)
        {
            this.Points = points;
        }

        public override void Dimension()
        {
            throw new NotImplementedException();
        }

        public override void Draw(Graphics g)
        {
            g.DrawPolygon(Pens.Red, Points.ToArray());
        }
    }
}
