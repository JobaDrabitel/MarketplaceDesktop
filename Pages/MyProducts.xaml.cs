using MarketplaceDesktop.Models;
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

namespace MarketplaceDesktop.Pages
{
	/// <summary>
	/// Логика взаимодействия для MyProducts.xaml
	/// </summary>
	public partial class MyProducts : Page
	{
		public ContentControl maincontent { get; set; }
		User User { get; set; }
		Marketplace1Context _context = new();
		public MyProducts(User user)
		{
			InitializeComponent();
			User = user;
			ProductsItemsControl.ItemsSource = _context.Products.Where(p => p.Users.First().UserId == user.UserId).ToList();
			CategoryComboBox.ItemsSource = _context.Categories.Select(c => c.Name).ToList();
		}
		private void Product_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			Border border = (Border)sender;
			Product selectedProduct = (Product)border.Tag;
			ProductDetailWindow detailWindow = new ProductDetailWindow(selectedProduct);
			detailWindow.Show();
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

		private void AddProduct_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
