using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Commonlication
{
    public class DB : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(@"Server=localhost;database=book;uid=root;pwd=123123;SslMode=None");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<SysConfig> SysConfigs { get; set; }
        public DbSet<Active> Actives { get; set; }
        public DbSet<Catory> Catorys { get; set; }
        public DbSet<ActiveOne> ActiveOnes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Voluntary> Voluntarys { get; set; }
        public DbSet<SysMenu> SysMenus { get; set; }

    }
}
