namespace AireLogicTestDanTimms
{
    //This file contains data containers for albums.

    public class Album
    {
        public int album_id { get; set; }
        public string album_mbid { get; set; }
        public string album_name { get; set; }
    }

    public class AlbumList
    {
        public Album album { get; set; }
    }
}
