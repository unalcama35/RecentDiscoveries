namespace SpotiAPI.Models
{
    public class SongVM
    {
        public SongVM(string name, string artist, string album, string song_Id, string song_Pic)
        {
            Name = name;
            Artist = artist;
            Album = album;
            Song_Id = song_Id;
            Song_Pic = song_Pic;
        }

        public string Name { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Song_Id { get; set; }
        public string Song_Pic { get; set; }
    }
}
