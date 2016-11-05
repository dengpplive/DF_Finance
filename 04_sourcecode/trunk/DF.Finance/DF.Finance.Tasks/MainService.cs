using DF.Finance.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DF.Finance.Tasks
{
    public partial class MainService : ServiceBase
    {
        public MainService()
        {
            InitializeComponent();

            // 读取需要运行的服务配置
            var runService = ConfigurationManager.AppSettings["RunService"].ToString();

            // 提醒服务
            if (runService.Contains(",TaskRemind,"))
            {
                taskList.Add(new TaskRemind());
            }
            
        }

        int RunNum = 0;                                     // 总执行次数
        IList<TaskBase> taskList = new List<TaskBase>();    // 异步服务清单
        DateTime RunStartTime = DateTime.Now;               // 开始运行时间

        public void DoTask()
        {
            do
            {
                for (int i = 0; i < taskList.Count; i++)
                {
                    // 如果该任务需要处理则开启新的线程处理之
                    taskList[i].Run();
                    //if (taskList[i].IsNeedRun() == true)
                    //{
                    //    string strLog = "处理任务:({0}){1}，上次执行{2}ms，总执行{3}次。";
                    //    strLog = string.Format(strLog,
                    //        taskList[i].NO,
                    //        taskList[i].Name,
                    //        taskList[i].RunElapsedTime,
                    //        taskList[i].RunAmount
                    //        );
                    //    // 添加事件记录
                    //    Log.LogMessage(strLog, null, taskList[i].Name);

                    //    // 异步处理 (异常在DoProcess内部处理)
                    //    Thread th = new Thread(new ThreadStart(taskList[i].DoProcess));
                    //    th.Start();
                    //    th.Join();
                    //}
                }
                RunNum++;
                if (RunNum % 100 == 0)
                {
                   // LogHelper.WriteLog("自[" + RunStartTime.ToString("yyMMdd HH:mm:ss") + "]运行:" + RunNum);
                }
                // 每次主线程循环的等待时间(实际处理会有该等待误差，但不影响)
                System.Threading.Thread.Sleep(3000);
            }
            while (true);
        }

        Thread thMain;
        protected override void OnStart(string[] args)
        {
            thMain = new Thread(new ThreadStart(DoTask));
            thMain.Start();
            //注册log4
            LogHelper.SetConfig(new System.IO.FileInfo(Utils.GetMapPath("XmlConfig/log4net.config").Replace(@"\bin\Debug", "")));
            LogHelper.WriteLog("东富主服务(DF_Msg_Service)启动成功!");
        }

        protected override void OnStop()
        {
            for (int i = 0; i < taskList.Count; i++)
            {
                taskList[i].Stop();
            }
            thMain.Abort();
            System.GC.Collect();
            LogHelper.WriteLog("服务停止!");
        }

        //关机时执行
        protected override void OnShutdown()
        {
            base.OnShutdown();
        }
    }
}
