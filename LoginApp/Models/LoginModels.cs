using Microsoft.EntityFrameworkCore;

namespace LoginApp.Models
{
    public partial class LoginModels :DbContext
    {
        public LoginModels(DbContextOptions<LoginModels> options): base(options) { }
        
        public DbSet<Users> Users { get; set; }
    }
}
