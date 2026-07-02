using JarvisAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisAI.Infraestructure.Data
{
    public class JarvisDbContext : DbContext
    {
        public JarvisDbContext(DbContextOptions<JarvisDbContext> options) : base(options) { }

        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
    }
}
