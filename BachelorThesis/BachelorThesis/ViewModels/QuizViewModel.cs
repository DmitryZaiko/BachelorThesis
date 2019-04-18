using BachelorThesis.Models;
using System;
using System.Collections.Generic;
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
        public Command LoadPageCommand;
        private string question;

        public QuizViewModel(URLHttpParams httpParams)
        {
            PageType = ItemType.QuizAnswer;
            LoadPageCommand = new Command(async () => await ExcuteLoadPageCommand(httpParams));
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
