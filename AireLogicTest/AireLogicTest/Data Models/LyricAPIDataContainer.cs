using System.Collections.Generic;

namespace AireLogicTestDanTimms
{
    /// <summary>
    /// The api retrieves a json file, with Root being the header.
    /// </summary>
    public class Root
    {
        public Message message { get; set; }
    }

    /// <summary>
    /// Message is a child of Root when the data is retrieved from the API
    /// </summary>
    public class Message
    {
        public Header header { get; set; }
        public Body body { get; set; }
    }

    /// <summary>
    /// Header is a child of Message when the data is retrieved from the API
    /// </summary>
    public class Header
    {
        public int status_code { get; set; }
        public double execute_time { get; set; }
        public int available { get; set; }
    }

    /// <summary>
    /// Body is a child of Message when the data is retrieved from the API
    /// </summary>
    public class Body
    {
        public List<ArtistList> artist_list { get; set; }
        public List<AlbumList> album_list { get; set; }
        public List<TrackList> track_list { get; set; }
        public Lyrics lyrics { get; set; }
    }
}
