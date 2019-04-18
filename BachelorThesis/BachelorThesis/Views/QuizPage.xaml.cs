using BachelorThesis.Models;
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
	public partial class QuizPage : ContentPage
	{
        QuizViewModel viewModel;
        static int count;
        static int possition;

        bool IsNewPage = true;
		public QuizPage (URLHttpParams httpParams)
		{
			InitializeComponent ();
            viewModel = new QuizViewModel(httpParams);
            this.BindingContext = viewModel;

            MessagingCenter.Subscribe<QuizViewModel>(this, "QuizesLoaded", (sender) => {
                if (sender.Quizes == null || sender.Quizes.Count < 2)
                    return;
                count = sender.Quizes.Count;
                possition = 1;
                ShowNextButton();
            });
        }

        public QuizPage (QuizViewModel vm)
        {
            InitializeComponent();
            viewModel = vm;
            this.BindingContext = viewModel;
        }

        protected override bool OnBackButtonPressed()
        {
            for (int i = 0; i < possition; i++)
                Navigation.PopModalAsync();
            return true;
        }

        void ShowNextButton()
        {
            nextButton.IsVisible = true;
            nextButton.Text += " (" + possition++ + "/" + count + ")";
        }

        async void OnNextButtonClicked(object sender, System.EventArgs e)
        {
            IsNewPage = false;
            await Navigation.PushModalAsync(new QuizPage(viewModel) { IsNewPage = false } );
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (IsNewPage)
            {
                viewModel.LoadPageCommand.Execute(null);
            }
            else
            {
                if (viewModel.Quizes.Count == 0 ) return;
                ShowNextButton();
                Item currentQuiz = viewModel.Quizes.Dequeue();
                viewModel.Question = currentQuiz.Name;
                URLHttpParams httpParams = new URLHttpParams(viewModel.PageType, currentQuiz.Id.ToString());
                viewModel.LoadItemsCommand.Execute(httpParams);
            }

        }
    }
}