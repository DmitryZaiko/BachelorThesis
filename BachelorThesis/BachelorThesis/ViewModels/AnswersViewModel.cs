using BachelorThesis.Models;
using BachelorThesis.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BachelorThesis.ViewModels
{
    class AnswersViewModel : BaseViewModel
    {
        public Question Question { get; set; }
        public ObservableCollection<Answer> Answers { get; set; }
        public Command LoadAnswersCommand { get; set; }

        public AnswersViewModel()
        {
            Answers = new ObservableCollection<Answer>();
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
                object responce = await AnswersService.DoAnswersGetRequest(Question.Id);

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
                        Answers.Add(answer);
                    }
                }


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
