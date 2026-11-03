using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Animation;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.Editors.DateTimeAdv;

internal class _2611 : BaseItemControl
{
	private _2613 __25C7;

	public _2613 _2612 => __25C7;

	public _2611()
	{
		__25C7 = new _2613();
		HostItem = __25C7;
		FocusCuesEnabled = false;
	}

	protected override void OnResize(EventArgs e)
	{
		__25C7.FixedSize = Dpi.ঔ(((Control)this).Size);
		base.OnResize(e);
	}
}
internal class _2613 : ItemContainer
{
	private Size __2618 = new Size(13, 18);

	private ItemContainer __2619;

	private LabelItem __261A;

	private Color __0CD1 = Color.Empty;

	private int __261B;

	private int __261C;

	private _2628 __261D;

	public Color _2615
	{
		get
		{
			return __0CD1;
		}
		set
		{
			__0CD1 = value;
			_261F();
		}
	}

	public int _2616
	{
		get
		{
			return __261B;
		}
		set
		{
			__261B = value;
			_2622();
		}
	}

	public int _2617
	{
		get
		{
			return __261C;
		}
		set
		{
			__261C = value;
			foreach (BaseItem subItem in __2619.SubItems)
			{
				if (subItem.Name.StartsWith("yearSelector"))
				{
					ButtonItem buttonItem = (ButtonItem)subItem;
					if (buttonItem.Text == __261C.ToString())
					{
						buttonItem.Checked = true;
						break;
					}
					if (buttonItem.Checked)
					{
						buttonItem.Checked = false;
					}
				}
			}
		}
	}

	public event EventHandler _2614;

	public _2613()
	{
		LayoutOrientation = eOrientation.Vertical;
		ItemContainer itemContainer = new ItemContainer
		{
			Name = "yearSelectonContainerHeader",
			AutoCollapseOnClick = false,
			MinimumSize = new Size(0, __2618.Height + 3),
			LayoutOrientation = eOrientation.Horizontal,
			HorizontalItemAlignment = eHorizontalItemsAlignment.Center,
			VerticalItemAlignment = eVerticalItemsAlignment.Middle,
			ItemSpacing = 2
		};
		ButtonItem buttonItem = new ButtonItem("NavDecreaseYears");
		buttonItem.GlobalItem = false;
		buttonItem._ຳ = false;
		buttonItem.Text = "<expand direction=\"left\"/>";
		buttonItem.Click += _2623;
		buttonItem.AutoCollapseOnClick = false;
		buttonItem.FixedSize = __2618;
		buttonItem._ບ = true;
		buttonItem.ClickAutoRepeat = true;
		itemContainer.SubItems.Add(buttonItem);
		LabelItem labelItem = new LabelItem("CenturyLabel");
		labelItem.AutoCollapseOnClick = false;
		labelItem.GlobalItem = false;
		labelItem.TextAlignment = (StringAlignment)1;
		labelItem.TextLineAlignment = (StringAlignment)1;
		labelItem.PaddingBottom = 2;
		labelItem.Text = "2010-2019";
		labelItem.Click += _2625;
		labelItem.Cursor = Cursors.Hand;
		__261A = labelItem;
		itemContainer.SubItems.Add(labelItem);
		buttonItem = new ButtonItem("NavIncreaseYears");
		buttonItem.GlobalItem = false;
		buttonItem._ຳ = false;
		buttonItem.Text = "<expand direction=\"right\"/>";
		buttonItem.Click += _2624;
		buttonItem.AutoCollapseOnClick = false;
		buttonItem.FixedSize = __2618;
		buttonItem._ບ = true;
		buttonItem.ClickAutoRepeat = true;
		itemContainer.SubItems.Add(buttonItem);
		SubItems.Add(itemContainer);
		itemContainer = new ItemContainer
		{
			LayoutOrientation = eOrientation.Horizontal,
			MultiLine = true
		};
		SubItems.Add(itemContainer);
		for (int i = 0; i < 12; i++)
		{
			ButtonItem buttonItem2 = new ButtonItem("yearSelector" + i);
			buttonItem2.Text = (2009 + i).ToString();
			buttonItem2.FixedSize = new Size(54, 35);
			buttonItem2._ບ = true;
			buttonItem2.OptionGroup = "year";
			buttonItem2.AutoCheckOnClick = true;
			buttonItem2.AutoCollapseOnClick = false;
			buttonItem2.Click += _261E;
			itemContainer.SubItems.Add(buttonItem2);
		}
		__2619 = itemContainer;
		_2616 = DateTime.Today.Year / 10 * 10;
	}

