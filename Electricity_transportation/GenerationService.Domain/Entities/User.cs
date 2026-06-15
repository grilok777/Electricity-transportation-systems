using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
        public DateOnly DateRegistration { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        //public ICollection<PowerPlant> PowerPlants { get; set; } = new List<PowerPlant>();
        public ICollection<OwnerDeal> Deals { get; set; } = new List<OwnerDeal>();
    }
}
