using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class MetroToolbarContainer : ImageItem, IDesignTimeProvider
{
	private ItemContainer _MainItemsContainer;

	private ButtonItem _ExpandButton;

	private ItemContainer _ExtraItemsContainer;

	private MetroToolbar _MetroToolbar;

	private bool _ExpandButtonVisible = true;

	[DefaultValue(true)]
	public bool ExpandButtonVisible
	{
		get
		{
			return _ExpandButtonVisible;
		}
		set
		{
			if (value != _ExpandButtonVisible)
			{
				bool expandButtonVisible = _ExpandButtonVisible;
				_ExpandButtonVisible = value;
				OnExpandButtonVisibleChanged(expandButtonVisible, value);
			}
		}
	}

	public ItemContainer MainItemsContainer => _MainItemsContainer;

	public ItemContainer ExtraItemsContainer => _ExtraItemsContainer;

	[Browsable(false)]
	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool Expanded
	{
		get
		{
			return base.Expanded;
		}
		set
		{
			base.Expanded = value;
			if (value)
			{
				return;
			}
			foreach (BaseItem subItem in SubItems)
			{
				subItem.Expanded = false;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override Size SubItemsImageSize
	{
		get
		{
			return base.SubItemsImageSize;
		}
		set
		{
		}
	}

	public MetroToolbarContainer(MetroToolbar parent)
	{
		_MetroToolbar = parent;
		m_IsContainer = true;
		AccessibleRole = (AccessibleRole)20;
		_MainItemsContainer = new ItemContainer();
		_MainItemsContainer.LayoutOrientation = eOrientation.Horizontal;
		_MainItemsContainer.GlobalItem = false;
		_MainItemsContainer.Displayed = true;
		_MainItemsContainer.ItemSpacing = 0;
		_MainItemsContainer.SetSystemContainer(b: true);
		_MainItemsContainer.ContainerControl = parent;
		_MainItemsContainer.DesignTimeVisible = false;
		_MainItemsContainer.Name = "_MainItemsContainer";
		SubItems.Add(_MainItemsContainer);
		_ExpandButton = new ButtonItem();
		_ExpandButton.Click += ExpandButtonClick;
		_ExpandButton.ImagePaddingHorizontal = 12;
		_ExpandButton.ImagePaddingVertical = 10;
		_ExpandButton.Name = "sysMetroToolbarExpandButton";
		_ExpandButton.SetSystemItem(b: true);
		_ExpandButton.ContainerControl = parent;
		_ExpandButton.DesignTimeVisible = false;
		UpdateExpandButtonImage();
		SubItems.Add(_ExpandButton);
		_ExtraItemsContainer = new ItemContainer();
		_ExtraItemsContainer.LayoutOrientation = eOrientation.Horizontal;
		_ExtraItemsContainer.GlobalItem = false;
		_ExtraItemsContainer.Displayed = true;
		_ExtraItemsContainer.MultiLine = true;
		_ExtraItemsContainer.SetSystemContainer(b: true);
		_ExtraItemsContainer.ContainerControl = parent;
		_ExtraItemsContainer.DesignTimeVisible = false;
		_ExtraItemsContainer.Name = "_ExtraItemsContainer";
		SubItems.Add(_ExtraItemsContainer);
	}

	protected override void Dispose(bool disposing)
	{
		if (_ExpandButton.Image != null)
		{
			Image image = _ExpandButton.Image;
			_ExpandButton.Image = null;
			image.Dispose();
		}
		base.Dispose(disposing);
	}

	private void UpdateExpandButtonImage()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		if (_ExpandButton.Image != null)
		{
			Image image = _ExpandButton.Image;
			_ExpandButton.Image = null;
			image.Dispose();
		}
		Bitmap val = new Bitmap(16, 16);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.SmoothingMode = (SmoothingMode)2;
			SolidBrush val3 = new SolidBrush(MetroRender.GetColorTable().ForeColor);
			try
			{
				val2.FillEllipse((Brush)(object)val3, new Rectangle(0, 2, 4, 4));
				val2.FillEllipse((Brush)(object)val3, new Rectangle(6, 2, 4, 4));
				val2.FillEllipse((Brush)(object)val3, new Rectangle(12, 2, 4, 4));
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		_ExpandButton.Image = (Image)(object)val;
	}

	private void ExpandButtonClick(object sender, EventArgs e)
	{
		_MetroToolbar.Expanded = !_MetroToolbar.Expanded;
	}

	public override void Paint(ItemPaintArgs pa)
	{
		if (!SuspendLayout)
		{
			_MainItemsContainer.Paint(pa);
			_ExpandButton.Paint(pa);
			_ExtraItemsContainer.Paint(pa);
		}
	}

	public override void RecalcSize()
	{
		if (ExpandButtonVisible)
		{
			_ExpandButton.Displayed = true;
			_ExpandButton.RecalcSize();
			_ExpandButton.SetDisplayRectangle(new Rectangle(WidthInternal - _ExpandButton.WidthInternal, TopInternal, _ExpandButton.WidthInternal, _ExpandButton.HeightInternal));
		}
		else
		{
			_ExpandButton.Displayed = false;
			_ExpandButton.HeightInternal = 0;
			_ExpandButton.WidthInternal = 0;
		}
		_MainItemsContainer.LeftInternal = LeftInternal;
		_MainItemsContainer.TopInternal = TopInternal;
		if (_MainItemsContainer.VisibleSubItems == 0)
		{
			_MainItemsContainer.MinimumSize = new Size(WidthInternal - _ExpandButton.WidthInternal, 28);
		}
		else
		{
			_MainItemsContainer.MinimumSize = Size.Empty;
			_MainItemsContainer.WidthInternal = WidthInternal - _ExpandButton.WidthInternal;
			_MainItemsContainer.HeightInternal = 0;
		}
		_MainItemsContainer.RecalcSize();
		_ExtraItemsContainer.TopInternal = TopInternal + Math.Max(_MainItemsContainer.HeightInternal, _ExpandButton.HeightInternal) + 1;
		if (_ExtraItemsContainer.VisibleSubItems == 0)
		{
			_ExtraItemsContainer.MinimumSize = new Size(WidthInternal, 28);
		}
		else
		{
			_ExtraItemsContainer.MinimumSize = Size.Empty;
			_ExtraItemsContainer.WidthInternal = WidthInternal;
			_ExtraItemsContainer.HeightInternal = 0;
		}
		_ExtraItemsContainer.RecalcSize();
		HeightInternal = Math.Max(_ExpandButton.HeightInternal, _MainItemsContainer.HeightInternal) + _ExtraItemsContainer.HeightInternal + 1;
		base.RecalcSize();
	}

	public override BaseItem Copy()
	{
		MetroToolbarContainer metroToolbarContainer = new MetroToolbarContainer(_MetroToolbar);
		CopyToItem(metroToolbarContainer);
		return metroToolbarContainer;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		RibbonStripContainerItem objCopy = copy as RibbonStripContainerItem;
		base.CopyToItem(objCopy);
	}

	protected virtual void OnExpandButtonVisibleChanged(bool oldValue, bool newValue)
	{
		NeedRecalcSize = true;
		_MetroToolbar.RecalcLayout();
	}

	public void UpdateSystemColors()
	{
		UpdateExpandButtonImage();
	}

	public InsertPosition GetInsertPosition(Point pScreen, BaseItem DragItem)
	{
		InsertPosition insertPosition = null;
		Point pt = ((Control)_MetroToolbar).PointToClient(pScreen);
		if (_MainItemsContainer.DisplayRectangle.Contains(pt) || !_MetroToolbar.Expanded)
		{
			insertPosition = ((IDesignTimeProvider)_MainItemsContainer).GetInsertPosition(pScreen, DragItem);
		}
		if (insertPosition == null && _MetroToolbar.Expanded)
		{
			insertPosition = ((IDesignTimeProvider)_ExtraItemsContainer).GetInsertPosition(pScreen, DragItem);
		}
		return insertPosition;
	}

	public void DrawReversibleMarker(int iPos, bool Before)
	{
		DesignTimeProviderContainer.DrawReversibleMarker(this, iPos, Before);
	}

	public void InsertItemAt(BaseItem objItem, int iPos, bool Before)
	{
		DesignTimeProviderContainer.InsertItemAt(this, objItem, iPos, Before);
	}
}