	private void _261E(object _٠, EventArgs ؿ)
	{
		ButtonItem buttonItem = (ButtonItem)_٠;
		int num = int.Parse(buttonItem.Text);
		_2617 = num;
		OnSelectedYearChanged(EventArgs.Empty);
	}

	protected virtual void OnSelectedYearChanged(EventArgs e)
	{
		this._2614?.Invoke(this, e);
	}

	private void _261F()
	{
		_2620(SubItems);
	}

	private void _2620(SubItemsCollection _2621)
	{
		foreach (BaseItem item in _2621)
		{
			if (item is ButtonItem)
			{
				((ButtonItem)item).ForeColor = __0CD1;
				((ButtonItem)item).HotForeColor = __0CD1;
			}
			else if (item is LabelItem)
			{
				((LabelItem)item).ForeColor = __0CD1;
			}
			else if (item is ItemContainer)
			{
				_2620(item.SubItems);
			}
		}
	}

	private void _2622()
	{
		int num = __261B - 1;
		foreach (BaseItem subItem in __2619.SubItems)
		{
			if (subItem.Name.StartsWith("yearSelector"))
			{
				ButtonItem buttonItem = (ButtonItem)subItem;
				buttonItem.Text = num.ToString();
				if (num == __261C)
				{
					buttonItem.Checked = true;
				}
				else if (buttonItem.Checked)
				{
					buttonItem.Checked = false;
				}
				num++;
			}
		}
		__261A.Text = $"{__261B}-{__261B + 10}";
		NeedRecalcSize = true;
		Refresh();
	}

	protected override void OnFixedSizeChanged(Size oldValue, Size newValue)
	{
		base.OnFixedSizeChanged(oldValue, newValue);
		Size size = newValue;
		size.Height -= __2618.Height + 4;
		size.Width -= 4;
		Size fixedSize = new Size(size.Width / 4, size.Height / 3);
		foreach (BaseItem subItem in __2619.SubItems)
		{
			if (subItem.Name.StartsWith("yearSelector"))
			{
				ButtonItem buttonItem = (ButtonItem)subItem;
				buttonItem.FixedSize = fixedSize;
			}
		}
	}

	private void _2623(object _٠, EventArgs ؿ)
	{
		if (_2616 - 10 > 1760)
		{
			_2616 -= 10;
		}
	}

	private void _2624(object _٠, EventArgs ؿ)
	{
		_2616 += 10;
	}

	private void _2625(object _٠, EventArgs ؿ)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		if (__261D == null)
		{
			__261D = new _2628();
			Control val = (Control)ContainerControl;
			((Control)__261D).Size = new Size(8, 8);
			((Control)__261D).Location = new Point(LeftInternal + (WidthInternal - 8) / 2, TopInternal + (HeightInternal - 8) / 2);
			__261D.Style = eDotNetBarStyle.StyleManagerControlled;
			if (BarFunctions.IsOffice2007Style(Style))
			{
				Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
				ElementStyle elementStyle = (ElementStyle)colorTable.StyleClasses[ElementStyleClassKeys.ItemPanelKey];
				__261D.BackgroundStyle.BackColor = elementStyle.BackColor;
				__261D.BackgroundStyle.BackColor2 = elementStyle.BackColor2;
			}
			else
			{
				__261D.BackgroundStyle.BackColor = SystemColors.Window;
			}
			__261D._2629._2615 = _2615;
			val.Controls.Add((Control)(object)__261D);
			Rectangle bounds = Bounds;
			_20EE obj = new _20EE(new ခ(__261D, "Bounds", bounds), AnimationEasing.EaseInOutQuad, 200);
			obj.AutoDispose = true;
			obj.Start();
			__261D._2629._2626 += _2626;
		}
	}

	private void _2626(object _٠, EventArgs ؿ)
	{
		_2616 = __261D._2629._262B;
		_ = Bounds;
		_20EE obj = new _20EE(new ခ(__261D, "Bounds", new Rectangle(LeftInternal + (WidthInternal - 8) / 2, TopInternal + (HeightInternal - 8) / 2, 8, 8)), AnimationEasing.EaseInOutQuad, 200);
		obj.AutoDispose = true;
		obj.AnimationCompleted += ચ;
		obj.Start();
	}

	public void _2627()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		if (__261D != null)
		{
			Control val = (Control)ContainerControl;
			val.Controls.Remove((Control)(object)__261D);
			__261D._2629._2626 -= _2626;
			((Component)(object)__261D).Dispose();
			__261D = null;
		}
	}

	private void ચ(object _٠, EventArgs ؿ)
	{
		_2627();
	}
}
internal class _2628 : BaseItemControl
{
	private _262A __261D;

