using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models
{
    public class Friend
    {
        
        public int Id { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateAdded { get; set; }
        public string Title { get; set; }
        public string Bio { get; set; }
        public string Summary { get; set; }
        public string Headline { get; set; }
        public string Slug { get; set; }
        public string StatusId { get; set; }
        public string PrimaryImage { get; set; }
        public string UserId { get; set; }
        public Image Image { get; set; }
        public List<Skills> Skills { get; set; }

    }
}
