using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ConsoleRedirection;
using System.IO;
using CAD;
using System.Linq;
using System.Data;

namespace BasicCad
{
    public partial class BasicCad_Form : Form
    {
        public TextBoxStreamWriter _writer;
        public int clicksNeeded { get; set; }

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
                treeView_Update();

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
            cadSystem = new CadSystem(new PointF(145, 24), this.Size, .25F, 96);
            cadSystem.shapeSystem.setBaseColors(
                new Pen(Color.FromArgb(255, 20, 20, 20), 2),
                new Pen(Color.OrangeRed, 2));
            AdjustSnapDistance.Value = (decimal)cadSystem.gridSystem.getGridIncrements();
            _writer = new ConsoleRedirection.TextBoxStreamWriter(txt_Console);
            // Redirect the out Console stream
            Console.SetOut(_writer);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            treeView_Init();
            togg2PLine_Click(sender, e);
        }


        private void treeView_Init()
        {
            TreeNode ShapesNode = new TreeNode();
            ShapesNode.Name = "MAIN_Shapes";
            ShapesNode.Text = "Shapes";
            this.treeView.Nodes.Add(ShapesNode);
        }

        private void treeView_Update()
        {
            //Validate Existing Branches
            TreeNode ShapeNode = treeView.Nodes[0];
            //Remove old
            RemoveAbandonedNodes(ShapeNode);
            DataView orderedView = ShapeSystem.DT_ShapeList.DefaultView;
            orderedView.Sort = "IdShape ASC";
            foreach (DataRowView row in orderedView)
            {
                var parentId = row[1].ToString();
                var Id = row[0].ToString();
                var name = row[2].ToString();
                var desc = row[3].ToString();

                //check for new nodes and add
                if (!(checkNodeExistence(ShapeNode, Id)))
                {
                    TreeNode newNode = new TreeNode();
                    newNode.Name = Id;
                    newNode.Text = name + Id;
                    TreeNode parentNode = FindParentNodesRecursive(ShapeNode, parentId);
                    if (parentNode != null)
                    {
                        parentNode.Nodes.Add(newNode);
                    }
                    else
                    {
                        if (parentId != "-1")
                        {
                            ShapeSystem.FixParentAbandonmentIssues(Convert.ToInt16(Id));
                        }
                        ShapeNode.Nodes.Add(newNode);
                    }

                }
            }

            treeView.ExpandAll();
        }

        //NODE RECURSION
        public TreeNode FindParentNodesRecursive(TreeNode oParentNode, string NameSearchFor)
        {
            TreeNode returnNode = null;
            // Start recursion on all subnodes.
            foreach (TreeNode oSubNode in oParentNode.Nodes)
            {
                if (oSubNode.Name == NameSearchFor)
                    return oSubNode;
                else if (oSubNode.Nodes.Count > 0)
                {
                    returnNode = FindParentNodesRecursive(oSubNode, NameSearchFor);
                }
            }
            return returnNode;
        }
        public void RemoveAbandonedNodes(TreeNode oParentNode)
        {
            // Start recursion on all subnodes.
            foreach (TreeNode oSubNode in oParentNode.Nodes)
            {
                if (!(ShapeSystem.DT_ShapeList.Rows.Contains(oSubNode.Name)))
                {
                    oSubNode.Nodes.Clear();
                    oSubNode.Remove();
                    break;
                }
                else if (oSubNode.Nodes.Count > 0)
                {
                    RemoveAbandonedNodes(oSubNode);
                }
            }
        }
        public bool checkNodeExistence(TreeNode oParentNode, string NameSearchFor)
        {
            bool Exists = false;
            // Start recursion on all subnodes.
            foreach (TreeNode oSubNode in oParentNode.Nodes)
            {
                if (oSubNode.Name == NameSearchFor)
                {
                    Exists = true;
                    break;
                }
                else if (oSubNode.Nodes.Count > 0)
                {
                    Exists = checkNodeExistence(oSubNode, NameSearchFor);
                    if (Exists) break;
                }
            }
            return Exists;
        }
        public void RemoveNodesRecursive(TreeNode oParentNode, string NameSearchFor)
        {
            // Start recursion on all subnodes.
            foreach (TreeNode oSubNode in oParentNode.Nodes)
            {
                if (oSubNode.Name == NameSearchFor)
                {
                    oSubNode.Nodes.Clear();
                    oSubNode.Remove();
                    break;
                }
                else if (oSubNode.Nodes.Count > 0)
                {
                    RemoveNodesRecursive(oSubNode, NameSearchFor);
                }
            }
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
            cadSystem.gridSystem.DrawGrid(e.Graphics);
            cadSystem.gridSystem.DrawOrigin(e.Graphics);
            cadSystem.shapeSystem.RefreshAll(e.Graphics);
            cadSystem.gridSystem.DrawSnaps(e.Graphics);

            if (cadSystem.clickCache.count() > 0)
            {
                PointF cursPos = this.PointToClient(Cursor.Position);
                cadSystem.gridSystem.DrawLineToCursor(e.Graphics, cadSystem.gridSystem.GetCursorReal(), cursPos);
            }
            cadSystem.gridSystem.DrawCurs(e.Graphics);

        }

