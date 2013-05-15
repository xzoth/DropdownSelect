using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DropdownSelect
{
    /// <summary>
    /// 表格内联下拉编辑控件
    /// </summary>
    public class GridPanelDropdownEditingControl : DropdownSelect, IDataGridViewEditingControl
    {
        #region Method

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public GridPanelDropdownEditingControl()
            : base()
        {
        }

        /// <summary>
        /// 重写作为内联编辑控件的文本改变时的处理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            EditingControlValueChanged = true;
            NotifyDataGridViewOfValueChange();
        }

        /// <summary>
        /// 通知内联编辑控件值改变
        /// </summary>
        protected virtual void NotifyDataGridViewOfValueChange()
        {
            EditingControlDataGridView.NotifyCurrentCellDirty(true);
        }

        #endregion

        #region IDataGridViewEditingControl 接口成员

        /// <summary>
        /// 应用单元格样式
        /// </summary>
        /// <param name="dataGridViewCellStyle"></param>
        public virtual void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            //this.Font = new Font("微软雅黑", dataGridViewCellStyle.Font.Size, FontStyle.Underline);//dataGridViewCellStyle.Font;  
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;
        }

        /// <summary>
        /// 获取或设置与之关联的表格控件
        /// </summary>
        public virtual DataGridView EditingControlDataGridView
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置编辑控件格式化后的值
        /// </summary>
        public virtual object EditingControlFormattedValue
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value.ToString();
                NotifyDataGridViewOfValueChange();
            }
        }

        /// <summary>
        /// 获取或设置内联编辑控件所在行索引
        /// </summary>
        public virtual int EditingControlRowIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置值是否发生变化
        /// </summary>
        public virtual bool EditingControlValueChanged
        {
            get;
            set;
        }

        /// <summary>
        /// 设置键盘值响应
        /// </summary>
        /// <param name="keyData"></param>
        /// <param name="dataGridViewWantsInputKey"></param>
        /// <returns></returns>
        public virtual bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            //当dataGridViewWantsInputKey为true时，如果不想内联控件处理，则isWants应为false
            bool isWants = true;

            switch (keyData & Keys.KeyCode)
            {
                //设置需要预留给编辑控件处理的键盘代码
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                case Keys.Enter:
                    isWants = true;
                    break;
                default:
                    isWants = !dataGridViewWantsInputKey;//默认取反，即要求内联控件处理所有表格控件未处理的键值
                    break;
            }

            return isWants;
        }

        /// <summary>
        /// 获得控件在编辑状态时的光标
        /// </summary>
        public virtual Cursor EditingPanelCursor
        {
            get { return Cursors.IBeam; }
        }

        /// <summary>
        /// 获取内联编辑控件的值
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            //return this.Text; //返回原值
            return EditingControlFormattedValue; //返回格式化后的值
        }

        /// <summary>
        /// 准备当前选中的单元格以进行编辑
        /// </summary>
        /// <param name="selectAll"></param>
        public virtual void PrepareEditingControlForEdit(bool selectAll)
        {

        }

        /// <summary>
        /// 获取或设置一个值，该值指示每当值更改时，是否需要重新定位单元格的内容
        /// </summary>
        public virtual bool RepositionEditingControlOnValueChange
        {
            get { return false; }
        }

        #endregion
    }
}
