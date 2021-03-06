﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ToastDemo
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            // 判断激活类型 确认是否由Toast通知激活应用
            if (args.Kind == ActivationKind.ToastNotification)
            {
                // 转换参数类型
                var toastargs = (ToastNotificationActivatedEventArgs)args;
                // 获取页面引用
                Frame root = Window.Current.Content as Frame;
                if (root == null)
                {
                    root = new Frame();
                    Window.Current.Content = root;
                }
                if (root.Content == null)
                    root.Navigate(typeof(MainPage));

                MainPage page = (MainPage)root.Content;
                // 判断用户点击哪个按钮
                if (toastargs.Argument == "ok") //确定按钮
                {
                    // 获取用户输入的内容
                    string name = toastargs.UserInput["name"] as string;
                    page.ShowToastArgs(name);
                }
                else //取消按钮
                {
                    //TODO
                }
            }
            Window.Current.Activate();
        }

        private async void RegToastBackgroundTask()
        {
            // 判断一下是否允许访问后台任务
            var status = await BackgroundExecutionManager.RequestAccessAsync();
            if (status == BackgroundAccessStatus.Denied || status == BackgroundAccessStatus.Unspecified)
                return;

            Type type = typeof(BackgroundTasks.ToastBackgroundTask);
            var task = BackgroundTaskRegistration.AllTasks.Values.FirstOrDefault(t => t.Name == type.Name);
            if (task == null)
            {
                // 注册后台任务
                var tBuilder = new BackgroundTaskBuilder();
                tBuilder.Name = type.Name;
                tBuilder.TaskEntryPoint = type.FullName;
                // 声明触发器
                var trigger = new ToastNotificationActionTrigger();
                tBuilder.SetTrigger(trigger);
                task = tBuilder.Register();
            }
        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // 当导航堆栈尚未还原时，导航到第一页，
                // 并通过将所需信息作为导航参数传入来配置
                // 参数
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // 确保当前窗口处于活动状态
            Window.Current.Activate();

            RegToastBackgroundTask();
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }
    }
}
