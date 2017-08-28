using System;
using System.Collections.Generic;
using ApiQuizGenerator.Models;
using System.Data.Common;
using Npgsql;
using System.Linq;
using ApiQuizGenerator.DAL;
using System.Reflection;

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
        public static PgSqlFunction GetValOr(this Dictionary<Type, PgSqlFunction> @this, Type key, PgSqlFunction orVal = null)
        {
            return @this.ContainsKey(key) ? @this[key] : orVal;
        }

        /// <summary>
        /// Casts an object to an integer or returns orVal if cast unsuccessful 
        /// </summary>
        /// <param name="@this"></param>
        /// <param name="orVal"></param>
        /// <returns></returns>
        public static int ToIntOr(this object @this, int orVal = 0)
        {
            int castInt;

            if (int.TryParse(@this.ToString(), out castInt))
                return castInt;
        
            return orVal; 
        }

        /// <summary>
        /// Cast array of NpgsqlParameter objects to a List of NpgsqlParameter objects. If @this
        /// is null null is returned. 
        /// Note if a NpgsqlParameter has a null value it is removed from the list by default. Set
        /// optional param removeNulls to false if you would like to keep null values in the returned list
        /// </summary>
        /// <param name="@this"></param>
        /// <returns></returns>
        public static List<NpgsqlParameter> ToListOrNull(this NpgsqlParameter[] @this, bool removeNulls = true)
        {
            List<NpgsqlParameter> paramList = null;

            // return early if @this is null or empty            
            if (@this == null || @this.Length == 0)
                return null;
            
            // either cast to list where NpgsqlParameter.Value != null or just cast to list depending on removeNulls flag 
            if (removeNulls)
                paramList = @this.Where(p => p.Value != null).ToList();
            else
                paramList = @this.ToList();

            return paramList;
        }
        
        /// <summary>
        /// Casts a DbDataReader object to T
        /// Note: Properties in object T must have custom ColumnName attribute with corresponding column name
        /// </summary>
        /// <param name="@this">reader object</param>
        public static T ToModel<T>(this DbDataReader @this) where T : class, new()
        {
            T objectCast = new T();

            // return early if no data 
            if (!@this.HasRows || @this.FieldCount == 0)
                return objectCast;

            Type objectCastType = objectCast.GetType();
            IEnumerable<PropertyInfo> objProperties = objectCastType.GetTypeInfo().DeclaredProperties;

            foreach(var propInfo in objProperties) 
            {
                // get column name attribute from property                    
                ColumnName columnNameAttr = propInfo.GetCustomAttribute(typeof(ColumnName), false) as ColumnName;
                string columnName = columnNameAttr !=null ? columnNameAttr.AttributeValue : null;

                if (@this[columnName] != DBNull.Value) 
                {
                    string propName = propInfo.Name;
                    
                    Type propType = propInfo.PropertyType;
                    if (propType == typeof(Guid) || propType == typeof (Guid?)) 
                    {
                        Guid parsedValue;
                        if (Guid.TryParse(@this[columnName].ToString(), out parsedValue)) 
                        {
                            if (propType == typeof(Guid)) 
                                propInfo.SetValue(objectCast, parsedValue, null);
                            else 
                                propInfo.SetValue(objectCast, (Guid?)parsedValue, null);
                        }
                    }
                    else if (propType == typeof (int) || propType == typeof (int?)) 
                    {
                        int parsedValue; 
                        if (int.TryParse(@this[columnName].ToString(), out parsedValue)) 
                        {
                            if (propType == typeof (int))
                                propInfo.SetValue(objectCast, parsedValue, null);
                            else 
                                propInfo.SetValue(objectCast, (int?)parsedValue, null);
                        }
                    } 
                    else if (propType == typeof (DateTime) || propType == typeof (DateTime?)) 
                    {
                        DateTime parsedValue;
                        if (DateTime.TryParse(@this[columnName].ToString(), out parsedValue)) 
                        {
                            if (propType == typeof (DateTime)) 
                                propInfo.SetValue(objectCast, parsedValue, null);
                            else
                                propInfo.SetValue(objectCast, (DateTime?)parsedValue, null);
                        }
                    } 
                    else if (propType == typeof (string)) 
                    {
                        propInfo.SetValue(objectCast, @this[columnName].ToString(), null);
                    }
                    
                } // end if @this[columnName] != DBNull.Value
            } // end foreach var propInfo in objProperties

            return objectCast;
        }

        /// <summary>
        /// Copies the data of one object to another. The target object 'pulls' properties of the first. 
        /// Any matching source properties are written to the target.
        /// 
        /// The object copy is a shallow copy only. Any nested types will be copied as 
        /// whole values rather than individual property assignments (ie. via assignment)
        /// Taken from: 
        /// https://weblog.west-wind.com/posts/2009/Aug/04/Simplistic-Object-Copying-in-NET
        /// </summary>
        /// <param name="source">The source object to copy from</param>
        /// <param name="target">The object to copy to</param>
        /// <param name="excludedProperties">A comma delimited list of properties that should not be copied</param>
        /// <param name="memberAccess">Reflection binding access</param>
        public static void CopyObjectData(object source, object target, string excludedProperties, BindingFlags memberAccess)
        {
            string[] excluded = null;
            if (!string.IsNullOrEmpty(excludedProperties))
                excluded = excludedProperties.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            MemberInfo[] miT = target.GetType().GetMembers(memberAccess);
            foreach (MemberInfo Field in miT)
            {
                string name = Field.Name;

                // Skip over any property exceptions
                if (!string.IsNullOrEmpty(excludedProperties) &&
                    excluded.Contains(name))
                    continue;

                if (Field.MemberType == MemberTypes.Field)
                {
                    FieldInfo SourceField = source.GetType().GetField(name);
                    if (SourceField == null)
                        continue;

                    object SourceValue = SourceField.GetValue(source);
                    ((FieldInfo)Field).SetValue(target, SourceValue);
                }
                else if (Field.MemberType == MemberTypes.Property)
                {
                    PropertyInfo piTarget = Field as PropertyInfo;
                    PropertyInfo SourceField = source.GetType().GetProperty(name, memberAccess);
                    if (SourceField == null)
                        continue;

                    if (piTarget.CanWrite && SourceField.CanRead)
                    {
                        object SourceValue = SourceField.GetValue(source, null);
                        piTarget.SetValue(target, SourceValue, null);
                    }
                }
            }
        }

        public static T GetObject<T>() where T : new()
        {
            return new T();
        }
    }
}
