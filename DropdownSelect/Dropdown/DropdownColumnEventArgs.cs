using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropdownSelect
{
    /// <summary>
    /// 下拉选择控件事件参数
    /// </summary>
    public class DropdownColumnEventArgs : EventArgs
    {
        #region Property

        /// <summary>
        /// 列
        /// </summary>
        public virtual DropdownColumn Column
        {
            get;
            set;
        }

        #endregion

        #region Method

        /// <summary>
        /// 下拉选择控件事件参数
        /// </summary>
        /// <param name="column"></param>
        public DropdownColumnEventArgs(DropdownColumn column)
        {
            this.Column = column;
        }

        #endregion
    }
}
