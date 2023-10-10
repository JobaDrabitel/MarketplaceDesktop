using MarketplaceDesktop.Models;
using MarketplaceDesktop.Pages;
using MarketplaceDesktop.Pages.Moderator;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace MarketplaceDesktop
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Marketplace1Context _context = new();
		public MainWindow()
		{
			InitializeComponent();
			ProductsItemsControl.ItemsSource = _context.Products.ToList();
			CategoryComboBox.ItemsSource = _context.Categories.Select(c => c.Name).ToList();
		}

		private async void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			//UserController userController = new(_context);
			//var email = txtEmail.Text;
			//var password = txtPassword.Password;

			//var user = await userController.AuthorizeUser(email, password);
			//switch (user.Roles.First().RoleId)
			//{
			//	case 2:
			//		{
			//			ModeratorPage moderatorPage = new(user);
			//			moderatorPage.Show();
			//			Hide();
			//			break;
			//		}
			//	case 3:
			//		{
			//			AdminPage adminPage = new(user);
			//			adminPage.Show();
			//			Hide();
			//			break;
			//		}
			//}
		}
		
	}
}
