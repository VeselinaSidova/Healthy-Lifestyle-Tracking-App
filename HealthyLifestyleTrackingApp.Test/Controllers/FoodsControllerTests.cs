using System;
using System.Linq;
using HealthyLifestyleTrackingApp.Controllers;
using HealthyLifestyleTrackingApp.Data.Enums;
using HealthyLifestyleTrackingApp.Data.Models;
using HealthyLifestyleTrackingApp.Models.Foods;
using HealthyLifestyleTrackingApp.Services.Foods.Models;
using HealthyLifestyleTrackingApp.Test.Data;
using MyTested.AspNetCore.Mvc;
using Shouldly;
using Xunit;
using static HealthyLifestyleTrackingApp.WebConstants;


namespace HealthyLifestyleTrackingApp.Test.Controllers
{
    public class FoodsControllerTests
    {
        [Fact]
        public void GetAllFoodsShouldReturnView()
           => MyController<FoodsController>
               .Calling(c => c.All(With.Default<AllFoodsQueryModel>()))
               .ShouldReturn()
               .View(view => view
                   .WithModelOfType<AllFoodsQueryModel>());

        [Fact]
        public void GetAllFoodsShouldReturnViewWithLoggedInUser()
          => MyController<FoodsController>
              .Instance(instance => instance
                  .WithUser())
              .Calling(c => c.All(With.Default<AllFoodsQueryModel>()))
              .ShouldReturn()
              .View(view => view
                  .WithModelOfType<AllFoodsQueryModel>());


        [Fact]
        public void GetDetailsShouldReturnNotFoundWhenInvalidFoodIdAndInvalidName()
          => MyController<FoodsController>
             .Calling(c => c.Details(int.MaxValue, "Name"))
             .ShouldReturn()
             .NotFound();


        [Fact]
        public void GetDetailsShouldReturnBadRequestWhenValidArticleIdAndInvalidName()
            => MyController<FoodsController>
               .Instance(instance => instance
                    .WithData(FoodsTestData.GetFoods(1)))
               .Calling(c => c.Details(1, "Invalid Name"))
               .ShouldReturn()
               .NotFound();


        [Fact]
        public void GetDetailsShouldShouldReturnCorrectFood()
           => MyController<FoodsController>
                .Instance(instance => instance
                    .WithData(FoodsTestData.GetFoods(1)))
                .Calling(c => c.Details(1, "Name 1"))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<FoodDetailsServiceModel>()
                    .Passing(food =>
                    {
                        food.Id.Equals(1);
                        food.Name.Equals("Name 1");
                    }));


        [Fact]
        public void GetCreateShouldBeForAuthorizedUsersAndReturnView()
                => MyController<FoodsController>
                      .Instance(instance => instance
                            .WithUser())
                      .Calling(c => c.Create())
                      .ShouldHave()
                      .ActionAttributes(attributes => attributes
                          .RestrictingForAuthorizedRequests())
                      .AndAlso()
                      .ShouldReturn()
                      .View();


