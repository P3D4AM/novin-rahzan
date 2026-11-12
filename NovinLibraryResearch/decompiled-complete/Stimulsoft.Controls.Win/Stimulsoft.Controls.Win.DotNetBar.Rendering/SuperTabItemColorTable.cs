using System;
using System.ComponentModel;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

[TypeConverter(typeof(SuperTabItemColorTableConvertor))]
public class SuperTabItemColorTable : ICloneable, IDisposable
{
	private SuperTabColorStates _Default;

	private SuperTabColorStates _Left;

	private SuperTabColorStates _Bottom;

	private SuperTabColorStates _Right;

	[Browsable(true)]
	[NotifyParentProperty(true)]
	[Description("Indicates the Default tab color settings.")]
	public SuperTabColorStates Default
	{
		get
		{
			return _Default;
		}
		set
		{
			SetNewColorTable(ref _Default, value);
		}
	}

	[Browsable(true)]
	[NotifyParentProperty(true)]
	[Description("Indicates the Left Aligned tab color settings.")]
	public SuperTabColorStates Left
	{
		get
		{
			return _Left;
		}
		set
		{
			SetNewColorTable(ref _Left, value);
		}
	}

	[Browsable(true)]
	[NotifyParentProperty(true)]
	[Description("Indicates the Bottom Aligned tab color settings.")]
	public SuperTabColorStates Bottom
	{
		get
		{
			return _Bottom;
		}
		set
		{
			SetNewColorTable(ref _Bottom, value);
		}
	}

	[Browsable(true)]
	[NotifyParentProperty(true)]
	[Description("Indicates the Right Aligned tab color settings.")]
	public SuperTabColorStates Right
	{
		get
		{
			return _Right;
		}
		set
		{
			SetNewColorTable(ref _Right, value);
		}
	}

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if (_Default.IsEmpty && _Left.IsEmpty && _Bottom.IsEmpty)
			{
				return _Right.IsEmpty;
			}
			return false;
		}
	}

	[Description("Event raised when the SuperTabItemColorTable is changed")]
	public event EventHandler<EventArgs> ColorTableChanged;

	public SuperTabItemColorTable()
	{
		_Default = new SuperTabColorStates();
		_Default.ColorTableChanged += SctColorTableChanged;
		_Left = new SuperTabColorStates();
		_Left.ColorTableChanged += SctColorTableChanged;
		_Bottom = new SuperTabColorStates();
		_Bottom.ColorTableChanged += SctColorTableChanged;
		_Right = new SuperTabColorStates();
		_Right.ColorTableChanged += SctColorTableChanged;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDefault()
	{
		return !_Default.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDefault()
	{
		Default = new SuperTabColorStates();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeLeft()
	{
		return !_Left.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetLeft()
	{
		Left = new SuperTabColorStates();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBottom()
	{
		return !_Bottom.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBottom()
	{
		Bottom = new SuperTabColorStates();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeRight()
	{
		return !_Right.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetRight()
	{
		Right = new SuperTabColorStates();
	}

	private void SetNewColorTable(ref SuperTabColorStates sct, SuperTabColorStates newSct)
	{
		if (sct != null)
		{
			sct.ColorTableChanged -= SctColorTableChanged;
		}
		sct = newSct;
		if (sct != null)
		{
			sct.ColorTableChanged += SctColorTableChanged;
		}
		OnColorTableChanged();
	}

	private void SctColorTableChanged(object sender, EventArgs e)
	{
		OnColorTableChanged();
	}

	private void OnColorTableChanged()
	{
		if (this.ColorTableChanged != null)
		{
			this.ColorTableChanged(this, EventArgs.Empty);
		}
	}

	public object Clone()
	{
		SuperTabItemColorTable superTabItemColorTable = new SuperTabItemColorTable();
		superTabItemColorTable.Default = (SuperTabColorStates)Default.Clone();
		superTabItemColorTable.Left = (SuperTabColorStates)Left.Clone();
		superTabItemColorTable.Bottom = (SuperTabColorStates)Bottom.Clone();
		superTabItemColorTable.Right = (SuperTabColorStates)Right.Clone();
		return superTabItemColorTable;
	}

	public void Dispose()
	{
		Default = null;
		Left = null;
		Bottom = null;
		Right = null;
	}
}
