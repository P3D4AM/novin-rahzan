using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.Editors;

public class VisualGroup : VisualItem
{
	private VisualItem _MouseDownItem;

	private VisualItem _MouseOverItem;

	private VisualItemCollection _Items;

	private VisualItem _FocusedItem;

	private int _HorizontalItemSpacing;

	private bool _IsRootVisual;

	private eVerticalAlignment _VerticalItemAlignment = eVerticalAlignment.Middle;

	private eHorizontalAlignment _HorizontalItemAlignment;

	public virtual VisualItemCollection Items => _Items;

	public VisualItem FocusedItem
	{
		get
		{
			return _FocusedItem;
		}
		internal set
		{
			if (_FocusedItem != value)
			{
				if (_FocusedItem != null)
				{
					_FocusedItem.ProcessLostFocus();
				}
				VisualItem focusedItem = _FocusedItem;
				_FocusedItem = value;
				if (_FocusedItem != null)
				{
					_FocusedItem.ProcessGotFocus();
				}
				OnFocusedItemChanged(focusedItem);
				InvalidateArrange();
			}
		}
	}

	[DefaultValue(0)]
	public int HorizontalItemSpacing
	{
		get
		{
			return _HorizontalItemSpacing;
		}
		set
		{
			if (_HorizontalItemSpacing != value)
			{
				_HorizontalItemSpacing = value;
				InvalidateArrange();
			}
		}
	}

	[DefaultValue(false)]
	public bool IsRootVisual
	{
		get
		{
			return _IsRootVisual;
		}
		set
		{
			_IsRootVisual = value;
		}
	}

	[DefaultValue(eVerticalAlignment.Middle)]
	public eVerticalAlignment VerticalItemAlignment
	{
		get
		{
			return _VerticalItemAlignment;
		}
		set
		{
			if (_VerticalItemAlignment != value)
			{
				_VerticalItemAlignment = value;
				OnArrangeInvalid();
			}
		}
	}

	[DefaultValue(eHorizontalAlignment.Left)]
	public eHorizontalAlignment HorizontalItemAlignment
	{
		get
		{
			return _HorizontalItemAlignment;
		}
		set
		{
			if (_HorizontalItemAlignment != value)
			{
				_HorizontalItemAlignment = value;
				OnArrangeInvalid();
			}
		}
	}

	public VisualGroup()
	{
		_Items = new VisualItemCollection(this);
	}

	internal virtual void ProcessItemsCollectionChanged(CollectionChangedInfo collectionChangedInfo)
	{
		OnItemsCollectionChanged(collectionChangedInfo);
	}

	protected virtual void OnItemsCollectionChanged(CollectionChangedInfo collectionChangedInfo)
	{
		if (collectionChangedInfo.ChangeType == eCollectionChangeType.Adding || collectionChangedInfo.ChangeType == eCollectionChangeType.Removing || collectionChangedInfo.ChangeType == eCollectionChangeType.Clearing)
		{
			if (collectionChangedInfo.Removed != null)
			{
				VisualItem[] removed = collectionChangedInfo.Removed;
				foreach (VisualItem visualItem in removed)
				{
					visualItem.Parent = null;
				}
			}
			if (collectionChangedInfo.ChangeType == eCollectionChangeType.Clearing)
			{
				foreach (VisualItem item in Items)
				{
					item.Parent = null;
				}
			}
			if (collectionChangedInfo.Added != null)
			{
				VisualItem[] added = collectionChangedInfo.Added;
				foreach (VisualItem visualItem3 in added)
				{
					if (visualItem3.Parent != null && visualItem3.Parent != this)
					{
						visualItem3.Parent.Items.Remove(visualItem3);
					}
					visualItem3.Parent = this;
				}
			}
		}
		InvalidateArrange();
	}

	internal void OnChildItemVisibleChanged(VisualItem visualItem)
	{
		InvalidateArrange();
	}

	protected virtual void OnFocusedItemChanged(VisualItem previousFocus)
	{
	}

