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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BasicCad_Form));
            this.Container_Shapes = new System.Windows.Forms.Panel();
            this.treeView = new System.Windows.Forms.TreeView();
            this.txt_Input = new System.Windows.Forms.TextBox();
            this.Container_Control = new System.Windows.Forms.Panel();
            this.txt_Console = new System.Windows.Forms.TextBox();
            this.ZoomOut = new System.Windows.Forms.Button();
            this.ZoomIn = new System.Windows.Forms.Button();
            this.AdjustSnapDistance = new System.Windows.Forms.NumericUpDown();
            this.ZoomScale = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.originToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lineSnapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onlyActiveLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDesignDialog = new System.Windows.Forms.SaveFileDialog();
            this.loadDesignDialog = new System.Windows.Forms.OpenFileDialog();
            this.label_Snap = new System.Windows.Forms.Label();
            this.label_Zoom = new System.Windows.Forms.Label();
            this.TabContainer = new System.Windows.Forms.TabControl();
            this.tabTools = new System.Windows.Forms.TabPage();
            this.ToolsFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.togg2PLine = new System.Windows.Forms.CheckBox();
            this.toggMoveDim = new System.Windows.Forms.CheckBox();
            this.TabExport = new System.Windows.Forms.TabPage();
            this.Container_Shapes.SuspendLayout();
            this.Container_Control.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AdjustSnapDistance)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.TabContainer.SuspendLayout();
            this.tabTools.SuspendLayout();
            this.ToolsFlowLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // Container_Shapes
            // 
            this.Container_Shapes.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Container_Shapes.Controls.Add(this.treeView);
            this.Container_Shapes.Dock = System.Windows.Forms.DockStyle.Left;
            this.Container_Shapes.Location = new System.Drawing.Point(0, 24);
            this.Container_Shapes.Name = "Container_Shapes";
            this.Container_Shapes.Size = new System.Drawing.Size(145, 748);
            this.Container_Shapes.TabIndex = 1;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView.FullRowSelect = true;
            this.treeView.HideSelection = false;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(145, 748);
            this.treeView.TabIndex = 3;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
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
            this.Container_Control.Location = new System.Drawing.Point(0, 772);
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
            this.ZoomOut.Location = new System.Drawing.Point(759, 49);
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
            this.ZoomIn.Location = new System.Drawing.Point(804, 49);
            this.ZoomIn.Name = "ZoomIn";
            this.ZoomIn.Size = new System.Drawing.Size(32, 32);
            this.ZoomIn.TabIndex = 3;
            this.ZoomIn.Text = "+";
            this.ZoomIn.UseVisualStyleBackColor = false;
            this.ZoomIn.Click += new System.EventHandler(this.button2_Click);
            // 
            // AdjustSnapDistance
            // 
            this.AdjustSnapDistance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AdjustSnapDistance.DecimalPlaces = 4;
            this.AdjustSnapDistance.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AdjustSnapDistance.Increment = new decimal(new int[] {
            625,
            0,
            0,
            262144});
            this.AdjustSnapDistance.Location = new System.Drawing.Point(666, 54);
            this.AdjustSnapDistance.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.AdjustSnapDistance.Minimum = new decimal(new int[] {
            625,
            0,
            0,
            262144});
            this.AdjustSnapDistance.Name = "AdjustSnapDistance";
            this.AdjustSnapDistance.Size = new System.Drawing.Size(78, 26);
            this.AdjustSnapDistance.TabIndex = 4;
            this.AdjustSnapDistance.Value = new decimal(new int[] {
            625,
            0,
            0,
            262144});
            this.AdjustSnapDistance.ValueChanged += new System.EventHandler(this.AdjustSnapDistance_ValueChanged);
            // 
            // ZoomScale
            // 
            this.ZoomScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ZoomScale.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ZoomScale.Location = new System.Drawing.Point(759, 88);
            this.ZoomScale.MaxLength = 100;
            this.ZoomScale.Name = "ZoomScale";
            this.ZoomScale.ReadOnly = true;
            this.ZoomScale.ShortcutsEnabled = false;
            this.ZoomScale.Size = new System.Drawing.Size(77, 23);
            this.ZoomScale.TabIndex = 5;
            this.ZoomScale.Text = "100%";
            this.ZoomScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Menu;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(945, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveTestToolStripMenuItem,
            this.loadTestToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveTestToolStripMenuItem
            // 
            this.saveTestToolStripMenuItem.Name = "saveTestToolStripMenuItem";
            this.saveTestToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.saveTestToolStripMenuItem.Text = "Save";
            this.saveTestToolStripMenuItem.Click += new System.EventHandler(this.saveTestToolStripMenuItem_Click);
            // 
            // loadTestToolStripMenuItem
            // 
            this.loadTestToolStripMenuItem.Name = "loadTestToolStripMenuItem";
            this.loadTestToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.loadTestToolStripMenuItem.Text = "Load";
            this.loadTestToolStripMenuItem.Click += new System.EventHandler(this.loadTestToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.originToolStripMenuItem,
            this.gridToolStripMenuItem,
            this.lineSnapsToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // originToolStripMenuItem
            // 
            this.originToolStripMenuItem.Checked = true;
            this.originToolStripMenuItem.CheckOnClick = true;
            this.originToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.originToolStripMenuItem.Name = "originToolStripMenuItem";
            this.originToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.originToolStripMenuItem.Text = "Origin";
            this.originToolStripMenuItem.Click += new System.EventHandler(this.originToolStripMenuItem_Click);
            // 
            // gridToolStripMenuItem
            // 
            this.gridToolStripMenuItem.Checked = true;
            this.gridToolStripMenuItem.CheckOnClick = true;
            this.gridToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.gridToolStripMenuItem.Name = "gridToolStripMenuItem";
            this.gridToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.gridToolStripMenuItem.Text = "Grid";
            this.gridToolStripMenuItem.Click += new System.EventHandler(this.gridToolStripMenuItem_Click);
            // 
            // lineSnapsToolStripMenuItem
            // 
            this.lineSnapsToolStripMenuItem.Checked = true;
            this.lineSnapsToolStripMenuItem.CheckOnClick = true;
            this.lineSnapsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.lineSnapsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.onlyActiveLineToolStripMenuItem});
            this.lineSnapsToolStripMenuItem.Name = "lineSnapsToolStripMenuItem";
            this.lineSnapsToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.lineSnapsToolStripMenuItem.Text = "Line Snaps";
            this.lineSnapsToolStripMenuItem.Click += new System.EventHandler(this.lineSnapsToolStripMenuItem_Click);
            // 
            // onlyActiveLineToolStripMenuItem
            // 
            this.onlyActiveLineToolStripMenuItem.Name = "onlyActiveLineToolStripMenuItem";
            this.onlyActiveLineToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.onlyActiveLineToolStripMenuItem.Text = "Only Active Line";
            this.onlyActiveLineToolStripMenuItem.Click += new System.EventHandler(this.onlyActiveLineToolStripMenuItem_Click);
            // 
            // saveDesignDialog
            // 
            this.saveDesignDialog.DefaultExt = "bc";
            this.saveDesignDialog.FileName = "design.bc";
            this.saveDesignDialog.Filter = "BasicCad files|*.bc";
            this.saveDesignDialog.SupportMultiDottedExtensions = true;
            this.saveDesignDialog.Title = "Save Design";
            // 
            // loadDesignDialog
            // 
            this.loadDesignDialog.FileName = "design.bs";
            this.loadDesignDialog.Filter = "BasicCad files|*.bc|All files|*.*";
            this.loadDesignDialog.Title = "Load saved design";
            // 
            // label_Snap
            // 
            this.label_Snap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Snap.Font = new System.Drawing.Font("Consolas", 8F);
            this.label_Snap.Location = new System.Drawing.Point(659, 34);
            this.label_Snap.Name = "label_Snap";
            this.label_Snap.Size = new System.Drawing.Size(92, 13);
            this.label_Snap.TabIndex = 7;
            this.label_Snap.Text = "Snap Spacing";
            this.label_Snap.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Zoom
            // 
            this.label_Zoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Zoom.Font = new System.Drawing.Font("Consolas", 8F);
            this.label_Zoom.Location = new System.Drawing.Point(757, 34);
            this.label_Zoom.Name = "label_Zoom";
            this.label_Zoom.Size = new System.Drawing.Size(80, 13);
            this.label_Zoom.TabIndex = 8;
            this.label_Zoom.Text = "Grid Zoom";
            this.label_Zoom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TabContainer
            // 
            this.TabContainer.Controls.Add(this.tabTools);
            this.TabContainer.Controls.Add(this.TabExport);
            this.TabContainer.Dock = System.Windows.Forms.DockStyle.Right;
            this.TabContainer.Location = new System.Drawing.Point(845, 24);
            this.TabContainer.Name = "TabContainer";
            this.TabContainer.SelectedIndex = 0;
            this.TabContainer.Size = new System.Drawing.Size(100, 748);
            this.TabContainer.TabIndex = 9;
            // 
            // tabTools
            // 
            this.tabTools.Controls.Add(this.ToolsFlowLayout);
            this.tabTools.Location = new System.Drawing.Point(4, 22);
            this.tabTools.Name = "tabTools";
            this.tabTools.Padding = new System.Windows.Forms.Padding(3);
            this.tabTools.Size = new System.Drawing.Size(92, 722);
            this.tabTools.TabIndex = 0;
            this.tabTools.Text = "Tools";
            this.tabTools.UseVisualStyleBackColor = true;
            // 
            // ToolsFlowLayout
            // 
            this.ToolsFlowLayout.Controls.Add(this.togg2PLine);
            this.ToolsFlowLayout.Controls.Add(this.toggMoveDim);
            this.ToolsFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolsFlowLayout.Location = new System.Drawing.Point(3, 3);
            this.ToolsFlowLayout.Name = "ToolsFlowLayout";
            this.ToolsFlowLayout.Size = new System.Drawing.Size(86, 716);
            this.ToolsFlowLayout.TabIndex = 0;
            // 
            // togg2PLine
            // 
            this.togg2PLine.Appearance = System.Windows.Forms.Appearance.Button;
            this.togg2PLine.Location = new System.Drawing.Point(3, 3);
            this.togg2PLine.Name = "togg2PLine";
            this.togg2PLine.Size = new System.Drawing.Size(80, 40);
            this.togg2PLine.TabIndex = 12;
            this.togg2PLine.Text = "2P Line";
            this.togg2PLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.togg2PLine.UseVisualStyleBackColor = true;
            this.togg2PLine.Click += new System.EventHandler(this.togg2PLine_Click);
            // 
            // toggMoveDim
            // 
            this.toggMoveDim.Appearance = System.Windows.Forms.Appearance.Button;
            this.toggMoveDim.Location = new System.Drawing.Point(3, 49);
            this.toggMoveDim.Name = "toggMoveDim";
            this.toggMoveDim.Size = new System.Drawing.Size(80, 40);
            this.toggMoveDim.TabIndex = 13;
            this.toggMoveDim.Text = "Move Dim";
            this.toggMoveDim.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toggMoveDim.UseVisualStyleBackColor = true;
            this.toggMoveDim.Click += new System.EventHandler(this.toggMoveDim_Click);
            // 
            // TabExport
            // 
            this.TabExport.Location = new System.Drawing.Point(4, 22);
            this.TabExport.Name = "TabExport";
            this.TabExport.Padding = new System.Windows.Forms.Padding(3);
            this.TabExport.Size = new System.Drawing.Size(92, 722);
            this.TabExport.TabIndex = 1;
            this.TabExport.Text = "Other";
            this.TabExport.UseVisualStyleBackColor = true;
            // 
            // BasicCad_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(945, 912);
            this.Controls.Add(this.TabContainer);
            this.Controls.Add(this.label_Zoom);
            this.Controls.Add(this.label_Snap);
            this.Controls.Add(this.ZoomScale);
            this.Controls.Add(this.AdjustSnapDistance);
            this.Controls.Add(this.ZoomIn);
            this.Controls.Add(this.ZoomOut);
            this.Controls.Add(this.Container_Shapes);
            this.Controls.Add(this.Container_Control);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(961, 950);
            this.Name = "BasicCad_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BasicCAD";
            this.Load += new System.EventHandler(this.BasicCad_Load);
            this.ResizeEnd += new System.EventHandler(this.BasicCad_ResizeEnd);
            this.Click += new System.EventHandler(this.BasicCad_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.BasicCad_Paint);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BasicCad_Form_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.BasicCad_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BasicCad_Form_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BasicCad_Form_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BasicCad_Form_MouseUp);
            this.Container_Shapes.ResumeLayout(false);
            this.Container_Control.ResumeLayout(false);
            this.Container_Control.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AdjustSnapDistance)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.TabContainer.ResumeLayout(false);
            this.tabTools.ResumeLayout(false);
            this.ToolsFlowLayout.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel Container_Shapes;
        private System.Windows.Forms.TextBox txt_Input;
        private System.Windows.Forms.Panel Container_Control;
        private System.Windows.Forms.TextBox txt_Console;
        private System.Windows.Forms.Button ZoomOut;
        private System.Windows.Forms.Button ZoomIn;
        private System.Windows.Forms.NumericUpDown AdjustSnapDistance;
        private System.Windows.Forms.TextBox ZoomScale;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveTestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadTestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveDesignDialog;
        private System.Windows.Forms.OpenFileDialog loadDesignDialog;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem originToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gridToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lineSnapsToolStripMenuItem;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Label label_Snap;
        private System.Windows.Forms.Label label_Zoom;
        private System.Windows.Forms.TabControl TabContainer;
        private System.Windows.Forms.TabPage tabTools;
        private System.Windows.Forms.TabPage TabExport;
        private System.Windows.Forms.FlowLayoutPanel ToolsFlowLayout;
        private System.Windows.Forms.ToolStripMenuItem onlyActiveLineToolStripMenuItem;
        private System.Windows.Forms.CheckBox togg2PLine;
        private System.Windows.Forms.CheckBox toggMoveDim;
    }
}

