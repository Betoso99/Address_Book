using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phonebook.Models
{
	public class Contacts : RealmObject
	{
		public string Photo { get; set; }
		public string Name { get; set; }
		public string Last { get; set; }
		public string Company { get; set; }
		public string Phone { get; set; }
		public string Mobile { get; set; }
		public string Email { get; set; }
		public string Home { get; set; }
	}
}
