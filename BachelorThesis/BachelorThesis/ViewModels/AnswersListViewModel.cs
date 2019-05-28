using BachelorThesis.Models;
using BachelorThesis.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BachelorThesis.ViewModels
{
    class AnswersListViewModel : BaseViewModel
    {
        public QuestionViewModel QuestionViewModel { get; set; }
        public ObservableCollection<AnswerViewModel> Answers { get; set; }
        public Command LoadAnswersCommand { get; set; }
        public Answer AnswerSelected { get; set; }




        public AnswersListViewModel()
        {
            Answers = new ObservableCollection<AnswerViewModel>();
            LoadAnswersCommand = new Command(async (args) => {
                await ExecuteLoadAnswersCommand();
            });
        }

        protected async Task ExecuteLoadAnswersCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Answers.Clear();
                var id = QuestionViewModel.Question.Id;
                object responce = await AnswersService.DoAnswersGetRequest(id);

                if (responce is ErrorMessage)
                {
                    ErrorMessage error = responce as ErrorMessage;
                    Debug.WriteLine(error.ErrorCode + " " + error.ErrorDescription);
                }
                else
                {
                    var answers = responce as IEnumerable<Answer>;
                    foreach (var answer in answers)
                    {
                        Answers.Add(new AnswerViewModel(answer));
                    }
                }

                OnPropertyChanged("IsExpanded");
                //MessagingCenter.Send<AnswersViewModel>(this, "AnswersLoaded");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[!] ExecuteLoadAnswersCommand --- " + ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
