using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// T 类型枚举列表
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    public class Enum<T> : IEnumerable<T>
    {
        #region Business Methods
        /// <summary>
        /// 返回类型为 IEnumerable&lt;T&gt; 的输入。
        /// </summary>
        /// <returns>类型为 IEnumerable&lt;T&gt; 的序列。</returns>
        public static IEnumerable<T> AsEnumerable()
        {
            return new Enum<T>();
        }
        #endregion

        #region IEnumerable<T> 成员

        /// <summary>
        /// 返回一个循环访问集合的枚举数。
        /// </summary>
        /// <returns>可用于循环访问集合的 IEnumerator&lt;T&gt; 。</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return Enum.GetValues(typeof(T))
                .OfType<T>()
                .AsEnumerable()
                .GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        /// <summary>
        /// 返回一个循环访问集合的枚举数。
        /// </summary>
        /// <returns>可用于循环访问集合的 IEnumerator 。</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IEnumerable<T> 成员

        /// <summary>
        /// 返回一个循环访问集合的枚举数。
        /// </summary>
        /// <returns>可用于循环访问集合的 IEnumerator&lt;T&gt; 。</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