        [Fact]
        public void PostCreateShouldBeAllowedOnlyForPostRequestsAndAuthorizedUsers()
            => MyController<FoodsController>
            .Instance(instance => instance
                .WithUser())
            .Calling(c => c.Create(With.Default<FoodFormModel>()))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForAuthorizedRequests()
                .RestrictingForHttpMethod(HttpMethod.Post));


        [Theory]
        [InlineData("Name", "Brand", 25.00, (StandardServingType)1, 1, "https://testphoto.com/photo.jpg", 255, 15.00, 22.00, 33.00)]
        public void PostCreateShouldSaveFoodAndSetTempDataMessageAndRedirectWhenValidModel(
            string name,
            string brand,
            double standardServingAmount,
            StandardServingType standardServingType,
            int categoryId,
            string imageUrl,
            int calories,
            double protein,
            double carbohydrates,
            double fat)
                => MyController<FoodsController>
                     .Instance(instance => instance
                        .WithUser()
                        .WithData(FoodCategoriesTestData.GetFoodCategories(1)))
                     .Calling(c => c.Create(new FoodFormModel
                     {
                         Name = name,
                         Brand = brand,
                         StandardServingAmount = standardServingAmount,
                         StandardServingType = standardServingType,
                         FoodCategoryId = categoryId,
                         ImageUrl = imageUrl,
                         Calories = calories,
                         Protein = protein,
                         Carbohydrates = carbohydrates,
                         Fat = fat,
                     }))
                     .ShouldHave()
                     .ValidModelState()
                     .AndAlso()
                     .ShouldHave()
                     .Data(data => data
                        .WithSet<Food>(foods => foods
                            .Any(f =>
                             f.Name == name &&
                             f.Brand == brand &&
                             f.StandardServingAmount == standardServingAmount &&
                             f.StandardServingType == standardServingType &&
                             f.FoodCategoryId == categoryId &&
                             f.ImageUrl == imageUrl &&
                             f.Calories == calories &&
                             f.Protein == protein &&
                             f.Carbohydrates == carbohydrates &&
                             f.Fat == fat)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
               .AndAlso()
               .ShouldReturn()
               .Redirect(redirect => redirect
                   .To<FoodsController>(c => c.Details(1, name)));


        [Theory]
        [InlineData("A", 25.00, (StandardServingType)2, 1, "https://testphoto.com/photo.jpg", 0, 49.00, 22.00, 33.00)]
        public void PostCreateShouldReturnViewWithSameModelWhenInvalidModelState(
            string name,
            double standardServingAmount,
            StandardServingType standardServingType,
            int categoryId,
            string imageUrl,
            int calories,
            double protein,
            double carbohydrates,
            double fat)
           => MyController<FoodsController>
               .Instance(instance => instance
                   .WithUser()
                   .WithData(FoodCategoriesTestData.GetFoodCategories(1)))
                 .Calling(c => c.Create(new FoodFormModel
                 {
                     Name = name,
                     StandardServingAmount = standardServingAmount,
                     StandardServingType = standardServingType,
                     FoodCategoryId = categoryId,
                     ImageUrl = imageUrl,
                     Calories = calories,
                     Protein = protein,
                     Carbohydrates = carbohydrates,
                     Fat = fat,
                 }))
               .ShouldHave()
               .InvalidModelState()
               .AndAlso()
               .ShouldReturn()
                .View(view => view
                    .WithModelOfType<FoodFormModel>()
                    .Passing(food =>
                    {
                        food.Name.Equals("A");
                        food.ImageUrl.Equals("https://testphoto.com/photo.jpg");
                        food.StandardServingAmount.Equals(25);
                        food.StandardServingType.Equals((StandardServingType)2);
                        food.FoodCategoryId.Equals(1);
                        food.Calories.Equals(0);
                        food.Protein.Equals(49);
                        food.Carbohydrates.Equals(22);
                        food.Fat.Equals(33);
                    }));


        [Fact]
        public void GetEditShouldBeAvailableForAuthorizedAdminsOnly()
            => MyController<FoodsController>
                .Instance(instance => instance
                    .WithData(FoodsTestData.GetFoods(1))
                    .WithUser(user => user.InRole("Administrator")))
                .Calling(c => c.Edit(1, "Name 1"))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<FoodFormModel>()
                    .Passing(food =>
                    {
                        food.Name.Equals("Name 1");
                        food.Brand.Equals("Brand 1");
                        food.ImageUrl.Equals("https://testphoto.com/photo.jpg");
                        food.StandardServingAmount.Equals(11);
                        food.StandardServingType.Equals((StandardServingType)2);
                        food.FoodCategoryId.Equals(1);
                        food.Calories.Equals(11);
                        food.Protein.Equals(1);
                        food.Carbohydrates.Equals(1);
                        food.Fat.Equals(1);
                        food.FoodTags.Count.Equals(0);
                    }));



        [Fact]
        public void GetEditShouldReturnNotFoundWhenInvalidId()
           => MyController<FoodsController>
                .Instance(instance => instance
                    .WithData(FoodsTestData.GetFoods(1))
                    .WithUser(user => user.InRole("Administrator")))
               .Calling(c => c.Edit(1, "InvalidName"))
               .ShouldReturn()
               .NotFound();


        [Fact]
        public void GetEditShouldReturnUnathorizedWhenNonAdminTriesToEdit()
          => MyController<FoodsController>
              .Instance(instance => instance
                    .WithData(FoodsTestData.GetFoods(1))
                    .WithUser())
               .Calling(c => c.Edit(1, "Name 1"))
               .ShouldReturn()
               .Unauthorized();


        [Fact]
        public void PostEditShouldHaveRestrictionsForHttpPostAndAuthorizedUsers()
          => MyController<FoodsController>
               .Instance(instance => instance
                   .WithUser(user => user.InRole("Administrator")))
              .Calling(c => c.Edit(
                  With.Any<int>(),
                  With.Default<FoodFormModel>()))
              .ShouldHave()
              .ActionAttributes(attrs => attrs
                  .RestrictingForHttpMethod(HttpMethod.Post)
                  .RestrictingForAuthorizedRequests());


        [Fact]
        public void PostEditShouldReturnNotFoundWhenInvalidId()
            => MyController<FoodsController>
                .Instance(instance => instance
                    .WithUser(user => user.InRole("Administrator")))
                .Calling(c => c.Edit(
                    With.Any<int>(),
                    With.Any<FoodFormModel>()))
                .ShouldReturn()
                .NotFound();


        [Fact]
        public void PostEditShouldReturUnauthorizedWhenNonAuthorTriesToEdit()
            => MyController<FoodsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(FoodsTestData.GetFoods(1)))
                .Calling(c => c.Edit(1, With.Empty<FoodFormModel>()))
                .ShouldReturn()
                .Unauthorized();


        [Fact]
        public void PostEditShouldReturnViewWithSameModelWhenInvalidModelState()
            => MyController<FoodsController>
                .Instance(instance => instance
                     .WithUser(user => user.InRole("Administrator"))
                    .WithData(FoodsTestData.GetFoods(1)))
                .Calling(c => c.Edit(1, With.Default<FoodFormModel>()))
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(With.Default<FoodFormModel>());


        [Theory]
        [InlineData(1, "Name", "Brand", 25.00, (StandardServingType)1, 1, "https://testphoto.com/photo.jpg", 255, 15.00, 22.00, 33.00)]
        public void PostEditShouldSaveArticleSetTempDataMessageAndRedirectWhenValidModelStateWhenAuthorEdits(
            int foodId,
            string name,
            string brand,
            double standardServingAmount,
            StandardServingType standardServingType,
            int categoryId,
            string imageUrl,
            int calories,
            double protein,
            double carbohydrates,
            double fat)
            => MyController<FoodsController>
                .Instance(instance => instance
                    .WithUser(user => user.InRole("Administrator"))
                    .WithData(FoodCategoriesTestData.GetFoodCategories(1)))
                    .Calling(c => c.Create(new FoodFormModel
                     {
                         Name = $"Edit {name}",
                         Brand = $"Edit {brand}",
                         StandardServingAmount = standardServingAmount,
                         StandardServingType = standardServingType,
                         FoodCategoryId = categoryId,
                         ImageUrl = $"{imageUrl}/test",
                         Calories = calories + 1,
                         Protein = protein + 5,
                         Carbohydrates = carbohydrates + 5,
                         Fat = fat + 5,
                     }))
                .ShouldHave()
                .ValidModelState()
                .AndAlso()
                .ShouldHave()
                .Data(data => data
                    .WithSet<Food>(set =>
                    {
                        set.Any();
                        var food = set.SingleOrDefault(a => a.Id == foodId);
                        food.ShouldNotBeNull();
                        food.Name.Equals($"Edit {name}");
                        food.Brand.Equals($"Edit {brand}");
                        food.StandardServingAmount.Equals(standardServingAmount);
                        food.StandardServingType.Equals(standardServingType);
                        food.ImageUrl.Equals($"{imageUrl}/test");
                        food.Calories.Equals(256);
                        food.Protein.Equals(20);
                        food.Carbohydrates.Equals(27);
                        food.Fat.Equals(33);
                    }))
                .AndAlso()
                .ShouldHave()
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<FoodsController>(c => c.Details(foodId, "Edit Name")));


        [Fact]
        public void GetTrackShouldOnlyBeAllowedForAuthorizedUsersAndShouldReturnView()
        => MyController<FoodsController>
          .Instance(instance => instance
                 .WithUser())
            .Calling(c => c.Track(1))
            .ShouldHave()
            .ActionAttributes(attrs => attrs
                .RestrictingForAuthorizedRequests());


        [Fact]
        public void PostTrackShouldReturnNotFoundWhenInvalidId()
            => MyController<FoodsController>
                .Instance(instance => instance
                    .WithUser())
                .Calling(c => c.Track(
                    With.Any<int>(),
                    With.Any<string>(),
                    With.Any<TrackFoodFormModel>()))
                .ShouldReturn()
                .NotFound();


        [Fact]
        public void PostTrackShouldReturnBadRequestWhenValidIdAndInvalidName()
            => MyController<FoodsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(FoodsTestData.GetFoods(1)))
                .Calling(c => c.Track(
                    1,
                    With.Any<string>(),
                    With.Any<TrackFoodFormModel>()))
                .ShouldReturn()
                .BadRequest();


        [Fact]
        public void PostTrackShouldReturnViewWithSameModelWhenInvalidModelState()
            => MyController<FoodsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(FoodsTestData.GetFoods(1)))
                .Calling(c => c.Track(1, "Name 1", With.Default<TrackFoodFormModel>()))
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(With.Default<TrackFoodFormModel>());


        [Theory]
        [InlineData(1, 200, (MealType)1)]
        public void PostCreateShouldSaveArticleSetTempDataMessageAndRedirectWhenValidModel(int foodId, int amount, MealType mealType)
          => MyController<FoodsController>
               .Instance(instance => instance
                   .WithData(FoodsTestData.GetFoods(1))
                   .WithUser())
              .Calling(c => c.Track(foodId, "Name 1", new TrackFoodFormModel
              {
                  AmountInGrams = amount,
                  MealType = mealType,
                  DateTracked = DateTime.Today,
              }))
              .ShouldHave()
              .ValidModelState()
              .AndAlso()
              .ShouldHave()
              .TempData(tempData => tempData
                   .ContainingEntryWithKey(GlobalMessageKey))
              .AndAlso()
              .ShouldReturn()
              .Redirect(redirect => redirect
                  .To<FoodsController>(c => c.All(With.Empty<AllFoodsQueryModel>())));


        [Fact]
        public void GetDeleteShouldOnlyBeAllowedForAuthorizedAdmins()
         => MyController<FoodsController>
           .Instance(instance => instance
                  .WithUser(user => user.InRole("Administrator")))
             .Calling(c => c.Delete(With.Empty<int>()))
             .ShouldHave()
             .ActionAttributes(attrs => attrs
                 .RestrictingForAuthorizedRequests());

        [Fact]
        public void GetDeleteShouldReturnNotFoundWhenInvalidId()
            => MyController<FoodsController>
                .Instance(instance => instance
                    .WithUser(user => user.InRole("Administrator"))
                    .WithData(FoodsTestData.GetFoods(1)))
                .Calling(c => c.Delete(2))
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void GetDeleteShouldReturnUnauthorizedWhenNonAdminTriedToEdit()
            => MyController<FoodsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(FoodsTestData.GetFoods(1)))
                .Calling(c => c.Delete(1))
                .ShouldReturn()
                .Unauthorized();


        [Fact]
        public void GetDeleteShouldDeleteFoodAndRedirectWhenAdminTriesToDelete()
           => MyController<FoodsController>
               .Instance(instance => instance
                   .WithUser(user => user.InRole("Administrator"))
                   .WithData(FoodsTestData.GetFoods(1)))
               .Calling(c => c.Delete(1))
               .ShouldHave()
               .Data(data => data
                   .WithSet<Food>(set => set.ShouldBeEmpty()))
               .AndAlso()
               .ShouldReturn()
               .Redirect(redirect => redirect
                   .To<FoodsController>(c => c.All(With.Empty<AllFoodsQueryModel>())));

    }
}

