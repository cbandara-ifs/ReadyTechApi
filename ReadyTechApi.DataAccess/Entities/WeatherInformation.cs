using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadyTechApi.DataAccess.Entities
{
    public class WeatherInformation
    {
        public main main { get; set; }
    }

    public class main
    {
        public double temp { get; set; }
    }
}
