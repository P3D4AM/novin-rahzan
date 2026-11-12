using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class HTMLHelp
{
	public struct tagHH_FTS_QUERY
	{
		private long cbStruct;

		private long fUniCodeStrings;

		private string pszSearchQuery;

		private long iProximity;

		private long fStemmedSearch;

		private long fTitleOnly;

		private long fExecute;

		private string pszWindow;
	}

	private const int HH_DISPLAY_TOPIC = 0;

	private const int HH_DISPLAY_TOC = 1;

	private const int HH_DISPLAY_INDEX = 2;

	private const int HH_DISPLAY_SEARCH = 3;

	private const int HH_HELP_CONTEXT = 15;

	public const int WM_CLOSE = 16;

	private Control m_ParentWindow;

	private string m_HelpFile = "";

	private IntPtr m_HtmlHelpWindowHandle = IntPtr.Zero;

	public Control Parent
	{
		get
		{
			return m_ParentWindow;
		}
		set
		{
			m_ParentWindow = value;
		}
	}

	public string HelpFile
	{
		get
		{
			return m_HelpFile;
		}
		set
		{
			m_HelpFile = value;
		}
	}

	[DllImport("hhctrl.ocx", CharSet = CharSet.Ansi, EntryPoint = "HtmlHelpA", SetLastError = true)]
	private static extern IntPtr HtmlHelp(IntPtr hwnd, string lpHelpFile, int wCommand, int dwData);

	[DllImport("hhctrl.ocx", CharSet = CharSet.Auto, EntryPoint = "HtmlHelp", SetLastError = true)]
	private static extern IntPtr HTMLHelpFtsQuery(IntPtr hwnd, string lpHelpFile, int wCommand, ref tagHH_FTS_QUERY dwData);

	[DllImport("user32")]
	private static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

	[DllImport("user32")]
	private static extern bool IsWindow(IntPtr hWnd);

	public HTMLHelp()
	{
	}

	public HTMLHelp(Control parent, string helpfile)
	{
		m_ParentWindow = parent;
		m_HelpFile = helpfile;
	}

	public void ShowContents()
	{
		if (File.Exists(m_HelpFile))
		{
			m_HtmlHelpWindowHandle = HtmlHelp(m_ParentWindow.Handle, m_HelpFile, 1, 0);
			return;
		}
		throw new InvalidOperationException("Help file could not be found.");
	}

	public void ShowIndex()
	{
		if (File.Exists(m_HelpFile))
		{
			m_HtmlHelpWindowHandle = HtmlHelp(m_ParentWindow.Handle, m_HelpFile, 2, 0);
			return;
		}
		throw new InvalidOperationException("Help file could not be found.");
	}

	public void ShowSearch()
	{
		if (File.Exists(m_HelpFile))
		{
			m_HtmlHelpWindowHandle = HtmlHelp(m_ParentWindow.Handle, m_HelpFile, 3, 0);
			return;
		}
		throw new InvalidOperationException("Help file could not be found.");
	}

	public void CloseHelpWindow()
	{
		if (m_HtmlHelpWindowHandle != IntPtr.Zero && IsWindow(m_HtmlHelpWindowHandle))
		{
			PostMessage(m_HtmlHelpWindowHandle, 16, 0, 0);
		}
		m_HtmlHelpWindowHandle = IntPtr.Zero;
	}
}
