using DF.Finance.Model;
using DF.Finance.Model.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DF.Finance.Common;
namespace DF.Finance.BLL
{
    /// <summary>
    /// 抽奖管理
    /// </summary>
    public class PrizeManageBLL
    {
        private static List<UserClient> usrList = new List<UserClient>();
        public static bool IsRun = false;

        /// <summary>
        /// 根据活动类型名称获取活动信息
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static Activity GetActivity(string Name, bool isExpires = true)
        {
            Activity activity = null;
            ActivityBLL activityBLL = new BLL.ActivityBLL();
            //获取当前时间有效的活动列表
            var dic = new SysDictionaryBLL().GetDicByTypeCode("act_type").Where(p => p.Name == Name).SingleOrDefault();
            if (dic != null)
            {
                activity = activityBLL.GetActivityingList(isExpires).Where(p => p.ActType == dic.Id).SingleOrDefault();
            }
            return activity;
        }
        /// <summary>
        /// 清空活动参与记录和用户中奖纪录
        /// </summary>
        public static void ResetRecord(int ActId)
        {
            new ActPrizeBLL().WQuantityClearZeroByActId(ActId);
            new ActRecordBLL().DeleteByActType(ActId);
            new AtcWinningBLL().DeleteByActType(ActId);
        }
        /// <summary>
        /// 开始抽奖
        /// </summary>
        public static PrizeView StartPrize(Activity activity)
        {
            PrizeView pv = new PrizeView();
            if (activity == null) return pv;
            if (!IsRun)
            {
                IsRun = true;
                try
                {
                    //用户请求的IP
                    string userIP = IPHelper.GetIP();
                    //初始化抽奖的用户
                    InitUserList(activity, userIP);
                    if (usrList.Count > 0)
                    {
                        //该活动的奖品总数
                        //int totalPrizeCount = new ActPrizeBLL().GetTotalQuantityByActId(activity.Id);
                        //int choujiangcount = usrList.Where(p => p.PrizeCount == 1).Count();
                        //int choujiangcount1 = usrList.Where(p => p.PrizeCount == 0).Count();
                        //当所有奖项奖品抽完 不在抽取 或者清空后再继续重新抽取 
                        //if (choujiangcount1==0)
                        //{
                        //清空活动记录
                        ResetRecord(activity.Id);
                        //参与活动的用户纪录
                        AddActRecord(activity, userIP);
                        //}
                        //获取奖项
                        var sysDicPrize = new SysDictionaryBLL().GetDicByTypeCode("award_grade");
                        //按照奖项依次进行随机抽取
                        sysDicPrize.ForEach(m =>
                        {
                            var actWinningList = RunPrizeType(activity, userIP, m);
                            pv.winners.AddRange(actWinningList);
                            //更新抽奖用户
                            pv.tels = usrList.OrderBy(k => k.PrizeCount).Select(u => u.User.Mobile).ToList();
                            //奖项信息
                            pv.awardgrades.Add(new AwardGrade()
                            {
                                type = m.Id,
                                typeName = m.Name,
                                WinningCount = pv.winners.Where(p => p.type == m.Id).Count()
                            });
                        });
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("抽奖", ex);
                }
                finally
                {
                    IsRun = false;
                }
            }
            return pv;
        }

        /// <summary>
        /// 从数据库结果中拿抽奖的数据
        /// </summary>
        /// <param name="activity">抽奖活动</param>
        /// <param name="mobile">手机号</param>
        /// <returns></returns>
        public static PrizeView GetPrizeResult(Activity activity, string mobile = "")
        {
            var result = new PrizeView();
            result.activity = activity;
            var recordBll = new ActRecordBLL();
            var winingBll = new AtcWinningBLL();
            //获取参与抽奖的用户的记录
            var recodList = recordBll.GetListByActId(activity.Id);
            //获取中奖纪录的用户
            var winingList = winingBll.GetWinListByActId(activity.Id);
            //设置的奖项
            var prizes = new SysDictionaryBLL().GetDicByTypeCode("award_grade");
            winingList.ForEach(p =>
            {
                var m = recodList.Where(p1 => p1.userId == p.userId).SingleOrDefault();
                var dic = prizes.Where(n => n.Id == p.PrizeClass).SingleOrDefault();
                if (m != null)
                {
                    result.winners.Add(new ActWinning()
                    {
                        name = m.UserName,
                        tel = m.Mobile,
                        type = p.PrizeClass,
                        typeName = dic != null ? dic.Name : ""
                    });
                }
            });
            //奖项信息
            prizes.ForEach(p =>
            {
                result.awardgrades.Add(new AwardGrade()
                {
                    type = p.Id,
                    typeName = p.Name,
                    WinningCount = result.winners.Where(q => q.type == p.Id).Count()
                });
            });
            //随机排序
            result.tels = recodList.Select(p => p.Mobile).OrderBy(p => new Guid()).ToList();
            result.winners = result.winners.OrderBy(p => new Guid()).ToList();
            //查询中奖情况
            if (!string.IsNullOrEmpty(mobile))
            {
                result.winners = result.winners.Where(p => p.tel == mobile).ToList();
                if (result.winners.Count > 0)
                {
                    result.awardgrades = result.awardgrades.Where(p => p.type == result.winners[0].type).ToList();
                }
                else
                {
                    result.awardgrades.Clear();
                }
                result.tels.Clear();
            }
            return result;
        }

        /// <summary>
        /// 处理号码和姓名
        /// </summary>
        /// <param name="pv">处理对象</param>
        /// <param name="replaceChar">替换字符</param>
        /// <returns></returns>
        public static PrizeView HandleString(PrizeView pv, char replaceChar)
        {
            //号码处理
            pv.tels = pv.tels.Select(p =>
            {
                if (p.Length == 11)
                    return p.Substring(0, 3).PadRight(7, replaceChar) + p.Substring(7);
                else
                    return p;
            }).ToList();
            pv.winners = pv.winners.Select(p =>
            {
                p.name = p.name.Substring(0, 1).PadRight(3, replaceChar);
                p.tel = p.tel.Substring(0, 3).PadRight(7, replaceChar) + p.tel.Substring(7);
                return p;
            }).ToList();
            return pv;
        }

        /// <summary>
        /// 初始化抽奖用户
        /// </summary>
        private static void InitUserList(Activity activity, string userIP)
        {
            //if (usrList.Count == 0)
            //{
            //获取带抽奖的用户
            UserClientBLL userClientBLL = new UserClientBLL();
            usrList = userClientBLL.GetInvolvedUserList();
            //参与活动的用户纪录
            //AddActRecord(activity, userIP);
            //}
        }

        /// <summary>
        /// 单独抽某个奖项 不同活动 不同的奖项
        /// </summary>
        /// <param name="activity">活动实体</param>
        /// <param name="userIP">用户IP</param>
        /// <param name="award_grade">奖项字典实体</param>
        /// <returns></returns>
        private static List<ActWinning> RunPrizeType(Activity activity, string userIP, SysDictionary award_grade)
        {
            List<ActWinning> result = new List<ActWinning>();
            //获取奖项的奖品设置
            ActPrizeBLL actPrizeBLL = new ActPrizeBLL();
            var actPrizeList = actPrizeBLL.GetActPrizeByClass(award_grade.Id);
            if (actPrizeList != null && actPrizeList.Count > 0)
            {
                //奖品信息
                var actPrize = actPrizeList.FirstOrDefault();
                //总的奖品数量
                int quantityCount = actPrize.Quantity;
                //每次摇奖数量
                int ernienNumCount = actPrize.ErnienNum;
                //中奖名额
                int patchCount = actPrize.NumberOfAwards;
                if (quantityCount > 0)// && ernienNumCount > 0)
                {
                    //设置的奖品数量没有中完 可以继续抽 中奖数量
                    if (actPrize.WQuantity < actPrize.Quantity)
                    {
                        //if (actPrize.WQuantity + ernienNumCount >= actPrize.Quantity)
                        //    patchCount = actPrize.Quantity - actPrize.WQuantity;

                        if (patchCount > quantityCount) patchCount = quantityCount;
                        #region 中奖
                        //批量抽取用户
                        var winingUser = RamdomPatch(patchCount, GetPrizeUserRule(usrList));
                        //批量添加用户中奖记录
                        List<AtcWinning> atcWinningList = new List<AtcWinning>();
                        winingUser.ForEach(p =>
                        {
                            result.Add(new ActWinning()
                            {
                                name = p.User.UserName,
                                tel = p.User.Mobile,
                                type = award_grade.Id,
                                typeName = award_grade.Name
                            });
                            atcWinningList.Add(new AtcWinning()
                            {
                                IsAward = 0,
                                ActId = activity.Id,
                                AwardPhone = p.User.Mobile,
                                PrizeId = actPrize.Id,
                                UserId = p.User.Id,
                                UserName = p.User.UserName,
                                Title = activity.Title,
                                PrizeName = actPrize.PrizeName
                            });
                        });
                        //更新已中奖品数量
                        actPrize.WQuantity = winingUser.Count;
                        actPrizeBLL.UpdateWQuantity(actPrize);

                        //添加中奖纪录
                        AtcWinningBLL atcWinningBLL = new AtcWinningBLL();
                        atcWinningBLL.PatchAdd(atcWinningList);
                        #endregion
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 添加活动参与纪录
        /// </summary>
        /// <param name="activity">活动信息</param>
        /// <param name="userIP">IP</param>
        private static void AddActRecord(Activity activity, string userIP)
        {
            ActRecordBLL actRecordBLL = new BLL.ActRecordBLL();
            List<ActRecord> recirdlist = new List<ActRecord>();
            usrList.ForEach(t =>
            {
                recirdlist.Add(new ActRecord()
                {
                    ActId = activity.Id,
                    ActName = activity.Title,
                    UserIP = userIP,
                    AddTime = System.DateTime.Now,
                    UserName = t.User.UserName,
                    UserId = t.User.Id
                });
            });
            actRecordBLL.PatchAdd(recirdlist);
        }
        /// <summary>
        /// 批量随机获取指定个数的用户
        /// </summary>
        /// <param name="count">数目</param>
        /// <param name="usrList">用户选项 带抽奖的用户</param>
        /// <returns></returns>
        private static List<UserClient> RamdomPatch(int count, List<UserClient> usrList)
        {
            var resultList = new List<UserClient>();
            if (usrList.Count < count) count = usrList.Count;
            for (int i = 0; i < count; i++)
            {
                var usr = GetUserRamdom(GetPrizeUserRule(usrList));
                resultList.Add(usr);
            }
            return resultList;
        }

        /// <summary>
        /// 随机获取一个
        /// </summary>
        /// <param name="usrList">用户列表</param>
        /// <returns></returns>
        private static UserClient GetUserRamdom(List<UserClient> usrList)
        {
            int count = usrList.Count;
            if (count > 1)
            {
                Random r = new Random(AssistantHelper.GetRandomSeed());
                int rd = r.Next(0, count);
                var userClient = usrList[rd];
                userClient.PrizeCount++;//抽奖次数
                return userClient;
            }
            else
            {
                usrList[0].PrizeCount++;
            }
            return usrList[0];
        }

        /// <summary>
        /// 获取抽奖用户的规则
        /// </summary>
        /// <param name="usrList">待选用户</param>
        /// <returns></returns>
        private static List<UserClient> GetPrizeUserRule(List<UserClient> usrList)
        {
            return usrList.Where(p => p.PrizeCount == 0).ToList();
        }

    }
}
