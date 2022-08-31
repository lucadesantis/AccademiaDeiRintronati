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
    public partial class AddDocumento : Form
    {
        private DataManager.Documento documento;
        private List<DataManager.Personaggio> Personaggi;
        private List<DataManager.Luogo> Luoghi;
        private string Note="";
        private string Link = "";

        public AddDocumento()
        {
            InitializeComponent();
            this.progressIndicator1.Visible = false;
            this.label2.Visible = false;
            this.documento = new DataManager.Documento();
            this.Personaggi=new List<DataManager.Personaggio>();
            this.Luoghi=new List<DataManager.Luogo>();
    }

        private void AddDocumento_Shown(object sender, EventArgs e)
        {
            this.PolpulateArchivi(-1);
            this.PolpulatePersonaggi();
            this.PolpulateLuoghi();
            this.PolpulateNote();
        }

        delegate void PolpulateArchiviCallBack(int id);
        private void PolpulateArchivi(int id)
        {
            if (this.InvokeRequired)
            {
                PolpulateArchiviCallBack d = new PolpulateArchiviCallBack(PolpulateArchivi);
                this.Invoke(d, new object[] {id});
            }
            else
            {
                this.ShowWait(true);

                this.comboBox1.Items.Clear();

                List<DataManager.Archivio> list = DataManager.getAllArchivi();

                foreach (DataManager.Archivio archivio in list)
                    this.comboBox1.Items.Add(archivio);

                if (id == -1)
                {
                    if (this.comboBox1.Items.Count > 0)
                        this.comboBox1.SelectedIndex = 0;
                }
                else
                {
                    for(int i=0;i<this.comboBox1.Items.Count;i++)
                    {
                        if (((DataManager.Archivio)this.comboBox1.Items[i]).ID==id)
                        {
                            this.comboBox1.SelectedIndex = i;
                            break;

                        }
                    }
                }
                this.ShowWait(false);
            }
        }

        delegate void PolpulatePersonaggiCallBack();
        private void PolpulatePersonaggi()
        {
            if (this.InvokeRequired)
            {
                PolpulatePersonaggiCallBack d = new PolpulatePersonaggiCallBack(PolpulatePersonaggi);
                this.Invoke(d, new object[] { });
            }
            else
            {
                this.ShowWait(true);

                string ret = "Nessun personaggio";
                int c = 0;

                foreach (DataManager.Personaggio p in this.Personaggi)
                {
                    if(c==0)
                        ret = ret + p.Nome;
                    else
                        ret = ret + ", "+p.Nome;
                }
                    
                this.label7.Text=ret;

                this.ShowWait(false);
            }
        }

        delegate void PolpulateLuoghiCallBack();
        private void PolpulateLuoghi()
        {
            if (this.InvokeRequired)
            {
                PolpulateLuoghiCallBack d = new PolpulateLuoghiCallBack(PolpulateLuoghi);
                this.Invoke(d, new object[] { });
            }
            else
            {
                this.ShowWait(true);

                string ret = "Nessun luogo";
                int c = 0;

                foreach (DataManager.Luogo l in this.Luoghi)
                {
                    if (c == 0)
                        ret = ret + l.Nome;
                    else
                        ret = ret + ", " + l.Nome;
                }

                this.label9.Text = ret;

                this.ShowWait(false);
            }
        }

        delegate void PolpulateNoteCallBack();
        private void PolpulateNote()
        {
            if (this.InvokeRequired)
            {
                PolpulateNoteCallBack d = new PolpulateNoteCallBack(PolpulateNote);
                this.Invoke(d, new object[] { });
            }
            else
            {
                this.ShowWait(true);

                if(this.Note=="" && this.Link=="")
                {
                    this.label11.Text = "Nessuna nota";
                }
                else
                {
                    if (this.Note == "")
                        this.label11.Text = this.Link;
                    else
                        this.label11.Text = this.Note;
                }

                this.ShowWait(false);
            }
        }

        delegate void ShowWaitCallBack(bool visible);
        private void ShowWait(bool visible)
        {
            if (this.InvokeRequired)
            {
                ShowWaitCallBack d = new ShowWaitCallBack(ShowWait);
                this.Invoke(d, new object[] { visible});
            }
            else
            {
                if (visible)
                {
                    this.progressIndicator1.Visible = true;
                    this.progressIndicator1.Start();
                    this.label2.Text = "Aggiornamento delle informazioni...";
                    this.label2.Visible = true;
                }
                else
                {
                    this.progressIndicator1.Stop();
                    this.progressIndicator1.Visible = false;
                    this.label2.Visible = false;
                }
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            AddArchivio frm = new AddArchivio();
            frm.onCloseForm += (so, ea) =>
            {
                if(!ea.Cancel)
                    PolpulateArchivi(ea.ID);
            };
            frm.ShowDialog(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
