using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapingApplication.Entities
{
    public class Exercises
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<About> Abouts { get; set; }
        public List <Instructions> Instructions { get; set; }
    }
}
