using System.ComponentModel.DataAnnotations;
using static HealthyLifestyleTrackingApp.Data.DataConstants.Article;

namespace HealthyLifestyleTrackingApp.Models.Articles
{
    public class CreateArticleFormModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; init; }

        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
        public string Content { get; set; }

        [Display(Name = "Image URL")]
        [Required]
        [Url]
        public string ImageUrl { get; init; }
    }
}
