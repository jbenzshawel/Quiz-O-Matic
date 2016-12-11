using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Npgsql; 
using ApiQuizGenerator.AppClasses; 
using ApiQuizGenerator.Models;

namespace ApiQuizGenerator.DAL
{
    public class DbContext 
    {
        public Quiz Quizes { get; set; }

        public QuizType QuizTypes { get; set; }

        public Question Questions { get; set; }

        public Answer Answers { get; set; }
    }

    public class Repository<T> where T: class
    {
        private string _connection 
        {
            get 
            {
                return "Host=localhost;Database=QuizGenerator;Username=QuizGeneratorAdmin;Password=localdbpw";
            }
        } 

        private Dictionary<string, string> _ListObjects { get; set; }

        public Repository() 
        {
            this._ListObjects = new Dictionary<string, string> 
            {
                { "Quizes", "list_quizes" }
            };
        }

        public List<T> All() 
        {
            var pgFunction = string.Empty;
            
            if (typeof(T) == typeof(Quiz)) 
            {
                var quiz = new Quiz(); 
                pgFunction = this._ListObjects[quiz.Definition];
            }

            if (string.IsNullOrEmpty(pgFunction)) 
            {
                throw new System.InvalidOperationException("Could not find pg function in _ListObjects. Model class not supported");
            }

            return GetDataList(pgFunction);;
        }

        private List<T> GetDataList(string command, List<NpgsqlParameter> paramz = null) 
        {
            List<T> allObjects = new List<T>(); 
            
            using (var pgCon = new NpgsqlConnection(this._connection)) 
            {
                pgCon.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = pgCon;
                    // Insert some data
                    cmd.CommandText = command;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            T listObj = reader.ToObject<T>();
                            if (listObj != null)
                                allObjects.Add(listObj);
                        }
                    } // end using SqlDataReader 
                } // end using NpgsqlCommand 
            } // end using NpgsqlConnection 

            return allObjects;
        }
    }
}