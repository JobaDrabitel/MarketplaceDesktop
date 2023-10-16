using MarketplaceDesktop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace MarketplaceDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для CreateOrderWindow.xaml
    /// </summary>
    public partial class CreateOrderWindow : Window
    {
		Product Product { get; set; }
		User User { get; set; }
		Marketplace1Context _context = new();
		Window Window {  get; set; }
		public CreateOrderWindow(User user, Product product, Window window)
		{
            InitializeComponent();
			User = user;
			Product = product;
			DataContext = Product;
			Window = window;
		}
		private async void CreateOrder_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				ProductController productController = new ProductController(_context);
				UserController userController = new UserController(_context);
				OrderController orderController = new OrderController(_context);

				// Получить количество товара, введенное в TextBox
				if (int.TryParse(QuantityTextBox.Text, out int quantity))
				{
					if (quantity > 0)
					{
						if (quantity <= Product.StockQuantity)
						{
							// Создать новый заказ
							Order order = new Order
							{
								TotalQuantity = quantity,
								TotalAmount = quantity * Product.Price,
								UserId = User.UserId,
								ProductId = Product.ProductId,
								CreateTime = DateTime.Now,
							};
							_context.Orders.Add(order);
							if (User.UserId != 0 && User != null)
							{
								var user = await _context.Users.FindAsync(User.UserId);
								if (user != null)
								{
									var productToRemove = user.ProductsNavigation.FirstOrDefault(p => p.ProductId == Product.ProductId);

									if (productToRemove != null)
									{
										user.ProductsNavigation.Remove(productToRemove);
										_context.SaveChanges(); // Сохраняем изменения в БД.
									}
										User = user;
								}
							}
							MessageBox.Show("Заказ оформлен");
							this.Close();
							Window.Content = new CartPage(User);
						}
						else
						{
							MessageBox.Show("Ошибка при выборе количества товара");
							QuantityTextBox.Text = Product.StockQuantity.ToString();
						}
					}
					else
					{
						MessageBox.Show("Количество товара должно быть больше 0");
					}
				}
				else
				{
					MessageBox.Show("Ошибка при выборе количества товара, введите целое число");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

	}
}
