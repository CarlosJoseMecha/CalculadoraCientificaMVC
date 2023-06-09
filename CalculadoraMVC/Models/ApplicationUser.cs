﻿using Microsoft.AspNetCore.Identity;

namespace CalculadoraMVC.Models;

public class ApplicationUser : IdentityUser
{
   public string? FirstName { get; set; }
   public string? LastName { get; set; }
   public ICollection<Operacion>? Operaciones { get; set; }
}

public class ApplicationRole : IdentityRole
{
}