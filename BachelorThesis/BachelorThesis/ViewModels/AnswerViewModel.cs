using BachelorThesis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BachelorThesis.ViewModels
{
    class AnswerViewModel : BaseViewModel
    {
        public Answer answer;
        private bool isExpanded;

        public Answer Answer {
            get
            {
                return answer;
            }
            set
            {
                SetProperty(ref answer, value);
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
            }
        }


        public string Body
        {
            get
            {
                if (IsExpanded)
                    return Answer.Body;
                else
                    return BodyPreview;
            }
        }

        public bool HasPreview
        {
            get
            {
                return Answer.Body.Length > 256;
            }
        }

        public string BodyPreview
        {
            get
            {
                return HasPreview ? Answer.Body.Remove(256) : Answer.Body;
            }
        }

        public AnswerViewModel()
        {
            Answer = new Answer();
            IsExpanded = false;
        }

        public AnswerViewModel(Answer answer)
        {
            IsExpanded = false;
            Answer = answer;
        }
    }
}
