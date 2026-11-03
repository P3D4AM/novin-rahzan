using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

[ToolboxBitmap(typeof(StyleManagerAmbient), "StyleManager.ico")]
[ProvideProperty("EnableAmbientSettings", typeof(Control))]
[ToolboxItem(true)]
public class StyleManagerAmbient : Component, IExtenderProvider
{
	private Dictionary<Control, eAmbientSettings> _ᐸ = new Dictionary<Control, eAmbientSettings>();

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

	[Editor("DevComponents.DotNetBar.Design.FlagEnumUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("Indicates ambient settings StyleManager is allowed to change on the control.")]
	[DefaultValue(eAmbientSettings.All)]
	public eAmbientSettings GetEnableAmbientSettings(Control c)
	{
		eAmbientSettings value = eAmbientSettings.All;
		if (_ᐸ.TryGetValue(c, out value))
		{
			return value;
		}
		return eAmbientSettings.All;
	}

	public void SetEnableAmbientSettings(Control c, eAmbientSettings ambientSettings)
	{
		if (_ᐸ.ContainsKey(c))
		{
			if (ambientSettings == eAmbientSettings.All)
			{
				_ᐸ.Remove(c);
			}
			else
			{
				_ᐸ[c] = ambientSettings;
			}
		}
		else if (ambientSettings != eAmbientSettings.All)
		{
			_ᐸ.Add(c, ambientSettings);
		}
	}

	internal bool Contains(Control c)
	{
		return _ᐸ.ContainsKey(c);
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
