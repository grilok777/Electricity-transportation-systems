using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OwnerDeal
    {
        public int Id { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public string NumberPhone { get; set; } = string.Empty;
        public string PlaceLocation { get; set; } = string.Empty;
        public DateOnly ConclusionDeal { get; set; }
        public DateOnly? CompetionDeal { get; set; }
        public DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;

        public ICollection<PowerPlant> PowerPlants { get; set; } = new List<PowerPlant>();
    }
}
