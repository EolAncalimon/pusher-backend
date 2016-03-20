using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubOrgVisualiser.Models
{
    public class NetworkData
    {
        public NetworkData()
        {
            Network = new Dictionary<string, List<string>>();
            NetworkCollabs = new List<NetworkCollab>();
        }
        public Dictionary<string, List<string>> Network { get; set; } 

        public List<NetworkCollab> NetworkCollabs { get; set; } 
    }
}
