using BachelorThesis.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BachelorThesis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestionsPage : ContentPage
    {
        QuestionsViewModel viewModel;
        public QuestionsPage()
        {
            viewModel = new QuestionsViewModel();
            BindingContext = viewModel;
            InitializeComponent();
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        async void OnQuestionsSelected(object sender, SelectedItemChangedEventArgs args)
        {
            throw new NotImplementedException();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Questions.Count == 0)
                viewModel.LoadQuestionsCommand.Execute(null);
        }
    }
}