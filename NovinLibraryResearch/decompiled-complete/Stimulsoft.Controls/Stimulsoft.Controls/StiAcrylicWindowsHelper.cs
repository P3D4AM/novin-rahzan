using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Stimulsoft.Controls;

public static class StiAcrylicWindowsHelper
{
	private struct WindowCompositionAttributeData
	{
		public WindowCompositionAttribute Attribute;

		public IntPtr Data;

		public int SizeOfData;
	}

	private enum WindowCompositionAttribute
	{
		ACCENT_POLICY = 19
	}

	private enum ACCENT
	{
		DISABLED,
		ENABLE_GRADIENT,
		ENABLE_TRANSPARENTGRADIENT,
		ENABLE_BLURBEHIND,
		ENABLE_ACRYLICBLURBEHIND,
		INVALID_STATE
	}

	private struct AccentPolicy
	{
		public ACCENT AccentState;

		public uint AccentFlags;

		public uint GradientColor;

		public uint AnimationId;
	}

	public static void EnableAcrylic(IntPtr handle, Color blurColor)
	{
		AccentPolicy structure = default(AccentPolicy);
		int num = Marshal.SizeOf(structure);
		structure.AccentFlags = 480u;
		structure.GradientColor = ToAbgr(blurColor);
		structure.AccentState = ACCENT.ENABLE_ACRYLICBLURBEHIND;
		IntPtr intPtr = Marshal.AllocHGlobal(num);
		Marshal.StructureToPtr(structure, intPtr, fDeleteOld: false);
		WindowCompositionAttributeData windowCompositionAttributeData = default(WindowCompositionAttributeData);
		windowCompositionAttributeData.Attribute = WindowCompositionAttribute.ACCENT_POLICY;
		windowCompositionAttributeData.SizeOfData = num;
		windowCompositionAttributeData.Data = intPtr;
		WindowCompositionAttributeData data = windowCompositionAttributeData;
		SetWindowCompositionAttribute(handle, ref data);
	}

	private static uint ToAbgr(Color color)
	{
		return (uint)((color.A << 24) | (color.B << 16) | (color.G << 8) | color.R);
	}

	[DllImport("user32.dll")]
	private static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);
}
