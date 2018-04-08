using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Weather_Apps
{
    [DataContract]
    class Response5Days
    {
        [DataMember(Name = "city")]
        public City city { get; set; }

        [DataMember(Name = "cod")]
        public string Cod { get; set; }

        [DataMember(Name = "list")]
        public List[] List { get; set; }
    }

    [DataContract]
    public class List
    {
        [DataMember(Name = "main")]
        public Main1 main1 { get; set; }
    }

    [DataContract]
    public class Main1
    {
        [DataMember(Name = "temp")]
        public string Temp1 { get; set; }
    }

    [DataContract]
    public class City
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
