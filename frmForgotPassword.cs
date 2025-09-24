using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace OrphanageSystem
{
    public partial class frmForgotPassword : Form
    {
        //initialize connections
        String connectionstring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\OrphanageDB.mdf;Integrated Security=True";
        SqlConnection cnn;
		SqlDataAdapter adapter;
		SqlDataReader reader;
		SqlCommand cmd;
		public frmForgotPassword()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
			string email = "", phone = "";
			try
			{
				cnn = new SqlConnection(connectionstring);
				cnn.Open();
				//initialize variables
				
					SqlCommand cmd = new SqlCommand("SELECT Email, PhoneNumber FROM tblAdopter WHERE Email = '" + txtEmail.Text +"' ", cnn);
					SqlDataReader reader = cmd.ExecuteReader();
					
					while (reader.Read())
					{
					email = reader.GetString(0);
					phone = reader.GetString(1); //assign value
					}
				reader.Close(); //close reader
								//validate
				
					if ( email == txtEmail.Text &&  phone == txtPhone.Text)
                {
					
					if (txtPassword.Text == txtPassword1.Text)
					{
						try
						{

							cmd = new SqlCommand("UPDATE tblAdopter SET Password = '" + txtPassword1.Text + "' WHERE Email = '" + email + "'", cnn);
							adapter = new SqlDataAdapter();
							adapter.UpdateCommand = cmd;
							adapter.UpdateCommand.ExecuteNonQuery();


							MessageBox.Show("Password Successfully updated!"); //feedback to user 
							this.Close(); //close form
							//Form1 form = new Form1();
							//form.Show();


						}
						catch (SqlException error)
						{
							MessageBox.Show(error.Message); //handle exception
						}
						}
					else
					{
						errorProvider1.SetError(txtPassword1, "Password does not match!");
					}
				}
					else
                {
					MessageBox.Show("Email or Phone number does not exist!");
				}


				
			}
			catch (SqlException error)
			{
				MessageBox.Show(error.Message); //handle exception
			}
		}

        private void frmForgotPassword_Load(object sender, EventArgs e)
        {

        }
    }
}
