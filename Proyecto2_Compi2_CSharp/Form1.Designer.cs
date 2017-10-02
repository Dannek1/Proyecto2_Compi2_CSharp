namespace Proyecto2_Compi2_CSharp
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.uMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.depurarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.códigoCompartidoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tabCtrlSalidas = new System.Windows.Forms.TabControl();
            this.tabPConsolaS = new System.Windows.Forms.TabPage();
            this.txtConsola = new System.Windows.Forms.TextBox();
            this.tabPErrores = new System.Windows.Forms.TabPage();
            this.txtErrores = new System.Windows.Forms.TextBox();
            this.tabPOptim = new System.Windows.Forms.TabPage();
            this.txtOp = new System.Windows.Forms.TextBox();
            this.tabP3D = new System.Windows.Forms.TabPage();
            this.txt3D = new System.Windows.Forms.TextBox();
            this.tabP3Dop = new System.Windows.Forms.TabPage();
            this.txt3Dop = new System.Windows.Forms.TextBox();
            this.TabCEntradas = new System.Windows.Forms.TabControl();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabCtrlSalidas.SuspendLayout();
            this.tabPConsolaS.SuspendLayout();
            this.tabPErrores.SuspendLayout();
            this.tabPOptim.SuspendLayout();
            this.tabP3D.SuspendLayout();
            this.tabP3Dop.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.treeView1);
            this.groupBox1.Location = new System.Drawing.Point(0, 52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(192, 304);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Explorador de Solución";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uMLToolStripMenuItem,
            this.depurarToolStripMenuItem,
            this.códigoCompartidoToolStripMenuItem,
            this.reportesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(743, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // uMLToolStripMenuItem
            // 
            this.uMLToolStripMenuItem.Name = "uMLToolStripMenuItem";
            this.uMLToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.uMLToolStripMenuItem.Text = "UML";
            this.uMLToolStripMenuItem.Click += new System.EventHandler(this.uMLToolStripMenuItem_Click);
            // 
            // depurarToolStripMenuItem
            // 
            this.depurarToolStripMenuItem.Name = "depurarToolStripMenuItem";
            this.depurarToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.depurarToolStripMenuItem.Text = "Depurar";
            // 
            // códigoCompartidoToolStripMenuItem
            // 
            this.códigoCompartidoToolStripMenuItem.Name = "códigoCompartidoToolStripMenuItem";
            this.códigoCompartidoToolStripMenuItem.Size = new System.Drawing.Size(125, 20);
            this.códigoCompartidoToolStripMenuItem.Text = "Código Compartido";
            // 
            // reportesToolStripMenuItem
            // 
            this.reportesToolStripMenuItem.Name = "reportesToolStripMenuItem";
            this.reportesToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.reportesToolStripMenuItem.Text = "Reportes";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton6,
            this.toolStripButton1,
            this.toolStripButton5,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(743, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tabCtrlSalidas
            // 
            this.tabCtrlSalidas.Controls.Add(this.tabPConsolaS);
            this.tabCtrlSalidas.Controls.Add(this.tabPErrores);
            this.tabCtrlSalidas.Controls.Add(this.tabPOptim);
            this.tabCtrlSalidas.Controls.Add(this.tabP3D);
            this.tabCtrlSalidas.Controls.Add(this.tabP3Dop);
            this.tabCtrlSalidas.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabCtrlSalidas.Location = new System.Drawing.Point(0, 389);
            this.tabCtrlSalidas.Name = "tabCtrlSalidas";
            this.tabCtrlSalidas.SelectedIndex = 0;
            this.tabCtrlSalidas.Size = new System.Drawing.Size(743, 142);
            this.tabCtrlSalidas.TabIndex = 3;
            this.tabCtrlSalidas.SelectedIndexChanged += new System.EventHandler(this.tabCtrlSalidas_SelectedIndexChanged);
            // 
            // tabPConsolaS
            // 
            this.tabPConsolaS.Controls.Add(this.txtConsola);
            this.tabPConsolaS.Location = new System.Drawing.Point(4, 22);
            this.tabPConsolaS.Name = "tabPConsolaS";
            this.tabPConsolaS.Padding = new System.Windows.Forms.Padding(3);
            this.tabPConsolaS.Size = new System.Drawing.Size(735, 116);
            this.tabPConsolaS.TabIndex = 0;
            this.tabPConsolaS.Text = "Consola de Salida";
            this.tabPConsolaS.UseVisualStyleBackColor = true;
            // 
            // txtConsola
            // 
            this.txtConsola.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConsola.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConsola.Location = new System.Drawing.Point(3, 3);
            this.txtConsola.Multiline = true;
            this.txtConsola.Name = "txtConsola";
            this.txtConsola.Size = new System.Drawing.Size(729, 110);
            this.txtConsola.TabIndex = 0;
            // 
            // tabPErrores
            // 
            this.tabPErrores.Controls.Add(this.txtErrores);
            this.tabPErrores.Location = new System.Drawing.Point(4, 22);
            this.tabPErrores.Name = "tabPErrores";
            this.tabPErrores.Padding = new System.Windows.Forms.Padding(3);
            this.tabPErrores.Size = new System.Drawing.Size(735, 116);
            this.tabPErrores.TabIndex = 1;
            this.tabPErrores.Text = "Errores";
            this.tabPErrores.UseVisualStyleBackColor = true;
            // 
            // txtErrores
            // 
            this.txtErrores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtErrores.Location = new System.Drawing.Point(3, 3);
            this.txtErrores.Multiline = true;
            this.txtErrores.Name = "txtErrores";
            this.txtErrores.Size = new System.Drawing.Size(729, 110);
            this.txtErrores.TabIndex = 0;
            // 
            // tabPOptim
            // 
            this.tabPOptim.Controls.Add(this.txtOp);
            this.tabPOptim.Location = new System.Drawing.Point(4, 22);
            this.tabPOptim.Name = "tabPOptim";
            this.tabPOptim.Padding = new System.Windows.Forms.Padding(3);
            this.tabPOptim.Size = new System.Drawing.Size(735, 116);
            this.tabPOptim.TabIndex = 2;
            this.tabPOptim.Text = "Salida Proceso de Optimizacion";
            this.tabPOptim.UseVisualStyleBackColor = true;
            this.tabPOptim.Click += new System.EventHandler(this.tabPage3_Click);
            // 
            // txtOp
            // 
            this.txtOp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOp.Location = new System.Drawing.Point(3, 3);
            this.txtOp.Multiline = true;
            this.txtOp.Name = "txtOp";
            this.txtOp.Size = new System.Drawing.Size(729, 110);
            this.txtOp.TabIndex = 0;
            // 
            // tabP3D
            // 
            this.tabP3D.Controls.Add(this.txt3D);
            this.tabP3D.Location = new System.Drawing.Point(4, 22);
            this.tabP3D.Name = "tabP3D";
            this.tabP3D.Padding = new System.Windows.Forms.Padding(3);
            this.tabP3D.Size = new System.Drawing.Size(735, 116);
            this.tabP3D.TabIndex = 3;
            this.tabP3D.Text = "Codigo 3D";
            this.tabP3D.UseVisualStyleBackColor = true;
            // 
            // txt3D
            // 
            this.txt3D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt3D.Location = new System.Drawing.Point(3, 3);
            this.txt3D.Multiline = true;
            this.txt3D.Name = "txt3D";
            this.txt3D.ReadOnly = true;
            this.txt3D.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt3D.Size = new System.Drawing.Size(729, 110);
            this.txt3D.TabIndex = 0;
            this.txt3D.WordWrap = false;
            // 
            // tabP3Dop
            // 
            this.tabP3Dop.Controls.Add(this.txt3Dop);
            this.tabP3Dop.Location = new System.Drawing.Point(4, 22);
            this.tabP3Dop.Name = "tabP3Dop";
            this.tabP3Dop.Padding = new System.Windows.Forms.Padding(3);
            this.tabP3Dop.Size = new System.Drawing.Size(735, 116);
            this.tabP3Dop.TabIndex = 4;
            this.tabP3Dop.Text = "Codigo 3D Optimizado";
            this.tabP3Dop.UseVisualStyleBackColor = true;
            // 
            // txt3Dop
            // 
            this.txt3Dop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt3Dop.Location = new System.Drawing.Point(3, 3);
            this.txt3Dop.Multiline = true;
            this.txt3Dop.Name = "txt3Dop";
            this.txt3Dop.Size = new System.Drawing.Size(729, 110);
            this.txt3Dop.TabIndex = 0;
            // 
            // TabCEntradas
            // 
            this.TabCEntradas.Location = new System.Drawing.Point(207, 52);
            this.TabCEntradas.Name = "TabCEntradas";
            this.TabCEntradas.SelectedIndex = 0;
            this.TabCEntradas.Size = new System.Drawing.Size(524, 331);
            this.TabCEntradas.TabIndex = 4;
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.Image = global::Proyecto2_Compi2_CSharp.Properties.Resources.Documents;
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(53, 22);
            this.toolStripButton6.Text = "Abrir";
            this.toolStripButton6.Click += new System.EventHandler(this.toolStripButton6_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = global::Proyecto2_Compi2_CSharp.Properties.Resources.document_new;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(62, 22);
            this.toolStripButton1.Text = "Nuevo";
            this.toolStripButton1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.Image = global::Proyecto2_Compi2_CSharp.Properties.Resources.Close;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(125, 22);
            this.toolStripButton5.Text = "Cerrar Documento";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = global::Proyecto2_Compi2_CSharp.Properties.Resources.document_save;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(69, 22);
            this.toolStripButton2.Text = "Guardar";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Image = global::Proyecto2_Compi2_CSharp.Properties.Resources.Folder__3_;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(99, 22);
            this.toolStripButton3.Text = "Crear Carpeta";
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.Image = global::Proyecto2_Compi2_CSharp.Properties.Resources.Media_WMP;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(76, 22);
            this.toolStripButton4.Text = "Compilar";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(3, 16);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(186, 285);
            this.treeView1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 531);
            this.Controls.Add(this.TabCEntradas);
            this.Controls.Add(this.tabCtrlSalidas);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Proyecto 2";
            this.groupBox1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabCtrlSalidas.ResumeLayout(false);
            this.tabPConsolaS.ResumeLayout(false);
            this.tabPConsolaS.PerformLayout();
            this.tabPErrores.ResumeLayout(false);
            this.tabPErrores.PerformLayout();
            this.tabPOptim.ResumeLayout(false);
            this.tabPOptim.PerformLayout();
            this.tabP3D.ResumeLayout(false);
            this.tabP3D.PerformLayout();
            this.tabP3Dop.ResumeLayout(false);
            this.tabP3Dop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem uMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem depurarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem códigoCompartidoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportesToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.TabControl tabCtrlSalidas;
        private System.Windows.Forms.TabPage tabPConsolaS;
        private System.Windows.Forms.TabPage tabPErrores;
        private System.Windows.Forms.TabPage tabPOptim;
        private System.Windows.Forms.TabPage tabP3D;
        private System.Windows.Forms.TabPage tabP3Dop;
        private System.Windows.Forms.TabControl TabCEntradas;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.TextBox txtConsola;
        private System.Windows.Forms.TextBox txtErrores;
        private System.Windows.Forms.TextBox txtOp;
        private System.Windows.Forms.TextBox txt3D;
        private System.Windows.Forms.TextBox txt3Dop;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.TreeView treeView1;
    }
}

