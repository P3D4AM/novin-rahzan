using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

public class StepItemContainer : ImageItem, IDesignTimeProvider
{
	private _249A ᅲ;

	private int __249C = 10;

	private int __249D;

	private int __21E7;

	[Category("Appearance")]
	[DefaultValue(10)]
	[Description("Gets or sets the arrow pointer width for the StepItem objects hosted within this container.")]
	public int PointerSize
	{
		get
		{
			return __249C;
		}
		set
		{
			if (value != __249C)
			{
				int oldValue = __249C;
				__249C = value;
				OnPointerSizeChanged(oldValue, value);
			}
		}
	}

	internal int _249B
	{
		get
		{
			return __249D;
		}
		set
		{
			__249D = value;
		}
	}

	internal int _21E4
	{
		get
		{
			return __21E7;
		}
		set
		{
			__21E7 = value;
		}
	}

	public StepItemContainer()
	{
		m_IsContainer = true;
		AutoCollapseOnClick = true;
		AccessibleRole = (AccessibleRole)20;
	}

	public override BaseItem Copy()
	{
		StepItemContainer stepItemContainer = new StepItemContainer();
		stepItemContainer.Name = Name;
		CopyToItem(stepItemContainer);
		return stepItemContainer;
	}

	internal void _0EBA(StepItemContainer _0EBB)
	{
		CopyToItem(_0EBB);
	}

	protected override void CopyToItem(BaseItem copy)
	{
		StepItemContainer objCopy = copy as StepItemContainer;
		base.CopyToItem(objCopy);
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
	}

	public override void RecalcSize()
	{
		bool _249E = true;
		Point location = Bounds.Location;
		Size empty = Size.Empty;
		bool flag = true;
		SubItemsCollection subItems = SubItems;
		bool _249F = true;
		int num = subItems.Count - 1;
		for (int num2 = num; num2 >= 0; num2--)
		{
			if (subItems[num2] is StepItem { Visible: not false } stepItem)
			{
				stepItem._249F = _249F;
				_249F = false;
			}
		}
		foreach (BaseItem item in subItems)
		{
			item.DesignMarkerOrientation = eDesignMarkerOrientation.Horizontal;
			StepItem stepItem2 = item as StepItem;
			if (!item.Visible)
			{
				item.Displayed = false;
				if (stepItem2 != null)
				{
					stepItem2._249F = false;
				}
				continue;
			}
			item.Displayed = true;
			if (stepItem2 != null)
			{
				stepItem2._249E = _249E;
				_249E = false;
				stepItem2.RecalcSize();
				stepItem2.SetDisplayRectangle(new Rectangle(location, stepItem2.Size));
				int num3 = stepItem2.Size.Width - __249C - 1;
				location.X += num3;
				empty.Width += num3;
			}
			else
			{
				_249E = true;
				item.RecalcSize();
				item.SetDisplayRectangle(new Rectangle(location, item.Size));
				location.X += item.WidthInternal;
				empty.Width += item.WidthInternal;
			}
			if (flag && empty.Height > 0 && empty.Height != item.HeightInternal)
			{
				flag = false;
			}
			empty.Height = Math.Max(empty.Height, item.HeightInternal);
		}
		empty.Width += __249C;
		if (!flag)
		{
			foreach (BaseItem item2 in subItems)
			{
				if (item2.Visible && item2.HeightInternal < empty.Height)
				{
					item2.TopInternal = (empty.Height - item2.HeightInternal) / 2;
				}
			}
		}
		m_Rect.Size = empty;
		__21E7 = empty.Height;
		__249D = empty.Width;
		base.RecalcSize();
	}

	public override void Paint(ItemPaintArgs p)
	{
		_249A _249A2 = ᆆ();
		_249A2._139A(this, p);
	}

	internal _249A ᆆ()
	{
		if (ᅲ == null)
		{
			ᅲ = new _249A();
		}
		return ᅲ;
	}

	protected virtual void OnPointerSizeChanged(int oldValue, int newValue)
	{
	}

	protected virtual InsertPosition GetContainerInsertPosition(Point pScreen, BaseItem dragItem)
	{
		return DesignTimeProviderContainer.GetInsertPosition(this, pScreen, dragItem);
	}

	private InsertPosition ᆉ(Point ॺ, BaseItem ᆊ)
	{
		return GetContainerInsertPosition(ॺ, ᆊ);
	}

	InsertPosition IDesignTimeProvider.GetInsertPosition(Point ॺ, BaseItem ᆊ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᆉ
		return this.ᆉ(ॺ, ᆊ);
	}

	private void ᆋ(int ᆌ, bool ᆍ)
	{
		DesignTimeProviderContainer.DrawReversibleMarker(this, ᆌ, ᆍ);
	}

	void IDesignTimeProvider.DrawReversibleMarker(int ᆌ, bool ᆍ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᆋ
		this.ᆋ(ᆌ, ᆍ);
	}

	private void ᆎ(BaseItem ਤ, int ᆌ, bool ᆍ)
	{
		DesignTimeProviderContainer.InsertItemAt(this, ਤ, ᆌ, ᆍ);
	}

	void IDesignTimeProvider.InsertItemAt(BaseItem ਤ, int ᆌ, bool ᆍ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᆎ
		this.ᆎ(ਤ, ᆌ, ᆍ);
	}
}
