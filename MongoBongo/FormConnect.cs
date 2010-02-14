using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Mongo;
using System.Net.Sockets;

namespace MongoBongo
{
    public partial class FormConnect : Form
    {

        public FormConnect()
        {
            InitializeComponent();
        }


        private void FormConnect_Load(object sender, EventArgs e)
        {
            cmdAuthType.SelectedIndex = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdAuthType_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool enabled = (cmdAuthType.SelectedIndex != 0);

            lblUserName.Enabled =
            lblPassword.Enabled =
            txtUserName.Enabled =
            txtPassword.Enabled = enabled;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            MainForm form = this.Owner as MainForm;

            if (form != null)
            {
                MongoContext context = new MongoContext(txtServerName.Text, int.Parse(txtPortNumber.Text), false);

                try
                {
                    context.Connect();
                }
                catch (SocketException exc)
                {
                    MessageBox.Show("Unable to connect: " + exc.Message, "Unable to connect!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                form.Context = context;
                this.Close();
            }
        }
    }
}
