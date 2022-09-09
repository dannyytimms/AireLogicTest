using System.Collections.Generic;

namespace AireLogicTestDanTimms
{
    //This file contains data containers for artists.

    public class Artist
    {
        public int artist_id { get; set; }
        public string artist_name { get; set; }
    }

    public class ArtistAliasList
    {
        public string artist_alias { get; set; }
    }

    public class ArtistCredits
    {
        public List<ArtistList> artist_list { get; set; }
    }

    public class ArtistList
    {
        public Artist artist { get; set; }
    }

    public class ArtistNameTranslation
    {
        public string language { get; set; }
        public string translation { get; set; }
    }

    public class ArtistNameTranslationList
    {
        public ArtistNameTranslation artist_name_translation { get; set; }
    }
}
