using System;
using System.Collections.Generic;
using ApiQuizGenerator.Models;
using Npgsql;
using NpgsqlTypes;
using ApiQuizGenerator.AppClasses;
using System.Threading.Tasks;

namespace ApiQuizGenerator.DAL
{
    public interface IRepository<T> 
    {
        Task<List<T>> All();

        Task<T> Get(Guid Id);

        Task<bool> Save(T obj);
    }

    public class Repository<T> : IRepository<T> 
        where T : class
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

        public async Task<T> Get(Guid id) 
        {
            T obj = null;
            var pgFunction = string.Empty;
            NpgsqlParameter sqlParam = null;

            // handle Quiz case
            if (typeof(T) == typeof(Quiz)) 
            {
                pgFunction = this._DataHelper.GetObjects.GetValOr(Quiz.Definition);
                sqlParam = new NpgsqlParameter("p_quiz_id", NpgsqlTypes.NpgsqlDbType.Uuid);
                sqlParam.Value = id;
            }

            var paramz = new List<NpgsqlParameter>();
            if (sqlParam != null)
            {
                paramz.Add(sqlParam);
                
                obj = await this._DataHelper.GetDataRow<T>(pgFunction, paramz);
            }

            return obj;
        }

        public async Task<List<T>> All()
        {
            var pgFunction = string.Empty;

            if (typeof(T) == typeof(Quiz))
            {
                pgFunction = this._DataHelper.ListObjects.GetValOr(Quiz.Definition);
            }

            return await this._DataHelper.GetDataList<T>(pgFunction);
        }

        public async Task<bool> Save(T obj)
        {
            var pgFunction = string.Empty;
            var paramz = new List<NpgsqlParameter>();

            if (typeof (T) == typeof (Quiz) && obj != null) 
            {
                var quiz = obj as Quiz;
                pgFunction = this._DataHelper.SaveObjects.GetValOr(Quiz.Definition);
                paramz = new List<NpgsqlParameter> 
                {
                    this._DataHelper.NpgParam(NpgsqlDbType.Text, "p_name", quiz.Name),
                    this._DataHelper.NpgParam(NpgsqlDbType.Text, "p_description", quiz.Description),
                    this._DataHelper.NpgParam(NpgsqlDbType.Integer, "p_type_id", quiz.TypeId),
                    this._DataHelper.NpgParam(NpgsqlDbType.Uuid, "p_quiz_id", quiz.Id)
                };
            }

            return await this._DataHelper.ExecuteNonQuey(pgFunction, paramz);
        }

    }
}