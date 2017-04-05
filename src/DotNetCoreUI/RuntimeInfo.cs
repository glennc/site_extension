using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreUI
{
    public class RuntimeInfo
    {
        public string TFM { get; set; }
        public Framework Framework { get; set; }

    }

    public class Framework
    {
        public string Name { get; set; }
        public string Version { get; set; }
    }

}
