namespace project2
{
    partial class product
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(product));
            this.Product_image = new Guna.UI2.WinForms.Guna2PictureBox();
            this.product_lbl = new System.Windows.Forms.Label();
            this.price_lbl = new System.Windows.Forms.Label();
            this.id_lbl = new System.Windows.Forms.Label();
            this.cartdetailview = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)(this.Product_image)).BeginInit();
            this.SuspendLayout();
            // 
            // Product_image
            // 
            this.Product_image.BackColor = System.Drawing.Color.Transparent;
            this.Product_image.FillColor = System.Drawing.Color.Transparent;
            this.Product_image.ImageRotate = 0F;
            this.Product_image.Location = new System.Drawing.Point(0, 0);
            this.Product_image.Name = "Product_image";
            this.Product_image.Size = new System.Drawing.Size(234, 194);
            this.Product_image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Product_image.TabIndex = 0;
            this.Product_image.TabStop = false;
            // 
            // product_lbl
            // 
            this.product_lbl.Font = new System.Drawing.Font("Bahnschrift Condensed", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.product_lbl.Location = new System.Drawing.Point(3, 197);
            this.product_lbl.Name = "product_lbl";
            this.product_lbl.Size = new System.Drawing.Size(228, 71);
            this.product_lbl.TabIndex = 1;
            this.product_lbl.Text = "product";
            this.product_lbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.product_lbl.MouseEnter += new System.EventHandler(this.product_lbl_MouseEnter);
            this.product_lbl.MouseLeave += new System.EventHandler(this.product_lbl_MouseLeave);
            // 
            // price_lbl
            // 
            this.price_lbl.Font = new System.Drawing.Font("Bahnschrift Condensed", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.price_lbl.Location = new System.Drawing.Point(-8, 268);
            this.price_lbl.Name = "price_lbl";
            this.price_lbl.Size = new System.Drawing.Size(242, 27);
            this.price_lbl.TabIndex = 2;
            this.price_lbl.Text = "price";
            this.price_lbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.price_lbl.MouseEnter += new System.EventHandler(this.price_lbl_MouseEnter);
            this.price_lbl.MouseLeave += new System.EventHandler(this.price_lbl_MouseLeave);
            // 
            // id_lbl
            // 
            this.id_lbl.AutoSize = true;
            this.id_lbl.Location = new System.Drawing.Point(216, 0);
            this.id_lbl.Name = "id_lbl";
            this.id_lbl.Size = new System.Drawing.Size(18, 16);
            this.id_lbl.TabIndex = 3;
            this.id_lbl.Text = "id";
            this.id_lbl.Visible = false;
            // 
            // cartdetailview
            // 
            this.cartdetailview.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.cartdetailview.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.cartdetailview.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.cartdetailview.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.cartdetailview.FillColor = System.Drawing.Color.White;
            this.cartdetailview.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cartdetailview.ForeColor = System.Drawing.Color.White;
            this.cartdetailview.Image = ((System.Drawing.Image)(resources.GetObject("cartdetailview.Image")));
            this.cartdetailview.Location = new System.Drawing.Point(184, 268);
            this.cartdetailview.Name = "cartdetailview";
            this.cartdetailview.Size = new System.Drawing.Size(47, 24);
            this.cartdetailview.TabIndex = 5;
            this.cartdetailview.Click += new System.EventHandler(this.cartdetailview_Click);
            // 
            // product
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.cartdetailview);
            this.Controls.Add(this.id_lbl);
            this.Controls.Add(this.price_lbl);
            this.Controls.Add(this.product_lbl);
            this.Controls.Add(this.Product_image);
            this.Name = "product";
            this.Size = new System.Drawing.Size(234, 295);
            this.Load += new System.EventHandler(this.product_Load);
            this.MouseEnter += new System.EventHandler(this.product_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.product_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.Product_image)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2PictureBox Product_image;
        private System.Windows.Forms.Label product_lbl;
        private System.Windows.Forms.Label price_lbl;
        private System.Windows.Forms.Label id_lbl;
        private Guna.UI2.WinForms.Guna2Button cartdetailview;
    }
}
