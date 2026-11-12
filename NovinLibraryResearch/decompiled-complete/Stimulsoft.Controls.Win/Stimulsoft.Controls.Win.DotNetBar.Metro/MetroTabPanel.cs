using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro;

[ToolboxItem(false)]
public class MetroTabPanel : PanelControl, IKeyTipsControl
{
	private class XPositionComparer : IComparer
	{
		int IComparer.Compare(object x, object y)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			if (x is Control && y is Control)
			{
				return ((Control)x).Left - ((Control)y).Left;
			}
			return new CaseInsensitiveComparer().Compare(x, y);
		}
	}

	private const string INFO_TEXT = "Drop controls here. Drag and Drop tabs and items to re-order.";

	private MetroTabItem _TabItem;

	private bool _UseCustomStyle;

	private bool _PopupMode;

	private MetroShell _MetroTab;

	private bool _ShowKeyTips;

	private string m_KeyTipsKeysStack = "";

	internal bool IsPopupMode => _PopupMode;

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether style of the panel is managed by tab control automatically. Set this to true if you would like to control style of the panel.")]
	public bool UseCustomStyle
	{
		get
		{
			return _UseCustomStyle;
		}
		set
		{
			_UseCustomStyle = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public MetroTabItem MetroTabItem
	{
		get
		{
			return _TabItem;
		}
		set
		{
			_TabItem = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public override DockStyle Dock
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Dock;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Dock = value;
		}
	}

	[Browsable(false)]
	public Size Size
	{
		get
		{
			return ((Control)this).Size;
		}
		set
		{
			((Control)this).Size = value;
		}
	}

	[Browsable(false)]
	public Point Location
	{
		get
		{
			return ((Control)this).Location;
		}
		set
		{
			((Control)this).Location = value;
		}
	}

	[Browsable(false)]
	public bool Visible
	{
		get
		{
			return ((Control)this).Visible;
		}
		set
		{
			((Control)this).Visible = value;
		}
	}

	[Browsable(false)]
	public override AnchorStyles Anchor
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Anchor;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Anchor = value;
		}
	}

	[Browsable(false)]
	[DefaultValue("")]
	public override string Text
	{
		get
		{
			return base.Text;
		}
		set
		{
			base.Text = value;
		}
	}

	bool IKeyTipsControl.ShowKeyTips
	{
		get
		{
			return _ShowKeyTips;
		}
		set
		{
			_ShowKeyTips = value;
			Control[] array = (Control[])(object)new Control[((ArrangedElementCollection)((Control)this).Controls).Count];
			((ArrangedElementCollection)((Control)this).Controls).CopyTo((Array)array, 0);
			Control[] array2 = array;
			foreach (Control val in array2)
			{
				if (val is IKeyTipsControl && val.Enabled && (val.Visible || !_ShowKeyTips))
				{
					((IKeyTipsControl)val).ShowKeyTips = _ShowKeyTips;
				}
			}
		}
	}

	string IKeyTipsControl.KeyTipsKeysStack
	{
		get
		{
			return m_KeyTipsKeysStack;
		}
		set
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			m_KeyTipsKeysStack = value;
			foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
			{
				Control val = item;
				if (val is IKeyTipsControl && val.Visible && val.Enabled)
				{
					((IKeyTipsControl)val).KeyTipsKeysStack = m_KeyTipsKeysStack;
				}
			}
		}
	}

	public MetroTabPanel()
	{
		base.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		if (StyleManager.Style == eStyle.Metro)
		{
			((Control)this).BackColor = MetroRender.GetColorTable().CanvasColor;
		}
	}

	protected override ElementStyle GetStyle()
	{
		if (!Style.Custom)
		{
			return GetDefaultStyle();
		}
		return base.GetStyle();
	}

	internal MetroShell GetMetroTab()
	{
		if (_PopupMode)
		{
			return _MetroTab;
		}
		return ((Control)this).Parent as MetroShell;
	}

	internal void SetPopupMode(bool popupMode, MetroShell rc)
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		_PopupMode = popupMode;
		Padding padding;
		if (_PopupMode)
		{
			_MetroTab = rc;
			padding = ((Control)this).Padding;
			if (((Padding)(ref padding)).Bottom > 0)
			{
				int height = ((Control)this).Height;
				padding = ((Control)this).Padding;
				((Control)this).Height = height + ((Padding)(ref padding)).Bottom;
				padding = ((Control)this).Padding;
				int left = ((Padding)(ref padding)).Left;
				padding = ((Control)this).Padding;
				int bottom = ((Padding)(ref padding)).Bottom;
				padding = ((Control)this).Padding;
				int right = ((Padding)(ref padding)).Right;
				padding = ((Control)this).Padding;
				((Control)this).Padding = new Padding(left, bottom, right, ((Padding)(ref padding)).Bottom);
			}
		}
		else
		{
			padding = ((Control)this).Padding;
			if (((Padding)(ref padding)).Top > 0)
			{
				int height2 = ((Control)this).Height;
				padding = ((Control)this).Padding;
				((Control)this).Height = height2 - ((Padding)(ref padding)).Top;
				padding = ((Control)this).Padding;
				int left2 = ((Padding)(ref padding)).Left;
				padding = ((Control)this).Padding;
				int right2 = ((Padding)(ref padding)).Right;
				padding = ((Control)this).Padding;
				((Control)this).Padding = new Padding(left2, 0, right2, ((Padding)(ref padding)).Bottom);
			}
			_MetroTab = null;
		}
	}

	private ElementStyle GetDefaultStyle()
	{
		return MetroRender.GetColorTable().MetroTab.TabPanelBackgroundStyle;
	}

	private Rectangle GetThemedRect(Rectangle r)
	{
		r.Y -= 6;
		r.Height += 6;
		return r;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		bool flag = true;
		if (DrawThemedPane && BarFunctions.ThemedOS)
		{
			Rectangle themedRect = GetThemedRect(((Control)this).ClientRectangle);
			eTabStripAlignment eTabStripAlignment = eTabStripAlignment.Top;
			Rectangle r = new Rectangle(0, 0, themedRect.Width, themedRect.Height);
			if (eTabStripAlignment == eTabStripAlignment.Right || eTabStripAlignment == eTabStripAlignment.Left)
			{
				r = new Rectangle(0, 0, r.Height, r.Width);
			}
			if (m_ThemeCachedBitmap == null || ((Image)m_ThemeCachedBitmap).Size != r.Size)
			{
				DisposeThemeCachedBitmap();
				Bitmap val = new Bitmap(r.Width, r.Height, e.Graphics);
				try
				{
					Graphics val2 = Graphics.FromImage((Image)(object)val);
					try
					{
						SolidBrush val3 = new SolidBrush(Color.Transparent);
						try
						{
							val2.FillRectangle((Brush)(object)val3, 0, 0, ((Image)val).Width, ((Image)val).Height);
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
						base.ThemeTab.DrawBackground(val2, ThemeTabParts.Pane, ThemeTabStates.Normal, r);
					}
					finally
					{
						val2.Dispose();
					}
				}
				finally
				{
					switch (eTabStripAlignment)
					{
					case eTabStripAlignment.Left:
						((Image)val).RotateFlip((RotateFlipType)3);
						break;
					case eTabStripAlignment.Right:
						((Image)val).RotateFlip((RotateFlipType)1);
						break;
					case eTabStripAlignment.Bottom:
						((Image)val).RotateFlip((RotateFlipType)2);
						break;
					}
					e.Graphics.DrawImageUnscaled((Image)(object)val, themedRect.X, themedRect.Y);
					m_ThemeCachedBitmap = val;
				}
			}
			else
			{
				e.Graphics.DrawImageUnscaled((Image)(object)m_ThemeCachedBitmap, themedRect.X, themedRect.Y);
			}
			flag = false;
		}
		if (flag)
		{
			base.OnPaint(e);
		}
		if (((Component)(object)this).DesignMode && ((ArrangedElementCollection)((Control)this).Controls).Count == 0 && ((Control)this).Text == "")
		{
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			clientRectangle.Inflate(-2, -2);
			StringFormat val4 = BarFunctions.CreateStringFormat();
			val4.Alignment = (StringAlignment)1;
			val4.LineAlignment = (StringAlignment)1;
			val4.Trimming = (StringTrimming)3;
			Font val5 = new Font(((Control)this).Font, (FontStyle)1);
			e.Graphics.DrawString("Drop controls here. Drag and Drop tabs and items to re-order.", val5, (Brush)new SolidBrush(ControlPaint.Dark(Style.BackColor)), (RectangleF)clientRectangle, val4);
			val5.Dispose();
			val4.Dispose();
		}
		if (((Control)this).Parent is MetroShell)
		{
			((MetroShell)(object)((Control)this).Parent).MetroTabStrip.InvalidateKeyTipsCanvas();
		}
	}

	protected override void OnResize(EventArgs e)
	{
		DisposeThemeCachedBitmap();
		base.OnResize(e);
	}

	bool IKeyTipsControl.ProcessMnemonicEx(char charCode)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		if (((ArrangedElementCollection)((Control)this).Controls).Count == 0)
		{
			return false;
		}
		Control[] array = (Control[])(object)new Control[((ArrangedElementCollection)((Control)this).Controls).Count];
		((ArrangedElementCollection)((Control)this).Controls).CopyTo((Array)array, 0);
		ArrayList arrayList = new ArrayList(array);
		arrayList.Sort(new XPositionComparer());
		foreach (Control item in arrayList)
		{
			Control val = item;
			if (val is IKeyTipsControl keyTipsControl && val.Visible && val.Enabled)
			{
				string keyTipsKeysStack = keyTipsControl.KeyTipsKeysStack;
				if (keyTipsControl.ProcessMnemonicEx(charCode))
				{
					return true;
				}
				if (keyTipsControl.KeyTipsKeysStack != keyTipsKeysStack)
				{
					((IKeyTipsControl)this).KeyTipsKeysStack = keyTipsControl.KeyTipsKeysStack;
					return false;
				}
			}
		}
		return false;
	}

	protected override void OnLayout(LayoutEventArgs levent)
	{
		((ScrollableControl)this).OnLayout(levent);
	}
}
