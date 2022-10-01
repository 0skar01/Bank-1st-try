using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Bank
{
    public class DB
    {
        private static readonly string connstring = "Data Source = MR10611W10; Initial Catalog = dbBanksimulation; Integrated Security=true";
        private static SqlConnection SqlConnect()
        {
            SqlConnection con = new(connstring);
            con.Open();
            return con;
        }
        //public static readonly List<KeyValuePair<string, string>> SqlTables = new()
        //{
        //    new ("Bank", "name, Bankleitzahl"),
        //    new ("Girokonten", "dispositionkredit, GirokontenID"),
        //};
        private static readonly Dictionary<string, string> sqlTables = new() {
            {"Bank", "Banken,Bname,Bankleitzahl" },
            {"Konto", "konton,kontonummer,kontostand,bankid,kundenid" },
            {"GiroKonto", "Girokonten,dispositionskredit,kontoid" },
            {"Kunde", "kunden,Kname,vorname,password" },
            {"KundenBank", "kundenBank,kundenid,bankid" },
            {"SparKonten", "Sparkonten,zinssatz,kontoid" },
            {"Termingeldkonto", "Termingeldkonten,festerBetrag,zinssatz,Terminzinssatz,Jahren,kontoid" },
            {"Transaktion", "transaktionen,Betrag,einzahlen,Handelsdatum,kontoid" },
        };
        private static string[] GetTable(string table)
        {
            //List<string> columns = new();
            string value = sqlTables[table];
            char[] separator = { ',' };
            string[] columns = value.Split(separator);
            return columns;
        }
        private static void SqlDisconnect(SqlConnection con)
        {
            con.Close();
        }
        private static string PrepareInsertStatement(string table, string[] werte)
        {
            string[] Table = GetTable(table);
            string statement = $"insert into {Table[0]}(";
            for (int i = 1; i < Table.Length; i++)
            {
                statement += (i != Table.Length - 1) ? $"{Table[i]}, " : $"{Table[i]}";
            }
            statement += ") values(";
            switch (table)
            {
                case "Bank":
                case "Kunde":
                    statement += InsertWithoutSubqueries(Table, werte);
                    break;
                case "KundenBank":
                    string[] FKTables = { "kunden", "Banken" };               
                    statement += InsertWithSubqueries(FKTables, Table, werte);
                    break;
            }

            statement += ");";
            Console.WriteLine(statement);
            return statement;
        }
        private static string InsertWithSubqueries(string[] fkTables, string[] table, string[] werte)
        {
            string statement = "";
            int x = 0;
            for(int i = 0; i < werte.Length; i += 2)
            {
                statement += $"(select {table[x+1]} from {fkTables[x]} where {werte[i]} = '{werte[(i+1)]}')";
                statement += (i < werte.Length - 2)?", ":"";
                x++;
            }          
            return statement;
        }
        private static string InsertWithoutSubqueries(string[] table, string[] werte)
        {
            string statement = "";      
            for (int i = 0; i < werte.Length; i++)
            {
                statement += (i != werte.Length - 1) ? $"'{werte[i]}', " : $"'{werte[i]}'";
            }
            return statement;
        }
        public static void Insert(string table, string[] werte)
        {
            try
            {
                SqlConnection con = SqlConnect();               
                string statement = PrepareInsertStatement(table, werte);
                //Console.WriteLine(query);
                SqlCommand cmd = new(statement, con);
                int response = cmd.ExecuteNonQuery();
                if (response < 1)
                {
                    Console.WriteLine("failer");
                }
                else
                {
                    Console.WriteLine($"{response} row(s) were added to the {table} Table");
                }
                SqlDisconnect(con);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void Update(string table, string[] werte)
        {
            try
            {
                SqlConnection con = SqlConnect();
                string statement = PrepareUpdateStatement(table, werte);
                SqlCommand cmd = new(statement, con);
                int response = cmd.ExecuteNonQuery();
                if (response < 1)
                {
                    Console.WriteLine("failer");
                }
                else
                {
                    Console.WriteLine($"{response} row(s) were updated in the {table} Table");
                }
                SqlDisconnect(con);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static string PrepareUpdateStatement(string table, string[] werte)
        {
            string fromTable = GetTable(werte[0])[0];
            string statement = $"update {table} set (";
            statement += GetTablePK(sqlTables[fromTable]);
            statement += $") = (";

            return statement;
        }
        private static string GetTablePK(string table)
        {
            SqlConnection con = SqlConnect();
            string query = $"select C.COLUMN_NAME from INFORMATION_SCHEMA.TABLE_CONSTRAINTS T JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE C ON C.CONSTRAINT_NAME = T.CONSTRAINT_NAME where T.CONSTRAINT_TYPE = 'PRIMARY KEY' and T.TABLE_NAME = '{table}'";
            SqlCommand cmd = new(query, con);
            SqlDataReader reader = cmd.ExecuteReader();
            string PK = "";
            while (reader.Read())
            {
                PK = (string)reader.GetValue(0);
            }
            return PK;
        }
    }
}