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
        Task<List<T>> All(string id = null);

        Task<T> Get(Guid Id);

        Task<bool> Save(T obj);

        Task<bool> Delete(T obj);
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
                pgFunction = _DataHelper.GetObjects.GetValOr(Question.Definition);
                sqlParam = _DataHelper.NpgParam(NpgsqlDbType.Integer, "p_question_id", id);
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
                sqlParam = _DataHelper.NpgParam(NpgsqlDbType.Uuid, "p_quiz_id", id);
            }

            obj = await _DataHelper.GetObject<T>(pgFunction, sqlParam);

            return obj;
        }

        public async Task<List<T>> All(string id = null)
        {
            var pgFunction = string.Empty;
            var paramz = new List<NpgsqlParameter>();

            if (typeof (T) == typeof (Quiz))
            {
                pgFunction = _DataHelper.ListObjects.GetValOr(Quiz.Definition);
            }

            if (typeof (T) == typeof (Question)) 
            {
                Guid quizId;
                if (Guid.TryParse(id, out quizId)) 
                {
                    pgFunction = _DataHelper.ListObjects.GetValOr(Question.Definition);
                    paramz.Add(_DataHelper.NpgParam(NpgsqlDbType.Uuid, "p_quiz_id", quizId));
                }
            }

            return await _DataHelper.GetDataList<T>(pgFunction, paramz);
        }

        public async Task<bool> Save(T obj)
        {
            var pgFunction = string.Empty;
            var paramz = new List<NpgsqlParameter>();

            if (obj != null && typeof (T) == typeof (Quiz)) 
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

            if (obj != null && typeof (T) == typeof (Question)) 
            {
                var question = obj as Question;
                pgFunction = _DataHelper.SaveObjects.GetValOr(Question.Definition);
                paramz = new List<NpgsqlParameter>
                {
                    _DataHelper.NpgParam(NpgsqlDbType.Text, "p_title", question.Title),
                    _DataHelper.NpgParam(NpgsqlDbType.Text, "p_attributes", question.Attributes),
                    _DataHelper.NpgParam(NpgsqlDbType.Uuid, "p_quiz_id", question.QuizId),     
                    _DataHelper.NpgParam(NpgsqlDbType.Integer, "p_question_id", question.Id)               
                };
            }

            return await _DataHelper.ExecuteNonQuey(pgFunction, paramz);
        }

        public async Task<bool> Delete(T obj)
        {
            var pgFunction = string.Empty;
            var paramz = new List<NpgsqlParameter>();

            if (obj != null && typeof (T) == typeof (Quiz)) 
            {
                var quiz = obj as Quiz;
                pgFunction = _DataHelper.DeleteObjects.GetValOr(Quiz.Definition);
                paramz.Add(_DataHelper.NpgParam(NpgsqlDbType.Uuid, "p_quiz_id", quiz.Id));
            }

            return await _DataHelper.ExecuteNonQuey(pgFunction, paramz);
        }
    }
}