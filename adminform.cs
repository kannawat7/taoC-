using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PdfiumViewer;
using AxAcroPDFLib;

namespace project2
{
    public partial class adminform : Form
    {

        private MySqlConnection databaseConnection()
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=project2;";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            return conn;
        }
        public adminform()
        {
            InitializeComponent();
        }

        private void adminform_Load(object sender, EventArgs e)
        {
            checkpayment();
            showproduct();
            showusers();
            showhistory();
            PopulateDayComboBox();
            PopulateMonthComboBox();
            PopulateYearComboBox();
            PopulateMonthComboBox2();
            PopulateYearComboBox2();
            um_passwordtextb.UseSystemPasswordChar = true;
            umpasswordCheckBox1.CheckedChanged += umpasswordCheckBox1_CheckedChanged;
        }
        private void showproduct()
        {
            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM product";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    Product_datagridview.DataSource = dt;
                    Product_datagridview.Columns["image"].Visible = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void showusers()
        {
            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM users";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    usersDataGridView.DataSource = dt;
                    usersDataGridView.Columns["image"].Visible = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void showhistory()
        {
            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();
                    string sql = @"SELECT users.email, history.product, history.price, history.amount, history.time
                           FROM history
                           INNER JOIN users ON history.idu = users.id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            HistoryDataGridView1.DataSource = dt;

                            
                            decimal totalPrice = 0;
                            int totalAmount = 0;

                            foreach (DataRow row in dt.Rows)
                            {
                                decimal price = Convert.ToDecimal(row["price"]);
                                int amount = Convert.ToInt32(row["amount"]);
                                totalPrice += price * amount;
                                totalAmount += amount;
                            }

                            
                            History_Totallbl.Text = $"{totalPrice:C}";
                            History_Amountlbl.Text = $"{totalAmount}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }
        private void Productimage_picturebox_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.png;*.gif)|*.bmp;*.jpg;*.png;*.gif|All files (*.*)|*.*";
                openFileDialog.Title = "Select an image file";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Productimage_picturebox.Image = new Bitmap(openFileDialog.FileName);
                }
            }
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            string product = Product_textb.Text;
            string price = Price_textb.Text;
            string amount = Amount_textb.Text;
            string brand = Brand_ComboBox.Text;
            string detail = Detail_textb.Text;

            if (string.IsNullOrEmpty(product) || string.IsNullOrEmpty(price) || string.IsNullOrEmpty(amount) || string.IsNullOrEmpty(brand))
            {
                MessageBox.Show("โปรดใส่ข้อมูลให้ครบ");
                return;
            }

            if (!float.TryParse(price, out float flaotprice) || flaotprice < 0)
            {
                MessageBox.Show("ราคาต้องเป็นตัวเลขที่มากกว่าหรือเท่ากับ 0");
                return;
            }

