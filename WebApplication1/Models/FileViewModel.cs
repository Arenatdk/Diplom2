using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class FileViewModel
    {

        public int ConferenceId { get; set; }
        public IFormFile File { get; set; }
    }
}
