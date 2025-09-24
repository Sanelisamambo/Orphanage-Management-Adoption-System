using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// Add the following namespace   
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace OrphanageSystem
{
    public partial class frmOrphanageSystem : Form
    {
        String connectionstring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\OrphanageDB.mdf;Integrated Security=True";
        SqlConnection cnn;
        public static string email = ""; //public variable
        public frmOrphanageSystem()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //clear existing errors
            errorProvider1.SetError(txtPhone, "");
            errorProvider1.SetError(txtPhone, "");

            // Create string variables that contain the patterns   
            string emailPattern = @"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$"; // Email address pattern                                                            
            Regex validatePhoneNumberRegex = new Regex("^\\+?[1-9][0-9]{7,14}$"); //Phone address pattern

            // Create a bool variable and use the Regex.IsMatch static method which returns true if a specific value matches a specific pattern  
            bool isEmailValid = Regex.IsMatch(txtEmail.Text, emailPattern);
            // Now you can check the result   
            if (!isEmailValid)
            {
                errorProvider1.SetError(txtEmail, "Please enter a valid email");
                
            }
            if (validatePhoneNumberRegex.IsMatch(txtPhone.Text) == false)
            {
                errorProvider1.SetError(txtPhone, "Please enter a valid phone number  e.g +27123456789");
            }// returns True

            //validate name and surname
            if (txtbxName.Text == "")
            {
                errorProvider1.SetError(txtbxName, "Required!");

            }
            if (txtbxSurname.Text == "")
            {
                errorProvider1.SetError(txtbxSurname, "Required!");

            }

            bool verify = false; //verify the password entered by user
            if ( txtbxPassword.Text == txtbxPassword1.Text)
            {
                verify = true; //change to positive value
            }
            else
            {
                errorProvider1.SetError(txtbxPassword1, "Password does not match!");
            }
            //if all validations are true
            if ( isEmailValid == true && validatePhoneNumberRegex.IsMatch(txtPhone.Text) == true
                && txtbxName.Text != "" && txtbxSurname.Text != "" && verify == true )
            {
                //validate gender
                string gender = "";
                if (rbtnMale.Checked)
                {
                    gender = "Male";
                }
                else if (rbtnFemale.Checked)
                {
                    gender = "Female";
                }
                if (gender != "")
                {
                   
                    try
                    {
                        cnn = new SqlConnection(connectionstring);
                        cnn.Open(); //open connection
                        SqlCommand cmd = new SqlCommand($"INSERT INTO tblAdopter VALUES ('{txtbxName.Text}', '{txtbxSurname.Text}', '{txtEmail.Text}', '{txtPhone.Text}', '{gender}', '{txtbxPassword1.Text}')", cnn);
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.InsertCommand = cmd;
                        adapter.InsertCommand.ExecuteNonQuery();

                        cnn.Close(); //close connection
                        MessageBox.Show("Account Successfully created!"); //feedback to user
                        if ( OrphanagetabC.SelectedTab == tabCreateAccount) //verify selected tab
                        {
                            OrphanagetabC.SelectedTab = tabLogin; //go to other tab
                        }
                    }
                    catch (SqlException error)
                    {
                        MessageBox.Show(error.Message); //print error
                    }
                    
                }
                else
                {
                    MessageBox.Show("Specify Gender!"); //feedback to user
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //check if account exists
                cnn = new SqlConnection(connectionstring);
                cnn.Open();
                //assign variables for validation
                string password_hash = ""; 
                bool validate = false;
                //read from table, get the email and password
                SqlCommand cmd = new SqlCommand("SELECT Email, Password FROM tblAdopter WHERE Email = '" + txtbxEmail.Text + "'", cnn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    validate = true; //assign true value
                    password_hash = reader.GetString(1); //assign password to string

                }
                
                reader.Close();//close reader
                if (txtPassword.Text == password_hash && validate == true)
                {
                    email = txtbxEmail.Text; //assign value to stay logged in
                    
                    lblMatch.Visible = false; //set false if true
                    frmDonor myform = new frmDonor(); //instance
                    myform.ShowDialog(); //display application form
                    this.Close(); //close current form
                }
                else
                {
                    lblMatch.Visible = true; //feedback to user
                }

                /*SqlCommand comm = new SqlCommand("SELECT Password FROM Users WHERE Username = '" + txtbxUsername.Text + "'", cnn);
                SqlDataReader read = comm.ExecuteReader();
                while (read.Read())
                {


                    if (txtbxPassword.Text != read.GetValue(0).ToString())
                    {
                        lblMatch.Visible = true;
                        break;
                    }
                    else
                    {

                    }
                }*/
                cnn.Close();
            }
            catch (SqlException error)
            {
                MessageBox.Show(error.Message); //print error
            }
        }

        private void lnkSignIn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (OrphanagetabC.SelectedTab == tabCreateAccount) // Check which tab is currently selected
            {
                OrphanagetabC.SelectedTab = tabLogin; // Switch to the other tab

            }
        }

        private void lnkSignUp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (OrphanagetabC.SelectedTab == tabLogin) // Check which tab is currently selected
            {
                OrphanagetabC.SelectedTab = tabCreateAccount; // Switch to the other tab

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (OrphanagetabC.SelectedTab == tabHome) // Check which tab is currently selected
            {
                OrphanagetabC.SelectedTab = tabLogin; // Switch to the other tab

            }
        }

        private void lnkForgot_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmForgotPassword myForm = new frmForgotPassword();
            myForm.ShowDialog(); // display 2nd form

            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAuthentication myform = new frmAuthentication(); //instance
            myform.ShowDialog(); //view form
            this.Close(); //close current form
        }

        private void frmOrphanageSystem_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Application.Exit(); //exit the program
        }
    }
}
