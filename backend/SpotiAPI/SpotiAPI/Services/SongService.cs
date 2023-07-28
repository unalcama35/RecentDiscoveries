using Newtonsoft.Json.Linq;
using SpotiAPI.Models;

namespace SpotiAPI.Services
{
    public class SongService
    {
        private readonly DataContext context;

        public SongService(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<Song>> GetAllSongsAsync()
        {
            return await this.context.Songs.ToListAsync();

        }

        public async Task<Song> GetSongByIdAsync(int id)
        {
            return await this.context.Songs.FindAsync(id);
        }

        public async Task<List<Song>> AddSong(Song song)
        {



            this.context.Songs.Add(song);
            await this.context.SaveChangesAsync();
            return await this.context.Songs.ToListAsync();

        }

        public async Task<List<SongVM>> AddSongsOfItems(JToken[] items, int userID)
        {
            foreach (var item in items)
            {
                var name = item["name"].ToString();
                var artist = item["artists"][0]["name"].ToString();
                var album = item["album"]["name"].ToString();
                var trackUrl = item["external_urls"]["spotify"].ToString();
                var imageUrl = item["album"]["images"][0]["url"].ToString();

                await AddSong(new Song
                {
                    Id = 0,
                    Name = name,
                    Artist = artist,
                    Album = album,
                    Song_Id = trackUrl,
                    Song_Pic = imageUrl,
                    UserID = userID
                });
            }

            var songs = await this.context.Songs
                            .Where(song => song.UserID == userID)
                            .ToListAsync();
            return createVM(songs);
            

        }
        private List<SongVM> createVM(List<Song> songs)
        {
            List<SongVM> songList = new List<SongVM>();
            foreach (var song in songs)
            {
                var name = song.Name;
                var artist = song.Artist;
                var album = song.Album;
                var trackUrl = song.Song_Id;
                var imageUrl = song.Song_Pic;

                songList.Add(new SongVM(name, artist, album, trackUrl, imageUrl));
            }
            return songList;
        }

        public async Task UpdateSongAsync(Song song)
        {
            var dbSong = await this.context.Songs.FindAsync(song.Id);
            if (dbSong == null)
                throw new InvalidOperationException("Song not found.");
            dbSong.Name = song.Name;
            dbSong.Artist = song.Artist;
            dbSong.Album = song.Album;
            dbSong.Song_Id = song.Song_Id;
            dbSong.Song_Pic = song.Song_Pic;

            await this.context.SaveChangesAsync();
        }


        public async Task DeleteSong(int id)
        {
            var dbSong = await this.context.Songs.FindAsync(id);
            if (dbSong == null)
                throw new InvalidOperationException("Song not found.");
            this.context.Songs.Remove(dbSong);
            await this.context.SaveChangesAsync();
            
        }

        public async Task DeleteAllSongs()
        {
            var allSongs = await this.context.Songs.ToListAsync();
            

            foreach(var song in allSongs)
            {
                this.context.Songs.Remove(song);
            }
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteAllSongsOfUser(int userID)
        {
            var songsToDelete = await context.Songs
                .Where(song => song.UserID == userID)
                .ToListAsync();

            context.Songs.RemoveRange(songsToDelete);
            await context.SaveChangesAsync();
        }

    }
}
