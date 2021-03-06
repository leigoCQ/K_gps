﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTO;
using DTO.GPS数据;
using Utility.存储;
using Utility.通用;
using 通用访问;
using 通用访问.DTO;

namespace GIS服务器
{
    public interface IBGPS数据
    {
        void 初始化();

        Dictionary<string, MGPS> 查询最后位置(DateTime __起始时间);

    }

    public class BGPS数据 : IBGPS数据
    {
        private IB数据库 _IB数据库 = H容器.取出<IB数据库>();

        private IT服务端 _IT服务端 = H容器.取出<IT服务端>();

        private IDGPS数据 _IDGPS数据 = H容器.取出<IDGPS数据>();

        private IB插件 _IB插件 = H容器.取出<IB插件>();

        private Dictionary<string, MGPS> _位置缓存 = new Dictionary<string, MGPS>();

        public void 初始化()
        {
            配置通用访问("GPS数据");
            _位置缓存 = _IDGPS数据.查询最后位置(DateTime.Now.AddHours(-1));
            _IB插件.GPS上报 += _IB插件_GPS上报;
            _IB数据库.清除过期数据 += _IDGPS数据.清除过期数据;
        }

        void _IB插件_GPS上报(string __号码, MGPS __GPS)
        {
            _位置缓存[__号码] = __GPS;
            _IDGPS数据.保存(__号码, __GPS);
        }

        private void 配置通用访问(string __对象名称)
        {
            var __对象 = new M对象(__对象名称, null);
            __对象.添加方法("查询轨迹", __实参列表 =>
            {
                var __条件 = HJSON.反序列化<M轨迹查询条件>(__实参列表["条件"]);
                var __字符串 = HJSON.序列化(查询轨迹(__条件));
                //return __字符串;
                return H序列化.AES压缩(__字符串);
            }, E角色.客户, new List<M形参> { new M形参("条件", new M元数据() {  结构= E数据结构.对象, 子成员列表 = new List<M子成员>
            {
                new M子成员("号码", "string"),
                new M子成员("开始时间", "string"),
                new M子成员("结束时间", "string"),
                new M子成员("页数", "int"),
                new M子成员("每页数量", "int"),
            } }) });
            __对象.添加方法("查询最后位置", __实参列表 =>
            {
                var __条件 = HJSON.反序列化<M最后位置查询条件>(__实参列表["条件"]);
                var __字符串 = HJSON.序列化(查询最后位置(__条件));
                return H序列化.AES压缩(__字符串);
            }, E角色.客户, new List<M形参> { new M形参("条件", new M元数据() {  结构= E数据结构.对象, 子成员列表 = new List<M子成员>
            {
                new M子成员("号码列表", "string", E数据结构.单值数组),
                new M子成员("页数", "int?"),
                new M子成员("每页数量", "int?"),
            } }) });
            __对象.添加方法("查询活跃号码", __实参列表 =>
            {
                var __条件 = HJSON.反序列化<M活跃号码查询条件>(__实参列表["条件"]);
                return HJSON.序列化(查询活跃号码(__条件));
            }, E角色.客户, new List<M形参> { new M形参("条件", new M元数据() {  结构= E数据结构.对象, 子成员列表 = new List<M子成员>
            {
                new M子成员("开始时间", "string"),
                new M子成员("结束时间", "string"),
                new M子成员("号码列表", "string", E数据结构.单值数组),
            } }) }, new M元数据 { 结构 = E数据结构.对象, 子成员列表 = new List<M子成员> { new M子成员("总数", "string"), new M子成员("号码列表", "string", E数据结构.单值数组), } });
            __对象.添加方法("统计活跃号码", __实参列表 =>
            {
                var __条件 = HJSON.反序列化<M活跃号码查询条件>(__实参列表["条件"]);
                return HJSON.序列化(统计活跃号码(__条件));
            }, E角色.客户, new List<M形参> { new M形参("条件", new M元数据() {  结构= E数据结构.对象, 子成员列表 = new List<M子成员>
            {
                new M子成员("开始时间", "string"),
                new M子成员("结束时间", "string"),
                new M子成员("号码列表", "string", E数据结构.单值数组),
            } }) }, new M元数据("int"));
            __对象.添加方法("统计频率", __实参列表 =>
            {
                var __条件 = HJSON.反序列化<M频率统计条件>(__实参列表["条件"]);
                return HJSON.序列化(统计频率(__条件));
            }, E角色.客户, new List<M形参> { new M形参("条件", new M元数据() {  结构= E数据结构.单值, 子成员列表 = new List<M子成员>
            {
                new M子成员("开始时间", "string"),
                new M子成员("结束时间", "string"),
                new M子成员("号码列表", "string", E数据结构.单值数组),
            } }) }, new M元数据("float") { 描述 = "(秒/次)" });
            _IT服务端.添加对象(__对象名称, () => __对象);
        }

        public Dictionary<string, MGPS> 查询最后位置(DateTime __起始时间)
        {
            return new Dictionary<string, MGPS>(_位置缓存);
        }

        public M轨迹查询结果 查询轨迹(M轨迹查询条件 __条件)
        {
            return _IDGPS数据.查询轨迹(__条件);
        }

        public M最后位置查询结果 查询最后位置(M最后位置查询条件 __条件)
        {
            return _IDGPS数据.查询最后位置(__条件);
        }

        public M活跃号码查询结果 查询活跃号码(M活跃号码查询条件 __条件)
        {
            return _IDGPS数据.查询活跃号码(__条件);
        }

        public int 统计活跃号码(M活跃号码查询条件 __条件)
        {
            return _IDGPS数据.统计活跃号码(__条件);
        }

        public float 统计频率(M频率统计条件 __条件)
        {
            return _IDGPS数据.统计频率(__条件);
        }

    }
}
