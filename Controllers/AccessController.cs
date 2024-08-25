using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LoginSistem.Models;
using LoginSistem.Data;
using LoginSistem.ViewModels;
using System.Security.Claims; //Guarda la informacion del Usuario
using Microsoft.AspNetCore.Authentication; //Permite la autenticacion de usuarios
using Microsoft.AspNetCore.Authentication.Cookies;


namespace LoginSistem.Controllers;
public class AccessController : Controller
{
    private readonly AppDBContext _appDbContext;
    public AccessController(AppDBContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserVM modelo)
    {
        if(modelo.Password != modelo.ConfirmPassword)
        {
            ViewData["Mensaje"]="Las Contraseñas no coinciden";
            return View();
        }

        User user = new User()
        {
            FullName = modelo.FullName,
            Email = modelo.Email,
            Password = modelo.Password
        };

        await _appDbContext.Users.AddAsync(user);
        await _appDbContext.SaveChangesAsync();

        if(user.IdUser != 0)
        {
            return RedirectToAction("Login","Access");
        }
        ViewData["Mensaje"]="Error Fatal, No se pudo crear el Usuario";
        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM modelo)
    {
        User user_found = await _appDbContext.Users
                            .Where(u => 
                             u.Email == modelo.Email && 
                             u.Password == modelo.Password).FirstOrDefaultAsync();

        if(user_found == null)
        {
            ViewData["Mensaje"]="El Usuario no existe, puede que el Correo o la Contraseña  sean Incorrectos";
            return View();
        }

        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user_found.FullName)
            //Agregar mas claims si es necesario
        };

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
        AuthenticationProperties properties = new AuthenticationProperties()
        {
            AllowRefresh =true
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            properties
        );
        
        return RedirectToAction("Index","Home");
    }

    


}
