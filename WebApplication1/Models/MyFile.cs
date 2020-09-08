using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class MyFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Size { get; set; }
        public int ConferenceId { get; set; }
        public Conference Conference { get; set; }
    }
}
