using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace PlaneUWP
{
    public class AirLine:INotifyPropertyChanged
    {
        public string comp;
        public string airlinenum;
        public string begintime;
        public string arrivetime;

        public int _remainticket;
        public int remainticket
        {
            get
            {
                return _remainticket;
            }

            set
            {
                _remainticket = value;PropertyChanged(this, new PropertyChangedEventArgs("remainticket"));
            }
        }
        public string cross;

        public string begincity;
        public string arrivecity;

        public string date;
        public Status status;

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
                return status.islate ? "是" : "否";
            }
        }
        public class Status
        {
            public bool islate=false;
            public bool iscanceled=false;
            public string newtime="";
        }
    }

}
