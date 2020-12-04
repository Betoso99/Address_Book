using Phonebook.Models;
using Phonebook.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace Phonebook.ViewModels
{
	class ContactsViewModel : BaseViewModel
	{
        public ICommand DeleteCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand AddNavigationCommand { get; set; }
        public ObservableCollection<Contacts> ContactsList { get; set; } = new ObservableCollection<Contacts>();
        Contacts _selected;
        public Contacts Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
                if (_selected != null)
                    DisplayElementSelect();
            }
        }

		public ContactsViewModel(ObservableCollection<Contacts> contacts)
        {
            AddNavigationCommand = new Command(async (_selected) =>
            {
                await App.Current.MainPage.Navigation.PushAsync(new AddContactPage(ContactsList));
            });

            DeleteCommand = new Command<Contacts>(async (_selected) => {
                if (ContactsList.Remove(_selected))
                    await App.Current.MainPage.DisplayAlert("Deleted", "Se ha borrado un contacto", "OK");
            });
            
            EditCommand = new Command<Contacts>(async (_selected) => {
                ContactsList.Remove(_selected);
                await App.Current.MainPage.Navigation.PushAsync(new AddContactPage(ContactsList, _selected));
            });
        }

        public async void DisplayElementSelect()
        {
            string Action, Title = "Choose", Cancel = "Cancel", But1 = $"Edit", But2 = "More Info", But3 = $"Call {Selected.Phone}";
            Action = await App.Current.MainPage.DisplayActionSheet($"{Title}", "", $"{Cancel}", $"{But1}", $"{But2}", $"{But3}");

            if (Action == But1)
            {
                ContactsList.Remove(_selected);
                await App.Current.MainPage.Navigation.PushAsync(new AddContactPage(ContactsList, _selected));
            }
            else if (Action == But2)
            {
                await App.Current.MainPage.DisplayAlert("Contact", $"Name: {Selected.Name} {Selected.Last} \n " +
                                                       $"Number : {Selected.Phone} \n " +
                                                       $"Mail : {Selected.Email}", "OK");
            }
            else if (Action == But3)
            {
                PlacePhoneCall(_selected.Phone);
            }
			else
			{
                return;
			}
        }

        public void PlacePhoneCall(string number)
        {
            try
            {
                PhoneDialer.Open(number);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }
    }
}
