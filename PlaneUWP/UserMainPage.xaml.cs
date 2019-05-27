
using System;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace PlaneUWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class UserMainPage : Page
    {
       
        string Date;
        string BeginCity;
        string ArriveCity;
        ObservableCollection<string> Messages;
        public UserMainPage()

        {
            
            this.InitializeComponent();
            
            ResultPage.ResultParam param = new ResultPage.ResultParam();
            param.airLines = DataBase.Instence.GetBuyedTickets(App.Instance.UserName);
            param.type = ResultPage.PageType.UserMessagePage;
            MyTicket.Navigate(Type.GetType("PlaneUWP.ResultPage_UserMain"), param);
            Messages = new ObservableCollection<string>(DataBase.Instence.GetMessage(App.Instance.UserName));
            this.DataContext = this;

        }

        private void Button_Click_Test(object sender, RoutedEventArgs e)
        {
            ResultPage.ResultParam param = new ResultPage.ResultParam();
            param.airLines = new DataBase().QueryAirline("包头","北京","6.1");
            param.type = ResultPage.PageType.UserSearchPage;
            App.Instance.JumpTo("ResultPage", param);
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            if(BeginCityText.Text==""|ArriveCityText.Text==""|DateText.Text=="")
            {
                await new ContentDialog
                {
                    Title = "请填写完整信息!",
                    CloseButtonText = "关闭",
                }.ShowAsync();
                return;
            }
            ResultPage.ResultParam param = new ResultPage.ResultParam();
            param.airLines=new DataBase().QueryAirline(BeginCityText.Text, ArriveCityText.Text, DateText.Text);
            bool NeedRecommand = true;
            foreach(var anairline in param.airLines)
                if (!(anairline._status.iscanceled|anairline.remainticket==0))//两种不能去的情况
                    NeedRecommand = false;
            param.type = ResultPage.PageType.UserSearchPage;
            if (!NeedRecommand)
            {
                param.type = ResultPage.PageType.UserSearchPage;
                App.Instance.JumpTo("ResultPage", param);
            }
            else
            {
                var Dialog = new MessageDialog("无可用航班,是否需要智能推荐?");
               
                Dialog.Commands.Add(new UICommand("好的", (c) => {
                    Debug.Print("调用了智能推荐!");
                    try
                    {
                        param.airLines = DataBase.Instence.GetRecommend(BeginCityText.Text, ArriveCityText.Text, DateText.Text, DataBase.Instence.GetDayAirLines(DateText.Text));


                    }
                    catch(StackOverflowException)
                    {
                        param.airLines = new List<AirLine>();
                    }
                    catch
                    {
                        param.airLines = new List<AirLine>();
                    }
                    param.type = ResultPage.PageType.UserSearchPage;
                    App.Instance.JumpTo("ResultPage", param);
                    return; }));
                Dialog.Commands.Add(new UICommand("不用了", (c) => { return; }));
                await Dialog.ShowAsync();

            }
           
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var a = App.Instance.rootFrame.BackStackDepth;
            base.OnNavigatedTo(e);
            var b = App.Instance.rootFrame.BackStackDepth;

        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataBase.Instence.DelAllMessage(App.Instance.UserName);
            Messages.Clear();
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (AirLineNum.Text == "")
            {
                await new ContentDialog
                {
                    Title="请填写航班号!",
                    CloseButtonText = "关闭",
                }.ShowAsync();
                return;
            }
            string airlinenum = AirLineNum.Text;
            ResultPage.ResultParam param = new ResultPage.ResultParam();
            param.airLines = DataBase.Instence.QueryAirlineByAirLineNum(airlinenum);
            param.type = ResultPage.PageType.UserSearchPage;
            App.Instance.JumpTo("ResultPage", param);

        }
    }
}
