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

        Task<T> Get(string id);

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

        /// <summary>
        /// Get a List&lt;T&gt; of Quiz or Question objects. If getting a list of
        /// questions id param is required and is the Guid QuizId corresponding to
        /// questions
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<T>> All(string id = null)
        {
            PgSqlObject pgSqlObject = _DataHelper.ListProcedures.GetValOr(typeof (T));
            var paramz = new List<NpgsqlParameter>();

            if (pgSqlObject != null && pgSqlObject.Parameters != null)
            {
                pgSqlObject.Parameters[0].Value = id;
                paramz.Add(pgSqlObject.Parameters[0]);
            }
            
            return await _DataHelper.GetDataList<T>(pgSqlObject.PgFunction, paramz);
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
            PgSqlObject pgSqlObject = _DataHelper.GetProcedures.GetValOr(typeof (T));
                    
            if (string.IsNullOrEmpty(id) || pgSqlObject == null)
            {
                return null;
            }
            
            if (pgSqlObject.Parameters != null) 
            {
                pgSqlObject.Parameters[0].Value = id;
            }

            return await _DataHelper.GetObject<T>(pgSqlObject.PgFunction, pgSqlObject.Parameters[0]);
        }

        public async Task<bool> Save(T obj)
        {
            var pgFunction = string.Empty;
            var paramz = new List<NpgsqlParameter>();

            if (obj != null && typeof (T) == typeof (Quiz)) 
            {
                var quiz = obj as Quiz;
                pgFunction = _DataHelper.SaveProcedures[Quiz.Definition];
                paramz = new List<NpgsqlParameter> 
                {
                    _DataHelper.NpgParam(NpgsqlDbType.Text, "p_name", quiz.Name),
                    _DataHelper.NpgParam(NpgsqlDbType.Text, "p_description", quiz.Description),
                    _DataHelper.NpgParam(NpgsqlDbType.Integer, "p_type_id", quiz.TypeId),
                    _DataHelper.NpgParam(NpgsqlDbType.Uuid, "p_quiz_id", quiz.Id)
                };
            }
            else if (obj != null && typeof (T) == typeof (Question)) 
            {
                var question = obj as Question;
                pgFunction = _DataHelper.SaveProcedures[Question.Definition];
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
                pgFunction = _DataHelper.DeleteProcedures[Quiz.Definition];
                paramz.Add(_DataHelper.NpgParam(NpgsqlDbType.Uuid, "p_quiz_id", quiz.Id));
            }
            else if (obj != null && typeof (T) == typeof (Question))
            {
                var question = obj as Question;
                pgFunction = _DataHelper.DeleteProcedures[Question.Definition];
                paramz.Add(_DataHelper.NpgParam(NpgsqlDbType.Integer, "p_question_id", question.Id));
            }

            return await _DataHelper.ExecuteNonQuey(pgFunction, paramz);
        }
    }
}