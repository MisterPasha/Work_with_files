using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Data_Structure_Assignment_v1._0
{
    class PhoneBook
    {
        string path;

        public PhoneBook(string pathIn)
        {
            this.path = pathIn;                                    //Creating a constructor with path to csv file as an argument
        }

        public struct details                                      // Creating a structure to devide details of person from csv file
        {                                                          // and assigning them to my variables.
            public string surname;                                 //
            public string forename;                                //
            public string extensionCode;                           //
            public string mobileNumber;                            //
        }

        private details[] phoneBookDetails;                         // Creating 1D Array of structure 'details'

        public void Add(string surname, string forename, string extensionCode, string mobileNumber)  // Created an Add function with three parameters which are instances of textboxes
        {
            bubbleSortByExtensionCode();
            bool formatIsCorrect = formatCheck(surname, forename, extensionCode, mobileNumber);   //Checks if the format of entered in textboxes surname, forename and extension code are correct.
            bool dublicateDetected = DublicateDetected(extensionCode);              //Checks if entered extension code has a dublicate

            if (formatIsCorrect && !dublicateDetected)                              // If format of text boxes is correct and extension code does not match to existing codes, it executes code below
            {
                StreamWriter sw = new StreamWriter(path, true);                                                 // Created a StreamWriter to be able to write in the file and gave it a path to the file
                sw.WriteLine($"{char.ToUpper(surname[0]) + surname.ToLower().Substring(1)}," +                  // Writing data from textboxes into the csv file  
                    $"{char.ToUpper(forename[0]) + forename.ToLower().Substring(1)},{extensionCode},{mobileNumber}");  // and capitalizing Surename and Forename
                sw.Close();                                                                                     // Closing StreamWriter, so other functions can access the file
                PopulateArrayFromCsv();                                                                         // Rewriting array from the csv file
                MessageBox.Show("Added!");                                                                      // Shows the message that record has been added
            }
        }

        public void Update(string surname, string forename, string extensionCode, string mobileNumber)       // Function that updates the record in both array and csv file by extension code, using surname, forename and extension code as a parameters
        {
            bool formatIsCorrect = formatCheck(surname, forename, extensionCode, mobileNumber);       // Checks if format of textboxes is correct

            if (formatIsCorrect)                                                        // If argument returns true - executes the code below
            {
                bubbleSortByExtensionCode();                                                                        // Performs sorting algorithm using extension code for fast searching
                int position = binarySearchByExtensionNumber(extensionCode);                                        // binarySearchByExtensionNumber function returns index of found record in variable
                if (position >= 0)                                                                                      // Checking that binary search did not return -1
                {
                    phoneBookDetails[position].surname = char.ToUpper(surname[0]) + surname.ToLower().Substring(1);     // Rewriting surname and capitalizing it
                    phoneBookDetails[position].forename = char.ToUpper(forename[0]) + forename.ToLower().Substring(1);  // Rewriting forename and capitalizing it
                    phoneBookDetails[position].mobileNumber = mobileNumber;                                             // Rewriting mobile number
                    rewriteCsvFileFromArray();                                                                          // Rewriting whole csv file from array, as record has been changed 
                    MessageBox.Show("Updated!");                                                                        // Message that record has been updated
                }
                else                                                                                                     // If binary search returned -1 meaning that index was not found
                {                                                                                                        // it shows the message
                    MessageBox.Show("No record with such code");                                                         //
                }
            }
        }

        public void Search(string surname, ListView lvDetails, Label message)                           // Function that searching a record by surname entered in textbox, has parameters: surname, ListView and Label
        {
            bubbleSortBySurname();                                                                      // Calling function that sorts an array alphabetically by surname for 
            if (surname != string.Empty)                                                                // If surname text box is not empty - executes code below
            {
                string rightFormatSurname = char.ToUpper(surname[0]) + surname.ToLower().Substring(1);  // Captilizing surname entered in surname textbox
                int[] surnamePositions = binarySearchBySurname(rightFormatSurname);                     // Putting returned from sequentialSearchBySurname indexes in surnamePositions array
                for (int i = 0; i < surnamePositions.Length; i++)                                       // Loop that adds records in the ListView using each returned index
                {
                    ListViewItem lvi = new ListViewItem();                                              // Creating an instance of ListViewItem class
                    lvi.Text = phoneBookDetails[surnamePositions[i]].surname;                           // Assigning surname in surname column of ListView
                    lvi.SubItems.Add(phoneBookDetails[surnamePositions[i]].forename);                   // Assigning forename in forename column of ListView
                    lvi.SubItems.Add(phoneBookDetails[surnamePositions[i]].extensionCode);              // Assigning extension code in extension code column of ListView
                    lvi.SubItems.Add(phoneBookDetails[surnamePositions[i]].mobileNumber);               // Assigning mobile number in mobile number column of ListView
                    lvDetails.Items.Add(lvi);                                                           // Adding the whole record in ListView
                }
                message.Text = $"{surnamePositions.Length} records have been found";                    // Shows the message of quantity records found
            }
            else
            {
                MessageBox.Show("Please enter surname");                                                // If surname tetxbox is empty it notifies the user 
            }

        }

        public void PopulateArrayFromCsv()                               //Loading data from csv file into structured array
        {
            int lineCount = File.ReadLines(path).Count();                // Counts number of lines in csv file
            StreamReader sr = new StreamReader(path);                    // Creating a StreamReader and assigning path to the csv file 
            phoneBookDetails = new details[lineCount];                   // Assigned a size of the array
            details currentPerson = new details();                       // Creating a instanse of the structure details for the current line
            string[] fields;                                             // Creating array to keep details separately for each line 


            while (!sr.EndOfStream)                                      // Loop will read a csv file until the end line by line
            {
                for (int i = 0; i < lineCount; i++)                      // Loop which counts lines from 0 
                {
                    string line = sr.ReadLine();                         // Reads a line
                    fields = line.Split(',');                            // Splitting a line with coma
                    currentPerson.surname = fields[0];                   //
                    currentPerson.forename = fields[1];                  // Assigning each person's detail to a structure's field
                    currentPerson.extensionCode = fields[2];             //
                    currentPerson.mobileNumber = fields[3];              //
                    phoneBookDetails[i] = currentPerson;                 // Assigning each person's structured array to the main Array 
                }
            }
            sr.Close();
        }

        private bool DublicateDetected(string txtExtension)                             // Function that performs binary search algorithm by extension number. txtExtension as a parameter
        {                                                                                // returns true if code has been found, false if has not
            int mid;
            int start = 0;
            int end = phoneBookDetails.Length - 1;
            bool found = false;
            bool allListSearched = false;
            string message = $"Extension code {txtExtension} has already been used";     // assigning string to the message variable

            while (!found && !allListSearched)                                                    //
            {                                                                                     // Performing binary search algorithm
                mid = (start + end) / 2;                                                          //
                                                                                                  //
                if (phoneBookDetails[mid].extensionCode == txtExtension)                          //
                {                                                                                 //
                    MessageBox.Show(message);                                                     // If found, returns true and shows displays the message
                    return true;                                                                  //
                }                                                                                 //
                else if (start > end)                                                             //
                {                                                                                 //
                    return false;                                                                 //
                }                                                                                 //
                else if (String.Compare(txtExtension, phoneBookDetails[mid].extensionCode) < 0)   //
                {                                                                                 //
                    end = mid - 1;                                                                //
                }                                                                                 //
                else                                                                              //
                {                                                                                 //
                    start = mid + 1;                                                              //
                }                                                                                 //
            }
            return false;
        }

        private bool missingData(string surname, string forename, string extencionCode, string mobileNumber)  // This function defines if any text box is empty.
        {                                                                                                     // Returnes boolean value
            if (surname == string.Empty || forename == string.Empty ||
                extencionCode == string.Empty || mobileNumber == string.Empty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void bubbleSortByExtensionCode()                                     // Function that sorting an array by extension code
        {
            bool sorted;
            details temp;                                                           // Instance of the structure details to keep temporary value

            do
            {
                sorted = true;
                for (int i = 0; i < phoneBookDetails.Length - 1; i++)                                                         // Performing sorting algorithm using loop do-while
                {                                                                                                             //
                    if (String.Compare(phoneBookDetails[i].extensionCode, phoneBookDetails[i + 1].extensionCode) > 0)         //
                    {                                                                                                         //
                        temp = phoneBookDetails[i];                                                                           //
                        phoneBookDetails[i] = phoneBookDetails[i + 1];                                                        //
                        phoneBookDetails[i + 1] = temp;                                                                       //
                        sorted = false;                                                                                       //
                    }                                                                                                         //
                }                                                                                                             //
            }
            while (!sorted);
        }

        public void bubbleSortBySurname()                                           // Function that sorting an array by surname
        {
            bool sorted;
            details temp;

            do
            {                                                                                                      // Performing sorting algorithm using loop do-while
                sorted = true;                                                                                     //
                for (int i = 0; i < phoneBookDetails.Length - 1; i++)                                              //
                {                                                                                                  //
                    if (String.Compare(phoneBookDetails[i].surname, phoneBookDetails[i + 1].surname) > 0)          //
                    {                                                                                              //
                        temp = phoneBookDetails[i];                                                                //
                        phoneBookDetails[i] = phoneBookDetails[i + 1];                                             //
                        phoneBookDetails[i + 1] = temp;                                                            //
                        sorted = false;                                                                            //
                    }                                                                                              //
                }                                                                                                  //
            }                                                                                                      //
            while (!sorted);
        }

        private int binarySearchByExtensionNumber(string codeWanted)                             // Function that performs binary search algorithm by extension number. codeWanted as a parameter
        {
            int mid = 0;
            int start = 0;
            int end = phoneBookDetails.Length - 1;
            bool found = false;
            bool allListSearched = false;

            while (!found && !allListSearched)                                                    //
            {                                                                                     // Performing binary search algorithm
                mid = (start + end) / 2;                                                          //
                                                                                                  //
                if (phoneBookDetails[mid].extensionCode == codeWanted)                            //
                {                                                                                 //
                    found = true;                                                                 //
                }                                                                                 //
                else if (start > end)                                                             //
                {                                                                                 //
                    allListSearched = true;                                                       //
                }                                                                                 //
                else if (String.Compare(codeWanted, phoneBookDetails[mid].extensionCode) < 0)     //
                {                                                                                 //
                    end = mid - 1;                                                                //
                }                                                                                 //
                else                                                                              //
                {                                                                                 //
                    start = mid + 1;                                                              //
                }                                                                                 //
            }
            if (found)                                                        // if value is found, it returnes its index, otherwise returns -1
            {                                                                 //
                return (mid);                                                 //
            }                                                                 //
            else                                                              //
            {                                                                 //
                return -1;                                                    //
            }                                                                 //
        }

        private int[] binarySearchBySurname(string surnameWanted)
        {
            int InitialMid;
            int mid = 0;
            int start = 0;
            int end = phoneBookDetails.Length - 1;
            bool found = false;
            bool allListSearched = false;
            ArrayList indexes = new ArrayList();                                // Creates an instance of ArrayList.
            int[] SurnameIndexes;                                               // Creates an array for surname indexes
            bool haveSurnamesUp = true;
            bool haveSurnamesDown = true;

            while (!found && !allListSearched)
            {
                mid = (start + end) / 2;

                if (phoneBookDetails[mid].surname.Contains(surnameWanted))
                {
                    indexes.Add(mid);
                    found = true;
                }
                else if (start > end)
                {
                    allListSearched = true;
                }
                else if (String.Compare(surnameWanted, phoneBookDetails[mid].surname) < 0)
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
                InitialMid = mid;
                while (haveSurnamesUp)
                {
                    mid++;
                    if (phoneBookDetails[mid].surname.Contains(surnameWanted))
                    {
                        indexes.Add(mid);
                    }
                    else
                    {
                        haveSurnamesUp = false;
                    }
                }
                while (haveSurnamesDown)
                {
                    if (InitialMid < 1)
                    {
                        haveSurnamesDown = false;
                    }
                    InitialMid -= 1;
                    if (phoneBookDetails[InitialMid].surname.Contains(surnameWanted))
                    {
                        indexes.Add(InitialMid);
                    }
                    else
                    {
                        haveSurnamesDown = false;
                    }
                }
            }
            SurnameIndexes = new int[indexes.Count];
            for (int i = 0; i < indexes.Count; i++)
            {
                SurnameIndexes[i] = Convert.ToInt32(indexes[i]);
            }
            return SurnameIndexes;
        }

        private void rewriteCsvFileFromArray()                                    // Function that rewrites the whole csv file using data from array
        {
            StreamWriter sw = new StreamWriter(path, false);                      // Creating instance of StreamWriter to be able to write in the file

            for (int i = 0; i < phoneBookDetails.Length; i++)                     // Loop performs as many times as many records in the array
            {
                sw.WriteLine($"{phoneBookDetails[i].surname},{phoneBookDetails[i].forename}," +
                    $"{phoneBookDetails[i].extensionCode},{phoneBookDetails[i].mobileNumber}"); //Writing in the values separated by coma
            }
            sw.Close();                                                            // Closing StreamWriter so other functions will have an access to the file
        }

        private bool formatCheck(string surname, string forename, string extensionCode, string mobileNumber)  // Function that checking if the format of surname, forename and extension code textboxes is correct
        {
            bool surnameHaveDigit = surname.Any(c => char.IsDigit(c));             // Checks if surname has a digit. Returns boolean
            bool forenameHaveDigit = forename.Any(c => char.IsDigit(c));           // Checks if forename has a digit. Returns boolean

            if (missingData(surname, forename, extensionCode, mobileNumber))       //
            {                                                                     // Checking for missing data in any of the fields using fuction missingData()
                MessageBox.Show("Please fill all the fields out!");
                return false;
            }
            try
            {                                                                    // Performing an error handling of extencion code textbox
                Convert.ToInt32(extensionCode);                                  // Using try/catch
            }                                                                    // If extencion code not in digit-only format - an error message will appear
            catch (FormatException)                                              //
            {                                                                    //
                MessageBox.Show("Wrong data format for extension code!");        //
                return false;
            }
            try
            {                                                                     // Performing an error handling of mobileNumber textbox
                Convert.ToInt64(mobileNumber);                                    // Using try/catch
            }                                                                     // If mobile number is not in digit-only format - an error message will appear
            catch (FormatException)                                               //
            {                                                                     //
                MessageBox.Show("Wrong data format for mobile number!");          //
                return false;
            }

            if (surnameHaveDigit || forenameHaveDigit)                           // checking if surname and forename are in correct format. No digits allowed in names
            {
                MessageBox.Show("Wrong data format!");
                return false;
            }
            else if (extensionCode.Length != 4)                                   //
            {                                                                     // Checking the Length of extencion code. it must be exactly 4 digits
                MessageBox.Show("Extension Code must be 4 digits!");              //
                return false;
            }
            else if (mobileNumber.Length < 11 || mobileNumber.Length > 12)        //
            {                                                                     // Checking the length of mobile number. it must be 11 or 12 digits
                MessageBox.Show("Please enter valid mobile number");              //
                return false;
            }
            else
            {
                return true;
            }
        }

        public void ListArray(ListView lvDetails)                            // Testing. Testing function to list the whole array in the ListView                     
        {                                                                    // Testing.           
            for (int i = 0; i < phoneBookDetails.Length; i++)                // Testing.           
            {                                                                // Testing.           
                ListViewItem lvi = new ListViewItem();                       // Testing. Creates an instanse of ListViewItem          
                lvi.Text = phoneBookDetails[i].surname;                      // Testing. Creating an instance of ListViewItem class          
                lvi.SubItems.Add(phoneBookDetails[i].forename);              // Testing. Assigning surname in surname column of ListView          
                lvi.SubItems.Add(phoneBookDetails[i].extensionCode);         // Testing. Assigning forename in forename column of ListView
                lvi.SubItems.Add(phoneBookDetails[i].mobileNumber);          // Testing. Assigning mobile number in mobile number column of ListView
                lvDetails.Items.Add(lvi);                                    // Testing. Assigning extension code in extension code column of ListView          
            }
        }

        public void Clear(ListView lvDeatails, TextBox surname, TextBox forename, TextBox extensionCode)   // Function that removing all records from the ListView using a single function Clear() and ListView parameter
        {
            lvDeatails.Items.Clear();
            surname.Text = string.Empty;
            forename.Text = string.Empty;
            extensionCode.Text = string.Empty;
        }
    }
}
