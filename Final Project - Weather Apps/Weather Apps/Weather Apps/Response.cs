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
    class Response
    {
        [DataMember(Name = "cod")]
        public string Cod { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "base")]
        public string Bas { get; set; }

        [DataMember(Name = "main")]
        public Main main { get; set; }

        [DataMember(Name = "wind")]
        public Wind wind { get; set; }

        [DataMember(Name = "weather")]
        public Weather[] Weather { get; set; }

        [DataMember(Name = "sys")]
        public Sys sys { get; set; }

    }

    [DataContract]
    public class Sys
    {
        [DataMember(Name = "sunrise")]
        public string Sunrise { get; set; }
        [DataMember(Name = "sunset")]
        public string Sunset { get; set; }
    }

    [DataContract]
    public class Weather
    {
        [DataMember(Name = "description")]
        public string description { get; set; }
        [DataMember(Name = "icon")]
        public string icon { get; set; }
    }

    [DataContract]
    public class Main
    {
        [DataMember(Name = "temp")]
        public string Temp { get; set; }

        [DataMember(Name = "pressure")]
        public string Pressure { get; set; }

        [DataMember(Name = "humidity")]
        public string Humidity { get; set; }
    }

    [DataContract]
    public class Wind
    {
        [DataMember(Name = "speed")]
        public string Speed { get; set; }
    }

}
