using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Animation;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.Editors.DateTimeAdv;

internal class YearSelectorContainer : ItemContainer
{
	private Size _FixedButtonSize = new Size(13, 18);

	private ItemContainer _YearsContainer;

	private LabelItem _CenturyLabel;

	private Color _TextColor = Color.Empty;

	private int _CenturyYearStart;

	private int _SelectedYear;

	private CenturySelectorControl _CenturySelector;

	public Color TextColor
	{
		get
		{
			return _TextColor;
		}
		set
		{
			_TextColor = value;
			OnTextColorChanged();
		}
	}

	public int CenturyYearStart
	{
		get
		{
			return _CenturyYearStart;
		}
		set
		{
			_CenturyYearStart = value;
			UpdateYears();
		}
	}

	public int SelectedYear
	{
		get
		{
			return _SelectedYear;
		}
		set
		{
			_SelectedYear = value;
			foreach (BaseItem subItem in _YearsContainer.SubItems)
			{
				if (subItem.Name.StartsWith("yearSelector"))
				{
					ButtonItem buttonItem = (ButtonItem)subItem;
					if (buttonItem.Text == _SelectedYear.ToString())
					{
						buttonItem.Checked = true;
						break;
					}
				}
			}
		}
	}

	public event EventHandler SelectedYearChanged;

