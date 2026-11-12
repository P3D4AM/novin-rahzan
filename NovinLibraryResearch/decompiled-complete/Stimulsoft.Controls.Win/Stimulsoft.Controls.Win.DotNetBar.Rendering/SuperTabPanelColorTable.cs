using System;
using System.ComponentModel;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

[TypeConverter(typeof(SuperTabPanelColorTableConvertor))]
public class SuperTabPanelColorTable : ICloneable
{
	private SuperTabPanelItemColorTable _Default;

	private SuperTabPanelItemColorTable _Left;

	private SuperTabPanelItemColorTable _Bottom;

	private SuperTabPanelItemColorTable _Right;

	[Browsable(true)]
	[NotifyParentProperty(true)]
	[Description("Indicates the Default tab panel color settings.")]
	public SuperTabPanelItemColorTable Default
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
	[Description("Indicates the Left Aligned tab panel color settings.")]
	public SuperTabPanelItemColorTable Left
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
	[Description("Indicates the Bottom Aligned tab panel color settings.")]
	public SuperTabPanelItemColorTable Bottom
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
	[Description("Indicates the Right Aligned tab panel color settings.")]
	public SuperTabPanelItemColorTable Right
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

	[Description("Event raised when the SuperTabPanelColorTable is changed")]
	public event EventHandler<EventArgs> ColorTableChanged;

	public SuperTabPanelColorTable()
	{
		_Default = new SuperTabPanelItemColorTable();
		_Default.ColorTableChanged += PctColorTableChanged;
		_Left = new SuperTabPanelItemColorTable();
		_Left.ColorTableChanged += PctColorTableChanged;
		_Bottom = new SuperTabPanelItemColorTable();
		_Bottom.ColorTableChanged += PctColorTableChanged;
		_Right = new SuperTabPanelItemColorTable();
		_Right.ColorTableChanged += PctColorTableChanged;
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
		Default = new SuperTabPanelItemColorTable();
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
		Left = new SuperTabPanelItemColorTable();
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
		Bottom = new SuperTabPanelItemColorTable();
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
		Right = new SuperTabPanelItemColorTable();
	}

	private void SetNewColorTable(ref SuperTabPanelItemColorTable sct, SuperTabPanelItemColorTable newSct)
	{
		if (sct != null)
		{
			sct.ColorTableChanged -= PctColorTableChanged;
		}
		sct = newSct;
		if (sct != null)
		{
			sct.ColorTableChanged += PctColorTableChanged;
		}
		OnColorTableChanged();
	}

	private void PctColorTableChanged(object sender, EventArgs e)
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
		SuperTabPanelColorTable superTabPanelColorTable = new SuperTabPanelColorTable();
		superTabPanelColorTable.Default = (SuperTabPanelItemColorTable)Default.Clone();
		superTabPanelColorTable.Left = (SuperTabPanelItemColorTable)Left.Clone();
		superTabPanelColorTable.Bottom = (SuperTabPanelItemColorTable)Bottom.Clone();
		superTabPanelColorTable.Right = (SuperTabPanelItemColorTable)Right.Clone();
		return superTabPanelColorTable;
	}
}
