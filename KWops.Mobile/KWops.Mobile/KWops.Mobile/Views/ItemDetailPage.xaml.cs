using KWops.Mobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace KWops.Mobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}