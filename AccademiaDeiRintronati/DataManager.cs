using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccademiaDeiRintronati
{
    public class DataManager
    {
        #region Oggetti
        public struct Archivio
        {
            public int ID;
            public string Nome;
            
            public Archivio(int i,string n)
            {
                this.ID = i;
                this.Nome = n;
            }

            public override string ToString()
            {
                return this.Nome;
            }
        }

        public struct Documento
        {
            public int ID;
            public DataSimply Data;
            public Archivio Archivio;
            public string RiferimentoArchivio;
            public string Regesto;
            public string Note;
            public string Link;
            public List<Personaggio> Personaggi;
            public List<Luogo> Luoghi;

        }

        public struct Personaggio
        {
            public int ID;
            public string Nome;
            public bool Maschio;
            public int Coniuge;
            public int Madre;
            public int Padre;
            public string Note;
        }

        public struct Luogo
        {
            public int ID;
            public string Nome;
            public float Lat;
            public float Lng;
        }
        #endregion

        public static List<Archivio> getAllArchivi()
        {
            string query = "SELECT * FROM ARCHIVI";

            string rdr = ResultQuery(query);

            List<Archivio> ret = new List<Archivio>();

            if (rdr == null)
                return null;

            if (rdr == "")
                return ret;

            JArray array = JArray.Parse(rdr);

            foreach (JObject o in array)
            {
                Archivio archivio = new Archivio((int)o["ID"], (string)o["NOME"]);
                ret.Add(archivio);
            }

            return ret;
        }

        public static int addArchivo(string nome)
        {
            string query = "INSERT INTO ARCHIVI(NOME) VALUES("+ FormatStringForSQL(nome)+");";
            query = query+"SELECT ID FROM ARCHIVI WHERE NOME="+ FormatStringForSQL(nome);

            string rdr = ResultQuery(query);

            int id = 0;

            try
            {
                JArray array = JArray.Parse(rdr);
                JToken o = array[0];

                id = (int)o["ID"];
            }
            catch(Exception ex)
            {
                id = -1;
            }
            return id;
        }


        public static List<Archivio> getArchiviByName(string nome)
        {
            string query = "SELECT * FROM ARCHIVI WHERE NOME="+ FormatStringForSQL(nome);

            string rdr = ResultQuery(query);

            List<Archivio> ret = new List<Archivio>();

            if (rdr == null)
                return null;

            if (rdr == "")
                return ret;

            JArray array=JArray.Parse(rdr);

            foreach(JObject o in array)
            {
                Archivio archivio = new Archivio((int)o["ID"], (string)o["NOME"]);
                ret.Add(archivio);
            }

            return ret;
        }

        private static string ResultQuery(string query)
        {
            try
            {
                string cs = @"URI=file:" + Utility.dbFile;

                using (var con = new SQLiteConnection(cs))
                {
                    con.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, con);
                    return Utility.ToJson(cmd.ExecuteReader());
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static bool NoResultQuery(string query)
        {
            try
            {
                string cs = @"URI=file:" + Utility.dbFile;

                using (var con = new SQLiteConnection(cs))
                {
                    con.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, con);
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static string FormatStringForSQL(string str)
        {
            return "'" + str.Replace("'", "''") + "'";
        }
    }
}
