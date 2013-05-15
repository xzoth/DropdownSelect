using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace DropdownSelect
{
    /// <summary>
    /// 多列下拉选择控件
    /// </summary>
    public class DropdownSelect : ComboBox
    {
        #region Member

        /// <summary>
        /// 是否需要初始化项
        /// </summary>
        //private bool IsNeedInitItems = true;
        /// <summary>
        /// 是否需要初始化显示
        /// </summary>
        //private bool IsNeedInitDisplay = true;
        /// <summary>
        /// 上一次击键键盘代码
        /// </summary>
        protected char LastKeyChar = (char)32;

        /// <summary>
        /// 文本改变事件自复位开关
        /// 设置此复位开关的目的是为了拦截基类及其他方法触发的“不在期望内”的文本改变事件
        /// </summary>
        protected bool AutoResetControl = false;

        #endregion

        #region Method

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public DropdownSelect()
        {
            Initialite();
        }

        /// <summary>
        /// 执行控件初始化
        /// </summary>
        protected virtual void Initialite()
        {
            //自绘下拉列表DrawMode必须为 OwnerDrawVariable 或者 OwnerDrawFixed
            base.DrawMode = DrawMode.OwnerDrawFixed;
            //this.DrawMode = DrawMode.OwnerDrawFixed;
        }

        /// <summary>
        /// 展开下拉列表
        /// </summary>
        public void Exapand()
        {
            if (!DroppedDown)
            {
                this.DroppedDown = true;
            }
        }

        /// <summary>
        /// 击键
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            ////退格键 或者 删除键
            //if (LastKeyChar == (char)8 || LastKeyChar == (char)46)
            //{

            //}

            base.OnKeyPress(e);
        }

        /// <summary>
        /// 击键
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            //if (IsApplyRowFilter && !string.IsNullOrEmpty(Text))
            //{
            //    //dataView.RowFilter = "cc = 'window'";
            //    //DataView.RowFilter = "bb IS NOT NULL";

            //    //临时备份当前文本
            //    string strCurrText = Text;
            //    //临时备份当前光标位置
            //    int currPosition = this.SelectionStart;

            //    DataView.RowFilter = "bb LIKE '%" + Text + "%'";
            //    InitListItems();
            //    Invalidate();

            //    //写回文本
            //    Text = strCurrText;
            //    //写回光标位置
            //    this.SelectionStart = currPosition;

            //}

            base.OnKeyDown(e);
        }

        /// <summary>
        /// 文本改变事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(EventArgs e)
        {
            #region 检查复位开关

            //拦截事件的执行
            if (AutoResetControl)
            {
                //复位
                AutoResetControl = false;
                return;
            }

            #endregion

            //如果需要展开列表
            if (!DroppedDown && IsExpandOnEdit)
            {
                //临时备份当前文本
                string strCurrText = Text;
                //临时备份当前光标位置
                int currPosition = this.SelectionStart;

                this.DroppedDown = true;

                //写回文本
                Text = strCurrText;
                //写回光标位置
                this.SelectionStart = currPosition;
            }

            if (IsApplyRowFilter)
            {
                //临时备份当前文本
                string strCurrText = Text;
                //临时备份当前光标位置
                int currPosition = this.SelectionStart;

                ApplyFilter();
                InitListItems();
                Invalidate();

                //写回文本
                Text = strCurrText;
                //写回光标位置
                this.SelectionStart = currPosition;
            }

            if (IsAutoSelect)
            {
                DoAutoSelect();
            }
        }

        /// <summary>
        /// 默认选中
        /// </summary>
        protected virtual void DoAutoSelect()
        {
            //匹配到的行索引
            int rowIndex = 0;
            //是否匹配
            bool isFound = false;

            //查找编辑器文本
            for (; rowIndex < DataView.Count; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < Columns.Count; columnIndex++)
                {
                    if (columns[columnIndex].Filterabled)
                    {
                        //项文本
                        string strText = DataView[rowIndex][Columns[columnIndex].ColumnName].ToString();
                        if (strText.Length >= Text.Length)
                        {
                            if (strText.IndexOf(Text, StringComparison.CurrentCultureIgnoreCase) > -1)
                            {
                                isFound = true;
                                break;
                            }
                        }
                    }
                }

                if (isFound) break;
            }

            //如果找到匹配字符
            if (isFound)
            {
                SelectedIndex = rowIndex;
            }
            //未找到匹配字符
            else
            {
                SelectedIndex = -1;
            }
        }

        /// <summary>
        /// 下拉触发事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);
        }

        /// <summary>
        /// 对数据源应用筛选
        /// </summary>
        protected virtual void ApplyFilter()
        {
            StringBuilder filterBuilder = new StringBuilder();
            string strFilteTemplate = "[{0}] LIKE '%{1}%' OR ";
            string strFilteText = GetFilterString();

            foreach (var column in Columns)
            {
                if (column.IsVisible && column.Filterabled)
                {
                    filterBuilder.AppendFormat(strFilteTemplate, column.ColumnName, strFilteText);
                }
            }

            filterBuilder.Remove(filterBuilder.Length - 3, 3);
            DataView.RowFilter = filterBuilder.ToString();
        }

        /// <summary>
        /// 获取筛选字符串
        /// </summary>
        /// <returns></returns>
        protected virtual string GetFilterString()
        {
            StringBuilder result = new StringBuilder(Text);
            result.Replace("[", "[[ ")
                  .Replace("]", " ]]")
                  .Replace("*", "[*]")
                  .Replace("%", "[%]")
                  .Replace("[[ ", "[[]")
                  .Replace(" ]]", "[]]")
                  .Replace("\'", "''");

            return result.ToString();
        }

        /// <summary>
        /// 重绘列表项
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            //当前绘制项索引
            int itemIndex = e.Index;
            if (DataView != null && DataView.Count > 0 && itemIndex > -1)
            {
                //绘制项左上角顶点坐标X
                int X = e.Bounds.X;
                //绘制项左上角顶点坐标Y
                int Y = e.Bounds.Y;
                //绘制项数据
                DataRowView itemData = DataView[itemIndex];

                //绘图对象
                Graphics graphics = e.Graphics;

                //绘制背景
                e.DrawBackground();

                int itemWidth = 0;

                //循环绘制每一列
                for (int columnIndex = 0; columnIndex < columns.Count; columnIndex++)
                {
                    if (!columns[columnIndex].IsVisible)
                    {
                        continue;
                    }

                    //绘制项
                    graphics.DrawString(itemData[columnIndex].ToString(),
                                          Font,
                                          new SolidBrush(e.ForeColor),
                                          new RectangleF(X, Y, columns[columnIndex].DrawWidth, ItemHeight));

                    //绘制纵线
                    int lineX = 0;
                    itemWidth += lineX = X + columns[columnIndex].DrawWidth - ColumnSpacing;
                    if (hasVerticalLine)
                    {
                        //graphics.DrawLine(new Pen(Color.Silver),
                        //                  new Point(lineX, Y),
                        //                  new Point(lineX, Y + ItemHeight));

                        graphics.DrawLine(new Pen(Color.Silver),
                                          new Point(lineX, Y),
                                          new Point(lineX, Screen.GetWorkingArea(this.FindForm()).Height));
                    }

                    //下一列偏移
                    X += columns[columnIndex].DrawWidth - 4;
                }

                if (hasHorizonLine)
                {
                    //绘制横线
                    graphics.DrawLine(new Pen(Color.Silver),
                                      0, Y + ItemHeight - 1F,
                                      Screen.GetWorkingArea(this.FindForm()).Width, Y + ItemHeight - 1F);
                }

                //绘制聚焦框
                e.DrawFocusRectangle();
                base.OnDrawItem(e);
            }
        }

        /// <summary>
        /// 列表列改变处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DropdownSelect_OnColumnChanged(object sender, DropdownColumnEventArgs e)
        {
            InitColumns();
            InitListItems();
            InitDropdownWidth();
        }

        /// <summary>
        /// 初始化列表项
        /// </summary>
        private void InitListItems()
        {
            Items.Clear();
            foreach (DataRowView rowView in DataView)
            {
                string strText = rowView[DisplayMember].ToString();
                Items.Add(strText);
            }

            //Invalidate();
        }

        /// <summary>
        /// 初始化列集合
        /// </summary>
        private void InitColumns()
        {
            if (DataSource != null && columns.Count == 0)
            {
                columns.Clear();

                //循环缓存列集合
                foreach (DataColumn dc in DataSource.Columns)
                {
                    columns.Add(new DropdownColumn() { ColumnName = dc.ColumnName, HeadTitle = dc.Caption });
                }

                //为每列监列改变事件
                for (int index = 0; index < columns.Count; index++)
                {
                    columns[index].OnColumnChanged += new ColumnChangeHandler(DropdownSelect_OnColumnChanged);
                }
            }
        }

        /// <summary>
        /// 初始化下拉列表宽度
        /// </summary>
        private void InitDropdownWidth()
        {
            int[] arrColumnWidth = new int[Columns.Count];
            SizeF size = new SizeF(2048F, ItemHeight);
            Graphics g = CreateGraphics();

            //计算并设置每一列的宽度
            //每列循环数据内容，取得最大列宽
            foreach (DataRowView rowView in DataView)
            {
                for (int columnIndex = 0; columnIndex < Columns.Count; columnIndex++)
                {
                    string strText = rowView[columnIndex].ToString();
                    //测量字符串宽度
                    int columnWidth = (int)g.MeasureString(strText, Font, size).Width;
                    //获得较宽宽度
                    if (columnWidth > arrColumnWidth[columnIndex])
                    {
                        arrColumnWidth[columnIndex] = columnWidth;
                    }
                }
            }

            DropDownWidth = 1;
            //将计算的列宽写入列缓存设置中
            for (int columnIndex = 0; columnIndex < arrColumnWidth.Length; columnIndex++)
            {
                if (Columns[columnIndex].Width < 0)
                {
                    Columns[columnIndex].DrawWidth = arrColumnWidth[columnIndex] + ColumnSpacing;
                }
                else
                {
                    Columns[columnIndex].DrawWidth = Columns[columnIndex].Width + ColumnSpacing;
                }
                if (Columns[columnIndex].IsVisible)
                {
                    DropDownWidth += Columns[columnIndex].DrawWidth;
                }
            }

            DropDownWidth += 16;
        }

        #endregion

        #region Property

        /// <summary>
        /// 获取或者设置每列之间的间隔距离
        /// </summary>
        private int columnSpacing = 8;
        /// <summary>
        /// 获取或者设置每列之间的间隔距离
        /// </summary>
        [Browsable(true),
         Description("获取或者设置每列之间的间隔距离"),
         Category("外观"),
         DefaultValue(8)]
        public virtual int ColumnSpacing
        {
            get
            {
                return columnSpacing;
            }
            set
            {
                if (columnSpacing != value)
                {
                    columnSpacing = value;
                }
            }
        }

        /// <summary>
        /// 获取或者设置下拉列表的项之间是否具有水平线
        /// </summary>
        private bool hasHorizonLine = false;
        /// <summary>
        /// 获取或者设置下拉列表的项之间是否具有水平线
        /// </summary>
        [Browsable(true),
         Description("获取或者设置下拉列表的项之间是否具有水平线"),
         Category("外观"),
         DefaultValue(false)]
        public virtual bool HasHorizonLine
        {
            get
            {
                return hasHorizonLine;
            }
            set
            {
                if (hasHorizonLine != value)
                {
                    hasHorizonLine = value;
                    //TODO: 触发改变事件，重绘列表
                    //Invalidate();
                }
            }
        }

        /// <summary>
        /// 获取或者设置下拉列表的列之间是否具有垂直线
        /// </summary>
        private bool hasVerticalLine = false;
        /// <summary>
        /// 获取或者设置下拉列表的列之间是否具有垂直线
        /// </summary>
        [Browsable(true),
         Description("获取或者设置下拉列表的列之间是否具有垂直线"),
         Category("外观"),
         DefaultValue(false)]
        public virtual bool HasVerticalLine
        {
            get
            {
                return hasVerticalLine;
            }
            set
            {
                if (hasVerticalLine != value)
                {
                    hasVerticalLine = value;
                    //TODO: 触发改变事件，重绘列表
                    //Invalidate();
                }
            }
        }

        /// <summary>
        /// 控制组合框的外观 重写为只读的
        /// </summary>
        public virtual new ComboBoxStyle DropDownStyle
        {
            get
            {
                return ComboBoxStyle.DropDown;
            }
        }

        /// <summary>
        /// 获取列表项的绘制模式 重写为只读的
        /// </summary>
        [ReadOnly(true)]
        public virtual new DrawMode DrawMode
        {
            get
            {
                return DrawMode.OwnerDrawFixed;
            }
        }

        /// <summary>
        /// 获取或设置编辑器文本
        /// </summary>
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (base.Text != value)
                {
                    AutoResetControl = true;
                    base.Text = value;
                    //触发文本改变事件
                    base.OnTextChanged(new EventArgs());
                }
            }
        }

        /// <summary>
        /// 获取或设置指定当前选定项的索引
        /// </summary>
        public override int SelectedIndex
        {
            get
            {
                return base.SelectedIndex;
            }
            set
            {
                if (base.SelectedIndex != value)
                {
                    //临时备份当前文本
                    string strCurrText = Text;
                    //临时备份当前光标位置
                    int currPosition = this.SelectionStart;

                    //处理索引变更
                    AutoResetControl = true;
                    base.SelectedIndex = value;
                    base.OnSelectedIndexChanged(new EventArgs());

                    //写回文本
                    Text = strCurrText;
                    //写回光标位置
                    this.SelectionStart = currPosition;
                }
            }
        }

        /// <summary>
        /// 获取或设置绑定的数据源
        /// </summary>
        [NonSerialized, XmlIgnore]
        private DataTable dataSource = null;
        /// <summary>
        /// 获取或设置绑定的数据源
        /// </summary>
        [XmlIgnore, Browsable(true)]
        public new DataTable DataSource
        {
            get
            {
                return dataSource;
            }
            set
            {
                if (value != null)
                {
                    if (value.Rows.Count > 1000)
                    {
                        throw new OverflowException("数据源过大，超过1000条。");
                    }

                    dataSource = value;
                    dataView = new DataView(dataSource);

                    InitColumns();
                    InitListItems();
                    InitDropdownWidth();

                    //要求重绘控件界面
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 数据源视图
        /// </summary>
        [NonSerialized, XmlIgnore]
        private DataView dataView = null;
        /// <summary>
        /// 数据源视图
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        protected virtual DataView DataView
        {
            get
            {
                if (dataView == null && DataSource != null)
                {
                    dataView = new DataView(DataSource);
                }

                return dataView;
            }
        }

        /// <summary>
        /// 列集合
        /// </summary>
        [NonSerialized]
        [XmlIgnore]
        protected DropdownColumnCollection columns = new DropdownColumnCollection();
        /// <summary>
        /// 列集合
        /// </summary>
        [XmlIgnore, Browsable(true)]
        public virtual DropdownColumnCollection Columns
        {
            get
            {
                return columns;
            }
        }

        /// <summary>
        /// 获取或设置作为编辑器显示的列成员
        /// </summary>
        public virtual new string DisplayMember
        {
            get
            {
                //默认为第一列
                if (string.IsNullOrEmpty(base.DisplayMember) && columns.Count > 0)
                {
                    base.DisplayMember = columns[0].ColumnName;
                }

                return base.DisplayMember;
            }
            set
            {
                base.DisplayMember = value;
            }
        }

        /// <summary>
        /// 获取或设置当前选中项指定列的值
        /// </summary>
        /// <param name="columnName">列名称</param>
        /// <returns></returns>
        [Browsable(false)]
        public object this[string columnName]
        {
            get
            {
                object result = null;
                if (SelectedIndex >= 0)
                {
                    result = DataSource.Rows[SelectedIndex][columnName];
                }
                return result;
            }
            internal protected set
            {
                DataSource.Rows[SelectedIndex][columnName] = value;
            }
        }

        /// <summary>
        /// 获取或设置当前选中项指定列索引的值
        /// </summary>
        /// <param name="columnIndex">列索引</param>
        /// <returns></returns>
        [Browsable(false)]
        public object this[int columnIndex]
        {
            get
            {
                object result = null;
                if (SelectedIndex >= 0)
                {
                    result = DataSource.Rows[SelectedIndex][columnIndex];
                }
                return result;
            }
            internal protected set
            {
                DataSource.Rows[SelectedIndex][columnIndex] = value;
            }
        }

        /// <summary>
        /// 获取或者设置是否在录入时自动展开下拉列表
        /// </summary>
        private bool isExpandOnEdit = true;
        /// <summary>
        /// 获取或者设置是否录入时自动展开下拉列表
        /// </summary>
        [Browsable(true)]
        [Description("获取或者设置是否在匹配到项时自动展开下拉列表")]
        [Category("行为")]
        [DefaultValue(true)]
        public bool IsExpandOnEdit
        {
            get
            {
                return isExpandOnEdit;
            }
            set
            {
                isExpandOnEdit = value;
            }
        }

        /// <summary>
        /// 获取或者设置是否在筛选时自动选中项
        /// </summary>
        private bool isAutoSelect = false;
        /// <summary>
        /// 获取或者设置是否在筛选时自动选中项
        /// </summary>
        [Browsable(true)]
        [Description("获取或者设置是否在筛选时自动选中项")]
        [Category("行为")]
        [DefaultValue(false)]
        public virtual bool IsAutoSelect
        {
            get
            {
                return isAutoSelect;
            }
            set
            {
                isAutoSelect = value;
            }
        }

        /// <summary>
        /// 获取或者设置是否根据录入的字符对下拉列表进行筛选过滤
        /// </summary>
        private bool isApplyRowFilter = true;
        /// <summary>
        /// 获取或者设置是否根据录入的字符对下拉列表进行筛选过滤
        /// </summary>
        [Browsable(true)]
        [Description("获取或者设置是否根据录入的字符对下拉列表进行筛选过滤")]
        [Category("行为")]
        [DefaultValue(true)]
        public virtual bool IsApplyRowFilter
        {
            get
            {
                return isApplyRowFilter;
            }
            set
            {
                isApplyRowFilter = value;
            }
        }

        /// <summary>
        /// 获取或者设置是否高亮关键字
        /// </summary>
        private bool isHighlightKeyword = false;
        /// <summary>
        /// 获取或者设置是否高亮关键字
        /// </summary>
        [Browsable(true)]
        [Description("获取或者设置是否高亮关键字")]
        [Category("行为")]
        [DefaultValue(false)]
        public virtual bool IsHighlightKeyword
        {
            get
            {
                return isHighlightKeyword;
            }
            private set
            {
                isHighlightKeyword = value;
            }
        }

        #endregion
    }
}
