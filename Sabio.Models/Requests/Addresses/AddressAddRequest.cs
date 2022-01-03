using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Addresses
{
    public class AddressAddRequest
    {
        [Required]
        public string LineOne { get; set; }
        [Required]
        public int SuiteNumber { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        [Range(00000, 99999)]
        public string PostalCode { get; set; }
        public bool IsActive { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
    }
}
