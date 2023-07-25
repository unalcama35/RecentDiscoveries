using Newtonsoft.Json.Linq;
using SpotiAPI.Models;

namespace SpotiAPI.Services
{
    public class LoginService
    {
        private readonly DataContext context;

        public LoginService(DataContext context)
        {
            this.context = context;
        }

        public async void Verify()
        {
            

        }



    }
}
