using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapingApplication.Enums;

namespace ScrapingApplication.Entities
{
    public class Exercises
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List <Instructions> Instructions { get; set; }
        public EquipmentEnum Equipment { get; set; }
        public MusclesEnum PrimaryMuscleGroup { get; set; }
        public MusclesEnum SecondaryMuscleGroups { get; set; }
        public ExerciseType ExerciseTypes { get; set; }
        public string GifId { get; set; }
    }
}
