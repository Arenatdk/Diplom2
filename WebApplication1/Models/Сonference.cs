using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Conference
    {
        public int Id { get; set; }
        public string Name { get; set; } // имя пользователя
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.mm.yyyy}")]
        public DateTime DateStart { get; set; }
        
        
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.mm.yyyy}")]
        public DateTime DateEnd { get; set; }
        public string Place { get; set; }
        public string Subjects { get; set; } //тематика
        public string KeyWords { get; set; } // Ключевые слова 

        public List<UserGoConf> UserGoConfs { get; set; }
        public List<CalendarEvent> CalendarEvents { get; set; }
        public List<MyFile> MyFiles { get; set; }
        public Conference()
        {
            UserGoConfs = new List<UserGoConf>();
        }


    }
}
