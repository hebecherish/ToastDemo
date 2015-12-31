using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Notifications;

namespace BackgroundTasks
{
    public sealed class ToastBackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            var details = taskInstance.TriggerDetails as ToastNotificationActionTriggerDetail;
            if (details != null)
            {
                string arg = details.Argument;
                if (arg == "ok")//判断用户是否点击ok
                {
                    // 保存数据
                    var settings = ApplicationData.Current.LocalSettings;
                    settings.Values["name"] = details.UserInput["name"];// 获取选择的项
                }
            }
            deferral.Complete();
        }
    }
}
