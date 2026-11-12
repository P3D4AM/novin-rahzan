using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Design.Components.FontIcons;

[ToolboxItem(false)]
public class StiFontIconSelectionButton : Control
{
	private Panel panelBoxIcon;

	private StiOperationButton buttonCustomIcon;

	private StiOperationButton buttonIcon;

	private StiPopup popupIcons;

	private StiFontIconSelectionControl controlFontIconSelection;

	private StiHintTextBox textBoxSearch;

	private Panel panelIcons;

	private bool isBuild;

	private Color iconColor = Color.FromArgb(68, 114, 196);

	private StiFontIcons icon;

	private bool isEmptyIcon;

	private bool showCustomIcon;

	public Color IconColor
	{
		get
		{
			return iconColor;
		}
		set
		{
			if (iconColor != value)
			{
				iconColor = value;
				RefreshSampleIcon();
			}
		}
	}

	private Color PreparedIconColor
	{
		get
		{
			Color color = IconColor.RemoveAlpha();
			if (color.R > 200 && color.G > 200 && color.B > 200)
			{
				return StiColorUtils.Dark(color, 50);
			}
			return color;
		}
	}

	[DefaultValue(StiFontIcons.Latin5)]
	public StiFontIcons Icon
	{
		get
		{
			return icon;
		}
		set
		{
			if (icon != value)
			{
				icon = value;
				InvokeIconChanged();
				RefreshSampleIcon();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public StiFontIcons? IconWithEmpty
	{
		get
		{
			if (!ShowEmptyIcon || !IsEmptyIcon)
			{
				return Icon;
			}
			return null;
		}
		set
		{
			if (ShowEmptyIcon && !value.HasValue)
			{
				Icon = StiFontIcons.Latin5;
				IsEmptyIcon = true;
			}
			else
			{
				Icon = value.GetValueOrDefault();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsEmptyIcon
	{
		get
		{
			return isEmptyIcon;
		}
		set
		{
			if (isEmptyIcon != value)
			{
				isEmptyIcon = value;
				InvokeIconChanged();
				RefreshSampleIcon();
			}
		}
	}

	[DefaultValue(false)]
	public bool ShowCustomIcon
	{
		get
		{
			return showCustomIcon;
		}
		set
		{
			if (showCustomIcon != value)
			{
				showCustomIcon = value;
				CheckState();
			}
		}
	}

	[DefaultValue(false)]
	public bool ShowEmptyIcon { get; set; }

	public event EventHandler CustomIconClick;

	public event EventHandler IconChanged;

	public StiFontIconSelectionButton()
	{
		((Control)this).ResizeRedraw = true;
		((Control)this).DoubleBuffered = true;
		InitControls();
	}

	protected void InvokeCustomIconClick()
	{
		this.CustomIconClick?.Invoke(this, EventArgs.Empty);
	}

	protected void InvokeIconChanged()
	{
		this.IconChanged?.Invoke(this, EventArgs.Empty);
	}

	private bool ShouldSerializeIconColor()
	{
		return IconColor != Color.FromArgb(68, 114, 196);
	}

	private void ButtonIcon_Click(object sender, EventArgs e)
	{
		CheckColorIcons();
		((ToolStripDropDown)popupIcons).Show(new Point(((Control)panelBoxIcon).RectangleToScreen(Rectangle.Empty).Left, ((Control)buttonIcon).RectangleToScreen(Rectangle.Empty).Bottom + ((Control)buttonIcon).Height + StiScale.I2));
		((Control)textBoxSearch).Focus();
	}

	private void ButtonCustomIcon_CustomIconClick(object sender, EventArgs e)
	{
		((Control)popupIcons).Hide();
		InvokeCustomIconClick();
	}

	private void PanelBoxIcon_Paint(object sender, PaintEventArgs e)
	{
		RefreshSampleIcon();
	}

	private void TextBoxSearch_TextChanged(object sender, EventArgs e)
	{
		StiHintTextBox stiHintTextBox = sender as StiHintTextBox;
		string filterName = ((Control)stiHintTextBox).Text.ToLower();
		BuildIconPanel(filterName);
	}

	private void FontIconSelectionControl_ValueChanged(object sender, EventArgs e)
	{
		StiFontIcons? selectedIcon = ((StiFontIconSelectionControl)sender).SelectedIcon;
		Icon = selectedIcon.GetValueOrDefault();
		IsEmptyIcon = ShowEmptyIcon && !selectedIcon.HasValue;
		RefreshSampleIcon();
		((Control)popupIcons).Hide();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		((Control)this).OnPaint(e);
		if (!isBuild)
		{
			Build();
		}
	}

	private void This_Load(object sender, EventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		popupIcons = new StiPopup((Control)(object)GetIconPanel());
		((Control)panelBoxIcon).Paint += new PaintEventHandler(PanelBoxIcon_Paint);
		CheckState();
	}

	private void InitControls()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		panelBoxIcon = new Panel
		{
			Dock = (DockStyle)5,
			BackColor = Color.White,
			Location = new Point(0, 0),
			Margin = new Padding(StiScale.XXI(2.0)),
			Padding = new Padding(StiScale.XXI(1.0))
		};
		StiOperationButton stiOperationButton = new StiOperationButton();
		((Control)stiOperationButton).Dock = (DockStyle)4;
		((Control)stiOperationButton).Margin = new Padding(0);
		((Control)stiOperationButton).Size = new Size(StiScale.XXI(23.0), StiScale.YYI(23.0));
		((Control)stiOperationButton).Font = new Font("Microsoft Sans Serif", 6.5f);
		((Control)stiOperationButton).Text = "...";
		((Control)stiOperationButton).TabStop = false;
		((ButtonBase)stiOperationButton).UseVisualStyleBackColor = true;
		buttonCustomIcon = stiOperationButton;
		((Control)buttonCustomIcon).Click += ButtonCustomIcon_CustomIconClick;
		StiOperationButton stiOperationButton2 = new StiOperationButton();
		((Control)stiOperationButton2).Dock = (DockStyle)4;
		((Control)stiOperationButton2).Margin = new Padding(0);
		((Control)stiOperationButton2).Size = new Size(StiScale.XXI(23.0), StiScale.YYI(23.0));
		((Control)stiOperationButton2).TabStop = false;
		((ButtonBase)stiOperationButton2).UseVisualStyleBackColor = true;
		buttonIcon = stiOperationButton2;
		((Control)buttonIcon).Click += ButtonIcon_Click;
	}

	private void Build()
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		((Control)panelBoxIcon).Controls.Add((Control)(object)buttonCustomIcon);
		((Control)panelBoxIcon).Controls.Add((Control)(object)buttonIcon);
		((Control)this).Controls.Add((Control)(object)panelBoxIcon);
		popupIcons = new StiPopup((Control)(object)GetIconPanel());
		((Control)panelBoxIcon).Paint += new PaintEventHandler(PanelBoxIcon_Paint);
		CheckState();
		isBuild = true;
	}

	private void CheckState()
	{
		((Control)buttonCustomIcon).Visible = ShowCustomIcon;
	}

	private Panel GetIconPanel()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		Panel val = new Panel
		{
			Width = StiScale.XXI(274.0),
			Height = StiScale.YYI(342.0),
			BackColor = StiUX.Background,
			Padding = new Padding(StiScale.I1)
		};
		((Control)val).Paint += new PaintEventHandler(StiIconControlHelper.PanelRootPainter);
		Panel val2 = new Panel
		{
			Dock = (DockStyle)1,
			Size = new Size(StiScale.XXI(150.0), StiScale.YYI(28.0)),
			Padding = new Padding(StiScale.I10, StiScale.I6, StiScale.I10, StiScale.I6),
			BackColor = StiUX.Background
		};
		StiHintTextBox stiHintTextBox = new StiHintTextBox();
		((Control)stiHintTextBox).Dock = (DockStyle)5;
		stiHintTextBox.AllowDrawBorder = false;
		stiHintTextBox.Hint = StiLocalization.Get("FormViewer", "Find");
		((Control)stiHintTextBox).Font = new Font("Arial", 10f);
		textBoxSearch = stiHintTextBox;
		((Control)textBoxSearch).TextChanged += TextBoxSearch_TextChanged;
		((Control)val2).Controls.Add((Control)(object)textBoxSearch);
		panelIcons = new Panel
		{
			AutoScroll = true,
			Dock = (DockStyle)5
		};
		((Control)val).Controls.Add((Control)(object)panelIcons);
		((Control)val).Controls.Add((Control)(object)val2);
		BuildIconPanel();
		return val;
	}

	private void RefreshSampleIcon()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		Graphics val = ((Control)panelBoxIcon).CreateGraphics();
		FontFamily fontFamilyIcons = StiFontIconsHelper.GetFontFamilyIcons();
		Font val2 = new Font(fontFamilyIcons, 13f);
		try
		{
			Color color = (((Control)this).Enabled ? PreparedIconColor : Color.LightGray);
			SolidBrush val3 = new SolidBrush(color);
			try
			{
				val.FillRectangle(Brushes.White, new Rectangle(StiScale.I2, StiScale.I4, ((Control)panelBoxIcon).Width - StiScale.I4, ((Control)panelBoxIcon).Height - StiScale.I6));
				if (!ShowEmptyIcon || (ShowEmptyIcon && !IsEmptyIcon))
				{
					val.DrawString(StiFontIconsHelper.GetContent(Icon), val2, (Brush)(object)val3, (RectangleF)new Rectangle(StiScale.I2, StiScale.I4, ((Control)panelBoxIcon).Width, ((Control)panelBoxIcon).Height));
				}
				val.DrawRectangle(new Pen((Brush)new SolidBrush(Color.FromArgb(150, 150, 150))), 0, 0, ((Control)panelBoxIcon).Width - 1, ((Control)panelBoxIcon).Height - 1);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private void BuildIconPanel(string filterName = null)
	{
		((Control)panelIcons).Controls.Clear();
		StiFontIconSelectionControl stiFontIconSelectionControl = new StiFontIconSelectionControl(Icon, IconColor, showCustomIcon: false, ShowEmptyIcon);
		((Control)stiFontIconSelectionControl).Location = new Point(StiScale.I2, StiScale.I2);
		((Control)stiFontIconSelectionControl).Width = ((Control)panelIcons).Width;
		((Control)stiFontIconSelectionControl).Anchor = (AnchorStyles)13;
		stiFontIconSelectionControl.TextFilter = filterName;
		controlFontIconSelection = stiFontIconSelectionControl;
		controlFontIconSelection.ValueChanged += FontIconSelectionControl_ValueChanged;
		((Control)panelIcons).Controls.Add((Control)(object)controlFontIconSelection);
	}

	private void CheckColorIcons()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		foreach (object item in (ArrangedElementCollection)((Control)(Panel)popupIcons.Content).Controls)
		{
			if (item is StiFontIconButton stiFontIconButton)
			{
				((Control)stiFontIconButton).ForeColor = IconColor;
			}
		}
		if (controlFontIconSelection != null)
		{
			controlFontIconSelection.IconColor = IconColor;
		}
	}
}
