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
    public partial class frmStaff : Form
    {
        public frmStaff()
        {
            InitializeComponent();
        }
        //initialize
        String connectionstring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\OrphanageDB.mdf;Integrated Security=True";
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        SqlDataReader reader;

        private void button3_Click(object sender, EventArgs e)
        {
            if (cbxAppID.SelectedIndex != -1)
            {
                if (int.TryParse(cbxAppID.SelectedItem.ToString(), out int ID)) ; //get ID
                try
                {

                    //update application
                    cnn = new SqlConnection(connectionstring);
                    cnn.Open(); //open connection

                    cmd = new SqlCommand("UPDATE tblApplication SET Approved = 'Declined' WHERE Application_ID = " + ID, cnn);
                    adapter = new SqlDataAdapter();
                    adapter.UpdateCommand = cmd;
                    adapter.UpdateCommand.ExecuteNonQuery();

                    MessageBox.Show("Application declined!\nReason:\n"+ txtReason.Text +
                        "\n\nFeedback will be sent to adopter's email."); //feedback
                    loadAll(); //update new info
                    cnn.Close();

                }
                catch (SqlException error)
                {
                    MessageBox.Show(error.Message); //error message
                }
                
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmOrphanageSystem frm = new frmOrphanageSystem(); //instance
            frm.ShowDialog(); //view main page
            this.Close(); //close current form
        }

        private void frmStaff_Load(object sender, EventArgs e)
        {
            loadAll();//load items
        }
        public void loadAll()
        {
            try
            {
                //show personal info
                cnn = new SqlConnection(connectionstring);
                cnn.Open(); //open connection

                adapter = new SqlDataAdapter("SELECT Application_ID, Email, Age, Employment, Monthly_Income, Adopting_reason, Approved FROM tblApplication", cnn); //selection statement
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dgvApplications.DataSource = dataTable;

                //load application ID to combobox
                cbxAppID.Items.Clear(); //clear existing items
                SqlCommand comm = new SqlCommand("SELECT Application_ID FROM tblApplication", cnn);
                SqlDataReader read = comm.ExecuteReader();

                while (read.Read())
                {

                   cbxAppID.Items.Add(read.GetValue(0));
                }

                cnn.Close(); //close connection
            }
            catch (SqlException error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void cbxAppID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cnn = new SqlConnection(connectionstring);
                cnn.Open();

                if (cbxAppID.SelectedIndex != -1)
                {
                    if (int.TryParse(cbxAppID.SelectedItem.ToString(), out int ID)) ; //get ID

                    //fill data grid view according to application id selected
                    adapter = new SqlDataAdapter("SELECT Application_ID, Email, Age, Employment, Monthly_Income, Adopting_reason, Approved FROM tblApplication WHERE Application_ID = " + ID, cnn); //selection statement
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgvApplications.DataSource = dataTable;

                }
            }
            catch (SqlException error)
            {
                MessageBox.Show(error.Message); //handle exception
            }
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (cbxAppID.SelectedIndex != -1)
            {
                if (int.TryParse(cbxAppID.SelectedItem.ToString(), out int ID)) ; //get ID
                try
                {

                    //update application
                    cnn = new SqlConnection(connectionstring);
                    cnn.Open(); //open connection

                    cmd = new SqlCommand("UPDATE tblApplication SET Approved = 'Approved' WHERE Application_ID = " + ID, cnn);
                    adapter = new SqlDataAdapter();
                    adapter.UpdateCommand = cmd;
                    adapter.UpdateCommand.ExecuteNonQuery();

                    MessageBox.Show("Application Successfully approved!"); //feedback
                    loadAll(); //update new info
                    cnn.Close();

                }
                catch (SqlException error)
                {
                    MessageBox.Show(error.Message); //error message
                }
                
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxAppStatus.SelectedIndex != -1)
            {
                try
                {
                    cnn = new SqlConnection(connectionstring);
                    cnn.Open();

                    //fill data grid view according to application status selected
                    adapter = new SqlDataAdapter("SELECT Application_ID, Email, Age, Employment, Monthly_Income, Adopting_reason, Approved FROM tblApplication WHERE Approved = '" + cbxAppStatus.SelectedItem.ToString() + "'", cnn); //selection statement
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgvApplications.DataSource = dataTable;


                }
                catch (SqlException error)
                {
                    MessageBox.Show(error.Message); //handle exception
                }
            }
        }
    }
}
