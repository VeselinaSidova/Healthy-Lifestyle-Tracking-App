using System.Collections.Generic;
using System.Linq;
using Xunit;
using HealthyLifestyleTrackingApp.Controllers;
using HealthyLifestyleTrackingApp.Data.Enums;
using HealthyLifestyleTrackingApp.Data.Models;
using HealthyLifestyleTrackingApp.Models.Foods;
using MyTested.AspNetCore.Mvc;


using static HealthyLifestyleTrackingApp.WebConstants;


namespace HealthyLifestyleTrackingApp.Test.Controllers
{
    public class FoodsControllerTests
    {
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


        //[Theory]
        //[InlineData("Name", "Brand", 25.00, (StandardServingType)1, "https://testphoto.com/photo.jpg", 255, 15, 22, 33, null)]
        //public void CreatePostShouldSaveArticleSetTempDataMessageAndRedirectWhenValidModel(
        //    string name, 
        //    string brand, 
        //    double standardServingAmount,
        //    StandardServingType standardServingType, 
        //    string imageUrl, 
        //    int calories,
        //    double protein,
        //    double carbohydrates,
        //    double fat,
        //    ICollection<int> foodTags)
        //        => MyController<FoodsController>
        //             .Instance(instance => instance
        //                .WithUser())
        //             .Calling(c => c.Create(new FoodFormModel
        //             {
        //                 Name = name,
        //                 Brand = brand,
        //                 StandardServingAmount = standardServingAmount,
        //                 StandardServingType = standardServingType,
        //                 ImageUrl = imageUrl,
        //                 Calories = calories,
        //                 Protein = protein,
        //                 Carbohydrates = carbohydrates,
        //                 Fat = fat,
        //                 FoodTags = foodTags
        //             }))
        //             .ShouldHave()
        //             .ValidModelState()
        //             .AndAlso()
        //             .ShouldHave()
        //             .Data(data => data
        //                .WithSet<Food>(foods => foods
        //                    .Any(f =>
        //                     f.Name == name &&
        //                     f.Brand == brand &&
        //                     f.StandardServingAmount == standardServingAmount &&
        //                     f.StandardServingType == standardServingType &&
        //                     f.ImageUrl == imageUrl &&
        //                     f.Calories == calories &&
        //                     f.Protein == protein &&
        //                     f.Carbohydrates == carbohydrates &&
        //                     f.Fat == fat &&
        //                     f.FoodTags == foodTags)))
        //        .TempData(tempData => tempData
        //            .ContainingEntryWithKey(GlobalMessageKey))
        //       .AndAlso()
        //       .ShouldReturn()
        //       .Redirect(redirect => redirect
        //           .To<ArticlesController>(c => c.Mine()));
    }
}
