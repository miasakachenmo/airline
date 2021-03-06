﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace PlaneUWP
{
    public class AirLine:INotifyPropertyChanged
    {
        public static int MinusTime(string timestr1,string timestr2)
        {
            timestr1 = timestr1.Replace(".",":");
            timestr2 = timestr2.Replace(".", ":");
            var Result1 = Regex.Match(timestr1, "\\d{2}");
            var Result2 = Regex.Match(timestr2, "\\d{2}");

            int H1 = int.Parse(timestr1.Split(":")[0]);
            int H2 = int.Parse(timestr2.Split(":")[0]);
            int M1 = int.Parse(timestr1.Split(":")[1]);
            int M2 = int.Parse(timestr2.Split(":")[1]);
            return (H1 - H2) * 60 + (M1 - M2);

        }

        public string comp;
        public string airlinenum;
        public string begintime;
        public string _arrivetime;
        public AirLine()
        {
            comp = "";
            airlinenum = "";
            begintime = "";
            arrivetime = "";
            begincity = "";
            arrivecity = "";
            _remainticket = 0;
            date = "";
            price = 0;
            status = new Status() { father = this,_islate=false,iscanceled=false,newtime="" };
            
        }
        public AirLine(AirLine a)
        {
            comp = a.comp;
            airlinenum = a.airlinenum;
            begintime = a.begintime;
            arrivetime = a.arrivetime;
            begincity = a.begincity;
            arrivecity = a.arrivecity;
            _remainticket = a._remainticket;
            date = a.date;
            price = a.price;
            status = new Status() { father = this, _islate = a._status._islate, _iscanceled = a._status._iscanceled, newtime = a._status._newtime };

        }
        //飞行时间:分钟
        public int flytime
        {
            get
            {
                string[] time1= begintime.Replace(".", ":").Split(":");
                string[] time2 = arrivetime.Replace(".", ":").Split(":");
                int m1 = int.Parse(time1[1]) + int.Parse(time1[0]) * 60;
                int m2 = int.Parse(time2[1]) + int.Parse(time2[0]) * 60; 
                return m2-m1;
            }
        }
        public int getValue()
        {
            return (price + this.flytime);
        }
        public string arrivetime {
            get
            {
                //if(status.islate)
               //     return (DateTime.Parse(_arrivetime.Replace(".", ":")) + new TimeSpan(0, 0, Int32.Parse(status.newtime), 0, 0)).ToString("hh:mm");
                return _arrivetime;
            }
            set
            {
                _arrivetime = value;
            }
        }
        public int _remainticket;
        public int remainticket
        {
            get
            {
                return _remainticket;
            }

            set
            {
                _remainticket = value;
                PropertyChanged(this, new PropertyChangedEventArgs("remainticket"));
            }
        }
        public string cross;
        public int _price;
        public int price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
                PropertyChanged(this, new PropertyChangedEventArgs("price"));

            }
        }
        public string begincity;
        public string arrivecity;

        public string date;
        public Status _status;
        public Status status
        {
            get
            {
                return _status;
            }
            set
            {
                value.father=this;
                _status = value;
                
            }
        }
        
        public Visibility Seeable
        {
            get
            {
                return itemType == ResultPage.PageType.UserMessagePage ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public ResultPage.PageType itemType;

        public event PropertyChangedEventHandler PropertyChanged=delegate { };

        public string islatestr
        {
            get
            {
                if (status.islate)
                    return status.newtime + "分";
                return "否";
            }
            set
            {

            }
        }
        public bool Show
        {
            get
            {
                return !_status.iscanceled;
            }
        }
        public string StatusStr
        {
            get
            {
                if (status.iscanceled)
                    return "已取消";
                if (status._islate)
                    return "晚点"+islatestr;
                else
                    return "正常";
            }
            set
            {

            }
        }
        public class Status
        {

            public AirLine father;
            public bool _islate=false;
            public bool islate { get { return _islate; } set
                {
                    _islate = value;
                    if(father!=null)
                        father.PropertyChanged(father, new PropertyChangedEventArgs("StatusStr"));
                }
            }
            public bool iscanceled
            {
                get
                {
                    return _iscanceled;
                }
                set
                {
                    _iscanceled = value;
                    if (father != null)
                        father.PropertyChanged(father, new PropertyChangedEventArgs("StatusStr"));
                }
            }
            public bool _iscanceled=false;
            public string _newtime="";
            public string newtime
            {
                get
                {
                    return _newtime;

                }
                set
                {
                    _newtime = value;
                    if (father != null)
                        father.PropertyChanged(father, new PropertyChangedEventArgs("StatusStr"));
                }
            }

        }
    }

}
