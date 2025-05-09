using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraping.Entities
{
    public class About
    {
        public int Id { get; set; }
        public string Equipment { get; set; }
        public string PrimaryMuscleGroup { get; set; }
        public string SecondaryMuscleGroups { get; set; }
        public string ExerciseType { get; set; }
        public string GifId {  get; set; }
    }
}
