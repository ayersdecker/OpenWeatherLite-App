using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openweather
{
    public class Report
    {
        public string Temp { get; set; }
        public string Weather { get; set; }

        public Report(string _temp, string _weather) 
        {
            Temp = _temp;
            Weather = _weather;
        }
    }
}
