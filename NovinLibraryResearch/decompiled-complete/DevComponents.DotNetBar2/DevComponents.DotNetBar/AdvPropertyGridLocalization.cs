using System;
using System.ComponentModel;
using System.Threading;

namespace DevComponents.DotNetBar;

[TypeConverter(typeof(ExpandableObjectConverter))]
[ToolboxItem(false)]
public class AdvPropertyGridLocalization : INotifyPropertyChanged
{
	private string __2E27 = "Categorized";

	private string __2E28 = "Alphabetical";

	private string __2E29 = "Error setting the value. ";

	private string __2E2A = "Quick Search";

	private PropertyChangedEventHandler চ;

	[Description("Tooltip used by Categorize toolbar button")]
	[DefaultValue("Categorized")]
	[Localizable(true)]
	public string CategorizeToolbarTooltip
	{
		get
		{
			return __2E27;
		}
		set
		{
			if (value != __2E27)
			{
				string ك = __2E27;
				__2E27 = value;
				_2E2B(ك, value);
			}
		}
	}

	[DefaultValue("Alphabetical")]
	[Localizable(true)]
	[Description("Tooltip used by Alphabetical toolbar button")]
	public string AlphabeticalToolbarTooltip
	{
		get
		{
			return __2E28;
		}
		set
		{
			if (value != __2E28)
			{
				string ك = __2E28;
				__2E28 = value;
				_2E2C(ك, value);
			}
		}
	}

	[DefaultValue("Error setting the value. ")]
	[Localizable(true)]
	[Description("")]
	public string ErrorSettingPropertyValueTooltip
	{
		get
		{
			return __2E29;
		}
		set
		{
			if (value != __2E29)
			{
				string ك = __2E29;
				__2E29 = value;
				_2E2D(ك, value);
			}
		}
	}

	[DefaultValue("Quick Search")]
	[Localizable(true)]
	[Description("Indicates watermark text displayed in search text-box.")]
	public string SearchBoxWatermarkText
	{
		get
		{
			return __2E2A;
		}
		set
		{
			if (value != __2E2A)
			{
				string ك = __2E2A;
				__2E2A = value;
				_2E2E(ك, value);
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged
	{
		add
		{
			PropertyChangedEventHandler propertyChangedEventHandler = চ;
			PropertyChangedEventHandler propertyChangedEventHandler2;
			do
			{
				propertyChangedEventHandler2 = propertyChangedEventHandler;
				PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Combine(propertyChangedEventHandler2, value);
				propertyChangedEventHandler = Interlocked.CompareExchange(ref চ, value2, propertyChangedEventHandler2);
			}
			while ((object)propertyChangedEventHandler != propertyChangedEventHandler2);
		}
		remove
		{
			PropertyChangedEventHandler propertyChangedEventHandler = চ;
			PropertyChangedEventHandler propertyChangedEventHandler2;
			do
			{
				propertyChangedEventHandler2 = propertyChangedEventHandler;
				PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Remove(propertyChangedEventHandler2, value);
				propertyChangedEventHandler = Interlocked.CompareExchange(ref চ, value2, propertyChangedEventHandler2);
			}
			while ((object)propertyChangedEventHandler != propertyChangedEventHandler2);
		}
	}

	private void _2E2B(string ك, string ل)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("CategorizeToolbarTooltip"));
	}

	private void _2E2C(string ك, string ل)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("AlphabeticalToolbarTooltip"));
	}

	private void _2E2D(string ك, string ل)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("ErrorSettingPropertyValueTooltip"));
	}

	private void _2E2E(string ك, string ل)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("SearchBoxWatermarkText"));
	}

	protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		চ?.Invoke(this, e);
	}
}
