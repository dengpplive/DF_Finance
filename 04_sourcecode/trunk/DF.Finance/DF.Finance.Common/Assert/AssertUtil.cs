﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using DF.Finance.Common.Resources;
namespace DF.Finance.Common.Assert
{
    /// <summary>
    /// 断言类 符合条件的过 不符合条件的抛出异常
    /// </summary>
    [DebuggerStepThrough]
    public static class AssertUtil
    {
        #region 抛出断言异常
        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="msg">异常消息</param>
        public static void Fault(string msg)
        {
            throw new AssertException(msg);
        }

        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="msg">异常消息</param>
        /// <param name="innerException">内部异常</param>
        public static void Fault(string msg, System.Exception innerException)
        {
            throw new AssertException(msg, innerException);
        }

        /// <summary>
        /// 调试的时候故意抛出异常
        /// 作用：
        ///     1，不让事务提交
        ///     2，执行断点
        /// </summary>
        public static void DebugFault()
        {
            throw new AssertException(Constant.DebugFault);
        }
        #endregion
        /// <summary>
        /// 断言是否正确的Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="zeroBase">Id是否基于零</param>
        /// <param name="message">错误消息</param>
        public static void IsValidId(int id, bool zeroBase = false, string message = "")
        {
            if (zeroBase)
                AssertUtil.AreBiggerOrEqual(id, 0, message);
            else
                AssertUtil.AreBiggerOrEqual(id, 1, message);
        }
        /// <summary>
        /// 断言是否正确的Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="zeroBase">Id是否基于零</param>
        /// <param name="message">错误消息</param>
        public static void IsValidId(long id, bool zeroBase = false, string message = "")
        {
            if (zeroBase)
                AssertUtil.AreBiggerOrEqual(id, 0, message);
            else
                AssertUtil.AreBiggerOrEqual(id, 1, message);
        }
        /// <summary>
        /// 断言对象不为空 为null 抛出断言异常
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">异常提示信息</param>
        public static void IsNotNull<T>(T o, string message = "")
        {
            if (o == null)
                Fault(message);
        }

        /// <summary>
        /// 断言对象为空
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">异常提示信息</param>     
        public static void IsNull<T>(T o, string message = "")
        {
            if (o != null)
                Fault(message);
        }

        /// <summary>
        /// 断言对象不为零
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">出错信息</param>
        public static void IsNotZero<T>(T o, string message = "")
        {
            if (o.Equals(0))
                Fault(message);
        }

        /// <summary>
        /// 断言对象为零
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">出错信息</param>
        public static void IsZero<T>(T o, string message = "")
        {
            if (!o.Equals(0))
                Fault(message);
        }

        /// <summary>
        /// 断定对象为指定类型
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">错误提示</param>
        public static void IsInstanceOfType<T>(object o, string message = "")
        {
            IsNotNull(o, Constant.ObjectIsNutNull);
            if (!(o is T))
                Fault(message);
        }

        /// <summary>
        /// 断定对象不为指定类型
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">错误提示</param>
        public static void IsNotInstanceOfType<T>(object o, string message)
        {
            IsNotNull(o, Constant.ObjectIsNutNull);
            if (o is T)
                Fault(message);
        }

        /// <summary>
        /// 断定两个对象为相同类型
        /// </summary>        
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="message">错误提示</param>
        public static void AreSame(object o1, object o2, string message)
        {
            IsNotNull(o1, Constant.ObjectIsNutNull);
            IsNotNull(o2, Constant.ObjectIsNutNull);
            if (o1.GetType() != o2.GetType())
                Fault(message);
        }

        /// <summary>
        /// 断定两个对象为不同类型
        /// </summary>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="message">错误提示</param>
        public static void AreNotSame(object o1, object o2, string message)
        {
            IsNotNull(o1, Constant.ObjectIsNutNull);
            IsNotNull(o2, Constant.ObjectIsNutNull);
            if (o1.GetType() == o2.GetType())
                Fault(message);
        }


