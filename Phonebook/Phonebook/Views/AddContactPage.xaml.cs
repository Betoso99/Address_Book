using Phonebook.Models;
using Phonebook.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Phonebook.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddContactPage : ContentPage
	{
		public AddContactPage(ObservableCollection<Contacts> contactsList)
		{
			InitializeComponent();
			BindingContext = new AddContactViewModel(contactsList);
		}

		public AddContactPage(ObservableCollection<Contacts> contactsList, Contacts selected)
		{
			InitializeComponent();
			BindingContext = new AddContactViewModel(contactsList, selected);
		}
	}
}