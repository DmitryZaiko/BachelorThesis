using BachelorThesis.Helpers;
using BachelorThesis.Models;
using BachelorThesis.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BachelorThesis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestionsPage : ContentPage
    {
        QuestionsViewModel viewModel;
        public QuestionsPage(int? lessonId = null)
        {
            viewModel = new QuestionsViewModel() { LessonId = lessonId};
            BindingContext = viewModel;
            InitializeComponent();
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            User user = JsonConvert.DeserializeObject<User>(Settings.UserSettings);
            if (user != null)
            {
                await Navigation.PushModalAsync(new NavigationPage(
                                                    new NewItemPage(viewModel.LessonId)
                                                    { viewModel = this.viewModel }));
            }
            else
            {
                await Navigation.PushModalAsync(new NavigationPage(new LoginPage()));
            }

        }

        async void OnQuestionsSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Question;
            if (item == null)
                return;

            await Navigation.PushAsync(new AnswersPage(item));
            QuestionsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Questions.Count == 0)
                viewModel.LoadQuestionsCommand.Execute(null);
        }
    }
}