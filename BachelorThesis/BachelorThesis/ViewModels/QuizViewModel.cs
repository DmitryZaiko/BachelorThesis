using BachelorThesis.Models;
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
    public class QuizViewModel : ItemsViewModel
    {
        public Queue<Item> Quizes { get; set; }
        public ObservableCollection<QuizAnswerViewModel> Answers { get; set; }
        public Command LoadPageCommand;
        private string question;
        private int rightAnswer;

        public QuizViewModel(URLHttpParams httpParams)
        {
            PageType = ItemType.QuizAnswer;
            Answers = new ObservableCollection<QuizAnswerViewModel>();
            LoadPageCommand = new Command(async () => await ExcuteLoadPageCommand(httpParams));
            MessagingCenter.Subscribe<ItemsViewModel>(this, "ItemsLoaded", (sender) => {
               Item i = Items.Where(x => ((QuizAnswer)x).IsRight == 0).First();
               foreach(var item in Items)
               {
                    var quizAnswer = new QuizAnswerViewModel(item as QuizAnswer);
                    Answers.Add(quizAnswer);
               }
               RightAnswer = i.Id;
            });
        }

        public string Question
        {
            get
            {
                return question;
            }

            set
            {
                SetProperty(ref question, value);
            }
        }

        public int RightAnswer
        {
            get
            {
                return rightAnswer;
            }

            set
            {
                SetProperty(ref rightAnswer, value);
            }
        }

        async Task  ExcuteLoadPageCommand(object obj)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var quizes = await DataStore.GetItemsAsync(true, obj);

                if (quizes == null) return;

                Quizes = new Queue<Item>(quizes);
                MessagingCenter.Send<QuizViewModel>(this, "QuizesLoaded");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }

            Item currentQuiz = Quizes.Dequeue();
            Question = currentQuiz.Name;
            URLHttpParams httpParams = new URLHttpParams(PageType, currentQuiz.Id.ToString());
            await ExecuteLoadItemsCommand(httpParams);
        }

    }
}
