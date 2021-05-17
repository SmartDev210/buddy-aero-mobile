using System.ComponentModel;
using WeavyMobile.ViewModels;
using Xamarin.Forms;

namespace WeavyMobile.Views
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