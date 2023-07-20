using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotiAPI.Services;

namespace SpotiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrontController : ControllerBase
    {

        private readonly DataContext context;
        private readonly SpotifyService service;

        public FrontController(DataContext context, SpotifyService service)
        {
            this.context = context;
            this.service = service;
        }

    }
}
