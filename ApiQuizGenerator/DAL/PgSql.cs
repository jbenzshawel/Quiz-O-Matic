using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using ApiQuizGenerator.Models;

namespace ApiQuizGenerator.AppClasses
{
    public class PgSqlObject
    {
        public string PgFunction { get; set; }

        public NpgsqlParameter[] Parameters { get; set; } 
    }
    
    /// <summary>
    /// Contains methods for retrieving data from a PostgreSql database. All Sql calls
    /// are done through stored procedures called out in the objects properties
    /// </summary>
    public class PgSql
    {
        
        /// <summary>
        /// PostgreSql Connection String
        /// </summary>
        /// <returns></returns>
        private string _connection 
        {
            get 
            {
                return "Host=localhost;Database=QuizGenerator;Username=QuizGeneratorAdmin;Password=localdbpw";
            }
        } 
        
        #region Object procedure Dictionary properties

        /// <summary>
        /// Dictionary with key of Table Name and value of stored procedure name to retrive a 
        /// list of objects from the key Table Name
        /// </summary>
        private Dictionary<Type, PgSqlObject> _listProcedures = null;
        public Dictionary<Type, PgSqlObject> ListProcedures 
        { 
            get 
            {
                if (_listProcedures == null)
                {
                    _listProcedures =  new Dictionary<Type, PgSqlObject> 
                    {
                        { typeof (Quiz), new PgSqlObject { PgFunction = "list_quizes" } },
                        { 
                            typeof (Question), new PgSqlObject 
                            {
                                PgFunction = "list_questions",
                                Parameters = new NpgsqlParameter[] { NpgParam(NpgsqlDbType.Uuid, "p_quiz_id") }
                            }
                        },
                        { 
                            typeof (Answer), new PgSqlObject 
                            {
                                PgFunction = "list_answers",
                                Parameters = new NpgsqlParameter[] { NpgParam(NpgsqlDbType.Uuid, "p_quiz_id") }
                            }
                        },
                        { 
                            typeof (Response), new PgSqlObject 
                            {
                                PgFunction = "list_responses",
                                Parameters = new NpgsqlParameter[] { NpgParam(NpgsqlDbType.Uuid, "p_quiz_id") }
                            }
                        }
                    };
                } // end if _listProcedures == null
                                    
                return _listProcedures;
            }
        }
        
        /// <summary>
        /// Dictionary with key of Table Name and value of stored procedure name to retrive a 
        /// single  object from the key Table Name
        /// </summary>
        private Dictionary<Type, PgSqlObject> _getProcedures = null;
        public Dictionary<Type, PgSqlObject> GetProcedures 
        { 
            get 
            {
                if (_getProcedures == null) 
                {
                    _getProcedures = new Dictionary<Type, PgSqlObject> 
                    {
                        { 
                            typeof(Quiz),  new PgSqlObject 
                            {
                                PgFunction = "get_quiz_by_id",
                                Parameters = new NpgsqlParameter[] { NpgParam(NpgsqlDbType.Uuid, "p_quiz_id") }
                            } 
                        },
                        { 
                            typeof(Question), new PgSqlObject 
                            {
                                PgFunction =  "get_question_by_id",
                                Parameters = new NpgsqlParameter[] { NpgParam(NpgsqlDbType.Integer, "p_question_id") }
                            } 
                        },
                        { 
                            typeof(Answer), new PgSqlObject 
                            {
                                PgFunction =  "get_answer_by_id",
                                Parameters = new NpgsqlParameter[] { NpgParam(NpgsqlDbType.Integer, "p_answer_id") }
                            } 
                        },
                        { 
                            typeof(Response), new PgSqlObject 
                            {
                                PgFunction =  "get_response_by_id",
                                Parameters = new NpgsqlParameter[] { NpgParam(NpgsqlDbType.Integer, "p_response_id") }
                            } 
                        }                
                    };
                } // end if _getProcedures == null
                    

                return _getProcedures;
            }
        }

        /// <summary>
        /// Dictionary with key of Table Name and value of stored procedure name to save (upsert)
        /// object in the key Table Name
        /// </summary>
        private Dictionary<Type, PgSqlObject> _saveProcedures = null;
        public Dictionary<Type, PgSqlObject> SaveProcedures 
        { 
            get 
            {
                if (_saveProcedures == null)
                {
                    _saveProcedures = new Dictionary<Type, PgSqlObject>
                    {
                        { 
                            typeof (Quiz), new PgSqlObject 
                            {
                                PgFunction = "save_quiz",
                                Parameters = new NpgsqlParameter[] 
                                {
                                    NpgParam(NpgsqlDbType.Text, "p_name"),
                                    NpgParam(NpgsqlDbType.Text, "p_description"),
                                    NpgParam(NpgsqlDbType.Integer, "p_type_id"),
                                    NpgParam(NpgsqlDbType.Uuid, "p_quiz_id")
                                }
                            } 
                        },
                        { 
                            typeof (Question), new PgSqlObject
                            {
                                PgFunction ="save_question",
                                Parameters = new NpgsqlParameter[]
                                {
                                    NpgParam(NpgsqlDbType.Text, "p_title"),
                                    NpgParam(NpgsqlDbType.Text, "p_attributes"),
                                    NpgParam(NpgsqlDbType.Uuid, "p_quiz_id"),    
                                    NpgParam(NpgsqlDbType.Integer, "p_question_id")        
                                }
                            }
                        },
                        { 
                            typeof (Answer), new PgSqlObject
                            {
                                PgFunction ="save_answer",
                                Parameters = new NpgsqlParameter[]
                                {
                                    NpgParam(NpgsqlDbType.Integer, "p_answer_id"),
                                    NpgParam(NpgsqlDbType.Integer, "p_question_id"),
                                    NpgParam(NpgsqlDbType.Text, "p_content"),    
                                    NpgParam(NpgsqlDbType.Text, "p_identifier"),   
                                    NpgParam(NpgsqlDbType.Text, "p_attributes")
                                }
                            }
                        },
                        { 
                            typeof (Response), new PgSqlObject
                            {
                                PgFunction ="save_response",
                                Parameters = new NpgsqlParameter[]
                                {
                                    NpgParam(NpgsqlDbType.Integer, "p_response_id"),
                                    NpgParam(NpgsqlDbType.Uuid, "p_quiz_id"),
                                    NpgParam(NpgsqlDbType.Integer, "p_question_id"),    
                                    NpgParam(NpgsqlDbType.Text, "p_value")
                                }
                            }
                        }
                    };
                } // end if _saveProcedures == null
                    
                
                return _saveProcedures;
            }
        }

