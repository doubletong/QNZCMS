using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QNZCMS.Services
{
    public class WritableOptions<T> : IWritableOptions<T> where T : class, new()
     {
         private readonly IWebHostEnvironment _environment;
         private readonly IOptionsMonitor<T> _options;
         private readonly IConfigurationRoot _configuration;
         private readonly IConfigurationSection _section;
         private readonly string _file;
    
         public WritableOptions(
             IWebHostEnvironment environment,
             IOptionsMonitor<T> options,
             IConfigurationRoot configuration,
             IConfigurationSection section,
             string file)
         {
             _environment = environment;
             _options = options;
             _configuration = configuration;
             _section = section;
             _file = file;
         }
    
         public T Value => _options.CurrentValue;
         public T Get(string name) => _options.Get(name);
    
         public void Update(Action<T> applyChanges)
         {
             var fileProvider = _environment.ContentRootFileProvider;
             var fileInfo = fileProvider.GetFileInfo(_file);
             var physicalPath = fileInfo.PhysicalPath;
    
             var json = File.ReadAllText(physicalPath);

             var jObject = JsonConvert.DeserializeObject<JObject>(json);
            
             // var sectionPath = key.Split(":")[0];
             
             
             var sectionObject = jObject.TryGetValue(_section.Key, out JToken section) ?
                 JsonConvert.DeserializeObject<T>(section.ToString()) : (Value ?? new T());
    
             applyChanges(sectionObject);

             var sectionPath = _section.Path.Split(":");
             // var pathOne = sectionPath[0];
             // var pathTwo = sectionPath[1];
             jObject[sectionPath[0]][sectionPath[1]][_section.Key] = JObject.Parse(JsonConvert.SerializeObject(sectionObject));
             File.WriteAllText(physicalPath, JsonConvert.SerializeObject(jObject, Formatting.Indented));
             _configuration.Reload();
         }
     }
}