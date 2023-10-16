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
using System.Windows.Shapes;

namespace MarketplaceDesktop.Pages
{
	/// <summary>
	/// Логика взаимодействия для RegWindow.xaml
	/// </summary>
	public partial class RegWindow : Window
	{
		Marketplace1Context _context = new();
		public RegWindow()
		{
			InitializeComponent();
		}
		private void AuthButton_Click(object sender, RoutedEventArgs e)
		{
			AuthWindow authWindow = new AuthWindow();
			authWindow.Show();
			this.Close();
		}

		private async void RegButton_Click(object sender, RoutedEventArgs e)
		{
			UserController userController = new(_context);
			string firstName = txtFirstName.Text;
			string lastName = txtLastName.Text;
			string email = txtEmail.Text;
			string password = txtPassword.Password;
			string passwordConfirm = txtPasswordConfirm.Password;
			string phoneNumber = txtPhone.Text;
			string imageUrl = txtURL.Text; 

			// Проверка введенных данных
			if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordConfirm) || string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(imageUrl))
			{
				MessageBox.Show("Все поля должны быть заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			// Проверка на правильный Email (можно использовать регулярные выражения)
			if (!IsValidEmail(email))
			{
				MessageBox.Show("Email указан неверно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			// Проверка на совпадение паролей
			if (password != passwordConfirm)
			{
				MessageBox.Show("Пароли не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			// Проверка номера телефона (11 цифр)
			if (phoneNumber.Length != 11 || !phoneNumber.All(char.IsDigit))
			{
				MessageBox.Show("Номер телефона должен состоять из 11 цифр.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			User user = new User()
			{
				FirstName = firstName,
				LastName = lastName,
				Email = email,
				PasswordHash = password,
				Phone = phoneNumber,
				ImageUrl = imageUrl,
				Roles = new List<Role>() { await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "User") }
			};
			user = await userController.PostUser(user);
			if (user == null) { MessageBox.Show("Произошла ошибка регистрации"); }
			else
			{
				MainWindow mainWindow = new MainWindow(user);
				mainWindow.Show();
				this.Close();
			}
		}

		// Метод для проверки правильности Email с использованием регулярного выражения
		private bool IsValidEmail(string email)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}


	}
}
