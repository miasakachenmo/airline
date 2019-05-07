
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace PlaneUWP
{
    class AirLineButton:Button
    {
        public AirLine airLine;
        public AirLineButton(AirLine temp):base()
        {
            airLine = temp;
        }
    }
}
