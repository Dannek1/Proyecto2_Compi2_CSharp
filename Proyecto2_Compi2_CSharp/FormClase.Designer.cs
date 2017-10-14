namespace Proyecto2_Compi2_CSharp
{
    partial class FormClase
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtNclase = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.txtNatrib = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTatrib = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.txtNfun = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTFun = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.txtNclase);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(22, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(270, 98);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Datos Principales";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(81, 66);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Crear Clase";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtNclase
            // 
            this.txtNclase.Location = new System.Drawing.Point(97, 39);
            this.txtNclase.Name = "txtNclase";
            this.txtNclase.Size = new System.Drawing.Size(145, 20);
            this.txtNclase.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nombre";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.txtNatrib);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtTatrib);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(22, 141);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(270, 206);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Atributos";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(24, 152);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(59, 17);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Arreglo";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(81, 177);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Añadir Atributo";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Visibilidad";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Publico +",
            "Privado -",
            "Protejido #"});
            this.comboBox1.Location = new System.Drawing.Point(97, 112);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(145, 21);
            this.comboBox1.TabIndex = 2;
            // 
            // txtNatrib
            // 
            this.txtNatrib.Location = new System.Drawing.Point(97, 75);
            this.txtNatrib.Name = "txtNatrib";
            this.txtNatrib.Size = new System.Drawing.Size(145, 20);
            this.txtNatrib.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Nombre";
            // 
            // txtTatrib
            // 
            this.txtTatrib.Location = new System.Drawing.Point(97, 39);
            this.txtTatrib.Name = "txtTatrib";
            this.txtTatrib.Size = new System.Drawing.Size(145, 20);
            this.txtTatrib.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Tipo";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox2);
            this.groupBox3.Controls.Add(this.button4);
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.comboBox2);
            this.groupBox3.Controls.Add(this.txtNfun);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.txtTFun);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Enabled = false;
            this.groupBox3.Location = new System.Drawing.Point(337, 24);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(270, 212);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Funciones";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(24, 154);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(59, 17);
            this.checkBox2.TabIndex = 7;
            this.checkBox2.Text = "Arreglo";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(134, 177);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(108, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "Añadir Parametro";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(24, 177);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Añadir Atributo";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Visibilidad";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Publico +",
            "Privado -",
            "Protejido #"});
            this.comboBox2.Location = new System.Drawing.Point(97, 112);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(145, 21);
            this.comboBox2.TabIndex = 2;
            // 
            // txtNfun
            // 
            this.txtNfun.Location = new System.Drawing.Point(97, 75);
            this.txtNfun.Name = "txtNfun";
            this.txtNfun.Size = new System.Drawing.Size(145, 20);
            this.txtNfun.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Nombre";
            // 
            // txtTFun
            // 
            this.txtTFun.Location = new System.Drawing.Point(97, 39);
            this.txtTFun.Name = "txtTFun";
            this.txtTFun.Size = new System.Drawing.Size(145, 20);
            this.txtTFun.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 42);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(28, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Tipo";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(410, 253);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(108, 23);
            this.button5.TabIndex = 7;
            this.button5.Text = "Cerrar";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // FormClase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 357);
            this.ControlBox = false;
            this.Controls.Add(this.button5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormClase";
            this.Text = "FormClase";
            this.Load += new System.EventHandler(this.FormClase_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtNclase;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox txtNatrib;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTatrib;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.TextBox txtNfun;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTFun;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
    }
}