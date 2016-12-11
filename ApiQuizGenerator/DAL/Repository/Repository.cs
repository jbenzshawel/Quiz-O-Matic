using System;
using System.Collections.Generic;
using ApiQuizGenerator.Models;
using Npgsql;
using ApiQuizGenerator.AppClasses;

namespace ApiQuizGenerator.DAL
{
    public interface IRepository<T> 
    {
        List<T> All();

        T Get(Guid Id);

    }

    public class Repository<T> : IRepository<T> where T : class
    {
        private DataHelper _DataHelper { get; set; }

        public Repository()
        {
            this._DataHelper = new DataHelper();
        }

        public Repository(DataHelper dataHelper)
        {
            this._DataHelper = dataHelper;

        }

        public T Get(Guid id) 
        {
            T obj = null;
            var pgFunction = string.Empty;
            NpgsqlParameter sqlParam = null;

            // handle Quiz case
            if (typeof(T) == typeof(Quiz)) 
            {
                var quiz = new Quiz();
                pgFunction = this._DataHelper.GetObjects.GetValOr(quiz.Definition);
                sqlParam = new NpgsqlParameter("p_quiz_id", NpgsqlTypes.NpgsqlDbType.Uuid);
                sqlParam.Value = id;
            }

            var paramz = new List<NpgsqlParameter>();
            if (sqlParam != null)
            {
                paramz.Add(sqlParam);
                
                obj = this._DataHelper.GetDataRow<T>(pgFunction, paramz);
            }

            return obj;
        }

        public List<T> All()
        {
            var pgFunction = string.Empty;

            if (typeof(T) == typeof(Quiz))
            {
                var quiz = new Quiz();
                pgFunction = this._DataHelper.ListObjects.GetValOr(quiz.Definition);
            }

            if (string.IsNullOrEmpty(pgFunction))
            {
                throw new System.InvalidOperationException("Could not find pg function in ListObjects. Model class not supported");
            }

            return this._DataHelper.GetDataList<T>(pgFunction);
        }

    }
}