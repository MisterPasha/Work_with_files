using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Data_Structure_Assignment_v1._0
{
    public partial class Form1 : Form
    {
        static readonly string path = "C:\\StudyStuff\\Nick\\CsharpProjects\\ASSIGNMENTS\\Assignment Data Structure And File Handling\\Data_Structure_Assignment_v1.1\\Data_Structure_Assignment_v1.0\\details.csv";  // Path to the csv file

        PhoneBook myPhoneBook = new PhoneBook(path);                                           //Creating an instance of class PhoneBook

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            myPhoneBook.PopulateArrayFromCsv();                                                 // Calling function PopulateArrayFromCsv from class PhoneBook
        }                                                                                       // To populate array from csv file

        private void btnPopulateList_Click(object sender, EventArgs e)                          //Testing. Populate List View from array
        {                                                                                       
            lvDetails.Items.Clear();                                                            //Testing. Deleting all records from the List View
            myPhoneBook.ListArray(lvDetails);                                                   //Testing. Populating List View from array
        }                                                                                            

        private void btnAdd_Click(object sender, EventArgs e)
        {
            myPhoneBook.Add(txtSurname.Text, txtForename.Text, txtExtencionCode.Text, txtMobileNumber.Text);           // Calling Add() function to add records in csv file
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            myPhoneBook.Update(txtSurname.Text, txtForename.Text, txtExtencionCode.Text, txtMobileNumber.Text);        // Calling Update() function to update the record in both array and csv file
        }

        private void btnSearch_Click(object sender, EventArgs e)                                 
        {
            lvDetails.Items.Clear();
            myPhoneBook.Search(txtSurname.Text, lvDetails, lblSearchMessage);                    //Calling Search() function
            tmrMessage.Enabled = true;                                                           // Enabling the timer to remove the notifying label
        }

        private void tmrMessage_Tick(object sender, EventArgs e)
        {
            lblSearchMessage.Text = string.Empty;                                                // Timer that emptying the label
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            myPhoneBook.Clear(lvDetails, txtSurname, txtForename, txtExtencionCode);                                      // Calling Clear() function
        }
    }

}
