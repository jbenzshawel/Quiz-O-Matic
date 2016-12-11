using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
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

        public Dictionary<string, string> SaveObjects { get; set; }

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

            this.SaveObjects = new Dictionary<string, string>
            {
                { "Quizes", "save_quiz" }
            };
        }

        public NpgsqlParameter NpgParam(NpgsqlDbType dbType, string paramName, object val) 
        {
            var param = new NpgsqlParameter(paramName, dbType);
            param.Value = val;
            return param;
        }

        public async Task<bool> ExecuteNonQuey(string command, List<NpgsqlParameter> paramz = null) 
        {
            int rowsAffected = 0;

            using (var pgCon = new NpgsqlConnection(this._connection)) 
            {
                pgCon.Open();
                
                try 
                {
                    NpgsqlCommand cmd = _GetSqlCommand(pgCon, command, paramz);
                    rowsAffected = await cmd.ExecuteNonQueryAsync(); 
                }
                catch (Exception ex) 
                {
                    throw ex;
                }
                finally
                {
                    if (pgCon.State == ConnectionState.Open)
                        pgCon.Close();
                }
            } // end using NpgsqlConnection

            // postgresql doesn't support rows affected (as far as I could tell)
            return rowsAffected == -1; 
        }
        
        public async Task <T> GetDataRow<T>(string command, List<NpgsqlParameter> paramz = null) 
            where T : class
        {
            T obj = null; 

            var result = await GetDataList<T>(command, paramz);
            if (result.Any()) 
            {
                obj = result[0];
            }

            return obj;
        }

        public async Task<List<T>> GetDataList<T>(string command, List<NpgsqlParameter> paramz = null) 
            where T : class
        {
            // list of generics for return
            List<T> allObjects = new List<T>(); 
            
            using (var pgCon = new NpgsqlConnection(this._connection)) 
            {
                pgCon.Open();
                
                try 
                {
                    NpgsqlCommand cmd = _GetSqlCommand(pgCon, command, paramz);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            T listObj = reader.ToObject<T>();
                            if (listObj != null)
                                allObjects.Add(listObj);
                        }
                    } // end using SqlDataReader 
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

        private NpgsqlCommand _GetSqlCommand(NpgsqlConnection connection, string command, List<NpgsqlParameter> paramz = null)
        {
            var cmd = new NpgsqlCommand();
            cmd.Connection = connection;
            
            // pass in the inputed sql command or stored procedure 
            cmd.CommandText = command;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            // if there are any parameters add them
            if (paramz != null && paramz.Any()) 
            {
                foreach (NpgsqlParameter param in paramz)
                    cmd.Parameters.Add(param);
            }

            return cmd;
        
        }
    }
}
