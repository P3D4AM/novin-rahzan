using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace DevComponents.DotNetBar;

[ToolboxItem(true)]
[ComVisible(false)]
[Designer("DevComponents.DotNetBar.Design.NavigationBarDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class NavigationBar : BarBaseControl, ISupportInitialize
{
	private const int ᯚ = 6;

	private NavigationBarContainer ଡ଼;

	private bool ᯛ;

	private Cursor ᯜ;

	private bool ᯝ;

	private EventHandler ᯞ;

	private bool ᖁ;

	private static RoundRectangleShapeDescriptor _ᯟ = new RoundRectangleShapeDescriptor();

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection Items => ଡ଼.SubItems;

	[Category("Behavior")]
	[Description("Indicates Configure Buttons button is visible.")]
	[Browsable(true)]
	[DefaultValue(true)]
	[DevCoBrowsable(true)]
	public bool ConfigureItemVisible
	{
		get
		{
			return ଡ଼.ConfigureItemVisible;
		}
		set
		{
			ଡ଼.ConfigureItemVisible = value;
			RecalcLayout();
		}
	}

	[Description("Indicates whether Show More Buttons and Show Fewer Buttons menu items are visible on Configure buttons menu.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public bool ConfigureShowHideVisible
	{
		get
		{
			return ଡ଼.ConfigureShowHideVisible;
		}
		set
		{
			ଡ଼.ConfigureShowHideVisible = value;
			RecalcLayout();
		}
	}

	[Description("Gets or sets whether Navigation Pane Options menu item is visible on Configure buttons menu.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public bool ConfigureNavOptionsVisible
	{
		get
		{
			return ଡ଼.ConfigureNavOptionsVisible;
		}
		set
		{
			ଡ଼.ConfigureNavOptionsVisible = value;
			RecalcLayout();
		}
	}

	[Description("Indicates whether Navigation Pane Add/Remove Buttons menu item is visible on Configure buttons menu.")]
	[DefaultValue(true)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	public bool ConfigureAddRemoveVisible
	{
		get
		{
			return ଡ଼.ConfigureAddRemoveVisible;
		}
		set
		{
			ଡ଼.ConfigureAddRemoveVisible = value;
			RecalcLayout();
		}
	}

	[Category("Behavior")]
	[Description("Indicates whether summary line is visible.")]
	[Browsable(true)]
	[DefaultValue(true)]
	[DevCoBrowsable(true)]
	public bool SummaryLineVisible
	{
		get
		{
			return ଡ଼.SummaryLineVisible;
		}
		set
		{
			ଡ଼.SummaryLineVisible = value;
			RecalcLayout();
		}
	}

	[Description("Indicates the padding in pixels at the top portion of the item.")]
	[DefaultValue(4)]
	[Category("Layout")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public int ItemPaddingTop
	{
		get
		{
			return ଡ଼.ItemPaddingTop;
		}
		set
		{
			ଡ଼.ItemPaddingTop = value;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Description("Indicates the padding in pixels at the bottom of the item.")]
	[DefaultValue(4)]
	[Browsable(true)]
	[Category("Layout")]
	[DevCoBrowsable(true)]
	public int ItemPaddingBottom
	{
		get
		{
			return ଡ଼.ItemPaddingBottom;
		}
		set
		{
			ଡ଼.ItemPaddingBottom = value;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Browsable(false)]
	public NavigationBarContainer ItemsContainer => ଡ଼;

	[Browsable(false)]
	public ButtonItem CheckedButton
	{
		get
		{
			foreach (BaseItem subItem in ଡ଼.SubItems)
			{
				if (subItem is ButtonItem && ((ButtonItem)subItem).Checked)
				{
					return subItem as ButtonItem;
				}
			}
			return null;
		}
	}

	[Description("Indicates whether images are automatically resized to size specified in ImageSizeSummaryLine when button is on the bottom summary line of navigation bar.")]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Browsable(true)]
	public bool AutoSizeButtonImage
	{
		get
		{
			return ଡ଼.AutoSizeButtonImage;
		}
		set
		{
			ଡ଼.AutoSizeButtonImage = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates size of the image that will be use to resize images to when button button is on the bottom summary line of navigation bar and AutoSizeButtonImage=true.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public Size ImageSizeSummaryLine
	{
		get
		{
			return ଡ଼.ImageSizeSummaryLine;
		}
		set
		{
			ଡ଼.ImageSizeSummaryLine = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public string LayoutDefinition
	{
		get
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlElement xmlElement = xmlDocument.CreateElement("navbarlayout");
			xmlDocument.AppendChild(xmlElement);
			SaveLayout(xmlElement);
			return xmlDocument.OuterXml;
		}
		set
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(value);
			LoadLayout(xmlDocument.FirstChild as XmlElement);
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Specifies the visual style of the control.")]
	[DefaultValue(eDotNetBarStyle.Office2003)]
	public eDotNetBarStyle Style
	{
		get
		{
			return ଡ଼.Style;
		}
		set
		{
			base.ColorScheme.ߕ = value;
			ଡ଼.Style = value;
			ᯡ();
			RecalcLayout();
		}
	}

	internal IShapeDescriptor ᯙ => _ᯟ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool IsSplitterMouseDown => ᯝ;

	[DefaultValue(false)]
	[Browsable(true)]
	[Description("Indicates whether splitter on top of the navigation bar is visible.")]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	public bool SplitterVisible
	{
		get
		{
			return ᯛ;
		}
		set
		{
			ᯛ = value;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	public event EventHandler OptionsDialogClosed
	{
		add
		{
			EventHandler eventHandler = ᯞ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᯞ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᯞ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᯞ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public NavigationBar()
	{
		ଡ଼ = new NavigationBarContainer();
		ଡ଼.GlobalItem = false;
		ଡ଼.ContainerControl = this;
		ଡ଼.Stretch = false;
		ଡ଼.Displayed = true;
		ଡ଼.Style = eDotNetBarStyle.Office2003;
		ଡ଼.SetOwner(this);
		SetBaseItemContainer(ଡ଼);
		SetDesignTimeDefaults();
	}

	internal void ᯠ()
	{
		OnOptionsDialogClosed(new EventArgs());
	}

	protected virtual void OnOptionsDialogClosed(EventArgs e)
	{
		ᯞ?.Invoke(this, e);
	}

	protected override void OnResize(EventArgs e)
	{
		((Control)this).OnResize(e);
		if (((Control)this).Width != 0 && ((Control)this).Height != 0)
		{
			RecalcSize();
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);
		RecalcSize();
	}

	protected override void RecalcSize()
	{
		if (ᖁ || !BarFunctions.IsHandleValid((Control)(object)this))
		{
			return;
		}
		base.RecalcSize();
		if (!ଡ଼.IsRecalculatingSize)
		{
			Rectangle itemContainerRectangle = GetItemContainerRectangle();
			if (ଡ଼.HeightInternal > 0)
			{
				((Control)this).Height = ଡ଼.HeightInternal + itemContainerRectangle.Top + Math.Max(0, ((Control)this).ClientRectangle.Bottom - itemContainerRectangle.Bottom);
			}
		}
	}

	public override void SetDesignTimeDefaults()
	{
		base.SetDesignTimeDefaults();
		ᯡ();
		base.BackgroundStyle.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		base.BackgroundStyle.Border = eBorderType.SingleLine;
	}

	private void ᯡ()
	{
		base.ColorScheme.ItemHotBorder = Color.Empty;
		base.ColorScheme.ItemBackground = base.ColorScheme.BarBackground;
		base.ColorScheme.ItemBackground2 = base.ColorScheme.BarBackground2;
		base.ColorScheme.ItemHotBorder = Color.Empty;
		base.ColorScheme.ItemPressedBorder = Color.Empty;
		base.ColorScheme.ItemCheckedBorder = Color.Empty;
		base.ColorScheme.ItemExpandedBackground = base.ColorScheme.ItemHotBackground;
		base.ColorScheme.ItemExpandedBackground2 = base.ColorScheme.ItemHotBackground2;
		base.ColorScheme.ItemExpandedShadow = Color.Empty;
		base.ColorScheme.ItemExpandedBorder = Color.Empty;
		base.ColorScheme.ResetChangedFlag();
		base.BackgroundStyle.ᗅ(base.ColorScheme);
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		Application.DoEvents();
		base.ColorScheme.Refresh();
		ᯡ();
	}

	public void ShowMoreButtons()
	{
		ଡ଼.ShowMoreButtons();
	}

	public void ShowFewerButtons()
	{
		ଡ଼.ShowFewerButtons();
	}

	protected override Rectangle GetItemContainerRectangle()
	{
		Rectangle itemContainerRectangle = base.GetItemContainerRectangle();
		if (ᯛ)
		{
			itemContainerRectangle.Y += 6;
			itemContainerRectangle.Height -= 6;
		}
		return itemContainerRectangle;
	}

	protected override void PaintContentOnTop(ItemPaintArgs pa)
	{
		base.PaintContentOnTop(pa);
		PaintSplitter(pa);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeImageSizeSummaryLine()
	{
		if (ଡ଼.ImageSizeSummaryLine.Width == 16)
		{
			return ଡ଼.ImageSizeSummaryLine.Height != 16;
		}
		return true;
	}

	private void ህ()
	{
		ᖁ = true;
	}

	void ISupportInitialize.BeginInit()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ህ
		this.ህ();
	}

	private void ሆ()
	{
		ᖁ = false;
		RecalcLayout();
	}

	void ISupportInitialize.EndInit()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ሆ
		this.ሆ();
	}

	public void SaveLayout(string fileName)
	{
		XmlDocument xmlDocument = new XmlDocument();
		XmlElement xmlElement = xmlDocument.CreateElement("navbarlayout");
		xmlDocument.AppendChild(xmlElement);
		SaveLayout(xmlElement);
		xmlDocument.Save(fileName);
	}

	public void SaveLayout(XmlElement xmlParent)
	{
		if (Items.Count == 0)
		{
			return;
		}
		XmlElement xmlElement = xmlParent.OwnerDocument.CreateElement("navigationbar");
		xmlParent.AppendChild(xmlElement);
		xmlElement.SetAttribute("height", ((Control)this).Height.ToString());
		foreach (BaseItem item in Items)
		{
			if (item is ButtonItem && item.Name != "")
			{
				XmlElement xmlElement2 = xmlParent.OwnerDocument.CreateElement("item");
				xmlElement.AppendChild(xmlElement2);
				xmlElement2.SetAttribute("index", Items.IndexOf(item).ToString());
				xmlElement2.SetAttribute("name", item.Name);
				xmlElement2.SetAttribute("visible", XmlConvert.ToString(item.Visible));
				xmlElement2.SetAttribute("checked", XmlConvert.ToString(((ButtonItem)item).Checked));
			}
		}
	}

	public void LoadLayout(string fileName)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(fileName);
		LoadLayout(xmlDocument.FirstChild as XmlElement);
	}

	public void LoadLayout(XmlElement xmlParent)
	{
		if (Items.Count == 0)
		{
			return;
		}
		if (!(xmlParent.FirstChild is XmlElement xmlElement) || xmlElement.Name != "navigationbar")
		{
			throw new InvalidOperationException("xmlParent parameter in LoadLayout does not contain expected XML child note with name navigationbar. Invalid XML layout file.");
		}
		try
		{
			((ISupportInitialize)this).BeginInit();
			((Control)this).Height = XmlConvert.ToInt32(xmlElement.GetAttribute("height"));
			foreach (XmlElement childNode in xmlElement.ChildNodes)
			{
				if (!Items.Contains(childNode.GetAttribute("name")))
				{
					continue;
				}
				BaseItem baseItem = Items[childNode.GetAttribute("name")];
				if (baseItem is ButtonItem)
				{
					ButtonItem buttonItem = baseItem as ButtonItem;
					buttonItem.Checked = XmlConvert.ToBoolean(childNode.GetAttribute("checked"));
					buttonItem.Visible = XmlConvert.ToBoolean(childNode.GetAttribute("visible"));
					int num = XmlConvert.ToInt32(childNode.GetAttribute("index"));
					if (Items.IndexOf(baseItem) != num)
					{
						Items.RemoveAt(Items.IndexOf(baseItem));
						Items.Insert(num, baseItem);
					}
				}
			}
		}
		finally
		{
			((ISupportInitialize)this).EndInit();
		}
	}

	protected override bool ProcessMnemonic(char charCode)
	{
		string text = "&" + charCode;
		text = text.ToLower();
		foreach (BaseItem item in Items)
		{
			string text2 = item.Text.ToLower();
			if (text2.IndexOf(text) >= 0 && item is ButtonItem)
			{
				((ButtonItem)item).Checked = true;
				return true;
			}
		}
		return false;
	}

	protected virtual void PaintSplitter(ItemPaintArgs pa)
	{
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Expected O, but got Unknown
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		if (!ᯛ)
		{
			return;
		}
		Rectangle splitterRectangle = GetSplitterRectangle();
		eDotNetBarStyle effectiveStyle = ଡ଼.EffectiveStyle;
		if (effectiveStyle == eDotNetBarStyle.Office2010)
		{
			splitterRectangle.Height--;
			splitterRectangle.Inflate(0, 0);
			splitterRectangle.Width -= 2;
		}
		if (effectiveStyle != eDotNetBarStyle.Office2010)
		{
			LinearGradientBrush val = BarFunctions.ॴ(splitterRectangle, pa.Colors.SplitterBackground, pa.Colors.SplitterBackground2, 90f);
			try
			{
				pa.Graphics.FillRectangle((Brush)(object)val, splitterRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			SolidBrush val2 = new SolidBrush(Color.White);
			try
			{
				int num = splitterRectangle.X + (splitterRectangle.Width - 34) / 2;
				int num2 = splitterRectangle.Y + 2;
				for (int i = 0; i < 9; i++)
				{
					pa.Graphics.FillRectangle((Brush)(object)val2, num, num2, 2, 2);
					num += 4;
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			SolidBrush val3 = new SolidBrush(Color.FromArgb(128, ControlPaint.Dark(pa.Colors.SplitterBackground)));
			try
			{
				int num3 = splitterRectangle.X + (splitterRectangle.Width - 34) / 2 - 1;
				int num4 = splitterRectangle.Y + 1;
				for (int j = 0; j < 9; j++)
				{
					pa.Graphics.FillRectangle((Brush)(object)val3, num3, num4, 2, 2);
					num3 += 4;
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			DisplayHelp.DrawLine(pa.Graphics, splitterRectangle.X, splitterRectangle.Bottom - 1, splitterRectangle.Right, splitterRectangle.Bottom - 1, pa.Colors.SplitterBorder, 1);
		}
		else
		{
			Rectangle rectangle = splitterRectangle;
			rectangle.Height--;
			SmoothingMode smoothingMode = pa.Graphics.SmoothingMode;
			pa.Graphics.SmoothingMode = (SmoothingMode)3;
			LinearGradientBrush val4 = new LinearGradientBrush(rectangle, pa.Colors.SplitterBackground, pa.Colors.SplitterBackground2, 90f);
			try
			{
				pa.Graphics.FillRectangle((Brush)(object)val4, rectangle);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			pa.Graphics.SmoothingMode = smoothingMode;
			DisplayHelp.DrawRectangle(pa.Graphics, pa.Colors.SplitterBorder, splitterRectangle);
		}
	}

	protected virtual Rectangle GetSplitterRectangle()
	{
		Rectangle result = new Rectangle(((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Y, ((Control)this).Width, 6);
		if (base.BackgroundStyle.Border == eBorderType.SingleLine)
		{
			if ((base.BackgroundStyle.BorderSide & eBorderSide.Top) == eBorderSide.Top)
			{
				result.Y++;
			}
			if ((base.BackgroundStyle.BorderSide & eBorderSide.Right) == eBorderSide.Right)
			{
				result.Width--;
			}
			if ((base.BackgroundStyle.BorderSide & eBorderSide.Left) == eBorderSide.Left)
			{
				result.X++;
				result.Width--;
			}
		}
		else if (base.BackgroundStyle.Border != 0)
		{
			if ((base.BackgroundStyle.BorderSide & eBorderSide.Top) == eBorderSide.Top)
			{
				result.Y += 2;
			}
			if ((base.BackgroundStyle.BorderSide & eBorderSide.Right) == eBorderSide.Right)
			{
				result.Width -= 2;
			}
			if ((base.BackgroundStyle.BorderSide & eBorderSide.Left) == eBorderSide.Left)
			{
				result.X += 2;
				result.Width -= 2;
			}
		}
		return result;
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		SplitterMouseMove(e);
		if (!ᯝ)
		{
			base.OnMouseMove(e);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SplitterMouseMove(MouseEventArgs e)
	{
		if (ᯛ && !ᯝ)
		{
			if (HitTestSplitter(e.X, e.Y))
			{
				if (ᯜ == (Cursor)null || GetDesignMode())
				{
					if (ᯜ == (Cursor)null)
					{
						if (GetDesignMode())
						{
							ᯜ = Cursor.Current;
						}
						else
						{
							ᯜ = ((Control)this).Cursor;
						}
					}
					if (GetDesignMode())
					{
						Cursor.Current = Cursors.SizeNS;
					}
					else
					{
						((Control)this).Cursor = Cursors.SizeNS;
					}
				}
			}
			else if (ᯜ != (Cursor)null)
			{
				if (GetDesignMode())
				{
					Cursor.Current = ᯜ;
				}
				else
				{
					((Control)this).Cursor = ᯜ;
				}
				ᯜ = null;
			}
		}
		if (ᯝ)
		{
			bool _2176 = false;
			BaseItem baseItem = ଡ଼.ఽ();
			if (baseItem == null)
			{
				_2176 = false;
			}
			else if (e.Y >= baseItem.DisplayRectangle.Bottom - 6)
			{
				_2176 = true;
			}
			int num = ଡ଼._2175(_2176);
			if (baseItem != null && e.Y >= baseItem.DisplayRectangle.Bottom - 6 && GetItemContainerRectangle().Height > num + 6)
			{
				ଡ଼.ShowFewerButtons();
			}
			else if (e.Y < 0 && Math.Abs(e.Y) >= num + 6)
			{
				ଡ଼.ShowMoreButtons();
			}
			else
			{
				RecalcLayout();
			}
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		SplitterMouseLeave();
		base.OnMouseLeave(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SplitterMouseLeave()
	{
		if (ᯜ != (Cursor)null)
		{
			if (GetDesignMode())
			{
				Cursor.Current = ᯜ;
			}
			else
			{
				((Control)this).Cursor = ᯜ;
			}
			ᯜ = null;
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		SplitterMouseDown(e);
		if (!ᯝ)
		{
			base.OnMouseDown(e);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SplitterMouseDown(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)((Control)this).Dock == 2 && HitTestSplitter(e.X, e.Y))
		{
			((Control)this).Capture = true;
			ᯝ = true;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool HitTestSplitter(int x, int y)
	{
		if (ᯛ && GetSplitterRectangle().Contains(x, y))
		{
			return true;
		}
		return false;
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		SplitterMouseUp(e);
		base.OnMouseUp(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SplitterMouseUp(MouseEventArgs e)
	{
		if (ᯝ)
		{
			ᯝ = false;
			((Control)this).Capture = false;
			if (ᯜ != (Cursor)null)
			{
				((Control)this).Cursor = ᯜ;
				ᯜ = null;
			}
		}
	}
}
