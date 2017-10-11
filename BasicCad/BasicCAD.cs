using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ConsoleRedirection;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Data;

namespace BasicCad
{
    public partial class BasicCad : Form
    {
        public TextBoxStreamWriter _writer;
        public List<string> CommandHistory = new List<string>();
        public static PointF CurrentPosition = new PointF(0, 0);
        public static bool RelativePositioning = false;
        public static Shape SelectedShape;
        public static Dictionary<string, Action<List<float>>> drawingFunctions = new Dictionary<string, Action<List<float>>>();
        public static Dictionary<string, Action> gridFunctions = new Dictionary<string, Action>();
        public static Queue<PointF> ClickCache = new Queue<PointF>();
        public static bool InProcess = false;
        internal int HistoryPointer = -1;

        public BasicCad()
        {
            InitializeComponent();
        }

        public void TogglePositioning()
        {
            Shape.gridSystem.relativePositioning = (!(Shape.gridSystem.relativePositioning));
            if (Shape.gridSystem.relativePositioning)
            {
                Console.WriteLine("Relative positioning is now active.");
            }
            else
            {
                Console.WriteLine("Global positioning is now active.");
            }

        }

        public void ParseInput(string text)
        {
            //Split Input
            UTIL.parsedObj result = UTIL.parsing.parseString(text, ";, ");

            //if (inProcess != null)
            //{
            //    if (result.type == "END")
            //    {
            //        inProcess = null;
            //        Invalidate();
            //        Update();
            //        return;
            //    }
            //    inProcess.AddPoint(result.value);
            //    Invalidate();
            //    Update();
            //    return;
            //}
            

            //Switch first Letter
            if (drawingFunctions.ContainsKey(result.type))
                drawingFunctions[result.type].Invoke(result.value);
            else if (gridFunctions.ContainsKey(result.type))
                gridFunctions[result.type].Invoke();
            else
                Console.WriteLine("Command not recognized");
            Invalidate();
            Update();

        }

        /**********************
         * HISTORY
         **********************/

        public void AddToHistory(string text)
        {
            if (CommandHistory.Count == 0)
            {
                CommandHistory.Add(text);
                HistoryPointer = CommandHistory.Count;
                return;
            }
            if (text != CommandHistory[CommandHistory.Count - 1])
            {
                CommandHistory.Add(text);
                HistoryPointer = CommandHistory.Count;
            }
        }

        public string GetPreviousCommand()
        {
            string command = null;
            if (HistoryPointer > -1)
            {
                HistoryPointer--;
                if (HistoryPointer > -1)
                {
                    command = CommandHistory[HistoryPointer];
                }
                else
                {
                    HistoryPointer++;
                }
            }
            return command;
        }

        public string GetNextCommand()
        {
            string command = null;
            if (HistoryPointer+1 < CommandHistory.Count)
            {
                HistoryPointer++;
                if (HistoryPointer + 1 <= CommandHistory.Count)
                {
                    command = CommandHistory[HistoryPointer];
                } else
                {
                    HistoryPointer--;
                }
            }
            return command;
        }

        /**********************
         * INPUT
         **********************/

        private void Textbox_Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddToHistory(txt_Input.Text);
                ParseInput(txt_Input.Text);
                txt_Input.Clear();

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            if (e.KeyCode == Keys.PageUp)
            {
                string txt = GetPreviousCommand();
                if (txt != null)
                {
                    txt_Input.Text = txt;
                    txt_Input.SelectionStart = txt_Input.Text.Length;
                    txt_Input.SelectionLength = 0;
                }
            }
            if (e.KeyCode == Keys.PageDown)
            {
                string txt = GetNextCommand();
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
            Shape.gridSystem = new GridSystem(this, 1, new PointF(145, 0), 96);
            _writer = new ConsoleRedirection.TextBoxStreamWriter(txt_Console);
            // Redirect the out Console stream
            Console.SetOut(_writer);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            Shape.basicPen = new Pen(Color.FromArgb(255,20,20,20), 2);
            Shape.activePen = new Pen(Color.OrangeRed, 2);

            //SET FUNCTIONS
            drawingFunctions.Add("L", Line.AddLine);
            drawingFunctions.Add("P", Path.AddPath);
            drawingFunctions.Add("C", Shape.gridSystem.SetCursor);
            Action PosToggle =
                () => TogglePositioning();
            gridFunctions.Add("R", PosToggle);
        }

