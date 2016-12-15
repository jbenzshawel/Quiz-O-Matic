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
                pgSqlObject = _PgSql.SaveProcedures.GetValOr(typeof (T));
                List<NpgsqlParameter> procedureParams = pgSqlObject.Parameters.ToList<NpgsqlParameter>(); 
                
                foreach(var property in objProperties)
                {
                    var prop = property.GetCustomAttribute(typeof(ColumnName), false) as ColumnName;
                    string columnName = prop !=null ? prop.AttributeValue : null;
                    // get column name attribute from property
                    if (!string.IsNullOrEmpty(columnName) && 
                    columnName.Replace("_", "").Equals(property.Name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        // find the parameter that matches the column name
                        var sqlParam = procedureParams.FirstOrDefault(p => p.ParameterName == ("p_" + columnName));
                        // add the param to the list of parameters
                        if (sqlParam != null && sqlParam.Value == null && !paramz.Contains(sqlParam)) 
                        {
                            sqlParam.Value = property.GetValue(obj, null);
                            paramz.Add(sqlParam);
                        }
                    }
                }
            }

            return await _PgSql.ExecuteNonQuey(pgSqlObject.PgFunction, paramz);
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