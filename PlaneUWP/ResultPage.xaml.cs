
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    class OList<T> : List<T>, INotifyCollectionChanged
    {

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
    public sealed partial class ResultPage : Page
    {
        ObservableCollection<AirLine> b;
        ObservableCollection<AirLine> airLines;

        StackPanel[] stackPanels;


        PageType type;


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            type = PageType.UserSearchPage;

            var temp = ((ResultParam)e.Parameter).airLines;
            airLines = new ObservableCollection<AirLine>(temp);

            foreach (AirLine airLine in airLines)
            {
                airLine.itemType = type;
            }

        }

        public enum PageType { UserMessagePage, UserSearchPage, AdminSearchPage };
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
            contentDialog.Content = new AdminOp(contentDialog, temp);
            await contentDialog.ShowAsync();
        }



        public int Order=1;//0升序,1降序
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string Item = (string)((ComboBox)sender).SelectedValue;
            OrderByProName(Item);
            SelectionChangedEventArgs a = e;
            int o = 2;
            return;
        }
        public void OrderByProName(string Item)
        {
            switch (Item)
            {
                case "余票":
                    ObSort(airLines, (a1, a2) => { return a1._remainticket > a2._remainticket; });
                    break;
                case "票价":
                    ObSort(airLines, (a1, a2) => { return a1.price > a2.price; });
                    break;
                case "时间":
                    ObSort(airLines, (a1, a2) => { return a1.flytime > a2.flytime; });
                    break;

            }
        }
        
        public delegate bool SortMethod(AirLine a1, AirLine a2);
         
        public void ObSort(ObservableCollection<AirLine> airLines,SortMethod Sort)
        {
            AirLine temp;
            bool CompareResult;
            for (int i = 0; i < airLines.Count; i++)
            {
                for (int j = 0; j < airLines.Count - i - 1; j++)
                {
                    CompareResult = Sort(airLines[j], airLines[j + 1]);
                    if ((CompareResult && Order == 1)|  ((!CompareResult)&&(Order!=1))  )
                    {
                        temp = airLines[j];
                        airLines[j] = airLines[j + 1];
                        airLines[j + 1] = temp;
                    }
                }
                    
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Order = (Order + 1) % 2;
            OrderByProName((string)Select.SelectedValue);
            ((Button)sender).Content = Order == 1 ? "升" : "降";
        }
    }
    
}
