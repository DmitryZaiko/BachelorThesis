using BachelorThesis.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BachelorThesis.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CompilerPage : ContentPage
	{
        CompilerViewModel viewModel;
		public CompilerPage ()
		{
			InitializeComponent ();
            viewModel = new CompilerViewModel();
            this.BindingContext = viewModel;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.GetUidCommand.Execute(null);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var code = codeEditor.Text;
            viewModel.PostCodeCommand.Execute(code);
        }
    }
}