	public _262A _2629 => __261D;

	public _2628()
	{
		__261D = new _262A();
		HostItem = __261D;
		FocusCuesEnabled = false;
	}

	protected override void OnResize(EventArgs e)
	{
		__261D.FixedSize = Dpi.ঔ(((Control)this).Size);
		base.OnResize(e);
	}
}
internal class _262A : ItemContainer
{
	private Size __2618 = new Size(13, 18);

	private ItemContainer __2619;

	private LabelItem __261A;

	private Color __0CD1 = Color.Empty;

	private int __261B;

	private int __261C;

	public Color _2615
	{
		get
		{
			return __0CD1;
		}
		set
		{
			__0CD1 = value;
			_261F();
		}
	}

	public int _2616
	{
		get
		{
			return __261B;
		}
		set
		{
			__261B = value;
			_2622();
		}
	}

	public int _262B
	{
		get
		{
			return __261C;
		}
		set
		{
			__261C = value;
			foreach (BaseItem subItem in __2619.SubItems)
			{
				if (subItem.Name.StartsWith("centurySelector"))
				{
					ButtonItem buttonItem = (ButtonItem)subItem;
					if ((int)buttonItem.Tag == __261C)
					{
						buttonItem.Checked = true;
						break;
					}
				}
			}
			OnSelectedCenturyChanged(EventArgs.Empty);
		}
	}

	public event EventHandler _2626;

	public _262A()
	{
		LayoutOrientation = eOrientation.Vertical;
		ItemContainer itemContainer = new ItemContainer
		{
			AutoCollapseOnClick = false,
			MinimumSize = new Size(0, __2618.Height + 3),
			LayoutOrientation = eOrientation.Horizontal,
			HorizontalItemAlignment = eHorizontalItemsAlignment.Center,
			VerticalItemAlignment = eVerticalItemsAlignment.Middle,
			ItemSpacing = 2
		};
		ButtonItem buttonItem = new ButtonItem("NavDecreaseYears");
		buttonItem.GlobalItem = false;
		buttonItem._ຳ = false;
		buttonItem.Text = "<expand direction=\"left\"/>";
		buttonItem.Click += _2623;
		buttonItem.AutoCollapseOnClick = false;
		buttonItem.FixedSize = __2618;
		buttonItem._ບ = true;
		buttonItem.ClickAutoRepeat = true;
		itemContainer.SubItems.Add(buttonItem);
		LabelItem item = new LabelItem("CenturyLabel")
		{
			AutoCollapseOnClick = false,
			GlobalItem = false,
			TextAlignment = (StringAlignment)1,
			TextLineAlignment = (StringAlignment)1,
			PaddingBottom = 2,
			Text = "2000-2099"
		};
		__261A = item;
		itemContainer.SubItems.Add(item);
		buttonItem = new ButtonItem("NavIncreaseYears");
		buttonItem.GlobalItem = false;
		buttonItem._ຳ = false;
		buttonItem.Text = "<expand direction=\"right\"/>";
		buttonItem.Click += _2624;
		buttonItem.AutoCollapseOnClick = false;
		buttonItem.FixedSize = __2618;
		buttonItem._ບ = true;
		buttonItem.ClickAutoRepeat = true;
		itemContainer.SubItems.Add(buttonItem);
		SubItems.Add(itemContainer);
		itemContainer = new ItemContainer
		{
			LayoutOrientation = eOrientation.Horizontal,
			MultiLine = true
		};
		SubItems.Add(itemContainer);
		for (int i = 0; i < 12; i++)
		{
			ButtonItem buttonItem2 = new ButtonItem("centurySelector" + i);
			buttonItem2.Text = (2009 + i).ToString();
			buttonItem2.FixedSize = new Size(54, 35);
			buttonItem2._ບ = true;
			buttonItem2.OptionGroup = "century";
			buttonItem2.AutoCheckOnClick = true;
			buttonItem2.Click += _262C;
			itemContainer.SubItems.Add(buttonItem2);
		}
		__2619 = itemContainer;
		_2616 = DateTime.Today.Year / 100 * 100;
	}

