using Phonebook.Models;
using Phonebook.Views;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Realms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace Phonebook.ViewModels
{
	class AddContactViewModel : BaseViewModel
	{
		public ICommand ScanCommand { get; set; }
		public ICommand AddCommand { get; set; }
		public ICommand PhotoCommand { get; set; }
		public ImageSource Source { get; set; }
		Contacts _addContact;
		public Contacts NewContact
        {
			get
			{
				return _addContact;
			}
			set
			{
				_addContact = value;
				if (_addContact != null)
					return;
			}
		}

		public AddContactViewModel(ObservableCollection<Contacts> contactsList)
		{
			Source = "Person.png";
			NewContact = new Contacts() { Photo = "Mine.jpg" };
			AddCommand = new Command(async () =>
			{
				contactsList.Add(new Contacts 
				{
					Name = NewContact.Name,
					Last = NewContact.Last,
					Company = NewContact.Company,
					Phone = NewContact.Phone,
					Mobile = NewContact.Mobile,
					Email = NewContact.Email,
					Home = NewContact.Home
				});
				//using (var realm = Realm.GetInstance())
				//{
				//	realm.Write(() =>
				//	{
				//		realm.Add(contactsList[contactsList.Count-1]);
				//	});
				//}
				await App.Current.MainPage.Navigation.PopAsync();
			});

			PhotoCommand = new Command(DisplayElementPhoto);

			ScanCommand = new Command<Contacts>(async (_selected) => {
				var ScanPage = new ZXingScannerPage();
				ScanPage.OnScanResult += (result) =>
				{
					ScanPage.IsScanning = false;

					Device.BeginInvokeOnMainThread(async () =>
					{
						var split = result.Text.Split('*');
						contactsList.Add(new Contacts
						{
							Name = split[0],
							Last = split[1],
							Company = null,
							Phone = split[2],
							Mobile = null,
							Email = null,
							Home = null
						});
						await App.Current.MainPage.Navigation.PopAsync();
						await App.Current.MainPage.Navigation.PopAsync();
						await App.Current.MainPage.DisplayAlert("Done It", $"The new contact is {result.Text}", "Ok");
					});
				};
				await App.Current.MainPage.Navigation.PushAsync(ScanPage);
			});
		}

		public AddContactViewModel(ObservableCollection<Contacts> contacts, Contacts _selected)
        {
            NewContact = _selected;
			AddCommand = new Command(async () => {
                contacts.Add(_selected);
                await App.Current.MainPage.Navigation.PopAsync();
            });
        }

		public async void DisplayElementPhoto()
		{
			string Action, Title = "Change Photo", Cancel = "Cancel", But1 = $"Take Photo", But2 = "Choose Photo";
			Action = await App.Current.MainPage.DisplayActionSheet($"{Title}", "", $"{Cancel}", $"{But1}", $"{But2}");

			if (Action == But1)
			{
				await CrossMedia.Current.Initialize();

				if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
					return;

				var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
				{
					SaveToAlbum = true
				});

				if (file != null)
				{
					Source = ImageSource.FromStream(() =>
					{
						return file.GetStream();
					});
				}
			}
			else if (Action == But2)
			{
				await CrossMedia.Current.Initialize();

				var file = await CrossMedia.Current.PickPhotoAsync();

				if (file != null)
				{
					Source = ImageSource.FromStream(() =>
					{
						return file.GetStream();
					});
				}
			}
			else
			{
				return;
			}
		}
	}
}