﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DTO;
using DTO.订阅;
using Utility.WindowsForm;

namespace 管理工具
{
    public partial class F订阅_详细 : UserControlK
    {
        private List<string> _列表;

        public F订阅_详细(List<string> __列表)
        {
            _列表 = __列表;
            InitializeComponent();
            this.out总数.Text = string.Format("总数: {0}", __列表.Count);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            for (int i = 0; i < _列表.Count; i++)
            {
                this.out号码列表.Rows.Add(_列表[i]);
            }
        }
    }
}
