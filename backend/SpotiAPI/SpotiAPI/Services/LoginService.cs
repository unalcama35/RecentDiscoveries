﻿using Microsoft.AspNetCore.Mvc;
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
                if(user.Password == log.Password)
                    return await UpdateLastLogin(user);

         
            return false;

        }

        private async Task<bool> UpdateLastLogin(User user)
        {
                user.LastLogin = DateTime.Now;

                await this.context.SaveChangesAsync();

                return true;
            
        }

        public async Task<int> GetIDAsync(String username)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);

            return user.Id;
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
