using DF.Finance.Common;
using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.BLL
{
    /// <summary>
    /// 邮件模板
    /// </summary>
    public class MailTemplateBLL : BaseBLL<MailTemplate>
    {
        /// <summary>
        /// 根据Id集合删除邮件模板
        /// </summary>
        /// <param name="Ids">多个Id使用","隔开</param>
        /// <returns></returns>
        public bool DeleteByIds(string Ids)
        {
            return dal.Delete(string.Format(" delete from MailTemplate where Id in({0})", Ids)) > 0;
        }
        /// <summary>
        /// 该邮件标题是否存在
        /// </summary>
        /// <param name="mailtitle">标题是否存在</param>
        /// <returns></returns>
        public bool ExistMailTitle(string mailtitle)
        {
            return dal.GetList(string.Format(" MailTitle ='{0}' ", mailtitle), null).SingleOrDefault() != null ? true : false;
        }
        /// <summary>
        /// 该邮件的模板标题是否存在
        /// </summary>
        /// <param name="title">模板标题</param>
        /// <returns></returns>
        public bool ExistTitle(string title)
        {
            return dal.GetList(string.Format(" Title ='{0}' ", title), null).SingleOrDefault() != null ? true : false;
        }
        /// <summary>
        /// 该邮件的调用别名是否存在
        /// </summary>
        /// <param name="code">调用别名</param>
        /// <returns></returns>
        public bool ExistCode(string code)
        {
            return dal.GetList(string.Format(" Code ='{0}' ", code), null).SingleOrDefault() != null ? true : false;
        }
        /// <summary>
        /// 查询活动列表
        /// </summary>
        /// <param name="dic">请求参数</param>
        /// <param name="order">排序</param>
        /// <returns></returns>
        public Page<MailTemplate> GetMailTemplateList(Dictionary<string, string> dic, string order = null)
        {
            StringBuilder sbWhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(dic["code"]))
            {
                sbWhere.AppendFormat(" and Code like '%{0}%' ", dic["code"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["mailtitle"]))
            {
                sbWhere.AppendFormat(" and MailTitle like '%{0}%' ", dic["mailtitle"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["title"]))
            {
                sbWhere.AppendFormat(" and Title like '%{0}%' ", dic["title"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["issys"]))
            {
                sbWhere.AppendFormat(" and IsSys ={0} ", dic["issys"].Replace("'", ""));
            }
            return this.GetPage(int.Parse(dic["offset"]), int.Parse(dic["limit"]), sbWhere.ToString(), order);
        }
    }
}
