using iTextSharp.text.pdf;
using iTextSharp.text;
using MySql.Data.MySqlClient;


using System;
using System.Collections.Generic;

using System.Drawing;
using System.IO;

using System.Text.RegularExpressions;

using System.Windows.Forms;




using System.Net.Mail;

using QRCoder;

using Saladpuk.PromptPay.Facades;

using System.Globalization;


using System.Net;
using PdfDocument = PdfiumViewer.PdfDocument;



namespace project2
{
    public partial class main : Form
    {
        public Panel Detailpanel
        {
            get { return detail_panel; }
        }
        public Label Detail_detail
        {
            get { return Detail_lbl; }
        }
        public Label Detail_id
        {
            get { return detail_idlbl; }
        }

        public Label Detail_name
        {
            get { return detail_productlbl; }
        }
        public Label Detail_price
        {
            get { return detailprice_lbl; }
        }

        public Label Detail_amount
        {
            get { return detailamount_lbl; }
        }

        public PictureBox Detail_image
        {
            get { return detail_picturebox; }
        }

        /* public Label Cart_totalamount
         {
             get { return Cart_totalAmountlbl; }
         }

         public Label Cart_totalprice
         {
             get { return Cart_totallbl; }
         }
        */

        public string email { get; set; }
        public string total { get; set; }
        private MySqlConnection databaseConnection()
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=project2;";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            return conn;
        }
        public main()
        {
            InitializeComponent();

        }


        private void main_Load(object sender, EventArgs e)
        {
            showstatus();
            showProfile();
            productitem();
            showcart();
            showamount();
            showtotalprice();


        }

        private void showProfile()
        {
            byte[] mimage = null;

            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM users WHERE email = @Email";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Email", email);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            pidlabel.Text = reader["id"].ToString();
                            Profile_email_label.Text = reader["email"].ToString();
                            Profile_tel_label.Text = reader["tel"].ToString();
                            Profile_address_label.Text = reader["address"].ToString();


                            Editemail_textb.Text = reader["email"].ToString();
                            Editpassword_textb.Text = reader["password"].ToString();
                            Edittel_textb.Text = reader["tel"].ToString();
                            Edit_addresstextb.Text = reader["address"].ToString();

                            mainemaillbl.Text = reader["email"].ToString();
                            if (!reader.IsDBNull(reader.GetOrdinal("image")))
                            {
                                mimage = (byte[])reader["image"];
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Error loading profile: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

            if (mimage != null)
            {
                using (MemoryStream ms = new MemoryStream(mimage))
                {
                    System.Drawing.Image userImage = System.Drawing.Image.FromStream(ms);

                    Profile_picture.Image = userImage;
                    Edit_imageprofile.Image = userImage;
                    mainProfilepictureBox.Image = userImage;
                }
            }
            else
            {

                Profile_picture.Image = null;
                Edit_imageprofile.Image = null;
            }
        }

