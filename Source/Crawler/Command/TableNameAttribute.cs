using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Command
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Property)]
   public class TableNameAttribute:Attribute
    {
        private string dbTableName;
        public TableNameAttribute(string tableName) {
            this.dbTableName = tableName;
        }
        public string GetTableName() {
            return this.dbTableName;
        }
    }
}
