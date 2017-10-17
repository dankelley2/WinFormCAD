using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ConsoleRedirection;
using System.IO;
using CAD;

namespace BasicCad
{
    public partial class BasicCad_Form : Form
    {
        public TextBoxStreamWriter _writer;

        //Create CadSystem
        CadSystem cadSystem;

        public BasicCad_Form()
        {
            InitializeComponent();
        }


        /**********************
         * INPUT
         **********************/

        private void Textbox_Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cadSystem.commandHistory.AddToHistory(txt_Input.Text);
                cadSystem.ParseInput(txt_Input.Text);
                txt_Input.Clear();

                e.Handled = true;
                e.SuppressKeyPress = true;
                Invalidate();
                Update();
            }
            if (e.KeyCode == Keys.PageUp)
            {
                string txt = cadSystem.commandHistory.GetPreviousCommand();
                if (txt != null)
                {
                    txt_Input.Text = txt;
                    txt_Input.SelectionStart = txt_Input.Text.Length;
                    txt_Input.SelectionLength = 0;
                }
            }
            if (e.KeyCode == Keys.PageDown)
            {
                string txt = cadSystem.commandHistory.GetNextCommand();
                if (txt != null)
                {
                    txt_Input.Text = txt;
                    txt_Input.SelectionStart = txt_Input.Text.Length;
                    txt_Input.SelectionLength = 0;
                }
            }
        }

        /**********************
         * FORM EVENTS
         **********************/

        private void BasicCad_Load(object sender, EventArgs e)
        {
            cadSystem = new CadSystem(new PointF(145, 0), this.Size ,.25F,96);
            cadSystem.shapeSystem.setBaseColors(
                new Pen(Color.FromArgb(255,20,20,20), 2), 
                new Pen(Color.OrangeRed, 2));
            AdjustSnapDistance.Value = (decimal)cadSystem.gridSystem.getGridIncrements();
            _writer = new ConsoleRedirection.TextBoxStreamWriter(txt_Console);
            // Redirect the out Console stream
            Console.SetOut(_writer);

            //List Dataset
            List_Shapes.DataSource = ShapeSystem.DT_ShapeList;
            List_Shapes.DisplayMember = "DisplayString";
            List_Shapes.ValueMember = "IdShape";
            //Shapes_TreeView.Nodes.Clear();
            //Shapes_TreeView_Init();
            //foreach (ShapeSystem.Shape S in cadSystem.shapeSystem.GetShapes())
            //{
            //    TreeNode newNode = new TreeNode();
            //    newNode.Name = S.IdShape.ToString();
            //    newNode.Text = S.MetaName + " " + S.MetaDesc;
            //    if (S.ParentId != -1)
            //    {
            //        Shapes_TreeView.Nodes[0].Nodes[Shapes_TreeView.Nodes.IndexOf(Shapes_TreeView.Nodes.Find(S.ParentId.ToString(), true)[0])].Nodes.Add(newNode);
            //    }
            //    else
            //    {
            //        Shapes_TreeView.Nodes[0].Nodes.Add(newNode);
            //    }
            //}
            //Shapes_TreeView.Refresh();

            

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        

        private void Shapes_TreeView_Init()
        {
            TreeNode ShapesNode = new TreeNode();
            ShapesNode.Name = "MAIN_Shapes";
            ShapesNode.Text = "Shapes";
            this.Shapes_TreeView.Nodes.Add(ShapesNode);
        }
        /**********************
         * PAINT
         **********************/

        private void BasicCad_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Default;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            cadSystem.gridSystem.Draw(e.Graphics);
            cadSystem.shapeSystem.RefreshAll(e.Graphics);

            if (cadSystem.clickCache.count() > 0)
            {
                PointF cursPos = this.PointToClient(Cursor.Position);
                cadSystem.gridSystem.DrawLineToCursor(e.Graphics, cadSystem.gridSystem.GetCursorReal(),cursPos);
            }
            cadSystem.gridSystem.DrawCurs(e.Graphics);
            
        }
        
        /**********************
         * Control EVENTS
         **********************/

        private void List_Shapes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (List_Shapes.SelectedIndex == -1)
                return;

            /**********************************
             * TODO: implement better dataset of shapes for this listbox to use
             **********************************/
            ShapeSystem.MakeActiveShape(cadSystem.shapeSystem.GetShapeById((int)List_Shapes.SelectedValue));
            Invalidate();
        }

        private void Button_RemoveShape_Click(object sender, EventArgs e)
        {
            //TODO: SEE ABOVE
            if (cadSystem.shapeSystem.RemoveActiveShape())
            {
                Invalidate();
            }
        }

        private void BasicCad_Click(object sender, EventArgs e)
        {
            PointF cursPos = this.PointToClient(Cursor.Position);

            if (cadSystem.gridSystem.IsWithinCurrentBounds(cursPos))
            {
                MouseEventArgs me = (MouseEventArgs)e;
                if (me.Button == MouseButtons.Left)
                {
                    cadSystem.gridSystem.SnapCursorToPoint(cursPos);
                }
                else if (me.Button == MouseButtons.Right)
                {
                    cadSystem.gridSystem.SnapCursorToPoint(cursPos);
                    PointF click = cadSystem.gridSystem.GetNearestSnapPoint(cursPos);
                    cadSystem.clickCache.enqueue(click);

                    if (!(cadSystem.InProcess) && cadSystem.clickCache.count() == 2)
                    {
                        cadSystem.InProcess = true;
                        bool prevPositioning = cadSystem.gridSystem.relativePositioning;
                        cadSystem.gridSystem.relativePositioning = false;
                        PointF P1 = cadSystem.clickCache.dequeue();
                        PointF P2;
                        if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                        {
                            P2 = cadSystem.clickCache.peek();
                        }
                        else
                        {
                            P2 = cadSystem.clickCache.dequeue();
                        }
                        string lineInput = "L " + P1.X.ToString() + " " + P1.Y.ToString() + " " + P2.X.ToString() + " " + P2.Y.ToString();
                        cadSystem.ParseInput(lineInput);
                        cadSystem.gridSystem.relativePositioning = prevPositioning;
                        cadSystem.InProcess = false;
                    }
                }
                else if (me.Button == MouseButtons.Middle)
                {
                    cadSystem.gridSystem.SnapOriginToPoint(cursPos);
                }
                Invalidate();
            }
        }

        private void BasicCad_ResizeEnd(object sender, EventArgs e)
        {
            cadSystem.gridSystem.resizeGrid(this.Size);
            Invalidate();
        }

        private void BasicCad_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cadSystem.gridSystem.ZoomOut();
            ZoomScale.Text = (cadSystem.gridSystem.getZoomScale() * 100).ToString() + "%";
            Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cadSystem.gridSystem.ZoomIn();
            ZoomScale.Text = (cadSystem.gridSystem.getZoomScale() * 100).ToString() + "%";
            Invalidate();
        }

        private void AdjustSnapDistance_ValueChanged(object sender, EventArgs e)
        {
            if (cadSystem.gridSystem.setGridIncrements((float)AdjustSnapDistance.Value))
            {
                Invalidate();
                Update();
            }
        }

        private void BasicCad_Form_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void BasicCad_Form_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void BasicCad_Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (cadSystem.clickCache.count() > 0)
            {
                Refresh();
            }
        }
    }
}

namespace ConsoleRedirection
{
    public class TextBoxStreamWriter : TextWriter
    {
        TextBox _output = null;

        public TextBoxStreamWriter(TextBox output)
        {
            _output = output;
        }

        public override void Write(char value)
        {
            base.Write(value);
            _output.AppendText(value.ToString()); // When character data is written, append it to the text box.
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}

namespace UTIL
{
}