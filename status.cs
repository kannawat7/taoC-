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
using PdfiumViewer;

namespace project2
{
    public partial class status : UserControl
    {
        private string _orderno;
        private string _status;
        
        private Image _slip;

        private string _comment;

        private byte[] _pdfrecipt;

        public byte[] Pdfrecipt
        {
            get { return _pdfrecipt; }
            set { _pdfrecipt = value; UpdatePdfViewer(); }
        }

        private void UpdatePdfViewer()
        {
            if (_pdfrecipt != null && _pdfrecipt.Length > 0)
            {
                // Create a temporary file path
                string tempFilePath = Path.Combine(Path.GetTempPath(), "temp.pdf");

                // Write the byte array to the temporary file
                File.WriteAllBytes(tempFilePath, _pdfrecipt);

                // Load the temporary file into the PDF viewer
                axAcroPDF1.LoadFile(tempFilePath);
            }
        }
        public string Comment
        {
            get { return _comment; }
            set { _comment = value; Commentlbl.Text = value; }
        }
        public Image Slip
        {
            get { return _slip; }
            set { _slip = value; slipPictureBox.Image = value; }
        }

        public string Orderno
        {
            get { return _orderno; }
            set { _orderno = value; OrderNolbl.Text = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; Statuslbl.Text = value; }
        }

        public status()
        {
            InitializeComponent();
        }
    }
}
