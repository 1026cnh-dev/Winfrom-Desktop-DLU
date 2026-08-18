namespace Bai01
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
            this.nhapten = new System.Windows.Forms.Label();
            this.txtTen = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtsaochep = new System.Windows.Forms.TextBox();
            this.saochep = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // nhapten
            // 
            this.nhapten.AutoSize = true;
            this.nhapten.Location = new System.Drawing.Point(384, 80);
            this.nhapten.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nhapten.Name = "nhapten";
            this.nhapten.Size = new System.Drawing.Size(139, 20);
            this.nhapten.TabIndex = 0;
            this.nhapten.Text = "Nhập tên của bạn:";
            // 
            // txtTen
            // 
            this.txtTen.Location = new System.Drawing.Point(537, 80);
            this.txtTen.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTen.Name = "txtTen";
            this.txtTen.Size = new System.Drawing.Size(148, 26);
            this.txtTen.TabIndex = 1;
            this.txtTen.TextChanged += new System.EventHandler(this.txtTen_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(537, 120);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 37);
            this.button1.TabIndex = 2;
            this.button1.Text = "Xác nhận";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.XacNhan_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(384, 194);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Tên bạn vừa nhập:";
            // 
            // txtsaochep
            // 
            this.txtsaochep.Location = new System.Drawing.Point(540, 194);
            this.txtsaochep.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtsaochep.Name = "txtsaochep";
            this.txtsaochep.Size = new System.Drawing.Size(148, 26);
            this.txtsaochep.TabIndex = 4;
            // 
            // saochep
            // 
            this.saochep.Location = new System.Drawing.Point(540, 251);
            this.saochep.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.saochep.Name = "saochep";
            this.saochep.Size = new System.Drawing.Size(112, 35);
            this.saochep.TabIndex = 5;
            this.saochep.Text = "Sao chép";
            this.saochep.UseVisualStyleBackColor = true;
            this.saochep.Click += new System.EventHandler(this.saochep_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 692);
            this.Controls.Add(this.saochep);
            this.Controls.Add(this.txtsaochep);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtTen);
            this.Controls.Add(this.nhapten);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Bai01";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label nhapten;
        private System.Windows.Forms.TextBox txtTen;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtsaochep;
        private System.Windows.Forms.Button saochep;
    }
}

