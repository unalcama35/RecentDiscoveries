using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotiAPI.Models;
using SpotiAPI.Services;

using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Caching.Memory;



namespace SpotiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {

      

        private readonly DataContext context;
        private readonly SongService songService;
        private readonly SpotifyService spotifyService;
        private readonly LoginService loginService;
       
    //  private readonly IMemoryCache memoryCache;
        public SongController(DataContext context, SongService songService, SpotifyService spotifyService, LoginService loginService)
        {
            this.context = context;
            this.songService = songService;
            this.spotifyService = spotifyService;
            this.loginService = loginService;
      //      this.memoryCache = memoryCache; 
        }




        //Database Part

        [HttpGet]
        public async Task<ActionResult<List<Song>>> Get()
        {
            var song = await songService.GetAllSongsAsync();

            return Ok(song);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Song>>> Get(int id)
        {

            var song =await songService.GetSongByIdAsync(id);
            if (song == null)
                return NotFound("Song not found.");
            return Ok(song);
        }


        [HttpPost]
        public async Task<ActionResult<List<Song>>> AddSong(Song song)
        {

            var response = await this.songService.AddSong(song);
            return Ok(response);
        }



        [HttpPut]
        public async Task<ActionResult<List<Song>>> UpdateSong(Song request)
        {
            try
            {
                await this.songService.UpdateSongAsync(request);
                return Ok(await this.context.Songs.ToListAsync());
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);  
            }

           
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Song>>> Delete(int id)
        {

            try
            {
                await this.songService.DeleteSong(id);
                return Ok(await this.context.Songs.ToListAsync());
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("all")]
        public async Task<ActionResult<List<Song>>> DeleteAll()
        {

            try
            {
                await this.songService.DeleteAllSongs();
                return Ok(await this.context.Songs.ToListAsync());
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }



        //Spotify Part


        [HttpGet("callback")]
        public async Task<ActionResult<string>> GetSpoti()
        {
            
           string authorCode = HttpContext.Request.Query["code"].ToString();


           var accessToken = await spotifyService.GenerateToken(authorCode);

           return Content("<script>window.close();</script>", "text/html");

           return Ok("Token Acquired");


        }

        [HttpGet("RecentLiked")]
        public async Task<ActionResult<string>> GetRecents()
        {

            var items = await spotifyService.GetRecentsAsync();
            await songService.DeleteAllSongs();
            JToken[] tracks = items.Select(item => item["track"]).ToArray();
            var songs = await songService.AddSongsOfItems(tracks);
            return Ok(songs);
        }

        [HttpGet("TopTracks")]
        public async Task<ActionResult<string>> GetTopTracks()
        {
            var items = await spotifyService.GetLongTermSongs();
         //   return items.ToString();
            await songService.DeleteAllSongs();
           // JToken[] tracks = items.Select(item => item["TrackObject"]).ToArray();

            var songs = await songService.AddSongsOfItems(items.ToArray());
            return Ok(songs);
        }

        [HttpPost("Login")]
        public ActionResult<string> Login()
        {
           spotifyService.LoginAsync();
           return Ok("Logged In");

        }

        [HttpPost("AppLogin")]
        public ActionResult<string> AppLogin(LoginDts log)
        {
            string username = log.LoginName;
            string password = log.Password;

            return Ok("Log in verified");
        }










    }
}
