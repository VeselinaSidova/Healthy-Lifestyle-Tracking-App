using System;
using System.ComponentModel.DataAnnotations;
using static HealthyLifestyleTrackingApp.Data.DataConstants.Article;

namespace HealthyLifestyleTrackingApp.Data.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        public string Content { get; set; }
        
        public int LifeCoachId { get; init; }

        public LifeCoach LifeCoach { get; set; }
    }
}
