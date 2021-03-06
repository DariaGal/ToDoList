﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClientModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Users;
using ToDoListAPI.Controllers.Authentication;

namespace ToDoListAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserService users;

        public AuthController(IUserService repository)
        {
            users = repository;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return Ok("Hello, it's To Do List!\n" +
                "....................／＞  フ\n" +
                "...................| _  _ l\n"+
                "................／` ミ ＿xノ\n"+
                ".............../         |\n"+
                "............../ ヽ       ﾉ\n"+
                "..............│     | | |\n"+
                "..........／￣|     | | |\n"+
                "..........| (￣ヽ＿_ヽ___)\n"+
                "...........＼二つ\n");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegistrationInfo registrationInfo, CancellationToken cancellationToken)
        {
            if(registrationInfo == null)
            {
                return BadRequest();
            }
            var passwordHash = BasicAuthenticationHandler.HashPassword(registrationInfo.Password);
            var userInfo = new UserInfo(registrationInfo.Login, passwordHash);
            User user = null;
            try
            {
                user = await users.CreateAsync(userInfo, cancellationToken);
            }
            catch(UserDuplicationException)
            {
                return Conflict();
            }

            return Ok(user);
        }

        /* public async Task<IActionResult> Login([FromBody] UserRegistrationInfo registrationInfo, CancellationToken cancellationToken)
         {
             var userInfo = new UserInfo(registrationInfo.Login, registrationInfo.Password);
             var user = await userRepository.RegisterUserAsync(userInfo, cancellationToken);

             return Ok();
         }*/
    }



}