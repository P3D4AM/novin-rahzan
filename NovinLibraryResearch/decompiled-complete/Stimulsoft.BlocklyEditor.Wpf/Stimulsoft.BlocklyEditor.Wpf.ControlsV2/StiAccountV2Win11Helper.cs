using System.Windows;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.BlocklyEditor.Wpf.ControlsV2;

public static class StiAccountV2Win11Helper
{
	public static double SeparatorLine
	{
		get
		{
			if (!StiOSVersion.IsWindows11())
			{
				return 1.0;
			}
			return StiScale.ScaleThickness;
		}
	}

	public static double DefaultRadius => StiOSVersion.IsWindows11() ? 2 : 0;

	public static CornerRadius WindowsFooterCornerRadius
	{
		get
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (!StiOSVersion.IsWindows11())
			{
				return new CornerRadius(0.0);
			}
			return new CornerRadius(0.0, 0.0, 9.0, 9.0);
		}
	}

	public static CornerRadius WindowCornerRadius
	{
		get
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (!StiOSVersion.IsWindows11())
			{
				return new CornerRadius(0.0);
			}
			return new CornerRadius(9.0);
		}
	}

	public static CornerRadius ButtonCornerRadius
	{
		get
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (!StiOSVersion.IsWindows11())
			{
				return new CornerRadius(0.0);
			}
			return new CornerRadius(2.0);
		}
	}

	public static Thickness Thickness1111
	{
		get
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (!StiOSVersion.IsWindows11())
			{
				return new Thickness(1.0);
			}
			return new Thickness(StiScale.ScaleThickness);
		}
	}

	public static Thickness Thickness1010
	{
		get
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			if (!StiOSVersion.IsWindows11())
			{
				return new Thickness(1.0, 0.0, 1.0, 0.0);
			}
			return new Thickness(StiScale.ScaleThickness, 0.0, StiScale.ScaleThickness, 0.0);
		}
	}

	public static Thickness Thickness1000
	{
		get
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			if (!StiOSVersion.IsWindows11())
			{
				return new Thickness(1.0, 0.0, 0.0, 0.0);
			}
			return new Thickness(StiScale.ScaleThickness, 0.0, 0.0, 0.0);
		}
	}

	public static Thickness Thickness0001
	{
		get
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			if (!StiOSVersion.IsWindows11())
			{
				return new Thickness(0.0, 0.0, 0.0, 1.0);
			}
			return new Thickness(0.0, 0.0, 0.0, StiScale.ScaleThickness);
		}
	}

	public static Thickness Thickness1011
	{
		get
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			if (!StiOSVersion.IsWindows11())
			{
				return new Thickness(1.0, 0.0, 1.0, 1.0);
			}
			return new Thickness(StiScale.ScaleThickness, 0.0, StiScale.ScaleThickness, StiScale.ScaleThickness);
		}
	}

	public static Thickness Thickness0101
	{
		get
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			if (!StiOSVersion.IsWindows11())
			{
				return new Thickness(0.0, 1.0, 0.0, 1.0);
			}
			return new Thickness(0.0, StiScale.ScaleThickness, 0.0, StiScale.ScaleThickness);
		}
	}

	public static Thickness Thickness1110
	{
		get
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			if (!StiOSVersion.IsWindows11())
			{
				return new Thickness(1.0, 1.0, 1.0, 0.0);
			}
			return new Thickness(StiScale.ScaleThickness, StiScale.ScaleThickness, StiScale.ScaleThickness, 0.0);
		}
	}
}
