﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace EnglishWebSimulatorApp.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the EnglishWebSimulatorAppUser class
    public class EnglishWebSimulatorAppUser : IdentityUser
    {
        public byte[] Pict { get; set; }
        public string NameImg { get; set; }
    }
}
