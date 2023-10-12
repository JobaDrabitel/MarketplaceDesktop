using MarketplaceDesktop.Models;
using MarketplaceDesktop.Pages.Moderator;
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
	/// Логика взаимодействия для AuthWindow.xaml
	/// </summary>
	public partial class AuthWindow : Window
	{
		Marketplace1Context _context = new Marketplace1Context();
		User User { get; set; }
		public AuthWindow()
		{
			InitializeComponent();
		}

		private void AuthButton_Click(object sender, RoutedEventArgs e)
		{
			string enteredEmail = txtEmail.Text;
			string enteredPassword = txtPassword.Password;

			if (string.IsNullOrWhiteSpace(enteredEmail) || !IsEmailValid(enteredEmail))
			{
				MessageBox.Show("Пожалуйста, введите правильный email.");
				return; 
			}

			if (string.IsNullOrWhiteSpace(enteredPassword))
			{
				MessageBox.Show("Пожалуйста, введите пароль.");
				return; 
			}
			if (IsUserAuthenticated(enteredEmail, enteredPassword))
			{
				switch (User.Roles.First().RoleId)
				{
					case 1: 
						{
							MainWindow main = new MainWindow(User);
							main.Show();
							this.Close();
							break;
						}
					case 2:
						{
							ModeratorPage moder = new (User);
							moder.Show();
							this.Close();
							break;
						}
					case 3:
						{
							AdminPage admin = new AdminPage(User); 
							admin.Show();
							this.Close();
							break;
						}
				}
				
			}
			else
			{
				MessageBox.Show("Неверный email или пароль. Попробуйте еще раз.");
			}
		}

		private bool IsEmailValid(string email)
		{
			return System.Text.RegularExpressions.Regex.IsMatch(
				email,
				@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$"
			);
		}
		private bool IsUserAuthenticated(string email, string password)
		{
				User = _context.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);

				if (User != null)
				{
					return true;
				}
				return false;
			

		}

		private void RegButton_Click(object sender, RoutedEventArgs e)
		{
			RegWindow regWindow = new RegWindow();
			regWindow.Show();
			Close();
        }
    }
}
