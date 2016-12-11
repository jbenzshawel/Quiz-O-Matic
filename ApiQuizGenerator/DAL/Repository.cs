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
            _DataHelper = new DataHelper();
        }

        public Repository(DataHelper _dataHelper)
        {
            _DataHelper = _dataHelper;
        }

        public async Task<T> Get(int id)
        {
            T obj = null;
            var pgFunction = string.Empty;
            NpgsqlParameter sqlParam = null;

            if (typeof (T) == typeof (Question)) 
            {
                pgFunction = this._DataHelper.GetObjects.GetValOr(Question.Definition);
                sqlParam = new NpgsqlParameter("p_question_id", NpgsqlTypes.NpgsqlDbType.Integer);
                sqlParam.Value = id;   
            }

            obj = await _DataHelper.GetObject<T>(pgFunction, sqlParam);

            return obj;
        }

        public async Task<T> Get(Guid id) 
        {
            T obj = null;
            var pgFunction = string.Empty;
            NpgsqlParameter sqlParam = null;

            if (typeof (T) == typeof( Quiz)) 
            {
                pgFunction = _DataHelper.GetObjects.GetValOr(Quiz.Definition);
                sqlParam = new NpgsqlParameter("p_quiz_id", NpgsqlTypes.NpgsqlDbType.Uuid);
                sqlParam.Value = id;
            }

            obj = await _DataHelper.GetObject<T>(pgFunction, sqlParam);

            return obj;
        }

        public async Task<List<T>> All()
        {
            var pgFunction = string.Empty;

            if (typeof(T) == typeof(Quiz))
            {
                pgFunction = _DataHelper.ListObjects.GetValOr(Quiz.Definition);
            }

            return await _DataHelper.GetDataList<T>(pgFunction);
        }

        public async Task<bool> Save(T obj)
        {
            var pgFunction = string.Empty;
            var paramz = new List<NpgsqlParameter>();

            if (typeof (T) == typeof (Quiz) && obj != null) 
            {
                var quiz = obj as Quiz;
                pgFunction = _DataHelper.SaveObjects.GetValOr(Quiz.Definition);
                paramz = new List<NpgsqlParameter> 
                {
                    _DataHelper.NpgParam(NpgsqlDbType.Text, "p_name", quiz.Name),
                    _DataHelper.NpgParam(NpgsqlDbType.Text, "p_description", quiz.Description),
                    _DataHelper.NpgParam(NpgsqlDbType.Integer, "p_type_id", quiz.TypeId),
                    _DataHelper.NpgParam(NpgsqlDbType.Uuid, "p_quiz_id", quiz.Id)
                };
            }

            return await _DataHelper.ExecuteNonQuey(pgFunction, paramz);
        }

    }
}