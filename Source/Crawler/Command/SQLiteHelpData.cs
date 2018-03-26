using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace Command
{
    public class SQLiteHelpData
    {
        private static SqLiteHelper sqLiteHelper;
       
        /// <summary>
        /// 返回所有符合条件的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where">where 1=1 {whereStr}</param>
        /// <returns></returns>
        public List<T> QueryAll<T>(string where =null)
        {

            List<T> list = new List<T>();
            Act(() =>
            {
                list = sqLiteHelper.QueryAll<T>(where);
            });
            return list;
        }
       
        /// <summary>
        /// 根据实体类创建表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void CreateTable<T>()
        {
            Act(() =>
            {
                Type type = typeof(T);
                List<string> columnNames = new List<string>();
                List<string> columnType = new List<string>();
                bool isTableColumn = true;
                foreach (var item in type.GetProperties())
                {
                    isTableColumn = true;
                    object[] attrs = item.GetCustomAttributes(true);
                    foreach (var attr in attrs)
                    {
                        if (attr is PropertyNameAttribute)
                        {
                            PropertyNameAttribute attr2 = attr as PropertyNameAttribute;
                            if (attr2.IsTableColumn)
                            {
                                isTableColumn = false;
                                break;
                            }
                        }
                    }
                    if (isTableColumn)
                    {
                        columnNames.Add(item.Name);
                        columnType.Add(item.PropertyType.Name);
                    }
                }
                sqLiteHelper.CreateTable(ExtendTableName.GetTableName(type), columnNames.ToArray(), columnType.ToArray());
            });

        }
        /// <summary>
        /// 删除表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void DropTable<T>()
        {
            Act(() =>
            {
                string sql = $"drop table [{ExtendTableName.GetTableName(typeof(T))}]";
                sqLiteHelper.ExecuteQuery(sql);
            });
        }
        public  void InsertChannel<T>()
        {
            //DropTable<ChannelModel>();
            CreateTable<T>();
            return;
            Act(()=> {
                //string DirStr =Path.Combine( AppDomain.CurrentDomain.BaseDirectory, @"DataFile\waterChannelXMLFile.xml");
                //List<T> list = XmlHelper.XmlToObjList<T>(DirStr, "Channel/WItem");
                //int i = 0;
                //foreach (var item in list)
                //{
                //    i++;
                //    sqLiteHelper.InsertValues("Channel", new string[] { i.ToString(), item.Code, item.CName, item.EName, item.Unit, item.UpperValue.ToString(), item.LowerValue.ToString(), item.DigitNum.ToString(), item.Enabled.ToString() });
                //}
            });
          
        }
  
        /// <summary>
        /// 根据Id删除某条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteById<T>(int id)
        {
            bool isDelete = false;
            Act(() =>
            {
                isDelete = sqLiteHelper.DeleteById<T>(id);
            });
            return isDelete;
        }
        /// <summary>
        /// 删除符合条件的记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool DeleteByWhere<T>(string where=null)
        {
            bool isDelete = false;
            Act(() =>
            {
                isDelete = sqLiteHelper.DeleteByWhere<T>(where);
            });
            return isDelete;
        }
        /// <summary>
        /// 更新 WHERE   {whereStr}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="whereStr">WHERE   {whereStr}</param>
        public void UpdateServer<T>(T t, string whereStr)
        {
            Act(() =>
            {
                sqLiteHelper.Update<T>(t, whereStr);
            });
        }
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void InsertModel<T>(T t)
        {
            Act(() =>
            {
                sqLiteHelper.Insert<T>(t);
            });
        }
        /// <summary>
        /// 添加一组数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tList"></param>
        public void InsertListModel<T>(List<T> tList) {
            Act(()=> {
                foreach (var item in tList)
                {
                    sqLiteHelper.Insert<T>(item);
                }
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
            Func<T,object> func= sqLiteHelper.InsertGetId<T>;
            return Fun(func,t);
        }
            /// <summary>
            /// 数据库是否存在该表
            /// </summary>
            /// <typeparam name="T"></typeparam>
            public void TableExist<T>()
        {
            Act(() =>
            {
                if (!sqLiteHelper.TableExist(ExtendTableName.GetTableName(typeof(T))))
                {
                    CreateTable<T>();
                }
            });
        }
        private void Act(Action act)
        {
            try
            {
                sqLiteHelper = new SqLiteHelper("data source=mydb2.db");
                act.Invoke();
            }
            finally
            {
                sqLiteHelper.CloseConnection();
            }
        }
        /// <summary>
        /// 根据Id查询数据集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        public T QueryById<T>(int Id)
        {
            Func<int, T> func = sqLiteHelper.QueryById<T>;
            return Fun(func, Id);
        }
        /// <summary>
        /// 传入sql语句，返回第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object QueryToObject(string sql)
        {
           Func<string,object> func= sqLiteHelper.QueryToObject;
            return Fun(func,sql);
        }
        private T Fun<T,T2>(Func<T2, T> function,T2 Id) {
            try
            {
                sqLiteHelper = new SqLiteHelper("data source=mydb2.db");
                return function.Invoke(Id);
            }
            finally {
                sqLiteHelper.CloseConnection();
            }
        }
    }
}