        /**********************
         * Control EVENTS
         **********************/

        private void BasicCad_Click(object sender, EventArgs e)
        {
            PointF cursPos = this.PointToClient(Cursor.Position);

            if (cadSystem.gridSystem.IsWithinCurrentBounds(cursPos))
            {
                MouseEventArgs me = (MouseEventArgs)e;
                if (me.Button == MouseButtons.Left)
                {

                    if (!(cadSystem.InProcess))
                    {
                        cadSystem.gridSystem.SnapCursorToPoint(cursPos);
                    }
                }
                else if (me.Button == MouseButtons.Right)
                {
                    cadSystem.gridSystem.SnapCursorToPoint(cursPos);
                    PointF click = cadSystem.gridSystem.GetNearestSnapPoint(cursPos);
                    //if nothing is in process
                    if (!(cadSystem.InProcess))
                    {
                        //CHECK ACTIVE TOOL
                        if (togg2PLine.Checked)
                        {
                            cadSystem.clickCache.clear();
                            cadSystem.clickCache.enqueue(click);
                            cadSystem.InProcess = true;
                            clicksNeeded = 2;
                        }
                    }
                    //else just add click
                    else
                    {
                        cadSystem.clickCache.enqueue(click);
                    }
                    //Check if clicks are done
                    if (cadSystem.clickCache.count() == clicksNeeded)
                    {
                        //CHECK ACTIVE TOOL
                        if (togg2PLine.Checked)
                        {
                            Make2PLine(sender, e);
                        }
                    }
                }
                else if (me.Button == MouseButtons.Middle)
                {
                    cadSystem.gridSystem.SnapOriginToPoint(cursPos);
                }
                Invalidate();
            }
        }