        /// <summary>
        /// 断言对象为真
        /// </summary>
        /// <param name="o">对象</param>
        /// <param name="message">异常提示信息</param>
        public static void IsTrue(bool o, string message)
        {
            if (!o)
                Fault(message);
        }

        /// <summary>
        /// 断言对象为假
        /// </summary>
        /// <param name="o">对象</param>
        /// <param name="message">异常提示信息</param>
        public static void IsFalse(bool o, string message)
        {
            if (o)
                Fault(message);
        }

        /// <summary>
        /// 断言集合对象不为空
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="o">集合对象</param>
        /// <param name="message">异常提示信息</param>
        public static void IsNotEmptyCollection<T>(ICollection<T> o, string message)
        {
            IsNotNull<ICollection<T>>(o, message);
            if (o.Count == 0)
                Fault(message);
        }

        /// <summary>
        /// 断言集合对象为空
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="o">集合对象</param>
        /// <param name="message">异常提示信息</param>
        public static void IsEmptyCollection<T>(ICollection<T> o, string message)
        {
            IsNotNull<ICollection<T>>(o, message);
            if (o.Count != 0)
                Fault(message);
        }

        /// <summary>
        /// 断言对象等于默认值
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">异常提示信息</param>
        public static void IsDefault<T>(T o, string message) where T : struct
        {
            if (!o.Equals(default(T)))
                Fault(message);
        }

        /// <summary>
        /// 断言对象不等于默认值
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">异常提示信息</param>
        public static void IsNotDefault<T>(T o, string message) where T : struct
        {
            if (o.Equals(default(T)))
                Fault(message);
        }

        #region 比较断言
        /// <summary>
        /// 断言两个对象相等
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="message">异常提示信息</param>
        public static void AreEqual<T>(T o1, T o2, string message) where T : IComparable<T>
        {
            IsNotNull<T>(o1, message);
            IsNotNull<T>(o2, message);
            if (o1.CompareTo(o2) != 0)
                Fault(message);
        }

        /// <summary>
        /// 断言两个对象不相等
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="message">异常提示信息</param>
        public static void AreNotEqual<T>(T o1, T o2, string message) where T : IComparable<T>
        {
            IsNotNull<T>(o1, message);
            IsNotNull<T>(o2, message);
            if (o1.CompareTo(o2) == 0)
                Fault(message);
        }

        /// <summary>
        /// 断言对象1到大于对象2
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="message">异常提示信息</param>
        public static void AreBigger<T>(T o1, T o2, string message) where T : IComparable<T>
        {
            IsNotNull<T>(o1, message);
            IsNotNull<T>(o2, message);
            if (o1.CompareTo(o2) <= 0)
                Fault(message);
        }

        /// <summary>
        /// 断言对象1到大于等于对象2
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="message">异常提示信息</param>
        public static void AreBiggerOrEqual<T>(T o1, T o2, string message) where T : IComparable<T>
        {
            IsNotNull<T>(o1, message);
            IsNotNull<T>(o2, message);
            if (o1.CompareTo(o2) < 0)
                Fault(message);
        }

        /// <summary>
        /// 断言对象1到小于对象2
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="message">异常提示信息</param>
        public static void AreSmaller<T>(T o1, T o2, string message) where T : IComparable<T>
        {
            IsNotNull<T>(o1, message);
            IsNotNull<T>(o2, message);
            if (o1.CompareTo(o2) >= 0)
                Fault(message);
        }

        /// <summary>
        /// 断言对象1到小于等于对象2
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="message">异常提示信息</param>
        public static void AreSmallerOrEqual<T>(T o1, T o2, string message) where T : IComparable<T>
        {
            IsNotNull<T>(o1, message);
            IsNotNull<T>(o2, message);
            if (o1.CompareTo(o2) > 0)
                Fault(message);
        }
        #endregion

        #region 范围比较断言
        /// <summary>
        /// 断言对象在值范围内
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="message">异常提示信息</param>
        public static void Between<T>(T o, T min, T max, string message) where T : IComparable<T>
        {
            IsNotNull<T>(o, message);
            IsNotNull<T>(min, message);
            IsNotNull<T>(max, message);
            if (!(o.CompareTo(min) >= 0 && o.CompareTo(max) <= 0))
                Fault(message);
        }