            if (!int.TryParse(amount, out int intamount) || intamount < 0)
            {
                MessageBox.Show("จำนวนต้องเป็นตัวเลขที่มากกว่าหรือเท่ากับ 0");
                return;
            }

            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();
                    string sql = "INSERT INTO product(product, price, amount, brand, detail, image) VALUES (@product, @price, @amount, @brand, @detail, @image)";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@product", product);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@brand", brand);
                    cmd.Parameters.AddWithValue("@detail", detail);

                    if (Productimage_picturebox.Image != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (Bitmap bmp = new Bitmap(Productimage_picturebox.Image))
                            {
                                bmp.Save(ms, Productimage_picturebox.Image.RawFormat);
                                byte[] imgBytes = ms.ToArray();
                                cmd.Parameters.AddWithValue("@image", imgBytes);
                            }
                        }
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@image", DBNull.Value);
                    }

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("เพิ่มข้อมูลเรียบร้อย");
                    Productimage_picturebox.Image = null;
                    Product_textb.Clear();
                    Price_textb.Clear();
                    Amount_textb.Clear();
                    Brand_ComboBox.SelectedIndex = -1;
                    Detail_textb.Clear();
                    showproduct();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("add" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void Product_datagridview_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < Product_datagridview.Rows.Count)
            {
                try
                {
                    Product_datagridview.Rows[e.RowIndex].Selected = true;

                    Product_textb.Text = Product_datagridview.Rows[e.RowIndex].Cells["product"].FormattedValue.ToString();
                    Price_textb.Text = Product_datagridview.Rows[e.RowIndex].Cells["price"].FormattedValue.ToString();
                    Amount_textb.Text = Product_datagridview.Rows[e.RowIndex].Cells["amount"].FormattedValue.ToString();
                    Brand_ComboBox.Text = Product_datagridview.Rows[e.RowIndex].Cells["brand"].FormattedValue.ToString();
                    Detail_textb.Text = Product_datagridview.Rows[e.RowIndex].Cells["detail"].FormattedValue.ToString();

                    var imageCellValue = Product_datagridview.Rows[e.RowIndex].Cells["image"].Value;
                    if (imageCellValue != DBNull.Value && imageCellValue is byte[] imageData && imageData.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            using (Bitmap bmp = new Bitmap(ms))
                            {
                                Productimage_picturebox.Image = new Bitmap(bmp);
                            }
                        }
                    }
                    else
                    {
                        Productimage_picturebox.Image = null;
                    }
                   

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("กรุณาเลือกแถวที่ถูกต้อง");
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (Product_datagridview.CurrentCell != null)
            {
                int selectedrow = Product_datagridview.CurrentCell.RowIndex;
                string id = Product_datagridview.Rows[selectedrow].Cells["id"].Value.ToString();

                if (!float.TryParse(Price_textb.Text, out float price) || price < 0)
                {
                    MessageBox.Show("ราคาต้องเป็นตัวเลขที่มากกว่าหรือเท่ากับ 0");
                    return;
                }

                if (!int.TryParse(Amount_textb.Text, out int amount) || amount < 0)
                {
                    MessageBox.Show("จำนวนต้องเป็นตัวเลขที่มากกว่าหรือเท่ากับ 0");
                    return;
                }

                using (MySqlConnection conn = databaseConnection())
                {

                    conn.Open();
                    string sql = "UPDATE product SET product=@product, price=@price, amount=@amount, brand=@brand, detail=@detail, image=@image WHERE id=@id";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@product", Product_textb.Text);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@brand", Brand_ComboBox.Text);
                    cmd.Parameters.AddWithValue("@detail", Detail_textb.Text);
                    cmd.Parameters.AddWithValue("@id", id);

                    if (Productimage_picturebox.Image != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            
                            Bitmap bmp = new Bitmap(Productimage_picturebox.Image);
                            
                            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            byte[] imgBytes = ms.ToArray();
                            cmd.Parameters.AddWithValue("@image", imgBytes);
                        }
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@image", DBNull.Value);
                    }

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("แก้ไขข้อมูลเรียบร้อยแล้ว");
                    showproduct();


                }
            }
            else
            {
                MessageBox.Show("กรุณาเลือกสินค้าที่ต้องการแก้ไข");
            }
        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            if (Product_datagridview.CurrentCell != null)
            {
                int selectedrow = Product_datagridview.CurrentCell.RowIndex;
                string id = Product_datagridview.Rows[selectedrow].Cells["id"].Value.ToString();

                DialogResult result = MessageBox.Show("คุณต้องการลบสินค้าหรือไม่", "ลบข้อมูล", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    using (MySqlConnection conn = databaseConnection())
                    {
                        try
                        {
                            conn.Open();
                            string sql = "DELETE FROM product WHERE id = @id";
                            MySqlCommand cmd = new MySqlCommand(sql, conn);
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("ลบข้อมูลเรียบร้อยแล้ว");
                            showproduct();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("del" + ex.Message);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("กรุณาเลือกสินค้าที่ต้องการลบ");
            }
        }

        private void closeAdmin_panel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("ออก", "ยืนยันการออก", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                loginandregister loginandregister = new loginandregister();
                loginandregister.Show();
                this.Close();
            }

        }

        private void usersDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < usersDataGridView.Rows.Count)
            {
                try
                {
                    usersDataGridView.Rows[e.RowIndex].Selected = true;

                    um_emailtextb.Text = usersDataGridView.Rows[e.RowIndex].Cells["email"].FormattedValue.ToString();
                    um_passwordtextb.Text = usersDataGridView.Rows[e.RowIndex].Cells["password"].FormattedValue.ToString();
                    um_teltexb.Text = usersDataGridView.Rows[e.RowIndex].Cells["tel"].FormattedValue.ToString();
                    um_adresstextb.Text = usersDataGridView.Rows[e.RowIndex].Cells["address"].FormattedValue.ToString();

                    var imageCellValue = usersDataGridView.Rows[e.RowIndex].Cells["image"].Value;
                    if (imageCellValue != DBNull.Value && imageCellValue is byte[] imageData && imageData.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            using (Bitmap bmp = new Bitmap(ms))
                            {
                                um_picturebox.Image = new Bitmap(bmp);
                            }
                        }
                    }
                    else
                    {
                        um_picturebox.Image = null;
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("กรุณาเลือกแถวที่ถูกต้อง");
            }
        }

        private void usermanagementBtn_Click(object sender, EventArgs e)
        {
            if (usermanagement_panel.Visible == false) {
                usermanagement_panel.Visible = true;
               
            }

        }

        private void productmanagementBtn_Click(object sender, EventArgs e)
        {
            if (usermanagement_panel.Visible == true )
            {
                usermanagement_panel.Visible = false;
                
            }
        }

        private void historyBtn_Click(object sender, EventArgs e)
        {
            if (history_panel.Visible == false)
            {

            history_panel.Visible = true;
            }
            
        
        }

        private void umpasswordCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            um_passwordtextb.UseSystemPasswordChar =!umpasswordCheckBox1.Checked;
        }

        private void umDelBtn_Click(object sender, EventArgs e)
        {
            if (usersDataGridView.CurrentCell != null)
            {
                int selectedrow = usersDataGridView.CurrentCell.RowIndex;
                string id = usersDataGridView.Rows[selectedrow].Cells["id"].Value.ToString();

                DialogResult result = MessageBox.Show("คุณต้องการลบสินค้าหรือไม่", "ลบข้อมูล", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    using (MySqlConnection conn = databaseConnection())
                    {
                        try
                        {
                            conn.Open();
                            string sql = "DELETE FROM users WHERE id = @id";
                            MySqlCommand cmd = new MySqlCommand(sql, conn);
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("ลบข้อมูลเรียบร้อยแล้ว");
                            showusers();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("กรุณาเลือกสินค้าที่ต้องการลบ");
            }
        }

        private void Search_textb_TextChanged(object sender, EventArgs e)
        {
            string searchText = Search_textb.Text.Trim();

            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM product WHERE product LIKE @searchText OR brand LIKE @searchText";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    Product_datagridview.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void refreshbtn_Click(object sender, EventArgs e)
        {
            showproduct();
            Search_textb.Clear();
        }

        private void umsearchtextb_TextChanged(object sender, EventArgs e)
        {
            string searchText = umsearchtextb.Text.Trim();

            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM users WHERE email LIKE @searchText OR tel LIKE @searchText";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    usersDataGridView.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void umRefreshBtn_Click(object sender, EventArgs e)
        {
            showusers();
            umsearchtextb.Clear();
        }

       

        private void umcloseBtn_Click(object sender, EventArgs e)
        {
            if (usermanagement_panel.Visible == true)
            {
                usermanagement_panel.Visible = false;

            }
        }

        private void closehistoryBtn_Click_1(object sender, EventArgs e)
        {
            if (history_panel.Visible==true) { 
                history_panel.Visible = false;
            
            }
        }


        private void PopulateDayComboBox()
        {
            for (int day = 1; day <= 32; day++)
            {
                dayComboBox1.Items.Add(day);
            }
            dayComboBox1.SelectedIndex = 0; 
        }

        private void PopulateMonthComboBox()
        {
            for (int month = 1; month <= 12; month++)
            {
                monthComboBox1.Items.Add(month);
            }
            monthComboBox1.SelectedIndex = 0;
        }

        private void PopulateYearComboBox()
        {
            for (int year = 2024; year <= 2050; year++)
            {
                yearComboBox1.Items.Add(year);
            }
            yearComboBox1.SelectedIndex = 0; 
        }

        private void PopulateMonthComboBox2()
        {
            for (int month = 1; month <= 12; month++)
            {
                monthComboBox2.Items.Add(month);
            }
            monthComboBox2.SelectedIndex = 0;
        }

        private void PopulateYearComboBox2()
        {
            for (int year = 2024; year <= 2050; year++)
            {
                yearComboBox2.Items.Add(year);
            }
            yearComboBox2.SelectedIndex = 0;
        }

        

       
        private void historyRefreshBtn_Click(object sender, EventArgs e)
        {
            showhistory();
            usersearrchTextbox.Controls.Clear();
        }

       
        private void DaysearchBtn_Click(object sender, EventArgs e)
        {
            int selectedDay = (int)dayComboBox1.SelectedItem;
            int selectedMonth = (int)monthComboBox1.SelectedItem;
            int selectedYear = (int)yearComboBox1.SelectedItem;

            string selectedDate = new DateTime(selectedYear, selectedMonth, selectedDay).ToString("yyyy-MM-dd");

            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();
                    string sql = @"SELECT users.email, history.product, history.price, history.amount, history.time
                           FROM history
                           INNER JOIN users ON history.idu = users.id
                           WHERE DATE(history.time) = @selectedDate";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@selectedDate", selectedDate);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            HistoryDataGridView1.DataSource = dt;


                            decimal totalPrice = 0;
                            int totalAmount = 0;

                            foreach (DataRow row in dt.Rows)
                            {
                                decimal price = Convert.ToDecimal(row["price"]);
                                int amount = Convert.ToInt32(row["amount"]);
                                totalPrice += price * amount;
                                totalAmount += amount;
                            }


                            History_Totallbl.Text = $"{totalPrice:C}";
                            History_Amountlbl.Text = $"{totalAmount}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }
        private void MonthSearchhistory_Click(object sender, EventArgs e)
        {
            int selectedMonth = (int)monthComboBox2.SelectedItem;
            int selectedYear = (int)yearComboBox2.SelectedItem;

            string sql = @"SELECT users.email, history.product, history.price, history.amount, history.time
                   FROM history
                   INNER JOIN users ON history.idu = users.id
                   WHERE YEAR(history.time) = @selectedYear AND MONTH(history.time) = @selectedMonth";

            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@selectedYear", selectedYear);
                        cmd.Parameters.AddWithValue("@selectedMonth", selectedMonth);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            HistoryDataGridView1.DataSource = dt;

                            
                            decimal totalPrice = 0;
                            int totalAmount = 0;

                            foreach (DataRow row in dt.Rows)
                            {
                                decimal price = Convert.ToDecimal(row["price"]);
                                int amount = Convert.ToInt32(row["amount"]);
                                totalPrice += price * amount;
                                totalAmount += amount;
                            }

                            
                            History_Totallbl.Text = $"{totalPrice:C}";
                            History_Amountlbl.Text = $"{totalAmount}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void checkpayment()
        {
            using (MySqlConnection conn = databaseConnection())
            {
                conn.Open();
                string sql = "SELECT checkpayment.order_no, users.email, checkpayment.pdfrecipt, checkpayment.slippayment, checkpayment.status,checkpayment.comment " +
                             "FROM checkpayment " +
                             "INNER JOIN users ON checkpayment.idu = users.id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                
                checkpaymentDataGridView.DataSource = dt;

                
                checkpaymentDataGridView.Columns["pdfrecipt"].Visible = false;
                checkpaymentDataGridView.Columns["slippayment"].Visible = false;
                
                checkpaymentDataGridView.CellClick += checkpaymentDataGridView_CellClick;
            }
        }

        

        
        private void CheckpaymentBtn_Click(object sender, EventArgs e)
        {
            if (checkpayment_panel.Visible == false)
            {
                checkpayment_panel.Visible = true;
            }
        }

        private void closecheck_Click(object sender, EventArgs e)
        {
            if (checkpayment_panel.Visible == true) { 
                checkpayment_panel.Visible=false;
            }
        }

        private void checkpaymentDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < checkpaymentDataGridView.Rows.Count)
            {
                try
                {
                    DataGridViewRow selectedRow = checkpaymentDataGridView.Rows[e.RowIndex];
                    checkpaymentDataGridView.Rows[e.RowIndex].Selected = true;

                    orderidTextBox.Text = selectedRow.Cells["order_no"].FormattedValue.ToString();
                    emailTextBox.Text = selectedRow.Cells["email"].FormattedValue.ToString();
                    StatuscomboBox.Text = selectedRow.Cells["status"].FormattedValue.ToString();
                    Commenttextb.Text = selectedRow.Cells["comment"].FormattedValue.ToString();
                    var imageCellValue = selectedRow.Cells["slippayment"].Value;
                    if (imageCellValue != DBNull.Value && imageCellValue is byte[] imageData && imageData.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            slipCheckPictureBox.Image = Image.FromStream(ms); 
                        }
                    }
                    else
                    {
                        slipCheckPictureBox.Image = null; 
                    }

                    
                    var pdfData = (byte[])selectedRow.Cells["pdfrecipt"].Value;
                    if (pdfData != null && pdfData.Length > 0)
                    {
                        
                        LoadPDF(pdfData);
                        axAcroPDF1.Visible = true; 
                    }
                    else
                    {
                        axAcroPDF1.Visible = false; 
                        MessageBox.Show("No PDF available for this record.");
                    }
                }
                catch 
                {
                    MessageBox.Show("กรุณาเลือกแถวที่ถูกต้อง");
                }
            }
            else
            {
                MessageBox.Show("กรุณาเลือกแถวที่ถูกต้อง"); 
            }
        }

        private void LoadPDF(byte[] pdfData)
        {
            // Save PDF data to a temporary file
            string tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".pdf");
            File.WriteAllBytes(tempFilePath, pdfData);

            // Load PDF file into Adobe Acrobat Reader control
            axAcroPDF1.LoadFile(tempFilePath);
        }

        private void EditpaymentBtn_Click(object sender, EventArgs e)
        {
            
            if (checkpaymentDataGridView.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow selectedRow = checkpaymentDataGridView.SelectedRows[0];

                    
                    int orderNo = Convert.ToInt32(selectedRow.Cells["order_no"].Value);

                    
                    using (MySqlConnection conn = databaseConnection())
                    {
                        conn.Open();

                        
                        string updateStatusSql = "UPDATE checkpayment SET status = @status,	comment = @comment WHERE order_no = @orderNo";
                        MySqlCommand updateStatusCmd = new MySqlCommand(updateStatusSql, conn);
                        updateStatusCmd.Parameters.AddWithValue("@status", StatuscomboBox.Text);
                        updateStatusCmd.Parameters.AddWithValue("@comment", Commenttextb.Text);
                        updateStatusCmd.Parameters.AddWithValue("@orderNo", orderNo);
                        updateStatusCmd.ExecuteNonQuery();

                       
                        selectedRow.Cells["status"].Value = StatuscomboBox.Text;
                       // selectedRow.Cells["comment"].Value = Commenttextb.Text;
                        MessageBox.Show("สถานะอัพเดื เรียบร้อยเเล้ว.");
                        checkpayment();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("กรุณาเลือกแถวที่ต้องการแก้ไข"); 
            }
        }

        private void usersearrchTextbox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(usersearrchTextbox.Text))
            {
                
                
                showhistory();
                return;
            }

            
            string sql = @"SELECT users.email, history.product, history.price, history.amount, history.time
                   FROM history
                   INNER JOIN users ON history.idu = users.id
                   WHERE users.email LIKE @user";

            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        
                        cmd.Parameters.AddWithValue("@user", usersearrchTextbox.Text + "%");

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            HistoryDataGridView1.DataSource = dt;

                            
                            decimal totalPrice = 0;
                            int totalAmount = 0;

                            foreach (DataRow row in dt.Rows)
                            {
                                if (row["price"] != DBNull.Value && row["amount"] != DBNull.Value)
                                {
                                    decimal price = Convert.ToDecimal(row["price"]);
                                    int amount = Convert.ToInt32(row["amount"]);
                                    totalPrice += price * amount;
                                    totalAmount += amount;
                                }
                            }

                            
                            History_Totallbl.Text = $"{totalPrice:C}";
                            History_Amountlbl.Text = $"{totalAmount}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}

