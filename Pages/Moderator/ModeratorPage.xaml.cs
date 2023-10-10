﻿using MarketplaceDesktop.Models;
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

namespace MarketplaceDesktop.Pages.Moderator
{
	/// <summary>
	/// Логика взаимодействия для ModeratorPage.xaml
	/// </summary>
	public partial class ModeratorPage : Window
	{
		User _user;
		public ModeratorPage(User user)
		{
			InitializeComponent();
			_user = user;
		}
	}
}
