using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
    public sealed partial class CreateAirLine : UserControl
    {
        AirLine airline;
        ContentDialog father;
        public CreateAirLine(ContentDialog contentDialog,AirLine newairLine)
        {

         
            this.InitializeComponent();
            father = contentDialog;
            airline = newairLine;
            this.DataContext = this;
        }
        //检测合法性
        public bool IsLegal()
        {
            if (airline.comp == "")
                return false;
            if (airline.airlinenum == "")
                return false;
            if (!IsLegalTimePair(airline.begintime, airline.arrivetime))
                return false;
            if (airline.begincity == "" || airline.arrivecity == "")
                return false;
            if (airline.remainticket < 0)
                return false;
            if(airline.price==0)
            {
                return false;
            }

            return true;
        }
        public static bool IsLegalTimePair(string timestr1, string timestr2)
        {
            var Result1 = Regex.Match(timestr1, "\\d\\d:\\d\\d");
            var Result2 = Regex.Match(timestr2, "\\d\\d:\\d\\d");

            if (!(Result1.Success && timestr1.Length == 5))
                return false;
            if (!(Result2.Success && timestr2.Length == 5))
                return false;
            Result1 = Regex.Match(timestr1, "\\d{2}");
            Result2 = Regex.Match(timestr2, "\\d{2}");

            int H1 = int.Parse(timestr1.Split(":")[0]);
            int H2 = int.Parse(timestr2.Split(":")[0]);
            int M1 = int.Parse(timestr1.Split(":")[1]);
            int M2 = int.Parse(timestr2.Split(":")[1]);
            if (H1 >= 24 || H1 < 0 || H2 >= 24 || H2 < 0 || M1 >= 60 || M1 < 0 || M2 >= 60 || M2 < 0)
                return false;
            if (H2 * 60 + M2 < H1 * 60 + M1)
                return false;
            return true;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if(!IsLegal())
            {
                TipsText.Text = "输入不合法,请检查输入后重新提交";
                return;
            }
            else
            {
                TipsText.Text = "输入合法!";
                DataBase.Instence.AddAirline(airline);
                father.Hide();
                await new ContentDialog()
                {
                    CloseButtonText = "关闭",
                    Title = "插入航班成功!",
                }.ShowAsync();
            }
        }
    }
}
