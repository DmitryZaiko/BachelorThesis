using BachelorThesis.ViewModels;
using System;
using BachelorThesis.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BachelorThesis.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        public LoginViewModel viewModel;
		public LoginPage ()
		{
            viewModel = new LoginViewModel();
			InitializeComponent ();
            this.BindingContext = viewModel;
		}

        private void OnLoginButtonClicked(object sender, EventArgs e)
        {
            viewModel.LoginCommand.Execute(null);
        }
    }
}