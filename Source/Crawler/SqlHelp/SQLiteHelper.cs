using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace SqlHelp
{
    public class SqLiteHelper
    {
        /// <summary>
        /// 数据库连接定义
        /// </summary>
        private SQLiteConnection dbConnection;

        /// <summary>
        /// SQL命令定义
        /// </summary>
        private SQLiteCommand dbCommand;

        /// <summary>
        /// 数据读取定义
        /// </summary>
        private SQLiteDataReader dataReader;
        private string _connectionString;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">连接SQLite库字符串</param>
        public SqLiteHelper(string connectionString)
        {
            try
            {
                _connectionString = connectionString;
                dbConnection = new SQLiteConnection(connectionString);
                dbConnection.Open();
            }
            catch (Exception e)
            {
                LogClass.LogToFile(e.Message);
            }
        }
        /// <summary>
        /// 执行SQL命令
        /// </summary>
        /// <returns>The query.</returns>
        /// <param name="queryString">SQL命令字符串</param>
        public SQLiteDataReader ExecuteQuery(string queryString)
        {
            try
            {

                SQLiteCommand sQLiteCommand = dbConnection.CreateCommand();
                sQLiteCommand.CommandText = queryString;
                dataReader = sQLiteCommand.ExecuteReader();
            }
            catch (Exception e)
            {
                Log(e.Message);
            }

            return dataReader;
        }
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void CloseConnection()
        {
            //销毁Commend
            if (dbCommand != null)
            {
                dbCommand.Cancel();
            }
            dbCommand = null;
            //销毁Reader
            if (dataReader != null)
            {
                dataReader.Close();
            }
            dataReader = null;
            //销毁Connection
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
            dbConnection = null;

        }

        /// <summary>
        /// 读取整张数据表
        /// </summary>
        /// <returns>The full table.</returns>
        /// <param name="tableName">数据表名称</param>
        public SQLiteDataReader ReadFullTable(string tableName)
        {
            string queryString = "SELECT * FROM " + tableName;
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// 向指定数据表中插入数据
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="tableName">数据表名称</param>
        /// <param name="values">插入的数值</param>
        public SQLiteDataReader InsertValues(string tableName, string[] values)
        {
            //获取数据表中字段数目
            int fieldCount = ReadFullTable(tableName).FieldCount;
            //当插入的数据长度不等于字段数目时引发异常
            if (values.Length != fieldCount)
            {
                throw new SQLiteException("values.Length!=fieldCount");
            }

            string queryString = "INSERT INTO " + tableName + " VALUES (" + "'" + values[0] + "'";
            for (int i = 1; i < values.Length; i++)
            {
                queryString += ", " + "'" + values[i] + "'";
            }
            queryString += " )";
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// 更新指定数据表内的数据
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="tableName">数据表名称</param>
        /// <param name="colNames">字段名</param>
        /// <param name="colValues">字段名对应的数据</param>
        /// <param name="key">关键字</param>
        /// <param name="value">关键字对应的值</param>
        /// <param name="operation">运算符：=,<,>,...，默认“=”</param>
        public SQLiteDataReader UpdateValues(string tableName, string[] colNames, string[] colValues, string key, string value, string operation = "=")
        {
            //当字段名称和字段数值不对应时引发异常
            if (colNames.Length != colValues.Length)
            {
                throw new SQLiteException("colNames.Length!=colValues.Length");
            }

            string queryString = "UPDATE " + tableName + " SET " + colNames[0] + "=" + "'" + colValues[0] + "'";
            for (int i = 1; i < colValues.Length; i++)
            {
                queryString += ", " + colNames[i] + "=" + "'" + colValues[i] + "'";
            }
            queryString += " WHERE " + key + operation + "'" + value + "'";
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// 删除指定数据表内的数据
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="tableName">数据表名称</param>
        /// <param name="colNames">字段名</param>
        /// <param name="colValues">字段名对应的数据</param>
        public SQLiteDataReader DeleteValuesOR(string tableName, string[] colNames, string[] colValues, string[] operations)
        {
            //当字段名称和字段数值不对应时引发异常
            if (colNames.Length != colValues.Length || operations.Length != colNames.Length || operations.Length != colValues.Length)
            {
                throw new SQLiteException("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
            }

            string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + "'" + colValues[0] + "'";
            for (int i = 1; i < colValues.Length; i++)
            {
                queryString += "OR " + colNames[i] + operations[0] + "'" + colValues[i] + "'";
            }
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// 删除指定数据表内的数据
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="tableName">数据表名称</param>
        /// <param name="colNames">字段名</param>
        /// <param name="colValues">字段名对应的数据</param>
        public SQLiteDataReader DeleteValuesAND(string tableName, string[] colNames, string[] colValues, string[] operations)
        {
            //当字段名称和字段数值不对应时引发异常
            if (colNames.Length != colValues.Length || operations.Length != colNames.Length || operations.Length != colValues.Length)
            {
                throw new SQLiteException("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
            }

            string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + "'" + colValues[0] + "'";
            for (int i = 1; i < colValues.Length; i++)
            {
                queryString += " AND " + colNames[i] + operations[i] + "'" + colValues[i] + "'";
            }
            return ExecuteQuery(queryString);
        }


        /// <summary>
        /// 创建数据表
        /// </summary> +
        /// <returns>The table.</returns>
        /// <param name="tableName">数据表名</param>
        /// <param name="colNames">字段名</param>
        /// <param name="colTypes">字段名类型</param>
        public SQLiteDataReader CreateTable(string tableName, string[] colNames, string[] colTypes)
        {
            string queryString = "CREATE TABLE IF NOT EXISTS " + tableName + "( " + colNames[0] + " " + colTypes[0];
            for (int i = 1; i < colNames.Length; i++)
            {
                queryString += ", " + colNames[i] + " " + colTypes[i];
            }
            queryString += "  ) ";
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// Reads the table.
        /// </summary>
        /// <returns>The table.</returns>
        /// <param name="tableName">Table name.</param>
        /// <param name="items">Items.</param>
        /// <param name="colNames">Col names.</param>
        /// <param name="operations">Operations.</param>
        /// <param name="colValues">Col values.</param>
        public SQLiteDataReader ReadTable(string tableName, string[] items, string[] colNames, string[] operations, string[] colValues)
        {
            string queryString = "SELECT " + items[0];
            for (int i = 1; i < items.Length; i++)
            {
                queryString += ", " + items[i];
            }
            queryString += " FROM " + tableName + " WHERE " + colNames[0] + " " + operations[0] + " " + colValues[0];
            for (int i = 0; i < colNames.Length; i++)
            {
                queryString += " AND " + colNames[i] + " " + operations[i] + " " + colValues[0] + " ";
            }
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// 本类log
        /// </summary>
        /// <param name="s"></param>
        static void Log(string s)
        {
            //  Console.WriteLine("class SqLiteHelper:::" + s);
            LogClass.LogToFile("class SqLiteHelper:::" + s);
        }
        #region
        private T ExcuteSql<T>(string sql, Func<SQLiteCommand, T> func)
        {
            if (dbConnection == null)
            {
                dbConnection = new SQLiteConnection(_connectionString);
                dbConnection.Open();
            }
            SQLiteCommand sQLiteCommand = dbConnection.CreateCommand();
            sQLiteCommand.CommandText = sql;
            return func(sQLiteCommand);

        }
        private T CreateT<T>(SQLiteDataReader reader)
        {
            var type = typeof(T);
            var t = Activator.CreateInstance(type);
            bool isTableColumn = true;
            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                object[] objArr = propertyInfo.GetCustomAttributes(true);
                isTableColumn = true;
                foreach (var item in objArr)
                {
                    if (item is PropertyNameAttribute)
                    {
                        PropertyNameAttribute proper = item as PropertyNameAttribute;
                        isTableColumn = proper.IsTableColumn;
                        break;
                    }
                }
                if (objArr.Length > 0 || !isTableColumn)
                {
                    if (!isTableColumn)
                    {
                        if (propertyInfo.CanWrite)
                        {
                            var value = reader[propertyInfo.Name];
                            if (!(value is DBNull))
                                propertyInfo.SetValue(t, value, null);
                        }
                    }
                }
                else
                {
                    if (propertyInfo.CanWrite)
                    {
                        var value = reader[propertyInfo.Name];
                        if (!(value is DBNull))
                            propertyInfo.SetValue(t, value, null);
                    }
                }
            }
            return (T)t;
        }

        /// <summary>
        /// 返回所有符合条件的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereStr">where 1=1 {whereStr}</param>
        /// <returns></returns>
        public List<T> QueryAll<T>(string whereStr = null)
        {
            Type type = typeof(T);
            string sqlStr = string.Format($"SELECT * FROM [{ ExtendTableName.GetTableName(type)}] where 1=1 {whereStr}");
            return ExcuteSql<List<T>>(sqlStr, c =>
            {
                var list = new List<T>();
                var reader = c.ExecuteReader();
                while (reader.Read())
                {
                    var item = (CreateT<T>(reader));
                    list.Add(item);
                }
                return list;
            });
        }
        /// <summary>
        /// 返回首行首列
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object QueryToObject(string sql)
        {
            return ExcuteSql<object>(sql, c => { return c.ExecuteScalar(); });
        }
        public T QueryById<T>(int id)
        {
            Type type = typeof(T);
            string columnStrings = string.Join(",", type.GetProperties().Select(x => $" [{x.Name}] "));
            string sqlStr = string.Format($"SELECT {columnStrings} FROM [{ExtendTableName.GetTableName(type)}] Where Id=@Id");
            return ExcuteSql<T>(sqlStr, c =>
            {
                c.Parameters.Add(new SQLiteParameter("@Id", id));
                var reader = c.ExecuteReader();
                if (reader.Read())
                {
                    return (CreateT<T>(reader));
                }
                return default(T);
            });
        }
        /// <summary>
        /// 更新
        /// WHERE   {whereStr}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        public bool Update<T>(T t, string whereStr)
        {
            var type = typeof(T);
            var values = new List<string>();
            //无法将类型为“System.Data.SqlClient.SQLiteParameter”的对象强制转换为类型“System.Data.SQLite.SQLiteParameter”。
            var par = new List<SQLiteParameter>();
            bool isTableColumn = true;
            foreach (var item in type.GetProperties())
            {
                isTableColumn = true;
                object[] objArr = item.GetCustomAttributes(true);
                foreach (var item2 in objArr)
                {
                    if (item2 is PropertyNameAttribute)
                    {
                        PropertyNameAttribute pro = item2 as PropertyNameAttribute;
                        if (pro.IsTableColumn)
                        {
                            isTableColumn = false;
                            break;
                        }
                    }
                }
                if (isTableColumn)
                {
                    var value = item.GetValue(t, null);
                    if (item.Name.ToLower() != "id")
                    {
                        par.Add(new SQLiteParameter(item.Name, value));
                        values.Add(string.Format($"[{item.Name}] = @{item.Name}"));
                    }
                }
            }
            var v = string.Join(",", values.ToArray());
            var sql = string.Format($"UPDATE [{ExtendTableName.GetTableName(type)}] SET {v} WHERE   {whereStr}");

            return ExcuteSql<bool>(sql, cmd =>
            {
                foreach (var parameter in par)
                    cmd.Parameters.Add(parameter);

                var result = cmd.ExecuteNonQuery();
                return result > 0;
            });
            // return RunCmd(sql, par.ToArray());
        }
        /// <summary>
        /// 根据Id删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteById<T>(int id)
        {
            var type = typeof(T);
            var sqlStr = string.Format($"DELETE FROM [{ExtendTableName.GetTableName(type)}] WHERE (Id = @Id)");
            return ExcuteSql<bool>(sqlStr, (c) =>
            {
                c.Parameters.Add(new SQLiteParameter("@Id", id));
                var result = c.ExecuteNonQuery();
                return result > 0;
            });
        }
        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where">WHERE 1=1 {where}</param>
        /// <returns></returns>
        public bool DeleteByWhere<T>(string where = null)
        {
            var type = typeof(T);
            var sqlStr = string.Format($"DELETE FROM [{ExtendTableName.GetTableName(type)}] WHERE 1=1 {where}");
            return ExcuteSql<bool>(sqlStr, (c) =>
            {
                // c.Parameters.Add(new SQLiteParameter("@Id", id));
                var result = c.ExecuteNonQuery();
                return result > 0;
            });
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Insert<T>(T t)
        {
            List<SQLiteParameter> par = new List<SQLiteParameter>();
            string sql = GetSql<T>(t, false, ref par);
            return ExcuteSql<bool>(sql, cmd =>
            {
                foreach (var parameter in par)
                    cmd.Parameters.Add(parameter);
                var result = cmd.ExecuteNonQuery();
                return result > 0;
            });
        }
        /// <summary>
        /// 添加一条数据，返回自增Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public object InsertGetId<T>(T t)
        {
            List<SQLiteParameter> par = new List<SQLiteParameter>();
            string sql = GetSql<T>(t, true, ref par);
            return ExcuteSql<object>(sql, cmd =>
            {
                foreach (var parameter in par)
                    cmd.Parameters.Add(parameter);
                return cmd.ExecuteScalar();
            });
        }
        private string GetSql<T>(T t, bool IsGetId, ref List<SQLiteParameter> par)
        {
            //插入实体时要判断一下时间的插入.还可以通过特性来判断一下数据的长度
            var type = typeof(T);
            var keys = new List<string>();
            var values = new List<string>();
            bool isTableColumn = true;
            foreach (var item in type.GetProperties())
            {
                isTableColumn = true;
                object[] objArr = item.GetCustomAttributes(true);
                foreach (var obj in objArr)
                {
                    if (obj is PropertyNameAttribute)
                    {
                        PropertyNameAttribute propertyNameAttribute = obj as PropertyNameAttribute;
                        if (propertyNameAttribute.IsTableColumn || propertyNameAttribute.IsPrimaryKey == 1)
                        {
                            isTableColumn = false;
                            break;
                        }
                    }
                }
                if (isTableColumn)
                {
                    var value = item.GetValue(t, null);
                    keys.Add(item.Name);
                    values.Add("@" + item.Name);
                    par.Add(new SQLiteParameter("@" + item.Name, value));
                }
            }
            var c = string.Join(",", keys.ToArray());
            var v = string.Join(",", values.ToArray());
            string sqlStr = string.Format($"INSERT INTO [{ExtendTableName.GetTableName(type)}] ({c}) " +
                $"VALUES({v})");
            if (IsGetId) { sqlStr += ";SELECT last_insert_rowid();"; }
            return sqlStr;
        }
        /// <summary>
        /// sqlite数据库是否存在该表
        /// true 存在
        /// false 不存在
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public bool TableExist(string tableName)
        {
            return ExcuteSql<bool>($"select count(*) from sqlite_master where type='table' and name='{tableName}'", cmd =>
            {
                var result = cmd.ExecuteNonQuery();
                return result > 0;
            });
        }
        #endregion
    }
}
