using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SpotiAPI.Models;
using System.IdentityModel.Tokens.Jwt;

namespace SpotiAPI.Services
{
    public class LoginService
    {
        private readonly DataContext context;

        public LoginService(DataContext context)
        {
            this.context = context;
        }

        public async Task<bool> Verify(LoginDts log)
        {
            User user = await this.context.Users.FirstOrDefaultAsync(u => u.Username == log.LoginName);
            if (user != null)
                return (user.Password == log.Password);
         
            return false;

        }

        public async Task<ActionResult<List<User>>> Register(User user)
        {
            try
            {
                this.context.Users.Add(user);
                await this.context.SaveChangesAsync();
                return await GetAllUsersAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error registering user: " + ex.Message);

                return null;
            }
        }

        


        public async Task<List<User>> GetAllUsersAsync()
        {
            return await this.context.Users.ToListAsync();

        }



    }
}