	public YearSelectorContainer()
	{
		LayoutOrientation = eOrientation.Vertical;
		ItemContainer itemContainer = new ItemContainer
		{
			AutoCollapseOnClick = false,
			MinimumSize = new Size(0, _FixedButtonSize.Height + 3),
			LayoutOrientation = eOrientation.Horizontal,
			HorizontalItemAlignment = eHorizontalItemsAlignment.Center,
			VerticalItemAlignment = eVerticalItemsAlignment.Middle,
			ItemSpacing = 2
		};
		ButtonItem buttonItem = new ButtonItem("NavDecreaseYears");
		buttonItem.GlobalItem = false;
		buttonItem._FadeEnabled = false;
		buttonItem.Text = "<expand direction=\"left\"/>";
		buttonItem.Click += YearsDecreaseClick;
		buttonItem.AutoCollapseOnClick = false;
		buttonItem.FixedSize = _FixedButtonSize;
		buttonItem._FixedSizeCenterText = true;
		buttonItem.ClickAutoRepeat = true;
		itemContainer.SubItems.Add(buttonItem);
		LabelItem labelItem = new LabelItem("CenturyLabel");
		labelItem.AutoCollapseOnClick = false;
		labelItem.GlobalItem = false;
		labelItem.TextAlignment = (StringAlignment)1;
		labelItem.TextLineAlignment = (StringAlignment)1;
		labelItem.PaddingBottom = 2;
		labelItem.Text = "2010-2019";
		labelItem.Click += ShowCenturySelector;
		labelItem.Cursor = Cursors.Hand;
		_CenturyLabel = labelItem;
		itemContainer.SubItems.Add(labelItem);
		buttonItem = new ButtonItem("NavIncreaseYears");
		buttonItem.GlobalItem = false;
		buttonItem._FadeEnabled = false;
		buttonItem.Text = "<expand direction=\"right\"/>";
		buttonItem.Click += YearsIncreaseClick;
		buttonItem.AutoCollapseOnClick = false;
		buttonItem.FixedSize = _FixedButtonSize;
		buttonItem._FixedSizeCenterText = true;
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
			buttonItem2._FixedSizeCenterText = true;
			buttonItem2.OptionGroup = "year";
			buttonItem2.AutoCheckOnClick = true;
			buttonItem2.Click += YearClick;
			itemContainer.SubItems.Add(buttonItem2);
		}
		_YearsContainer = itemContainer;
		CenturyYearStart = DateTime.Today.Year / 10 * 10;
	}

	private void YearClick(object sender, EventArgs e)
	{
		ButtonItem buttonItem = (ButtonItem)sender;
		int selectedYear = int.Parse(buttonItem.Text);
		SelectedYear = selectedYear;
		OnSelectedYearChanged(EventArgs.Empty);
	}

	protected virtual void OnSelectedYearChanged(EventArgs e)
	{
		this.SelectedYearChanged?.Invoke(this, e);
	}

	private void OnTextColorChanged()
	{
		SetTextColor(SubItems);
	}

	private void SetTextColor(SubItemsCollection subItems)
	{
		foreach (BaseItem subItem in subItems)
		{
			if (subItem is ButtonItem)
			{
				((ButtonItem)subItem).ForeColor = _TextColor;
				((ButtonItem)subItem).HotForeColor = _TextColor;
			}
			else if (subItem is LabelItem)
			{
				((LabelItem)subItem).ForeColor = _TextColor;
			}
			else if (subItem is ItemContainer)
			{
				SetTextColor(subItem.SubItems);
			}
		}
	}

	private void UpdateYears()
	{
		int num = _CenturyYearStart - 1;
		foreach (BaseItem subItem in _YearsContainer.SubItems)
		{
			if (subItem.Name.StartsWith("yearSelector"))
			{
				ButtonItem buttonItem = (ButtonItem)subItem;
				buttonItem.Text = num.ToString();
				if (num == _SelectedYear)
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
		_CenturyLabel.Text = $"{_CenturyYearStart}-{_CenturyYearStart + 10}";
		NeedRecalcSize = true;
		Refresh();
	}

	protected override void OnFixedSizeChanged(Size oldValue, Size newValue)
	{
		base.OnFixedSizeChanged(oldValue, newValue);
		Size size = newValue;
		size.Height -= _FixedButtonSize.Height + 4;
		size.Width -= 4;
		Size fixedSize = new Size(size.Width / 4, size.Height / 3);
		foreach (BaseItem subItem in _YearsContainer.SubItems)
		{
			if (subItem.Name.StartsWith("yearSelector"))
			{
				ButtonItem buttonItem = (ButtonItem)subItem;
				buttonItem.FixedSize = fixedSize;
			}
		}
	}

	private void YearsDecreaseClick(object sender, EventArgs e)
	{
		if (CenturyYearStart - 10 > 1760)
		{
			CenturyYearStart -= 10;
		}
	}

	private void YearsIncreaseClick(object sender, EventArgs e)
	{
		CenturyYearStart += 10;
	}

	private void ShowCenturySelector(object sender, EventArgs e)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		if (_CenturySelector == null)
		{
			_CenturySelector = new CenturySelectorControl();
			Control val = (Control)ContainerControl;
			((Control)_CenturySelector).Size = new Size(8, 8);
			((Control)_CenturySelector).Location = new Point(LeftInternal + (WidthInternal - 8) / 2, TopInternal + (HeightInternal - 8) / 2);
			_CenturySelector.Style = eDotNetBarStyle.StyleManagerControlled;
			if (BarFunctions.IsOffice2007Style(Style))
			{
				Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
				ElementStyle elementStyle = (ElementStyle)colorTable.StyleClasses[ElementStyleClassKeys.ItemPanelKey];
				_CenturySelector.BackgroundStyle.BackColor = elementStyle.BackColor;
				_CenturySelector.BackgroundStyle.BackColor2 = elementStyle.BackColor2;
			}
			else
			{
				_CenturySelector.BackgroundStyle.BackColor = SystemColors.Window;
			}
			_CenturySelector.CenturySelector.TextColor = TextColor;
			val.Controls.Add((Control)(object)_CenturySelector);
			Rectangle bounds = Bounds;
			AnimationRectangle animationRectangle = new AnimationRectangle(new AnimationRequest(_CenturySelector, "Bounds", bounds), AnimationEasing.EaseInOutQuad, 200);
			animationRectangle.AutoDispose = true;
			animationRectangle.Start();
			_CenturySelector.CenturySelector.SelectedCenturyChanged += SelectedCenturyChanged;
		}
	}

	private void SelectedCenturyChanged(object sender, EventArgs e)
	{
		CenturyYearStart = _CenturySelector.CenturySelector.SelectedCentury;
		Rectangle bounds = Bounds;
		AnimationRectangle animationRectangle = new AnimationRectangle(new AnimationRequest(_CenturySelector, "Bounds", new Rectangle(LeftInternal + (WidthInternal - 8) / 2, TopInternal + (HeightInternal - 8) / 2, 8, 8)), AnimationEasing.EaseInOutQuad, 200);
		animationRectangle.AutoDispose = true;
		animationRectangle.AnimationCompleted += AnimationCompleted;
		animationRectangle.Start();
	}

	private void AnimationCompleted(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		Control val = (Control)ContainerControl;
		val.Controls.Remove((Control)(object)_CenturySelector);
		_CenturySelector.CenturySelector.SelectedCenturyChanged -= SelectedCenturyChanged;
		((Component)(object)_CenturySelector).Dispose();
		_CenturySelector = null;
	}
}
