﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.CodingChallenge
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public string Description { get; set; }     
        public int SeasonTermId { get; set; }
        public int TeacherId { get; set; }
        public string SeasonTerm { get; set; }
        public string Teacher { get; set; }
        public List<Student> Students { get; set; }
    }
}
