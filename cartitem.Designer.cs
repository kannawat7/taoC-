namespace project2
{
    partial class cartitem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(cartitem));
            this.Cart_productpicturebox = new Guna.UI2.WinForms.Guna2PictureBox();
            this.Cart_productlbl = new System.Windows.Forms.Label();
            this.Cart_pricelbl = new System.Windows.Forms.Label();
            this.Cart_idlbl = new System.Windows.Forms.Label();
            this.cart_delbtn = new Guna.UI2.WinForms.Guna2Button();
            this.Cart_amountlbl = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Cart_productpicturebox)).BeginInit();
            this.SuspendLayout();
            // 
            // Cart_productpicturebox
            // 
            this.Cart_productpicturebox.FillColor = System.Drawing.Color.Transparent;
            this.Cart_productpicturebox.ImageRotate = 0F;
            this.Cart_productpicturebox.Location = new System.Drawing.Point(0, 0);
            this.Cart_productpicturebox.Name = "Cart_productpicturebox";
            this.Cart_productpicturebox.Size = new System.Drawing.Size(137, 132);
            this.Cart_productpicturebox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Cart_productpicturebox.TabIndex = 0;
            this.Cart_productpicturebox.TabStop = false;
            // 
            // Cart_productlbl
            // 
            this.Cart_productlbl.Font = new System.Drawing.Font("Bahnschrift Condensed", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cart_productlbl.Location = new System.Drawing.Point(158, 16);
            this.Cart_productlbl.Name = "Cart_productlbl";
            this.Cart_productlbl.Size = new System.Drawing.Size(258, 103);
            this.Cart_productlbl.TabIndex = 1;
            this.Cart_productlbl.Text = "Product";
            this.Cart_productlbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Cart_pricelbl
            // 
            this.Cart_pricelbl.AutoSize = true;
            this.Cart_pricelbl.Font = new System.Drawing.Font("Bahnschrift", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cart_pricelbl.Location = new System.Drawing.Point(475, 58);
            this.Cart_pricelbl.Name = "Cart_pricelbl";
            this.Cart_pricelbl.Size = new System.Drawing.Size(49, 21);
            this.Cart_pricelbl.TabIndex = 5;
            this.Cart_pricelbl.Text = "Price";
            // 
            // Cart_idlbl
            // 
            this.Cart_idlbl.AutoSize = true;
            this.Cart_idlbl.Font = new System.Drawing.Font("Bahnschrift", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cart_idlbl.Location = new System.Drawing.Point(846, 0);
            this.Cart_idlbl.Name = "Cart_idlbl";
            this.Cart_idlbl.Size = new System.Drawing.Size(23, 21);
            this.Cart_idlbl.TabIndex = 7;
            this.Cart_idlbl.Text = "id";
            this.Cart_idlbl.Visible = false;
            // 
            // cart_delbtn
            // 
            this.cart_delbtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.cart_delbtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.cart_delbtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.cart_delbtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.cart_delbtn.FillColor = System.Drawing.Color.Transparent;
            this.cart_delbtn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cart_delbtn.ForeColor = System.Drawing.Color.White;
            this.cart_delbtn.Image = ((System.Drawing.Image)(resources.GetObject("cart_delbtn.Image")));
            this.cart_delbtn.Location = new System.Drawing.Point(798, 44);
            this.cart_delbtn.Name = "cart_delbtn";
            this.cart_delbtn.Size = new System.Drawing.Size(59, 44);
            this.cart_delbtn.TabIndex = 9;
            this.cart_delbtn.Click += new System.EventHandler(this.cart_delbtn_Click);
            // 
            // Cart_amountlbl
            // 
            this.Cart_amountlbl.AutoSize = true;
            this.Cart_amountlbl.Font = new System.Drawing.Font("Bahnschrift", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cart_amountlbl.Location = new System.Drawing.Point(666, 58);
            this.Cart_amountlbl.Name = "Cart_amountlbl";
            this.Cart_amountlbl.Size = new System.Drawing.Size(69, 21);
            this.Cart_amountlbl.TabIndex = 10;
            this.Cart_amountlbl.Text = "Amount";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bahnschrift", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(612, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 21);
            this.label1.TabIndex = 11;
            this.label1.Text = "X";
            // 
            // cartitem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Cart_amountlbl);
            this.Controls.Add(this.cart_delbtn);
            this.Controls.Add(this.Cart_idlbl);
            this.Controls.Add(this.Cart_pricelbl);
            this.Controls.Add(this.Cart_productlbl);
            this.Controls.Add(this.Cart_productpicturebox);
            this.Name = "cartitem";
            this.Size = new System.Drawing.Size(869, 132);
            this.Load += new System.EventHandler(this.cartitem_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Cart_productpicturebox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2PictureBox Cart_productpicturebox;
        private System.Windows.Forms.Label Cart_productlbl;
        private System.Windows.Forms.Label Cart_pricelbl;
        private System.Windows.Forms.Label Cart_idlbl;
        private Guna.UI2.WinForms.Guna2Button cart_delbtn;
        private System.Windows.Forms.Label Cart_amountlbl;
        private System.Windows.Forms.Label label1;
    }
}
