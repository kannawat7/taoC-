namespace project2
{
    partial class status
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(status));
            this.label1 = new System.Windows.Forms.Label();
            this.OrderNolbl = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Statuslbl = new System.Windows.Forms.Label();
            this.slipPictureBox = new Guna.UI2.WinForms.Guna2PictureBox();
            this.axAcroPDF1 = new AxAcroPDFLib.AxAcroPDF();
            this.label3 = new System.Windows.Forms.Label();
            this.Commentlbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.slipPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axAcroPDF1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bahnschrift", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 390);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "Order";
            // 
            // OrderNolbl
            // 
            this.OrderNolbl.AutoSize = true;
            this.OrderNolbl.Font = new System.Drawing.Font("Bahnschrift Condensed", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OrderNolbl.Location = new System.Drawing.Point(81, 390);
            this.OrderNolbl.Name = "OrderNolbl";
            this.OrderNolbl.Size = new System.Drawing.Size(42, 21);
            this.OrderNolbl.TabIndex = 3;
            this.OrderNolbl.Text = "Order";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Bahnschrift", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(159, 390);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 21);
            this.label2.TabIndex = 4;
            this.label2.Text = "Status";
            // 
            // Statuslbl
            // 
            this.Statuslbl.AutoSize = true;
            this.Statuslbl.Font = new System.Drawing.Font("Bahnschrift Condensed", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Statuslbl.Location = new System.Drawing.Point(224, 390);
            this.Statuslbl.Name = "Statuslbl";
            this.Statuslbl.Size = new System.Drawing.Size(45, 21);
            this.Statuslbl.TabIndex = 5;
            this.Statuslbl.Text = "Status";
            // 
            // slipPictureBox
            // 
            this.slipPictureBox.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.slipPictureBox.ImageRotate = 0F;
            this.slipPictureBox.Location = new System.Drawing.Point(3, 3);
            this.slipPictureBox.Name = "slipPictureBox";
            this.slipPictureBox.Size = new System.Drawing.Size(425, 348);
            this.slipPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.slipPictureBox.TabIndex = 6;
            this.slipPictureBox.TabStop = false;
            // 
            // axAcroPDF1
            // 
            this.axAcroPDF1.Enabled = true;
            this.axAcroPDF1.Location = new System.Drawing.Point(327, 3);
            this.axAcroPDF1.Name = "axAcroPDF1";
            this.axAcroPDF1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAcroPDF1.OcxState")));
            this.axAcroPDF1.Size = new System.Drawing.Size(316, 283);
            this.axAcroPDF1.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Bahnschrift", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(369, 390);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 21);
            this.label3.TabIndex = 8;
            this.label3.Text = "Comment";
            // 
            // Commentlbl
            // 
            this.Commentlbl.Font = new System.Drawing.Font("Bahnschrift Condensed", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Commentlbl.Location = new System.Drawing.Point(459, 352);
            this.Commentlbl.Name = "Commentlbl";
            this.Commentlbl.Size = new System.Drawing.Size(398, 98);
            this.Commentlbl.TabIndex = 9;
            this.Commentlbl.Text = "Comment";
            this.Commentlbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // status
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.Commentlbl);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.axAcroPDF1);
            this.Controls.Add(this.slipPictureBox);
            this.Controls.Add(this.Statuslbl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.OrderNolbl);
            this.Controls.Add(this.label1);
            this.Name = "status";
            this.Size = new System.Drawing.Size(860, 450);
            ((System.ComponentModel.ISupportInitialize)(this.slipPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axAcroPDF1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label OrderNolbl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Statuslbl;
        private Guna.UI2.WinForms.Guna2PictureBox slipPictureBox;
        private AxAcroPDFLib.AxAcroPDF axAcroPDF1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label Commentlbl;
    }
}
