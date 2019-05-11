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
            AButton.Content = "取消";
            AButton.Click += (sender, e) => { father.Hide(); };
            BButton.Content = "延误";
            BButton.Click += (sender, e) => { father.Hide(); };

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
            if (Int32.Parse( airLine.remainticket)!=0)
            {
                AButton.Content = "买票";
            }
            else
            {
                AButton.Content = "抢票";
            }
            AButton.Click += (sender, e) => {father.Hide();};

            BButton.Visibility = Visibility.Collapsed;
            Input.Visibility = Visibility.Collapsed;
            return;
        }

    }
}
