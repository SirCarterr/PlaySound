using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaySound.Model
{
    public class Audio
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string HotKey1 { get; set; }
        public string HotKey2 { get; set; }

        public bool IsEditEnabled { get; set; } = false;
    }
}
