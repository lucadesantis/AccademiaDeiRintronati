using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccademiaDeiRintronati
{
    public partial class AddArchivio : Form
    {
        public delegate void onCloseFormHandler(object sender, CloseFormEventArgs e);
        public event onCloseFormHandler onCloseForm;

        public class CloseFormEventArgs
        {
            public bool Cancel;
            public int ID;

            public CloseFormEventArgs(int id, bool cancel)
            {
                this.Cancel = cancel;
                this.ID = id;
            }
        }

        private bool cancel=false;
        private int id;

        public AddArchivio()
        {
            InitializeComponent();
            this.FormClosed += AddArchivio_FormClosed;
            this.label2.Visible = false;
            this.progressIndicator1.Visible = false;
            this.ActiveControl = this.textBox1;
        }

        private void AddArchivio_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (onCloseForm == null)
                return;

            CloseFormEventArgs args = new CloseFormEventArgs(this.id,this.cancel);
            onCloseForm(this, args);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.cancel = true;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBox1.Text))
            {
                MessageBox.Show("È necessario assegnare un nome all'Archivio per registrarlo nella sorgente dei dati.", Utility.appName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string nome = this.textBox1.Text;

            if(DataManager.getArchiviByName(nome).Count>0)
            {
                MessageBox.Show("Esiste già nella sorgente dei dati un archivio con questo nome.", Utility.appName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int i = DataManager.addArchivo(nome);

            if(i==-1)
                MessageBox.Show("Si é verificato un errore durante l'accesso alla sorgente dei dati non é stato possibile portare a termine l'operazione richiesta.", Utility.appName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            this.cancel = false;
            this.id = i;

            this.Close();
        }
    }
}
