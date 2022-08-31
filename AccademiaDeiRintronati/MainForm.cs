using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccademiaDeiRintronati
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!File.Exists(Utility.dbFile))
            {
                DialogResult dialogResult = MessageBox.Show("Il file con la sorgente dei dati non esiste, volete crearne uno ?\n\nScegliendo Si verrà creata una nuova sorgente dati, mentre selezionando No uscirete dall'applicazione.", Utility.appName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                    this.GeneraDB();
                else if (dialogResult == DialogResult.No)
                    this.Close();
            }

            this.Text=Utility.appName;
        }

        private void GeneraDB()
        {
            if(!Utility.generateEmptyDB())
            {
                MessageBox.Show("Si é verificato un errore durante la generazione della sorgente dei dati, impossibile continuare.", Utility.appName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            AddArchivio frm = new AddArchivio();
            frm.ShowDialog(this);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            AddDocumento frm = new AddDocumento();
            frm.ShowDialog(this);
        }
    }
}