        /**********************
         * PAINT
         **********************/

        private void BasicCad_Paint(object sender, PaintEventArgs e)
        {
            Shape.gridSystem.Draw(e.Graphics);
            Shape.RefreshAll(e.Graphics);
            Shape.gridSystem.DrawCurs(e.Graphics);

            List_Shapes.Items.Clear();
            foreach (Shape S in Shape.ShapeList)
            {
                int newItem = List_Shapes.Items.Add(S.MetaName + ";" + S.IdShape.ToString());
            }
        }
        
        /**********************
         * Control EVENTS
         **********************/

        private void List_Shapes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = List_Shapes.SelectedIndex;
            if (List_Shapes.SelectedIndex == -1)
                return;
            UTIL.parsedObj result = UTIL.parsing.parseString(List_Shapes.SelectedItems[0].ToString(), ";, ");
            SelectedShape = Shape.GetShapeById((int)result.value[0]);
            foreach (Shape S in Shape.ShapeList)
            {
                S.isActiveShape = false;
            }
            SelectedShape.isActiveShape = true;
            List_Shapes.SelectedIndex = index;
            Invalidate();
        }

        private void Button_RemoveShape_Click(object sender, EventArgs e)
        {
            if (SelectedShape != null)
                Shape.ShapeList.Remove(SelectedShape);
            Invalidate();
        }

        private void BasicCad_Click(object sender, EventArgs e)
        {
            PointF cursPos = this.PointToClient(Cursor.Position);

            if (Shape.gridSystem.IsWithinCurrentBounds(cursPos))
            {
                MouseEventArgs me = (MouseEventArgs)e;
                Shape.gridSystem.SnapCursorToPoint(cursPos);
                if (me.Button == MouseButtons.Right)
                {
                    PointF click = Shape.gridSystem.GetNearestSnapPoint(cursPos);
                    ClickCache.Enqueue(click);

                    if (!(InProcess) && ClickCache.Count == 2)
                    {
                        InProcess = true;
                        bool prevPositioning = Shape.gridSystem.relativePositioning;
                        Shape.gridSystem.relativePositioning = false;
                        PointF P1 = ClickCache.Dequeue();
                        PointF P2;
                        if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                        {
                            P2 = ClickCache.Peek();
                        }
                        else
                        {
                            P2 = ClickCache.Dequeue();
                        }
                        string lineInput = "L " + P1.X.ToString() + " " + P1.Y.ToString() + " " + P2.X.ToString() + " " + P2.Y.ToString();
                        ParseInput(lineInput);
                        Shape.gridSystem.relativePositioning = prevPositioning;
                        InProcess = false;
                    }
                }
                Invalidate();
            }
        }

        private void BasicCad_ResizeEnd(object sender, EventArgs e)
        {
            Shape.gridSystem.resizeGrid();
            Invalidate();
        }

        private void BasicCad_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Shape.gridSystem.ZoomOut();
            Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Shape.gridSystem.ZoomIn();
            Invalidate();
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
    static class parsing
    {
        static char delim = ';';
        public static parsedObj parseString(string input,string delims)
        {
            foreach (char c in delims)
            {
                if (input.IndexOf(c) != -1)
                {
                    delim = c;
                }
            }
            string[] text_array = input.Split(delim);
            parsedObj retObj = new parsedObj();

            if (Regex.IsMatch(text_array[0], @"^[a-zA-Z]+?.*?")) // if starts with a string
            {
                retObj.type = text_array[0].ToUpper();
                if (text_array.Length > 1)
                {
                    for (int i = 1; i < text_array.Length; i++)
                    {
                        retObj.value.Add(float.Parse(text_array[i], CultureInfo.InvariantCulture.NumberFormat));
                    }
                }
            }

            //if (!(Regex.IsMatch(text_array[0], @"^[a-zA-Z]+?.*?"))) // if it doesn't start with a string
            //{
            //        for (int i = 0; i < text_array.Length; i++)
            //        {
            //            retObj.value.Add(float.Parse(text_array[i], CultureInfo.InvariantCulture.NumberFormat));
            //        }
            //    
            //}

            return retObj;
        }
    }

    public class parsedObj
    {
        public string type = null;
        public List<float> value = new List<float>();
    }
}