using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

[DefaultEvent("SelectedColorChanged")]
[ToolboxItem(true)]
[ComVisible(false)]
[ToolboxBitmap(typeof(ColorPickerButton), "Controls.ColorPickerButton.ico")]
[Designer("DevComponents.DotNetBar.Design.ColorPickerButtonDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class ColorPickerButton : ButtonX
{
	private EventHandler Ṳ;

	private ColorPreviewEventHandler ṳ;

	[Category("Colors")]
	[Description("Array of ColorItem objects that will be used as standard colors instead of built-in color palette.")]
	[Browsable(false)]
	[DefaultValue(null)]
	public ColorItem[][] CustomStandardColors
	{
		get
		{
			return _2ACF().CustomStandardColors;
		}
		set
		{
			_2ACF().CustomStandardColors = value;
		}
	}

	[Category("Colors")]
	[Browsable(false)]
	[Description("Array of ColorItem objects that will be used as theme colors instead of built-in color palette.")]
	[DefaultValue(null)]
	public ColorItem[][] CustomThemeColors
	{
		get
		{
			return _2ACF().CustomThemeColors;
		}
		set
		{
			_2ACF().CustomThemeColors = value;
		}
	}

	protected new virtual bool ExecuteCommandOnClick => false;

	[Browsable(false)]
	[DefaultValue(null)]
	public IWin32Window OwnerWindow
	{
		get
		{
			return _2ACF().OwnerWindow;
		}
		set
		{
			_2ACF().OwnerWindow = value;
		}
	}

	[Description("Indicates more colors menu item is visible which allows user to open Custom Colors dialog box.")]
	[DevCoSerialize]
	[Category("Appearance")]
	[DefaultValue(true)]
	[Browsable(true)]
	public bool DisplayMoreColors
	{
		get
		{
			return _2ACF().DisplayMoreColors;
		}
		set
		{
			_2ACF().DisplayMoreColors = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Color SelectedColor
	{
		get
		{
			return _2ACF().SelectedColor;
		}
		set
		{
			_2ACF().SelectedColor = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates whether theme colors are displayed on drop-down.")]
	[DevCoSerialize]
	[Browsable(true)]
	[DefaultValue(true)]
	public bool DisplayThemeColors
	{
		get
		{
			return _2ACF().DisplayThemeColors;
		}
		set
		{
			_2ACF().DisplayThemeColors = value;
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Browsable(true)]
	[DevCoSerialize]
	[Description("Indicates whether standard colors are displayed on drop-down.")]
	public bool DisplayStandardColors
	{
		get
		{
			return _2ACF().DisplayStandardColors;
		}
		set
		{
			_2ACF().DisplayStandardColors = value;
		}
	}

	[Category("Behaviour")]
	[Browsable(true)]
	[Description("Indicates rectangle in Image coordinates where selected color will be painted. Property will have effect only if Image property is used to set the image.")]
	public Rectangle SelectedColorImageRectangle
	{
		get
		{
			return _2ACF().SelectedColorImageRectangle;
		}
		set
		{
			_2ACF().SelectedColorImageRectangle = value;
		}
	}

	[Description("Occurs when color is chosen from drop-down color picker or from Custom Colors dialog box.")]
	public event EventHandler SelectedColorChanged
	{
		add
		{
			EventHandler eventHandler = Ṳ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ṳ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = Ṳ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ṳ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when mouse is moving over the colors presented by the color picker")]
	public event ColorPreviewEventHandler ColorPreview
	{
		add
		{
			ColorPreviewEventHandler colorPreviewEventHandler = ṳ;
			ColorPreviewEventHandler colorPreviewEventHandler2;
			do
			{
				colorPreviewEventHandler2 = colorPreviewEventHandler;
				ColorPreviewEventHandler value2 = (ColorPreviewEventHandler)Delegate.Combine(colorPreviewEventHandler2, value);
				colorPreviewEventHandler = Interlocked.CompareExchange(ref ṳ, value2, colorPreviewEventHandler2);
			}
			while ((object)colorPreviewEventHandler != colorPreviewEventHandler2);
		}
		remove
		{
			ColorPreviewEventHandler colorPreviewEventHandler = ṳ;
			ColorPreviewEventHandler colorPreviewEventHandler2;
			do
			{
				colorPreviewEventHandler2 = colorPreviewEventHandler;
				ColorPreviewEventHandler value2 = (ColorPreviewEventHandler)Delegate.Remove(colorPreviewEventHandler2, value);
				colorPreviewEventHandler = Interlocked.CompareExchange(ref ṳ, value2, colorPreviewEventHandler2);
			}
			while ((object)colorPreviewEventHandler != colorPreviewEventHandler2);
		}
	}

	public ColorPickerButton()
	{
		ColorPickerDropDown colorPickerDropDown = _2ACF();
		colorPickerDropDown.SelectedColorChanged += _2AD1;
		colorPickerDropDown.ColorPreview += _2AD0;
	}

	public DialogResult DisplayMoreColorsDialog()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return _2ACF().DisplayMoreColorsDialog();
	}

	protected override ButtonItem CreateButtonItem()
	{
		return new ColorPickerDropDown();
	}

	private ColorPickerDropDown _2ACF()
	{
		return base.InternalItem as ColorPickerDropDown;
	}

	private void _2AD0(object _٠, ColorPreviewEventArgs ؿ)
	{
		OnColorPreview(ؿ);
	}

	protected virtual void OnColorPreview(ColorPreviewEventArgs e)
	{
		if (ṳ != null)
		{
			ṳ(this, e);
		}
	}

	private void _2AD1(object _٠, EventArgs ؿ)
	{
		OnSelectedColorChanged(ؿ);
		ExecuteCommand();
	}

	protected virtual void OnSelectedColorChanged(EventArgs e)
	{
		if (Ṳ != null)
		{
			Ṳ(this, e);
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
		return _2ACF().ShouldSerializeSelectedColorImageRectangle();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectedColorImageRectangle()
	{
		_2ACF().ResetSelectedColorImageRectangle();
	}

	public void InvokeColorPreview(ColorPreviewEventArgs e)
	{
		_2ACF().InvokeColorPreview(e);
	}

	public void UpdateSelectedColorImage()
	{
		_2ACF().UpdateSelectedColorImage();
	}
}
