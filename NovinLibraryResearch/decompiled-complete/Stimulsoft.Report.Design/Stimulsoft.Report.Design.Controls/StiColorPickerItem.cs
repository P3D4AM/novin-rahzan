using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Design.Components;

namespace Stimulsoft.Report.Design.Controls;

public class StiColorPickerItem : ButtonItem
{
	private Panel rootPanel;

	private StiPopup colorPopup;

	private StiColorSelectionControl colorSelectionControl;

	private bool isSelected;

	private Color selectedColor;

	private Image colorIcon;

	public Color SelectedColor
	{
		get
		{
			return selectedColor;
		}
		set
		{
			selectedColor = value;
			InvokeSelectedColorChanged();
			Invalidate();
		}
	}

	public bool IsFromStyle
	{
		get
		{
			return GetColorControl().IsFromStyle;
		}
		set
		{
			GetColorControl().IsFromStyle = value;
		}
	}

	public Image ColorIcon
	{
		get
		{
			return colorIcon;
		}
		set
		{
			ref Image reference = ref colorIcon;
			object obj = ((value != null) ? value.Clone() : null);
			reference = (Image)((obj is Image) ? obj : null);
		}
	}

	public event EventHandler SelectedColorChanged;

	public StiColorPickerItem()
	{
		FixedSize = new Size(StiScale.XXI(34.0), StiScale.YYI(22.0));
	}

	protected void InvokeSelectedColorChanged()
	{
		this.SelectedColorChanged?.Invoke(this, EventArgs.Empty);
	}

	private void ColorPopup_Closed(object sender, ToolStripDropDownClosedEventArgs e)
	{
		isSelected = false;
		Invalidate();
	}

	private void ColorSelectionControl_ColorChanged(object sender, EventArgs e)
	{
		selectedColor = colorSelectionControl.SelectedColor;
		InvokeSelectedColorChanged();
		((ToolStripDropDown)colorPopup).Close();
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		Image obj = colorIcon;
		if (obj != null)
		{
			obj.Dispose();
		}
	}

	protected override void OnClick()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		base.OnClick();
		isSelected = true;
		colorPopup = new StiPopup((Control)(object)GetPanel());
		((ToolStripDropDown)colorPopup).Closed += new ToolStripDropDownClosedEventHandler(ColorPopup_Closed);
		((ToolStripDropDown)colorPopup).Show(new Point(ScreenRectangle.Left, ScreenRectangle.Bottom));
	}

	public override void Paint(ItemPaintArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		e.Graphics.SmoothingMode = (SmoothingMode)3;
		Rectangle bounds = Bounds;
		Rectangle rectangle = new Rectangle(bounds.X, bounds.Top, bounds.Width, bounds.Height);
		if (base.IsMouseOver && Enabled)
		{
			SolidBrush val = new SolidBrush(ColorTranslator.FromHtml("#e0ecff"));
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val, rectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (isSelected && Enabled)
		{
			SolidBrush val2 = new SolidBrush(ColorTranslator.FromHtml("#9ebae6"));
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val2, rectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (ColorIcon != null)
		{
			try
			{
				if (Enabled)
				{
					e.Graphics.DrawImage(ColorIcon, new Point(bounds.X + StiScale.I5, bounds.Top + StiScale.I2));
				}
				else
				{
					object obj = ColorIcon.Clone();
					Bitmap inputImage = (Bitmap)((obj is Bitmap) ? obj : null);
					inputImage = ImageHelper.CreateGrayScaleImage((Image)(object)inputImage);
					e.Graphics.DrawImage((Image)(object)inputImage, new Point(bounds.X + StiScale.I5, bounds.Top + StiScale.I2));
					((Image)inputImage).Dispose();
				}
			}
			catch
			{
			}
		}
		PaintSelectedColor(e.Graphics);
		PaintExpandButton(e.Graphics);
	}

	private void PaintSelectedColor(Graphics g)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		Rectangle rectangle = new Rectangle(Bounds.Left + StiScale.I6, Bounds.Bottom - StiScale.I5, StiScale.I15, StiScale.I2);
		SolidBrush val = new SolidBrush(SelectedColor);
		try
		{
			g.FillRectangle((Brush)(object)val, rectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public void PaintExpandButton(Graphics g)
	{
		Rectangle displayRectangle = DisplayRectangle;
		int verticalScrollBarWidth = SystemInformation.VerticalScrollBarWidth;
		StiButtonHelper.DrawDownArrow(g, new Rectangle(displayRectangle.Right - verticalScrollBarWidth, displayRectangle.Y, verticalScrollBarWidth, displayRectangle.Height), Enabled, Color.Black, ColorTranslator.FromHtml("#aaaaaa"));
	}

	private StiColorSelectionControl GetColorControl()
	{
		StiColorSelectionControl stiColorSelectionControl = new StiColorSelectionControl(SelectedColor, isExpression: false, null);
		((Control)stiColorSelectionControl).Location = new Point(StiScale.I1, StiScale.I1);
		((Control)stiColorSelectionControl).Width = StiScale.YYI(163.0);
		((Control)stiColorSelectionControl).Anchor = (AnchorStyles)13;
		stiColorSelectionControl.SelectedColor = SelectedColor;
		colorSelectionControl = stiColorSelectionControl;
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

	private Panel GetRootPanel()
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
}
