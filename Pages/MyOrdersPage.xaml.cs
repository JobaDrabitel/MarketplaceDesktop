using MarketplaceDesktop.Models;
using Microsoft.EntityFrameworkCore;
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
	/// Логика взаимодействия для MyOrdersPage.xaml
	/// </summary>
	public partial class MyOrdersPage : Page
	{
		Marketplace1Context _context = new();
		User User { get; set; }
		public MyOrdersPage(User user)
		{
			InitializeComponent();
			DataContext = user.Orders.Where(o => o.TotalQuantity!= 0);
			User = user;
		}
		private void Product_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			Border border = (Border)sender;
			Product selectedProduct = (Product)border.Tag;
			ProductDetailWindow detailWindow = new ProductDetailWindow(selectedProduct, User);
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
			var selectedCategory = CategoryComboBox.SelectedItem;
			Window.GetWindow(this).Content = new ProductSearchPage(User, (string)selectedCategory, SearchTextBox.Text);
		}

		private async void CartButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				UserController userController = new UserController(_context);
				User = await userController.GetUser(1);
				Button button = (Button)sender;
				Product selectedProduct = (Product)button.Tag;
				if (User.ProductsNavigation.Contains(selectedProduct))
				{
					MessageBox.Show("Продукт уже в корзине");
					return;
				}
				User.ProductsNavigation.Add(selectedProduct);
				await userController.PutUser(User.UserId, User);
				await _context.SaveChangesAsync();
				MessageBox.Show("Продукт добавлен в корзину");
			}
			catch (Exception ex) { MessageBox.Show("Error"); }
		}
		private void Cart_Click(object sender, RoutedEventArgs e)
		{
			Window.GetWindow(this).Content = new CartPage(User);
		}

		private void MyProducts_Click(object sender, RoutedEventArgs e)
		{
			Window.GetWindow(this).Content = new MyProducts(User);
		}
		private void Home_Click(object sender, RoutedEventArgs e)
		{
			Window.GetWindow(this).Content = new MainPage(User);
		}


		private void AddProduct_Click(object sender, RoutedEventArgs e)
		{
			AddProductWindow addProductWindow = new(User, Window.GetWindow(this));
			addProductWindow.Show();
		}
		private void TextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			if (sender is TextBox textBox)
			{
				textBox.SelectAll();
			}
		}
		private void Profile_Click(object sender, RoutedEventArgs e)
		{
			Window.GetWindow(this).Content = new MyProfilePage(User);
		}
	

	private async void CancelOrderButton_Click(object sender, RoutedEventArgs e)
		{
			OrderController orderController = new(_context);
			Order order = (sender as Button)?.Tag as Order;

			if (order != null)
			{
				var cancelOrder = await _context.Orders.FindAsync(order.OrderId);
				cancelOrder.TotalQuantity = 0;
				cancelOrder = await orderController.PutOrder(cancelOrder.OrderId, cancelOrder);
				User = await _context.Users.FindAsync(User.UserId);
				MessageBox.Show("Заказ отправлен на рассмотрение");
				Window.GetWindow(this).Content = new MyOrdersPage(User);
			}
		}
	}
}
