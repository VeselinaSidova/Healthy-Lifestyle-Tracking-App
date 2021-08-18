using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HealthyLifestyleTrackingApp.Infrastructure;
using HealthyLifestyleTrackingApp.Models.Tracker;
using HealthyLifestyleTrackingApp.Services.Tracker;
using static HealthyLifestyleTrackingApp.WebConstants;

namespace HealthyLifestyleTrackingApp.Controllers
{
    public class TrackerController : Controller
    {
        private readonly ITrackerService tracked;

        public TrackerController(ITrackerService tracked)
        {
            this.tracked = tracked;
        }

        [Authorize]
        public IActionResult SelectDate()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult SelectDate(DatePickerModel datePicker)
        {
            var selectedDate = datePicker.Date;

            var selectedDateString = selectedDate.ToString("dd-MM-yyyy");

            return RedirectToAction(nameof(ViewTracked), new { selectedDateString = selectedDateString });
        }

        [Authorize]
        public IActionResult ViewTracked([FromQuery] AllTrackedQueryModel query, string selectedDateString)
        {
            DateTime selectedDate = DateTime.Parse(selectedDateString);

            var userId = this.User.GetId();

            var foodQueryResult = this.tracked.AllTrackedFoods(
               selectedDate,
               userId);

            var exerciseQueryResult = this.tracked.AllTrackedExercises(
                selectedDate, 
                userId);

            query.DateTracked = foodQueryResult.DateTracked.Date;
            query.Foods = foodQueryResult.Foods;
            query.Exercises = exerciseQueryResult.Exercises;

            return View(query);
        }

        [Authorize]
        public IActionResult DeleteTrackedFood(int id)
        {
            var userId = this.User.GetId();

            this.tracked.DeleteTrackedFood(id, userId);

            TempData[GlobalMessageKey] = "Food was deleted from tracker.";

            return RedirectToAction(nameof(SelectDate));
        }

        [Authorize]
        public IActionResult DeleteTrackedExercise(int id)
        {
            var userId = this.User.GetId();

            this.tracked.DeleteTrackedExercise(id, userId);

            TempData[GlobalMessageKey] = "Exercise was deleted from tracker.";

            return RedirectToAction(nameof(SelectDate));
        }
    }
}
