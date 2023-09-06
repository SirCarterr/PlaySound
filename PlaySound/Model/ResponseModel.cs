using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaySound.Model
{
    public class ResponseModel
    {
        public object Data { get; set; }
        public bool IsError { get; set; }
        public string Error { get; set; }
    }
}
