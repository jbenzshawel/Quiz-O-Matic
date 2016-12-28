using System;
using System.Collections.Generic;
using ApiQuizGenerator.Models;
using Npgsql;
using NpgsqlTypes;

namespace ApiQuizGenerator.DAL
{
    /// <summary>
    /// Contains Dictionary&lt;Type, PgSqlFunction&gt; Propertis for getting List, Get, Save, and Delete 
    /// PgSql Function name by Type of Model (DB Table)
    /// </summary>
    public class PgSqlObjects
    {
        /// <summary>
        /// Dictionary with key of Model type and value of stored procedure name to retrive a 
        /// list of objects from the table associated with the Model type key
        /// </summary>
        private Dictionary<Type, PgSqlFunction> _listProcedures = null;
        public Dictionary<Type, PgSqlFunction> ListProcedures 
        { 
            get 
            {
                if (_listProcedures == null)
                {
                    _listProcedures =  new Dictionary<Type, PgSqlFunction> 
                    {
                        { typeof (Quiz), new PgSqlFunction { Name = "list_quizes" } },
                        { typeof (QuizType), new PgSqlFunction { Name = "list_quiz_types" } },                        
                        { 
                            typeof (Question), new PgSqlFunction 
                            {
                                Name = "list_questions",
                                Parameters = new NpgsqlParameter[] { PgSql.NpgParam(NpgsqlDbType.Uuid, "p_quiz_id") }
                            }
                        },
                        { 
                            typeof (Answer), new PgSqlFunction 
                            {
                                Name = "list_answers",
                                Parameters = new NpgsqlParameter[] { PgSql.NpgParam(NpgsqlDbType.Uuid, "p_quiz_id") }
                            }
                        },
                        { 
                            typeof (Response), new PgSqlFunction 
                            {
                                Name = "list_responses",
                                Parameters = new NpgsqlParameter[] { PgSql.NpgParam(NpgsqlDbType.Uuid, "p_quiz_id") }
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
        private Dictionary<Type, PgSqlFunction> _getProcedures = null;
        public Dictionary<Type, PgSqlFunction> GetProcedures 
        { 
            get 
            {
                if (_getProcedures == null) 
                {
                    _getProcedures = new Dictionary<Type, PgSqlFunction> 
                    {
                        { 
                            typeof(Quiz),  new PgSqlFunction 
                            {
                                Name = "get_quiz_by_id",
                                Parameters = new NpgsqlParameter[] { PgSql.NpgParam(NpgsqlDbType.Uuid, "p_quiz_id") }
                            } 
                        },
                        { 
                            typeof(Question), new PgSqlFunction 
                            {
                                Name =  "get_question_by_id",
                                Parameters = new NpgsqlParameter[] { PgSql.NpgParam(NpgsqlDbType.Integer, "p_question_id") }
                            } 
                        },
                        { 
                            typeof(Answer), new PgSqlFunction 
                            {
                                Name =  "get_answer_by_id",
                                Parameters = new NpgsqlParameter[] { PgSql.NpgParam(NpgsqlDbType.Integer, "p_answer_id") }
                            } 
                        },
                        { 
                            typeof(Response), new PgSqlFunction 
                            {
                                Name =  "get_response_by_id",
                                Parameters = new NpgsqlParameter[] { PgSql.NpgParam(NpgsqlDbType.Integer, "p_response_id") }
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
        private Dictionary<Type, PgSqlFunction> _saveProcedures = null;
        public Dictionary<Type, PgSqlFunction> SaveProcedures 
        { 
            get 
            {
                if (_saveProcedures == null)
                {
                    _saveProcedures = new Dictionary<Type, PgSqlFunction>
                    {
                        { 
                            typeof (Quiz), new PgSqlFunction 
                            {
                                Name = "save_quiz",
                                Parameters = new NpgsqlParameter[] 
                                {
                                    PgSql.NpgParam(NpgsqlDbType.Text, "p_name"),
                                    PgSql.NpgParam(NpgsqlDbType.Text, "p_description"),
                                    PgSql.NpgParam(NpgsqlDbType.Integer, "p_type_id"),
                                    PgSql.NpgParam(NpgsqlDbType.Uuid, "p_quiz_id")
                                }
                            } 
                        },
                        { 
                            typeof (Question), new PgSqlFunction
                            {
                                Name ="save_question",
                                Parameters = new NpgsqlParameter[]
                                {
                                    PgSql.NpgParam(NpgsqlDbType.Text, "p_title"),
                                    PgSql.NpgParam(NpgsqlDbType.Text, "p_attributes"),
                                    PgSql.NpgParam(NpgsqlDbType.Uuid, "p_quiz_id"),    
                                    PgSql.NpgParam(NpgsqlDbType.Integer, "p_question_id")        
                                }
                            }
                        },
                        { 
                            typeof (Answer), new PgSqlFunction
                            {
                                Name ="save_answer",
                                Parameters = new NpgsqlParameter[]
                                {
                                    PgSql.NpgParam(NpgsqlDbType.Integer, "p_answer_id"),
                                    PgSql.NpgParam(NpgsqlDbType.Integer, "p_question_id"),
                                    PgSql.NpgParam(NpgsqlDbType.Text, "p_content"),    
                                    PgSql.NpgParam(NpgsqlDbType.Text, "p_identifier"),   
                                    PgSql.NpgParam(NpgsqlDbType.Text, "p_attributes")
                                }
                            }
                        },
                        { 
                            typeof (Response), new PgSqlFunction
                            {
                                Name ="save_response",
                                Parameters = new NpgsqlParameter[]
                                {
                                    PgSql.NpgParam(NpgsqlDbType.Integer, "p_response_id"),
                                    PgSql.NpgParam(NpgsqlDbType.Uuid, "p_quiz_id"),
                                    PgSql.NpgParam(NpgsqlDbType.Integer, "p_question_id"),    
                                    PgSql.NpgParam(NpgsqlDbType.Text, "p_value")
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
        private Dictionary<Type, PgSqlFunction> _deleteProcedures = null;
        public Dictionary<Type, PgSqlFunction> DeleteProcedures 
        { 
            get
            {
                if (_deleteProcedures == null)
                {
                    _deleteProcedures = new Dictionary<Type, PgSqlFunction>
                    {
                        { 
                            typeof (Quiz), new PgSqlFunction 
                            {
                                Name =  "delete_quiz",
                                Parameters = new NpgsqlParameter[] { PgSql.NpgParam(NpgsqlDbType.Uuid, "p_quiz_id") }
                            }
                        },
                        { 
                            typeof (Question), new PgSqlFunction 
                            {
                                Name = "delete_question",
                                Parameters = new NpgsqlParameter[] { PgSql.NpgParam(NpgsqlDbType.Integer, "p_question_id")}
                            }
                        },
                        { 
                            typeof (Answer), new PgSqlFunction 
                            {
                                Name = "delete_answer",
                                Parameters = new NpgsqlParameter[] { PgSql.NpgParam(NpgsqlDbType.Integer, "p_answer_id")}
                            }
                        },
                        { 
                            typeof (Response), new PgSqlFunction 
                            {
                                Name = "delete_response",
                                Parameters = new NpgsqlParameter[] { PgSql.NpgParam(NpgsqlDbType.Integer, "p_response_id")}
                            }
                        }
                    };
                } // end if _deleteProcedures == null 

                return _deleteProcedures;
            }
        }
    }
}
