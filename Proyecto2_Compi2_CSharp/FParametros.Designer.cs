namespace Proyecto2_Compi2_CSharp
{
    partial class FParametros
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
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.txtNatrib = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTatrib = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(30, 111);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(59, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Arreglo";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(103, 141);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Añadir Atributo";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtNatrib
            // 
            this.txtNatrib.Location = new System.Drawing.Point(103, 70);
            this.txtNatrib.Name = "txtNatrib";
            this.txtNatrib.Size = new System.Drawing.Size(145, 20);
            this.txtNatrib.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Nombre";
            // 
            // txtTatrib
            // 
            this.txtTatrib.Location = new System.Drawing.Point(103, 34);
            this.txtTatrib.Name = "txtTatrib";
            this.txtTatrib.Size = new System.Drawing.Size(145, 20);
            this.txtTatrib.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Tipo";
            // 
            // FParametros
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 194);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtNatrib);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTatrib);
            this.Controls.Add(this.label2);
            this.Name = "FParametros";
            this.Text = "FParametros";
            this.Load += new System.EventHandler(this.FParametros_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtNatrib;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTatrib;
        private System.Windows.Forms.Label label2;
    }
}