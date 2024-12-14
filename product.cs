using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project2
{
    public partial class product : UserControl
    {

        private MySqlConnection databaseConnection()
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=project2;";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            return conn;
        }
        private Panel _detailPanel;
        private Label _detailproduct;
        private Label _detailid;
        private Label _detailLabel;
        private Label _detailprice;
        private Label _detailamount;
        private PictureBox _detailimage;

        

        private string _id;
        private string _product;
        private decimal _price;
        private Image _image;


        public string Id
        {
            get { return _id; }
            set { _id = value; id_lbl.Text = value; }
        }

        public string Product
        {
            get { return _product; }
            set { _product = value; product_lbl.Text = value; }
        }

        public decimal Price
        {
            get { return _price; }
            set { _price = value; price_lbl.Text = value.ToString("C2"); }
        }



        public Image Image
        {
            get { return _image; }
            set { _image = value; Product_image.Image = value; }
        }

        public product()
        {
            InitializeComponent();
        }

        private void product_Load(object sender, EventArgs e)
        {

        }

        private void product_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Color.Black;
        }

        private void product_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
        }

        private void product_lbl_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Color.Black;
            this.ForeColor = Color.White;
        }

        private void product_lbl_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
            this.ForeColor = Color.Black;
        }

        private void price_lbl_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Color.Black;
            this.ForeColor = Color.White;
        }

        private void price_lbl_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
            this.ForeColor = Color.Black;
        }

        public product(Panel detailPanel,Label detail_id,Label detail_detail,Label detail_price,Label deatail_amount,PictureBox detail_image,Label detail_product) : this()
        {
            
            _detailPanel = detailPanel;
            _detailid =  detail_id;
            _detailLabel = detail_detail;
            _detailprice = detail_price;
            _detailamount = deatail_amount;
            _detailimage = detail_image;
            _detailproduct = detail_product;
        }


        private void cartdetailview_Click(object sender, EventArgs e)
        {
            if (_detailPanel != null && _detailPanel.Visible == false)
            {
                _detailPanel.Visible = true;

                byte[] mimage = null;
                using (MySqlConnection conn = databaseConnection())
                {
                    try
                    {
                        conn.Open();
                        string sql = "SELECT id,product,detail,price,amount,image FROM product WHERE id=@Id";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@Id", Id);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                _detailid.Text = reader["id"].ToString();
                                _detailproduct.Text = reader["product"].ToString();
                                _detailLabel.Text = reader["detail"].ToString();
                                _detailprice.Text = reader["price"].ToString();
                                _detailamount.Text = reader["amount"].ToString();
                                
                                if (!reader.IsDBNull(reader.GetOrdinal("image")))
                                {
                                    mimage = (byte[])reader["image"];
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                    finally
                    {
                        conn.Close();
                    }

                    if (_detailimage != null)
                    {
                        if (mimage != null)
                        {
                            using (MemoryStream ms = new MemoryStream(mimage))
                            {
                                Image userImage = Image.FromStream(ms);
                                _detailimage.Image = userImage;
                            }
                        }
                        else
                        {
                            _detailimage.Image = null;
                        }
                    }
                }
            }

        }
    }
}
