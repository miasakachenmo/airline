﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ObservableCollection<AirLine> airLines;
        public AdminOp(ContentDialog contentDialog,AirLine _airLine,ObservableCollection<AirLine> _airLines=null)
        {
            airLines = _airLines;
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
                if(airLine._status.iscanceled)
                {
                    father.Hide();
                    return;
                }
                DataBase.Instence.AirlineCanael(airLine);
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
                if (airLine._status.iscanceled)
                {
                    father.Hide();
                    await new ContentDialog()
                    {
                        CloseButtonText = "关闭",
                        Title = $"您不能延误一个已经取消的航班",
                        FullSizeDesired = false
                    }.ShowAsync();
                    return;
                }
                if (Input.Text == "")
                {
                    

                    father.Hide();
                    await new ContentDialog()
                    {
                        CloseButtonText = "关闭",
                        Title = $"请输入延误时间(分钟)",
                        FullSizeDesired = false
                    }.ShowAsync();
                    return;
                }
                try
                {
                    var a= int.Parse(Input.Text);
                    if(a<0)
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    father.Hide();
                    await new ContentDialog()
                    {
                        CloseButtonText = "关闭",
                        Title = $"时间格式有误",
                        FullSizeDesired = false
                    }.ShowAsync();
                    return;
                }
            DataBase.Instence.AirlineLate(airLine,Input.Text);
                father.Hide();
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
            AButton.Click += async (sender, e) => { DataBase.Instence.DelTicket(App.Instance.UserName,airLine.airlinenum,airLine.date); father.Hide();
            await new ContentDialog()
                {
                    CloseButtonText = "关闭",
                    Title = $"退票成功!",
                    FullSizeDesired = false
                }.ShowAsync();
                airLines.Remove(airLine);
            };

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
                    father.Hide();
                    await new ContentDialog()
                    {
                        CloseButtonText = "关闭",
                        Title = $"已经加入抢票列表,抢票成功时将通知您",
                        FullSizeDesired = false
                    }.ShowAsync();
                };
            }
            

            BButton.Visibility = Visibility.Collapsed;
            Input.Visibility = Visibility.Collapsed;
            return;
        }

    }
}
