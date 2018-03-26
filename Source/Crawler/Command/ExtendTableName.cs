using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Command
{
   public static  class ExtendTableName
    {
        public static string GetTableName(Type type) {
            object[] obj = type.GetCustomAttributes(true);
            foreach (var item in obj)
            {
                if (item is TableNameAttribute) {
                    TableNameAttribute tableNameAttribute = item as TableNameAttribute;
                    return tableNameAttribute.GetTableName();
                }
            }
            return type.Name;
        }
        /// <summary>
        /// 返回sql语句
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetSqlstr(Type type) {
            foreach (var item in type.GetProperties())
            {
               // object[] 
            }
            return "";
        }
    }
}
