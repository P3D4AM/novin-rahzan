using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.Metro.Rendering;

namespace DevComponents.DotNetBar.Metro;

[Designer("DevComponents.DotNetBar.Design.MetroTabPanelDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(false)]
public class MetroTabPanel : PanelControl, IKeyTipsControl
{
	private class ᰣ : IComparer
	{
		private int _1C24(object و, object ى)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			if (و is Control && ى is Control)
			{
				return ((Control)و).Left - ((Control)ى).Left;
			}
			return new CaseInsensitiveComparer().Compare(و, ى);
		}

		int IComparer.Compare(object و, object ى)
		{
			//ILSpy generated this explicit interface implementation from .override directive in ᰤ
			return this._1C24(و, ى);
		}
	}

	private const string ᑙ = "Drop controls here. Drag and Drop tabs and items to re-order.";

	private MetroTabItem _ᑚ;

	private bool _ᑛ;

	private bool _ᑜ;

	private MetroShell _Ꮀ;

	private bool _ᑝ;

	private string _0A00 = "";

	internal bool ᑗ => _ᑜ;

	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates whether style of the panel is managed by tab control automatically. Set this to true if you would like to control style of the panel.")]
	[DefaultValue(false)]
	public bool UseCustomStyle
	{
		get
		{
			return _ᑛ;
		}
		set
		{
			_ᑛ = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(null)]
	[Browsable(false)]
	public MetroTabItem MetroTabItem
	{
		get
		{
			return _ᑚ;
		}
		set
		{
			_ᑚ = value;
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

	bool IKeyTipsControl.ᑘ
	{
		get
		{
			return _ᑝ;
		}
		set
		{
			_ᑝ = value;
			Control[] array = (Control[])(object)new Control[((ArrangedElementCollection)((Control)this).Controls).Count];
			((ArrangedElementCollection)((Control)this).Controls).CopyTo((Array)array, 0);
			Control[] array2 = array;
			foreach (Control val in array2)
			{
				if (val is IKeyTipsControl && val.Enabled && (val.Visible || !_ᑝ))
				{
					((IKeyTipsControl)val).ShowKeyTips = _ᑝ;
				}
			}
		}
	}

	string IKeyTipsControl._09B5
	{
		get
		{
			return _0A00;
		}
		set
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			_0A00 = value;
			foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
			{
				Control val = item;
				if (val is IKeyTipsControl && val.Visible && val.Enabled)
				{
					((IKeyTipsControl)val).KeyTipsKeysStack = _0A00;
				}
			}
		}
	}

	public MetroTabPanel()
	{
		base.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		if (StyleManager.IsMetro(StyleManager.Style))
		{
			((Control)this).BackColor = MetroRender.GetColorTable().CanvasColor;
		}
	}

	protected override ElementStyle GetStyle()
	{
		if (!Style.Custom)
		{
			return ᑡ();
		}
		return base.GetStyle();
	}

	internal MetroShell ᐩ()
	{
		if (_ᑜ)
		{
			return _Ꮀ;
		}
		return ((Control)this).Parent as MetroShell;
	}

	internal void ᑞ(bool ᑟ, MetroShell ᑠ)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		_ᑜ = ᑟ;
		if (_ᑜ)
		{
			_Ꮀ = ᑠ;
			Padding padding = ((Control)this).Padding;
			if (((Padding)(ref padding)).Bottom > 0)
			{
				int height = ((Control)this).Height;
				Padding padding2 = ((Control)this).Padding;
				((Control)this).Height = height + ((Padding)(ref padding2)).Bottom;
				Padding padding3 = ((Control)this).Padding;
				int left = ((Padding)(ref padding3)).Left;
				Padding padding4 = ((Control)this).Padding;
				int bottom = ((Padding)(ref padding4)).Bottom;
				Padding padding5 = ((Control)this).Padding;
				int right = ((Padding)(ref padding5)).Right;
				Padding padding6 = ((Control)this).Padding;
				((Control)this).Padding = new Padding(left, bottom, right, ((Padding)(ref padding6)).Bottom);
			}
		}
		else
		{
			Padding padding7 = ((Control)this).Padding;
			if (((Padding)(ref padding7)).Top > 0)
			{
				int height2 = ((Control)this).Height;
				Padding padding8 = ((Control)this).Padding;
				((Control)this).Height = height2 - ((Padding)(ref padding8)).Top;
				Padding padding9 = ((Control)this).Padding;
				int left2 = ((Padding)(ref padding9)).Left;
				Padding padding10 = ((Control)this).Padding;
				int right2 = ((Padding)(ref padding10)).Right;
				Padding padding11 = ((Control)this).Padding;
				((Control)this).Padding = new Padding(left2, 0, right2, ((Padding)(ref padding11)).Bottom);
			}
			_Ꮀ = null;
		}
	}

	private ElementStyle ᑡ()
	{
		return MetroRender.GetColorTable().MetroTab.TabPanelBackgroundStyle;
	}

	private Rectangle ᑢ(Rectangle ल)
	{
		ल.Y -= 6;
		ल.Height += 6;
		return ल;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Expected O, but got Unknown
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		bool flag = true;
		if (ᑲ && BarFunctions.ThemedOS)
		{
			Rectangle rectangle = ᑢ(((Control)this).ClientRectangle);
			eTabStripAlignment eTabStripAlignment = eTabStripAlignment.Top;
			Rectangle ल = new Rectangle(0, 0, rectangle.Width, rectangle.Height);
			if (eTabStripAlignment == eTabStripAlignment.Right || eTabStripAlignment == eTabStripAlignment.Left)
			{
				ल = new Rectangle(0, 0, ल.Height, ल.Width);
			}
			if (m_ThemeCachedBitmap == null || ((Image)m_ThemeCachedBitmap).Size != ल.Size)
			{
				DisposeThemeCachedBitmap();
				Bitmap val = new Bitmap(ल.Width, ल.Height, e.Graphics);
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
						base.ᑧ._0EE5(val2, ᑷ.ᒀ, ᒂ.ཊ, ल);
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
					e.Graphics.DrawImageUnscaled((Image)(object)val, rectangle.X, rectangle.Y);
					m_ThemeCachedBitmap = val;
				}
			}
			else
			{
				e.Graphics.DrawImageUnscaled((Image)(object)m_ThemeCachedBitmap, rectangle.X, rectangle.Y);
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
			((MetroShell)(object)((Control)this).Parent).MetroTabStrip.ગ();
		}
	}

	protected override void OnResize(EventArgs e)
	{
		DisposeThemeCachedBitmap();
		base.OnResize(e);
	}

	private bool ᑣ(char ઈ)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		if (((ArrangedElementCollection)((Control)this).Controls).Count == 0)
		{
			return false;
		}
		Control[] array = (Control[])(object)new Control[((ArrangedElementCollection)((Control)this).Controls).Count];
		((ArrangedElementCollection)((Control)this).Controls).CopyTo((Array)array, 0);
		ArrayList arrayList = new ArrayList(array);
		arrayList.Sort(new ᰣ());
		foreach (Control item in arrayList)
		{
			Control val = item;
			if (val is IKeyTipsControl keyTipsControl && val.Visible && val.Enabled)
			{
				string keyTipsKeysStack = keyTipsControl.KeyTipsKeysStack;
				if (keyTipsControl.ProcessMnemonicEx(ઈ))
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

	bool IKeyTipsControl.ProcessMnemonicEx(char ઈ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᑣ
		return this.ᑣ(ઈ);
	}

	protected override void OnLayout(LayoutEventArgs levent)
	{
		((ScrollableControl)this).OnLayout(levent);
	}
}
