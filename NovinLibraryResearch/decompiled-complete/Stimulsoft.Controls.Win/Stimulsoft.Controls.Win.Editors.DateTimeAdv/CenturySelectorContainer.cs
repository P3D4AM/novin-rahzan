using System;
using System.Drawing;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.Editors.DateTimeAdv;

internal class CenturySelectorContainer : ItemContainer
{
	private Size _FixedButtonSize = new Size(13, 18);

	private ItemContainer _YearsContainer;

	private LabelItem _CenturyLabel;

	private Color _TextColor = Color.Empty;

	private int _CenturyYearStart;

	private int _SelectedYear;

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

	public int SelectedCentury
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
				if (subItem.Name.StartsWith("centurySelector"))
				{
					ButtonItem buttonItem = (ButtonItem)subItem;
					if ((int)buttonItem.Tag == _SelectedYear)
					{
						buttonItem.Checked = true;
						break;
					}
				}
			}
			OnSelectedCenturyChanged(EventArgs.Empty);
		}
	}

	public event EventHandler SelectedCenturyChanged;

	public CenturySelectorContainer()
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
		LabelItem labelItem = new LabelItem("CenturyLabel")
		{
			AutoCollapseOnClick = false,
			GlobalItem = false,
			TextAlignment = (StringAlignment)1,
			TextLineAlignment = (StringAlignment)1,
			PaddingBottom = 2,
			Text = "2000-2099"
		};
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
			ButtonItem buttonItem2 = new ButtonItem("centurySelector" + i);
			buttonItem2.Text = (2009 + i).ToString();
			buttonItem2.FixedSize = new Size(54, 35);
			buttonItem2._FixedSizeCenterText = true;
			buttonItem2.OptionGroup = "century";
			buttonItem2.AutoCheckOnClick = true;
			buttonItem2.Click += CenturyClick;
			itemContainer.SubItems.Add(buttonItem2);
		}
		_YearsContainer = itemContainer;
		CenturyYearStart = DateTime.Today.Year / 100 * 100;
	}

	private void CenturyClick(object sender, EventArgs e)
	{
		ButtonItem buttonItem = (ButtonItem)sender;
		int selectedCentury = (int)buttonItem.Tag;
		SelectedCentury = selectedCentury;
	}

	protected virtual void OnSelectedCenturyChanged(EventArgs e)
	{
		this.SelectedCenturyChanged?.Invoke(this, e);
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
		int num = _CenturyYearStart - 10;
		foreach (BaseItem subItem in _YearsContainer.SubItems)
		{
			if (subItem.Name.StartsWith("centurySelector"))
			{
				ButtonItem buttonItem = (ButtonItem)subItem;
				buttonItem.Text = $"<div>{num}-<br/>{num + 9}</div>";
				buttonItem.Tag = num;
				if (num == _SelectedYear)
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
		_CenturyLabel.Text = $"{_CenturyYearStart}-{_CenturyYearStart + 99}";
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
			if (subItem.Name.StartsWith("centurySelector"))
			{
				ButtonItem buttonItem = (ButtonItem)subItem;
				buttonItem.FixedSize = fixedSize;
			}
		}
	}

	private void YearsDecreaseClick(object sender, EventArgs e)
	{
		if (CenturyYearStart - 100 > 1760)
		{
			CenturyYearStart -= 100;
		}
	}

	private void YearsIncreaseClick(object sender, EventArgs e)
	{
		CenturyYearStart += 100;
	}
}
