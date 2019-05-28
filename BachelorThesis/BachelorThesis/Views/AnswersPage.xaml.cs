﻿using BachelorThesis.Helpers;
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
		public AnswersPage (Question question)
		{
			InitializeComponent ();
            viewModel = new AnswersListViewModel() { QuestionViewModel = new QuestionViewModel(question)};
            this.BindingContext = viewModel;
		}

        async void OnAnswerButtonClicked(object sender, System.EventArgs e)
        {
            User user = JsonConvert.DeserializeObject<User>(Settings.UserSettings);
            if (user != null)
            {
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

        async void OnAnswerSelected(object sender, SelectedItemChangedEventArgs args)
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
    }
}