﻿using MarketplaceDesktop.Models;
using MarketplaceDesktop.Pages ;
using MarketplaceDesktop.Pages.Moderator;
using Microsoft.EntityFrameworkCore;
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
		User User { get; set; }
		public AdminPage(User user)
		{
			InitializeComponent();
			RefreshProductList();
			RefreshUserList();
			RefreshReviewList();
			RefreshOrdersList();
			ApprovehProductList();
			User = user;
		}
		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			RefreshProductList();
			RefreshUserList();
			RefreshReviewList();
		}

		private void RefreshProductList()
		{
			var products = db.Products.ToList();
			ProductGrid.ItemsSource = products.Where(p => p.UpdatedAt != null);
		}
		private void ApprovehProductList()
		{
			var products = db.Products.ToList();
			ProductsApproveGrid.ItemsSource = products.Where(p => p.UpdatedAt == null);
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
		}private void AddReviewButton_Click(object sender, RoutedEventArgs e)
		{
			ReviewWindow productWindow = new ReviewWindow(new Review());
			if (productWindow.ShowDialog() == true)
			{
				Review review = productWindow.Review;
				db.Reviews.Add(review);
				db.SaveChanges();
				RefreshReviewList();
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
	
		private void Logout_Click(object sender, RoutedEventArgs e)
		{
			User = null;
			AuthWindow authWindow = new AuthWindow();
			authWindow.Show();
			this.Close();
		}
		private void RefreshUserList()
		{
			var users = db.Users.ToList();
			UserGrid.ItemsSource = users;
		}private void RefreshReviewList()
		{
			var reviews = db.Reviews.ToList();
			ReviewsGrid.ItemsSource = reviews;
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

		private void RefreshOrdersList()
		{
			var orders = db.Orders.Where(o => o.TotalQuantity == 0).ToList();
			CancelOrdersGrid.ItemsSource = orders;
		}
		private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
		{
			User selectedUser = UserGrid.SelectedItem as User;
			if (selectedUser == null) return;
			db.Users.Remove(selectedUser);
			db.SaveChanges();
			RefreshUserList();
		}private void DeleteReviewButton_Click(object sender, RoutedEventArgs e)
		{
			Review selectedReview = ReviewsGrid.SelectedItem as Review;
			if (selectedReview == null) return;
			db.Reviews.Remove(selectedReview);
			db.SaveChanges();
			RefreshReviewList();
		}
		private void Profile_Click(object sender, RoutedEventArgs e)
		{
			Window.GetWindow(this).Content = new MyProfilePage(User);
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			ProductController productController = new ProductController(db);
			Product selectedProduct = ProductsApproveGrid.SelectedItem as Product;
			if (selectedProduct == null) return;
			selectedProduct.UpdatedAt = DateTime.Now;
			await productController.PutProduct(selectedProduct.ProductId, selectedProduct);
			db.SaveChanges();
			ApprovehProductList();
		}
		private async void AcceptCancelOrder_Click(object sender, RoutedEventArgs e)
		{
			OrderController orderController = new(db);
			Order selectedOrder = CancelOrdersGrid.SelectedItem as Order;
			if (selectedOrder == null) return;
			await orderController.PutOrder(selectedOrder.OrderId, selectedOrder);
			db.SaveChanges();
			RefreshOrdersList();
		}
		private void RejectCancelOrder_Click(object sender, RoutedEventArgs e)
		{
			Order selectedOrder = CancelOrdersGrid.SelectedItem as Order;
			if (selectedOrder == null) return;
			selectedOrder.TotalQuantity = Convert.ToInt32(selectedOrder.TotalAmount / selectedOrder.Product.Price);
			db.SaveChanges();
			RefreshOrdersList();
		}
	}
}
