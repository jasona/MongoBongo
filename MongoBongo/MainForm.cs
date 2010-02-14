using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MongoSharp;
using MongoSharp.Protocol.SystemMessages.Responses;

namespace MongoBongo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            this.Load += new EventHandler(MainForm_Load);
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            ShowConnectForm();
        }

        void connect_FormDisposed(Object sender, EventArgs e)
        {
            this.Refresh();
            if (Server != null)
            {
                List<DatabaseInfo> databases = Server.GetAllDatabases().ToList<DatabaseInfo>();

                TreeNode serverNode = new TreeNode(string.Format("({0}:{1})", Server.ServerName, Server.ServerPort), 0, 0);

                tvItems.Nodes.Add(serverNode);

                toolStripStatusLabel1.Text = "Loading...";
                statusStrip1.Update();
                toolStripProgressBar1.Visible = true;
                toolStripProgressBar1.Maximum = databases.Count;
                foreach (DatabaseInfo db in databases)
                {
                    TreeNode node = new TreeNode(db.Name, 1, 1);
                    serverNode.Nodes.Add(node);

                    AddCollectionNodes(db, node);
                    toolStripProgressBar1.Increment(1);
                }
                toolStripStatusLabel1.Text = "Ready";
                toolStripProgressBar1.Visible = false;
            }
        }

        public void AddCollectionNodes(DatabaseInfo db, TreeNode dbNode)
        {
            var database = Server.GetDatabase(db.Name);

            foreach(CollectionInfo collection in database.GetAllCollections())
            {
                TreeNode colNode = new TreeNode(collection.Name, 2, 2);

                dbNode.Nodes.Add(colNode);
            }
        }

        public MongoServer Server { get; set; }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you wish to disconnect?", "Disconnect Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Server = null;
                tvItems.Nodes.Clear();
            }
        }

        private void ShowConnectForm()
        {
            using (FormConnect connect = new FormConnect())
            {
                connect.Disposed += new EventHandler(connect_FormDisposed);
                connect.ShowDialog(this);
            }
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowConnectForm();
        }
    }
}
