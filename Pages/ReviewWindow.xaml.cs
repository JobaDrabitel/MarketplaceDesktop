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

namespace MarketplaceDesktop.Pages
{
	/// <summary>
	/// Логика взаимодействия для ReviewWindow.xaml
	/// </summary>
	public partial class ReviewWindow : Window
	{
		public Review Review { get; set; }

		public ReviewWindow(Review review)
		{
			InitializeComponent();
			Review = review;
			DataContext = Review;
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}

		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}
	}
}
