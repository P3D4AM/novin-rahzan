using System.Security;
using System.Windows.Forms;

namespace Stimulsoft.Base.Drawing;

[SuppressUnmanagedCodeSecurity]
public sealed class StiOSFeature
{
	public static bool IsLayeredWindows { get; }

	public static bool IsThemes { get; }

	static StiOSFeature()
	{
		IsLayeredWindows = ((FeatureSupport)OSFeature.Feature).GetVersionPresent(OSFeature.LayeredWindows) != null;
		IsThemes = ((FeatureSupport)OSFeature.Feature).GetVersionPresent(OSFeature.Themes) != null;
	}
}
