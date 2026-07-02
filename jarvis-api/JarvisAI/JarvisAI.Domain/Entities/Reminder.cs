using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisAI.Domain.Entities
{
    public class Reminder
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool Done { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
