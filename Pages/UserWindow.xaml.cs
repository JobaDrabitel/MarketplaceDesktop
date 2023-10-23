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
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
		Marketplace1Context _context = new Marketplace1Context();
		public User User { get; set; }

		public UserWindow(User user)
		{
			InitializeComponent();
			User = user;
			DataContext = User;
			var roles = _context.Roles.Select(r => r.RoleName).ToList();
			RoleComboBox.ItemsSource = roles;
		}

		private async void Save_Click(object sender, RoutedEventArgs e)
		{
			var roleName = RoleComboBox.SelectedItem as string;
			var role = _context.Roles.FirstOrDefault(r => r.RoleName == roleName);
			role.Users.Add(User);
			_context.Users.Add(User);
			await _context.SaveChangesAsync();
			DialogResult = true;
		}

		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}
	}
}
