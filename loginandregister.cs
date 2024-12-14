using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project2
{
    public partial class loginandregister : Form
    {
        private MySqlConnection databaseConnection()
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=project2;";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            return conn;
        }
        public loginandregister()
        {
            InitializeComponent();
        }

        private void closebtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void registerbtn_Click(object sender, EventArgs e)
        {
            if (register_panel.Visible == false)
            {
                register_panel.Visible = true;
            }
           
        }

        private void createaccountBtn_Click(object sender, EventArgs e)
        {
            string Registeremail = Register_Emailtextb.Text;
            string Registerpassword = Register_passwordtextb.Text;
            string Registerconpassword = Register_conpasswordtextb.Text;

            if (string.IsNullOrEmpty(Registeremail) || string.IsNullOrEmpty(Registerpassword) || string.IsNullOrEmpty(Registerconpassword))
            {
                MessageBox.Show("โปรดใส่ข้อมูลให้ครบ");
                return;
            }

            if (!IsValidEmail(Registeremail))
            {
                MessageBox.Show("โปรดใส่อีเมลในรูปแบบที่ถูกต้อง");
                return;
            }

            if (!IsValidPassword(Registerpassword))
            {
                MessageBox.Show("รหัสผ่านต้องมีความยาวมากกว่า 8 ตัวอักษร และต้องมีตัวพิมพ์ใหญ่ ตัวพิมพ์เล็ก และสัญลักษณ์พิเศษ");
                return;
            }

            if (Registerpassword != Registerconpassword)
            {
                MessageBox.Show("รหัสผ่านไม่ตรงกัน");
                return;
            }

            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();
                    string sql = "INSERT INTO users ( email, password, image) VALUES ( @email, @password, @image)";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("email", Registeremail);
                    cmd.Parameters.AddWithValue("password", Registerpassword);


                    if (Register_image.Image != null)
                    {
                        using (var ms = new System.IO.MemoryStream())
                        {
                            Register_image.Image.Save(ms, Register_image.Image.RawFormat);
                            byte[] imgBytes = ms.ToArray();
                            cmd.Parameters.AddWithValue("@image", imgBytes);
                        }
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@image", DBNull.Value);
                    }

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("บัญชีถูกสร้างเรียบร้อยแล้ว");
                    Register_image.Image = null;
                    Register_Emailtextb.Clear();
                    Register_passwordtextb.Clear();
                    Register_conpasswordtextb.Clear();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn?.Close();
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

        private void closeRegister_panel_Click(object sender, EventArgs e)
        {
            if (register_panel.Visible == true)
            {
                register_panel.Visible = false;
            }
        }

        private void Register_image_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.png;*.gif)|*.bmp;*.jpg;*.png;*.gif|All files (*.*)|*.*";
            openFileDialog.Title = "Select an image file";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {

                    Register_image.Image = Image.FromFile(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Register_passwordCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            Register_passwordtextb.UseSystemPasswordChar = !Register_passwordCheckBox1.Checked;
        }

        private void Register_conpasswordCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            Register_conpasswordtextb.UseSystemPasswordChar = !Register_conpasswordCheckBox1.Checked;
        }

        private void loginandregister_Load(object sender, EventArgs e)
        {
            Register_passwordtextb.UseSystemPasswordChar = true;
            Register_conpasswordtextb.UseSystemPasswordChar=true;
            Login_passwordtextb.UseSystemPasswordChar=true;
            Register_passwordCheckBox1.CheckedChanged += Register_passwordCheckBox1_CheckedChanged;
            Register_conpasswordCheckBox1.CheckedChanged += Register_conpasswordCheckBox1_CheckedChanged;
            loginpasswordCheckBox1.CheckedChanged += loginpasswordCheckBox1_CheckedChanged;


;
            

        }



        private void loginpasswordCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            Login_passwordtextb.UseSystemPasswordChar = !loginpasswordCheckBox1.Checked;
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            string loginemail = Login_emailtextb.Text;
            string loginpassword = Login_passwordtextb.Text;

            if (string.IsNullOrEmpty(loginemail) || string.IsNullOrEmpty(loginpassword))
            {
                MessageBox.Show("โปรดกรอกรหัสผ่านและอีเมล");
                return;
            }

            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();


                    string usersql = "SELECT COUNT(*) FROM users WHERE email = @email AND password = @password";
                    MySqlCommand usercmd = new MySqlCommand(usersql, conn);
                    usercmd.Parameters.AddWithValue("@email", loginemail);
                    usercmd.Parameters.AddWithValue("@password", loginpassword);
                    int userCount = Convert.ToInt32(usercmd.ExecuteScalar());

                    if (userCount > 0)
                    {
                        main m = new main();
                        m.email = loginemail;
                        m.Show();

                        this.Hide();
                        return;
                    }


                    string adminsql = "SELECT COUNT(*) FROM admin WHERE admin = @email AND password = @password";
                    MySqlCommand admincmd = new MySqlCommand(adminsql, conn);
                    admincmd.Parameters.AddWithValue("@email", loginemail);
                    admincmd.Parameters.AddWithValue("@password", loginpassword);
                    int adminCount = Convert.ToInt32(admincmd.ExecuteScalar());

                    if (adminCount > 0)
                    {
                        adminform adminForm = new adminform();
                        adminForm.Show();
                        this.Hide();
                        return;
                    }

                    MessageBox.Show("อีเมลหรือรหัสผ่านไม่ถูกต้อง");

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

        
    }
}

