using CSharpNewAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpNewAPI.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<AppUser> Users { get; set; }
        public DatabaseContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }

    }
}
