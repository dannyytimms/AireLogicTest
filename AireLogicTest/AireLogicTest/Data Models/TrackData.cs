namespace AireLogicTestDanTimms
{
    //This file contains data containers for tracks.

    public class Track
    {
        public int track_id { get; set; }
        public string track_name { get; set; }
    }

    public class TrackList
    {
        public Track track { get; set; }
    }

    public class Lyrics
    {
        public string lyrics_body { get; set; }
    }
}
