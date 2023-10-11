using MarketplaceDesktop.Models;
using MarketplaceDesktop.Pages ;
using MarketplaceDesktop.Pages.Moderator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace MarketplaceDesktop.Pages
{
	/// <summary>
	/// Логика взаимодействия для AdminPage.xaml
	/// </summary>
	public partial class AdminPage : Window
	{
		Marketplace1Context db = new Marketplace1Context();
		User _user;
		public AdminPage(User user)
		{
			InitializeComponent();
			_user = user;
			RefreshProductList();
			RefreshUserList();
		}
		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			RefreshProductList();
			RefreshUserList();
		}

		private void RefreshProductList()
		{
			var products = db.Products.ToList();
			ProductGrid.ItemsSource = products;
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			ProductWindow productWindow = new ProductWindow(new Product());
			if (productWindow.ShowDialog() == true)
			{
				Product product = productWindow.Product;
				db.Products.Add(product);
				db.SaveChanges();
				RefreshProductList();
			}
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			Product selectedProduct = ProductGrid.SelectedItem as Product;
			if (selectedProduct == null) return;

			ProductWindow productWindow = new ProductWindow(selectedProduct);

			if (productWindow.ShowDialog() == true)
			{
				db.SaveChanges();
				RefreshProductList();
			}
		}

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			Product selectedProduct = ProductGrid.SelectedItem as Product;
			if (selectedProduct == null) return;
			db.Products.Remove(selectedProduct);
			db.SaveChanges();
			RefreshProductList();
		}

		void AdminWindow_Click(object sender, RoutedEventArgs e)
		{

		}
		void MyProfile_Click(object sender, RoutedEventArgs e)
		{
			
		}
		void Logout_Click(object sender, RoutedEventArgs e)
		{

		}
		private void RefreshUserList()
		{
			var users = db.Users.ToList();
			UserGrid.ItemsSource = users;
		}

		private void AddUserButton_Click(object sender, RoutedEventArgs e)
		{
			UserWindow userWindow = new UserWindow(new User());
			if (userWindow.ShowDialog() == true)
			{
				User user = userWindow.User;
				db.Users.Add(user);
				db.SaveChanges();
				RefreshUserList();
			}
		}

		private void EditUserButton_Click(object sender, RoutedEventArgs e)
		{
			User selectedUser = UserGrid.SelectedItem as User;
			if (selectedUser == null) return;

			UserWindow userWindow = new UserWindow(new User
			{
				UserId = selectedUser.UserId,
				FirstName = selectedUser.FirstName,
				LastName = selectedUser.LastName,
				Email = selectedUser.Email,
				PasswordHash = selectedUser.PasswordHash,
				Phone = selectedUser.Phone
			});

			if (userWindow.ShowDialog() == true)
			{
				selectedUser = db.Users.Find(userWindow.User.UserId);
				if (selectedUser != null)
				{
					selectedUser.FirstName = userWindow.User.FirstName;
					selectedUser.LastName = userWindow.User.LastName;
					selectedUser.Email = userWindow.User.Email;
					selectedUser.PasswordHash = userWindow.User.PasswordHash;
					selectedUser.Phone = userWindow.User.Phone;
					db.SaveChanges();
					RefreshUserList();
				}
			}
		}

		private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
		{
			User selectedUser = UserGrid.SelectedItem as User;
			if (selectedUser == null) return;
			db.Users.Remove(selectedUser);
			db.SaveChanges();
			RefreshUserList();
		}

	}
}
