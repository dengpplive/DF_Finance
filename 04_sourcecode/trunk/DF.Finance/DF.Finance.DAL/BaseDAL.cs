using DF.Finance.Common;
using DF.Finance.DBUtility.PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// 作    者:Jim.li
// 功    能:针对 数据访问层 公共方法处理
// 创建日期:2016-05-07
namespace DF.Finance.DAL
{

    public class BaseDAL<T>
    {
        protected DF.Finance.DBUtility.PetaPoco.Database db = new DF.Finance.DBUtility.PetaPoco.Database("DefaultConnection");

        public BaseDAL()
        {

        }
        #region 私有方法
        /// <summary>
        /// 获取主键名称
        /// </summary>
        /// <returns></returns>
        private string PrimaryKeyName()
        {
            return ((typeof(T).GetCustomAttributes(typeof(PrimaryKeyAttribute), true))[0] as PrimaryKeyAttribute).Value;
        }
        /// <summary>
        /// 获取表名
        /// </summary>
        /// <returns></returns>
        private string TableName()
        {
            return ((typeof(T).GetCustomAttributes(typeof(TableNameAttribute), true))[0] as TableNameAttribute).Value;
        }
        /// <summary>
        /// 获得查询语句
        /// </summary>
        /// <returns></returns>
        protected Sql GetSql(string where, string order)
        {
            Sql s = Sql.Builder;
            s.Select("*")
                .From(TableName());
            if (!string.IsNullOrEmpty(where))
            {
                s.Where(where);
            }
            if (!string.IsNullOrEmpty(order))
            {
                s.OrderBy(order);
            }
            return s;
        }
        #endregion
        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public T GetModel(object id)
        {
            return db.SingleOrDefault<T>(string.Format("WHERE {0}=@0", PrimaryKeyName()), id);
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>返回当前插入编号</returns>
        public object Add(T model)
        {
            return db.Insert(model);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>是否成功</returns>
        public int Update(T model)
        {
            return db.Update(model);
        }
        /// <summary>
        /// 根据实体的主键字段 修改指定的列
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="columns">指定需要修改的列字段 除主键外</param>
        /// <returns>更新条数</returns>
        public int Update(T model, IEnumerable<string> columns)
        {
            return db.Update(model, columns);
        }
        /// <summary>
        /// sql更新
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="updateSQL">sql</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public int Update<T>(string sql, params object[] args)
        {

            return db.Update<T>(sql, args);
        }
        /// <summary>
        /// 判断是否存在此
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where">根据条件判断是否存在此数据</param>
        /// <returns></returns>
        public bool Exists(string where)
        {
            return db.Exists<T>(where, null);
        }
        /// <summary>
        /// 删除 根据主键删除
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>是否成功</returns>
        public int Delete(T model)
        {
            return db.Delete(model);
        }
        /// <summary>
        /// 删除 根据sql 条件和参数删除
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">sql条件  带 where </param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public int Delete(string sql, params object[] args)
        {
            return db.Delete<T>(sql, args);
        }

        /// <summary>
        /// 按条件查询数据
        /// </summary>
        /// <param name="where">不带 WHERE </param>
        /// <returns></returns>
        public List<T> GetList(string where, string order)
        {
            return db.Query<T>(GetSql(where, order)).ToList();
        }
        /// <summary>
        /// 执行一条sql语句
        /// </summary>
        /// <param name="sql">sql</param>
        /// <returns>list T</returns>
        public List<T> GetListExecute(string sql)
        {
            return db.Query<T>(sql).ToList();
        }

        /// <summary>
        ///查询分页数据
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">大小</param>
        /// <param name="where">条件 不带 WHERE</param>
        /// <returns></returns>
        public Page<T> GetPageList(int pageIndex, int pageSize, string where, string order)
        {
            return db.Page<T>(pageIndex, pageSize, GetSql(where, order));
        }

        /// <summary>
        ///查询分页数据
        /// </summary>
        /// <param name="pageIndex">跳过多少条</param>
        /// <param name="pageSize">去多少条数据</param>
        /// <param name="where">条件 不带 WHERE</param>
        /// <returns></returns>
        public Page<T> GetPage(int skip, int limit, string where, string order)
        {
            var pageIndex = (skip + limit) % limit == 0 ? ((skip + limit) / limit) : (((skip + limit) / limit) + 1);
            if (pageIndex == 0) pageIndex = 1;
            return GetPageList(pageIndex, limit, where, order);
        }
        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="skip">跳过多少条</param>
        /// <param name="limit">去多少条数据</param>
        /// <param name="sql">查询的sql语句</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public Page<T> GetPageBySQL(int skip, int limit, string sql, string order = null, params object[] args)
        {
            var pageIndex = (skip + limit) % limit == 0 ? ((skip + limit) / limit) : (((skip + limit) / limit) + 1);
            if (pageIndex == 0) pageIndex = 1;
            if (!string.IsNullOrEmpty(order)) sql += " order by " + order + " ";
            return db.Page<T>(pageIndex, limit, sql, args);
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
            if (!string.IsNullOrEmpty(order)) sql += " order by " + order + " ";
            return db.Page<T>(pageIndex, pageSize, sql, args);
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
            return db.ExecuteScalar<T>(sql, args);
        }
        /// <summary>
        /// 根据条件查询数量
        /// </summary>
        /// <param name="where">where</param>
        /// <returns>long</returns>
        public long InquireNumber(string where)
        {
            Sql s = Sql.Builder;
            s.Select("Count(*)");
            s.From(TableName());
            if (!string.IsNullOrEmpty(where))
            {
                s.Where(where);
            }
            return db.ExecuteScalar<long>(s);
        }
        /// <summary>
        /// 事务，执行多条sql语句
        /// </summary>
        /// <param name="SQLStringList"></param>
        /// <returns></returns>
        public int DoTrans(List<String> SQLStringList)
        {
            int count = 0;
            using (var scope = db.GetTransaction())
            {
                foreach (var sql in SQLStringList)
                {
                    db.Execute(sql);
                    count += 1;
                }
                scope.Complete();
            }
            return count;
        }
    }
}
