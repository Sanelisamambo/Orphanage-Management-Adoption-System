using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrphanageSystem
{
    public partial class frmAuthentication : Form
    {
        public frmAuthentication()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if ( txtPass.Text == "Orphanage") //validate password
            {
                this.Close(); //close current form
                frmStaff myform = new frmStaff(); //instance
                
                myform.ShowDialog(); //view staff formm
                
            }
        }

        private void frmAuthentication_Load(object sender, EventArgs e)
        {

        }
    }
}
