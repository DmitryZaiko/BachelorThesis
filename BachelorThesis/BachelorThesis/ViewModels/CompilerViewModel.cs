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
        private HtmlWebViewSource result = new HtmlWebViewSource() { Html = "Default Text"};

        public HtmlWebViewSource Result {
            get
            {
                return result;
            }
            set
            {
                SetProperty(ref result, value);
            }
        }

        public CompilerViewModel()
        {
            GetUidCommand  = new Command(async (args) => {
                await CompilerService.DoGetUIDRequest();
            });

            PostCodeCommand = new Command(async (args) => {
                var html = await CompilerService.DoPostCodeRequest(args as string) as string;
                Result = new HtmlWebViewSource() { Html = html };
            });
        }
    }
}
