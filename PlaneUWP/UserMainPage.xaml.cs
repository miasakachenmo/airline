
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
    public sealed partial class UserMainPage : Page
    {
        
        string Date;
        string BeginCity;
        string ArriveCity;
        public UserMainPage()

        {
            
            this.InitializeComponent();
            //MyTicket.Navigate(Type.GetType("PlaneUWP."+"ResultPage"),ResultPage.PageType.UserMessagePage);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            ResultPage.ResultParam param = new ResultPage.ResultParam();
            param.airLines=new DataBase().QueryAirline(BeginCityText.Text, ArriveCityText.Text, DateText.Text);
            param.type = ResultPage.PageType.UserSearchPage;

            MainPage.Instance.JumpTo("ResultPage",param);
        }
    }
}
