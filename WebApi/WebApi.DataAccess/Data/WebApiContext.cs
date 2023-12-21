using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.DataAccess.Entities;

namespace WebApi.DataAccess.Data
{
    public class WebApiContext : DbContext
    {
        public WebApiContext (DbContextOptions<WebApiContext> options)
            : base(options)
        {
        }

        public DbSet<WebApi.DataAccess.Entities.Quiz> Quiz { get; set; } = default!;
    }
}
