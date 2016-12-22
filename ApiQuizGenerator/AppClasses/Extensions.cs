using System;
using System.Collections.Generic;
using ApiQuizGenerator.Models;
using System.Data.Common;

namespace ApiQuizGenerator.AppClasses
{
    public static class Extensions 
    {
        /// <summary>
        /// Gets value from Dictionary if it exists else returns null or optional orVal
        /// </summary>
        /// <param name="@this"></param>
        /// <param name="key"></param>
        /// <param name="orVal"></param>
        /// <returns></returns>
        public static PgSqlObject GetValOr(this Dictionary<Type, PgSqlObject> @this, Type key, PgSqlObject orVal = null)
        {
            return @this.ContainsKey(key) ? @this[key] : orVal;
        }
        
        /// <summary>
        /// Casts a DbDataReader object to a Quiz or Question object
        /// </summary>
        /// <param name="@this">reader object</param>
        public static T ToModel<T>(this DbDataReader @this) where T : class
        {
            T objectCast = null;

            // return early if no data 
            if (!@this.HasRows || @this.FieldCount == 0)
                return objectCast;

            // ToDo: use GetColumnSchema for generic mapping
            // map NpgsqlDataReader to Quiz type
            if (typeof (T) == typeof (Quiz)) 
            {
                var quiz = new Quiz 
                {
                    Id = Guid.Parse(@this["quiz_id"].ToString()),
                    Name = @this["name"].ToString(),
                    Description = @this["description"].ToString(),
                    Type = @this["type"] != DBNull.Value ? @this["type"].ToString() : null,
                    TypeId = @this["type_id"] != DBNull.Value ? Int32.Parse(@this["type_id"].ToString()) : 0,
                    Created = @this["created"] != DBNull.Value ? DateTime.Parse(@this["created"].ToString()) : DateTime.MinValue,
                    Updated = @this["updated"] != DBNull.Value ? (DateTime?)DateTime.Parse(@this["updated"].ToString()) : null              
                };

                objectCast = quiz as T;
            }

            if (typeof (T) == typeof (Question))
            {
                var question = new Question
                {
                    Id = Int32.Parse(@this["question_id"].ToString()),
                    Title = @this["title"].ToString(),
                    Attributes = @this["attributes"].ToString(),
                    QuizId = Guid.Parse(@this["quiz_id"].ToString())
                };

                objectCast = question as T;
            }

            if (typeof (T) == typeof (Answer))
            {
                var answer = new Answer
                {
                    Id = Int32.Parse(@this["answer_id"].ToString()),
                    Content = @this["content"].ToString(),
                    Identifier = @this["identifier"].ToString(),
                    Attributes = @this["attributes"].ToString(),
                    QuestionId = Int32.Parse(@this["question_id"].ToString())
                };
                
                objectCast = answer as T;   
            }

            if (typeof (T) == typeof (Response))
            {
                var response = new Response
                {
                    Id = Int32.Parse(@this["response_id"].ToString()),
                    QuizId = Guid.Parse(@this["quiz_id"].ToString()),
                    QuestionId = Int32.Parse(@this["question_id"].ToString()),
                    Value = @this["value"].ToString(),
                    Created = DateTime.Parse(@this["created"].ToString())
                };
                
                objectCast = response as T;   
            }

            return objectCast;
        }

        public static T GetObject<T>() where T : new()
        {
            return new T();
        }
    }
}
