using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace SmartHomeApi
{
    public class SingleValueReading
    {
        public Guid Id {get; set;}
        public DateTime CollectedAtUtc {get; set;}
        public int Location {get; set;}
        public float Value {get; set;}
        public int ReadingType {get; set;}

        public static SingleValueReading Deserialize(JObject jo)
        {
            SingleValueReading ToReturn = JsonConvert.DeserializeObject<SingleValueReading>(jo.ToString());
            if (ToReturn != null)
            {
                return ToReturn;
            }
            else
            {
                throw new Exception("Unable to deserialize SingleValueReading from JSON");
            }
        }

        public static SingleValueReading[] Deserialize(JArray arr)
        {
            List<SingleValueReading> ToReturn = new List<SingleValueReading>();
            foreach (JObject jo in arr)
            {
                SingleValueReading svr = SingleValueReading.Deserialize(jo);
                ToReturn.Add(svr);
            }
            return ToReturn.ToArray();
        }
    }
}