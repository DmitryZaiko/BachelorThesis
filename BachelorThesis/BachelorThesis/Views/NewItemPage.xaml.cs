using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using BachelorThesis.Models;
using Newtonsoft.Json;
using BachelorThesis.Helpers;
using BachelorThesis.Services;

namespace BachelorThesis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        public Question Question { get; set; }

        public NewItemPage()
        {
            User user = JsonConvert.DeserializeObject<User>(Settings.UserSettings);
            InitializeComponent();

            Question = new Question();
            Question.UserId = user.Id;
            Question.LessonId = null;

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            var question = await QuestionsService.DoQuestionsAddRequest(Question);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}