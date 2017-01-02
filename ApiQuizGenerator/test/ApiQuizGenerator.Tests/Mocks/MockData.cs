using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ApiQuizGenerator.Tests.Mocks
{
    public class MockData
    {
        public static string String 
        { 
            get 
            { 
                return "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";
            }
        }

        /// <summary>
        /// A random integer between 1 and 100
        /// </summary>
        /// <returns></returns>
        public static int Integer 
        {
            get 
            {
                return RandomNumber(1, 100);
            }
        }

        /// <summary>
        /// A new Guid
        /// </summary>
        /// <returns></returns>
        public static Guid Guid 
        {
            get 
            {
                return Guid.NewGuid();
            }
        }

        /// <summary>
        /// A semi random DateTime in the future (now plus a random number of minutes)
        /// </summary>
        /// <returns></returns>
        public static DateTime DateTime
        {
            get 
            {
                return DateTime.Now.AddMinutes((double)Integer);
            }
        }

        /// <summary>
        /// Returns a random integer 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int RandomNumber(int min, int max)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            object syncLock = new object();
            lock(syncLock) { // synchronize
                return random.Next(min, max);
            }
        }

        /// <summary>
        /// Generates IQueryable&lt;T&gt; of mock objects with random data for testing 
        /// Note this relies a lot on reflection and is not very fast but since this is only used
        /// for testing for optimal performance isn't critical
        /// </summary>
        /// <param name="numberMocks"></param>
        /// <returns></returns>
        public static IQueryable<T> GetMockQueryable<T>(int numberMocks) 
            where T : class, new()
        {
            List<T> mockObjects = new List<T>();
            
            for (var i = 0; i < numberMocks; i++) 
            {
                mockObjects.Add(GetMockObj<T>());
            }

            return mockObjects.AsQueryable();
        }

        public static T GetMockObj<T>() where T : class, new()
        {
            T obj = new T(); 
            IEnumerable<PropertyInfo> objProperties = obj.GetType().GetTypeInfo().DeclaredProperties;

            // loop over the properties and set mock data for each prop
            foreach (PropertyInfo property in objProperties)
            {
                Type propType = property.PropertyType;

                if (propType == typeof (string))
                {
                    property.SetValue(obj, MockData.String, null);
                }
                else if (propType == typeof (Guid))
                {
                    property.SetValue(obj, MockData.Guid, null);
                }
                else if (propType == typeof (int))
                {
                    property.SetValue(obj, MockData.Integer, null);
                }
                else if (propType == typeof (DateTime))
                {
                property.SetValue(obj, MockData.DateTime, null);
                } 
                else if (propType == typeof (DateTime?))
                {
                property.SetValue(obj, (DateTime?)MockData.DateTime, null);
                }
            }
                
            object objCopy = new T(); 
            Extensions.CopyObjectData(obj, objCopy, string.Empty, BindingFlags.Public | BindingFlags.Instance);

            return objCopy as T;
         }
    }
}