	private void _262C(object _٠, EventArgs ؿ)
	{
		ButtonItem buttonItem = (ButtonItem)_٠;
		int num = (int)buttonItem.Tag;
		_262B = num;
	}

	protected virtual void OnSelectedCenturyChanged(EventArgs e)
	{
		this._2626?.Invoke(this, e);
	}

	private void _261F()
	{
		_2620(SubItems);
	}

	private void _2620(SubItemsCollection _2621)
	{
		foreach (BaseItem item in _2621)
		{
			if (item is ButtonItem)
			{
				((ButtonItem)item).ForeColor = __0CD1;
				((ButtonItem)item).HotForeColor = __0CD1;
			}
			else if (item is LabelItem)
			{
				((LabelItem)item).ForeColor = __0CD1;
			}
			else if (item is ItemContainer)
			{
				_2620(item.SubItems);
			}
		}
	}

	private void _2622()
	{
		int num = __261B - 10;
		foreach (BaseItem subItem in __2619.SubItems)
		{
			if (subItem.Name.StartsWith("centurySelector"))
			{
				ButtonItem buttonItem = (ButtonItem)subItem;
				buttonItem.Text = $"<div>{num}-<br/>{num + 9}</div>";
				buttonItem.Tag = num;
				if (num == __261C)
				{
					buttonItem.Checked = true;
				}
				else if (buttonItem.Checked)
				{
					buttonItem.Checked = false;
				}
				num += 10;
			}
		}
		__261A.Text = $"{__261B}-{__261B + 99}";
		NeedRecalcSize = true;
		Refresh();
	}

	protected override void OnFixedSizeChanged(Size oldValue, Size newValue)
	{
		base.OnFixedSizeChanged(oldValue, newValue);
		Size size = newValue;
		size.Height -= __2618.Height + 4;
		size.Width -= 4;
		Size fixedSize = new Size(size.Width / 4, size.Height / 3);
		foreach (BaseItem subItem in __2619.SubItems)
		{
			if (subItem.Name.StartsWith("centurySelector"))
			{
				ButtonItem buttonItem = (ButtonItem)subItem;
				buttonItem.FixedSize = fixedSize;
			}
		}
	}

	private void _2623(object _٠, EventArgs ؿ)
	{
		if (_2616 - 100 > 1760)
		{
			_2616 -= 100;
		}
	}

	private void _2624(object _٠, EventArgs ؿ)
	{
		_2616 += 100;
	}
}
internal class _262D : BaseItemControl
{
	private _262F __25C8;

	public _262F _262E => __25C8;

	public _262D()
	{
		__25C8 = new _262F();
		HostItem = __25C8;
		FocusCuesEnabled = false;
	}

	protected override void OnResize(EventArgs e)
	{
		__25C8.FixedSize = Dpi.ঔ(((Control)this).Size);
		base.OnResize(e);
	}
}
internal class _262F : ItemContainer
{
	private const string _2633 = "monthSelector";

	private Size __2618 = new Size(13, 18);

	private ItemContainer __2634;

	private Color __0CD1 = Color.Empty;

	private int __2635 = 1;

	private bool __2636;

	public bool ᮄ
	{
		get
		{
			foreach (BaseItem subItem in __2634.SubItems)
			{
				if (subItem is ButtonItem && subItem.Name.StartsWith("monthSelector") && ((ButtonItem)subItem).Checked)
				{
					return false;
				}
			}
			return true;
		}
		set
		{
			if (!value)
			{
				return;
			}
			__2635 = 1;
			foreach (BaseItem subItem in __2634.SubItems)
			{
				if (subItem is ButtonItem && subItem.Name.StartsWith("monthSelector") && ((ButtonItem)subItem).Checked)
				{
					((ButtonItem)subItem).Checked = false;
				}
			}
		}
	}

