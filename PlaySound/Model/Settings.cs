using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaySound.Model
{
    public class Settings
    {
        public string DirectoryPath { get; set; }
        public string PlayAudioKey { get; set; }
        public string StopAudioKey { get; set; }
        public string NextAudioKey { get; set; }
        public string PreviousAudioKey { get; set; }
    }
}
