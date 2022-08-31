using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccademiaDeiRintronati
{
    class Utility
    {

        //Restituisce il nome dell'applicazione
        public static string appName
        {
            get
            {
                return "Accademia dei Rintronati";
            }
        }

        //Restituisce la directory in cui si trova l'eseguibile
        public static string appPath
        {
            get
            {
                return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);// + OSHelper.OSProperties.PathSep();
            }
        }

        //restituisce il percorso completo del file contenete la sorgente dati
        public static string dbFile
        {
            get
            {
                return Path.Combine(appPath, "database.sqllite");
            }
        }

        public static bool generateEmptyDB()
        {
            try
            {
                SQLiteConnection.CreateFile(dbFile);

                string cs = @"URI=file:" + dbFile;

                using (var con = new SQLiteConnection(cs))
                {
                    con.Open();

                    string query = "";
                    SQLiteCommand cmd;

                    //Creazione tabella PERSONAGGI
                    query = "CREATE TABLE PERSONAGGI(";
                    query = query + "	ID INTEGER PRIMARY KEY AUTOINCREMENT,";
                    query = query + "	NOME TEXT NOT NULL,";
                    query = query + "	SESSO TEXT NOT NULL,";
                    query = query + "	CONIUGE INTEGER,";
                    query = query + "	MADRE INTEGER,";
                    query = query + "	PADRE INTEGER,";
                    query = query + "	NOTE TEXT";
                    query = query + ")";

                    cmd = new SQLiteCommand(query, con);
                    cmd.ExecuteNonQuery();

                    //Creazione tabella DOCUMENTI
                    query = "CREATE TABLE DOCUMENTI(";
                    query = query + "	ID INTEGER PRIMARY KEY AUTOINCREMENT,";
                    query = query + "	DATA TEXT NOT NULL,";
                    query = query + "	REGESTO TEXT NOT NULL,";
                    query = query + "	ID_ARCHIVIO INTEGER NOT NULL,";
                    query = query + "	POSIZIONE_ARCHIVIO TEXT NOT NULL,";
                    query = query + "	LINK TEXT,";
                    query = query + "	NOTE TEXT";
                    query = query + ")";

                    cmd = new SQLiteCommand(query, con);
                    cmd.ExecuteNonQuery();

                    //Creazione tabella LUOGHI
                    query = "CREATE TABLE LUOGHI(";
                    query = query + "	ID INTEGER PRIMARY KEY AUTOINCREMENT,";
                    query = query + "	NOME TEXT NOT NULL,";
                    query = query + "	LAT REAL NOT NULL,";
                    query = query + "	LNG REAL NOT NULL";
                    query = query + ")";

                    cmd = new SQLiteCommand(query, con);
                    cmd.ExecuteNonQuery();

                    //Creazione tabella ARCHIVI
                    query = "CREATE TABLE ARCHIVI(";
                    query = query + "	ID INTEGER PRIMARY KEY AUTOINCREMENT,";
                    query = query + "	NOME TEXT NOT NULL";
                    query = query + ")";

                    cmd = new SQLiteCommand(query, con);
                    cmd.ExecuteNonQuery();

                    //Creazione tabella LINK_LUOGHI
                    query = "CREATE TABLE LINK_LUOGHI(";
                    query = query + "	ID_DOCUMENTO INTEGER NOT NULL,";
                    query = query + "	ID_LUOGO INTEGER NOT NULL";
                    query = query + ")";

                    cmd = new SQLiteCommand(query, con);
                    cmd.ExecuteNonQuery();

                    //Creazione tabella LINK_PRSONAGGI
                    query = "CREATE TABLE LINK_PRSONAGGI(";
                    query = query + "	ID_DOCUMENTO INTEGER NOT NULL,";
                    query = query + "	ID_PERSONAGGIO INTEGER NOT NULL";
                    query = query + ")";

                    cmd = new SQLiteCommand(query, con);
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        //Converte un IDataReader in un flusso in formato JSON
        public static string ToJson(IDataReader rdr)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.WriteStartArray();

                int c = 0;

                while (rdr.Read())
                {
                    jsonWriter.WriteStartObject();

                    int fields = rdr.FieldCount;

                    for (int i = 0; i < fields; i++)
                    {
                        jsonWriter.WritePropertyName(rdr.GetName(i));
                        jsonWriter.WriteValue(rdr[i]);

                    }

                    jsonWriter.WriteEndObject();

                    c = c + 1;
                }

                jsonWriter.WriteEndArray();

                if (c > 0)
                    return sw.ToString();

                return "";

            }
        }
    }
}
