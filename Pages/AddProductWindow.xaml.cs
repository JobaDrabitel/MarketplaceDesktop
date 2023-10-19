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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace MarketplaceDesktop.Pages
{
	/// <summary>
	/// Логика взаимодействия для AddProductWindow.xaml
	/// </summary>
	public partial class AddProductWindow : Window
	{
		Window Window { get; set; }
		User User {  get; set; } = new User();
		Marketplace1Context marketplaceContext = new Marketplace1Context();
		public AddProductWindow(User user, Window window)
		{
			InitializeComponent();
			User = user;
			categoryComboBox.ItemsSource = marketplaceContext.Categories.ToList();
			Window = window;
		}

		private async void SaveProductButton_Click(object sender, RoutedEventArgs e)
		{
			UserController userController = new UserController(marketplaceContext);
			Product newProduct = new Product
			{
				Name = txtName.Text,
				Description = txtDescription.Text,
				Price = decimal.Parse(txtPrice.Text),
				StockQuantity = int.Parse(txtStock.Text),
				ImageUrl = txtUrl.Text,
				CreatedAt = DateTime.Now,

				Categories = new List<Category>() { (Category)categoryComboBox.SelectedItem } 
			};
			User.Products.Add(newProduct);
			await userController.PutUser(User.UserId, User);

			try
			{
				await marketplaceContext.SaveChangesAsync();
				MessageBox.Show("Продукт успешно добавлен.");
				this.Close();
				Window.Content = new MyProducts(User);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Произошла ошибка при добавлении продукта: " + ex.Message);
			}
		}
	}
}
