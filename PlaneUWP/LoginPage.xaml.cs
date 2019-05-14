using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class LoginPage : Page
    {
        public int Debug = 0;
        public LoginPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
          
            base.OnNavigatedTo(e);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string UserName = UserNameInput.Text;
            string PassWord = PassWordInput.Text;
            
            if (Debug==1)
            {
                App.Instance.JumpTo("AdminMainPage", SenderType: typeof(LoginPage));
                return;
            }
            else if(Debug==2)
            {
                App.Instance.JumpTo("UserMainPage", SenderType: typeof(LoginPage));

                return;
            }
            if(PassWord==DataBase.Instence.GetPassWord(UserName))
            {
                if (DataBase.Instence.IsAdmin(UserName))
                {
                    App.Instance.UserName = UserName;
                    App.Instance.UserType = "admin";
                    await new ContentDialog
                    {
                        Title = "登录成功",
                        Content = $"欢迎管理员{UserName}",
                        CloseButtonText = "好的"
                    }.ShowAsync();
                    App.Instance.JumpTo("AdminMainPage", SenderType: typeof(LoginPage));
                    
                }
                else
                {
                    App.Instance.UserName = UserName;
                    App.Instance.UserType = "user";
                    await new ContentDialog
                    {
                        Title = "登录成功",
                        Content = $"欢迎用户{UserName}",
                        CloseButtonText = "好的"
                    }.ShowAsync();
                    App.Instance.JumpTo("UserMainPage", SenderType: typeof(LoginPage));
                }
            }
            else
            {
                await new ContentDialog
                {
                    Title = "登录失败",
                    Content = $"密码或用户名输入错误,请重新登陆",
                    CloseButtonText = "好的"
                }.ShowAsync();
            }
            
        }
    }
}
