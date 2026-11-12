using System.Security;

namespace Stimulsoft.Controls.DesktopIntegration;

[SuppressUnmanagedCodeSecurity]
internal static class SafeNativeMethods
{
	public const uint THBN_CLICKED = 6144u;

	public const int DWM_SIT_DISPLAYFRAME = 1;

	public const int WM_COMMAND = 273;

	public const int WM_SYSCOMMAND = 274;

	public const int WM_ACTIVATE = 6;

	public const int WM_CLOSE = 16;

	public const int WM_DWMSENDICONICTHUMBNAIL = 803;

	public const int WM_DWMSENDICONICLIVEPREVIEWBITMAP = 806;

	public const int SC_CLOSE = 61536;

	public const int WA_ACTIVE = 1;

	public const int WA_CLICKACTIVE = 2;

	public const int DWMWA_FORCE_ICONIC_REPRESENTATION = 7;

	public const int DWMWA_HAS_ICONIC_BITMAP = 10;
}
