using System;

namespace ApiQuizGenerator.AppClasses
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnName : Attribute 
    {
        public string AttributeValue { get; set; }
        public ColumnName(string _attributeValue)
        {
            AttributeValue = _attributeValue;
        }
    }
}