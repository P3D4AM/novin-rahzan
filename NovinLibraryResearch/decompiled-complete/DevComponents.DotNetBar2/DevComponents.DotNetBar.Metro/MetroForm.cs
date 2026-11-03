using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro.ColorTables;
using DevComponents.DotNetBar.Metro.Rendering;

namespace DevComponents.DotNetBar.Metro;

public class MetroForm : OfficeForm
{
	private const string ᐳ = "<font size=\"7\">SETTINGS</font>";

	private const string ᐴ = "<font size=\"7\">HELP</font>";

	private ButtonItem _ᐸ;

	private ButtonItem _ᐹ;

	private BorderColors[] _ய;

	private string _ᐻ = "";

	private string _ᐺ = "";

	private EventHandler Ꮸ;

	private EventHandler Ꮹ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public BorderColors[] BorderColors
	{
		get
		{
			return _ய;
		}
		set
		{
			_ய = value;
			((Control)this).Invalidate();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool EnableGlass
	{
		get
		{
			return base.EnableGlass;
		}
		set
		{
			base.EnableGlass = value;
		}
	}

	protected override bool UseCornerSize => false;

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Indicates whether SETTINGS button is visible.")]
	public bool SettingsButtonVisible
	{
		get
		{
			return _ᐸ.Visible;
		}
		set
		{
			if (value != _ᐸ.Visible)
			{
				_ᐸ.Visible = value;
				if (((Control)this).IsHandleCreated)
				{
					_0BE5();
				}
			}
		}
	}

	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether HELP button is visible.")]
	public bool HelpButtonVisible
	{
		get
		{
			return _ᐹ.Visible;
		}
		set
		{
			if (value != _ᐹ.Visible)
			{
				_ᐹ.Visible = value;
				_0BE5();
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates HELP button text")]
	[DefaultValue("")]
	public string HelpButtonText
	{
		get
		{
			return _ᐻ;
		}
		set
		{
			if (value != _ᐻ)
			{
				string ᐻ = _ᐻ;
				_ᐻ = value;
				OnHelpButtonTextChanged(ᐻ, value);
			}
		}
	}

	[Description("Indicates SETTINGS button text")]
	[DefaultValue("")]
	[Category("Appearance")]
	public string SettingsButtonText
	{
		get
		{
			return _ᐺ;
		}
		set
		{
			if (value != _ᐺ)
			{
				string ᐺ = _ᐺ;
				_ᐺ = value;
				OnSettingsButtonTextChanged(ᐺ, value);
			}
		}
	}

	[Description("Occurs when SETTINGS button, if displayed, is clicked.")]
	public event EventHandler SettingsButtonClick
	{
		add
		{
			EventHandler eventHandler = Ꮸ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ꮸ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = Ꮸ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ꮸ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when HELP button, if displayed, is clicked.")]
	public event EventHandler HelpButtonClick
	{
		add
		{
			EventHandler eventHandler = Ꮹ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ꮹ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = Ꮹ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ꮹ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public MetroForm()
	{
		if (!StyleManager.IsMetro(StyleManager.Style))
		{
			StyleManager.Style = eStyle.Metro;
		}
		StyleManager.Register((Control)(object)this);
		base.EnableGlass = false;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			StyleManager.Unregister((Control)(object)this);
		}
		base.Dispose(disposing);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		Ꮒ();
		base.OnHandleCreated(e);
	}

	protected override Color[] GetBorderColors(int borderSize)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		MetroColorTable colorTable = MetroRender.GetColorTable();
		MetroFormColorTable metroForm = colorTable.MetroForm;
		Color backColor = ((Control)this).BackColor;
		BorderColors[] array = _ய ?? metroForm.BorderColors;
		Color[] array2 = new Color[((int)((Form)this).FormBorderStyle == 1) ? 1 : (array.Length + ((borderSize > array.Length) ? 1 : 0))];
		if ((int)((Form)this).FormBorderStyle == 1 && borderSize > 1)
		{
			array2 = new Color[4]
			{
				array[0].Left,
				default(Color),
				default(Color),
				default(Color)
			};
			ref Color reference = ref array2[1];
			reference = array2[0];
			ref Color reference2 = ref array2[2];
			reference2 = array2[0];
			array2[3] = backColor;
			return array2;
		}
		for (int i = 0; i < array2.Length; i++)
		{
			if (i > array.Length - 1)
			{
				array2[i] = backColor;
				continue;
			}
			ref Color reference3 = ref array2[i];
			reference3 = array[i].Left;
		}
		return array2;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		base.StyleManagerStyleChanged(newStyle);
		if (BarFunctions.IsHandleValid((Control)(object)this))
		{
			Ꮒ();
		}
	}

	private void Ꮒ()
	{
		StyleManager.UpdateMetroAmbientColors((Control)(object)this);
	}

	protected override GraphicsPath GetFormPath(Rectangle bounds)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		val.AddRectangle(bounds);
		return val;
	}

	protected override LabelItem CreateTitleLabel()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		LabelItem labelItem = new LabelItem();
		labelItem.GlobalItem = false;
		try
		{
			labelItem.Font = new Font("Segoe UI", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		}
		catch
		{
			labelItem.Font = SystemFonts.MenuFont;
		}
		labelItem.Stretch = true;
		labelItem.TextLineAlignment = (StringAlignment)1;
		labelItem.TextAlignment = (StringAlignment)1;
		labelItem.Text = ((Control)this).Text;
		labelItem.PaddingLeft = 3;
		labelItem.PaddingRight = 1;
		return labelItem;
	}

	protected override void CreateAdditionalCaptionItems(GenericItemContainer captionContainer)
	{
		_ᐸ = new ButtonItem("sysSettingsButton");
		_ᐸ.Text = "<font size=\"7\">SETTINGS</font>";
		_ᐸ.Click += ặ;
		_ᐸ.SetSystemItem(b: true);
		_ᐸ.CanCustomize = false;
		_ᐸ.Visible = false;
		captionContainer.SubItems.Add(_ᐸ);
		_ᐹ = new ButtonItem("sysHelpButton");
		_ᐹ.Text = "<font size=\"7\">HELP</font>";
		_ᐹ.SetSystemItem(b: true);
		_ᐹ.CanCustomize = false;
		_ᐹ.Visible = false;
		_ᐹ.Click += Ẹ;
		captionContainer.SubItems.Add(_ᐹ);
		base.CreateAdditionalCaptionItems(captionContainer);
	}

	protected virtual void OnHelpButtonTextChanged(string oldValue, string newValue)
	{
		if (string.IsNullOrEmpty(newValue))
		{
			_ᐹ.Text = "<font size=\"7\">HELP</font>";
		}
		else
		{
			_ᐹ.Text = "<font size=\"7\">" + newValue + "</font>";
		}
		_0BE5();
	}

	protected virtual void OnSettingsButtonTextChanged(string oldValue, string newValue)
	{
		if (string.IsNullOrEmpty(newValue))
		{
			_ᐸ.Text = "<font size=\"7\">SETTINGS</font>";
		}
		else
		{
			_ᐸ.Text = "<font size=\"7\">" + newValue + "</font>";
		}
		_0BE5();
	}

	private void ặ(object _٠, EventArgs ؿ)
	{
		OnSettingsButtonClick(ؿ);
	}

	private void Ẹ(object _٠, EventArgs ؿ)
	{
		OnHelpButtonClick(ؿ);
	}

	protected virtual void OnSettingsButtonClick(EventArgs e)
	{
		Ꮸ?.Invoke(this, e);
	}

	protected virtual void OnHelpButtonClick(EventArgs e)
	{
		Ꮹ?.Invoke(this, e);
	}
}
