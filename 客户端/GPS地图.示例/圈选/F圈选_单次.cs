﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using 显示地图;

namespace GPS地图.示例.圈选
{
    public partial class F圈选_单次 : UserControl
    {
        public E圈选方式 当前方式 { get; private set; }

        private UInt64 _圈选图形索引;

        private F地图 _I地图;

        private List<Point> _鼠标点击位置列表 = new List<Point>();

        public F圈选_单次(F地图 __IF地图, bool __自动删除圈选 = false)
        {
            _I地图 = __IF地图;
            自动删除圈选 = __自动删除圈选;
            InitializeComponent();
        }

        public bool 自动删除圈选 { get; set; }

        public void 删除圈选区域()
        {
            if (_圈选图形索引 == 0)
            {
                return;
            }
            switch (当前方式)
            {
                case E圈选方式.无:
                    break;
                case E圈选方式.矩形:
                    _I地图.删除多边形(_圈选图形索引);
                    break;
                case E圈选方式.圆形:
                    _I地图.删除圆(_圈选图形索引);
                    break;
                case E圈选方式.多边形:
                    _I地图.删除线(_圈选图形索引);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.do矩形.ForeColor = Color.Yellow;
            当前方式 = E圈选方式.矩形;

            this.do矩形.Click += do矩形_Click;
            this.do圆形.Click += do圆形_Click;
            this.do多边形.Click += do多边形_Click;
            this.do关闭.Click += do关闭_Click;
            _I地图.MouseDown += out地图_MouseDown;
            _I地图.MouseMove += out地图_MouseMove;
            _I地图.MouseUp += out地图_MouseUp;
            _I地图.MouseDoubleClick += out地图_MouseDoubleClick;
        }

        void do关闭_Click(object sender, EventArgs e)
        {
            On请求关闭();
        }

        public event Action 请求关闭;

        protected virtual void On请求关闭()
        {
            var handler = 请求关闭;
            if (handler != null) handler();
        }

        void do圆形_Click(object sender, EventArgs e)
        {
            this.do圆形.ForeColor = Color.Yellow;
            this.do矩形.ForeColor = Color.White;
            this.do多边形.ForeColor = Color.White;
            清除();
            当前方式 = E圈选方式.圆形;
        }

        private void 清除()
        {
            _鼠标点击位置列表.Clear();
            if (_圈选图形索引 > 0)
            {
                switch (当前方式)
                {
                    case E圈选方式.无:
                        break;
                    case E圈选方式.矩形:
                        _I地图.删除多边形(_圈选图形索引);
                        break;
                    case E圈选方式.圆形:
                        _I地图.删除圆(_圈选图形索引);
                        break;
                    case E圈选方式.多边形:
                        _I地图.删除线(_圈选图形索引);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        void do矩形_Click(object sender, EventArgs e)
        {
            this.do圆形.ForeColor = Color.White;
            this.do矩形.ForeColor = Color.Yellow;
            this.do多边形.ForeColor = Color.White;
            清除();
            当前方式 = E圈选方式.矩形;
        }

        void do多边形_Click(object sender, EventArgs e)
        {
            this.do圆形.ForeColor = Color.White;
            this.do矩形.ForeColor = Color.White;
            this.do多边形.ForeColor = Color.Yellow;
            清除();
            当前方式 = E圈选方式.多边形;
        }

        public event Action<M经纬度, M经纬度> 处理矩形圈选结束;

        protected virtual void On处理矩形圈选结束(M经纬度 左上角, M经纬度 右下角)
        {
            var handler = 处理矩形圈选结束;
            if (handler != null) handler(HGPS坐标转换.转原始坐标(左上角), HGPS坐标转换.转原始坐标(右下角));
        }

        public event Action<M经纬度, int> 处理圆形圈选结束;

        protected virtual void On处理圆形圈选结束(M经纬度 圆心, int 半径)
        {
            var handler = 处理圆形圈选结束;
            if (handler != null) handler(HGPS坐标转换.转原始坐标(圆心), 半径);
        }

        public event Action<List<M经纬度>> 处理多边形圈选结束;

        protected virtual void On处理多边形圈选结束(List<M经纬度> 顶点列表)
        {
            var handler = 处理多边形圈选结束;
            if (handler != null) handler(顶点列表.Select(HGPS坐标转换.转原始坐标).ToList());
        }

        void out地图_MouseDown(object sender, MouseEventArgs e)
        {
            if (!this.Visible || e.Button != MouseButtons.Right || 当前方式 == E圈选方式.无 || e.Clicks != 1)
            {
                return;
            }
            _鼠标点击位置列表.Add(e.Location);
            //Debug.WriteLine("鼠标按下位置: " + _鼠标按下位置);
        }

        void out地图_MouseMove(object sender, MouseEventArgs e)
        {
            if (!this.Visible)
            {
                return;
            }
            //Debug.WriteLine("鼠标移动位置: " + e.Location);
            switch (当前方式)
            {
                case E圈选方式.无:
                    break;
                case E圈选方式.矩形:
                    if (e.Button != MouseButtons.Right)
                    {
                        return;
                    }
                    绘制矩形(e);
                    break;
                case E圈选方式.圆形:
                    if (e.Button != MouseButtons.Right)
                    {
                        return;
                    }
                    绘制圆形(e);
                    break;
                case E圈选方式.多边形:
                    绘制多边形(e);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void 绘制矩形(MouseEventArgs e)
        {
            if (_鼠标点击位置列表.Count == 0)
            {
                return;
            }
            var __起点 = _I地图.屏幕坐标转经纬度(_鼠标点击位置列表[0]);
            var __终点 = _I地图.屏幕坐标转经纬度(e.Location);
            _I地图.删除矩形(_圈选图形索引);
            _圈选图形索引 = _I地图.添加矩形(__起点,__终点, new M区域绘制参数 { 边框宽度 = 1, 边框颜色 = Color.FromArgb(255, 0, 0, 255), 填充颜色 = Color.FromArgb(55, 135, 206, 235) });
        }

        private void 绘制圆形(MouseEventArgs e)
        {
            if (_鼠标点击位置列表.Count == 0)
            {
                return;
            }
            var __起点 = _I地图.屏幕坐标转经纬度(_鼠标点击位置列表[0]);
            var __终点 = _I地图.屏幕坐标转经纬度(e.Location);
            _I地图.删除圆(_圈选图形索引);
            var __圆心 = __起点;
            var __半径 = H地图算法.测量两点间间距(__圆心, __终点);
            _圈选图形索引 = _I地图.添加圆(
                __圆心,
                __半径,
                new M区域绘制参数 { 边框宽度 = 1, 边框颜色 = Color.FromArgb(255, 0, 0, 255), 填充颜色 = Color.FromArgb(55, 135, 206, 235) });
        }

        private void 绘制多边形(MouseEventArgs e)
        {
            if (_鼠标点击位置列表.Count == 0)
            {
                return;
            }
            _I地图.删除线(_圈选图形索引);
            var __顶点列表 = _鼠标点击位置列表.Select(q => _I地图.屏幕坐标转经纬度(q)).ToList();
            __顶点列表.Add(_I地图.屏幕坐标转经纬度(e.Location));
            _圈选图形索引 = _I地图.添加线(__顶点列表, 1, Color.FromArgb(255, 0, 0, 255));
        }

        void out地图_MouseUp(object sender, MouseEventArgs e)
        {
            if (!this.Visible || e.Button != MouseButtons.Right || 当前方式 == E圈选方式.无)
            {
                return;
            }
            //Debug.WriteLine("鼠标释放位置: " + e.Location);
            switch (当前方式)
            {
                case E圈选方式.无:
                    break;
                case E圈选方式.矩形:
                    if (_鼠标点击位置列表.Count == 0)
                    {
                        return;
                    }
                    var __矩形起点 = _I地图.屏幕坐标转经纬度(_鼠标点击位置列表[0]);
                    var __矩形终点 = _I地图.屏幕坐标转经纬度(e.Location);
                    if (自动删除圈选)
                    {
                        _I地图.删除多边形(_圈选图形索引);
                    }
                    //获取圈选内容
                    On处理矩形圈选结束(__矩形起点, __矩形终点);
                    _鼠标点击位置列表.Clear();
                    break;
                case E圈选方式.圆形:
                    if (_鼠标点击位置列表.Count == 0)
                    {
                        return;
                    }
                    var __圆形起点 = _I地图.屏幕坐标转经纬度(_鼠标点击位置列表[0]);
                    var __圆形终点 = _I地图.屏幕坐标转经纬度(e.Location);
                    if (自动删除圈选)
                    {
                        _I地图.删除圆(_圈选图形索引);
                    }
                    //获取圈选内容
                    var __圆心 = __圆形起点;
                    var __半径 = H地图算法.测量两点间间距(__圆心, __圆形终点);
                    On处理圆形圈选结束(__圆心, __半径);
                    _鼠标点击位置列表.Clear();
                    break;
                case E圈选方式.多边形:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void out地图_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!this.Visible || e.Button != MouseButtons.Right || 当前方式 != E圈选方式.多边形)
            {
                return;
            }
            if (_鼠标点击位置列表.Count < 4)
            {
                _鼠标点击位置列表.Clear();
                return;
            }
            _I地图.删除线(_圈选图形索引);
            _鼠标点击位置列表.Add(_鼠标点击位置列表[0]);
            var __顶点列表 = _鼠标点击位置列表.Select(q => _I地图.屏幕坐标转经纬度(q)).ToList();
            if (!自动删除圈选)
            {
                _圈选图形索引 = _I地图.添加线(__顶点列表, 1, Color.FromArgb(255, 0, 0, 255));
            }
            __顶点列表.RemoveAt(__顶点列表.Count - 1);
            On处理多边形圈选结束(__顶点列表);
            _鼠标点击位置列表.Clear();
        }
    }
}
