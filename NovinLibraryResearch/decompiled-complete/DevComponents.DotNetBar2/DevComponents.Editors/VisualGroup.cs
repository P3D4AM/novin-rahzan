using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace DevComponents.Editors;

public class VisualGroup : VisualItem
{
	private VisualItem _ᡦ;

	private VisualItem _ᡧ;

	private VisualItemCollection _ᡨ;

	private VisualItem _ᡩ;

	private eOrientation _ᡪ;

	private int _ᡫ;

	private bool _ᡬ;

	private eVerticalAlignment _ᡭ = eVerticalAlignment.Middle;

	private eHorizontalAlignment _ᡮ;

	public virtual VisualItemCollection Items => _ᡨ;

	public VisualItem FocusedItem
	{
		get
		{
			return _ᡩ;
		}
		internal set
		{
			if (_ᡩ != value)
			{
				if (_ᡩ != null)
				{
					_ᡩ.ᡣ();
				}
				VisualItem ᡩ = _ᡩ;
				_ᡩ = value;
				if (_ᡩ != null)
				{
					_ᡩ.ᡢ();
				}
				OnFocusedItemChanged(ᡩ);
				InvalidateArrange();
			}
		}
	}

	internal eOrientation ᡥ
	{
		get
		{
			return _ᡪ;
		}
		set
		{
			_ᡪ = value;
			InvalidateArrange();
			InvalidateRender();
		}
	}

	[DefaultValue(0)]
	public int HorizontalItemSpacing
	{
		get
		{
			return _ᡫ;
		}
		set
		{
			if (_ᡫ != value)
			{
				_ᡫ = value;
				InvalidateArrange();
			}
		}
	}

	[DefaultValue(false)]
	public bool IsRootVisual
	{
		get
		{
			return _ᡬ;
		}
		set
		{
			_ᡬ = value;
		}
	}

	[DefaultValue(eVerticalAlignment.Middle)]
	public eVerticalAlignment VerticalItemAlignment
	{
		get
		{
			return _ᡭ;
		}
		set
		{
			if (_ᡭ != value)
			{
				_ᡭ = value;
				OnArrangeInvalid();
			}
		}
	}

	[DefaultValue(eHorizontalAlignment.Left)]
	public eHorizontalAlignment HorizontalItemAlignment
	{
		get
		{
			return _ᡮ;
		}
		set
		{
			if (_ᡮ != value)
			{
				_ᡮ = value;
				OnArrangeInvalid();
			}
		}
	}

	public VisualGroup()
	{
		_ᡨ = new VisualItemCollection(this);
	}

