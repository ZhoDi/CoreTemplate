using System;
using System.Threading;
using System.Timers;

namespace CommonUtils
{
    /// <summary>
    /// 进程相关
    /// </summary>
    public class ThreadUtil
    {
        /// <summary>
        /// 新建时间片，无需开启，立即执行
        /// </summary>
        public static System.Threading.Timer TimerOnce(Action action, int second)
        {
            var timer = new System.Threading.Timer(delegate (object arg)
            {
                action();
            }, null, 0, second * 1000);
            return timer;
        }

        /// <summary>
        /// 新建时间片，手动开启，延时执行
        /// </summary>
        public static System.Timers.Timer TimerDelay(Action action, int second)
        {
            var timer = new System.Timers.Timer(second * 1000);
            timer.Elapsed += delegate (object sender, ElapsedEventArgs e)
            {
                action();
            };
            return timer;
        }

        /// <summary>
        /// 新建线程
        /// </summary>
        public static Thread New(ThreadStart action)
        {
            return new Thread(action);
        }

        /// <summary>
        /// 线程执行
        /// </summary>
        public static void Start(ThreadStart action)
        {
            new Thread(action).Start();
        }

        /// <summary>
        /// 等待,单位:秒
        /// </summary>
        public static void Wait(int second)
        {
            Thread.Sleep(second * 1000);
        }

        /// <summary>
        /// 等待,单位:秒
        /// </summary>
        public static void Wait(double second)
        {
            Thread.Sleep((int)(second * 1000));
        }

        /// <summary>
        /// 休眠,单位:秒
        /// </summary>
        public static void Sleep(int second)
        {
            Thread.Sleep(second * 1000);
        }

        /// <summary>
        /// 休眠,单位:秒
        /// </summary>
        public static void Sleep(double second)
        {
            Thread.Sleep((int)(second * 1000));
        }

        #region 循环

        /// <summary>
        /// 为线程循环定义循环体
        /// </summary>
        private static System.Timers.Timer mLoopTimer = null;

        /// <summary>
        /// 为线程循环定义委托
        /// </summary>
        private static Action mLoopAction;

        /// <summary>
        /// 执行委托
        /// </summary>
        private static void LoopRun()
        {
            if (mLoopAction != null)
                try
                {
                    mLoopAction();
                }
                catch (Exception ex)
                {
                    LogUtil.Log(ex);
                }
        }

        /// <summary>
        /// 为线程循环定义方法
        /// </summary>
        private static void LoopElapsed(object sender, ElapsedEventArgs e)
        {
            LoopRun();
        }

        /// <summary>
        /// 开始线程循环
        /// </summary>
        /// <param name="second"></param>
        /// <param name="ifStartOnce"></param>
        private static void Loop(double second, bool ifStartOnce)
        {
            if (ifStartOnce)
            {
                Thread thread = new Thread(new ThreadStart(LoopRun));
                thread.IsBackground = true;
                thread.Start();
            }
            mLoopTimer = new System.Timers.Timer();
            mLoopTimer.Interval = second * 1000;
            mLoopTimer.Elapsed += LoopElapsed;
            mLoopTimer.Start();
        }

        /// <summary>
        /// 开始线程循环,仅支持一个方法
        /// </summary>
        public static void Loop(Action action, double second, bool ifStartOnce = true)
        {
            ClearLoop();
            mLoopAction = action;
            Loop(second, ifStartOnce);
        }

        /// <summary>
        /// 关闭线程循环
        /// </summary>
        public static void ClearLoop()
        {
            if (mLoopTimer != null)
            {
                mLoopTimer.Stop();
                mLoopTimer.Close();
                mLoopTimer.Dispose();
            }
            mLoopAction = null;
        }

        #endregion

        #region 计划

        /// <summary>
        /// 为线程循环定义循环体
        /// </summary>
        private static System.Timers.Timer mPlanTimer = null;

        /// <summary>
        /// 为线程循环定义委托
        /// </summary>
        private static Action mPlanAction;

        /// <summary>
        /// 为线程循环定义方法
        /// </summary>
        private static void PlanElapsed(object sender, ElapsedEventArgs e)
        {
            if (mPlanTimer != null)
            {
                mPlanTimer.Stop();
                mPlanTimer.Close();
                mPlanTimer.Dispose();
            }
            if (mPlanAction != null)
            {
                mPlanAction();
            }
            mPlanAction = null;
        }

        /// <summary>
        /// 计划执行
        /// </summary>
        public static void Plan(Action action, double second)
        {
            mPlanAction = action;
            mPlanTimer = new System.Timers.Timer();
            mPlanTimer.Interval = second * 1000;
            mPlanTimer.Elapsed += PlanElapsed;
            mPlanTimer.Start();
        }

        /// <summary>
        /// 取消执行
        /// </summary>
        public static void CancelPlan()
        {
            if (mPlanTimer != null)
            {
                mPlanTimer.Stop();
                mPlanTimer.Close();
                mPlanTimer.Dispose();
            }
            mPlanAction = null;
        }

        #endregion
    }
}
