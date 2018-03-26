using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Command;

namespace waterEmluatorMDI.Common
{
   public class FromViewBLL
    {
       /// <summary>
       /// 根据实体类，生成dataGridView表头
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="dataGridView"></param>
        public static void SetDataGridView<T>(DataGridView dataGridView) {
            Type type = typeof(T);
            foreach (var item in type.GetProperties())
            {
                object[] objAttrs = item.GetCustomAttributes(true);
                foreach (var attr in objAttrs)
                {
                    if (attr is PropertyNameAttribute) {
                        PropertyNameAttribute propertyNameAttribute = attr as PropertyNameAttribute;
                        if (propertyNameAttribute.IsShow)
                        { break; }
                            DataGridViewTextBoxColumn dgColumn = new DataGridViewTextBoxColumn();
                        dgColumn.Name = item.Name;
                        dgColumn.DataPropertyName = item.Name;
                        dgColumn.HeaderText = propertyNameAttribute.ShowCName;
                        dataGridView.Columns.Add(dgColumn);
                    }
                }
            }
        }
     /// <summary>
     ///  将dataGridView中的数据赋值给实体类
     /// </summary>
     /// <typeparam name="T"></typeparam>
     /// <param name="dataGridView"></param>
     /// <returns></returns>
        public static List<T> SetTToGriddViewData<T>(DataGridView dataGridView) {
            Type type = typeof(T);
            List<T> listT = new List<T>();
            bool isDataColunm = false;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                object t = Activator.CreateInstance(type);
                foreach (var pro in type.GetProperties())
                {
                    isDataColunm = false;
                    foreach (DataGridViewColumn columnName in dataGridView.Columns)
                    {
                        if (columnName.Name.Equals(pro.Name)) { isDataColunm = true; break; }
                    }
                    if (isDataColunm) { 
                    object v = row.Cells[pro.Name].Value;
                    if (v != null) {
                        pro.SetValue(t,v,null);
                    }
                    }
                }
                listT.Add((T)t);
            }
            return listT;
        }
        /*
        /// <summary>
        /// ComboBox 赋值
        /// </summary>
        /// <param name="list"></param>
        /// <param name="comboBox"></param>
        public static void SetCombox(List<ShowComboxValueModel> list,ComboBox comboBox) {
            comboBox.DataSource = list;
            comboBox.DisplayMember = "Name";
            comboBox.ValueMember = "Value";
        }
        */
        #region  站点树操作
        /*
        /// <summary>
        /// 站点树
        /// </summary>
        /// <param name="treeView"></param>
        public static void SetStationTree(TreeView treeView)
        {
            treeView.Nodes.Clear();
            treeView.CheckBoxes = true;
            foreach (var item in GlobalFromSize.stationModelAll.FindAll(p => p.ParentId == 0))
            {
                TreeNode root = new TreeNode();
                root.Text = item.StationName;
                root.Tag = item.Id;
                SetTree(item, root);
                treeView.Nodes.Add(root);
            }
            treeView.ExpandAll();//展开树
        }
        /// <summary>
        /// 递归调用给树节点赋值
        /// </summary>
        /// <param name="stationModel"></param>
        /// <param name="treeNode"></param>
        private static void SetTree(StationModel stationModel, TreeNode treeNode)
        {
            foreach (var item in GlobalFromSize.stationModelAll.FindAll(p => p.ParentId == stationModel.Id))
            {
                TreeNode chldNode = new TreeNode();
                chldNode.Text = item.StationUpCode + "_" + item.StationName;
                chldNode.Tag = item.Id;
                chldNode.Checked = item.IsUpData;
                treeNode.Nodes.Add(chldNode);
                SetTree(item, chldNode);
            }
        }
        */
        //递归字节点跟随其全选或全不选
        public static void ChangeChild(TreeNode node,bool state) {
            node.Checked = state;
            foreach (TreeNode tn in node.Nodes)
            {
                ChangeChild(tn,state);
            }
        }
        //递归父节点跟随其全选或全不选
        public static void ChangeParent(TreeNode node) {
            if (node.Parent != null) {
                //兄弟节点被选中的个数
                int brotherNodeCheckedCount = 0;
                //遍历该节点的兄弟节点
                foreach (TreeNode item in node.Parent.Nodes)
                {
                    if (item.Checked == true) {
                        brotherNodeCheckedCount++;
                    }
                }
                //兄弟节点全没选，其父节点也不选
                if (brotherNodeCheckedCount == 0) {
                    node.Parent.Checked = false;
                    ChangeParent(node.Parent);
                }
                //兄弟节点只要有一个被选，其父节点也被选
                if (brotherNodeCheckedCount >= 0) {
                    node.Parent.Checked = true;
                    ChangeParent(node.Parent);
                }
            }
        }
        #endregion
    }
}
