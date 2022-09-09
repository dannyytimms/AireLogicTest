using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AireLogicTestDanTimms
{
    public class LyricAverager
    {
        private static readonly HttpClient client = new HttpClient();

        //API call data:
        private const string rootURL = "https://api.musixmatch.com/ws/1.1/";
        private const string apiKey = "&apikey=3389b79f930d106a7c59d70857e297a1";

        private const string artistSearchQuery = "artist.search?";
        private const string albumSearch = "artist.albums.get?";

        private const string albumTracksGet = "album.tracks.get?";
        private const string trackLyricsGet = "track.lyrics.get?";

        private const string albumID = "&album_id=";
        private const string trackID = "&track_id=";
        private const string artistID = "&artist_id=";

        private const string format = "&format=json";
        private const string artist = "&q_artist=";

        //End

        /// <summary>
        /// We use delimiters to calculate the word count foreach set of lyrics excluding spaces and other unwanted characters.
        /// </summary>
        private static char[] delimiters = new char[] { ' ', '\r', '\n' };

        /// <summary>
        /// Used for storing information about artists we want to compare.
        /// </summary>
        private static List<ArtistComparisonData> artistsToCompare = new List<ArtistComparisonData>();

        private static string currentArtist;
        private static string url;

        /// <summary>
        /// Get the lyrics through artist searches. The API used for this project runs through IDs so we find the Artist ID,
        /// followed by the ID for each album & track.
        /// </summary>
        /// <returns></returns>
        public async Task ProcessLyrics()
        {
            int _id = await GetArtistID();

            if (_id == -1)
            {
                Console.WriteLine("Unable to get the artist. Restarting the application.");
                await ProcessLyrics();
                return;
            }

            var _albumData = await GetAlbums(_id);
            var _albumIds = _albumData.message.body.album_list.Select(item => item.album.album_id).ToList();

            if (_albumIds.Count > 0)
            {
                await ProcessAlbumData(_albumIds, _albumData.message.body.album_list);
            }
            else
            {
                Console.WriteLine("There are no albums available. Restarting the program.");
                await ProcessLyrics();
            }
        }

        /// <summary>
        /// Gets the Artist's UID from the input provided.
        /// </summary>
        /// <returns></returns>
        private async Task<int> GetArtistID()
        {
            Console.WriteLine("Enter an artists name:");
            currentArtist = Console.ReadLine();

            //Get artist URL
            url = rootURL + artistSearchQuery + format + artist + currentArtist + apiKey;

            try
            {
                var _artistData = await TryGetData(url);

                //Providing we spell the artist's name correctly, the first index of the list should be the artist we're looking for.
                //An alternative solution would be to provide the user the ability to select an artist from the list of the response data,
                //but that is out of the scope of this demo.
                int _id = _artistData.message.body.artist_list[0].artist.artist_id;

                Console.WriteLine("Got Artist & ID");
                return _id;
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error getting the artist: " + e.Message);
                return -1;
            }
        }

        /// <summary>
        /// Gets the artist's albums from their provided UID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<Root> GetAlbums(int id)
        {
            //Get Albums URL
            url = rootURL + albumSearch + format + artistID + id + apiKey;

            try
            {
                var _albumData = await TryGetData(url);

                Console.WriteLine("Got Albums");
                return _albumData;
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error getting the albums: " + e.Message);
                return null;
            }
        }

        /// <summary>
        /// Processes the tracks from each artists album, aswell as parsing the track data to count the lyrics.
        /// </summary>
        /// <param name="albumIds"></param>
        /// <param name="albumList"></param>
        /// <returns></returns>
        private async Task ProcessAlbumData(List<int> albumIds, List<AlbumList> albumList)
        {
            List<int> _wordCounts = new List<int>();

            try
            {
                foreach (int i in albumIds)
                {
                    //Get tracks from album URL
                    url = rootURL + albumTracksGet + format + albumID + i + apiKey;

                    var _album = await TryGetData(url);
                    if (_album == null)
                    {
                        Console.WriteLine("Couldn't get album, skipping...");
                        continue;
                    }

                    string albumName = albumList.Where(item => item.album.album_id == i).FirstOrDefault().album.album_name;

                    Console.WriteLine("\nAlbum: " + albumName + "\n");
                    _wordCounts.AddRange(await ProcessTrackData(_album));
                    Console.WriteLine("-------------------");

                }

                int _averageLyricCount = _wordCounts.Take(_wordCounts.Count).Sum() / _wordCounts.Count;
                await CalculateLyricAverages(_averageLyricCount);
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error getting the track lyrics: " + e.Message);
            }
        }

        /// <summary>
        /// Calculates the lyric count for each track in an album
        /// </summary>
        /// <param name="album"></param>
        /// <returns>returns the lyric count for each track as a list to be calculated as an average.</returns>
        private async Task<List<int>> ProcessTrackData(Root album)
        {
            if (album.message.body.track_list.Count == 0)
            {
                Console.WriteLine("No tracks available on this album.");
                return null;
            }

            List<int> totalCountForAlbum = new List<int>();

            foreach (var v in album.message.body.track_list)
            {
                url = rootURL + trackLyricsGet + format + trackID + v.track.track_id + apiKey;
                var _track = await TryGetData(url);
                if (_track == null)
                {
                    continue;
                }

                if (_track.message.body.lyrics.lyrics_body.Length > 0)
                {
                    int _count = _track.message.body.lyrics.lyrics_body.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
                    Console.WriteLine("Track: " + v.track.track_name + " - Count: " + _count);
                    totalCountForAlbum.Add(_count);
                }
            }
            return totalCountForAlbum;
        }

        /// <summary>
        /// Displays the data from this artist.
        /// </summary>
        /// <param name="averageLyricCount"></param>
        /// <returns></returns>
        private async Task CalculateLyricAverages(int averageLyricCount)
        {
            Console.WriteLine("Average lyrics for this artist: " + averageLyricCount);

            if (artistsToCompare.Count > 0)
            {
                artistsToCompare.Add(new ArtistComparisonData { artistName = currentArtist, averageLyricCount = averageLyricCount });

                foreach (var v in artistsToCompare)
                {
                    Console.WriteLine("Artist: " + v.artistName + "-- Average lyrics: " + v.averageLyricCount);
                }
            }

            await CheckComparisonOfArtists(averageLyricCount);
        }

        /// <summary>
        /// Allows the user to compare this artist with any number of other artists.
        /// </summary>
        /// <param name="averageLyrics"></param>
        /// <returns></returns>
        private async Task CheckComparisonOfArtists(int averageLyrics)
        {
            Console.WriteLine("Would you like to compare with another artist? Y/N");
            string _input = Console.ReadLine();
            if (_input.ToUpper() == "Y")
            {
                artistsToCompare.Add(new ArtistComparisonData { artistName = currentArtist, averageLyricCount = averageLyrics });
                await ProcessLyrics();
            }
        }

        /// <summary>
        /// Perform the GET request to our lyrics api. 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<Root> TryGetData(string url)
        {
            try
            {
                url = url.Replace(" ", "%20");
                var _stringTask = client.GetStreamAsync(url);
                return await JsonSerializer.DeserializeAsync<Root>(await _stringTask);
            }
            catch (Exception e)
            {

                Console.WriteLine("Couldn't get data: " + e.Message);
                return null;
            }
        }
    }
}
