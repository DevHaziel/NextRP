using BrokeProtocol.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextRP
{
    public class Core : Plugin
    {
        public static Core Instance { get; private set; }
        public static Random rnd = new Random();
        public Core()
        {
            Instance = this;
            Info = new PluginInfo("Next RP", "next");
        }
    }
}
