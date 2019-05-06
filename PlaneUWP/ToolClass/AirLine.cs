using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneUWP.ToolClass
{
    public class AirLine
    {
        public string comp;
        public string airlinenum;
        public string begintime;
        public string arrivetime;

        public string remainticket;
        public string cross;

        public string begincity;
        public string arrivecity;

        public string date;
        public Status status;
        public class Status
        {
            public bool islate=false;
            public bool iscanceled=false;
            public string newtime="";
        }
    }

}
