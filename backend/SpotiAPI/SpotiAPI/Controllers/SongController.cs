using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotiAPI.Models;
using SpotiAPI.Services;

using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

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
        private readonly TokenService tokenService;
        private readonly EmailService emailService;
       
    //  private readonly IMemoryCache memoryCache;
        public SongController(DataContext context, SongService songService, SpotifyService spotifyService, LoginService loginService, TokenService tokenService, EmailService emailService)
        {
            this.context = context;
            this.songService = songService;
            this.spotifyService = spotifyService;
            this.loginService = loginService;
            this.tokenService = tokenService;
            this.emailService = emailService;
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
        public async Task<ActionResult<string>> GetRecents([FromQuery] string userToken)
        {
            
            var items = await spotifyService.GetRecentsAsync();
            JToken[] tracks = items.Select(item => item["track"]).ToArray();
            var usersname = tokenService.ValidateToken(userToken).Identity.Name;
            var userID = await loginService.GetIDAsync(usersname);

            await songService.DeleteAllSongsOfUser(userID);

            var songs = await songService.AddSongsOfItems(tracks, userID);
            return Ok(songs);
        }

        [HttpGet("TopTracks")]
        public async Task<ActionResult<string>> GetTopTracks([FromQuery] string userToken)
        {
            var items = await spotifyService.GetLongTermSongs();
         //   return items.ToString();
            // JToken[] tracks = items.Select(item => item["TrackObject"]).ToArray();
            var usersname = tokenService.ValidateToken(userToken).Identity.Name;
            var userID = await loginService.GetIDAsync(usersname);

            await songService.DeleteAllSongsOfUser(userID);

            var songs = await songService.AddSongsOfItems(items.ToArray(), userID);
            return Ok(songs);
        }

        [HttpPost("Login")]
        public ActionResult<string> Login()
        {
           spotifyService.LoginAsync();
           return Ok("Logged In");

        }

        [HttpPost("AppLogin")]
        public async Task<ActionResult> AppLogin(LoginDts log)
        {
            string username = log.LoginName;

            var verified = await this.loginService.Verify(log);
            if (verified) {
                await spotifyService.LoginAsync();

                var profileurl = await spotifyService.GetProfilePic();
                return Ok(new { message = tokenService.GenerateToken(username), profilepic = profileurl }); 
            }
            else
                return Ok(new { message = "Account not found." });

            
        }


        [HttpPost("AppRegister")]
        public async Task<ActionResult<List<User>>> AppRegister(User user)
        {

            var response = await this.loginService.Register(user);

            return response;
            
        }

        [HttpPost("AppValidate")]
        public async Task<ActionResult<string>> AppValidate(string token)
        {
            var response = this.tokenService.ValidateToken(token);
            if (response != null)
                return response.Identity.Name;
            else
                return ("Token Invalid");

        }

        [HttpPost("EmailSongs")]
        public async Task<ActionResult<string>> MailSongs([FromQuery] string authToken)
        {
            try
            {
               string username = (await this.AppValidate(authToken)).Value;
               int userID = await this.loginService.GetIDAsync(username);
               string to = await this.loginService.GetEmailAsync(userID);
               var songs = await this.songService.GetAllSongsByUserId(userID);
               await this.emailService.SendEmail(to, "My Spotify Songs", FormatSongs(songs));
               return "Email sent";
            }
            catch (Exception ex)
            {
                return (ex.Message + "; Exception occured");
            }
        }

        private string FormatSongs(List<SongVM> songs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var song in songs)
            {
                sb.AppendLine($"Name: {song.Name}");
                sb.AppendLine($"Artist: {song.Artist}");
                sb.AppendLine($"Album: {song.Album}");
                sb.AppendLine($"Song Id: {song.Song_Id}");
                sb.AppendLine($"Song Pic: {song.Song_Pic}");
                sb.AppendLine();
            }
            return sb.ToString();
        }












    }
}
