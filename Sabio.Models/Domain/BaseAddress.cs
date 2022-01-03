using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain
{
    public class BaseAddress
    {
		public int Id { get; set; }
		public string LineOne { get; set; }		
		public string City { get; set; }
		public string State { get; set; }
		public string PostalCode { get; set; }
		

	}
}