        /// <summary>
        /// Dictionary with key of Table Name and value of stored procedure name to delete a 
        /// single object from the key Table Name
        /// </summary>
        private Dictionary<Type, PgSqlObject> _deleteProcedures = null;
        public Dictionary<Type, PgSqlObject> DeleteProcedures 
        { 
            get
            {
                if (_deleteProcedures == null)
                {
                    _deleteProcedures = new Dictionary<Type, PgSqlObject>
                    {
                        { 
                            typeof (Quiz), new PgSqlObject 
                            {
                                PgFunction =  "delete_quiz",
                                Parameters = new NpgsqlParameter[] { NpgParam(NpgsqlDbType.Uuid, "p_quiz_id") }
                            }
                        },
                        { 
                            typeof (Question), new PgSqlObject 
                            {
                                PgFunction = "delete_question",
                                Parameters = new NpgsqlParameter[] { NpgParam(NpgsqlDbType.Integer, "p_question_id")}
                            }
                        },
                        { 
                            typeof (Answer), new PgSqlObject 
                            {
                                PgFunction = "delete_answer",
                                Parameters = new NpgsqlParameter[] { NpgParam(NpgsqlDbType.Integer, "p_answer_id")}
                            }
                        },
                        { 
                            typeof (Response), new PgSqlObject 
                            {
                                PgFunction = "delete_response",
                                Parameters = new NpgsqlParameter[] { NpgParam(NpgsqlDbType.Integer, "p_response_id")}
                            }
                        }
                    };
                } // end if _deleteProcedures == null 

                return _deleteProcedures;
            }
        }

        #endregion

        /// <summary>
        /// Creates an NpgsqlParameter with type, name, and value passed in 
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public NpgsqlParameter NpgParam(NpgsqlDbType dbType, string paramName, object val = null) 
        {
            var param = new NpgsqlParameter(paramName, dbType);
            // add value if set 
            if (val != null)
                param.Value = val;

            return param;
        }

        /// <summary>
        /// Executes a nonquery sql statement and returns boolean if successful 
        /// </summary>
        /// <param name="command">stored procedure name</param>
        /// <param name="paramz">optional params</param>
        /// <returns></returns>
        public async Task<bool> ExecuteNonQuey(string command, List<NpgsqlParameter> paramz = null) 
        {
            int rowsAffected = 0;

            using (var pgCon = new NpgsqlConnection(this._connection)) 
            {
                pgCon.Open();
                
                try 
                {
                    NpgsqlCommand cmd = _NpgSqlCommand(pgCon, command, paramz);
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

            // postgresql / npgsql doesn't support rows affected (as far as I could tell)
            return rowsAffected == -1; 
        }
        
        /// <summary>
        /// Gets an object from T table corresponding to T id as sqlParam
        /// </summary>
        /// <param name="pgFunction"></param>
        /// <param name="sqlParam"></param>
        /// <returns></returns>
        public async Task<T> GetObject<T>(string pgFunction, NpgsqlParameter sqlParam)
            where T: class
        {
            T obj = null;

            var paramz = new List<NpgsqlParameter>();
            if (sqlParam != null)
            {
                paramz.Add(sqlParam);
                
                obj = await GetDataRow<T>(pgFunction, paramz);
            }
            
            return obj;
        }

        /// <summary>
        /// Gets a single row from the database and casts to class T
        /// </summary>
        /// <param name="pgFunction">stored procedure name</param>
        /// <param name="paramz">optional list of params</param>
        /// <returns></returns>
        public async Task<T> GetDataRow<T>(string pgFunction, List<NpgsqlParameter> paramz = null) 
            where T : class
        {
            T obj = null; 

            var result = await GetDataList<T>(pgFunction, paramz);
            if (result.Any()) 
            {
                obj = result[0];
            }

            return obj;
        }

        /// <summary>
        /// Retrieves a list of objects of type T from the database 
        /// </summary>
        /// <param name="pgFunction">stored procedure name</param>
        /// <param name="paramz">optional params</param>
        /// <returns></returns>
        public async Task<List<T>> GetDataList<T>(string pgFunction, List<NpgsqlParameter> paramz = null) 
            where T : class
        {
            // list of generics for return
            List<T> allObjects = new List<T>(); 
            
            using (var pgCon = new NpgsqlConnection(this._connection)) 
            {
                pgCon.Open();
                
                try 
                {
                    NpgsqlCommand cmd = _NpgSqlCommand(pgCon, pgFunction, paramz);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            T listObj = reader.ToModel<T>();
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

        /// <summary>
        /// Creates a NpgSqlCommand of type Stored Procedure with name and optional parameters
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="command"></param>
        /// <param name="paramz"></param>
        /// <returns></returns>
        private NpgsqlCommand _NpgSqlCommand(NpgsqlConnection connection, string command, List<NpgsqlParameter> paramz = null)
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
