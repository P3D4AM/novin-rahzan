using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxBitmap(typeof(ColorPickerButton), "Controls.ColorPickerButton.ico")]
[ToolboxItem(false)]
[DefaultEvent("SelectedColorChanged")]
[ComVisible(false)]
public class ColorPickerButton : ButtonX
{
	private bool IsMouseOver { get; set; }

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsFromStyle
	{
		get
		{
			return GetColorPickerDropDown().IsFromStyle;
		}
		set
		{
			GetColorPickerDropDown().IsFromStyle = value;
		}
	}

	[DefaultValue(null)]
	[Browsable(false)]
	[Category("Colors")]
	[Description("Array of ColorItem objects that will be used as standard colors instead of built-in color palette.")]
	public ColorItem[][] CustomStandardColors
	{
		get
		{
			return GetColorPickerDropDown().CustomStandardColors;
		}
		set
		{
			GetColorPickerDropDown().CustomStandardColors = value;
		}
	}

	[DefaultValue(null)]
	[Browsable(false)]
	[Category("Colors")]
	[Description("Array of ColorItem objects that will be used as theme colors instead of built-in color palette.")]
	public ColorItem[][] CustomThemeColors
	{
		get
		{
			return GetColorPickerDropDown().CustomThemeColors;
		}
		set
		{
			GetColorPickerDropDown().CustomThemeColors = value;
		}
	}

	protected new virtual bool ExecuteCommandOnClick => false;

	[Browsable(true)]
	[DefaultValue(true)]
	[DevCoSerialize]
	[Category("Appearance")]
	[Description("Indicates more colors menu item is visible which allows user to open Custom Colors dialog box.")]
	public bool DisplayMoreColors
	{
		get
		{
			return GetColorPickerDropDown().DisplayMoreColors;
		}
		set
		{
			GetColorPickerDropDown().DisplayMoreColors = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color SelectedColor
	{
		get
		{
			return GetColorPickerDropDown().SelectedColor;
		}
		set
		{
			GetColorPickerDropDown().SelectedColor = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[DevCoSerialize]
	[Category("Appearance")]
	[Description("Indicates whether theme colors are displayed on drop-down.")]
	public bool DisplayThemeColors
	{
		get
		{
			return GetColorPickerDropDown().DisplayThemeColors;
		}
		set
		{
			GetColorPickerDropDown().DisplayThemeColors = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[DevCoSerialize]
	[Category("Appearance")]
	[Description("Indicates whether standard colors are displayed on drop-down.")]
	public bool DisplayStandardColors
	{
		get
		{
			return GetColorPickerDropDown().DisplayStandardColors;
		}
		set
		{
			GetColorPickerDropDown().DisplayStandardColors = value;
		}
	}

	[Description("Indicates rectangle in Image coordinates where selected color will be painted. Property will have effect only if Image property is used to set the image.")]
	[Category("Behaviour")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Rectangle SelectedColorImageRectangle
	{
		get
		{
			return GetColorPickerDropDown().SelectedColorImageRectangle;
		}
		set
		{
			GetColorPickerDropDown().SelectedColorImageRectangle = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Image Image
	{
		get
		{
			return base.Image;
		}
		set
		{
			base.Image = value;
		}
	}

	[Description("Occurs when color is chosen from drop-down color picker or from Custom Colors dialog box.")]
	public event EventHandler SelectedColorChanged;

	[Description("Occurs when mouse is moving over the colors presented by the color picker")]
	public event ColorPreviewEventHandler ColorPreview;

	protected override void OnMouseEnter(EventArgs e)
	{
		base.OnMouseEnter(e);
		IsMouseOver = true;
		((Control)this).Invalidate();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		base.OnMouseEnter(e);
		IsMouseOver = false;
		((Control)this).Invalidate();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		Graphics graphics = e.Graphics;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		DrawBackground(graphics, clientRectangle);
		DrawBorder(graphics, clientRectangle);
		DrawArrow(graphics, clientRectangle);
		if (((Control)this).Enabled)
		{
			DrawSelectedColor(graphics, clientRectangle);
		}
	}

	private void DrawBackground(Graphics g, Rectangle rect)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		if (!((Control)this).Enabled)
		{
			g.FillRectangle(Brushes.Gainsboro, rect);
			return;
		}
		if (IsMouseOver)
		{
			SolidBrush val = new SolidBrush(ColorTranslator.FromHtml("0xe0ecff"));
			try
			{
				g.FillRectangle((Brush)(object)val, rect);
				return;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		g.FillRectangle(Brushes.White, rect);
	}

	public void DrawBorder(Graphics graphics, Rectangle rect)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		Color color = ((((Control)this).Focused || IsMouseOver) ? StiColors.SelectedText : SystemColors.ControlDark);
		if (!((Control)this).Enabled)
		{
			color = Color.Gray;
		}
		Pen val = new Pen(color);
		try
		{
			graphics.DrawRectangle(val, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void DrawArrow(Graphics g, Rectangle rect)
	{
		int verticalScrollBarWidth = SystemInformation.VerticalScrollBarWidth;
		StiButtonHelper.DrawDownArrow(g, new Rectangle(rect.Right - verticalScrollBarWidth, rect.Y, verticalScrollBarWidth, rect.Height), ((Control)this).Enabled);
	}

	private void DrawSelectedColor(Graphics g, Rectangle rect)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		rect.Inflate(-StiScale2.I2, -StiScale2.I2);
		rect.Width -= SystemInformation.VerticalScrollBarWidth - StiScale2.I2;
		SolidBrush val = new SolidBrush(SelectedColor);
		try
		{
			g.FillRectangle((Brush)(object)val, rect);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public ColorPickerButton()
	{
		ColorPickerDropDown colorPickerDropDown = GetColorPickerDropDown();
		colorPickerDropDown.Localize();
		colorPickerDropDown.SelectedColorChanged += InternalSelectedColorChanged;
		colorPickerDropDown.ColorPreview += InternalColorPreview;
	}

	public DialogResult DisplayMoreColorsDialog()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return GetColorPickerDropDown().DisplayMoreColorsDialog();
	}

	protected override ButtonItem CreateButtonItem()
	{
		return new ColorPickerDropDown();
	}

	private ColorPickerDropDown GetColorPickerDropDown()
	{
		return base.InternalItem as ColorPickerDropDown;
	}

	private void InternalColorPreview(object sender, ColorPreviewEventArgs e)
	{
		OnColorPreview(e);
	}

	protected virtual void OnColorPreview(ColorPreviewEventArgs e)
	{
		if (this.ColorPreview != null)
		{
			this.ColorPreview(this, e);
		}
	}

	private void InternalSelectedColorChanged(object sender, EventArgs e)
	{
		OnSelectedColorChanged(e);
		ExecuteCommand();
	}

	protected virtual void OnSelectedColorChanged(EventArgs e)
	{
		if (this.SelectedColorChanged != null)
		{
			this.SelectedColorChanged(this, e);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSubItems()
	{
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSelectedColorImageRectangle()
	{
		return GetColorPickerDropDown().ShouldSerializeSelectedColorImageRectangle();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectedColorImageRectangle()
	{
		GetColorPickerDropDown().ResetSelectedColorImageRectangle();
	}

	public void InvokeColorPreview(ColorPreviewEventArgs e)
	{
		GetColorPickerDropDown().InvokeColorPreview(e);
	}

	public void UpdateSelectedColorImage()
	{
		GetColorPickerDropDown().UpdateSelectedColorImage();
	}
}
