using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Command
{
    [AttributeUsage(AttributeTargets.Property)]
   public class PropertyNameAttribute:Attribute
    {
        /// <summary>
        /// 数据库是否有该字段
        /// false 有该字段，true 没有该字段
        /// </summary>
        public bool IsTableColumn { get; set; }
        /// <summary>
        /// 前台是否显示
        /// false 显示
        /// true 不显示
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 是否是自增Id
        /// 1 是
        /// 2 不是
        /// </summary>
        public int IsPrimaryKey { get; set; }
        /// <summary>
        /// 前台展示字符串名
        /// </summary>
        public string ShowCName { get; set; }
        /// <summary>
        /// 数据库表字段名
        /// </summary>
        public string TableColumnName { get; set; }
       
    }
}
