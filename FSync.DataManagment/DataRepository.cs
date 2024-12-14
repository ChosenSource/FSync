using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FSync.DataManagment
{
    public class DataRepository : IDisposable
    {
        private static string _datafilepath = "data.json";
        public DataRepository()
        {
            if (!File.Exists("data.json"))
            {
                File.Create("data.json");
            }
        }
        public static List<SyncData> ReadFromJson()
        {
            if (!File.Exists(_datafilepath))
            {
                return new List<SyncData>(); // Return empty list if file does not exist  
            }

            string json = File.ReadAllText(_datafilepath);
            return JsonConvert.DeserializeObject<List<SyncData>>(json) ?? new List<SyncData>();
        }
        private static void SaveToJson(SyncData data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(_datafilepath, json);
        }

        public static SyncData GetByName(string name) 
        
        { 
            var list = ReadFromJson();
            return list.Where(x => x.Name == name).FirstOrDefault();
        
        }

        private static void SaveToJson( List<SyncData> dataList)
        {
            string json = JsonConvert.SerializeObject(dataList, Formatting.Indented);
            File.WriteAllText(_datafilepath , json);
        }

        public static void Delete(string name) 
        {
            List<SyncData> existingData = ReadFromJson();
            existingData.Remove(GetByName(name));
            SaveToJson(existingData);

        }
        public static void Add(SyncData newData)
        {

            if (ReadFromJson() == null)
            {
                SaveToJson(newData);
            }
            else
            {
                if (GetByName(newData.Name) != null)
                {
                    Console.WriteLine("A sync with this name already exists");
                }
                else 
                {
                    List<SyncData> existingData = ReadFromJson();
                    existingData.Add(newData);
                    SaveToJson(existingData);
                }

            }
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
