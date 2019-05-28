using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using BachelorThesis.Models;
using Newtonsoft.Json;
using BachelorThesis.Helpers;
using BachelorThesis.Services;
using BachelorThesis.ViewModels;

namespace BachelorThesis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        public Question Question { get; set; }
        public QuestionsListViewModel viewModel { get; set; }

        public NewItemPage(int? lessonId)
        {
            User user = JsonConvert.DeserializeObject<User>(Settings.UserSettings);
            InitializeComponent();

            Question = new Question();
            Question.UserId = user.Id;
            Question.LessonId = lessonId;

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            var question = await QuestionsService.DoQuestionsAddRequest(Question);
            await Navigation.PopModalAsync();
            viewModel.LoadQuestionsCommand.Execute(null);
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}