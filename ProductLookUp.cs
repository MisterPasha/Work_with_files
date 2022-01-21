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
    public partial class frmProductLookup : Form
    {
        struct product
        {
            public string prodCode;
            public string description;
            public double price;
            public int quantity;
        }

        product[] partsTable;

        string path = "C:\\StudyStuff\\Nick\\CsharpProjects\\GUI\\FindProduct\\productFile.csv";

        public frmProductLookup()
        {
            InitializeComponent();
        }

        private void frmProductLookup_Load(object sender, EventArgs e)
        {
            loadData();
            bubbleSort();
            this.FormClosing += new FormClosingEventHandler(saveOnClose);
            
        }

        public void loadData()
        {    
            int lineCount = File.ReadLines(path).Count();

            partsTable = new product[lineCount];

            product currentPart = new product();

            string[] fields;

            StreamReader sr = new StreamReader(path);
            lvProducts.Items.Clear();
            while (!sr.EndOfStream)
            {
                for (int i = 0; i < lineCount; i++)
                {
                    string line = sr.ReadLine();
                    fields = line.Split(',');
                    currentPart.prodCode = fields[0];
                    currentPart.description = fields[1];
                    currentPart.price = Convert.ToDouble(fields[2]);
                    currentPart.quantity = Convert.ToInt32(fields[3]);
                    partsTable[i] = currentPart;
                }
            }
            sr.Close();
        }

        public void listParts()
        {
            for (int i = 0; i < partsTable.Length; i++)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = Convert.ToString(partsTable[i].prodCode);
                lvi.SubItems.Add(Convert.ToString(partsTable[i].description));
                lvi.SubItems.Add(Convert.ToString(partsTable[i].price));
                lvi.SubItems.Add(Convert.ToString(partsTable[i].quantity));
                lvProducts.Items.Add(lvi);
            }
        }

        private void btnListArray_Click(object sender, EventArgs e)
        {
            lvProducts.Items.Clear();
            loadData();
            listParts();
        }

        public void bubbleSort()
        {
            bool sorted;
            product temp;

            do
            {
                sorted = true;
                for (int i = 0; i < partsTable.Length-1; i++)
                {
                    if (String.Compare(partsTable[i].prodCode, partsTable[i + 1].prodCode) > 0)
                    {
                        temp = partsTable[i];
                        partsTable[i] = partsTable[i + 1];
                        partsTable[i + 1] = temp;
                        sorted = false;
                    }
                }
            }
            while (!sorted);
        }

        private int binarySearch(string codeWanted)
        {
            int mid = 0;
            int start = 0;
            int end = partsTable.Length - 1;
            bool found = false;
            bool allListSearched = false;

            while (!found && !allListSearched)
            {
                mid = (start + end) / 2;

                if (partsTable[mid].prodCode == codeWanted)
                {
                    found = true;
                }
                else if (start > end)
                {
                    allListSearched = true;
                }
                else if (String.Compare(codeWanted, partsTable[mid].prodCode) < 0)
                {
                    end = mid - 1;
                }
                else
                {
                    start = mid + 1;
                }
            }
            if (found)
            {
                return (mid);
            }
            else
            {
                return -1;
            }
        }

        private void btnFindProduct_Click(object sender, EventArgs e)
        {
            int searched = binarySearch(txtProdCode.Text);

            if (searched == -1)
            {
                lblMessage.Text = "No product with such code";
                tmrMessage.Enabled = true;
            }
            else
            {
                lvProducts.Items.Clear();
                ListViewItem lvi = new ListViewItem();
                lvi.Text = Convert.ToString(partsTable[searched].prodCode);
                lvi.SubItems.Add(Convert.ToString(partsTable[searched].description));
                lvi.SubItems.Add(Convert.ToString(partsTable[searched].price));
                lvi.SubItems.Add(Convert.ToString(partsTable[searched].quantity));
                lvProducts.Items.Add(lvi);
            }
        }

        private void tmrMessage_Tick(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
        }

        private void btnSale_Click(object sender, EventArgs e)
        {
            int searched = binarySearch(txtProdCode.Text);

            if (txtProdCode.Text == string.Empty)
            {
                lblMessage.Text = "Please enter the code of the product you want to sell";
                tmrMessage.Enabled = true;
            }
            else if (searched == -1)
            {
                lblMessage.Text = "No product with such code";
                tmrMessage.Enabled = true;
            }
            else
            {
                if (partsTable[searched].quantity > 0)
                {
                    lvProducts.Items.Clear();
                    partsTable[searched].quantity -= 1;
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = Convert.ToString(partsTable[searched].prodCode);
                    lvi.SubItems.Add(Convert.ToString(partsTable[searched].description));
                    lvi.SubItems.Add(Convert.ToString(partsTable[searched].price));
                    lvi.SubItems.Add(Convert.ToString(partsTable[searched].quantity));
                    lvProducts.Items.Add(lvi);
                }
                else
                {
                    lblMessage.Text = $"Item {txtProdCode.Text} is out of stock";
                }
            }
        }

        private void saveAll()
        {
            StreamWriter sw = new StreamWriter(path, false);
            
            for (int i = 0; i < partsTable.Length; i++)
            {
                sw.WriteLine($"{partsTable[i].prodCode},{partsTable[i].description},{partsTable[i].price},{partsTable[i].quantity}");
            }
            sw.Close();
        }

        private void saveOnClose(object sender, FormClosingEventArgs e)
        {
            saveAll();
        }

        private void tmrAutoSave_Tick(object sender, EventArgs e)
        {
            bool changeOccured = false;
            StreamReader sr = new StreamReader(path);
            
           
            for (int i = 0; i < partsTable.Length; i++)
            {
                string line = sr.ReadLine();
                string[] fields = line.Split(',');
                if(Convert.ToInt32(fields[3]) != partsTable[i].quantity)
                {
                    changeOccured = true;
                }
            }
            sr.Close();
           
            if(changeOccured)
            {
                saveAll();
            }
            
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            frmAddNewProduct newProductAdd = new frmAddNewProduct();
            newProductAdd.Show();
        }

        private void btnBuy_Click(object sender, EventArgs e)
        {
            int searched = binarySearch(txtProdCode.Text);

            if (txtProdCode.Text == string.Empty)
            {
                lblMessage.Text = "Please enter the code of the product you want to sell";
                tmrMessage.Enabled = true;
            }
            else if (searched == -1)
            {
                lblMessage.Text = "No product with such code";
                tmrMessage.Enabled = true;
            }
            else
            {  
                lvProducts.Items.Clear();
                partsTable[searched].quantity += 1;
                ListViewItem lvi = new ListViewItem();
                lvi.Text = Convert.ToString(partsTable[searched].prodCode);
                lvi.SubItems.Add(Convert.ToString(partsTable[searched].description));
                lvi.SubItems.Add(Convert.ToString(partsTable[searched].price));
                lvi.SubItems.Add(Convert.ToString(partsTable[searched].quantity));
                lvProducts.Items.Add(lvi);
            }
        }
    }
}