	protected override void OnPaint(PaintInfo p)
	{
		if (!IsLayoutValid)
		{
			PerformLayout(p);
		}
		Point renderOffset = p.RenderOffset;
		Point renderOffset2 = p.RenderOffset;
		renderOffset2.Offset(base.RenderBounds.Location);
		p.RenderOffset = renderOffset2;
		Graphics graphics = p.Graphics;
		Region val = null;
		bool flag = false;
		val = graphics.Clip;
		Rectangle renderBounds = base.RenderBounds;
		renderBounds.Offset(renderOffset);
		graphics.SetClip(renderBounds, (CombineMode)1);
		flag = true;
		bool parentEnabled = p.ParentEnabled;
		p.ParentEnabled = p.ParentEnabled && base.Enabled;
		bool renderSystemItemsOnly = p.RenderSystemItemsOnly;
		foreach (VisualItem item in _Items)
		{
			if (!renderSystemItemsOnly || item.ItemType == eSystemItemType.SystemButton)
			{
				if (_VerticalItemAlignment == eVerticalAlignment.Middle)
				{
					item.RenderBounds = new Rectangle(p.RenderOffset.X + item.Location.X, p.RenderOffset.Y + item.Location.Y + (base.Size.Height - item.Size.Height) / 2, item.Size.Width, item.Size.Height);
				}
				else if (_VerticalItemAlignment == eVerticalAlignment.Bottom)
				{
					item.RenderBounds = new Rectangle(p.RenderOffset.X + item.Location.X, p.RenderOffset.Y + item.Location.Y + (base.Size.Height - item.Size.Height), item.Size.Width, item.Size.Height);
				}
				else
				{
					item.RenderBounds = new Rectangle(p.RenderOffset.X + item.Location.X, p.RenderOffset.Y + item.Location.Y, item.Size.Width, item.Size.Height);
				}
				if (item.RenderBounds.IntersectsWith(renderBounds))
				{
					p.RenderOffset = Point.Empty;
					item.ProcessPaint(p);
					p.RenderOffset = renderOffset2;
				}
			}
		}
		p.ParentEnabled = parentEnabled;
		if (flag)
		{
			if (val != null)
			{
				graphics.Clip = val;
			}
			else
			{
				graphics.ResetClip();
			}
		}
		if (val != null)
		{
			val.Dispose();
		}
		p.RenderOffset = renderOffset;
		base.OnPaint(p);
	}

	protected override void OnIsRightToLeftChanged()
	{
		foreach (VisualItem item in _Items)
		{
			item.IsRightToLeft = base.IsRightToLeft;
		}
		base.OnIsRightToLeftChanged();
	}

	public override void PerformLayout(PaintInfo pi)
	{
		Point empty = Point.Empty;
		Size empty2 = Size.Empty;
		Size availableSize = pi.AvailableSize;
		Size size = availableSize;
		bool flag = false;
		Size empty3 = Size.Empty;
		foreach (VisualItem item in new VisualCollectionEnumerator(_Items, base.IsRightToLeft))
		{
			if (item.Visible)
			{
				item.Location = empty;
				pi.AvailableSize = new Size(availableSize.Width - empty.X, availableSize.Height);
				item.PerformLayout(pi);
				if (item.Alignment == eItemAlignment.Right)
				{
					item.Location = new Point(size.Width - item.Size.Width, empty.Y);
					size.Width -= item.Size.Width + _HorizontalItemSpacing;
					empty3.Width += item.Size.Width + _HorizontalItemSpacing;
					flag = true;
				}
				else
				{
					empty.X += item.Size.Width + _HorizontalItemSpacing;
					empty2.Width += item.Size.Width + _HorizontalItemSpacing;
					empty3.Width += item.Size.Width + _HorizontalItemSpacing;
				}
				if (item.Size.Height > empty2.Height)
				{
					empty2.Height = item.Size.Height;
				}
			}
		}
		if (empty2.Width > 0)
		{
			empty2.Width -= _HorizontalItemSpacing;
		}
		if (flag && _HorizontalItemAlignment != 0)
		{
			if (_HorizontalItemAlignment == eHorizontalAlignment.Right)
			{
				if (size.Width < availableSize.Width)
				{
					size.Width--;
				}
				foreach (VisualItem item2 in new VisualCollectionEnumerator(_Items, !base.IsRightToLeft))
				{
					if (item2.Visible && item2.Alignment != eItemAlignment.Right && !(item2 is LockUpdateCheckBox))
					{
						item2.Location = new Point(size.Width - item2.Size.Width, empty.Y);
						size.Width -= item2.Size.Width + _HorizontalItemSpacing;
					}
				}
			}
			else
			{
				int num = (size.Width - empty2.Width) / 2;
				foreach (VisualItem item3 in new VisualCollectionEnumerator(_Items, base.IsRightToLeft))
				{
					if (item3.Visible && item3.Alignment != eItemAlignment.Right && !(item3 is LockUpdateCheckBox))
					{
						item3.Location = new Point(item3.Location.X + num, item3.Location.Y);
					}
				}
			}
		}
		base.CalculatedSize = empty3;
		if (flag)
		{
			empty2.Width = availableSize.Width;
		}
		pi.AvailableSize = availableSize;
		base.Size = empty2;
		base.RenderBounds = new Rectangle(pi.RenderOffset, empty2);
		base.PerformLayout(pi);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		VisualItem itemAt = GetItemAt(e.X, e.Y);
		if (itemAt != null)
		{
			_MouseDownItem = itemAt;
			_MouseOverItem = itemAt;
			itemAt.ProcessMouseDown(e);
			if (_MouseDownItem == itemAt && CanFocus(itemAt))
			{
				FocusedItem = itemAt;
			}
		}
		else if (itemAt == null && base.IsFocused)
		{
			foreach (VisualItem item in _Items)
			{
				if (item is VisualInputBase && CanFocus(item))
				{
					FocusedItem = item;
					break;
				}
			}
		}
		base.OnMouseDown(e);
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		if (FocusedItem != null)
		{
			FocusedItem.ProcessMouseWheel(e);
		}
		base.OnMouseWheel(e);
	}

