using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherPlugin.Models
{
    public class ErrorResponse : IResponse
    {
        public int cod { get; set; }
        public string message { get; set; }
    }
}
