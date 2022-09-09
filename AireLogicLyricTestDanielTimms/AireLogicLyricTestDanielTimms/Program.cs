using System.Threading.Tasks;
using System.Net.Http;
using AireLogicLyricTestDanielTimms;
using System.Text.Json;
using System;
using System.Collections.Generic;

public class Program
{
    //private static readonly HttpClient client = new HttpClient();

    //private const string apiKey = "&apikey=3389b79f930d106a7c59d70857e297a1";
    //private const string rootURL = "https://api.musixmatch.com/ws/1.1/";
    //private const string sampleSearch = "artist.search?q_artist=prodigy&page_size=5/";
    //private const string lyricsMatcher = "matcher.lyrics.get";
    //private const string artistSearch = "artist.search";
    //private const string format = "?format=json&callback=callback";
    //private const string artist = "&q_artist=";
    //private const string track = "&q_track=";

    static async Task Main(string[] args)
    {
       // await ProcessRepositories();
    }

    //private static async Task ProcessRepositories()
    //{
    //    string url = rootURL + artistSearch + format + artist + "Avenged Sevenfold" + apiKey;
    //    var stringTask = client.GetStreamAsync(url);
    //    Console.WriteLine(url);
    //    var data = await JsonSerializer.DeserializeAsync<List<artist>>(await stringTask);
    //   // var msg = await stringTask;
    //    //Console.Write(msg);
    //    Console.WriteLine();
    //    //var parsed = JsonConvert.DeserializeObject<List<artist>>(msg);
    //    Console.Write(data);
    //}
}