        private void Make2PLine(object sender, EventArgs e)
        {
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
                cadSystem.InProcess = false;
            }
            string lineInput = "L " + P1.X.ToString() + " " + P1.Y.ToString() + " " + P2.X.ToString() + " " + P2.Y.ToString();
            cadSystem.ParseInput(lineInput);
            cadSystem.gridSystem.relativePositioning = prevPositioning;
            treeView_Update();
        }
        private void Make2PRect(object sender, EventArgs e)
        {
            bool prevPositioning = cadSystem.gridSystem.relativePositioning;
            cadSystem.gridSystem.relativePositioning = false;
            PointF P1 = cadSystem.clickCache.dequeue();
            PointF P2 = cadSystem.clickCache.dequeue();
            cadSystem.InProcess = false;
            string RectInput = "RT " + P1.X.ToString() + " " + P1.Y.ToString() + " " + P2.X.ToString() + " " + P2.Y.ToString();
            cadSystem.ParseInput(RectInput);
            cadSystem.gridSystem.relativePositioning = prevPositioning;
            treeView_Update();
        }


        /**********************************
         * OTHER EVENTS
         *********************************/


        private void BasicCad_ResizeEnd(object sender, EventArgs e)
        {
            cadSystem.gridSystem.resizeGrid(this.Size);
            Invalidate();
        }

        private void BasicCad_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                CancelCommand();
            }
            if (e.KeyCode == Keys.Delete)
            {
                deleteToolStripMenuItem_Click(sender, e);
            }
        }
        private void CancelCommand()
        {
            if (cadSystem.InProcess)
            {
                int numItems = cadSystem.clickCache.clear();
                cadSystem.InProcess = false;
                Console.WriteLine("Cleared ClickQueue of {0} items", numItems);
                Invalidate();
            }
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
            PointF cursPos = this.PointToClient(Cursor.Position);
            if (e.Button == MouseButtons.Left && toggMoveDim.Checked)
            {
                if (!cadSystem.shapeSystem.ActivateShapeUnderPoint<ShapeSystem.LineDimension>(cursPos))
                {
                    cadSystem.shapeSystem.DeselectActiveShapes();
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (!cadSystem.shapeSystem.ActivateShapeUnderPoint(cursPos))
                {
                    cadSystem.shapeSystem.DeselectActiveShapes();
                }
            }
        }

        private void BasicCad_Form_MouseUp(object sender, MouseEventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
        }

        private void BasicCad_Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (cadSystem.clickCache.count() > 0)
            {
                PointF start = cadSystem.gridSystem.GetCursorReal();
                PointF end = this.PointToClient(Cursor.Position);
                RectangleF invalidrect = new RectangleF(Math.Min(start.X, end.X) - 30,
                               Math.Min(start.Y, end.Y) - 30,
                               Math.Abs(start.X - end.X) + 60,
                               Math.Abs(start.Y - end.Y) + 60);
                Invalidate(new Region(invalidrect));
            }
            if (toggMoveDim.Checked == true && cadSystem.shapeSystem.GetActiveShape() is ShapeSystem.LineDimension)
            {
                if (e.Button == MouseButtons.Left)
                {
                    cadSystem.shapeSystem.AdjustDimByRealCursor(this.PointToClient(Cursor.Position), cadSystem.shapeSystem.GetActiveShape().IdShape);
                    Invalidate();
                }
            }
        }

        private void saveTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveDesignDialog.FileName = "";
            saveDesignDialog.ShowDialog();

            if (saveDesignDialog.FileName != "")
            {
                cadSystem.SerializeAndSaveShapes(saveDesignDialog.FileName);
            }
            else
            {
                Console.WriteLine("Save dialog cancelled.");
            }
        }

        private void loadTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadDesignDialog.FileName = "";
            loadDesignDialog.ShowDialog();

            if (loadDesignDialog.FileName != "")
            {
                cadSystem.DeSerializeAndLoadShapes(loadDesignDialog.FileName);
                treeView_Update();
                Invalidate();
            }
            else
            {
                Console.WriteLine("Load dialog cancelled.");
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cadSystem.shapeSystem.RemoveActiveShape())
            {
                treeView_Update();
                Invalidate();
            }
        }

        private void BasicCad_Form_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void originToolStripMenuItem_Click(object sender, EventArgs e)
        {
            originToolStripMenuItem.Checked = cadSystem.gridSystem.toggleOrigin();
            Invalidate();
        }

        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gridToolStripMenuItem.Checked = cadSystem.gridSystem.toggleGrid();
            Invalidate();
        }

        private void lineSnapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lineSnapsToolStripMenuItem.Checked = cadSystem.gridSystem.toggleSnaps();
            Invalidate();
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Name == "MAIN_Shapes") { return; }
            ShapeSystem.MakeActiveShape(cadSystem.shapeSystem.GetShapeById(Convert.ToInt16(e.Node.Name)));
            Invalidate();
        }

        private void onlyActiveLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            onlyActiveLineToolStripMenuItem.Checked = cadSystem.gridSystem.toggleActiveSnaps();
            Invalidate();
        }

        private void uncheckAllInToolStrip()
        {
            CancelCommand();
            foreach (CheckBox C in this.ToolsFlowLayout.Controls)
            {
                    C.Checked = false;
            }
        }

        private void togg2PLine_Click(object sender, EventArgs e)
        {
            uncheckAllInToolStrip();
            togg2PLine.Checked = !(togg2PLine.Checked);
        }

        private void toggMoveDim_Click(object sender, EventArgs e)
        {
            uncheckAllInToolStrip();
            toggMoveDim.Checked = true;
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