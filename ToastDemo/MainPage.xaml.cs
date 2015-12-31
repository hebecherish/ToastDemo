using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace ToastDemo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            //string xml = "<toast scenario=\"reminder\">" +
            //              "<visual>" +
            //                  "<binding template=\"ToastGeneric\">" +
            //                      "<text>Title</text>" +
            //                      "<image placement=\"inline\" src=\"ms-appx:///Assets/1.jpg\" />" +
            //                  "</binding>" +
            //              "</visual>" +
            //           "</toast>";
            //ShowToast(xml);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var settings = ApplicationData.Current.LocalSettings;
            var val = settings.Values["name"];
            if (null!=val)
            {
                string name = val.ToString();
                //TODO
                ShowToastArgs(name);
            }
            base.OnNavigatedTo(e);
        }

        public void ShowToastArgs(string arg)
        {
            txtArg.Text = arg;
        }

        private void foregroundToast_Click(object sender, RoutedEventArgs e)
        {
            string xml = "<toast>" +
                          "<visual>" +
                              "<binding template=\"ToastGeneric\">" +
                                  "<text>Title</text>" +
                              "</binding>" +
                          "</visual>" +
                          "<actions>" +
                                  "<input id=\"name\" type=\"text\" placeHolderContent=\"请输入姓名\" />" +
                                  "<action content = \"确定\" arguments = \"ok\" activationType=\"foreground\"/>" +
                                  "<action content = \"取消\" arguments = \"cancel\" activationType=\"foreground\"/>" +
                          "</actions >" +
                       "</toast>";
            ShowToast(xml);
        }

        private void backgroundToast_Click(object sender, RoutedEventArgs e)
        {
            string xml = "<toast>" +
                        "<visual>" +
                            "<binding template=\"ToastGeneric\">" +
                                "<text>Title</text>" +
                            "</binding>" +
                        "</visual>" +
                        "<actions>" +
                                "<input id=\"name\" type=\"text\" placeHolderContent=\"请输入姓名\" />" +
                                "<action content = \"确定\" arguments = \"ok\" activationType=\"background\"/>" +
                                "<action content = \"取消\" arguments = \"cancel\" activationType=\"background\"/>" +
                        "</actions >" +
                     "</toast>";
            ShowToast(xml);
        }

        private static void ShowToast(string xml)
        {
            // 创建XML文档
            XmlDocument doc = new XmlDocument();
            // 加载XML
            doc.LoadXml(xml);
            // 创建通知实例
            ToastNotification notification = new ToastNotification(doc);
            // 显示通知
            ToastNotifier nt = ToastNotificationManager.CreateToastNotifier();
            nt.Show(notification);
        }
    }
}
