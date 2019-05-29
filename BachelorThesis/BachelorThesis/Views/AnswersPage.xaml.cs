using BachelorThesis.Helpers;
using BachelorThesis.Models;
using BachelorThesis.Services;
using BachelorThesis.ViewModels;
using Newtonsoft.Json;
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
	public partial class AnswersPage : ContentPage
	{
        AnswersListViewModel viewModel;
        private static readonly int MAX_LENGTH = 1024;
		public AnswersPage (Question question)
		{
			InitializeComponent ();
            Title = question.Header;
            viewModel = new AnswersListViewModel() { QuestionViewModel = new QuestionViewModel(question)};

            TapGestureRecognizer textTap = new TapGestureRecognizer();
            TapGestureRecognizer tagTap = new TapGestureRecognizer();

            textTap.Tapped += OnLabelTapped;
            tagTap.Tapped += OnLabelTapped;

            questionLabel.GestureRecognizers.Add(textTap);
            questionTagLabel.GestureRecognizers.Add(tagTap);

            this.BindingContext = viewModel;
        }

        private void OnLabelTapped(object sender, EventArgs e)
        {
            viewModel.QuestionViewModel.IsExpanded = !viewModel.QuestionViewModel.IsExpanded;
        }

        async void OnAnswerButtonClicked(object sender, System.EventArgs e)
        {
            User user = JsonConvert.DeserializeObject<User>(Settings.UserSettings);
            if (user != null)
            {
                if (answerEditor.Text == null || answerEditor.Text.Length == 0
                    || answerEditor.Text.Length > MAX_LENGTH) return;

                Answer newAnswer = new Answer()
                {
                    Body = answerEditor.Text,
                    UserId = user.Id,
                    QuestionId = viewModel.QuestionViewModel.Question.Id
                };
                answerEditor.Text = null;
                var answer = await AnswersService.DoAnswersAddRequest(newAnswer);
                viewModel.LoadAnswersCommand.Execute(null);
            }
            else
            {
                await Navigation.PushModalAsync(new NavigationPage(new LoginPage()));
            }

        }

        void SetState(bool isValid, VisualElement element)
        {
            string visualState = isValid ? "Normal" : "Invalid";
            VisualStateManager.GoToState(element, visualState);
        }

        void OnAnswerSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as AnswerViewModel;

            if (item == null) return;

            item.IsExpanded = !item.IsExpanded;
            AnswersListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Answers.Count == 0)
                viewModel.LoadAnswersCommand.Execute(null);
        }

        private void OnAnswerEditorChanged(object sender, TextChangedEventArgs e)
        {
            if (answerEditor.Text == null) return;
            if (answerEditor.Text.Length > MAX_LENGTH && counterLabel.TextColor == Color.Gray)
                SetState(false, counterLabel);
            else if (answerEditor.Text.Length <= MAX_LENGTH && counterLabel.TextColor == Color.Red)
                SetState(true, counterLabel);
        }
    }
}