using DF.Finance.Common;
using DF.Finance.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// 作    者:Jim.li
// 功    能:针对 业务逻辑层 公共方法处理
// 创建日期:2016-05-07
namespace DF.Finance.BLL
{

    public class BaseBLL<T>
    {
        protected BaseDAL<T> dal = new BaseDAL<T>();

        public BaseBLL()
        {

        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public T GetModel(object id)
        {
            return dal.GetModel(id);
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>返回当前插入编号</returns>
        public object Add(T model)
        {
            return dal.Add(model);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>是否成功</returns>
        public bool Update(T model)
        {
            return dal.Update(model) > 0;
        }
        /// <summary>
        /// 判断是否存在此
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where">根据条件判断是否存在此数据</param>
        /// <returns></returns>
        public bool Exists(string where)
        {
            return dal.Exists(where);
        }
        /// <summary>
        /// 根据实体的主键字段 修改指定的列
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="columns">指定需要修改的列字段 除主键外</param>
        /// <returns>是否成功</returns>
        public bool Update(T model, IEnumerable<string> columns)
        {
            return dal.Update(model, columns) > 0;
        }
        /// <summary>
        /// sql更新
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="updateSQL">UPDATE tablename之后的语句可以带where条件</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public bool Update<T>(string sql, params object[] args)
        {
            return dal.Update<T>(sql, args) > 0;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>是否成功</returns>
        public bool Delete(T model)
        {
            return dal.Delete(model) > 0;
        }
        /// <summary>
        /// 删除 根据sql 条件和参数删除
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">sql条件  带 where </param>
        /// <param name="args">参数</param>
        /// <returns>是否成功</returns>
        public bool Delete(string sql, params object[] args)
        {
            return dal.Delete(sql, args) > 0;
        }
        /// <summary>
        /// 按条件查询数据
        /// </summary>
        /// <param name="where">不带 WHERE </param>
        /// <param name="order">不带 ORDER BY</param>
        /// <returns></returns>
        public List<T> GetList(string where = null, string order = null)
        {
            return dal.GetList(where, order);
        }
        /// <summary>
        /// 执行一条sql语句
        /// </summary>
        /// <param name="sql">sql</param>
        /// <returns>list T</returns>
        public List<T> GetListExecute(string sql)
        {
            return dal.GetListExecute(sql);
        }
        /// <summary>
        ///查询分页数据
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">大小</param>
        /// <param name="where">条件 不带 WHERE</param>
        /// <param name="order">不带 ORDER BY</param>
        /// <returns></returns>
        public Page<T> GetPageList(int pageIndex, int pageSize, string where = null, string order = null)
        {
            return dal.GetPageList(pageIndex, pageSize, where, order);
        }

        /// <summary>
        ///查询分页数据
        /// </summary>
        /// <param name="pageIndex">跳过多少条</param>
        /// <param name="pageSize">去多少条数据</param>
        /// <param name="where">条件 不带 WHERE</param>
        /// <returns></returns>
        public Page<T> GetPage(int skip, int limit, string where = null, string order = null)
        {
            return dal.GetPage(skip, limit, where, order);
        }
        /// <summary>
        /// sql查询分页数据
        /// </summary>
        /// <param name="skip">跳过多少条</param>
        /// <param name="limit">去多少条数据</param>
        /// <param name="sql">查询的sql语句</param>      
        /// <param name="order">排序不带order by</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public Page<T> GetPageBySQL(int skip, int limit, string sql, string order, params object[] args)
        {
            return dal.GetPageBySQL(skip, limit, sql, order, args);
        }
        /// <summary>
        /// 基础查询分页数据
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="sql">纯的sql语句</param>
        /// <param name="order">排序 不含 order by</param>
        /// <param name="args">sql参数</param>
        /// <returns></returns>
        public Page<T> GetPager(int pageIndex, int pageSize, string sql, string order = null, params object[] args)
        {
            return dal.GetPager(pageIndex, pageSize, sql, order, args);
        }
        /// <summary>
        /// 执行sql语句 返回指定类的数据
        /// </summary>
        /// <typeparam name="T">返回指定类的数据</typeparam>
        /// <param name="sql">sql</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, params object[] args)
        {
            return dal.ExecuteScalar<T>(sql, args);
        }
        /// <summary>
        /// 根据条件查询数量
        /// </summary>
        /// <param name="where">where</param>
        /// <returns>long</returns>
        public long InquireNumber(string where)
        {
            return dal.InquireNumber(where);
        }
        /// <summary>
        /// 事务，执行多条sql语句
        /// </summary>
        /// <param name="SQLStringList"></param>
        /// <returns></returns>
        public int DoTrans(List<String> SQLStringList)
        {
            return dal.DoTrans(SQLStringList);
        }

    }
}
