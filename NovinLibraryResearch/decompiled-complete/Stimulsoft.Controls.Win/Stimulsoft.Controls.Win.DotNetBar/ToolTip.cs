using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using Stimulsoft.Controls.Win.DotNetBar.Metro.ColorTables;
using Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class ToolTip : Control
{
	private const long WS_POPUP = 2147483648L;

	private const long WS_CLIPSIBLINGS = 67108864L;

	private const long WS_CLIPCHILDREN = 33554432L;

	private const long WS_EX_TOOLWINDOW = 128L;

	private const long WS_EX_TOPMOST = 8L;

	private string m_ToolTip;

	private BodyElement m_TextMarkup;

	private eDotNetBarStyle m_Style = eDotNetBarStyle.Office2003;

	private PopupShadow m_DropShadow;

	private bool m_AntiAlias;

	public Rectangle ReferenceRectangle = Rectangle.Empty;

	private static bool _MarkupEnabled = true;

	private int _ColorDepth = -1;

	public string Text
	{
		get
		{
			return m_ToolTip;
		}
		set
		{
			m_ToolTip = value;
			if (MarkupEnabled && MarkupParser.IsMarkup(ref m_ToolTip))
			{
				m_TextMarkup = MarkupParser.Parse(m_ToolTip);
			}
			else
			{
				m_TextMarkup = null;
			}
			if (m_TextMarkup != null)
			{
				AntiAlias = true;
			}
		}
	}

	public static bool MarkupEnabled
	{
		get
		{
			return _MarkupEnabled;
		}
		set
		{
			_MarkupEnabled = value;
		}
	}

	public bool AntiAlias
	{
		get
		{
			return m_AntiAlias;
		}
		set
		{
			m_AntiAlias = value;
		}
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Control)this).CreateParams;
			createParams.Style = -2046820352;
			createParams.ExStyle = 136;
			createParams.Caption = "";
			return createParams;
		}
	}

	public eDotNetBarStyle Style
	{
		get
		{
			return m_Style;
		}
		set
		{
			m_Style = value;
			if (BarFunctions.IsOffice2007Style(m_Style))
			{
				AntiAlias = true;
			}
		}
	}

	public ToolTip()
	{
		m_ToolTip = "";
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).Font = SystemFonts.DefaultFont;
		((Control)this).BackColor = SystemColors.Control;
		((Control)this).ForeColor = SystemColors.ControlText;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Invalid comparison between Unknown and I4
		Color infoText = SystemColors.InfoText;
		Color color = SystemColors.Info;
		Color color2 = Color.Empty;
		Color color3 = ColorScheme.GetColor("767676");
		if (_ColorDepth == -1)
		{
			_ColorDepth = Screen.FromControl((Control)(object)this).BitsPerPixel;
		}
		if (_ColorDepth < 8)
		{
			color = Color.White;
			infoText = Color.Black;
		}
		else if (m_Style == eDotNetBarStyle.Metro)
		{
			MetroColorTable colorTable = MetroRender.GetColorTable();
			color = colorTable.CanvasColor;
			infoText = colorTable.ForeColor;
		}
		else if (BarFunctions.IsOffice2007Style(m_Style) && ((Control)this).BackColor == SystemColors.Control)
		{
			color = ColorScheme.GetColor("FFFFFF");
			color2 = ColorScheme.GetColor("E4E4F0");
			infoText = ColorScheme.GetColor("4C4C4C");
		}
		else
		{
			infoText = GetToolTipColor();
			if (((Control)this).BackColor != SystemColors.Control)
			{
				color = ((Control)this).BackColor;
			}
		}
		Graphics graphics = e.Graphics;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		if (m_AntiAlias)
		{
			graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			graphics.SmoothingMode = (SmoothingMode)4;
		}
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)3;
		if (BarFunctions.IsOffice2007Style(m_Style))
		{
			DisplayHelp.DrawRectangle(graphics, color3, clientRectangle);
		}
		else
		{
			ControlPaint.DrawBorder3D(graphics, clientRectangle, (Border3DStyle)5, (Border3DSide)2063);
		}
		clientRectangle.Inflate(-1, -1);
		if (color2.IsEmpty)
		{
			DisplayHelp.FillRectangle(graphics, clientRectangle, color);
		}
		else
		{
			DisplayHelp.FillRectangle(graphics, clientRectangle, color, color2);
		}
		clientRectangle.Offset(1, 0);
		graphics.SmoothingMode = smoothingMode;
		if (m_TextMarkup == null)
		{
			TextDrawing.DrawString(graphics, GetDrawText(), ((Control)this).Font, infoText, clientRectangle, GetStringFormat());
			return;
		}
		MarkupDrawContext d = new MarkupDrawContext(graphics, ((Control)this).Font, infoText, (int)((Control)this).RightToLeft == 1, e.ClipRectangle, hotKeyPrefixVisible: true);
		m_TextMarkup.Bounds = new Rectangle(clientRectangle.Location, m_TextMarkup.Bounds.Size);
		m_TextMarkup.Render(d);
	}

	public void ShowToolTip()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Invalid comparison between Unknown and I4
		if (!((Control)this).IsHandleCreated)
		{
			((Control)this).CreateControl();
		}
		Size size = Size.Empty;
		Graphics val = ((Control)this).CreateGraphics();
		if (m_AntiAlias)
		{
			val.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			val.SmoothingMode = (SmoothingMode)4;
		}
		try
		{
			val.PageUnit = (GraphicsUnit)2;
			if (m_TextMarkup == null)
			{
				size = TextDrawing.MeasureString(val, GetDrawText(), ((Control)this).Font, Screen.PrimaryScreen.WorkingArea.Size, GetStringFormat());
			}
			else
			{
				MarkupDrawContext d = new MarkupDrawContext(val, ((Control)this).Font, SystemColors.Control, (int)((Control)this).RightToLeft == 1);
				m_TextMarkup.Measure(Screen.PrimaryScreen.WorkingArea.Size, d);
				size = m_TextMarkup.Bounds.Size;
				m_TextMarkup.Arrange(new Rectangle(Point.Empty, size), d);
			}
		}
		finally
		{
			val.SmoothingMode = (SmoothingMode)0;
			val.TextRenderingHint = (TextRenderingHint)0;
			val.Dispose();
		}
		val = null;
		Point mousePosition = Control.MousePosition;
		Rectangle rectangle = new Rectangle(Control.MousePosition.X, Control.MousePosition.Y, size.Width, size.Height);
		rectangle.Inflate(2, 2);
		rectangle.Offset(12, 24);
		ScreenInformation screenInformation = BarFunctions.ScreenFromPoint(mousePosition);
		if (screenInformation != null)
		{
			Size size2 = screenInformation.WorkingArea.Size;
			size2.Width -= (int)((float)size2.Width * 0.2f);
			if (rectangle.Right > screenInformation.WorkingArea.Right)
			{
				rectangle.X = rectangle.Left - (rectangle.Right - screenInformation.WorkingArea.Right);
			}
			if (rectangle.Bottom > screenInformation.Bounds.Bottom)
			{
				if (ReferenceRectangle.IsEmpty)
				{
					rectangle.Y = mousePosition.Y - rectangle.Height;
				}
				else
				{
					rectangle.Y = ReferenceRectangle.Y - rectangle.Height - 1;
				}
			}
			if (rectangle.Contains(Control.MousePosition.X, Control.MousePosition.Y))
			{
				if (rectangle.Height + Control.MousePosition.Y + 1 <= screenInformation.WorkingArea.Height && (ReferenceRectangle.IsEmpty || !ReferenceRectangle.IntersectsWith(new Rectangle(rectangle.X, Control.MousePosition.Y + 1, rectangle.Width, rectangle.Height))))
				{
					rectangle.Y = Control.MousePosition.Y + 1;
				}
				else
				{
					rectangle.Y = Control.MousePosition.Y - rectangle.Height - 1;
				}
			}
		}
		((Control)this).Location = rectangle.Location;
		((Control)this).ClientSize = rectangle.Size;
		if (NativeFunctions.ShowDropShadow)
		{
			if (m_DropShadow == null)
			{
				m_DropShadow = new PopupShadow(NativeFunctions.AlphaBlendingSupported);
				((Control)m_DropShadow).CreateControl();
			}
			((Control)m_DropShadow).Hide();
		}
		NativeFunctions.SetWindowPos(((Control)this).Handle, new IntPtr(0), 0, 0, 0, 0, 83);
		if (m_DropShadow != null)
		{
			NativeFunctions.SetWindowPos(((Control)m_DropShadow).Handle, ((Control)this).Handle, rectangle.Left + 5, rectangle.Top + 5, rectangle.Width - 2, rectangle.Height - 2, 80);
			m_DropShadow.UpdateShadow();
		}
	}

	private string GetDrawText()
	{
		string text = m_ToolTip.Replace("\\\\n", "{spec_nl}");
		text = text.Replace("\\n", Environment.NewLine);
		return text.Replace("{spec_nl}", "\\n");
	}

	private eTextFormat GetStringFormat()
	{
		return eTextFormat.VerticalCenter | eTextFormat.WordBreak;
	}

	private Color GetToolTipColor()
	{
		if (((Control)this).ForeColor != SystemColors.ControlText)
		{
			return ((Control)this).ForeColor;
		}
		Color result = SystemColors.WindowText;
		try
		{
			RegistryKey val = Registry.CurrentUser.OpenSubKey("Control Panel\\Colors\\", false);
			string text = val.GetValue("InfoText").ToString();
			val.Close();
			val = null;
			if (text == null || text == "")
			{
				return result;
			}
			char[] separator = new char[1] { ' ' };
			string[] array = text.Split(separator, 3);
			if (array.GetUpperBound(0) == 2)
			{
				result = Color.FromArgb(Convert.ToInt16(array[0]), Convert.ToInt16(array[1]), Convert.ToInt16(array[2]));
			}
		}
		catch (Exception)
		{
		}
		return result;
	}

	protected override void OnLocationChanged(EventArgs e)
	{
		((Control)this).OnLocationChanged(e);
		if (m_DropShadow != null)
		{
			NativeFunctions.SetWindowPos(((Control)m_DropShadow).Handle, new IntPtr(0), ((Control)this).Left + 5, ((Control)this).Top + 5, 0, 0, 81);
		}
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (!((Control)this).Visible && m_DropShadow != null)
		{
			((Control)m_DropShadow).Hide();
			((Component)(object)m_DropShadow).Dispose();
			m_DropShadow = null;
		}
		((Control)this).OnVisibleChanged(e);
	}
}
