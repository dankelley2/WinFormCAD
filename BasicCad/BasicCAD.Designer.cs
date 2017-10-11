namespace BasicCad
{
    partial class BasicCad_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Container_Shapes = new System.Windows.Forms.Panel();
            this.List_Shapes = new System.Windows.Forms.ListBox();
            this.Button_RemoveShape = new System.Windows.Forms.Button();
            this.txt_Input = new System.Windows.Forms.TextBox();
            this.Container_Control = new System.Windows.Forms.Panel();
            this.txt_Console = new System.Windows.Forms.TextBox();
            this.ZoomOut = new System.Windows.Forms.Button();
            this.ZoomIn = new System.Windows.Forms.Button();
            this.Container_Shapes.SuspendLayout();
            this.Container_Control.SuspendLayout();
            this.SuspendLayout();
            // 
            // Container_Shapes
            // 
            this.Container_Shapes.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Container_Shapes.Controls.Add(this.List_Shapes);
            this.Container_Shapes.Controls.Add(this.Button_RemoveShape);
            this.Container_Shapes.Dock = System.Windows.Forms.DockStyle.Left;
            this.Container_Shapes.Location = new System.Drawing.Point(0, 0);
            this.Container_Shapes.Name = "Container_Shapes";
            this.Container_Shapes.Size = new System.Drawing.Size(145, 771);
            this.Container_Shapes.TabIndex = 1;
            // 
            // List_Shapes
            // 
            this.List_Shapes.BackColor = System.Drawing.SystemColors.Menu;
            this.List_Shapes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.List_Shapes.FormattingEnabled = true;
            this.List_Shapes.IntegralHeight = false;
            this.List_Shapes.Location = new System.Drawing.Point(0, 23);
            this.List_Shapes.MinimumSize = new System.Drawing.Size(145, 484);
            this.List_Shapes.Name = "List_Shapes";
            this.List_Shapes.Size = new System.Drawing.Size(145, 748);
            this.List_Shapes.TabIndex = 2;
            this.List_Shapes.SelectedIndexChanged += new System.EventHandler(this.List_Shapes_SelectedIndexChanged);
            // 
            // Button_RemoveShape
            // 
            this.Button_RemoveShape.Dock = System.Windows.Forms.DockStyle.Top;
            this.Button_RemoveShape.Location = new System.Drawing.Point(0, 0);
            this.Button_RemoveShape.Name = "Button_RemoveShape";
            this.Button_RemoveShape.Size = new System.Drawing.Size(145, 23);
            this.Button_RemoveShape.TabIndex = 1;
            this.Button_RemoveShape.Text = "Remove Selected";
            this.Button_RemoveShape.UseVisualStyleBackColor = true;
            this.Button_RemoveShape.Click += new System.EventHandler(this.Button_RemoveShape_Click);
            // 
            // txt_Input
            // 
            this.txt_Input.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txt_Input.Location = new System.Drawing.Point(0, 120);
            this.txt_Input.MinimumSize = new System.Drawing.Size(945, 20);
            this.txt_Input.Name = "txt_Input";
            this.txt_Input.Size = new System.Drawing.Size(945, 20);
            this.txt_Input.TabIndex = 0;
            this.txt_Input.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Textbox_Input_KeyDown);
            // 
            // Container_Control
            // 
            this.Container_Control.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Container_Control.Controls.Add(this.txt_Console);
            this.Container_Control.Controls.Add(this.txt_Input);
            this.Container_Control.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Container_Control.Location = new System.Drawing.Point(0, 771);
            this.Container_Control.Name = "Container_Control";
            this.Container_Control.Size = new System.Drawing.Size(945, 140);
            this.Container_Control.TabIndex = 0;
            // 
            // txt_Console
            // 
            this.txt_Console.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txt_Console.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Console.ForeColor = System.Drawing.Color.Beige;
            this.txt_Console.Location = new System.Drawing.Point(0, 0);
            this.txt_Console.Multiline = true;
            this.txt_Console.Name = "txt_Console";
            this.txt_Console.ReadOnly = true;
            this.txt_Console.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_Console.Size = new System.Drawing.Size(945, 120);
            this.txt_Console.TabIndex = 1;
            // 
            // ZoomOut
            // 
            this.ZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ZoomOut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ZoomOut.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold);
            this.ZoomOut.Location = new System.Drawing.Point(856, 12);
            this.ZoomOut.Name = "ZoomOut";
            this.ZoomOut.Size = new System.Drawing.Size(32, 32);
            this.ZoomOut.TabIndex = 2;
            this.ZoomOut.Text = "-";
            this.ZoomOut.UseVisualStyleBackColor = false;
            this.ZoomOut.Click += new System.EventHandler(this.button1_Click);
            // 
            // ZoomIn
            // 
            this.ZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ZoomIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ZoomIn.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ZoomIn.Location = new System.Drawing.Point(901, 12);
            this.ZoomIn.Name = "ZoomIn";
            this.ZoomIn.Size = new System.Drawing.Size(32, 32);
            this.ZoomIn.TabIndex = 3;
            this.ZoomIn.Text = "+";
            this.ZoomIn.UseVisualStyleBackColor = false;
            this.ZoomIn.Click += new System.EventHandler(this.button2_Click);
            // 
            // BasicCad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(945, 911);
            this.Controls.Add(this.ZoomIn);
            this.Controls.Add(this.ZoomOut);
            this.Controls.Add(this.Container_Shapes);
            this.Controls.Add(this.Container_Control);
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(961, 950);
            this.Name = "BasicCad";
            this.Text = "BasicCAD";
            this.Load += new System.EventHandler(this.BasicCad_Load);
            this.ResizeEnd += new System.EventHandler(this.BasicCad_ResizeEnd);
            this.Click += new System.EventHandler(this.BasicCad_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.BasicCad_Paint);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.BasicCad_KeyUp);
            this.Container_Shapes.ResumeLayout(false);
            this.Container_Control.ResumeLayout(false);
            this.Container_Control.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel Container_Shapes;
        private System.Windows.Forms.ListBox List_Shapes;
        private System.Windows.Forms.Button Button_RemoveShape;
        private System.Windows.Forms.TextBox txt_Input;
        private System.Windows.Forms.Panel Container_Control;
        private System.Windows.Forms.TextBox txt_Console;
        private System.Windows.Forms.Button ZoomOut;
        private System.Windows.Forms.Button ZoomIn;
    }
}

