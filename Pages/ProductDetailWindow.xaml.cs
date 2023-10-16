using MarketplaceDesktop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using static System.Net.Mime.MediaTypeNames;

namespace MarketplaceDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductDetailWindow.xaml
    /// </summary>
    public partial class ProductDetailWindow : Window
    {
		Marketplace1Context _context = new();
		User User { get; set; }
		Product Product { get; set; }
        public ProductDetailWindow(Product product, User user)
        {
            InitializeComponent();
			DataContext = product;
			ReviewsControl.ItemsSource = product.Reviews.ToList();
            UserControl.ItemsSource = product.Users.ToList();
            User = user;
			Product = product;
		}
		private async void AddCommentButton_Click(object sender, RoutedEventArgs e)
		{
			int rating = Convert.ToInt32( RatingSlider.Value);
			string commentText = CommentTextBox.Text;
			string imageUrl = ImageUrlTextBox.Text;
			Review review = new Review()
			{
				UserId = User.UserId,
				ProductId = Product.ProductId,
				Rating = rating,
				Comment = commentText,
				CreatedAt = DateTime.UtcNow,
				ImageUrl = imageUrl
			};
			try
			{
				_context.Reviews.Add(review);
				await _context.SaveChangesAsync();
				MessageBox.Show("Комментарий добавлен");
				ReviewsControl.ItemsSource = Product.Reviews.ToList();
			}
			catch { MessageBox.Show("Ошибка в публикации комментария"); }
		}
	}
}
