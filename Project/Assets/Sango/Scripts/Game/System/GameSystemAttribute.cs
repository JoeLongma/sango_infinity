using Sango.Game.Action;
using Sango.Game.Player;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Sango.Game
{
    /// <summary>
    /// 模块属性,定义模块的执行优先级 order越小越优先 以及模块的别名
    /// </summary>
    public class GameSystemAttribute : Attribute
    {
        /// <summary>
        /// 是否忽略创建,为true则不会创建
        /// </summary>
        public bool ignore;

        /// <summary>
        /// 排序标志, 越小越前面, nickName相同则只会创建order最大的
        /// </summary>
        public int order;

        /// <summary>
        /// 别名
        /// </summary>
        public string nickName;
    }
}
