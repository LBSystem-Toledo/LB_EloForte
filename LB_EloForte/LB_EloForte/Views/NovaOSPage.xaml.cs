using LB_EloForte.ViewModels;
using Xamarin.Forms;

namespace LB_EloForte.Views
{
    public partial class NovaOSPage : ContentPage
    {
        public NovaOSPage()
        {
            InitializeComponent();
        }

        private void pkServico_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var viewModel = (NovaOSPageViewModel)BindingContext;
            if (viewModel.BuscarPrecoItem.CanExecute())
                viewModel.BuscarPrecoItem.Execute();
        }

        private void pkTabPreco_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var viewModel = (NovaOSPageViewModel)BindingContext;
            if (viewModel.BuscarPrecoItem.CanExecute())
                viewModel.BuscarPrecoItem.Execute();
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender != null)
            {
                var texto = (Entry)sender;
                if (!string.IsNullOrWhiteSpace(texto.Text))
                {
                    if (texto.Text.Contains(","))
                        texto.Text = texto.Text.Replace(",", ".");
                }
            }
        }
    }
}
