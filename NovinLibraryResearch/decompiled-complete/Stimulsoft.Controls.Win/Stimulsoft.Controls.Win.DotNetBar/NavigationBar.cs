using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[ComVisible(false)]
public class NavigationBar : BarBaseControl, ISupportInitialize
{
	private NavigationBarContainer m_ItemContainer;

	private bool m_SplitterVisible;

	private const int SPLITTER_SIZE = 6;

	private Cursor m_SavedSplitterCursor;

	private bool m_SplitterMouseDown;

	private bool m_Initializing;

	private static RoundRectangleShapeDescriptor _ButtonShape = new RoundRectangleShapeDescriptor();

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection Items => m_ItemContainer.SubItems;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates Configure Buttons button is visible.")]
	[DefaultValue(true)]
	public bool ConfigureItemVisible
	{
		get
		{
			return m_ItemContainer.ConfigureItemVisible;
		}
		set
		{
			m_ItemContainer.ConfigureItemVisible = value;
			RecalcLayout();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether Show More Buttons and Show Fewer Buttons menu items are visible on Configure buttons menu.")]
	[DefaultValue(true)]
	public bool ConfigureShowHideVisible
	{
		get
		{
			return m_ItemContainer.ConfigureShowHideVisible;
		}
		set
		{
			m_ItemContainer.ConfigureShowHideVisible = value;
			RecalcLayout();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Gets or sets whether Navigation Pane Options menu item is visible on Configure buttons menu.")]
	[DefaultValue(true)]
	public bool ConfigureNavOptionsVisible
	{
		get
		{
			return m_ItemContainer.ConfigureNavOptionsVisible;
		}
		set
		{
			m_ItemContainer.ConfigureNavOptionsVisible = value;
			RecalcLayout();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether Navigation Pane Add/Remove Buttons menu item is visible on Configure buttons menu.")]
	[DefaultValue(true)]
	public bool ConfigureAddRemoveVisible
	{
		get
		{
			return m_ItemContainer.ConfigureAddRemoveVisible;
		}
		set
		{
			m_ItemContainer.ConfigureAddRemoveVisible = value;
			RecalcLayout();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether summary line is visible.")]
	[DefaultValue(true)]
	public bool SummaryLineVisible
	{
		get
		{
			return m_ItemContainer.SummaryLineVisible;
		}
		set
		{
			m_ItemContainer.SummaryLineVisible = value;
			RecalcLayout();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates the padding in pixels at the top portion of the item.")]
	[DefaultValue(4)]
	public int ItemPaddingTop
	{
		get
		{
			return m_ItemContainer.ItemPaddingTop;
		}
		set
		{
			m_ItemContainer.ItemPaddingTop = value;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates the padding in pixels at the bottom of the item.")]
	[DefaultValue(4)]
	public int ItemPaddingBottom
	{
		get
		{
			return m_ItemContainer.ItemPaddingBottom;
		}
		set
		{
			m_ItemContainer.ItemPaddingBottom = value;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Browsable(false)]
	public NavigationBarContainer ItemsContainer => m_ItemContainer;

	[Browsable(false)]
	public ButtonItem CheckedButton
	{
		get
		{
			foreach (BaseItem subItem in m_ItemContainer.SubItems)
			{
				if (subItem is ButtonItem && ((ButtonItem)subItem).Checked)
				{
					return subItem as ButtonItem;
				}
			}
			return null;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates whether images are automatically resized to size specified in ImageSizeSummaryLine when button is on the bottom summary line of navigation bar.")]
	[DefaultValue(true)]
	public bool AutoSizeButtonImage
	{
		get
		{
			return m_ItemContainer.AutoSizeButtonImage;
		}
		set
		{
			m_ItemContainer.AutoSizeButtonImage = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates size of the image that will be use to resize images to when button button is on the bottom summary line of navigation bar and AutoSizeButtonImage=true.")]
	public Size ImageSizeSummaryLine
	{
		get
		{
			return m_ItemContainer.ImageSizeSummaryLine;
		}
		set
		{
			m_ItemContainer.ImageSizeSummaryLine = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies the visual style of the control.")]
	[DefaultValue(eDotNetBarStyle.Office2003)]
	public eDotNetBarStyle Style
	{
		get
		{
			return m_ItemContainer.Style;
		}
		set
		{
			base.ColorScheme.Style = value;
			m_ItemContainer.Style = value;
			ApplyColorSchemeDefaults();
			RecalcLayout();
		}
	}

	internal IShapeDescriptor ButtonShape => _ButtonShape;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsSplitterMouseDown => m_SplitterMouseDown;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates whether splitter on top of the navigation bar is visible.")]
	[DefaultValue(false)]
	public bool SplitterVisible
	{
		get
		{
			return m_SplitterVisible;
		}
		set
		{
			m_SplitterVisible = value;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	public event EventHandler OptionsDialogClosed;

	public NavigationBar()
	{
		m_ItemContainer = new NavigationBarContainer();
		m_ItemContainer.GlobalItem = false;
		m_ItemContainer.ContainerControl = this;
		m_ItemContainer.Stretch = false;
		m_ItemContainer.Displayed = true;
		m_ItemContainer.Style = eDotNetBarStyle.Office2003;
		m_ItemContainer.SetOwner(this);
		SetBaseItemContainer(m_ItemContainer);
		SetDesignTimeDefaults();
	}

	internal void InvokeOptionsDialogClosed()
	{
		OnOptionsDialogClosed(new EventArgs());
	}

	protected virtual void OnOptionsDialogClosed(EventArgs e)
	{
		this.OptionsDialogClosed?.Invoke(this, e);
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
		if (m_Initializing || !BarFunctions.IsHandleValid((Control)(object)this))
		{
			return;
		}
		base.RecalcSize();
		if (!m_ItemContainer.IsRecalculatingSize)
		{
			Rectangle itemContainerRectangle = GetItemContainerRectangle();
			if (m_ItemContainer.HeightInternal > 0)
			{
				((Control)this).Height = m_ItemContainer.HeightInternal + itemContainerRectangle.Top + Math.Max(0, ((Control)this).ClientRectangle.Bottom - itemContainerRectangle.Bottom);
			}
		}
	}

	public override void SetDesignTimeDefaults()
	{
		base.SetDesignTimeDefaults();
		ApplyColorSchemeDefaults();
		base.BackgroundStyle.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		base.BackgroundStyle.Border = eBorderType.SingleLine;
	}

	private void ApplyColorSchemeDefaults()
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
		base.BackgroundStyle.ApplyColorScheme(base.ColorScheme);
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		Application.DoEvents();
		base.ColorScheme.Refresh();
		ApplyColorSchemeDefaults();
	}

	public void ShowMoreButtons()
	{
		m_ItemContainer.ShowMoreButtons();
	}

	public void ShowFewerButtons()
	{
		m_ItemContainer.ShowFewerButtons();
	}

	protected override Rectangle GetItemContainerRectangle()
	{
		Rectangle itemContainerRectangle = base.GetItemContainerRectangle();
		if (m_SplitterVisible)
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
		if (m_ItemContainer.ImageSizeSummaryLine.Width == 16)
		{
			return m_ItemContainer.ImageSizeSummaryLine.Height != 16;
		}
		return true;
	}

	void ISupportInitialize.BeginInit()
	{
		m_Initializing = true;
	}

	void ISupportInitialize.EndInit()
	{
		m_Initializing = false;
		RecalcLayout();
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
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Expected O, but got Unknown
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		if (!m_SplitterVisible)
		{
			return;
		}
		Rectangle splitterRectangle = GetSplitterRectangle();
		eDotNetBarStyle effectiveStyle = m_ItemContainer.EffectiveStyle;
		if (effectiveStyle == eDotNetBarStyle.Office2010)
		{
			splitterRectangle.Height--;
			splitterRectangle.Inflate(0, 0);
			splitterRectangle.Width -= 2;
		}
		if (effectiveStyle != eDotNetBarStyle.Office2010)
		{
			LinearGradientBrush val = BarFunctions.CreateLinearGradientBrush(splitterRectangle, pa.Colors.SplitterBackground, pa.Colors.SplitterBackground2, 90f);
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
		if (!m_SplitterMouseDown)
		{
			base.OnMouseMove(e);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SplitterMouseMove(MouseEventArgs e)
	{
		if (m_SplitterVisible && !m_SplitterMouseDown)
		{
			if (HitTestSplitter(e.X, e.Y))
			{
				if (m_SavedSplitterCursor == (Cursor)null || GetDesignMode())
				{
					if (m_SavedSplitterCursor == (Cursor)null)
					{
						if (GetDesignMode())
						{
							m_SavedSplitterCursor = Cursor.Current;
						}
						else
						{
							m_SavedSplitterCursor = ((Control)this).Cursor;
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
			else if (m_SavedSplitterCursor != (Cursor)null)
			{
				if (GetDesignMode())
				{
					Cursor.Current = m_SavedSplitterCursor;
				}
				else
				{
					((Control)this).Cursor = m_SavedSplitterCursor;
				}
				m_SavedSplitterCursor = null;
			}
		}
		if (m_SplitterMouseDown)
		{
			bool reduceSize = false;
			BaseItem firstVisibleItem = m_ItemContainer.GetFirstVisibleItem();
			if (firstVisibleItem == null)
			{
				reduceSize = false;
			}
			else if (e.Y >= firstVisibleItem.DisplayRectangle.Bottom - 6)
			{
				reduceSize = true;
			}
			int changeSizeHeight = m_ItemContainer.GetChangeSizeHeight(reduceSize);
			if (firstVisibleItem != null && e.Y >= firstVisibleItem.DisplayRectangle.Bottom - 6 && GetItemContainerRectangle().Height > changeSizeHeight + 6)
			{
				m_ItemContainer.ShowFewerButtons();
			}
			else if (e.Y < 0 && Math.Abs(e.Y) >= changeSizeHeight + 6)
			{
				m_ItemContainer.ShowMoreButtons();
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
		if (m_SavedSplitterCursor != (Cursor)null)
		{
			if (GetDesignMode())
			{
				Cursor.Current = m_SavedSplitterCursor;
			}
			else
			{
				((Control)this).Cursor = m_SavedSplitterCursor;
			}
			m_SavedSplitterCursor = null;
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		SplitterMouseDown(e);
		if (!m_SplitterMouseDown)
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
			m_SplitterMouseDown = true;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool HitTestSplitter(int x, int y)
	{
		if (m_SplitterVisible && GetSplitterRectangle().Contains(x, y))
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
		if (m_SplitterMouseDown)
		{
			m_SplitterMouseDown = false;
			((Control)this).Capture = false;
			if (m_SavedSplitterCursor != (Cursor)null)
			{
				((Control)this).Cursor = m_SavedSplitterCursor;
				m_SavedSplitterCursor = null;
			}
		}
	}
}
