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

namespace BinaryFileDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            string prodCode, description;
            int price, quantity;
            string path = "C:\\StudyStuff\\Nick\\CsharpProjects\\GUI\\BinaryFileDemo\\products.dat";
            BinaryWriter bw = new BinaryWriter(File.Open(path, FileMode.Create));

            prodCode = "0001";
            description = "Widget**";
            price = 260;
            quantity = 50;

            bw.Write(prodCode);
            bw.Write(description);
            bw.Write(price);
            bw.Write(quantity);

            prodCode = "0002";
            description = "Bolt****";
            price = 3;
            quantity = 5000;

            bw.Write(prodCode);
            bw.Write(description);
            bw.Write(price);
            bw.Write(quantity);

            prodCode = "0003";
            description = "Nut*****";
            price = 4;
            quantity = 3021;

            bw.Write(prodCode);
            bw.Write(description);
            bw.Write(price);
            bw.Write(quantity);

            prodCode = "0004";
            description = "Rivet***";
            price = 1;
            quantity = 4096;

            bw.Write(prodCode);
            bw.Write(description);
            bw.Write(price);
            bw.Write(quantity);

            bw.Close();
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            string prodCode, description;
            int price, quantity;

            string fileName = "C:\\StudyStuff\\Nick\\CsharpProjects\\GUI\\BinaryFileDemo\\products.dat";

            BinaryReader br = new BinaryReader(File.Open(fileName, FileMode.Open));
            lstOutput.Items.Clear();
            br.BaseStream.Seek(5, SeekOrigin.Begin);
            description = br.ReadString();
            lstOutput.Items.Add(description);
            br.BaseStream.Seek(27, SeekOrigin.Begin);
            description = br.ReadString();
            lstOutput.Items.Add(description);
            br.BaseStream.Seek(36, SeekOrigin.Begin);
            price = br.ReadInt32();
            lstOutput.Items.Add(price);
            br.BaseStream.Seek(71, SeekOrigin.Begin);
            description = br.ReadString();
            lstOutput.Items.Add(description);
            br.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string prodCode, description;
            int price, quantity;

            string fileName = "C:\\StudyStuff\\Nick\\CsharpProjects\\GUI\\BinaryFileDemo\\products.dat";

            BinaryWriter bw = new BinaryWriter(File.Open(fileName, FileMode.Open));
            bw.BaseStream.Seek(71, SeekOrigin.Begin);
            description = "Rivet4mm";
            bw.Write(description);
            price = 2;
            bw.Write(price);
            bw.Close();
        }
    }
}
