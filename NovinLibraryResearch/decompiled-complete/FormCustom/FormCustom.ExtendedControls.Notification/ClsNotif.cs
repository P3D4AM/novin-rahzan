using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FormCustom.ExtendedControls.Notification;

[ToolboxBitmap(typeof(ClsNotif), "Icon.ico")]
[DefaultEvent("Click")]
internal class ClsNotif : Component
{
	private const int SW_SHOWNOACTIVATE = 4;

	private const int HWND_TOPMOST = -1;

	private const uint SWP_NOACTIVATE = 16u;

	private bool disposed;

	private frmPopup frmPopup;

	private Timer tmrAnimation;

	private Timer tmrWait;

	private bool isAppearing = true;

	private bool markedForDisposed;

	private bool mouseIsOn;

	private int maxPosition;

	private double maxOpacity;

	private int posStart;

	private int posStop;

	private double opacityStart;

	private double opacityStop;

	private int realAnimationDuration;

	private Stopwatch sw;

	private Size imageSize = new Size(0, 0);

	[Description("Color of the window header.")]
	[Category("Header")]
	[DefaultValue(typeof(Color), "ControlDark")]
	public Color HeaderColor { get; set; }

	[Description("Color of the window background.")]
	[DefaultValue(typeof(Color), "Control")]
	[Category("Appearance")]
	public Color BodyColor { get; set; }

	[Category("Title")]
	[Description("Color of the title text.")]
	[DefaultValue(typeof(Color), "Gray")]
	public Color TitleColor { get; set; }

	[Category("Content")]
	[Description("Color of the content text.")]
	[DefaultValue(typeof(Color), "ControlText")]
	public Color ContentColor { get; set; }

	[Category("Appearance")]
	[DefaultValue(typeof(Color), "WindowFrame")]
	[Description("Color of the window border.")]
	public Color BorderColor { get; set; }

	[Category("Buttons")]
	[Description("Border color of the close and options buttons when the mouse is over them.")]
	[DefaultValue(typeof(Color), "WindowFrame")]
	public Color ButtonBorderColor { get; set; }

	[Description("Background color of the close and options buttons when the mouse is over them.")]
	[Category("Buttons")]
	[DefaultValue(typeof(Color), "Highlight")]
	public Color ButtonHoverColor { get; set; }

	[DefaultValue(typeof(Color), "HotTrack")]
	[Description("Color of the content text when the mouse is hovering over it.")]
	[Category("Content")]
	public Color ContentHoverColor { get; set; }

	[DefaultValue(50)]
	[Description("Gradient of window background color.")]
	[Category("Appearance")]
	public int GradientPower { get; set; }

	[Category("Content")]
	[Description("Font of the content text.")]
	public Font ContentFont { get; set; }

	[Category("Title")]
	[Description("Font of the title.")]
	public Font TitleFont { get; set; }

	[Description("Size of the icon image.")]
	[Category("Image")]
	public Size ImageSize
	{
		get
		{
			if (imageSize.Width != 0)
			{
				return imageSize;
			}
			Image image = Image;
			if (image == null)
			{
				return new Size(0, 0);
			}
			return image.Size;
		}
		set
		{
			imageSize = value;
		}
	}

	[Category("Image")]
	[Description("Icon image to display.")]
	public Image Image { get; set; }

	[Category("Header")]
	[DefaultValue(true)]
	[Description("Whether to show a 'grip' image within the window header.")]
	public bool ShowGrip { get; set; }

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Whether to scroll the window or only fade it.")]
	private bool Scroll { get; set; }

	[Description("Content text to display.")]
	[Category("Content")]
	public string ContentText { get; set; }

	[Description("Title text to display.")]
	[Category("Title")]
	public string TitleText { get; set; }

	[Category("Title")]
	[Description("Padding of title text.")]
	public Padding TitlePadding { get; private set; }

	[Category("Content")]
	[Description("Padding of content text.")]
	public Padding ContentPadding { get; set; }

