﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTO.GPS数据
{
    public class M轨迹查询条件
    {
        public string 号码 { get; set; }

        public DateTime 开始时间 { get; set; }

        public DateTime 结束时间 { get; set; }

        public int? 页码 { get; set; }

        public int? 每页数量 { get; set; }
    }
}
