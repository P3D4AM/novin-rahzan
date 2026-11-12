using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Design.Components;

[ToolboxItem(false)]
public class StiColorBoxXF : StiButtonEditBase, IStiPopupParentControl
{
	private Panel rootPanel;

	private StiPopup colorPopup;

	private StiColorSelectionControl colorSelectionControl;

	private ToolTip tooltTip;

	private bool isFromStyle;

	private bool showColorBox = true;

	private bool showColorName = true;

	private Color selectedColor = Color.Transparent;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool LockPopupInvoke { get; set; }

	[Browsable(false)]
	public override string Text
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			((Control)this).Text = value;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	public bool IsFromStyle
	{
		get
		{
			return isFromStyle;
		}
		set
		{
			if (isFromStyle != value)
			{
				isFromStyle = value;
				((Control)this).Invalidate();
			}
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	public bool ShowColorBox
	{
		get
		{
			return showColorBox;
		}
		set
		{
			if (showColorBox != value)
			{
				showColorBox = value;
				((Control)this).Invalidate();
			}
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	public bool ShowColorName
	{
		get
		{
			return showColorName;
		}
		set
		{
			if (showColorName != value)
			{
				showColorName = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Category("Behavior")]
	public Color SelectedColor
	{
		get
		{
			return selectedColor;
		}
		set
		{
			if (selectedColor != value)
			{
				selectedColor = value;
				InvokeSelectedColorChanged();
				((Control)this).Invalidate();
			}
		}
	}

	public event EventHandler SelectedColorChanged;

	protected void InvokeSelectedColorChanged()
	{
		this.SelectedColorChanged?.Invoke(this, EventArgs.Empty);
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		base.OnPaint(p);
		Graphics graphics = p.Graphics;
		Rectangle rect = Rectangle.Empty;
		Rectangle rectangle = Rectangle.Empty;
		Rectangle buttonRect = StiControlPaint.GetButtonRect(((Control)this).ClientRectangle, flat: true, (RightToLeft)0);
		Rectangle rectangle2 = new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
		if (showColorBox)
		{
			rect = ((!showColorName) ? new Rectangle(rectangle2.X + StiScaleUI.I2, rectangle2.Y + StiScaleUI.I2, ((Control)this).Width - buttonRect.Width - StiScaleUI.I4 - 1, ((Control)this).Height - StiScaleUI.I5 + 1) : new Rectangle(rectangle2.X + StiScaleUI.I2, rectangle2.Y + StiScaleUI.I2, StiScaleUI.XXI(22.0), ((Control)this).Height - StiScaleUI.I5 + 1));
			if (StiScaleUI.Id == SystemScaleID.x1)
			{
				rect.Height--;
			}
		}
		if (showColorName)
		{
			rectangle = ((!showColorBox) ? new Rectangle(StiScaleUI.I2, StiScaleUI.I1, ((Control)this).Width - buttonRect.Width - StiScaleUI.I2, ((Control)this).Height - StiScaleUI.I2) : new Rectangle(rect.Right + StiScaleUI.I2, StiScaleUI.I1, ((Control)this).Width - rect.Width - buttonRect.Width - StiScaleUI.I4, ((Control)this).Height - StiScaleUI.I2));
		}
		if (showColorBox)
		{
			if (((Control)this).Enabled)
			{
				StiDrawing.FillRectangle(graphics, SelectedColor, rect);
				StiDrawing.DrawRectangle(graphics, StiUX.IsDark ? StiUX.InputBorder : Color.Gray, rect);
			}
			else
			{
				StiDrawing.DrawRectangle(graphics, StiUX.IsDark ? StiUX.InputBorder : SystemColors.ControlDark, rect);
			}
		}
		if (!showColorName)
		{
			return;
		}
		StringFormat val = new StringFormat();
		try
		{
			val.LineAlignment = (StringAlignment)1;
			val.FormatFlags = (StringFormatFlags)4096;
			string colorName = GetColorName();
			if (((Control)this).Enabled)
			{
				SolidBrush val2 = new SolidBrush(((Control)this).ForeColor);
				try
				{
					graphics.DrawString(colorName, ((Control)this).Font, (Brush)(object)val2, (RectangleF)rectangle, val);
					return;
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			SolidBrush val3 = new SolidBrush(StiUX.IsDark ? StiUX.ItemForegroundDisabled : SystemColors.ControlDark);
			try
			{
				graphics.DrawString(colorName, ((Control)this).Font, (Brush)(object)val3, (RectangleF)rectangle, val);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void OnEnabledChanged(EventArgs e)
	{
		base.OnEnabledChanged(e);
		((Control)base.TextBox).Visible = false;
	}

	private void ColorPopup_Closed(object sender, ToolStripDropDownClosedEventArgs e)
	{
		((Control)this).Invalidate();
	}

	private void ColorSelectionControl_ColorChanged(object sender, EventArgs e)
	{
		selectedColor = colorSelectionControl.SelectedColor;
		InvokeSelectedColorChanged();
		((ToolStripDropDown)colorPopup).Close();
		((Control)this).Invalidate();
	}

	private string GetColorName()
	{
		if (SelectedColor == Color.Transparent && IsFromStyle)
		{
			return StiLocalization.Get("FormStyleDesigner", "FromStyle");
		}
		if (SelectedColor.IsSystemColor)
		{
			return StiLocalization.Get("PropertySystemColors", SelectedColor.Name);
		}
		if (SelectedColor.IsKnownColor)
		{
			return StiLocalization.Get("PropertyColor", SelectedColor.Name);
		}
		return StiColorFXHelper.HexConverter(SelectedColor);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			Image buttonBitmap = base.ButtonBitmap;
			if (buttonBitmap != null)
			{
				buttonBitmap.Dispose();
			}
			((Component)(object)tooltTip)?.Dispose();
		}
		((Control)this).Dispose(disposing);
	}

	protected override void OnMouseHover(EventArgs e)
	{
		base.OnMouseHover(e);
		string colorName = GetColorName();
		ShowTooltip(colorName);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		base.OnMouseLeave(e);
		HideTooltip();
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		base.OnMouseDown(e);
		colorPopup = new StiPopup((Control)(object)GetPanel());
		((ToolStripDropDown)colorPopup).Closed += new ToolStripDropDownClosedEventHandler(ColorPopup_Closed);
		((ToolStripDropDown)colorPopup).Show(new Point(((Control)this).RectangleToScreen(((Control)this).ClientRectangle).Left, ((Control)this).RectangleToScreen(((Control)this).ClientRectangle).Bottom));
	}

	private StiColorSelectionControl GetColorControl()
	{
		StiColorSelectionControl obj = new StiColorSelectionControl(SelectedColor, isExpression: false, null)
		{
			IsFromStyle = IsFromStyle
		};
		((Control)obj).Location = new Point(StiScale.I1, StiScale.I1);
		((Control)obj).Width = StiScale.YYI(163.0);
		((Control)obj).Anchor = (AnchorStyles)13;
		obj.SelectedColor = SelectedColor;
		colorSelectionControl = obj;
		colorSelectionControl.ColorChanged += ColorSelectionControl_ColorChanged;
		return colorSelectionControl;
	}

	private Panel GetPanel()
	{
		rootPanel = GetRootPanel();
		((Control)rootPanel).Height = GetColorControl().GetHeight() + StiScale.I2;
		((Control)rootPanel).Controls.Add((Control)(object)GetColorControl());
		return rootPanel;
	}

	private static Panel GetRootPanel()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		Panel val = new Panel
		{
			Width = StiScale.XXI(165.0),
			BackColor = StiUX.Background,
			Padding = new Padding(StiScale.I1)
		};
		((Control)val).Paint += new PaintEventHandler(StiIconControlHelper.PanelRootPainter);
		return val;
	}

	private void ShowTooltip(string text)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		if (tooltTip == null)
		{
			tooltTip = new ToolTip
			{
				InitialDelay = 1000,
				ReshowDelay = 1000
			};
		}
		if (!tooltTip.Active || !(tooltTip.GetToolTip((Control)(object)this) == text))
		{
			ToolTip obj = tooltTip;
			if (obj != null)
			{
				obj.Hide((IWin32Window)(object)this);
			}
			ToolTip obj2 = tooltTip;
			if (obj2 != null)
			{
				obj2.SetToolTip((Control)(object)this, text);
			}
		}
	}

	private void HideTooltip()
	{
		ToolTip obj = tooltTip;
		if (obj != null)
		{
			obj.Hide((IWin32Window)(object)this);
		}
	}

	public StiColorBoxXF()
	{
		((Control)base.TextBox).Visible = false;
	}
}
