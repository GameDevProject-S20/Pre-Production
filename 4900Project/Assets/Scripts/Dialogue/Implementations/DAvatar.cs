using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogue
{
    /// <summary>
    /// Simple implementation for an Avatar
    /// </summary>
    class DAvatar : IDAvatar
    {
        public string Name { get; set; }
        public string PathToImage { get; set; }
    }
}
