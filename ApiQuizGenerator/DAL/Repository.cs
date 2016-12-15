using System.Reflection;
using System.Collections.Generic;
using ApiQuizGenerator.Models;
using Npgsql;
using NpgsqlTypes;
using ApiQuizGenerator.AppClasses;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace ApiQuizGenerator.DAL
{
    public interface IRepository<T> 
    {
        Task<List<T>> All(string id = null);

        Task<T> Get(string id);

        Task<bool> Save(T obj);

        Task<bool> Delete(string id);
    }

    public class Repository<T> : IRepository<T> 
        where T : class
    {

        private PgSql _PgSql { get; set; }

        public Repository()
        {
            _PgSql = new PgSql();
        }

        public Repository(PgSql _dataHelper)
        {
            _PgSql = _dataHelper;
        }

        /// <summary>
        /// Get a List&lt;T&gt; of Quiz or Question objects. If getting a list of
        /// questions id param is required and is the Guid QuizId corresponding to
        /// questions
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<T>> All(string id = null)
        {
            List<T> allObjects = new List<T>();
            PgSqlObject pgSqlObject = _PgSql.ListProcedures.GetValOr(typeof (T));
            var paramz = new List<NpgsqlParameter>();

            if (pgSqlObject != null)
            {
                // id param only needed for list questions / answers
                if (pgSqlObject.Parameters != null)
                {
                    pgSqlObject.Parameters[0].Value = id;
                    paramz.Add(pgSqlObject.Parameters[0]);
                }
                allObjects = await _PgSql.GetDataList<T>(pgSqlObject.PgFunction, paramz);                
            }

            return allObjects;
        }

        /// <summary>
        /// Get Quiz or Question by id (string id parsed to integer or Guid depending on
        /// Type T)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> Get(string id) 
        {
            T obj = null;
            PgSqlObject pgSqlObject = _PgSql.GetProcedures.GetValOr(typeof (T));
                    
            if (!string.IsNullOrEmpty(id) && pgSqlObject != null && pgSqlObject.Parameters != null)
            {
                pgSqlObject.Parameters[0].Value = id;
                obj = await _PgSql.GetObject<T>(pgSqlObject.PgFunction, pgSqlObject.Parameters[0]);
            }
            
            return obj;
        }

        public async Task<bool> Save(T obj)
        {
            PgSqlObject pgSqlObject = null;
            var paramz = new List<NpgsqlParameter>();

            if (obj != null) 
            {
                IEnumerable<PropertyInfo> objProperties = obj.GetType().GetTypeInfo().DeclaredProperties;
                List<NpgsqlParameter> procedureParams = pgSqlObject.Parameters.ToList<NpgsqlParameter>(); 
                
                foreach(var property in objProperties)
                {
                    ColumnName columnNameAttr = ((object[])property.GetCustomAttributes(typeof (string), false))[0] as ColumnName;
                    if (columnNameAttr != null)
                    {
                        var sqlParam = procedureParams.FirstOrDefault(p => p.ParameterName == ("p_" + columnNameAttr.AttributeValue));
                        if (sqlParam != null) 
                            paramz.Add(_PgSql.NpgParam(sqlParam.NpgsqlDbType, sqlParam.ParameterName, property.GetValue(obj, null)));
                    }
                }
            }

            // if (obj != null && typeof (T) == typeof (Quiz)) 
            // {
            //     var quiz = obj as Quiz;
            //     pgSqlObject = _PgSql.SaveProcedures.GetValOr(typeof (T));
                
            // }
            // else if (obj != null && typeof (T) == typeof (Question)) 
            // {
            //     var question = obj as Question;
            //     pgFunction = _PgSql.SaveProcedures[Question.Definition];
            //     paramz = new List<NpgsqlParameter>
            //     {
            //         _PgSql.NpgParam(NpgsqlDbType.Text, "p_title", question.Title),
            //         _PgSql.NpgParam(NpgsqlDbType.Text, "p_attributes", question.Attributes),
            //         _PgSql.NpgParam(NpgsqlDbType.Uuid, "p_quiz_id", question.QuizId),     
            //         _PgSql.NpgParam(NpgsqlDbType.Integer, "p_question_id", question.Id)               
            //     };
            // }

            return await _PgSql.ExecuteNonQuey(pgFunction, paramz);
        }

        public async Task<bool> Delete(string id)
        {
            bool status = false;
            PgSqlObject pgSqlObject = _PgSql.DeleteProcedures.GetValOr(typeof (T));
            var paramz = new List<NpgsqlParameter>();

            // delete procedures take one parameter which is id of object to delete
            if (!string.IsNullOrEmpty(id) && pgSqlObject != null && pgSqlObject.Parameters != null)
            {
                pgSqlObject.Parameters[0].Value = id;
                paramz.Add(pgSqlObject.Parameters[0]);

                status = await _PgSql.ExecuteNonQuey(pgSqlObject.PgFunction, paramz);            
            }

            return status;
        }
    }
}