	protected virtual bool CanFocus(VisualItem v)
	{
		if (v != null && v.Visible && (v.Focusable || v is VisualInputGroup))
		{
			return v.Enabled;
		}
		return false;
	}

	protected override void OnClick(EventArgs e)
	{
		if (_MouseDownItem != null)
		{
			_MouseDownItem.ProcessClick();
		}
		base.OnClick(EventArgs.Empty);
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		if (_MouseDownItem != null)
		{
			_MouseDownItem.ProcessMouseClick(e);
		}
		base.OnMouseClick(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (_MouseDownItem != null)
		{
			_MouseDownItem.ProcessMouseUp(e);
			_MouseDownItem = null;
		}
		base.OnMouseUp(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (_MouseDownItem != null)
		{
			_MouseDownItem.ProcessMouseMove(e);
		}
		else
		{
			VisualItem itemAt = GetItemAt(e.X, e.Y);
			if (itemAt != _MouseOverItem && _MouseOverItem != null)
			{
				_MouseOverItem.ProcessMouseLeave();
				_MouseOverItem = null;
			}
			if (itemAt != null && _MouseOverItem == null)
			{
				_MouseOverItem = itemAt;
				_MouseOverItem.ProcessMouseEnter();
			}
			if (_MouseOverItem != null)
			{
				_MouseOverItem.ProcessMouseMove(e);
			}
		}
		base.OnMouseMove(e);
	}

	protected override void OnMouseLeave()
	{
		if (_MouseDownItem != null)
		{
			_MouseDownItem.ProcessMouseLeave();
		}
		else if (_MouseOverItem != null)
		{
			_MouseOverItem.ProcessMouseLeave();
			_MouseOverItem = null;
		}
		base.OnMouseLeave();
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		if (_FocusedItem != null)
		{
			_FocusedItem.ProcessKeyDown(e);
		}
		base.OnKeyDown(e);
	}

	protected override void OnKeyUp(KeyEventArgs e)
	{
		if (_FocusedItem != null)
		{
			_FocusedItem.ProcessKeyUp(e);
		}
		base.OnKeyUp(e);
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		if (_FocusedItem != null)
		{
			_FocusedItem.ProcessKeyPress(e);
		}
		base.OnKeyPress(e);
	}

	protected override bool OnCmdKey(ref Message msg, Keys keyData)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		if (_FocusedItem != null && _FocusedItem.ProcessCmdKey(ref msg, keyData))
		{
			return true;
		}
		foreach (VisualItem item in Items)
		{
			if (item is VisualButtonBase && item.ProcessCmdKey(ref msg, keyData))
			{
				return true;
			}
		}
		return base.OnCmdKey(ref msg, keyData);
	}

	public VisualItem GetItemAt(int x, int y)
	{
		for (int num = _Items.Count - 1; num >= 0; num--)
		{
			VisualItem visualItem = _Items[num];
			if (visualItem.Visible && visualItem.RenderBounds.Contains(x, y))
			{
				return visualItem;
			}
		}
		return null;
	}

	private VisualItem GetFirstVisibleItem()
	{
		foreach (VisualItem item in new VisualCollectionEnumerator(_Items, base.IsRightToLeft))
		{
			if (item.Visible)
			{
				return item;
			}
		}
		return null;
	}

	protected override void OnGotFocus()
	{
		OnGroupFocused();
		base.OnGotFocus();
	}

	protected virtual void OnGroupFocused()
	{
		if (FocusedItem == null)
		{
			VisualItem firstVisibleItem = GetFirstVisibleItem();
			if (firstVisibleItem != null)
			{
				FocusedItem = firstVisibleItem;
			}
		}
	}

	protected override void OnLostFocus()
	{
		FocusedItem = null;
		base.OnLostFocus();
	}

	internal virtual void ProcessInputComplete()
	{
		OnInputComplete();
	}

	protected virtual void OnInputComplete()
	{
	}

	internal void ProcessInputChanged(VisualInputBase input)
	{
		OnInputChanged(input);
	}

	protected virtual void OnInputChanged(VisualInputBase input)
	{
	}
}
