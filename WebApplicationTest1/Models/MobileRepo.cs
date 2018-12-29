using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;


namespace WebApplicationTest1.Models
{
    public class MobileRepo : IRepository<Mobile>
    {
        List<Mobile> mobiles = new List<Mobile>();

        DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<Mobile>));

        public void Create(Mobile t)
        {
            mobiles = GetAll() as List<Mobile>;
            mobiles.Add(t);
            using (FileStream fs = new FileStream("wwwroot/data/mobile.json", FileMode.Create))
            {
                
                jsonFormatter.WriteObject(fs, mobiles);
            }
        }

        public void Delete(IEnumerable<Mobile> t)
        {
            using (FileStream fs = new FileStream("wwwroot/data/mobile.json", FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, t);
            }
        }

        public Mobile Get(string name)
        {
            //unnesessary
            throw new NotImplementedException();
        }

        public IEnumerable<Mobile> GetAll()
        {
            
            using (FileStream fs = new FileStream("wwwroot/data/mobile.json", FileMode.OpenOrCreate))
            {
                mobiles = (List<Mobile>)jsonFormatter.ReadObject(fs);
            }
            return mobiles;
        }

        public void Update(Mobile t)
        {
            mobiles = GetAll() as List<Mobile>;
            mobiles.Add(t);
            using (FileStream fs = new FileStream("wwwroot/data/mobile.json", FileMode.Create))
            {

                jsonFormatter.WriteObject(fs, mobiles);
            }
        }
    }
}
