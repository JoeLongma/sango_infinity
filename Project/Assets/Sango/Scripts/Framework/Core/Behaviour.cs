/*
'*******************************************************************
'Tank Framework
'*******************************************************************
*/

using System;
using UnityEngine;
namespace Sango
{

    /// <summary>
    /// 游戏容器衔接类, 负责衔接功能容器与脚本
    /// </summary>
    public class Behaviour : MonoBehaviour
    {
        /// <summary>
        /// 获取对象, 传入对象路径
        /// </summary>
        /// <param name="namePath"> e.g: (root/)UI/nameLabel</param>
        /// <returns>找到的Transform</returns>
        public Transform GetTransform(string namePath)
        {
            Transform rs = transform.Find(namePath);
            if (rs == null && Config.isDebug)
                Log.Warning("在 " + gameObject.name + " 中无法找到节点:" + namePath);
            return rs;
        }

        /// <summary>
        /// 获取对象, 传入对象路径
        /// </summary>
        /// <param name="namePath"> e.g: (root/)UI/nameLabel</param>
        /// <returns>找到的GameObject</returns>
        public GameObject GetObject(string namePath)
        {
            Transform trans = GetTransform(namePath);
            if (trans != null)
                return trans.gameObject;
            return null;
        }

        /// <summary>
        /// 获取对象上的T接口
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="namePath">e.g: (root/)UI/nameLabel</param>
        /// <returns>T接口</returns>
        public T GetComponent<T>(string namePath) where T : Component
        {
            Transform trans = GetTransform(namePath);
            if (trans != null)
                return trans.GetComponent<T>();
            return null;
        }

        /// <summary>
        /// 获取对象上的接口,传入接口type
        /// </summary>
        /// <param name="namePath">e.g: (root/)UI/nameLabel</param>
        /// <param name="typeName">类型字符串名字</param>
        /// <returns>接口</returns>
        public Component GetComponent(string namePath, string typeName)
        {
            Transform trans = GetTransform(namePath);
            if (trans != null)
                return trans.GetComponent(typeName);
            return null;
        }

        public Component GetComponent(string namePath, Type t)
        {
            Transform trans = GetTransform(namePath);
            if (trans != null)
                return trans.GetComponent(t);
            return null;
        }
    }

}