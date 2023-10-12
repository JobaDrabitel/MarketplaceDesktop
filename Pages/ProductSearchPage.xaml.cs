using MarketplaceDesktop.Models;
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
    /// Логика взаимодействия для ProductSearchPage.xaml
    /// </summary>
    public partial class ProductSearchPage : Page
    {
		User User { get; set; }
		Marketplace1Context _context = new();
		public ProductSearchPage(User user, int categoryId, string searchTerm)
		{
			InitializeComponent();
			User = user;
			ProductsItemsControl.ItemsSource = _context.Products.ToList();
			CategoryComboBox.ItemsSource = _context.Categories.Select(c => c.Name).ToList();
		}
		private void Product_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			Border border = (Border)sender;
			Product selectedProduct = (Product)border.Tag;
			ProductDetailWindow detailWindow = new ProductDetailWindow(selectedProduct);
			detailWindow.Show();
		}

		private async void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			this.Content = new MainWindow(User);
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
			this.Content = new CartPage(User);
		}
	}
}
