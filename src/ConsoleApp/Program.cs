using System.Diagnostics;
using System.Text;


string path = "../../test.txt";

int sizeFile = (int)Math.Pow(1024, 3);

await CreateFile(path, sizeFile);

Stopwatch stopwatch = new();
stopwatch.Start();
CountCharactersOfFile(path);
stopwatch.Stop();

Console.WriteLine($"Time running: {stopwatch}");

static async Task CreateFile(string path, long fileSize)
{
    if (File.Exists(path)) return;

    long totalWritten = 0;
    const int bufferSize = 1024;

    using Stream writeFile = File.OpenWrite(path);


    byte[] buffer = new byte[bufferSize];
    Random random = new();
    while (totalWritten < fileSize)
    {
        for (int i = 0; i < bufferSize; i++)
        {
            int num = random.Next(0, 26);
            char letter = (char)('a' + num);
            buffer[i] = (byte)letter;
        }
        int bytesToWrite = (int)Math.Min(bufferSize, fileSize - totalWritten);
        await writeFile.WriteAsync(buffer.AsMemory(0, bytesToWrite));
        totalWritten += bytesToWrite;

        long processing = totalWritten * 100 / fileSize;
        Console.WriteLine($"Processing: {processing} %");
    }
    Console.WriteLine("Created file");
}

static void CountCharactersOfFile(string path)
{
    Console.WriteLine("Init count of characters");
    using Stream fileStream = File.OpenRead(path);

    int bufferSize = 1024;
    byte[] buffer = new byte[bufferSize];

    int bytesRead = 0;
    Dictionary<string, int> tableOfCharacters = [];
    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
    {
        foreach (byte item in buffer)
        {
            string key = Encoding.UTF8.GetString([item]);
            if (!tableOfCharacters.TryGetValue(key, out int value))
                tableOfCharacters.Add(key, 1);
            else tableOfCharacters[key] = value + 1;
        }
    }
    LogDictionary(tableOfCharacters);
    Console.WriteLine("Finish count of characters");
}

static void LogDictionary(Dictionary<string, int> dictionary)
{
    foreach (KeyValuePair<string, int> item in dictionary)
    {
        Console.WriteLine($"{item.Key}: {item.Value}");
    }
}