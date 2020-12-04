using Phonebook.ViewModels;
using Phonebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

namespace Phonebook.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContacsPage : ContentPage
	{
		public ContacsPage(ObservableCollection<Contacts> contactsList)
		{
			InitializeComponent();
			BindingContext = new ContactsViewModel(contactsList);
		}

		public void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
		{
			string toLower = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(e.NewTextValue);
			var ContainerSearch = BindingContext as ContactsViewModel;
			ContactList.BeginRefresh();
			if (string.IsNullOrWhiteSpace(toLower))
			{
				ContactList.ItemsSource = ContainerSearch.ContactsList;
			}
			else
			{
				ContactList.ItemsSource = ContainerSearch.ContactsList.Where(s => s.Name.Contains(toLower));
			}
			ContactList.EndRefresh();
			SearchBar searchBar = (SearchBar)sender;
		}

	}
}