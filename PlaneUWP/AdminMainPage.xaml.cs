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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace PlaneUWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AdminMainPage : Page
    {
        public AdminMainPage()
        {
            this.InitializeComponent();
        }
        private async void Create_AirLine(object sender, RoutedEventArgs e)
        {
            var a = new AirLine();
            a.airlinenum = "test";
            a.comp = "testcomp";
            a.begincity = "";
            var temp = new ContentDialog();
            temp.Content = new CreateAirLine(temp, a);
            temp.CloseButtonText = "关闭";
            await temp.ShowAsync();

        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ResultPage.ResultParam param = new ResultPage.ResultParam();

            if (BeginCityText.Text == "" | ArriveCityText.Text == "" | DateText.Text == "")
            {
                await new ContentDialog
                {
                    Title = "请填写完整信息!",
                    CloseButtonText = "关闭",
                }.ShowAsync();
                return;
            }
            param.airLines = DataBase.Instence.QueryAirline(BeginCityText.Text, ArriveCityText.Text, DateText.Text);
            param.type = ResultPage.PageType.AdminSearchPage;
            DataPresenter.Navigate(Type.GetType("PlaneUWP.ResultPage_AdminMain"), param);
        }
        private async void Button_Click2(object sender, RoutedEventArgs e)
        {
            ResultPage.ResultParam param = new ResultPage.ResultParam();

            if (AirLineNum.Text=="")
            {
                await new ContentDialog
                {
                    Title = "请填写航班号!",
                    CloseButtonText = "关闭",
                }.ShowAsync();
                return;
            }
            param.airLines = DataBase.Instence.QueryAirlineByAirLineNum(AirLineNum.Text);
            param.type = ResultPage.PageType.AdminSearchPage;
            DataPresenter.Navigate(Type.GetType("PlaneUWP.ResultPage_AdminMain"), param);
        }
    }
}
