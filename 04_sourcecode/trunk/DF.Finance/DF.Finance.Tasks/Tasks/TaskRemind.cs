using DF.Finance.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Tasks
{
    class TaskRemind : TaskBase
    {
        #region 必需实现的属性
        public override string NO
        {
            get { return "TaskRemind"; }
        }

        public override string Name
        {
            get { return "提醒生成任务"; }
        }

        public override string Remark
        {
            get { return "提醒任务生成：12小时一次"; }
        }

        public override int IntervalSecond
        {
            //6小时执行一次
            get { return 21600; }
        }
        #endregion
        public TaskRemind()
        {

            this.OnStart += TaskRemind_OnStart;

        }

        void TaskRemind_OnStart()
        {
            var dt = DateTime.Now;
            if (dt.Hour >= 0 && dt.Hour <= 6)
            {
                new BLL.RemindBLL().RemindTasks();
                LogHelper.WriteLog(string.Format("时间：{0}，执行了{1}", DateTime.Now, this.Name));
            }
        }
    }
}
