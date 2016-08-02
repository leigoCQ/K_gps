﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utility.WindowsForm;

namespace GPS地图.示例
{
    public partial class F主窗口 : UserControlK
    {
        public F主窗口()
        {
            InitializeComponent();
            this.uTab1.添加("号码本", new F实时显示_号码本 { Dock = DockStyle.Fill });
            //this.uTab1.添加("号码段", new F实时显示_号码段 { Dock = DockStyle.Fill });
            this.uTab1.添加("按频率回放", new F按频率回放 { Dock = DockStyle.Fill });
            //this.uTab1.添加("按时间回放", new F按时间回放 { Dock = DockStyle.Fill });
            this.uTab1.添加("扩展", new F扩展 { Dock = DockStyle.Fill });

            this.uTab1.激活("号码本");
        }
    }
}
