using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DropdownSelect
{
    /// <summary>
    /// 表格下拉列
    /// </summary>
    public class GridPanelDropdownColumn : DataGridViewColumn
    {
        #region Method

        /// <summary>
        /// 默认构造函数
        /// 从基类构造中指定了新单元格模板为：GridPanelDropdownCell
        /// </summary>
        public GridPanelDropdownColumn()
            : base(new GridPanelDropdownCell())
        {
        }

        #endregion

        #region Property

        /// <summary>
        /// 获取或设置用于创建新单元格的模板
        /// 从默认构造函数中指定了模板为：GridPanelDropdownCell
        /// </summary>
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value != null && !value.GetType().IsAssignableFrom(typeof(GridPanelDropdownCell)))
                {
                    throw new InvalidCastException("单元格模板必须是GridPanelDropdownCell类型");
                }

                base.CellTemplate = value;
            }
        }

        /// <summary>
        /// 绑定的下拉数据源
        /// </summary>
        public virtual DataTable ListSource
        {
            get;
            set;
        }

        #endregion
    }
}
