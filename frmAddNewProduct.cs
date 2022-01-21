using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace FindProduct
{
    public partial class frmAddNewProduct : Form
    {
        public frmAddNewProduct()
        {
            InitializeComponent();
        }

        private void frmAddNewProduct_Load(object sender, EventArgs e)
        {

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string path = "C:\\StudyStuff\\Nick\\CsharpProjects\\GUI\\FindProduct\\productFile.csv";
            int lineCount = File.ReadLines(path).Count();

            string productCode = txtprodCode.Text;
            string description = txtDescription.Text;
            string price = txtPrice.Text;
            string quantity = txtQuantity.Text;
            bool dublicateDetected = false;
            bool pathIsCorrect = true;


            if (pathIsCorrect)
            {
                string[] productCodesArray = new string[lineCount];
                StreamReader sr = new StreamReader(path);
                while (!sr.EndOfStream)
                {
                    for (int i = 0; i < lineCount; i++)
                    {
                        string line = sr.ReadLine();
                        string[] fields = line.Split(',');
                        productCodesArray[i] = fields[0];
                    }
                }
                sr.Close();

                foreach (string code in productCodesArray)
                {
                    if (txtprodCode.Text == code)
                    {
                        dublicateDetected = true;
                    }
                }

                if (dublicateDetected)
                {
                    string message = "Product Code Already In Use";
                    MessageBox.Show(message);
                }
                else
                {
                    StreamWriter sw = new StreamWriter(path, true);
                    sw.WriteLine($"{productCode},{description},{price},{quantity}");
                    sw.Close();
                    showMessage();
                    clearFields();
                }
            }
        }
      
        private void showMessage()
        {
            lblMessage.Text = $"Product {txtprodCode.Text} has been added";
            tmrMessage.Enabled = true;
        }

        private void tmrMessage_Tick(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            tmrMessage.Enabled = false;
        }

        private void clearFields()
        {
            txtDescription.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtprodCode.Text = string.Empty;
            txtQuantity.Text = string.Empty;
        }
    }
}