        private void productitem()
        {
            ProductflowLayoutPanel.Controls.Clear();
            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();
                    string csql = "SELECT id,product,price,image FROM product";
                    MySqlCommand ccmd = new MySqlCommand(csql, conn);




                    using (MySqlDataReader reader = ccmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            decimal price = reader.GetDecimal(reader.GetOrdinal("price"));

                            product prd = new product(this.detail_panel, this.Detail_id, this.Detail_lbl, this.detailprice_lbl, this.detailamount_lbl, this.detail_picturebox, this.detail_productlbl)
                            {
                                Id = reader["id"].ToString(),
                                Product = reader["product"].ToString(),

                                Price = price



                            };
                            if (!reader.IsDBNull(reader.GetOrdinal("image")))
                            {
                                byte[] imgBytes = (byte[])reader["image"];
                                using (MemoryStream ms = new MemoryStream(imgBytes))
                                {
                                    prd.Image = System.Drawing.Image.FromStream(ms);
                                }
                            }



                            ProductflowLayoutPanel.Controls.Add(prd);

                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
        }

        private void homebtn_Click(object sender, EventArgs e)
        {
            productitem();
        }





        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (!IsValidEmail(Editemail_textb.Text))
            {
                MessageBox.Show("โปรดใส่อีเมลในรูปแบบที่ถูกต้อง");
                return;
            }

            if (!IsValidPassword(Editpassword_textb.Text))
            {
                MessageBox.Show("รหัสผ่านต้องมีความยาวมากกว่า 8 ตัวอักษร และต้องมีตัวพิมพ์ใหญ่ ตัวพิมพ์เล็ก และสัญลักษณ์พิเศษ");
                return;
            }

            if (!IsValidPhoneNumber(Edittel_textb.Text))
            {
                MessageBox.Show("โปรดใส่หมายเลขโทรศัพท์ที่ถูกต้อง");
                return;
            }

            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();
                    string sql = "UPDATE users SET email=@Email, password=@Password, tel=@Tel, address=@Address, image=@Image WHERE id=@Id";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@Email", Editemail_textb.Text);
                    cmd.Parameters.AddWithValue("@Password", Editpassword_textb.Text);
                    cmd.Parameters.AddWithValue("@Tel", Edittel_textb.Text);
                    cmd.Parameters.AddWithValue("@Address", Edit_addresstextb.Text);
                    cmd.Parameters.AddWithValue("@Id", pidlabel.Text);

                    if (Edit_imageprofile.Image != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (Bitmap bmp = new Bitmap(Edit_imageprofile.Image))
                            {
                                bmp.Save(ms, Edit_imageprofile.Image.RawFormat);
                                byte[] imgBytes = ms.ToArray();
                                cmd.Parameters.AddWithValue("@Image", imgBytes);
                            }
                        }
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Image", DBNull.Value);
                    }

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("แก้ไขข้อมูลเรียบร้อยแล้ว");
                    showProfile();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving profile: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }


        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return !string.IsNullOrEmpty(email) && Regex.IsMatch(email, emailPattern);
        }
        private bool IsValidPassword(string password)
        {
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_]).{9,}$";
            return !string.IsNullOrEmpty(password) && Regex.IsMatch(password, passwordPattern);
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            string phonePattern = @"^\d{10}$";
            return !string.IsNullOrEmpty(phoneNumber) && Regex.IsMatch(phoneNumber, phonePattern);
        }

        private void Edit_imageprofile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.png;*.gif)|*.bmp;*.jpg;*.png;*.gif|All files (*.*)|*.*";
                openFileDialog.Title = "Select an image file";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Edit_imageprofile.Image = new Bitmap(openFileDialog.FileName);
                }
            }
        }

        private void guna2PictureBox3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("ออก", "ยืนยันการออก", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                loginandregister loginandregister = new loginandregister();
                loginandregister.Show();
                this.Close();
            }
        }

        private void Viewbtn_Click(object sender, EventArgs e)
        {
            Viewprofile_panel.BringToFront();
            if (Viewprofile_panel.Visible == false)
            {
                Viewprofile_panel.Visible = true;
            }
        }

        private void Settingbtn_Click(object sender, EventArgs e)
        {
            Editprofile_panel.BringToFront();
            if (Editprofile_panel.Visible == false)
            {
                Editprofile_panel.Visible = true;
            }
            else
            {
                Editprofile_panel.Visible = false;
            }
        }



        private void backBtn_Click_1(object sender, EventArgs e)
        {
            if (Editprofile_panel.Visible == true)
            {
                Editprofile_panel.Visible = false;
            }
        }

       

        private void detailbackBtn_Click(object sender, EventArgs e)
        {

            using (MySqlConnection conn = databaseConnection())
            {

            }



            if (detail_panel.Visible == true)
            {
                detail_panel.Visible = false;
            }
        }

        private void AddCartBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(detail_idlbl.Text) || string.IsNullOrEmpty(pidlabel.Text) || string.IsNullOrEmpty(detail_productlbl.Text))
            {
                MessageBox.Show("ข้อมูลสินค้าไม่ถูกต้อง");
                return;
            }

            using (MySqlConnection con = databaseConnection())
            {

                con.Open();


                string getStockQuery = "SELECT amount FROM product WHERE id = @productId";
                MySqlCommand getStockCmd = new MySqlCommand(getStockQuery, con);
                getStockCmd.Parameters.AddWithValue("@productId", detail_idlbl.Text);
                int currentStock = Convert.ToInt32(getStockCmd.ExecuteScalar());


                int amountToAdd = Convert.ToInt32(detailNumericUpDown1.Value);


                if (amountToAdd > currentStock)
                {
                    MessageBox.Show("จำนวนสินค้าที่ต้องการเกินกว่าสต็อกที่มีอยู่");
                    return;
                }
                if (amountToAdd <= 0)
                {
                    MessageBox.Show("เลือกจำนวนสินค้าใหม่");
                    return;
                }

                string checkExistenceQuery = "SELECT COUNT(*) FROM cart WHERE uid = @uid AND product = @product";
                MySqlCommand checkExistenceCmd = new MySqlCommand(checkExistenceQuery, con);
                checkExistenceCmd.Parameters.AddWithValue("@uid", pidlabel.Text);
                checkExistenceCmd.Parameters.AddWithValue("@product", detail_productlbl.Text);
                int count = Convert.ToInt32(checkExistenceCmd.ExecuteScalar());

                if (count > 0)
                {

                    string updateQuery = "UPDATE cart SET amount = amount + @amount WHERE uid = @uid AND product = @product";
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, con);
                    updateCmd.Parameters.AddWithValue("@uid", pidlabel.Text);
                    updateCmd.Parameters.AddWithValue("@product", detail_productlbl.Text);
                    updateCmd.Parameters.AddWithValue("@amount", amountToAdd);

                    int rowsAffected = updateCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {

                        string reduceStockQuery = "UPDATE product SET amount = amount - @amount WHERE id = @productId";
                        MySqlCommand reduceStockCmd = new MySqlCommand(reduceStockQuery, con);
                        reduceStockCmd.Parameters.AddWithValue("@amount", amountToAdd);
                        reduceStockCmd.Parameters.AddWithValue("@productId", detail_idlbl.Text);
                        reduceStockCmd.ExecuteNonQuery();


                        int newStock = currentStock - amountToAdd;
                        detailamount_lbl.Text = newStock.ToString();

                        MessageBox.Show($"เพิ่ม {amountToAdd} จำนวน {detail_productlbl.Text} ไปในตะกร้า");
                    }
                    else
                    {
                        MessageBox.Show("ไม่สามารถเพิ่มสินค้าในตะกร้าได้");
                    }
                }
                else
                {

                    string insertQuery = "INSERT INTO cart (uid, product, price, amount, image) VALUES (@uid, @product, @price, @amount, @image)";
                    MySqlCommand insertCmd = new MySqlCommand(insertQuery, con);
                    insertCmd.Parameters.AddWithValue("@uid", pidlabel.Text);
                    insertCmd.Parameters.AddWithValue("@product", detail_productlbl.Text);
                    insertCmd.Parameters.AddWithValue("@price", Convert.ToDecimal(detailprice_lbl.Text));
                    insertCmd.Parameters.AddWithValue("@amount", amountToAdd);


                    byte[] imageBytes;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (Bitmap bmp = new Bitmap(detail_picturebox.Image))
                        {
                            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        }
                        imageBytes = ms.ToArray();
                    }
                    insertCmd.Parameters.AddWithValue("@image", imageBytes);

                    int rowsAffected = insertCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {

                        string reduceStockQuery = "UPDATE product SET amount = amount - @amount WHERE id = @productId";
                        MySqlCommand reduceStockCmd = new MySqlCommand(reduceStockQuery, con);
                        reduceStockCmd.Parameters.AddWithValue("@amount", amountToAdd);
                        reduceStockCmd.Parameters.AddWithValue("@productId", detail_idlbl.Text);
                        reduceStockCmd.ExecuteNonQuery();


                        int newStock = currentStock - amountToAdd;
                        detailamount_lbl.Text = newStock.ToString();

                        MessageBox.Show($"เพิ่ม {amountToAdd} จำนวน {detail_productlbl.Text} ไปในตะกร้า.");
                    }
                    else
                    {
                        MessageBox.Show("ไม่สามารถเพิ่มสินค้าในตะกร้าได้");
                    }
                }


                detailNumericUpDown1.Value = 1;


            }
        }




        private System.Drawing.Image ByteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                return System.Drawing.Image.FromStream(ms);
            }
        }

        private void mainsearchTextb_TextChanged(object sender, EventArgs e)
        {
            string searchText = mainsearchTextb.Text.Trim();

            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT id, product, price, image FROM product WHERE product LIKE @searchText OR brand LIKE @searchText";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        ProductflowLayoutPanel.Controls.Clear();

                        while (reader.Read())
                        {
                            decimal price = reader.GetDecimal(reader.GetOrdinal("price"));
                            product prd = new product(this.detail_panel, this.Detail_id, this.Detail_lbl, this.detailprice_lbl, this.detailamount_lbl, this.detail_picturebox, this.detail_productlbl)
                            {
                                Id = reader["id"].ToString(),
                                Product = reader["product"].ToString(),
                                Price = price
                            };

                            if (!reader.IsDBNull(reader.GetOrdinal("image")))
                            {
                                byte[] imgBytes = (byte[])reader["image"];
                                using (MemoryStream ms = new MemoryStream(imgBytes))
                                {
                                    prd.Image = System.Drawing.Image.FromStream(ms);
                                }
                            }

                            ProductflowLayoutPanel.Controls.Add(prd);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void showcart()
        {
            cartflowLayoutPanel.Controls.Clear();
            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT id,product,price,amount,image FROM cart where uid=@Uid";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Uid", pidlabel.Text);



                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            decimal price = reader.GetDecimal(reader.GetOrdinal("price"));
                            cartitem crt = new cartitem
                            {
                                Id = reader["id"].ToString(),
                                Product = reader["product"].ToString(),
                                Amount = Convert.ToInt32(reader["amount"]),
                                Price = price,

                                uid = pidlabel.Text,

                            };
                            crt.RequestShowAmount += Cartitem_RequestShowAmount;
                            crt.ResquestShowtotalprice += Cartitem_RequestShowtotalprice;
                            if (!reader.IsDBNull(reader.GetOrdinal("image")))
                            {
                                byte[] imgBytes = (byte[])reader["image"];
                                using (MemoryStream ms = new MemoryStream(imgBytes))
                                {
                                    crt.Image = System.Drawing.Image.FromStream(ms);
                                }
                            }



                            cartflowLayoutPanel.Controls.Add(crt);

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void showstatus()
        {
            statusflowLayoutPanel.Controls.Clear();

            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT order_no, idu, pdfrecipt, slippayment, status,comment FROM checkpayment WHERE idu = @idu";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@idu", pidlabel.Text);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            status statusControl = new status
                            {
                                Orderno = reader["order_no"].ToString(),
                                Status = reader["status"].ToString(),
                                Comment =reader["comment"].ToString(),
                            };

                            if (!reader.IsDBNull(reader.GetOrdinal("slippayment")))
                            {
                                byte[] imgBytes = (byte[])reader["slippayment"];
                                using (MemoryStream ms = new MemoryStream(imgBytes))
                                {
                                    statusControl.Slip = System.Drawing.Image.FromStream(ms);
                                }
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("pdfrecipt")))
                            {
                                byte[] pdfBytes = (byte[])reader["pdfrecipt"];
                                statusControl.Pdfrecipt = pdfBytes;
                            }

                            statusflowLayoutPanel.Controls.Add(statusControl);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    
                }
            }
        }

        

        private void Cartitem_RequestShowAmount(object sender, EventArgs e)
        {
            showamount();
        }

        private void Cartitem_RequestShowtotalprice(object sender, EventArgs e)
        {
            showtotalprice();
        }
        private void showamount()
        {
            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();
                    string sql = "select SUM(amount) as totalAmount from cart where uid=@Uid";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Uid", pidlabel.Text);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            object totalAmount = reader["totalAmount"];
                            if (totalAmount != DBNull.Value)
                            {
                                Cart_totalAmountlbl.Text = totalAmount.ToString();
                            }
                            else
                            {
                                Cart_totalAmountlbl.Text = "0";
                            }
                        }
                    }
                }


                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void showtotalprice()
        {
            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();
                    
                    string sql = "SELECT price, amount FROM cart WHERE uid=@Uid";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Uid", pidlabel.Text);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        decimal totalPrice = 0;

                        
                        while (reader.Read())
                        {
                            decimal price = reader.GetDecimal(reader.GetOrdinal("price"));
                            int amount = reader.GetInt32(reader.GetOrdinal("amount"));
                            totalPrice += price * amount;
                        }

                        
                        decimal vat = totalPrice * 0.07m; 
                        decimal totalPriceWithVat = totalPrice + vat;

                        
                        Cart_totallbl.Text = totalPriceWithVat.ToString("C2"); 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        private void homeBtn_Click_1(object sender, EventArgs e)
        {
            mainsearchTextb.Clear();
            productitem();

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            cart_panel.BringToFront();

            if (cart_panel.Visible == false)
            {

                showcart();
                showamount();
                showtotalprice();

                cart_panel.Visible = true;
            }
        }

        private void cartbackbtn_Click(object sender, EventArgs e)
        {
            if (cart_panel.Visible == true)
            {
                cart_panel.Visible = false;
            }
        }

        private class CartItem
        {
            public string Product { get; set; }
            public decimal Price { get; set; }
            public int Amount { get; set; }
        }
        private void payBtn_Click(object sender, EventArgs e)
        {
            if (qr_panel.Visible == false)
            {
                qr_panel.Visible = true;
            }

            bool addressIsNull = false;

            
            using (MySqlConnection conn = databaseConnection())
            {
                conn.Open();

                string selectAddressSql = "SELECT address FROM users WHERE id=@UserId AND address IS NULL";
                MySqlCommand selectAddressCmd = new MySqlCommand(selectAddressSql, conn);
                selectAddressCmd.Parameters.AddWithValue("@UserId", pidlabel.Text);

                object addressResult = selectAddressCmd.ExecuteScalar();
                if (addressResult != null && addressResult == DBNull.Value)
                {
                    addressIsNull = true;
                }
            }

           
            if (addressIsNull)
            {
                MessageBox.Show("กรุณาอัพเดทที่อยู่ของคุณก่อนดำเนินการสั่งซื้อ.");
                return;
            }
            string qrData;
            try
            {
                
                decimal totalAmount = decimal.Parse(Cart_totallbl.Text, NumberStyles.Currency);

                
                qrData = PPay.StaticQR.MobileNumber("0610453230").Amount((double)totalAmount).CreateCreditTransferQrCode();
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid total amount format.");
                return;
            }

            
            QRCoder.QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrData, QRCoder.QRCodeGenerator.ECCLevel.H);
            QRCoder.QRCode qrCode = new QRCoder.QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(2);

            
            qrcodepicturebox.Image = qrCodeImage;

            
            pdf();
            SendEmailWithAttachment();
        }

        private void backqrBtn_Click(object sender, EventArgs e)
        {
            if (qr_panel.Visible == true)
            {
                qr_panel.Visible = false;
            }


        }

        private void pdf()
        {
            

            
            string saveDirectory = @"C:\Users\kanna\Desktop\project\project2\pdfrecipt";
            string saveFileName = "receipt.pdf";
            string filePath = Path.Combine(saveDirectory, saveFileName);

            try
            {
                using (MySqlConnection conn = databaseConnection())
                {
                    conn.Open();

                    // Retrieve all items in the cart for the current user
                    string selectSql = "SELECT product, price, amount FROM cart WHERE uid=@Uid";
                    MySqlCommand selectCmd = new MySqlCommand(selectSql, conn);
                    selectCmd.Parameters.AddWithValue("@Uid", pidlabel.Text);
                    List<CartItem> cartItems = new List<CartItem>();

                    using (MySqlDataReader reader = selectCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cartItems.Add(new CartItem
                            {
                                Product = reader["product"].ToString(),
                                Price = reader.GetDecimal("price"),
                                Amount = reader.GetInt32("amount")
                            });
                        }
                    }

                   

                    DateTime currentTime = DateTime.Now;

                    // Calculate total price and VAT
                    decimal totalPrice = 0;
                    decimal vatRate = 0.07m; // 7% VAT rate
                    decimal totalVAT = 0;

                    // Create a document for the receipt in memory
                    Document document = new Document();
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                    document.Open();

                    string fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fonts", "THSarabunNew.ttf");
                    BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    iTextSharp.text.Font thaiFont = new iTextSharp.text.Font(baseFont, 16);

                    // Add title and date to the document
                    document.Add(new Paragraph("                                                                      Mango Shop", thaiFont));
                    document.Add(new Paragraph("ใบรายการสั่งซื้อ", thaiFont));
                    document.Add(new Paragraph("เวลา: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), thaiFont));
                    document.Add(new Paragraph(" ", thaiFont)); // Empty line

                    // Add table for the receipt with adjusted column widths
                    PdfPTable table = new PdfPTable(4);
                    float[] columnWidths = new float[] { 5f, 2f, 2f, 2f };
                    table.SetWidths(columnWidths);
                    table.AddCell(new PdfPCell(new Phrase("สินค้า", thaiFont)));
                    table.AddCell(new PdfPCell(new Phrase("ราคา", thaiFont)));
                    table.AddCell(new PdfPCell(new Phrase("จำนวน", thaiFont)));
                    table.AddCell(new PdfPCell(new Phrase("ราคารวม", thaiFont)));

                    // Loop through each item in the cart and insert into history
                    foreach (var item in cartItems)
                    {
                        // Add row to the PDF table
                        table.AddCell(new PdfPCell(new Phrase(item.Product, thaiFont)));
                        table.AddCell(new PdfPCell(new Phrase(item.Price.ToString("C2"), thaiFont)));
                        table.AddCell(new PdfPCell(new Phrase(item.Amount.ToString(), thaiFont)));
                        table.AddCell(new PdfPCell(new Phrase((item.Price * item.Amount).ToString("C2"), thaiFont)));

                        totalPrice += item.Price * item.Amount;
                    }

                    // Calculate VAT
                    totalVAT = totalPrice * vatRate;

                    // Add VAT row to the PDF table
                    table.AddCell(new PdfPCell(new Phrase("", thaiFont)));
                    table.AddCell(new PdfPCell(new Phrase("", thaiFont)));
                    table.AddCell(new PdfPCell(new Phrase("VAT (7%)", thaiFont)));
                    table.AddCell(new PdfPCell(new Phrase(totalVAT.ToString("C2"), thaiFont)));

                    // Add total price row
                    table.AddCell(new PdfPCell(new Phrase("", thaiFont)));
                    table.AddCell(new PdfPCell(new Phrase("", thaiFont)));
                    table.AddCell(new PdfPCell(new Phrase("ราคารวมทั้งสิ้น", thaiFont)));
                    table.AddCell(new PdfPCell(new Phrase((totalPrice + totalVAT).ToString("C2"), thaiFont)));

                    document.Add(table);

                    
                    string qrData = PPay.StaticQR.MobileNumber("0610453230").Amount((double)(totalPrice + totalVAT)).CreateCreditTransferQrCode();

                    QRCoder.QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrData, QRCoder.QRCodeGenerator.ECCLevel.H);
                    QRCoder.QRCode qrCode = new QRCoder.QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(2);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        iTextSharp.text.Image qrCodeImagePdf = iTextSharp.text.Image.GetInstance(ms.ToArray());
                        qrCodeImagePdf.SetAbsolutePosition(30f, 180f); // Adjust position as needed
                        document.Add(qrCodeImagePdf);
                    }

                    // Add user's address and email to the document
                    document.Add(new Paragraph(" ", thaiFont));
                    document.Add(new Paragraph($"ที่อยู่:  {Profile_address_label.Text}", thaiFont));
                    document.Add(new Paragraph($"โทรศัพท์:  {Profile_tel_label.Text}", thaiFont));
                    document.Add(new Paragraph($"อีเมล:  {Profile_email_label.Text}", thaiFont));

                    document.Add(new Paragraph(" ", thaiFont));
                    document.Add(new Paragraph("ธนาคาร กรุงไทย: 678-5830-435", thaiFont));
                    document.Add(new Paragraph("กัณวัฒน์ พิสาดรัมย์", thaiFont));
                    document.Add(new Paragraph(" ", thaiFont));
                    document.Add(new Paragraph("      พร้อมเพย์ QR", thaiFont));

                    document.Close();

                    

                    showcart();
                    showamount();
                    showtotalprice();

                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
       

        private void backreciptBtn_Click(object sender, EventArgs e)
        {
            if (recipt_panel.Visible == true)
            {
                recipt_panel.Visible = false;
            }
        }

        

        private void reciptPictureBox2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.png;*.gif)|*.bmp;*.jpg;*.png;*.gif|All files (*.*)|*.*";
                openFileDialog.Title = "Select an image file";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    slipPaymentPictureBox.Image = new Bitmap(openFileDialog.FileName);
                }
            }
        }

        private void nextqrBtn_Click(object sender, EventArgs e)
        {
            if (recipt_panel.Visible == false)
            {
                recipt_panel.Visible = true;
            }
        }

        private void browsePdfreciptBtn_Click(object sender, EventArgs e)
        {
            
        }

        private void SubmitBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(pdffilename.Text))
            {
                MessageBox.Show("โปรดใส่ข้อมูล PDF ใบเสร็จเพื่อไว้สำหรับตรวจสอบ");
                return;
            }

            if (slipPaymentPictureBox.Image == null)
            {
                MessageBox.Show("โปรดใส่ slip ใบเสร็จเพื่อไว้สำหรับตรวจสอบ");
                return;
            }
            
                using (MySqlConnection conn = databaseConnection())
                {
                    conn.Open();

                    
                    string selectSql = "SELECT product, price, amount FROM cart WHERE uid=@Uid";
                    MySqlCommand selectCmd = new MySqlCommand(selectSql, conn);
                    selectCmd.Parameters.AddWithValue("@Uid", pidlabel.Text);

                    List<CartItem> cartItems = new List<CartItem>();

                    using (MySqlDataReader reader = selectCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cartItems.Add(new CartItem
                            {
                                Product = reader["product"].ToString(),
                                Price = reader.GetDecimal("price"),
                                Amount = reader.GetInt32("amount")
                            });
                        }
                    }

                    
                    string insertSql = "INSERT INTO history(idu, product, price, amount, time) VALUES(@idu, @product, @price, @amount, @time)";
                    MySqlCommand insertCmd = new MySqlCommand(insertSql, conn);

                    DateTime currentTime = DateTime.Now;

                    
                    foreach (var item in cartItems)
                    {
                        insertCmd.Parameters.Clear();
                        insertCmd.Parameters.AddWithValue("@idu", pidlabel.Text);
                        insertCmd.Parameters.AddWithValue("@product", item.Product);
                        insertCmd.Parameters.AddWithValue("@price", item.Price);
                        insertCmd.Parameters.AddWithValue("@amount", item.Amount);
                        insertCmd.Parameters.AddWithValue("@time", currentTime);

                        insertCmd.ExecuteNonQuery();
                    }

                    
                    string deleteSql = "DELETE FROM cart WHERE uid=@Uid";
                    MySqlCommand deleteCmd = new MySqlCommand(deleteSql, conn);
                    deleteCmd.Parameters.AddWithValue("@Uid", pidlabel.Text);
                    deleteCmd.ExecuteNonQuery();

                    
                    showcart();
                    showamount();
                    showtotalprice();
                    sendpayment();



                }
            
            
                
            
        }

        private void sendpayment()
        {
            string status = "In progress";
            using (MySqlConnection conn = databaseConnection())
            {
                conn.Open();

                string insertCheckPaymentSql = "INSERT INTO checkpayment (idu, pdfrecipt, slippayment,status) VALUES (@idu,@pdfrecipt, @slippayment,@status)";
                MySqlCommand insertCheckPaymentCmd = new MySqlCommand(insertCheckPaymentSql, conn);
                insertCheckPaymentCmd.Parameters.AddWithValue("@idu", pidlabel.Text);
                insertCheckPaymentCmd.Parameters.AddWithValue("@status", status);

                string pdfFilePath = pdffilename.Text;
                if (File.Exists(pdfFilePath))
                {
                    byte[] pdfBytes = File.ReadAllBytes(pdfFilePath);
                    insertCheckPaymentCmd.Parameters.AddWithValue("@pdfrecipt", pdfBytes);
                }
                else
                {
                    MessageBox.Show("No valid PDF file selected.");
                    return;
                }

                if (slipPaymentPictureBox.Image != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        slipPaymentPictureBox.Image.Save(ms, slipPaymentPictureBox.Image.RawFormat);
                        byte[] imgBytes = ms.ToArray();
                        insertCheckPaymentCmd.Parameters.AddWithValue("@slippayment", imgBytes);
                    }
                }
                else
                {
                    insertCheckPaymentCmd.Parameters.AddWithValue("@slippayment", DBNull.Value);
                }

                insertCheckPaymentCmd.ExecuteNonQuery();

                
                pdffilename.Controls.Clear();
                slipPaymentPictureBox.Image = null;
                qr_panel.Visible = false;
                MessageBox.Show("บันทึกรายการสั่งซื้อและใบเสร็จเสร็จสิ้น");
            }

        }


        private iTextSharp.text.Image ConvertImageToITextImage(System.Drawing.Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png); 
                return iTextSharp.text.Image.GetInstance(ms.ToArray());
            }
        }

        private void SendEmailWithAttachment()
        {
            string attachmentPath = @"C:\Users\kanna\Desktop\project\project2\pdfrecipt\receipt.pdf";
            string fromAddress = "iceza55yoo@gmail.com";
            string recipientEmail = "";

            using (MySqlConnection connmail = databaseConnection())
            {
                connmail.Open();
                MySqlCommand commandmail = new MySqlCommand("SELECT email FROM users WHERE email = @Email", connmail);
                commandmail.Parameters.AddWithValue("@Email", Profile_email_label.Text);

                using (MySqlDataReader reader = commandmail.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        recipientEmail = reader.GetString("email");
                    }
                    else
                    {
                        MessageBox.Show("Email not found for the specified seller.");
                        return;
                    }
                }
            }

            string toAddress = recipientEmail;
            string subject = "Receipt Notification";
            string body = "Thank you for your purchase! Attached is your receipt for the transaction.";

            using (MailMessage mail = new MailMessage(fromAddress, toAddress, subject, body))
            {
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential("iceza55yoo@gmail.com", "qffn xbcg zzor qmaw");
                    smtpClient.EnableSsl = true;

                    try
                    {
                        
                        if (File.Exists(attachmentPath))
                        {
                            Attachment attachment = new Attachment(attachmentPath);
                            mail.Attachments.Add(attachment);
                        }
                        else
                        {
                            MessageBox.Show("Attachment file not found: " + attachmentPath);
                            return;
                        }

                        smtpClient.Send(mail);
                        MessageBox.Show("Email sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to send email: " + ex.Message);
                    }
                }
            }
        }

        private void closeprofile_panel_Click(object sender, EventArgs e)
        {
            if (Viewprofile_panel.Visible == true)
            {
                Viewprofile_panel.Visible = false;
            }
        }

       

        private void StatusBtn_Click(object sender, EventArgs e)
        {
            if (status_panel.Visible == false) {
                showstatus();
                status_panel.Visible = true;
            }
        }

        private void back_status_Click(object sender, EventArgs e)
        {
            if (status_panel.Visible == true) { 
                status_panel.Visible=false;
            }
        }

        private void BrowsePdfBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                pdffilename.Text = filePath;

                
                
            }
        }

        
    }
}


