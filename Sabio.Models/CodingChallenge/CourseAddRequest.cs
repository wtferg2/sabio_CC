using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.CodingChallenge
{
    public class CourseAddRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int SeasonTermId { get; set; }
        public int TeacherId { get; set; }
    }
}
