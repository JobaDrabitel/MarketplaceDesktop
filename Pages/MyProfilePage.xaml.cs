using MarketplaceDesktop.Models;
using MarketplaceDesktop.Pages;
using System;
using System.Collections.Generic;
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
	/// Логика взаимодействия для ProfilePage.xaml
	/// </summary>
	public partial class MyProfilePage : Page
	{
		User User { get; set; }
		Marketplace1Context _context = new();
		public MyProfilePage(User user)
		{
			InitializeComponent();
			User = user;
			CategoryComboBox.ItemsSource = _context.Categories.Select(c => c.Name).ToList();
			DataContext = User;
			ProfileItemControl.ItemsSource = _context.Users.Where(u => u.UserId == User.UserId).ToList();
		}
		
		private void Logout_Click(object sender, RoutedEventArgs e)
		{
			User = null;
			AuthWindow authWindow = new AuthWindow();
			authWindow.Show();
			Window.GetWindow(this).Close();
		}
		private async void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			var selectedCategory = CategoryComboBox.SelectedItem;
			Window.GetWindow(this).Content = new ProductSearchPage(User, (string)selectedCategory, SearchTextBox.Text);
		}

		private async void CartButton_Click(object sender, RoutedEventArgs e)
		{
			UserController userController = new UserController(_context);
			User = await userController.GetUser(1);
			Button button = (Button)sender;
			Product selectedProduct = (Product)button.Tag;
			User.ProductsNavigation.Add(selectedProduct);
			await userController.PutUser(User.UserId, User);
			await _context.SaveChangesAsync();
		}
		private void Cart_Click(object sender, RoutedEventArgs e)
		{
			Window.GetWindow(this).Content = new CartPage(User);
		}
		private void Check_User()
		{

		}

		private void Home_Click(object sender, RoutedEventArgs e)
		{
			Window.GetWindow(this).Content = new MainPage(User);
		}
		private void MyProducts_Click(object sender, RoutedEventArgs e)
		{
			Window.GetWindow(this).Content = new MyProducts(User);
		}

	

		private async void SaveChangesButton_Click(object sender, RoutedEventArgs e)
		{
			User currentUser = (User)((Button)sender).Tag;

			await _context.SaveChangesAsync();

			if (!ValidateUserData())
			{
				MessageBox.Show("Некорректные данные. Пожалуйста, проверьте введенные значения.");
				return;
			}

		
			MessageBox.Show("Изменения сохранены успешно.");
		}
		private bool ValidateUserData()
		{
			return true;
		}

		private void MyOrdersButton_Click(object sender, RoutedEventArgs e)
		{
			Window.GetWindow(this).Content = new MyOrdersPage(User);
		}
	}
}