	internal virtual void ᡯ(CollectionChangedInfo ᡰ)
	{
		OnItemsCollectionChanged(ᡰ);
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

	internal void ᡱ(VisualItem ᡲ)
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
		foreach (VisualItem item in _ᡨ)
		{
			if (renderSystemItemsOnly && item.ᡇ != _1885._1886)
			{
				continue;
			}
			if (_ᡭ == eVerticalAlignment.Middle)
			{
				if (_ᡪ == eOrientation.Horizontal)
				{
					item.RenderBounds = new Rectangle(p.RenderOffset.X + item.Location.X, p.RenderOffset.Y + item.Location.Y + (base.Size.Height - item.Size.Height) / 2, item.Size.Width, item.Size.Height);
				}
				else
				{
					item.RenderBounds = new Rectangle(p.RenderOffset.X + item.Location.X + (base.Size.Width - item.Size.Width) / 2, p.RenderOffset.Y + item.Location.Y, item.Size.Width, item.Size.Height);
				}
			}
			else if (_ᡭ == eVerticalAlignment.Bottom)
			{
				if (_ᡪ == eOrientation.Horizontal)
				{
					item.RenderBounds = new Rectangle(p.RenderOffset.X + item.Location.X, p.RenderOffset.Y + item.Location.Y + (base.Size.Height - item.Size.Height), item.Size.Width, item.Size.Height);
				}
				else
				{
					item.RenderBounds = new Rectangle(p.RenderOffset.X + item.Location.X + (base.Size.Width - item.Size.Width), p.RenderOffset.Y + item.Location.Y, item.Size.Width, item.Size.Height);
				}
			}
			else
			{
				item.RenderBounds = new Rectangle(p.RenderOffset.X + item.Location.X, p.RenderOffset.Y + item.Location.Y, item.Size.Width, item.Size.Height);
			}
			if (item.RenderBounds.IntersectsWith(renderBounds))
			{
				p.RenderOffset = Point.Empty;
				item.ᡤ(p);
				p.RenderOffset = renderOffset2;
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
		foreach (VisualItem item in _ᡨ)
		{
			item.IsRightToLeft = base.IsRightToLeft;
		}
		base.OnIsRightToLeftChanged();
	}

	public override void PerformLayout(PaintInfo pi)
	{
		if (_ᡪ == eOrientation.Horizontal)
		{
			Point empty = Point.Empty;
			Size empty2 = Size.Empty;
			Size availableSize = pi.AvailableSize;
			Size size = availableSize;
			bool flag = false;
			Size empty3 = Size.Empty;
			foreach (VisualItem item in new ᢇ(_ᡨ, base.IsRightToLeft))
			{
				if (item.Visible)
				{
					item.Location = empty;
					pi.AvailableSize = new Size(availableSize.Width - empty.X, availableSize.Height);
					item.PerformLayout(pi);
					if (item.Alignment == eItemAlignment.Right)
					{
						item.Location = new Point(size.Width - item.Size.Width, empty.Y);
						size.Width -= item.Size.Width + _ᡫ;
						empty3.Width += item.Size.Width + _ᡫ;
						flag = true;
					}
					else
					{
						empty.X += item.Size.Width + _ᡫ;
						empty2.Width += item.Size.Width + _ᡫ;
						empty3.Width += item.Size.Width + _ᡫ;
					}
					if (item.Size.Height > empty2.Height)
					{
						empty2.Height = item.Size.Height;
					}
				}
			}
			if (empty2.Width > 0)
			{
				empty2.Width -= _ᡫ;
			}
			if (flag && _ᡮ != 0)
			{
				if (_ᡮ == eHorizontalAlignment.Right)
				{
					if (size.Width < availableSize.Width)
					{
						size.Width--;
					}
					foreach (VisualItem item2 in new ᢇ(_ᡨ, !base.IsRightToLeft))
					{
						if (item2.Visible && item2.Alignment != eItemAlignment.Right && !(item2 is LockUpdateCheckBox))
						{
							item2.Location = new Point(size.Width - item2.Size.Width, empty.Y);
							size.Width -= item2.Size.Width + _ᡫ;
						}
					}
				}
				else
				{
					int num = (size.Width - empty2.Width) / 2;
					foreach (VisualItem item3 in new ᢇ(_ᡨ, base.IsRightToLeft))
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
		}
		else
		{
			ᡳ(pi);
		}
		base.PerformLayout(pi);
	}

	private void ᡳ(PaintInfo ᡴ)
	{
		Point empty = Point.Empty;
		Size empty2 = Size.Empty;
		Size availableSize = ᡴ.AvailableSize;
		Size size = availableSize;
		bool flag = false;
		Size empty3 = Size.Empty;
		foreach (VisualItem item in new ᢇ(_ᡨ, base.IsRightToLeft))
		{
			if (item.Visible)
			{
				item.Location = empty;
				ᡴ.AvailableSize = new Size(availableSize.Width, availableSize.Height - empty.Y);
				item.PerformLayout(ᡴ);
				if (item.Alignment == eItemAlignment.Right)
				{
					item.Location = new Point(empty.X, size.Height - item.Size.Height);
					size.Height -= item.Size.Height + _ᡫ;
					empty3.Height += item.Size.Height + _ᡫ;
					flag = true;
				}
				else
				{
					empty.Y += item.Size.Height + _ᡫ;
					empty2.Height += item.Size.Height + _ᡫ;
					empty3.Height += item.Size.Height + _ᡫ;
				}
				if (item.Size.Width > empty2.Width)
				{
					empty2.Width = item.Size.Width;
				}
			}
		}
		if (empty2.Height > 0)
		{
			empty2.Height -= _ᡫ;
		}
		if (flag && _ᡮ != 0)
		{
			if (_ᡮ == eHorizontalAlignment.Right)
			{
				if (size.Height < availableSize.Height)
				{
					size.Height--;
				}
				foreach (VisualItem item2 in new ᢇ(_ᡨ, !base.IsRightToLeft))
				{
					if (item2.Visible && item2.Alignment != eItemAlignment.Right && !(item2 is LockUpdateCheckBox))
					{
						item2.Location = new Point(empty.X, size.Height - item2.Size.Height);
						size.Height -= item2.Size.Height + _ᡫ;
					}
				}
			}
			else
			{
				int num = (size.Height - empty2.Height) / 2;
				foreach (VisualItem item3 in new ᢇ(_ᡨ, base.IsRightToLeft))
				{
					if (item3.Visible && item3.Alignment != eItemAlignment.Right && !(item3 is LockUpdateCheckBox))
					{
						item3.Location = new Point(item3.Location.X, item3.Location.Y + num);
					}
				}
			}
		}
		base.CalculatedSize = empty3;
		if (flag)
		{
			empty2.Height = availableSize.Height;
		}
		ᡴ.AvailableSize = availableSize;
		base.Size = empty2;
		base.RenderBounds = new Rectangle(ᡴ.RenderOffset, empty2);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		VisualItem itemAt = GetItemAt(e.X, e.Y);
		if (itemAt != null)
		{
			_ᡦ = itemAt;
			_ᡧ = itemAt;
			itemAt.ᡚ(e);
			if (_ᡦ == itemAt && CanFocus(itemAt))
			{
				FocusedItem = itemAt;
			}
		}
		else if (itemAt == null && base.IsFocused)
		{
			foreach (VisualItem item in _ᡨ)
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
			FocusedItem.ᡙ(e);
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
		if (_ᡦ != null)
		{
			_ᡦ.ᡝ();
		}
		base.OnClick(EventArgs.Empty);
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		if (_ᡦ != null)
		{
			_ᡦ.ᡞ(e);
		}
		base.OnMouseClick(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (_ᡦ != null)
		{
			_ᡦ.ᡛ(e);
			_ᡦ = null;
		}
		base.OnMouseUp(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (_ᡦ != null)
		{
			_ᡦ.ᡘ(e);
		}
		else
		{
			VisualItem itemAt = GetItemAt(e.X, e.Y);
			if (itemAt != _ᡧ && _ᡧ != null)
			{
				_ᡧ.ᡗ();
				_ᡧ = null;
			}
			if (itemAt != null && _ᡧ == null)
			{
				_ᡧ = itemAt;
				_ᡧ.ᡖ();
			}
			if (_ᡧ != null)
			{
				_ᡧ.ᡘ(e);
			}
		}
		base.OnMouseMove(e);
	}

	protected override void OnMouseHover(EventArgs e)
	{
		if (_ᡧ != null)
		{
			_ᡧ.ᡜ(e);
		}
		base.OnMouseHover(e);
	}

	protected override void OnMouseLeave()
	{
		if (_ᡦ != null)
		{
			_ᡦ.ᡗ();
		}
		else if (_ᡧ != null)
		{
			_ᡧ.ᡗ();
			_ᡧ = null;
		}
		base.OnMouseLeave();
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		if (_ᡩ != null)
		{
			_ᡩ.ᡟ(e);
		}
		base.OnKeyDown(e);
	}

	protected override void OnKeyUp(KeyEventArgs e)
	{
		if (_ᡩ != null)
		{
			_ᡩ.ᡠ(e);
		}
		base.OnKeyUp(e);
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		if (_ᡩ != null)
		{
			_ᡩ.ᡡ(e);
		}
		base.OnKeyPress(e);
	}

	protected override bool OnCmdKey(ref Message msg, Keys keyData)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		if (_ᡩ != null && _ᡩ.ᘭ(ref msg, keyData))
		{
			return true;
		}
		foreach (VisualItem item in Items)
		{
			if (item is VisualButtonBase && item.ᘭ(ref msg, keyData))
			{
				return true;
			}
		}
		return base.OnCmdKey(ref msg, keyData);
	}

	public VisualItem GetItemAt(int x, int y)
	{
		for (int num = _ᡨ.Count - 1; num >= 0; num--)
		{
			VisualItem visualItem = _ᡨ[num];
			if (visualItem.Visible && visualItem.RenderBounds.Contains(x, y))
			{
				return visualItem;
			}
		}
		return null;
	}

	private VisualItem ఽ()
	{
		foreach (VisualItem item in new ᢇ(_ᡨ, base.IsRightToLeft))
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
			VisualItem visualItem = ఽ();
			if (visualItem != null)
			{
				FocusedItem = visualItem;
			}
		}
	}

	protected override void OnLostFocus()
	{
		FocusedItem = null;
		base.OnLostFocus();
	}

	internal virtual void ᡵ()
	{
		OnInputComplete();
	}

	protected virtual void OnInputComplete()
	{
	}

	internal void ᡶ(VisualInputBase ᡷ)
	{
		OnInputChanged(ᡷ);
	}

	protected virtual void OnInputChanged(VisualInputBase input)
	{
	}
}