	public Color _2615
	{
		get
		{
			return __0CD1;
		}
		set
		{
			__0CD1 = value;
			_261F();
		}
	}

	public int _2631
	{
		get
		{
			return __2635;
		}
		set
		{
			if (value <= 0)
			{
				value = 1;
			}
			if (value > 12)
			{
				value = 12;
			}
			__2635 = value;
			ButtonItem buttonItem = (ButtonItem)__2634.SubItems["monthSelector" + __2635];
			if (buttonItem != null)
			{
				buttonItem.Checked = true;
			}
		}
	}

	public bool _2632
	{
		get
		{
			return __2636;
		}
		set
		{
			__2636 = value;
			_2638();
		}
	}

	public event EventHandler _2630;

	public _262F()
	{
		LayoutOrientation = eOrientation.Vertical;
		ItemContainer itemContainer = new ItemContainer
		{
			LayoutOrientation = eOrientation.Horizontal,
			MultiLine = true,
			AutoCollapseOnClick = false
		};
		SubItems.Add(itemContainer);
		for (int i = 1; i <= 12; i++)
		{
			ButtonItem buttonItem = new ButtonItem("monthSelector" + i);
			buttonItem.Text = i.ToString();
			buttonItem.Tag = i;
			buttonItem.FixedSize = new Size(54, 35);
			buttonItem._ບ = true;
			buttonItem.OptionGroup = "month";
			buttonItem.AutoCheckOnClick = true;
			buttonItem.Click += _2637;
			buttonItem.AutoCollapseOnClick = false;
			itemContainer.SubItems.Add(buttonItem);
		}
		__2634 = itemContainer;
	}

	private void _2637(object _٠, EventArgs ؿ)
	{
		ButtonItem buttonItem = (ButtonItem)_٠;
		int num = (int)buttonItem.Tag;
		_2631 = num;
		OnSelectedMonthChanged(EventArgs.Empty);
	}

	protected virtual void OnSelectedMonthChanged(EventArgs e)
	{
		this._2630?.Invoke(this, e);
	}

	private void _261F()
	{
		_2620(SubItems);
	}

	private void _2620(SubItemsCollection _2621)
	{
		foreach (BaseItem item in _2621)
		{
			if (item is ButtonItem)
			{
				((ButtonItem)item).ForeColor = __0CD1;
				((ButtonItem)item).HotForeColor = __0CD1;
			}
			else if (item is LabelItem)
			{
				((LabelItem)item).ForeColor = __0CD1;
			}
			else if (item is ItemContainer)
			{
				_2620(item.SubItems);
			}
		}
	}

	private void _2638()
	{
		string[] array = new string[0];
		if (__2636)
		{
			CultureInfo activeCulture = DateTimeInput.GetActiveCulture();
			array = activeCulture.DateTimeFormat.AbbreviatedMonthNames;
		}
		foreach (BaseItem subItem in __2634.SubItems)
		{
			if (subItem.Name.StartsWith("monthSelector"))
			{
				ButtonItem buttonItem = (ButtonItem)subItem;
				int num = (int)buttonItem.Tag;
				if (__2636)
				{
					buttonItem.Text = array[num - 1];
				}
				else
				{
					buttonItem.Text = num.ToString();
				}
			}
		}
		NeedRecalcSize = true;
		Refresh();
	}

	protected override void OnFixedSizeChanged(Size oldValue, Size newValue)
	{
		base.OnFixedSizeChanged(oldValue, newValue);
		Size size = newValue;
		size.Height -= __2618.Height + 4;
		size.Width -= 4;
		Size fixedSize = new Size(size.Width / 4, size.Height / 3);
		foreach (BaseItem subItem in __2634.SubItems)
		{
			if (subItem.Name.StartsWith("monthSelector"))
			{
				ButtonItem buttonItem = (ButtonItem)subItem;
				buttonItem.FixedSize = fixedSize;
			}
		}
	}
}
internal enum _2639
{
	ᘶ,
	_263A,
	_263B,
	_263C,
	_263D
}
