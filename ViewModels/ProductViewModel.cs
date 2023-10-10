using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class ProductViewModel : INotifyPropertyChanged
{
	private int _productId;
	private string _name;
	private decimal _price;

	public int ProductId
	{
		get { return _productId; }
		set
		{
			if (_productId != value)
			{
				_productId = value;
				OnPropertyChanged();
			}
		}
	}

	public string Name
	{
		get { return _name; }
		set
		{
			if (_name != value)
			{
				_name = value;
				OnPropertyChanged();
			}
		}
	}

	public decimal Price
	{
		get { return _price; }
		set
		{
			if (_price != value)
			{
				_price = value;
				OnPropertyChanged();
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
