using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Help;

public class StiHelpViewerForm : Form, IStiThreadForm
{
	private Timer timerUrl;

	private static StiHelpViewerForm currentForm;

	private bool isCustomHelp;

	private IContainer components;

	private WebBrowser webBrowser;

	internal static string Url { get; set; }

	private StiHelpViewerForm()
	{
		InitializeComponent();
		currentForm = this;
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Form)this).StartPosition = (FormStartPosition)0;
		StiFormSettings.Load((Form)(object)this);
		((Control)this).Text = StiLocalization.Get("HelpDesigner", "StimulsoftHelp");
	}

	public void InvokeCustomHelp()
	{
		isCustomHelp = StiOptions.Engine.GlobalEvents.InvokeOpenCustomHelp(this);
	}

	private void LoadPosition()
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Invalid comparison between Unknown and I4
		StiSettings.Load();
		Size size = new Size(800, 700);
		int @int = StiSettings.GetInt("HelpViewerForm", "ScreenNumber", -999);
		Point point;
		if (@int == -999)
		{
			Rectangle rectangle = Rectangle.Empty;
			if (Screen.AllScreens.Length == 1 || Form.ActiveForm == null)
			{
				rectangle = Screen.PrimaryScreen.Bounds;
			}
			else
			{
				Form activeForm = Form.ActiveForm;
				Point location;
				Size size2;
				if ((int)activeForm.WindowState == 2)
				{
					location = activeForm.RestoreBounds.Location;
					size2 = activeForm.RestoreBounds.Size;
				}
				else
				{
					location = activeForm.Location;
					size2 = activeForm.Size;
				}
				bool flag = false;
				for (int i = 0; i < Screen.AllScreens.Length; i++)
				{
					Screen val = Screen.AllScreens[i];
					if (val.Bounds.IntersectsWith(new Rectangle(location, size2)))
					{
						rectangle = val.Bounds;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					rectangle = Screen.PrimaryScreen.Bounds;
				}
			}
			point = new Point(rectangle.Right - size.Width, rectangle.Top + (rectangle.Height - size.Height) / 2);
			((Form)this).SetBoundsCore(point.X, point.Y, size.Width, size.Height, (BoundsSpecified)15);
			return;
		}
		int primaryScreenNumber = StiFormHelper.GetPrimaryScreenNumber();
		point = new Point(StiSettings.GetInt("HelpViewerForm", "WindowXPos", 0), StiSettings.GetInt("HelpViewerForm", "WindowYPos", 0));
		size.Width = StiSettings.GetInt("HelpViewerForm", "WindowWidth", size.Width);
		size.Height = StiSettings.GetInt("HelpViewerForm", "WindowHeight", size.Height);
		if (@int != primaryScreenNumber)
		{
			if (@int + 1 > Screen.AllScreens.Length)
			{
				Screen primaryScreen = Screen.PrimaryScreen;
				point = primaryScreen.Bounds.Location;
			}
			else
			{
				Screen primaryScreen = Screen.AllScreens[@int];
				if (!primaryScreen.Bounds.Contains(new Rectangle(point, size)))
				{
					point = primaryScreen.Bounds.Location;
				}
			}
		}
		else
		{
			Screen primaryScreen = Screen.PrimaryScreen;
			if (StiSettings.GetInt("HelpViewerForm", "ScreenNumber", -1) == -1)
			{
				Rectangle bounds = primaryScreen.Bounds;
				point = new Point(bounds.Right - size.Width, bounds.Top + bounds.Height - size.Height);
			}
		}
		((Form)this).SetBoundsCore(point.X, point.Y, size.Width, size.Height, (BoundsSpecified)15);
	}

	private void SetUrl(string url)
	{
		if (isCustomHelp)
		{
			currentForm.webBrowser.Url = new Uri(url);
			return;
		}
		string text = StiOptions.Configuration.ApplicationDirectory + "\\Help\\" + StiLocalization.CultureName + "\\" + url;
		if (File.Exists(text))
		{
			currentForm.webBrowser.Url = new Uri(text, UriKind.Absolute);
			return;
		}
		string cultureName = StiLocalization.CultureName;
		string text2 = ((!(cultureName == "ru")) ? "en" : "ru");
		url = "https://www.stimulsoft.com/" + text2 + "/documentation/online/" + url;
		currentForm.webBrowser.Url = new Uri(url);
	}

	public static void Show(string url)
	{
		if (currentForm == null)
		{
			Thread thread = new Thread(RunHelp);
			thread.TrySetApartmentState(ApartmentState.STA);
			thread.Name = "StiHelp";
			thread.Start();
		}
		Url = url;
	}

	private static void RunHelp()
	{
		Application.Run((Form)(object)new StiHelpViewerForm());
	}

	private void This_FormClosed(object sender, FormClosedEventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
		currentForm = null;
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			((Form)this).Close();
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		((Form)this).OnLoad(e);
		InvokeCustomHelp();
		if (Url != null)
		{
			string url = Url;
			Url = null;
			SetUrl(url);
		}
		timerUrl = new Timer
		{
			Interval = 1500
		};
		timerUrl.Tick += TimerUrl_Tick;
		timerUrl.Start();
	}

	private void TimerUrl_Tick(object sender, EventArgs e)
	{
		timerUrl.Stop();
		if (Url != null)
		{
			string url = Url;
			Url = null;
			SetUrl(url);
			((Form)this).Activate();
		}
		timerUrl.Start();
	}

	private void WebBrowser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			((Form)this).Close();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Expected O, but got Unknown
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiHelpViewerForm));
		webBrowser = new WebBrowser();
		((Control)this).SuspendLayout();
		webBrowser.AllowWebBrowserDrop = false;
		((Control)webBrowser).Dock = (DockStyle)5;
		((Control)webBrowser).Location = new Point(0, 0);
		((Control)webBrowser).MinimumSize = new Size(20, 20);
		((Control)webBrowser).Name = "webBrowser";
		webBrowser.ScrollBarsEnabled = false;
		((Control)webBrowser).Size = new Size(884, 703);
		((Control)webBrowser).TabIndex = 1;
		((Control)webBrowser).PreviewKeyDown += new PreviewKeyDownEventHandler(WebBrowser_PreviewKeyDown);
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(884, 703);
		((Control)this).Controls.Add((Control)(object)webBrowser);
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiHelpViewerForm";
		((Form)this).FormClosed += new FormClosedEventHandler(This_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)this).ResumeLayout(false);
	}
}
