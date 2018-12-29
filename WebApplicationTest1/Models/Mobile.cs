using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationTest1.Models
{
    [DataContract]
    public class Mobile
    {
        [Required(ErrorMessage = "Введите название")]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string SourceURL { get; set; }

        public Mobile()
        {

        }

        public Mobile(string name, string source)
        {
            Name = name;
            SourceURL = source;
        }
    }
}
