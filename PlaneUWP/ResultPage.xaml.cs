
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
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
    public sealed partial class ResultPage : Page
    {
        List<AirLine> airLines;
        StackPanel[] stackPanels;

        
        PageType type;
        

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
       App.Instance.rootFrame.CanGoBack ?
       AppViewBackButtonVisibility.Visible :
       AppViewBackButtonVisibility.Collapsed;

            base.OnNavigatedTo(e);
            type = ((ResultParam)e.Parameter).type;
            airLines= ((ResultParam)e.Parameter).airLines;

            foreach (AirLine airLine in airLines)
            {
                airLine.itemType = type;
            }
        }

        public enum PageType {UserMessagePage,UserSearchPage,AdminSearchPage};
        public class ResultParam
        {

            public List<AirLine> airLines;
            public PageType type;

        }
        /*
        public void AddLine(AirLine airLine)
        {
            // PlaneNumber, CompName, BeginPlace, ArrivePlace, BeginTime, ArriveTime, Mid, IsLate ,LastTicket
            string[] temp = { airLine.airlinenum, airLine.comp, airLine.begincity, airLine.arrivecity, airLine.begintime, airLine.arrivetime,"无" ,airLine.status.islate?"是":"否",airLine.remainticket};
            AddLine(temp,airLine);
        }
        public void AddLine(string[] data,AirLine airLine)
        {
            
            for(int i=0;i<data.Length;i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Height = 25;

                  
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                textBlock.Margin = new Thickness(0, 3, 0, 0);

                textBlock.FontSize = 15;
                textBlock.Text = data[i];
                if (i == 1)
                    textBlock.FontSize = 10;
                stackPanels[i].Children.Add(textBlock);
            }
            
            
            AirLineButton button = new AirLineButton(airLine);
            
            button.FontSize = 12;
            button.Height = 28;
            
            button.HorizontalAlignment = HorizontalAlignment.Center;
            button.VerticalAlignment = VerticalAlignment.Top;
            switch(type)
            {
                case PageType.UserSearchPage:
                    button.Content = "购票!";
                    button.Click += ButtonBuy;
                    break;
                case PageType.UserMessagePage:
                    button.Content = "退票";
                    button.Click += ButtonCancel;
                    break;
                case PageType.AdminSearchPage:
                    button.Content = "操作";
                    button.Click += ButtonLate;
                    break;
                    
            }


            //Buy.Children.Add(button);
        }
        public void ButtonBuy(object sender, RoutedEventArgs e)//买票
        {

        }
        public void ButtonCancel(object sender, RoutedEventArgs e)//退票
        {

        }
        public async void ButtonLate(object sender, RoutedEventArgs e)//管理员设置延误或取消
        {
            AirLineButton temp = (AirLineButton)sender;
            var contentDialog = new ContentDialog()
            {
                Title=$"输入你希望对{temp.airLine.date}航班{temp.airLine.airlinenum}进行的操作",
                FullSizeDesired = false
            };
            
            contentDialog.Content = new AdminOp(contentDialog);
            await contentDialog.ShowAsync();
            
        }
        */

        public ResultPage()
        {
            this.InitializeComponent();
            this.DataContext = this;

            //StackPanel[] t = { PlaneNumber, CompName, BeginPlace, ArrivePlace, BeginTime, ArriveTime, Mid, IsLate ,LastTicket};
            //stackPanels = t;
        }
        
        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            AirLine temp = (AirLine)e.ClickedItem;
            var contentDialog = new ContentDialog()
            {
                CloseButtonText = "关闭",
                Title = $"输入你希望对{temp.date}航班{temp.airlinenum}进行的操作",
                FullSizeDesired = false
            };
            contentDialog.Content = new AdminOp(contentDialog,temp);
            await contentDialog.ShowAsync();
        }
    }
}
