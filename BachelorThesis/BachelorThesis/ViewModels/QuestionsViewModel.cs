using BachelorThesis.Models;
using BachelorThesis.Services;
using BachelorThesis.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BachelorThesis.ViewModels
{
    public class QuestionsViewModel : BaseViewModel
    {
        public ObservableCollection<Question> Questions { get; set; }
        public Command LoadQuestionsCommand { get; set; }
        public int? LessonId { get; set; }

        public QuestionsViewModel()
        {
            Questions = new ObservableCollection<Question>();
            LoadQuestionsCommand = new Command(async (args) => {
                await ExecuteLoadQuestionsCommand();
            });

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                //Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
        }

        protected async Task ExecuteLoadQuestionsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Questions.Clear();
                object responce = await QuestionsService.DoQuestionsGetRequest(LessonId);

                if (responce is ErrorMessage)
                {
                    ErrorMessage error = responce as ErrorMessage;
                    Debug.WriteLine(error.ErrorCode + " " + error.ErrorDescription);
                }
                else
                {
                   var questions = responce as IEnumerable<Question>;
                   foreach (var question in questions)
                   {
                        Questions.Add(question);
                   }
                }


                MessagingCenter.Send<QuestionsViewModel>(this, "QuestionsLoaded");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[!] ExecuteLoadQuestionsCommand --- " + ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
