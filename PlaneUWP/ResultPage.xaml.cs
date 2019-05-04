using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
        StackPanel[] stackPanels;
        public void AddLine(string[] data)
        {

            for(int i=0;i<data.Length;i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.FontSize = 20;
                textBlock.Text = data[i];
                stackPanels[i].Children.Add(textBlock);
            }
            Button button = new Button();
            button.Content = "购票!";
            button.Background = new SolidColorBrush(Colors.Azure);
            Buy.Children.Add(button);

        }
        public ResultPage()
        {
            this.InitializeComponent();
            StackPanel[] t = { PlaneNumber, CompName, BeginPlace, ArrivePlace, BeginTime, ArriveTime, Mid, IsLate ,LastTicket};
            stackPanels = t;
            string[] temps = { "A0001", "南方航空", "长春", "南阳", "13:40", "15:50", "无", "否" };
            for(int i=0;i<50;i++)
                AddLine(temps);

        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {

        }
    }
}
