namespace CD3_Bai3
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
            this.btnDocXML = new System.Windows.Forms.Button();
            this.btnGhiXML = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDocXML
            // 
            this.btnDocXML.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDocXML.Location = new System.Drawing.Point(72, 54);
            this.btnDocXML.Name = "btnDocXML";
            this.btnDocXML.Size = new System.Drawing.Size(137, 32);
            this.btnDocXML.TabIndex = 0;
            this.btnDocXML.Text = "Đọc XML";
            this.btnDocXML.UseVisualStyleBackColor = true;
            this.btnDocXML.Click += new System.EventHandler(this.btnDocXML_Click);
            // 
            // btnGhiXML
            // 
            this.btnGhiXML.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGhiXML.Location = new System.Drawing.Point(276, 54);
            this.btnGhiXML.Name = "btnGhiXML";
            this.btnGhiXML.Size = new System.Drawing.Size(137, 32);
            this.btnGhiXML.TabIndex = 1;
            this.btnGhiXML.Text = "Ghi XML";
            this.btnGhiXML.UseVisualStyleBackColor = true;
            this.btnGhiXML.Click += new System.EventHandler(this.btnGhiXML_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 265);
            this.Controls.Add(this.btnGhiXML);
            this.Controls.Add(this.btnDocXML);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDocXML;
        private System.Windows.Forms.Button btnGhiXML;
    }
}

