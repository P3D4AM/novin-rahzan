using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Metro.ColorTables;
using Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro;

public class MetroForm : OfficeForm
{
	private const string DefaultSettingsButtonText = "<font size=\"7\">SETTINGS</font>";

	private const string DefaultHelpButtonText = "<font size=\"7\">HELP</font>";

	private ButtonItem _Settings;

	private ButtonItem _Help;

	private string _HelpButtonText = "";

	private string _SettingsButtonText = "";

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether SETTINGS button is visible.")]
	public bool SettingsButtonVisible
	{
		get
		{
			return _Settings.Visible;
		}
		set
		{
			if (value != _Settings.Visible)
			{
				_Settings.Visible = value;
				RecalcSize();
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
			return _Help.Visible;
		}
		set
		{
			if (value != _Help.Visible)
			{
				_Help.Visible = value;
				RecalcSize();
			}
		}
	}

	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates HELP button text")]
	public string HelpButtonText
	{
		get
		{
			return _HelpButtonText;
		}
		set
		{
			if (value != _HelpButtonText)
			{
				string helpButtonText = _HelpButtonText;
				_HelpButtonText = value;
				OnHelpButtonTextChanged(helpButtonText, value);
			}
		}
	}

	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates SETTINGS button text")]
	public string SettingsButtonText
	{
		get
		{
			return _SettingsButtonText;
		}
		set
		{
			if (value != _SettingsButtonText)
			{
				string settingsButtonText = _SettingsButtonText;
				_SettingsButtonText = value;
				OnSettingsButtonTextChanged(settingsButtonText, value);
			}
		}
	}

	[Description("Occurs when SETTINGS button, if displayed, is clicked.")]
	public event EventHandler SettingsButtonClick;

	[Description("Occurs when HELP button, if displayed, is clicked.")]
	public event EventHandler HelpButtonClick;

	public MetroForm()
	{
		if (StyleManager.Style != eStyle.Metro)
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
		UpdateColorScheme();
		base.OnHandleCreated(e);
	}

	protected override Color[] GetBorderColors(int borderSize)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		MetroColorTable colorTable = MetroRender.GetColorTable();
		MetroFormColorTable metroForm = colorTable.MetroForm;
		Color canvasColor = colorTable.CanvasColor;
		Color[] array = new Color[((int)((Form)this).FormBorderStyle == 1) ? 1 : (metroForm.BorderColors.Length + ((borderSize > metroForm.BorderColors.Length) ? 1 : 0))];
		for (int i = 0; i < array.Length; i++)
		{
			if (i > metroForm.BorderColors.Length - 1)
			{
				array[i] = canvasColor;
			}
			else
			{
				array[i] = metroForm.BorderColors[i].Left;
			}
		}
		return array;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		UpdateColorScheme();
	}

	private void UpdateColorScheme()
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

	protected override Rectangle GetInnerFormBounds()
	{
		Rectangle result = new Rectangle(3, 3, ((Control)this).Width - 7, ((Control)this).Height - 2);
		if (((Form)this).RightToLeftLayout)
		{
			result = new Rectangle(3, 3, ((Control)this).Width - 6, ((Control)this).Height - 2);
		}
		return result;
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
		_Settings = new ButtonItem("sysSettingsButton");
		_Settings.Text = "<font size=\"7\">SETTINGS</font>";
		_Settings.Click += InternalSettingsButtonClick;
		_Settings.SetSystemItem(b: true);
		_Settings.CanCustomize = false;
		_Settings.Visible = false;
		captionContainer.SubItems.Add(_Settings);
		_Help = new ButtonItem("sysHelpButton");
		_Help.Text = "<font size=\"7\">HELP</font>";
		_Help.SetSystemItem(b: true);
		_Help.CanCustomize = false;
		_Help.Visible = false;
		_Help.Click += InternalHelpButtonClick;
		captionContainer.SubItems.Add(_Help);
		base.CreateAdditionalCaptionItems(captionContainer);
	}

	protected virtual void OnHelpButtonTextChanged(string oldValue, string newValue)
	{
		if (string.IsNullOrEmpty(newValue))
		{
			_Help.Text = "<font size=\"7\">HELP</font>";
		}
		else
		{
			_Help.Text = "<font size=\"7\">" + newValue + "</font>";
		}
		RecalcSize();
	}

	protected virtual void OnSettingsButtonTextChanged(string oldValue, string newValue)
	{
		if (string.IsNullOrEmpty(newValue))
		{
			_Settings.Text = "<font size=\"7\">SETTINGS</font>";
		}
		else
		{
			_Settings.Text = "<font size=\"7\">" + newValue + "</font>";
		}
		RecalcSize();
	}

	private void InternalSettingsButtonClick(object sender, EventArgs e)
	{
		OnSettingsButtonClick(e);
	}

	private void InternalHelpButtonClick(object sender, EventArgs e)
	{
		OnHelpButtonClick(e);
	}

	protected virtual void OnSettingsButtonClick(EventArgs e)
	{
		this.SettingsButtonClick?.Invoke(this, e);
	}

	protected virtual void OnHelpButtonClick(EventArgs e)
	{
		this.HelpButtonClick?.Invoke(this, e);
	}
}
