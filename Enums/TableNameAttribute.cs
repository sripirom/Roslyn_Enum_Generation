using System;

namespace EnumGenerator.Enums
{
    [AttributeUsage(AttributeTargets.Enum, Inherited = false)]
    public class TableNameAttribute : Attribute
    {
        public string TableName { get; }
        public string ColumnId { get; }
        public TableNameAttribute(string tableName, string columnId)
        {
            TableName = tableName;
            ColumnId = columnId;
        }
    }
}