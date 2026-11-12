using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxBitmap(typeof(StyleManagerAmbient), "StyleManager.ico")]
[ToolboxItem(false)]
[ProvideProperty("EnableAmbientSettings", typeof(Control))]
public class StyleManagerAmbient : Component, IExtenderProvider
{
	private Dictionary<Control, eAmbientSettings> _Settings = new Dictionary<Control, eAmbientSettings>();

	public StyleManagerAmbient()
	{
		StyleManager.RegisterAmbientManager(this);
	}

	public StyleManagerAmbient(IContainer container)
		: this()
	{
		container.Add(this);
	}

	protected override void Dispose(bool disposing)
	{
		StyleManager.UnregisterAmbientManager(this);
		base.Dispose(disposing);
	}

	[DefaultValue(eAmbientSettings.All)]
	[Description("Indicates ambient settings StyleManager is allowed to change on the control.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.FlagEnumUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public eAmbientSettings GetEnableAmbientSettings(Control c)
	{
		eAmbientSettings value = eAmbientSettings.All;
		if (_Settings.TryGetValue(c, out value))
		{
			return value;
		}
		return eAmbientSettings.All;
	}

	public void SetEnableAmbientSettings(Control c, eAmbientSettings ambientSettings)
	{
		if (_Settings.ContainsKey(c))
		{
			if (ambientSettings == eAmbientSettings.All)
			{
				_Settings.Remove(c);
			}
			else
			{
				_Settings[c] = ambientSettings;
			}
		}
		else if (ambientSettings != eAmbientSettings.All)
		{
			_Settings.Add(c, ambientSettings);
		}
	}

	internal bool Contains(Control c)
	{
		return _Settings.ContainsKey(c);
	}

	public bool CanExtend(object extendee)
	{
		if (extendee is Control)
		{
			return true;
		}
		return false;
	}
}
