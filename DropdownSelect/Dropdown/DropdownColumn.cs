using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropdownSelect
{
    /// <summary>
    /// 列变更委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ColumnChangeHandler(object sender, DropdownColumnEventArgs e);

    /// <summary>
    /// 下拉选择控件列
    /// </summary>
    public class DropdownColumn
    {
        #region Member

        /// <summary>
        /// 列名
        /// </summary>
        private string columnName;
        /// <summary>
        /// 获取或设置列的可见性
        /// </summary>
        private bool isVisible = true;
        /// <summary>
        /// 列宽度
        /// </summary>
        private int width = -1;

        #endregion

        #region Event

        /// <summary>
        /// 列改变事件
        /// </summary>
        public event ColumnChangeHandler OnColumnChanged;

        #endregion

        #region Method

        public DropdownColumn() { }

        #endregion

        #region Property

        /// <summary>
        /// 绘制的列宽度
        /// </summary>
        internal virtual int DrawWidth
        {
            get;
            set;
        }

        /// <summary>
        /// 列宽度
        /// </summary>
        public virtual int Width
        {
            get
            {
                return width;
            }
            set
            {
                if (width != value)
                {
                    width = value;
                }
            }
        }

        private string headTitle = string.Empty;
        /// <summary>
        /// 列头
        /// </summary>
        public string HeadTitle
        {
            get
            {
                return HeadTitle;
            }
            set
            {
                headTitle = value;
            }
        }


        /// <summary>
        /// 列名
        /// </summary>
        public virtual string ColumnName
        {
            get
            {
                return columnName;
            }
            set
            {
                if (columnName != value)
                {
                    columnName = value;
                    if (OnColumnChanged != null)
                        OnColumnChanged(this, new DropdownColumnEventArgs(this));
                }
            }
        }

        /// <summary>
        /// 获取或设置列的可见性
        /// </summary>
        public virtual bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                if (isVisible != value)
                {
                    isVisible = value;
                    if (OnColumnChanged != null)
                        OnColumnChanged(this, new DropdownColumnEventArgs(this));
                }
            }
        }

        private bool filterabled = true;
        /// <summary>
        /// 允许筛选
        /// </summary>
        public bool Filterabled
        {
            get
            {
                return filterabled;
            }
            set
            {
                filterabled = value;
            }
        }
        #endregion
    }
}
