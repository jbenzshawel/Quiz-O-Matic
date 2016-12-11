using System;
using System.Collections.Generic;
using System.Reflection;
using Npgsql; 
using ApiQuizGenerator.Models;

namespace ApiQuizGenerator.AppClasses 
{
    public static class Extensions 
    {
        public static T ToObject<T>(this NpgsqlDataReader @this) where T : class
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
                };

                objectCast = quiz as T;
            }

            return objectCast;
        }

        public static T GetObject<T>() where T : new()
        {
            return new T();
        }
    }
}
