using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LoginSistem.Models;

[Table("users")]
public class User
{
    [Column("id_user")]
    public int IdUser { get; set; }
    [Column("full_name")]
    public string FullName { get; set; }
    [Column("email")]
    public string Email { get; set; }
    [Column("password")]
    public string Password { get; set; }
}