	[Category("Image")]
	[Description("Padding of icon image.")]
	public Padding ImagePadding { get; set; }

	[DefaultValue(9)]
	[Category("Header")]
	[Description("Height of window header.")]
	public int HeaderHeight { get; set; }

	[Category("Buttons")]
	[DefaultValue(true)]
	[Description("Whether to show the close button.")]
	public bool ShowCloseButton { get; set; }

	[Category("Buttons")]
	[DefaultValue(false)]
	[Description("Whether to show the options button.")]
	public bool ShowOptionsButton { get; set; }

	[Description("Context menu to open when clicking on the options button.")]
	[Category("Behavior")]
	public ContextMenuStrip OptionsMenu { get; set; }

	[Category("Behavior")]
	[Description("Time in milliseconds the window is displayed.")]
	[DefaultValue(3000)]
	public int Delay { get; set; }

	[DefaultValue(1000)]
	[Category("Behavior")]
	[Description("Time in milliseconds needed to make the window appear or disappear.")]
	public int AnimationDuration { get; set; }

	[Description("Interval in milliseconds used to draw the animation.")]
	[Category("Behavior")]
	[DefaultValue(10)]
	public int AnimationInterval { get; set; }

	[Category("Appearance")]
	[Description("Size of the window.")]
	public Size Size { get; set; }

	[Category("Content")]
	[Description("Show Content Right To Left,نمایش پیغام چپ به راست فعال شود")]
	public bool IsRightToLeft { get; set; }

	public event EventHandler Click;

	public event EventHandler Close;

	public event EventHandler Appear;

	public event EventHandler Disappear;

	[DllImport("user32.dll")]
	private static extern bool SetWindowPos(int hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

	[DllImport("user32.dll")]
	private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

	private static void ShowInactiveTopmost(Form frm)
	{
		ShowWindow(((Control)frm).Handle, 4);
		SetWindowPos(((Control)frm).Handle.ToInt32(), -1, ((Control)frm).Left, ((Control)frm).Top, ((Control)frm).Width, ((Control)frm).Height, 16u);
	}

	public void ResetImageSize()
	{
		imageSize = Size.Empty;
	}

	private bool ShouldSerializeImageSize()
	{
		return !imageSize.Equals((object?)Size.Empty);
	}

	private void ResetTitlePadding()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		TitlePadding = Padding.Empty;
	}

