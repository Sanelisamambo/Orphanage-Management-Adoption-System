using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//add namespaces
using System.Data.SqlClient;
using System.Collections;

namespace OrphanageSystem
{
    public partial class frmDonor : Form
    {
        public frmDonor()
        {
            InitializeComponent();
        }
        String connectionstring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\OrphanageDB.mdf;Integrated Security=True";
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        SqlDataReader reader;
        double income = 0.00; //public field
        string childName = ""; //public field

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmOrphanageSystem myform = new frmOrphanageSystem();
            myform.ShowDialog(); //go to main page
            this.Close(); //close tab
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if ( tabctrlDonor.SelectedTab == tabApp) // Check which tab is currently selected
            {
                tabctrlDonor.SelectedTab = tabApp1; // Switch to the other tab
            }
        }
        public int getCount()
        {
            int count = 0;
            try
            {
                cnn = new SqlConnection(connectionstring);
                cnn.Open(); //open connection
                SqlCommand comm = new SqlCommand("SELECT COUNT(*) FROM tblApplication", cnn);
                count = (int)comm.ExecuteScalar(); //get num of elements
                cnn.Close(); //close connection
            }
            catch (SqlException error)
            {
                MessageBox.Show(error.Message);
            }

            return count;

        }
        public void loadAll()
        {
            
            lblLog.Text = "Logged in as " + frmOrphanageSystem.email; //show user currently logged in
            
            try
            {
                //show personal info
                cnn = new SqlConnection(connectionstring);
                cnn.Open(); //open connection

                adapter = new SqlDataAdapter("SELECT * FROM tblChild", cnn); //selection statement
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dgvChildren.DataSource = dataTable;

                //filter according to child id
                SqlCommand comm = new SqlCommand("SELECT Child_ID FROM tblChild", cnn);
                SqlDataReader read = comm.ExecuteReader();

                while (read.Read())
                {

                    cbxChildID.Items.Add(read.GetValue(0));
                }

                cnn.Close(); //close connection
            }
            catch (SqlException error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtAnnualIncome, ""); //clear existing errors
            if (double.TryParse(txtAnnualIncome.Text, out income))
            {
                if (tabctrlDonor.SelectedTab == tabApp1) // Check which tab is currently selected
                {
                    tabctrlDonor.SelectedTab = tabApp2; // Switch to the other tab
                }
            }
            else
            {
                errorProvider1.SetError(txtAnnualIncome, "Numeric values only!");
            }
            
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (tabctrlDonor.SelectedTab == tabApp2) // Check which tab is currently selected
            {
                tabctrlDonor.SelectedTab = tabApp1; // Switch to the other tab
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (tabctrlDonor.SelectedTab == tabApp1) // Check which tab is currently selected
            {
                tabctrlDonor.SelectedTab = tabApp; // Switch to the other tab
            }
        }

        private void scrlAge_Scroll(object sender, ScrollEventArgs e)
        {
            lblAge.Text = scrlAge.Value.ToString(); //display current value
        }

        

        private void frmDonor_Load(object sender, EventArgs e)
        {
            loadAll(); //call method
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            //sort according to gender
            if (rbtnMale.Checked)
            {
                try
                {
                    cnn = new SqlConnection(connectionstring);
                    cnn.Open();
                    adapter = new SqlDataAdapter("SELECT * FROM tblChild WHERE Gender = '" + rbtnMale.Text + "'", cnn);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgvChildren.DataSource = dataTable;
                    
                    cnn.Close();
                }
                catch (SqlException error)
                {
                    MessageBox.Show(error.Message); //validation
                }
            }
            
            
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            //sort according to gender
            if (rbtnFemale.Checked)
            {
                try
                {
                    cnn = new SqlConnection(connectionstring);
                    cnn.Open();
                    adapter = new SqlDataAdapter("SELECT * FROM tblChild WHERE Gender = '" + rbtnFemale.Text + "'", cnn);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgvChildren.DataSource = dataTable;

                    cnn.Close();
                }
                catch (SqlException error)
                {
                    MessageBox.Show(error.Message); //validation
                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            frmOrphanageSystem myform = new frmOrphanageSystem(); //instance
            //Random random = new Random();
            //int appID = random.Next(100, 1000); //generate application ID
            int appID = getCount() + 1; //get application id
            
            string medAid = "", medCondition = "", employment = "";
            //check whether user has medical aid or not
            if (rbtnYes.Checked)
            {
                medAid = "Yes"; 
            }
            else if (rbtnNo.Checked)
            {
                medAid = "No";
            }

            //check whether adopter has medic condition history or not
            if (rbtnMedNone.Checked)
            {
                medCondition = "None";
            }
            else if (rbtnMedYes.Checked)
            {
                medCondition = txtConditionHistory.Text; //get condition history
            }

            //check whether adopter is employed or not
            if (rbtnEmployed.Checked)
            {
                employment = rbtnEmployed.Text;
            }
            else if (rbtnSelfEmployed.Checked)
            {
                employment = rbtnSelfEmployed.Text;
            }
            else if (rbtnUnemployed.Checked)
            {
                employment = rbtnUnemployed.Text;
            }
            try
            {
                //create new application
                cnn = new SqlConnection(connectionstring);
                cnn.Open();
                cmd = new SqlCommand($"INSERT INTO tblApplication VALUES ('{appID}', '{frmOrphanageSystem.email}', '{txtBirth.Text}',  {scrlAge.Value}, '{medAid}', '{medCondition}', '{employment}', {income},  '{txtReasonAdopt.Text}', '{txtSelfDetails}', '{childName}', '{"Pending"}' )", cnn);
                adapter = new SqlDataAdapter();
                adapter.InsertCommand = cmd;
                adapter.InsertCommand.ExecuteNonQuery();

                //feedback to user
                MessageBox.Show("Application sucessfully submitted! \nProgress regarding your application will be " +
                    "sent to " + frmOrphanageSystem.email + "\n\tThank you!");
                frmOrphanageSystem frm = new frmOrphanageSystem(); //redirect user to main page
                frm.ShowDialog(); //go to main page
                cnn.Close();
            }
            catch (SqlException error)
            {
                MessageBox.Show(error.Message); //error message
            }
        }

        private void cbxChildID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cnn = new SqlConnection(connectionstring);
                cnn.Open();

                if (cbxChildID.SelectedIndex != -1)
                {
                    if (int.TryParse(cbxChildID.SelectedItem.ToString(), out int ID)) ; //get ID
                    
                    SqlCommand cmd = new SqlCommand("SELECT Name, Surname FROM tblChild WHERE Child_ID = " + ID, cnn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        childName = reader.GetString(0) + " " + reader.GetString(1); //get child details
                    }
                    reader.Close(); //close reader
                    //fill data grid view according to child id selected
                    adapter = new SqlDataAdapter("SELECT * FROM tblChild WHERE Child_ID = " + ID, cnn); //selection statement
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgvChildren.DataSource = dataTable; 

                }
            }
            catch (SqlException error)
            {
                MessageBox.Show(error.Message); //handle exception
            }
        }
    }
}
