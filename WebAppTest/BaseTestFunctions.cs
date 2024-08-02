using System;
using System.IO;
using System.Text.Json;

namespace WebAppTest;

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
