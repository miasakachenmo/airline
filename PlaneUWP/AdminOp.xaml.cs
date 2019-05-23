using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace PlaneUWP
{
    public sealed partial class AdminOp : UserControl
    {
        public AirLine airLine;
        ContentDialog father;
        public AdminOp(ContentDialog contentDialog,AirLine _airLine)
        {
            airLine = _airLine;
            this.InitializeComponent();
            father = contentDialog;
            switch(airLine.itemType)
            {
                case ResultPage.PageType.AdminSearchPage:
                    AdminSearchPage();
                    break;
                case ResultPage.PageType.UserMessagePage:
                    UserMessagePage();
                    break;
                case ResultPage.PageType.UserSearchPage:
                    UserSearchPage();
                    break;
            }
            

        }
        public void AdminSearchPage()
        {
            AButton.Content = "取消航班";
            AButton.Click += async (sender, e) => {
                DataBase.Instence.AirlineCanael(airLine.airlinenum, airLine.date);
                father.Hide();
                await new ContentDialog()
                {
                    CloseButtonText = "关闭",
                    Title = $"取消成功!",
                    FullSizeDesired = false
                }.ShowAsync();
                 };
            BButton.Content = "延误";
            BButton.Click += async (sender, e) => {
                if (Input.Text == "")
                    return;
                DataBase.Instence.AirlineLate(airLine,Input.Text); father.Hide();
                await new ContentDialog()
                {
                    CloseButtonText = "关闭",
                    Title = $"延误成功!",
                    FullSizeDesired = false
                }.ShowAsync();
            };

        }
        public void UserMessagePage()
        {
            AButton.Content = "退票";
            AButton.Click += (sender, e) => { father.Hide(); };

            BButton.Visibility = Visibility.Collapsed;
            Input.Visibility = Visibility.Collapsed;
            return;
            
        }
        public void UserSearchPage()
        {
            if ( airLine.remainticket!=0)
            {
                AButton.Content = "买票";
                AButton.Click += async (sender, e) => {
                    DataBase.Instence.AddTicket(App.Instance.UserName, airLine.airlinenum, airLine.date);
                    airLine.remainticket -= 1;
                    father.Hide();
                    await new ContentDialog()
                    {
                        CloseButtonText = "关闭",
                        Title = $"买票成功!",
                        FullSizeDesired = false
                    }.ShowAsync();
                };
            }
            else
            {
                AButton.Content = "抢票";
                AButton.Click += async (sender, e) => {
                    DataBase.Instence.AddTicket(App.Instance.UserName,airLine.airlinenum,airLine.date,"1");
                    await new ContentDialog()
                    {
                        CloseButtonText = "关闭",
                        Title = $"已经加入抢票列表",
                        FullSizeDesired = false
                    }.ShowAsync();
                    father.Hide(); };
            }
            

            BButton.Visibility = Visibility.Collapsed;
            Input.Visibility = Visibility.Collapsed;
            return;
        }

    }
}
