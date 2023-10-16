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

namespace MarketplaceDesktop
{
    /// <summary>
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
		User User {get; set;}
		Marketplace1Context _context = new();
		public CartPage(User user)
		{
			InitializeComponent();
			User = user;
			ProductsItemsControl.ItemsSource = User.ProductsNavigation.ToList();
			DataContext = User.ProductsNavigation.ToList();
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
		private void Profile_Click(object sender, RoutedEventArgs e)
		{
			Window.GetWindow(this).Content = new MyProfilePage(User);
		}
		private void QuantityTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			// Проверка, что вводится только число
			if (!int.TryParse(e.Text, out _))
			{
				e.Handled = true;
			}
		}

		private async void CreateOrderButton_Click(object sender, RoutedEventArgs e)
		{
			Product product = (Product)((Button)sender).Tag;
			CreateOrderWindow createOrderWindow = new CreateOrderWindow(User, product, Window.GetWindow(this));
			createOrderWindow.Show();
		}

	}
}
