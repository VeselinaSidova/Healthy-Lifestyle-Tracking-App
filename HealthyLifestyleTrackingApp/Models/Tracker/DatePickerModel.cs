using System;
using System.ComponentModel.DataAnnotations;

namespace HealthyLifestyleTrackingApp.Models.Tracker
{
    public class DatePickerModel
    {
        [Required]
        public DateTime Date { get; set; }
    }
}
