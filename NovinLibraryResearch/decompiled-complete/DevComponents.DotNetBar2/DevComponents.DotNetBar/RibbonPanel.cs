using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[Designer("DevComponents.DotNetBar.Design.RibbonPanelDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(false)]
public class RibbonPanel : PanelControl, IKeyTipsControl
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

	private class _245D : IComparer
	{
		private int _1C24(object و, object ى)
		{
			if (و is RibbonBar && ى is RibbonBar)
			{
				return ((RibbonBar)و).ResizeOrderIndex - ((RibbonBar)ى).ResizeOrderIndex;
			}
			return new CaseInsensitiveComparer().Compare(و, ى);
		}

		int IComparer.Compare(object و, object ى)
		{
			//ILSpy generated this explicit interface implementation from .override directive in ᰤ
			return this._1C24(و, ى);
		}
	}

	private const string ᑙ = "Drop RibbonBar or other controls here. Drag and Drop tabs and items to re-order.";

	private RibbonTabItem ᰐ;

	private bool ᖔ;

	private bool ᰑ;

	private bool ᰒ;

	private ElementStyle ᚯ = new ElementStyle();

	private RightToLeft ೡ;

	private bool ᰓ;

	private ButtonX ᰔ;

	private ButtonX ᰕ;

	private Rectangle ᰖ = Rectangle.Empty;

	private int ᔁ;

	private bool ᰗ;

	private RibbonControl ᰘ;

	private bool _09FE;

	private string _0A00 = "";

	internal bool ᑗ => ᰗ;

	internal bool ᰏ
	{
		get
		{
			return ᰓ;
		}
		set
		{
			ᰓ = value;
		}
	}

	[Category("Layout")]
	[Description("Indicates whether default control layout is used instead of Rendering layout for RibbonBar controls positioning.")]
	[Browsable(true)]
	[DefaultValue(false)]
	public bool DefaultLayout
	{
		get
		{
			return ᰑ;
		}
		set
		{
			ᰑ = value;
			((Control)this).PerformLayout();
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether style of the panel is managed by tab control automatically. Set this to true if you would like to control style of the panel.")]
	[Category("Appearance")]
	public bool UseCustomStyle
	{
		get
		{
			return ᖔ;
		}
		set
		{
			ᖔ = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(null)]
	[Browsable(false)]
	public RibbonTabItem RibbonTabItem
	{
		get
		{
			return ᰐ;
		}
		set
		{
			ᰐ = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Layout")]
	[Description("Whether last RibbonBar is stretched to fill available space inside of the panel.")]
	public bool StretchLastRibbonBar
	{
		get
		{
			return ᰒ;
		}
		set
		{
			ᰒ = value;
			((Control)this).PerformLayout();
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

	[DefaultValue("")]
	[Browsable(false)]
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
			return _09FE;
		}
		set
		{
			_09FE = value;
			Control[] array = (Control[])(object)new Control[((ArrangedElementCollection)((Control)this).Controls).Count];
			((ArrangedElementCollection)((Control)this).Controls).CopyTo((Array)array, 0);
			Control[] array2 = array;
			foreach (Control val in array2)
			{
				if (val is IKeyTipsControl && val.Enabled && (val.Visible || !_09FE))
				{
					((IKeyTipsControl)val).ShowKeyTips = _09FE;
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

	public RibbonPanel()
	{
		((Control)this).BackColor = SystemColors.Control;
	}

	protected override ElementStyle GetStyle()
	{
		if (!Style.Custom)
		{
			return ᑡ();
		}
		return base.GetStyle();
	}

	internal RibbonControl ᯇ()
	{
		if (ᰗ)
		{
			return ᰘ;
		}
		return ((Control)this).Parent as RibbonControl;
	}

	internal void ᑞ(bool ᑟ, RibbonControl ᑠ)
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
		ᰗ = ᑟ;
		if (ᰗ)
		{
			ᰘ = ᑠ;
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
			ᰘ = null;
		}
	}

	private ElementStyle ᑡ()
	{
		RibbonControl ribbonControl = ᯇ();
		if (ribbonControl != null && base.ColorSchemeStyle != ribbonControl.Style)
		{
			base.ColorSchemeStyle = ribbonControl.Style;
		}
		ᚯ.SetColorScheme(base.ColorScheme);
		if (BarFunctions.IsOffice2007Style(base.ᑦ) || (ribbonControl != null && BarFunctions.IsOffice2007Style(ribbonControl.EffectiveStyle)))
		{
			ᚯ.Reset();
			Office2007ColorTable office2007ColorTable = null;
			if (ribbonControl != null)
			{
				office2007ColorTable = ribbonControl.ጪ();
			}
			else
			{
				if (!(GlobalManager.Renderer is Office2007Renderer))
				{
					return Style;
				}
				office2007ColorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
			}
			if (StyleManager.IsMetro(base.ᑦ) || (ribbonControl != null && StyleManager.IsMetro(ribbonControl.EffectiveStyle)))
			{
				ᚯ.Border = eStyleBorderType.Solid;
			}
			else
			{
				ᚯ.Border = eStyleBorderType.Double;
			}
			if (ribbonControl == null || !ribbonControl.IsPopupMode)
			{
				ᚯ.BorderTop = eStyleBorderType.None;
			}
			ᚯ.BorderWidth = 1;
			ᚯ.CornerDiameter = office2007ColorTable.RibbonControl.CornerSize;
			ᚯ.CornerType = eCornerType.Rounded;
			if (!ᰗ)
			{
				ᚯ.CornerTypeTopLeft = eCornerType.Square;
				ᚯ.CornerTypeTopRight = eCornerType.Square;
			}
			else if (office2007ColorTable.InitialColorScheme == eOffice2007ColorScheme.Black)
			{
				ᚯ.BorderTop = eStyleBorderType.None;
			}
			if (ribbonControl != null && (ribbonControl.EffectiveStyle == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(ribbonControl.EffectiveStyle)))
			{
				ᚯ.CornerTypeBottomRight = eCornerType.Square;
				ᚯ.CornerTypeBottomLeft = eCornerType.Square;
				if (ribbonControl.RibbonStrip.ষ)
				{
					ᚯ.BorderLeftWidth = 0;
					ᚯ.BorderRightWidth = 0;
					ᚯ.BorderLeft = eStyleBorderType.None;
					ᚯ.BorderRight = eStyleBorderType.None;
				}
				else if (StyleManager.IsMetro(ribbonControl.EffectiveStyle))
				{
					ᚯ.BorderLeft = eStyleBorderType.None;
					ᚯ.BorderRight = eStyleBorderType.None;
				}
				else
				{
					ᚯ.BorderLeft = eStyleBorderType.Solid;
					ᚯ.BorderRight = eStyleBorderType.Solid;
				}
			}
			else if (ribbonControl != null && ribbonControl.EffectiveStyle == eDotNetBarStyle.Windows7)
			{
				ᚯ.CornerType = eCornerType.Square;
				ᚯ.CornerTypeBottomRight = eCornerType.Square;
				ᚯ.CornerTypeBottomLeft = eCornerType.Square;
				ᚯ.CornerTypeTopRight = eCornerType.Square;
				ᚯ.CornerTypeTopLeft = eCornerType.Square;
				ᚯ.CornerDiameter = 0;
				if (ribbonControl.RibbonStrip.ষ)
				{
					ᚯ.BorderLeft = eStyleBorderType.None;
					ᚯ.BorderRight = eStyleBorderType.None;
				}
			}
			ᚯ.BorderColor = office2007ColorTable.RibbonControl.OuterBorder.Start;
			ᚯ.BorderColor2 = office2007ColorTable.RibbonControl.OuterBorder.End;
			if (office2007ColorTable.RibbonControl.InnerBorder != null)
			{
				ᚯ.BorderColorLight = office2007ColorTable.RibbonControl.InnerBorder.Start;
				ᚯ.BorderColorLight2 = office2007ColorTable.RibbonControl.InnerBorder.End;
				ᚯ.BorderBottom = eStyleBorderType.Double;
			}
			else
			{
				ᚯ.BorderBottom = eStyleBorderType.Solid;
			}
			ᚯ.BorderWidth = 1;
			ᚯ.BackColorGradientAngle = 90;
			ᚯ.BackColor = office2007ColorTable.RibbonBar.Default.TopBackground.Start;
			if (office2007ColorTable.RibbonControl.PanelTopBackgroundHeight == 0f && office2007ColorTable.RibbonControl.PanelBottomBackground == null)
			{
				ᚯ.BackColor = office2007ColorTable.RibbonControl.PanelTopBackground.Start;
				ᚯ.BackColor2 = office2007ColorTable.RibbonControl.PanelTopBackground.End;
			}
			else
			{
				ᚯ.BackColorBlend.Add(new BackgroundColorBlend(office2007ColorTable.RibbonControl.PanelTopBackground.Start, 0f));
				ᚯ.BackColorBlend.Add(new BackgroundColorBlend(office2007ColorTable.RibbonControl.PanelTopBackground.End, office2007ColorTable.RibbonControl.PanelTopBackgroundHeight));
				ᚯ.BackColorBlend.Add(new BackgroundColorBlend(office2007ColorTable.RibbonControl.PanelBottomBackground.Start, office2007ColorTable.RibbonControl.PanelTopBackgroundHeight));
				ᚯ.BackColorBlend.Add(new BackgroundColorBlend(office2007ColorTable.RibbonControl.PanelBottomBackground.End, 1f));
			}
		}
		else
		{
			ApplyLabelStyle(ᚯ);
			ᚯ.BorderBottom = eStyleBorderType.Solid;
		}
		return ᚯ;
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		if (!(e.Control is RibbonBar))
		{
			ᰑ = false;
		}
		base.OnControlAdded(e);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	protected override void OnLayout(LayoutEventArgs levent)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Invalid comparison between Unknown and I4
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		if (ᰑ || ((Component)(object)this).DesignMode)
		{
			((ScrollableControl)this).OnLayout(levent);
		}
		else
		{
			if (((Control)this).ClientRectangle.Width == 0 || ((Control)this).ClientRectangle.Height == 0 || !((Control)this).IsHandleCreated)
			{
				return;
			}
			Form val = ((Control)this).FindForm();
			if (val != null && (int)val.WindowState == 1)
			{
				return;
			}
			if (ೡ != ((Control)this).RightToLeft)
			{
				ೡ = ((Control)this).RightToLeft;
				Rectangle rectangle = ᰙ();
				foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
				{
					Control val2 = item;
					if (val2 is RibbonBar)
					{
						val2.Left = rectangle.Width - val2.Width - val2.Left;
					}
				}
			}
			if (!ᰓ)
			{
				LayoutRibbons();
			}
		}
	}

	private Rectangle ᰙ()
	{
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.X += ((ScrollableControl)this).DockPadding.Left;
		clientRectangle.Width -= ((ScrollableControl)this).DockPadding.Left + ((ScrollableControl)this).DockPadding.Right;
		clientRectangle.Y += ((ScrollableControl)this).DockPadding.Top;
		clientRectangle.Height -= ((ScrollableControl)this).DockPadding.Top + (((base.ᑦ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(base.ᑦ)) && ((ScrollableControl)this).DockPadding.Bottom == 3) ? 2 : ((ScrollableControl)this).DockPadding.Bottom);
		if (StyleManager.IsMetro(base.ᑦ) && ((ScrollableControl)this).DockPadding.All == 0)
		{
			clientRectangle.Y += 3;
			clientRectangle.Height -= 6;
		}
		return clientRectangle;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void LayoutRibbons()
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		int num = 2;
		Rectangle ᰜ = ᰙ();
		int num2 = -num;
		ArrayList arrayList = new ArrayList();
		ArrayList arrayList2 = new ArrayList();
		ArrayList arrayList3 = new ArrayList();
		Control val = null;
		int num3 = 0;
		bool flag = false;
		RibbonBar ribbonBar = null;
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val2 = item;
			if (val2 is RibbonBar ribbonBar2 && ((Control)ribbonBar2).Visible)
			{
				if (Dpi.Factor.Width > 1f)
				{
					ribbonBar2.RecalcLayout();
				}
				int width = ribbonBar2.ᅑ().Width;
				num2 += width + num;
				if (ribbonBar2.ჷ != null && ribbonBar2.ჷ.Visible)
				{
					ribbonBar = ribbonBar2;
				}
				if (ribbonBar2.ResizeOrderIndex != 0)
				{
					flag = true;
				}
				arrayList.Add(ribbonBar2);
				arrayList2.Add(ribbonBar2);
				if (ribbonBar2.OverflowState)
				{
					arrayList3.Add(ribbonBar2);
				}
				if (((Control)ribbonBar2).Left > num3)
				{
					num3 = ((Control)ribbonBar2).Left;
					val = val2;
				}
			}
		}
		arrayList2.Sort(new ᰣ());
		if (flag)
		{
			arrayList.Sort(new _245D());
			arrayList3.Sort(new _245D());
		}
		else
		{
			arrayList.Sort(new ᰣ());
			arrayList3.Sort(new ᰣ());
		}
		int ᰟ = 0;
		if (num2 > ᰜ.Width)
		{
			int num4 = num2 - ᰜ.Width;
			if (ribbonBar != null && !ribbonBar.ჷ.ᥦ)
			{
				int width2 = ribbonBar.ᅑ().Width;
				if (ribbonBar.ჷ.WidthInternal - ribbonBar.ჷ.MinimumSize.Width >= num4)
				{
					((Control)ribbonBar).Size = new Size(width2 - num4, ᰜ.Height);
				}
				else
				{
					((Control)ribbonBar).Size = new Size(width2 - (ribbonBar.ჷ.WidthInternal - ribbonBar.ჷ.MinimumSize.Width), ᰜ.Height);
				}
				num4 -= width2 - ribbonBar.ᅑ().Width;
			}
			if (num4 > 0)
			{
				int num5 = arrayList.Count - 1;
				for (int num6 = num5; num6 >= 0; num6--)
				{
					RibbonBar ribbonBar3 = arrayList[num6] as RibbonBar;
					int width3 = ribbonBar3.ᅑ().Width;
					if (width3 > num4 && ribbonBar3.AutoSizeItems)
					{
						((Control)ribbonBar3).Size = new Size(width3 - num4, ᰜ.Height);
					}
					else
					{
						((Control)ribbonBar3).Size = new Size(1, ᰜ.Height);
					}
					num4 -= width3 - ribbonBar3.ᅑ().Width;
					if (num4 <= 0)
					{
						break;
					}
				}
			}
			if (num4 != 0)
			{
				ᰟ = Math.Abs(num4);
			}
		}
		else if (num2 < ᰜ.Width)
		{
			int num7 = ᰜ.Width - num2;
			ArrayList arrayList4 = new ArrayList();
			arrayList4.AddRange(arrayList3.ToArray());
			foreach (RibbonBar item2 in arrayList4)
			{
				int width4 = item2.ᅑ().Width;
				((Control)item2).Size = new Size(width4 + num7, ᰜ.Height);
				num7 -= item2.ᅑ().Width - width4;
				if (num7 >= 0 && !item2.OverflowState)
				{
					arrayList3.Remove(item2);
				}
				if (num7 <= 0 || item2.ᅑ().Width - width4 == 0)
				{
					break;
				}
			}
			if (num7 > 0 && arrayList3.Count == 0)
			{
				int num8 = arrayList.Count - 1;
				for (int num9 = num8; num9 >= 0; num9--)
				{
					RibbonBar ribbonBar5 = arrayList[num9] as RibbonBar;
					if (!ribbonBar5.OverflowState && !arrayList4.Contains(ribbonBar5))
					{
						int width5 = ribbonBar5.ᅑ().Width;
						if (width5 != ((Control)ribbonBar5).Width || ((Control)ribbonBar5).Height != ᰜ.Height || !ribbonBar5.ჹ.IsEmpty || !ribbonBar5.ჸ.IsEmpty || ribbonBar5.ჷ != null)
						{
							if (((Control)ribbonBar5).Width == width5 + num7)
							{
								if (ribbonBar5 != val || !ᰒ)
								{
									num7 = 0;
								}
								break;
							}
							((Control)ribbonBar5).Size = new Size(width5 + num7, ᰜ.Height);
							num7 -= ribbonBar5.ᅑ().Width - width5;
							if (num7 <= 0)
							{
								break;
							}
						}
					}
				}
			}
			if (num7 > 0)
			{
				ᰟ = num7;
			}
		}
		ᰚ(arrayList2, ᰜ, num, val, ᰟ);
	}

	private void ᰚ(ArrayList ᰛ, Rectangle ᰜ, int ᰝ, Control ᰞ, int ᰟ)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		Point location = ᰜ.Location;
		int num = 0;
		if ((int)((Control)this).RightToLeft == 1)
		{
			int num2 = 0;
			foreach (RibbonBar item in ᰛ)
			{
				num2 += item.ᅑ().Width + ᰝ;
			}
			if (num2 < ᰜ.Width)
			{
				num = ᰝ + ᰜ.Width - num2;
			}
		}
		Rectangle b = Rectangle.Empty;
		location.X += ᔁ;
		foreach (RibbonBar item2 in ᰛ)
		{
			Size size = item2.ᅑ();
			if (item2 == ᰞ && ᰒ && !item2.OverflowState)
			{
				size.Width += ᰟ;
			}
			Rectangle rectangle = new Rectangle(location.X + num, location.Y, size.Width, ᰜ.Height);
			if (((Component)(object)this).DesignMode)
			{
				TypeDescriptor.GetProperties(item2)["Bounds"].SetValue(item2, rectangle);
			}
			else
			{
				((Control)item2).Bounds = rectangle;
			}
			b = Rectangle.Union(rectangle, b);
			location.X += ((Control)item2).Width + ᰝ;
		}
		ᰖ = b;
		if (ᰖ.Right > ((Control)this).Width)
		{
			if (ᰔ == null)
			{
				ᰔ = new ButtonX();
				((Control)ᰔ).Text = "<expand direction=\"right\"/>";
				ᰔ.ColorTable = eButtonColor.OrangeWithBackground;
				((Control)ᰔ).Click += ᰠ;
			}
			((Control)ᰔ).Bounds = new Rectangle(((Control)this).Width - 12, 0, 12, ((Control)this).Height - 2);
			((Control)this).Controls.Add((Control)(object)ᰔ);
			((Control)ᰔ).BringToFront();
		}
		else if (ᰔ != null)
		{
			((Control)ᰔ).Visible = false;
			((Control)this).Controls.Remove((Control)(object)ᰔ);
			((Component)(object)ᰔ).Dispose();
			ᰔ = null;
		}
		if (ᔁ < 0)
		{
			if (ᰕ == null)
			{
				ᰕ = new ButtonX();
				((Control)ᰕ).Text = "<expand direction=\"left\"/>";
				ᰕ.ColorTable = eButtonColor.OrangeWithBackground;
				((Control)ᰕ).Click += ᰡ;
			}
			((Control)ᰕ).Bounds = new Rectangle(0, 0, 12, ((Control)this).Height - 2);
			((Control)this).Controls.Add((Control)(object)ᰕ);
			((Control)ᰕ).BringToFront();
		}
		else if (ᰕ != null)
		{
			((Control)ᰕ).Visible = false;
			((Control)this).Controls.Remove((Control)(object)ᰕ);
			((Component)(object)ᰕ).Dispose();
			ᰕ = null;
		}
	}

	private void ᰠ(object _٠, EventArgs ؿ)
	{
		ScrollRight();
	}

	public void ScrollRight()
	{
		if (ᔁ + ᰖ.Width > ((Control)this).Width)
		{
			ᰢ(-Math.Min(((Control)this).Width, ᰖ.Right - ((Control)this).Width + 2));
		}
	}

	private void ᰡ(object _٠, EventArgs ؿ)
	{
		ScrollLeft();
	}

	public void ScrollLeft()
	{
		ᰢ(Math.Min(Math.Abs(ᔁ), ((Control)this).Width));
	}

	public void ResetScrollPosition()
	{
		ᰢ(-ᔁ);
	}

	private void ᰢ(int _066A)
	{
		ᔁ += _066A;
		((Control)this).PerformLayout();
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
			eTabStripAlignment eTabStripAlignment2 = eTabStripAlignment.Top;
			Rectangle ल = new Rectangle(0, 0, rectangle.Width, rectangle.Height);
			if (eTabStripAlignment2 == eTabStripAlignment.Right || eTabStripAlignment2 == eTabStripAlignment.Left)
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
					switch (eTabStripAlignment2)
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
			e.Graphics.DrawString("Drop RibbonBar or other controls here. Drag and Drop tabs and items to re-order.", val5, (Brush)new SolidBrush(ControlPaint.Dark(Style.BackColor)), (RectangleF)clientRectangle, val4);
			val5.Dispose();
			val4.Dispose();
		}
		if (((Control)this).Parent is RibbonControl)
		{
			((RibbonControl)(object)((Control)this).Parent).RibbonStrip.ગ();
		}
	}

	protected override void OnResize(EventArgs e)
	{
		ᔁ = 0;
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
}
