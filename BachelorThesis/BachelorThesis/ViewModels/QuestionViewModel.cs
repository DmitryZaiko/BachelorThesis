using BachelorThesis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BachelorThesis.ViewModels
{
    class QuestionViewModel : BaseViewModel
    {
        public Question question;
        private bool isExpanded;

        public Question Question
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

        public bool IsExpanded
        {
            get
            {
                return isExpanded;
            }
            set
            {
                SetProperty(ref isExpanded, value);
                OnPropertyChanged("Body");
                OnPropertyChanged("MoreTag");
            }
        }

        public string MoreTag
        {
            get {
                if (!IsExpanded)
                {
                    return "Lasīt vairāk";
                }
                else
                {
                    return "Sagriezt";
                }
            }
        }

        public string Body
        {
            get
            {
                if (IsExpanded)
                    return Question.Body;
                else
                    return BodyPreview;
            }
        }

        public bool HasPreview
        {
            get
            {
                return Question.Body.Length > 128;
            }
        }

        public string BodyPreview
        {
            get
            {
                return HasPreview ? Question.Body.Remove(128) + " ..." : Question.Body;
            }
        }

        public QuestionViewModel()
        {
            Question = new Question();
            IsExpanded = false;
        }

        public QuestionViewModel(Question question)
        {
            IsExpanded = false;
            Question = question;
        }
    }
}
