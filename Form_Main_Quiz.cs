using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment_Programming_Design
{
    public partial class frmMainQuiz : Form
    {
        int[] randomNumbersArr = new int[6];
        int equation1_Answer;
        int equation2_Answer;
        int equation3_Answer;

        public frmMainQuiz()
        {
            InitializeComponent();
        }

        private void GenerateNumbers()
        {
            Random randomNumber = new Random();
            if (cboMagnitude.Text == "0-10")
            {
                if (chkNegative.Checked)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        int randNum = randomNumber.Next(-10, 11);
                        randomNumbersArr[i] = randNum;
                    }
                }
                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        int randNum = randomNumber.Next(0, 11);
                        randomNumbersArr[i] = randNum;
                    }
                }
            }
            if (cboMagnitude.Text == "0-100")
            {
                if (chkNegative.Checked)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        int randNum = randomNumber.Next(-100, 101);
                        randomNumbersArr[i] = randNum;
                    }
                }
                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        int randNum = randomNumber.Next(0, 101);
                        randomNumbersArr[i] = randNum;
                    }
                }
            }
            if (cboMagnitude.Text == "0-1000")
            {
                if (chkNegative.Checked)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        int randNum = randomNumber.Next(-1000, 1001);
                        randomNumbersArr[i] = randNum;
                    }
                }
                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        int randNum = randomNumber.Next(0, 1001);
                        randomNumbersArr[i] = randNum;
                    }
                }
            }
        }

        private void btnGenQuestions_Click(object sender, EventArgs e)
        {
            lblCheckEq1.Text = string.Empty;
            lblCheckEq2.Text = string.Empty;
            lblCheckEq3.Text = string.Empty;
            lblEquation1.Visible = true;
            lblEquation2.Visible = true;
            lblEquation3.Visible = true;
            txtEquation1.Visible = true;
            txtEquation2.Visible = true;
            txtEquation3.Visible = true;
            btnCheckAnswer.Visible = true;
            GenerateNumbers();
            ClearTextBoxes();
            if (optAdd.Checked)
            {
                lblEquation1.Text = $"{randomNumbersArr[0]} + {randomNumbersArr[1]} =";
                lblEquation2.Text = $"{randomNumbersArr[2]} + {randomNumbersArr[3]} =";
                lblEquation3.Text = $"{randomNumbersArr[4]} + {randomNumbersArr[5]} =";
                equation1_Answer = randomNumbersArr[0] + randomNumbersArr[1];
                equation2_Answer = randomNumbersArr[2] + randomNumbersArr[3];
                equation3_Answer = randomNumbersArr[4] + randomNumbersArr[5];
            }
            else if (optMultiplication.Checked)
            {
                lblEquation1.Text = $"{randomNumbersArr[0]} x {randomNumbersArr[1]} =";
                lblEquation2.Text = $"{randomNumbersArr[2]} x {randomNumbersArr[3]} =";
                lblEquation3.Text = $"{randomNumbersArr[4]} x {randomNumbersArr[5]} =";
                equation1_Answer = randomNumbersArr[0] * randomNumbersArr[1];
                equation2_Answer = randomNumbersArr[2] * randomNumbersArr[3];
                equation3_Answer = randomNumbersArr[4] * randomNumbersArr[5];
            }
            else if (optSubstract.Checked)
            {
                lblEquation1.Text = $"{randomNumbersArr[0]} - {randomNumbersArr[1]} =";
                lblEquation2.Text = $"{randomNumbersArr[2]} - {randomNumbersArr[3]} =";
                lblEquation3.Text = $"{randomNumbersArr[4]} - {randomNumbersArr[5]} =";
                equation1_Answer = randomNumbersArr[0] - randomNumbersArr[1];
                equation2_Answer = randomNumbersArr[2] - randomNumbersArr[3];
                equation3_Answer = randomNumbersArr[4] - randomNumbersArr[5];
            }
        }

        private void btnCheckAnswer_Click(object sender, EventArgs e)
        {
            if (Valid())
            {
                if (Convert.ToInt32(txtEquation1.Text) == equation1_Answer)
                {
                    lblCheckEq1.Text = "Correct";
                    lblCheckEq1.ForeColor = Color.LightGreen;
                }
                else if (Convert.ToInt32(txtEquation1.Text) != equation1_Answer)
                {
                    lblCheckEq1.Text = "Wrong";
                    lblCheckEq1.ForeColor = Color.Red;
                }

                if (Convert.ToInt32(txtEquation2.Text) == equation2_Answer)
                {
                    lblCheckEq2.Text = "Correct";
                    lblCheckEq2.ForeColor = Color.LightGreen;
                }
                else if (Convert.ToInt32(txtEquation2.Text) != equation2_Answer)
                {
                    lblCheckEq2.Text = "Wrong";
                    lblCheckEq2.ForeColor = Color.Red;
                }

                if (Convert.ToInt32(txtEquation3.Text) == equation3_Answer)
                {
                    lblCheckEq3.Text = "Correct";
                    lblCheckEq3.ForeColor = Color.LightGreen;
                }
                else if (Convert.ToInt32(txtEquation3.Text) != equation3_Answer)
                {
                    lblCheckEq3.Text = "Wrong";
                    lblCheckEq3.ForeColor = Color.Red;
                }
            }
        }

        private void ClearTextBoxes()
        {
            txtEquation1.Text = string.Empty;
            txtEquation2.Text = string.Empty;
            txtEquation3.Text = string.Empty;
        }

        private bool Valid()
        {
            bool answer1_HaveLetter = txtEquation1.Text.Any(c => char.IsLetter(c));
            bool answer2_HaveLetter = txtEquation2.Text.Any(c => char.IsLetter(c));
            bool answer3_HaveLetter = txtEquation3.Text.Any(c => char.IsLetter(c));

            if (txtEquation1.Text == string.Empty || txtEquation1.Text == string.Empty || txtEquation1.Text == string.Empty)
            {
                MessageBox.Show("Please fill the answer boxes");
                return false;
            }
            else if (answer1_HaveLetter || answer2_HaveLetter || answer3_HaveLetter)
            {
                MessageBox.Show("Only digits for the answers");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnSwitchToTimesFrm_Click(object sender, EventArgs e)
        {
            frmTimesTable TimesTableForm = new frmTimesTable();
            TimesTableForm.Show();
        }
    }
}
