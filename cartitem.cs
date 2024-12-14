using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Transactions;
using System.Windows.Forms;

namespace project2
{
    public partial class cartitem : UserControl
    {
        private MySqlConnection databaseConnection()
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=project2;";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            return conn;
        }

        public cartitem()
        {
            InitializeComponent();
        }
        public event EventHandler RequestShowAmount;
        public event EventHandler ResquestShowtotalprice;
        public string uid { get; set; }

        private string _id;
        private string _product;
        private int _amount;
        private decimal _price;
        private Image _image;

        
        public string Id
        {
            get { return _id; }
            set { _id = value; Cart_idlbl.Text = value; }
        }

        public string Product
        {
            get { return _product; }
            set { _product = value; Cart_productlbl.Text = value; }
        }

        public int Amount
        {
            get { return _amount; }
            set { _amount = value; Cart_amountlbl.Text = value.ToString(); }
        }

        public decimal Price
        {
            get { return _price; }
            set { _price = value; Cart_pricelbl.Text = value.ToString("C2"); }
        }

        public Image Image
        {
            get { return _image; }
            set { _image = value; Cart_productpicturebox.Image = value; }
        }
        private void cartitem_Load(object sender, EventArgs e)
        {
            
        }
        

        protected virtual void OnRequestShowAmount()
        {
            RequestShowAmount?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnRequestShowPrice() {
            ResquestShowtotalprice?.Invoke(this, EventArgs.Empty);
        }


        private void cart_delbtn_Click(object sender, EventArgs e)
        {
            delcartproduct();
            OnRequestShowAmount();
            OnRequestShowPrice();
        }


        private void delcartproduct()
        {
            using (MySqlConnection conn = databaseConnection())
            {
                try
                {

                    conn.Open();

                   
                    string restoreStockQuery = "UPDATE product SET amount = amount + @Amount WHERE product = @Product";
                    MySqlCommand restoreStockCmd = new MySqlCommand(restoreStockQuery, conn);
                    restoreStockCmd.Parameters.AddWithValue("@Amount", _amount);
                    restoreStockCmd.Parameters.AddWithValue("@Product", _product);
                    restoreStockCmd.ExecuteNonQuery();

                    
                    string deleteCartQuery = "DELETE FROM cart WHERE id = @Id";
                    MySqlCommand deleteCartCmd = new MySqlCommand(deleteCartQuery, conn);
                    deleteCartCmd.Parameters.AddWithValue("@Id", _id);
                    deleteCartCmd.ExecuteNonQuery();


                    this.Parent.Controls.Remove(this);
                    MessageBox.Show($"สินค้า {_product} จำนวน {_amount} ออกจากตระกร้าเเล้ว");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }


        




    }
 }