        /// <summary>
        /// 断言对象不在值范围内
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="message">异常提示信息</param>
        public static void NotBetween<T>(T o, T min, T max, string message) where T : IComparable<T>
        {
            IsNotNull<T>(o, message);
            IsNotNull<T>(min, message);
            IsNotNull<T>(max, message);
            if (o.CompareTo(min) >= 0 || o.CompareTo(max) <= 0)
                Fault(message);
        }

        /// <summary>
        /// 断言存在覆盖
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="message">异常提示信息</param>
        public static void Coverage<T>(T o1, T o2, T min, T max, string message) where T : IComparable<T>
        {
            IsNotNull<T>(o1, message);
            IsNotNull<T>(o2, message);
            IsNotNull<T>(min, message);
            IsNotNull<T>(max, message);
            AreSmaller<T>(o1, o2, message);
            if (o1.CompareTo(max) >= 0 || o2.CompareTo(min) <= 0)
                Fault(message);
        }

        /// <summary>
        /// 断言不存在覆盖
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="message">异常提示信息</param>
        public static void NotCoverage<T>(T o1, T o2, T min, T max, string message) where T : IComparable<T>
        {
            IsNotNull<T>(o1, message);
            IsNotNull<T>(o2, message);
            IsNotNull<T>(min, message);
            IsNotNull<T>(max, message);
            AreSmaller<T>(o1, o2, message);
            if (!(o1.CompareTo(max) > 0 || o2.CompareTo(min) < 0))
                Fault(message);
        }
        #endregion

        /// <summary>
        /// 断言字符串非空并且非空字符组成
        /// </summary>
        /// <param name="o">字符串</param>
        /// <param name="message">异常提示信息</param>
        public static void NotNullOrWhiteSpace(string o, string message)
        {
            if (string.IsNullOrEmpty(o) || o == string.Empty)
                Fault(message);
        }

        /// <summary>
        /// 断言字符串为空或者由空字符组成
        /// </summary>
        /// <param name="o">字符串</param>
        /// <param name="message">异常提示信息</param>
        public static void NullOrWhiteSpace(string o, string message)
        {
            if (!string.IsNullOrEmpty(o) && o != string.Empty)
                Fault(message);
        }

        /// <summary>
        /// 判断对象是否存在于枚举类型定义中
        /// </summary>
        /// <typeparam name="E">枚举类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">异常提示信息</param>
        public static void IsInEnum<E>(object o, string message) where E : struct
        {
            if (!Enum.IsDefined(typeof(E), o))
                Fault(message);
        }

        /// <summary>
        /// 判断字符串内是否由数字和小数点组成
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="message">异常提示信息</param>
        public static void IsDecimal(string str, string message)
        {
            NotNullOrWhiteSpace(str, message);
            try
            {
                decimal.Parse(str);
            }
            catch
            {
                Fault(message);
            }
        }

        /// <summary>
        /// 判断字符串内是否由数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="message">异常提示信息</param>
        public static void IsLong(string str, string message)
        {
            NotNullOrWhiteSpace(str, message);
            try
            {
                long.Parse(str);
            }
            catch
            {
                Fault(message);
            }
        }

        /// <summary>
        /// 正则表达式验证
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="message">异常提示信息</param>
        /// <param name="options">正则表达式配置</param>
        public static void IsValid(string input, string pattern, string message, RegexOptions options = RegexOptions.ECMAScript)
        {
            Regex regex = new Regex(pattern, options);
            AssertUtil.IsTrue(regex.IsMatch(input), message);
        }

        /// <summary>
        /// 是否为唯一的集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="message">消息</param>
        public static void IsUniqueCollection<T>(IEnumerable<T> list, string message)
        {
            AssertUtil.AreEqual(list.Distinct().Count(), list.Count(), message);
        }
    }
}
