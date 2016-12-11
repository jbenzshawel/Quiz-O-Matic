using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Npgsql;
using ApiQuizGenerator.AppClasses;

namespace ApiQuizGenerator.DAL
{
    public class DataHelper
    {

        private string _connection 
        {
            get 
            {
                return "Host=localhost;Database=QuizGenerator;Username=QuizGeneratorAdmin;Password=localdbpw";
            }
        } 
        
        public Dictionary<string, string> ListObjects { get; set; }
        
        public Dictionary<string, string> GetObjects { get; set; }

        public DataHelper() 
        {
            this.ListObjects = new Dictionary<string, string> 
            {
                { "Quizes", "list_quizes" }
            };

            this.GetObjects = new Dictionary<string, string> 
            {
                { "Quizes", "get_quiz_by_id" }
            };
        }

        public T GetDataRow<T>(string command, List<NpgsqlParameter> paramz = null) 
            where T : class
        {
            T obj = null; 

            var result = GetDataList<T>(command, paramz);
            if (result.Any()) 
            {
                obj = result[0];
            }

            return obj;
        }

        public List<T> GetDataList<T>(string command, List<NpgsqlParameter> paramz = null) 
            where T : class
        {
            // list of generics for return
            List<T> allObjects = new List<T>(); 
            
            using (var pgCon = new NpgsqlConnection(this._connection)) 
            {
                pgCon.Open();
                
                try 
                {
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = pgCon;
                        
                        // pass in the inputed sql command or stored procedure 
                        cmd.CommandText = command;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // if there are any parameters add them
                        if (paramz != null && paramz.Any()) 
                        {
                            foreach (NpgsqlParameter param in paramz)
                                cmd.Parameters.Add(param);
                        }

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                T listObj = reader.ToObject<T>();
                                if (listObj != null)
                                    allObjects.Add(listObj);
                            }
                        } // end using SqlDataReader 

                    } // end using NpgsqlCommand
                }
                catch (Exception ex) 
                {
                    // ToDo: Add logger
                    throw ex; 
                }
                finally 
                {
                    if (pgCon.State == ConnectionState.Open)
                        pgCon.Close();
                }
            } // end using NpgsqlConnection 

            return allObjects;
        }
    }
}