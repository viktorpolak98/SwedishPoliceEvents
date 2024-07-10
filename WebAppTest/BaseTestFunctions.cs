using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApp.Repositories;

namespace WebAppTest
{
    internal class BaseTestFunctions
    {
        internal JsonDocument CreateTestDataDocument(string documentName)
        {
            
            string json;
            string directory = Environment.CurrentDirectory;
            directory = Directory.GetParent(directory).Parent.Parent.FullName;

            string path = $"{directory}\\TestData\\{documentName}"; 

            using (StreamReader reader = new(path))
            {
                json = reader.ReadToEnd();
            }

            return JsonDocument.Parse(json);

        }
    }
}
