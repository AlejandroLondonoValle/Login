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


}
