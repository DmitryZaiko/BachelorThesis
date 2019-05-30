using BachelorThesis.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BachelorThesis.ViewModels
{
    public class CompilerViewModel : BaseViewModel
    {
        public Command GetUidCommand { get; set; }
        public Command PostCodeCommand { get; set; }
        private string result = "  Programmas izvade";
        private string text = "  using System.IO;\n  using System;\n\n  class Program\n  {\n    static void " +
                              "Main()\n    {\n        Console.WriteLine(\"Hello, World!\");\n    }\n      \n  }";

        public string Result {
            get
            {
                return result;
            }
            set
            {
                SetProperty(ref result, value);
            }
        }

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                SetProperty(ref text, value);
            }
        }


        public CompilerViewModel()
        {
            GetUidCommand  = new Command(async (args) => {
                await CompilerService.DoGetUIDRequest();
            });

            PostCodeCommand = new Command(async (args) => {
                var html = await CompilerService.DoPostCodeRequest(args as string) as string;
                Result = html;
                MessagingCenter.Send<CompilerViewModel>(this, "CompileActivityEnded");
            });
        }
    }
}