	private bool ShouldSerializeTitlePadding()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		Padding titlePadding = TitlePadding;
		return !((object)(Padding)(ref titlePadding)).Equals((object?)Padding.Empty);
	}

	private void ResetContentPadding()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		ContentPadding = Padding.Empty;
	}

	private bool ShouldSerializeContentPadding()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		Padding contentPadding = ContentPadding;
		return !((object)(Padding)(ref contentPadding)).Equals((object?)Padding.Empty);
	}

	private void ResetImagePadding()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		ImagePadding = Padding.Empty;
	}

	private bool ShouldSerializeImagePadding()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		Padding imagePadding = ImagePadding;
		return !((object)(Padding)(ref imagePadding)).Equals((object?)Padding.Empty);
	}

	public ClsNotif()
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Expected O, but got Unknown
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Expected O, but got Unknown
		HeaderColor = SystemColors.ControlDark;
		BodyColor = SystemColors.Control;
		TitleColor = Color.Gray;
		ContentColor = SystemColors.ControlText;
		BorderColor = SystemColors.WindowFrame;
		ButtonBorderColor = SystemColors.WindowFrame;
		ButtonHoverColor = SystemColors.Highlight;
		ContentHoverColor = SystemColors.HotTrack;
		GradientPower = 50;
		ContentFont = SystemFonts.DialogFont;
		TitleFont = SystemFonts.CaptionFont;
		ShowGrip = true;
		Scroll = true;
		TitlePadding = new Padding(0);
		ContentPadding = new Padding(0);
		ImagePadding = new Padding(0);
		HeaderHeight = 9;
		ShowCloseButton = true;
		ShowOptionsButton = false;
		Delay = 3000;
		AnimationInterval = 10;
		AnimationDuration = 1000;
		Size = new Size(400, 100);
		frmPopup = new frmPopup(this);
		((Form)frmPopup).FormBorderStyle = (FormBorderStyle)0;
		((Form)frmPopup).StartPosition = (FormStartPosition)0;
		((Form)frmPopup).FormBorderStyle = (FormBorderStyle)0;
		((Control)frmPopup).MouseEnter += frmPopup_MouseEnter;
		((Control)frmPopup).MouseLeave += frmPopup_MouseLeave;
		frmPopup.CloseClick += frmPopup_CloseClick;
		frmPopup.LinkClick += frmPopup_LinkClick;
		frmPopup.ContextMenuOpened += frmPopup_ContextMenuOpened;
		frmPopup.ContextMenuClosed += frmPopup_ContextMenuClosed;
		((Control)frmPopup).VisibleChanged += frmPopup_VisibleChanged;
		tmrAnimation = new Timer();
		tmrAnimation.Tick += tmAnimation_Tick;
		tmrWait = new Timer();
		tmrWait.Tick += tmWait_Tick;
	}

	public void Popup()
	{
		if (disposed)
		{
			return;
		}
		if (!((Control)frmPopup).Visible)
		{
			((Form)frmPopup).Size = Size;
			((Form)frmPopup).TopMost = true;
			if (Scroll)
			{
				posStart = Screen.PrimaryScreen.WorkingArea.Bottom;
				posStop = Screen.PrimaryScreen.WorkingArea.Bottom - ((Control)frmPopup).Height;
			}
			else
			{
				posStart = Screen.PrimaryScreen.WorkingArea.Bottom - ((Control)frmPopup).Height;
				posStop = Screen.PrimaryScreen.WorkingArea.Bottom - ((Control)frmPopup).Height;
			}
			opacityStart = 0.0;
			opacityStop = 1.0;
			((Form)frmPopup).Opacity = opacityStart;
			((Form)frmPopup).Location = new Point(Screen.PrimaryScreen.WorkingArea.Right - ((Form)frmPopup).Size.Width - 1, posStart);
			ShowInactiveTopmost((Form)(object)frmPopup);
			isAppearing = true;
			tmrWait.Interval = Delay;
			tmrAnimation.Interval = AnimationInterval;
			realAnimationDuration = AnimationDuration;
			tmrAnimation.Start();
			sw = Stopwatch.StartNew();
			return;
		}
		if (!isAppearing)
		{
			((Form)frmPopup).Size = Size;
			if (Scroll)
			{
				posStart = ((Control)frmPopup).Top;
				posStop = Screen.PrimaryScreen.WorkingArea.Bottom - ((Control)frmPopup).Height;
			}
			else
			{
				posStart = Screen.PrimaryScreen.WorkingArea.Bottom - ((Control)frmPopup).Height;
				posStop = Screen.PrimaryScreen.WorkingArea.Bottom - ((Control)frmPopup).Height;
			}
			opacityStart = ((Form)frmPopup).Opacity;
			opacityStop = 1.0;
			isAppearing = true;
			realAnimationDuration = Math.Max((int)sw.ElapsedMilliseconds, 1);
			sw.Restart();
		}
		((Control)frmPopup).Invalidate();
	}

	private void Hide()
	{
		tmrAnimation.Stop();
		tmrWait.Stop();
		((Control)frmPopup).Hide();
		if (markedForDisposed)
		{
			Dispose();
		}
	}

	private void frmPopup_ContextMenuClosed(object sender, EventArgs e)
	{
		if (!mouseIsOn)
		{
			tmrWait.Interval = Delay;
			tmrWait.Start();
		}
	}

	private void frmPopup_ContextMenuOpened(object sender, EventArgs e)
	{
		tmrWait.Stop();
	}

	private void frmPopup_LinkClick(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		MessageBox.Show(ContentText);
		this.Click?.Invoke(this, EventArgs.Empty);
	}

	private void frmPopup_CloseClick(object sender, EventArgs e)
	{
		Hide();
		this.Close?.Invoke(this, EventArgs.Empty);
	}

	private void frmPopup_VisibleChanged(object sender, EventArgs e)
	{
		if (((Control)frmPopup).Visible)
		{
			if (this.Appear != null)
			{
				this.Appear(this, EventArgs.Empty);
			}
		}
		else if (this.Disappear != null)
		{
			this.Disappear(this, EventArgs.Empty);
		}
	}

	private void tmAnimation_Tick(object sender, EventArgs e)
	{
		long elapsedMilliseconds = sw.ElapsedMilliseconds;
		int num = (int)(posStart + (posStop - posStart) * elapsedMilliseconds / realAnimationDuration);
		bool flag = posStop - posStart < 0;
		if ((flag && num < posStop) || (!flag && num > posStop))
		{
			num = posStop;
		}
		double num2 = opacityStart + (opacityStop - opacityStart) * (double)elapsedMilliseconds / (double)realAnimationDuration;
		bool flag2 = opacityStop - opacityStart < 0.0;
		if ((flag2 && num2 < opacityStop) || (!flag2 && num2 > opacityStop))
		{
			num2 = opacityStop;
		}
		((Control)frmPopup).Top = num;
		((Form)frmPopup).Opacity = num2;
		if (elapsedMilliseconds <= realAnimationDuration)
		{
			return;
		}
		sw.Reset();
		tmrAnimation.Stop();
		if (isAppearing)
		{
			if (Scroll)
			{
				posStart = Screen.PrimaryScreen.WorkingArea.Bottom - ((Control)frmPopup).Height;
				posStop = Screen.PrimaryScreen.WorkingArea.Bottom;
			}
			else
			{
				posStart = Screen.PrimaryScreen.WorkingArea.Bottom - ((Control)frmPopup).Height;
				posStop = Screen.PrimaryScreen.WorkingArea.Bottom - ((Control)frmPopup).Height;
			}
			opacityStart = 1.0;
			opacityStop = 0.0;
			realAnimationDuration = AnimationDuration;
			isAppearing = false;
			maxPosition = ((Control)frmPopup).Top;
			maxOpacity = ((Form)frmPopup).Opacity;
			if (!mouseIsOn)
			{
				tmrWait.Stop();
				tmrWait.Start();
			}
		}
		else
		{
			((Control)frmPopup).Hide();
			if (markedForDisposed)
			{
				Dispose();
			}
		}
	}

	private void tmWait_Tick(object sender, EventArgs e)
	{
		tmrWait.Stop();
		tmrAnimation.Interval = AnimationInterval;
		tmrAnimation.Start();
		sw.Restart();
	}

	private void frmPopup_MouseLeave(object sender, EventArgs e)
	{
		if (((Control)frmPopup).Visible)
		{
			ContextMenuStrip optionsMenu = OptionsMenu;
			if (optionsMenu == null || !((ToolStripDropDown)optionsMenu).Visible)
			{
				tmrWait.Interval = Delay;
				tmrWait.Start();
			}
		}
		mouseIsOn = false;
	}

	private void frmPopup_MouseEnter(object sender, EventArgs e)
	{
		if (!isAppearing)
		{
			((Control)frmPopup).Top = maxPosition;
			((Form)frmPopup).Opacity = maxOpacity;
			tmrAnimation.Stop();
		}
		tmrWait.Stop();
		mouseIsOn = true;
	}

	protected override void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (isAppearing)
			{
				markedForDisposed = true;
				return;
			}
			if (disposing)
			{
				((Component)(object)frmPopup)?.Dispose();
				tmrAnimation.Tick -= tmAnimation_Tick;
				tmrWait.Tick -= tmWait_Tick;
				((Component)(object)tmrAnimation).Dispose();
				((Component)(object)tmrWait).Dispose();
			}
			disposed = true;
		}
		base.Dispose(disposing);
	}
}
