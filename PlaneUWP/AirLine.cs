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
            status = new Status() { father = this,_islate=false,iscanceled=false,newtime="" };
            
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
        public class Status
        {

            public AirLine father;
            public bool _islate=false;
            public bool islate { get { return _islate; } set
                {
                    _islate = value;
                    if(father!=null)
                        father.PropertyChanged(father, new PropertyChangedEventArgs("islatestr"));
                }
            }
            public bool iscanceled=false;
            public string newtime="";
        }
    }

}
