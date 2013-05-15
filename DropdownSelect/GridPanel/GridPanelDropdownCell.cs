using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DropdownSelect
{
    // <summary>
    /// 表格内联下拉编辑单元格
    /// </summary>
    public class GridPanelDropdownCell : DataGridViewTextBoxCell
    {
        #region Method

        /// <summary>
        /// 初始化内联下拉编辑控件
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="initialFormattedValue"></param>
        /// <param name="dataGridViewCellStyle"></param>
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

            //获得编辑控件
            GridPanelDropdownEditingControl editingControl = DataGridView.EditingControl as GridPanelDropdownEditingControl;

            //获得编辑列
            GridPanelDropdownColumn editingColumn = OwningColumn as GridPanelDropdownColumn;

            if (editingControl != null && editingColumn != null)
            {
                //设置默认显示列与数据绑定列一致
                editingControl.DisplayMember = editingColumn.DataPropertyName;
                //editingControl.EditingControlDataGridView = OwningColumn.DataGridView;
                editingControl.DataSource = editingColumn.ListSource;

                if (Value != null)
                {
                    editingControl.Text = this.Value.ToString();
                }
                else
                {
                    editingControl.Text = string.Empty;
                }

                //editingControl.DroppedDown = true;
            }
        }

        #endregion

        #region Property

        /// <summary>
        /// 获取新记录在单元格行中的默认值
        /// </summary>
        public override object DefaultNewRowValue
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 返回所寄宿编辑器控件的类型
        /// </summary>
        public override Type EditType
        {
            get
            {
                return typeof(GridPanelDropdownEditingControl);
            }
        }

        /// <summary>
        /// 返回值类型
        /// </summary>
        public override Type ValueType
        {
            get
            {
                return typeof(string);
            }
        }

        #endregion
    }
}
