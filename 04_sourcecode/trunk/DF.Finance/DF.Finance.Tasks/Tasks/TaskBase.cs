/*
版权信息：
作    者：jim.li
完成日期：2015-09-25
内容说明：任务基础类
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Configuration;

using System.Data;
using System.Threading;
using DF.Finance.Common;
namespace DF.Finance.Tasks
{
    /// <summary>
    /// 异步任务定义基类
    /// </summary>
    public abstract class TaskBase
    {
        #region ITask 接口成员
        // 任务编号
        abstract public string NO { get; }
        // 任务名称
        abstract public string Name { get; }
        // 备注
        abstract public string Remark { get; }
        // 运行间隔秒数
        abstract public int IntervalSecond { get; }
        #endregion

        #region TaskBase 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        public TaskBase()
        {
            this.ProcessStatus = ProcessStatus.从未执行;
            this.nextRunTime = DateTime.Now;
            //初始化
            if (OnInit != null)
            {
                OnInit();
            }
        }
        #endregion

        #region 运行时属性
        /// <summary>
        /// 获取或设置运行状态
        /// </summary>
        public ProcessStatus ProcessStatus { get; set; }
        private DateTime lastRunTime = DateTime.MinValue;
        /// <summary>
        /// 获取上次运行时间
        /// </summary>
        public DateTime LastRunTime { get { return lastRunTime; } }
        // 运行耗用时间
        long lastRunElapsedTime = 0;
        /// <summary>
        /// 获取上一次运行所耗用时间(毫秒)
        /// </summary>
        /// <returns>耗用时间(毫秒)</returns>
        public long RunElapsedTime { get { return lastRunElapsedTime; } }
        // 运行次数
        long runAmount = 0;
        /// <summary>
        /// 获取已运行次数
        /// </summary>
        /// <returns>运行次数</returns>
        public long RunAmount { get { return runAmount; } }
        //protected bool nextRunFlag = false;
        ///// <summary>
        ///// 获取下次是否已经运行(标识下次的运行是否已经运行过)
        ///// </summary>
        //public bool NextRunFlag { get { return nextRunFlag; } }
        protected DateTime nextRunTime = DateTime.MaxValue;
        /// <summary>
        /// 获取下次运行时间
        /// </summary>
        public DateTime NextRunTime { get { return nextRunTime; } }
        #endregion

        #region  事件
        /// <summary>
        /// 处理事件方法的代理
        /// </summary>
        public delegate void ProcessHandler();
        /// <summary>
        /// 初始化方法
        /// </summary>
        public event ProcessHandler OnInit;
        /// <summary>
        /// 处理前的事件
        /// </summary>
        public event ProcessHandler OnStart;
        /// <summary>
        /// 处理完成的时间
        /// </summary>
        public event ProcessHandler OnEnd;
        /// <summary>
        /// 任务运行时出错时处理的事件
        /// </summary>
        public event ProcessHandler OnError;
        /// <summary>
        /// 任务结束时出错的处理的事件
        /// </summary>
        public event ProcessHandler OnEndError;
        #endregion

        // 异步处理线程
        Thread thTask = null;
        #region  Run 执行方法
        public void Run()
        {
            if (IsNeedRun())
            {
                string strLog = "处理任务:({0}){1}，上次执行{2}ms，总执行{3}次。";
                strLog = string.Format(strLog, this.NO, this.Name, this.RunElapsedTime, this.RunAmount);
                // 添加事件记录
                this.LogMessage(strLog);


                thTask = new Thread(new ThreadStart(DoProcess));
                thTask.Start();
                thTask.Join();

            }
        }
        #endregion
        #region Stop 停止方法
        public void Stop()
        {
            this.thTask.Abort();
        }
        #endregion

        #region public DoProcess 处理方法
        /// <summary>
        /// 处理方法
        /// </summary>
        public void DoProcess()
        {
            if (OnStart != null)
            {
                try
                {
                    this.ProcessStatus = ProcessStatus.处理中;

                    // 下次将运行
                    // this.nextRunFlag = true;
                    // 上次(本次)运行时间
                    this.lastRunTime = DateTime.Now;

                    // 运行计数
                    runAmount++;

                    // 运行计时
                    Stopwatch sw = new Stopwatch();
                    sw.Start();

                    // 计算下次运行时间
                    // 1.如果使用IsNeedRun检测，必须计算下次运行时间
                    // 2.如果使用IsNeedProcess检测，则可以移除以下代码
                    // 3.IsNeedRun检测效率高，但运行效率降低一点
                    // 4.IsNeedProcess每次检测所有配置，检测效率低
                    // 5.暂用IsNeedRun 做检测
                    ComputingNextRunTime();

                    // 执行
                    OnStart();
                    this.ProcessStatus = ProcessStatus.待命;

                    sw.Stop();
                    lastRunElapsedTime = sw.ElapsedMilliseconds;
                }
                catch (Exception ex)
                {
                    // 记录异常
                    LogException("处理任务:[" + this.Name + "] 发生异常。", ex);
                    this.ProcessStatus = ProcessStatus.异常中;
                    //出错给 -1 耗时。
                    lastRunElapsedTime = -1;

                    // 出错处理
                    if (OnError != null)
                    {
                        OnError();
                    }
                }
            }

            if (OnEnd != null && this.ProcessStatus != ProcessStatus.异常中)
            {
                try
                {
                    OnEnd();
                }
                catch (Exception ex)
                {
                    // 记录异常
                    LogException("结束任务:[" + this.Name + "] 发生异常。", ex);
                    this.ProcessStatus = ProcessStatus.异常中;
                    //出错给 -1 耗时。
                    lastRunElapsedTime = -1;

                    // 出错处理
                    if (OnEndError != null)
                    {
                        OnEndError();
                    }
                }
            }
        }
        #endregion

        //virtual 允许重写的方法
        #region public virtual IsNeedRun 根据下次运行时间和下次运行标识检测是否需要运行
        /// <summary>
        /// 根据下次运行时间和下次运行标识检测是否需要运行
        /// (这种方式检测效率较高，但需要每运行一次，计算下一次运行时间)
        /// </summary>
        /// <returns>是否需要运行</returns>
        public virtual bool IsNeedRun()
        {
            if (this.ProcessStatus == ProcessStatus.处理中)
            {
                return false;
            }
            // 如果当前时间大于下次运行时间 并且 下次还没有运行，则允许运行
            if (DateTime.Now >= NextRunTime)//&& nextRunFlag == false)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region protected virtual ComputingNextRunTime 计算下一次运行时间
        /// <summary>
        /// 计算下一次运行时间
        /// 如重写，设置nextRunTime及nextRunFlag字段
        /// </summary>
        protected virtual void ComputingNextRunTime()
        {
            this.nextRunTime = this.lastRunTime.AddSeconds(this.IntervalSecond);
        }
        #endregion


        protected void LogMessage(string msg)
        {
            LogHelper.WriteLog(msg + " 任务编号：" + this.NO);
        }
        protected void LogException(string title, Exception ex, string msg = "")
        {
            LogHelper.WriteLog(msg + " 任务编号：" + this.NO, ex);
        }
    }

    /// <summary>
    /// 运行状态
    /// </summary>
    public enum ProcessStatus
    {
        从未执行,
        /// <summary>
        /// 正确完成并待命
        /// </summary>
        待命,
        处理中,
        /// <summary>
        /// 上次失败
        /// </summary>
        异常中
    }
}