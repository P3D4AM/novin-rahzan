using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro.Rendering;

namespace DevComponents.DotNetBar.Metro;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class MetroToolbarContainer : ImageItem, IDesignTimeProvider
{
	private SimpleItemContainer __2417;

	private ButtonItem __2418;

	private ItemContainer __2419;

	private MetroToolbar __241A;

	private bool __241B = true;

	[DefaultValue(true)]
	public bool ExpandButtonVisible
	{
		get
		{
			return __241B;
		}
		set
		{
			if (value != __241B)
			{
				bool oldValue = __241B;
				__241B = value;
				OnExpandButtonVisibleChanged(oldValue, value);
			}
		}
	}

	public SimpleItemContainer MainItemsContainer => __2417;

	public ItemContainer ExtraItemsContainer => __2419;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(false)]
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
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
		__241A = parent;
		m_IsContainer = true;
		AccessibleRole = (AccessibleRole)20;
		__2417 = new SimpleItemContainer();
		__2417.GlobalItem = false;
		__2417.Displayed = true;
		__2417.ᆁ(_0BCF: true);
		__2417.ContainerControl = parent;
		__2417.DesignTimeVisible = false;
		__2417.Name = "_MainItemsContainer";
		SubItems.Add(__2417);
		__2418 = new ButtonItem();
		__2418.Click += _1CE4;
		__2418.ImagePaddingHorizontal = 12;
		__2418.ImagePaddingVertical = 10;
		__2418.Name = "sysMetroToolbarExpandButton";
		__2418.SetSystemItem(b: true);
		__2418.ContainerControl = parent;
		__2418.DesignTimeVisible = false;
		_2123();
		SubItems.Add(__2418);
		__2419 = new ItemContainer();
		__2419.LayoutOrientation = eOrientation.Horizontal;
		__2419.GlobalItem = false;
		__2419.Displayed = true;
		__2419.MultiLine = true;
		__2419.ᆁ(_0BCF: true);
		__2419.ContainerControl = parent;
		__2419.DesignTimeVisible = false;
		__2419.Name = "_ExtraItemsContainer";
		SubItems.Add(__2419);
	}

	protected override void Dispose(bool disposing)
	{
		if (__2418.Image != null)
		{
			Image image = __2418.Image;
			__2418.Image = null;
			image.Dispose();
		}
		base.Dispose(disposing);
	}

	private void _2123()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		if (__2418.Image != null)
		{
			Image image = __2418.Image;
			__2418.Image = null;
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
		__2418.Image = (Image)(object)val;
	}

	private void _1CE4(object _٠, EventArgs ؿ)
	{
		__241A.Expanded = !__241A.Expanded;
	}

	public override void Paint(ItemPaintArgs pa)
	{
		if (!SuspendLayout)
		{
			__2417.Paint(pa);
			__2418.Paint(pa);
			__2419.Paint(pa);
		}
	}

	public override void RecalcSize()
	{
		if (ExpandButtonVisible)
		{
			__2418.Displayed = true;
			__2418.RecalcSize();
			__2418.SetDisplayRectangle(new Rectangle(WidthInternal - __2418.WidthInternal, TopInternal, __2418.WidthInternal, __2418.HeightInternal));
		}
		else
		{
			__2418.Displayed = false;
			__2418.HeightInternal = 0;
			__2418.WidthInternal = 0;
		}
		__2417.LeftInternal = LeftInternal;
		__2417.TopInternal = TopInternal;
		if (__2417.VisibleSubItems == 0)
		{
			__2417.MinimumSize = new Size(WidthInternal - __2418.WidthInternal, 28);
		}
		else
		{
			__2417.MinimumSize = Size.Empty;
			__2417.WidthInternal = WidthInternal - __2418.WidthInternal;
			__2417.HeightInternal = 0;
		}
		__2417.RecalcSize();
		__2419.TopInternal = TopInternal + Math.Max(__2417.HeightInternal, __2418.HeightInternal) + 1;
		if (__2419.VisibleSubItems == 0)
		{
			__2419.MinimumSize = new Size(WidthInternal, 28);
		}
		else
		{
			__2419.MinimumSize = Size.Empty;
			__2419.WidthInternal = WidthInternal;
			__2419.HeightInternal = 0;
		}
		__2419.RecalcSize();
		HeightInternal = Math.Max(__2418.HeightInternal, __2417.HeightInternal) + __2419.HeightInternal + 1;
		base.RecalcSize();
	}

	public override BaseItem Copy()
	{
		MetroToolbarContainer metroToolbarContainer = new MetroToolbarContainer(__241A);
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
		__241A.RecalcLayout();
	}

	public void UpdateSystemColors()
	{
		_2123();
	}

	public InsertPosition GetInsertPosition(Point pScreen, BaseItem DragItem)
	{
		InsertPosition insertPosition = null;
		Point pt = ((Control)__241A).PointToClient(pScreen);
		if (__2417.DisplayRectangle.Contains(pt) || !__241A.Expanded)
		{
			insertPosition = ((IDesignTimeProvider)__2417).GetInsertPosition(pScreen, DragItem);
		}
		if (insertPosition == null && __241A.Expanded)
		{
			insertPosition = ((IDesignTimeProvider)__2419).GetInsertPosition(pScreen, DragItem);
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
