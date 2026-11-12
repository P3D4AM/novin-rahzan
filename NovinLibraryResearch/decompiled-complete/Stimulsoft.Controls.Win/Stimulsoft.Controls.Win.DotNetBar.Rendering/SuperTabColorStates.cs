using System;
using System.ComponentModel;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

[TypeConverter(typeof(SuperTabItemColorTableConvertor))]
public class SuperTabColorStates
{
	private SuperTabItemStateColorTable _Normal;

	private SuperTabItemStateColorTable _Selected;

	private SuperTabItemStateColorTable _MouseOver;

	private SuperTabItemStateColorTable _SelectedMouseOver;

	private SuperTabItemStateColorTable _Disabled;

	[Browsable(true)]
	[NotifyParentProperty(true)]
	[Description("Indicates the tab colors when the tab is not selected, and the mouse is not over it.")]
	public SuperTabItemStateColorTable Normal
	{
		get
		{
			return _Normal;
		}
		set
		{
			SetNewColorTable(ref _Normal, value);
		}
	}

	[Browsable(true)]
	[NotifyParentProperty(true)]
	[Description("Indicates the tab colors when the tab is selected, but the mouse is not over the it.")]
	public SuperTabItemStateColorTable Selected
	{
		get
		{
			return _Selected;
		}
		set
		{
			SetNewColorTable(ref _Selected, value);
		}
	}

	[Browsable(true)]
	[NotifyParentProperty(true)]
	[Description("Indicates the tab colors when the tab is not selected, but the mouse is over it.")]
	public SuperTabItemStateColorTable MouseOver
	{
		get
		{
			return _MouseOver;
		}
		set
		{
			SetNewColorTable(ref _MouseOver, value);
		}
	}

	[Browsable(true)]
	[NotifyParentProperty(true)]
	[Description("Indicates the tab colors when the tab is selected, and the mouse is over it.")]
	public SuperTabItemStateColorTable SelectedMouseOver
	{
		get
		{
			return _SelectedMouseOver;
		}
		set
		{
			SetNewColorTable(ref _SelectedMouseOver, value);
		}
	}

	[Browsable(true)]
	[NotifyParentProperty(true)]
	[Description("Indicates the tab colors when the tab is disabled, is not selected, and the mouse is not over it.")]
	public SuperTabItemStateColorTable Disabled
	{
		get
		{
			return _Disabled;
		}
		set
		{
			SetNewColorTable(ref _Disabled, value);
		}
	}

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if (!_Normal.IsEmpty)
			{
				return false;
			}
			if (!_Selected.IsEmpty)
			{
				return false;
			}
			if (!_MouseOver.IsEmpty)
			{
				return false;
			}
			if (!_SelectedMouseOver.IsEmpty)
			{
				return false;
			}
			if (!_Disabled.IsEmpty)
			{
				return false;
			}
			return true;
		}
	}

	[Description("Event raised when the SuperTabColorStates is changed")]
	public event EventHandler<EventArgs> ColorTableChanged;

	public SuperTabColorStates()
	{
		_Normal = new SuperTabItemStateColorTable();
		_Normal.ColorTableChanged += SctColorTableChanged;
		_Selected = new SuperTabItemStateColorTable();
		_Selected.ColorTableChanged += SctColorTableChanged;
		_MouseOver = new SuperTabItemStateColorTable();
		_MouseOver.ColorTableChanged += SctColorTableChanged;
		_SelectedMouseOver = new SuperTabItemStateColorTable();
		_SelectedMouseOver.ColorTableChanged += SctColorTableChanged;
		_Disabled = new SuperTabItemStateColorTable();
		_Disabled.ColorTableChanged += SctColorTableChanged;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeNormal()
	{
		return !_Normal.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetNormal()
	{
		Normal = new SuperTabItemStateColorTable();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSelected()
	{
		return !_Selected.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelected()
	{
		Selected = new SuperTabItemStateColorTable();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMouseOver()
	{
		return !_MouseOver.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetMouseOver()
	{
		MouseOver = new SuperTabItemStateColorTable();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSelectedMouseOver()
	{
		return !_SelectedMouseOver.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectedMouseOver()
	{
		SelectedMouseOver = new SuperTabItemStateColorTable();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDisabled()
	{
		return !_Disabled.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDisabled()
	{
		Disabled = new SuperTabItemStateColorTable();
	}

	private void SetNewColorTable(ref SuperTabItemStateColorTable sct, SuperTabItemStateColorTable newSct)
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
		SuperTabColorStates superTabColorStates = new SuperTabColorStates();
		superTabColorStates.Normal = (SuperTabItemStateColorTable)Normal.Clone();
		superTabColorStates.Selected = (SuperTabItemStateColorTable)Selected.Clone();
		superTabColorStates.MouseOver = (SuperTabItemStateColorTable)MouseOver.Clone();
		superTabColorStates.SelectedMouseOver = (SuperTabItemStateColorTable)SelectedMouseOver.Clone();
		superTabColorStates.Disabled = (SuperTabItemStateColorTable)Disabled.Clone();
		return superTabColorStates;
	}

	public void Dispose()
	{
		Normal = null;
		MouseOver = null;
		SelectedMouseOver = null;
		Selected = null;
		Disabled = null;
	}
}
