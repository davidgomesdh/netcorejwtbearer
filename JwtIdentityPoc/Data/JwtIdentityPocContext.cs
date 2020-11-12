using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using JwtIdentityPoc.Models;

namespace JwtIdentityPoc.Data
{
    public class JwtIdentityPocContext : DbContext
    {
        public JwtIdentityPocContext (DbContextOptions<JwtIdentityPocContext> options)
            : base(options)
        {
        }

        public DbSet<JwtIdentityPoc.Models.User> User { get; set; }
    }
}
