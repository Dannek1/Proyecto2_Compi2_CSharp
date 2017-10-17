namespace Proyecto2_Compi2_CSharp
{
    partial class uml
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.uMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.analisisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.diagramaCodigoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.codigoDiagramaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtCodigo = new ScintillaNET.Scintilla();
            this.oLCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tREEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uMLToolStripMenuItem,
            this.analisisToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1258, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // uMLToolStripMenuItem
            // 
            this.uMLToolStripMenuItem.Name = "uMLToolStripMenuItem";
            this.uMLToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.uMLToolStripMenuItem.Text = "Regresar";
            this.uMLToolStripMenuItem.Click += new System.EventHandler(this.uMLToolStripMenuItem_Click);
            // 
            // analisisToolStripMenuItem
            // 
            this.analisisToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.diagramaCodigoToolStripMenuItem,
            this.codigoDiagramaToolStripMenuItem});
            this.analisisToolStripMenuItem.Name = "analisisToolStripMenuItem";
            this.analisisToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.analisisToolStripMenuItem.Text = "Analisis";
            // 
            // diagramaCodigoToolStripMenuItem
            // 
            this.diagramaCodigoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oLCToolStripMenuItem,
            this.tREEToolStripMenuItem});
            this.diagramaCodigoToolStripMenuItem.Name = "diagramaCodigoToolStripMenuItem";
            this.diagramaCodigoToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.diagramaCodigoToolStripMenuItem.Text = "Diagrama-Codigo";
            // 
            // codigoDiagramaToolStripMenuItem
            // 
            this.codigoDiagramaToolStripMenuItem.Name = "codigoDiagramaToolStripMenuItem";
            this.codigoDiagramaToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.codigoDiagramaToolStripMenuItem.Text = "Codigo-Diagrama";
            this.codigoDiagramaToolStripMenuItem.Click += new System.EventHandler(this.codigoDiagramaToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(220, 488);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Elementos UML";
            // 
            // button6
            // 
            this.button6.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.Image = global::Proyecto2_Compi2_CSharp.Properties.Resources.Dependencia;
            this.button6.Location = new System.Drawing.Point(33, 429);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(161, 46);
            this.button6.TabIndex = 5;
            this.button6.Text = "Dependencia";
            this.button6.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.Image = global::Proyecto2_Compi2_CSharp.Properties.Resources.Asociacion_2;
            this.button5.Location = new System.Drawing.Point(33, 376);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(161, 46);
            this.button5.TabIndex = 4;
            this.button5.Text = "Asociación";
            this.button5.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Image = global::Proyecto2_Compi2_CSharp.Properties.Resources.Composision;
            this.button4.Location = new System.Drawing.Point(33, 309);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(161, 56);
            this.button4.TabIndex = 3;
            this.button4.Text = "Composición";
            this.button4.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Image = global::Proyecto2_Compi2_CSharp.Properties.Resources.herencia1;
            this.button3.Location = new System.Drawing.Point(33, 249);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(161, 44);
            this.button3.TabIndex = 2;
            this.button3.Text = "Agregación";
            this.button3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Image = global::Proyecto2_Compi2_CSharp.Properties.Resources.herencia;
            this.button2.Location = new System.Drawing.Point(33, 189);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(161, 54);
            this.button2.TabIndex = 1;
            this.button2.Text = "Herencia";
            this.button2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Image = global::Proyecto2_Compi2_CSharp.Properties.Resources.clase1;
            this.button1.Location = new System.Drawing.Point(33, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(161, 164);
            this.button1.TabIndex = 0;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Location = new System.Drawing.Point(220, 24);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(672, 482);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Diseño de Clases";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(666, 463);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtCodigo);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox3.Location = new System.Drawing.Point(898, 24);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(360, 488);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Codigo de Alto Nivel";
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(3, 16);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(345, 463);
            this.txtCodigo.TabIndex = 0;
            // 
            // oLCToolStripMenuItem
            // 
            this.oLCToolStripMenuItem.Name = "oLCToolStripMenuItem";
            this.oLCToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.oLCToolStripMenuItem.Text = "OLC";
            this.oLCToolStripMenuItem.Click += new System.EventHandler(this.oLCToolStripMenuItem_Click);
            // 
            // tREEToolStripMenuItem
            // 
            this.tREEToolStripMenuItem.Name = "tREEToolStripMenuItem";
            this.tREEToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.tREEToolStripMenuItem.Text = "TREE";
            this.tREEToolStripMenuItem.Click += new System.EventHandler(this.tREEToolStripMenuItem_Click);
            // 
            // uml
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1258, 512);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "uml";
            this.Text = "uml";
            this.Load += new System.EventHandler(this.uml_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem uMLToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private ScintillaNET.Scintilla txtCodigo;
        private System.Windows.Forms.ToolStripMenuItem analisisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem diagramaCodigoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem codigoDiagramaToolStripMenuItem;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.ToolStripMenuItem oLCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tREEToolStripMenuItem;
    }
}