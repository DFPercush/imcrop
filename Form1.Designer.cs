
namespace imcrop
{
	partial class Form1
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
			this.components = new System.ComponentModel.Container();
			this.disp = new System.Windows.Forms.PictureBox();
			this.fileList = new System.Windows.Forms.ListView();
			this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.btnBrowseDest = new System.Windows.Forms.Button();
			this.btnBrowseSource = new System.Windows.Forms.Button();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitImageArea = new System.Windows.Forms.SplitContainer();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnMove = new System.Windows.Forms.Button();
			this.btnPNG = new System.Windows.Forms.Button();
			this.btnJpg = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.btnDuplicate = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.disp)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitImageArea)).BeginInit();
			this.splitImageArea.Panel1.SuspendLayout();
			this.splitImageArea.Panel2.SuspendLayout();
			this.splitImageArea.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// disp
			// 
			this.disp.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.disp.Cursor = System.Windows.Forms.Cursors.Default;
			this.disp.Location = new System.Drawing.Point(4, 38);
			this.disp.Margin = new System.Windows.Forms.Padding(20);
			this.disp.Name = "disp";
			this.disp.Size = new System.Drawing.Size(94, 71);
			this.disp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.disp.TabIndex = 0;
			this.disp.TabStop = false;
			this.disp.Click += new System.EventHandler(this.disp_Click);
			this.disp.Paint += new System.Windows.Forms.PaintEventHandler(this.disp_Paint);
			this.disp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.disp_MouseDown);
			this.disp.MouseMove += new System.Windows.Forms.MouseEventHandler(this.disp_MouseMove);
			this.disp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.disp_MouseUp);
			// 
			// fileList
			// 
			this.fileList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colDate,
            this.colSize});
			this.fileList.FullRowSelect = true;
			this.fileList.HideSelection = false;
			this.fileList.Location = new System.Drawing.Point(3, 38);
			this.fileList.Name = "fileList";
			this.fileList.Size = new System.Drawing.Size(127, 137);
			this.fileList.TabIndex = 2;
			this.fileList.UseCompatibleStateImageBehavior = false;
			this.fileList.View = System.Windows.Forms.View.Details;
			this.fileList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.fileList_ColumnClick);
			this.fileList.SelectedIndexChanged += new System.EventHandler(this.fileList_SelectedIndexChanged);
			// 
			// colName
			// 
			this.colName.Text = "Name";
			this.colName.Width = 101;
			// 
			// colDate
			// 
			this.colDate.Text = "Date";
			this.colDate.Width = 85;
			// 
			// colSize
			// 
			this.colSize.Text = "Size";
			this.colSize.Width = 72;
			// 
			// textBox1
			// 
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Location = new System.Drawing.Point(0, 0);
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(400, 20);
			this.textBox1.TabIndex = 3;
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// textBox2
			// 
			this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox2.Location = new System.Drawing.Point(0, 0);
			this.textBox2.Name = "textBox2";
			this.textBox2.ReadOnly = true;
			this.textBox2.Size = new System.Drawing.Size(392, 20);
			this.textBox2.TabIndex = 4;
			// 
			// btnBrowseDest
			// 
			this.btnBrowseDest.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnBrowseDest.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnBrowseDest.Location = new System.Drawing.Point(392, 0);
			this.btnBrowseDest.Name = "btnBrowseDest";
			this.btnBrowseDest.Size = new System.Drawing.Size(58, 24);
			this.btnBrowseDest.TabIndex = 5;
			this.btnBrowseDest.Text = "dest...";
			this.btnBrowseDest.UseVisualStyleBackColor = true;
			this.btnBrowseDest.Click += new System.EventHandler(this.btnBrowseDest_Click);
			// 
			// btnBrowseSource
			// 
			this.btnBrowseSource.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnBrowseSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnBrowseSource.Location = new System.Drawing.Point(400, 0);
			this.btnBrowseSource.Name = "btnBrowseSource";
			this.btnBrowseSource.Size = new System.Drawing.Size(40, 24);
			this.btnBrowseSource.TabIndex = 6;
			this.btnBrowseSource.Text = "src..";
			this.btnBrowseSource.UseVisualStyleBackColor = true;
			this.btnBrowseSource.Click += new System.EventHandler(this.btnBrowseSource_Click);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.textBox1);
			this.splitContainer1.Panel1.Controls.Add(this.btnBrowseSource);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.textBox2);
			this.splitContainer1.Panel2.Controls.Add(this.btnBrowseDest);
			this.splitContainer1.Size = new System.Drawing.Size(894, 24);
			this.splitContainer1.SplitterDistance = 440;
			this.splitContainer1.TabIndex = 7;
			// 
			// splitImageArea
			// 
			this.splitImageArea.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitImageArea.Location = new System.Drawing.Point(0, 24);
			this.splitImageArea.Name = "splitImageArea";
			// 
			// splitImageArea.Panel1
			// 
			this.splitImageArea.Panel1.Controls.Add(this.fileList);
			// 
			// splitImageArea.Panel2
			// 
			this.splitImageArea.Panel2.Controls.Add(this.disp);
			this.splitImageArea.Size = new System.Drawing.Size(894, 538);
			this.splitImageArea.SplitterDistance = 298;
			this.splitImageArea.TabIndex = 10;
			this.splitImageArea.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitImageArea_SplitterMoved);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.btnDelete);
			this.flowLayoutPanel1.Controls.Add(this.btnDuplicate);
			this.flowLayoutPanel1.Controls.Add(this.btnMove);
			this.flowLayoutPanel1.Controls.Add(this.btnPNG);
			this.flowLayoutPanel1.Controls.Add(this.btnJpg);
			this.flowLayoutPanel1.Controls.Add(this.label1);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 24);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(894, 32);
			this.flowLayoutPanel1.TabIndex = 11;
			// 
			// btnDelete
			// 
			this.btnDelete.Location = new System.Drawing.Point(816, 3);
			this.btnDelete.Margin = new System.Windows.Forms.Padding(50, 3, 3, 3);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(75, 23);
			this.btnDelete.TabIndex = 0;
			this.btnDelete.Text = "DELETE";
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnMove
			// 
			this.btnMove.Location = new System.Drawing.Point(560, 3);
			this.btnMove.Margin = new System.Windows.Forms.Padding(50, 3, 3, 3);
			this.btnMove.Name = "btnMove";
			this.btnMove.Size = new System.Drawing.Size(75, 23);
			this.btnMove.TabIndex = 4;
			this.btnMove.Text = "Move";
			this.btnMove.UseVisualStyleBackColor = true;
			this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
			// 
			// btnPNG
			// 
			this.btnPNG.Location = new System.Drawing.Point(396, 3);
			this.btnPNG.Margin = new System.Windows.Forms.Padding(50, 3, 3, 3);
			this.btnPNG.Name = "btnPNG";
			this.btnPNG.Size = new System.Drawing.Size(111, 23);
			this.btnPNG.TabIndex = 1;
			this.btnPNG.Text = "PNG";
			this.btnPNG.UseVisualStyleBackColor = true;
			this.btnPNG.Click += new System.EventHandler(this.btnPNG_Click);
			// 
			// btnJpg
			// 
			this.btnJpg.Location = new System.Drawing.Point(230, 3);
			this.btnJpg.Margin = new System.Windows.Forms.Padding(50, 3, 3, 3);
			this.btnJpg.Name = "btnJpg";
			this.btnJpg.Size = new System.Drawing.Size(113, 23);
			this.btnJpg.TabIndex = 2;
			this.btnJpg.Text = "JPG";
			this.btnJpg.UseVisualStyleBackColor = true;
			this.btnJpg.Click += new System.EventHandler(this.btnJpg_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(142, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "label1";
			// 
			// timer1
			// 
			this.timer1.Interval = 1000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// btnDuplicate
			// 
			this.btnDuplicate.Location = new System.Drawing.Point(688, 3);
			this.btnDuplicate.Margin = new System.Windows.Forms.Padding(50, 3, 3, 3);
			this.btnDuplicate.Name = "btnDuplicate";
			this.btnDuplicate.Size = new System.Drawing.Size(75, 23);
			this.btnDuplicate.TabIndex = 5;
			this.btnDuplicate.Text = "Duplicate";
			this.btnDuplicate.UseVisualStyleBackColor = true;
			this.btnDuplicate.Click += new System.EventHandler(this.btnDuplicate_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(894, 562);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.splitImageArea);
			this.Controls.Add(this.splitContainer1);
			this.KeyPreview = true;
			this.Name = "Form1";
			this.Text = "imcrop";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
			this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Form1_PreviewKeyDown);
			this.Resize += new System.EventHandler(this.Form1_Resize);
			((System.ComponentModel.ISupportInitialize)(this.disp)).EndInit();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitImageArea.Panel1.ResumeLayout(false);
			this.splitImageArea.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitImageArea)).EndInit();
			this.splitImageArea.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox disp;
		private System.Windows.Forms.ListView fileList;
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.ColumnHeader colDate;
		private System.Windows.Forms.ColumnHeader colSize;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Button btnBrowseDest;
		private System.Windows.Forms.Button btnBrowseSource;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitImageArea;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnPNG;
		private System.Windows.Forms.Button btnJpg;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnMove;
		private System.Windows.Forms.Button btnDuplicate;
	}
}

