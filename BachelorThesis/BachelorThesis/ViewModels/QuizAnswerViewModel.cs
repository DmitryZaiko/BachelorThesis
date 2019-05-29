using BachelorThesis.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BachelorThesis.ViewModels
{
    public class QuizAnswerViewModel : BaseViewModel
    {
        private bool isItemSelected = false;
        public QuizAnswer QuizAnswer { get; set; }

        public QuizAnswerViewModel(QuizAnswer quizAnswer)
        {
            QuizAnswer = quizAnswer;
        }

        public bool IsItemSelected
        {
            get
            {
                return isItemSelected;
            }
            set
            {
                SetProperty(ref isItemSelected, value);
                OnPropertyChanged("AnswerColor");
            }
        }

        public Color AnswerColor
        {
            get
            {
                if (!IsItemSelected) return Color.White;
                return QuizAnswer.IsRight == 0 ? Color.FromHex("#85F49D") : Color.FromHex("#FF8D8C");
            }
        }
    }
}
