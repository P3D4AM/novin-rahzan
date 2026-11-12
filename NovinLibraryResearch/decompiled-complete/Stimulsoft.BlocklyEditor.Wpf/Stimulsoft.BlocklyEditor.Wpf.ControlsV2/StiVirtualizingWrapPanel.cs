using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace Stimulsoft.BlocklyEditor.Wpf.ControlsV2;

[DesignTimeVisible(false)]
public class StiVirtualizingWrapPanel : VirtualizingPanel, IScrollInfo
{
	private TranslateTransform trans = new TranslateTransform();

	private Size extent = new Size(0.0, 0.0);

	private Size viewport = new Size(0.0, 0.0);

	private Point offset;

	public static readonly DependencyProperty ScrollOffsetProperty = DependencyProperty.RegisterAttached("ScrollOffset", typeof(int), typeof(StiVirtualizingWrapPanel), new PropertyMetadata((object)0));

	public int ChildWidth { get; set; } = 30;


	public int ChildHeight { get; set; } = 30;


	public ScrollViewer ScrollOwner { get; set; }

	public bool CanHorizontallyScroll { get; set; }

	public bool CanVerticallyScroll { get; set; }

	public double HorizontalOffset => ((Point)(ref offset)).X;

	public double VerticalOffset => ((Point)(ref offset)).Y;

	public double ExtentHeight => ((Size)(ref extent)).Height;

	public double ExtentWidth => ((Size)(ref extent)).Width;

	public double ViewportHeight => ((Size)(ref viewport)).Height;

	public double ViewportWidth => ((Size)(ref viewport)).Width;

	public int ScrollOffset
	{
		get
		{
			return Convert.ToInt32(((DependencyObject)this).GetValue(ScrollOffsetProperty));
		}
		set
		{
			((DependencyObject)this).SetValue(ScrollOffsetProperty, (object)value);
		}
	}

	public StiVirtualizingWrapPanel()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		((UIElement)this).RenderTransform = (Transform)(object)trans;
	}

	protected override Size MeasureOverride(Size availableSize)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			UpdateScrollInfo(availableSize);
			int firstVisibleItemIndex = 0;
			int lastVisibleItemIndex = 0;
			GetVisibleRange(ref firstVisibleItemIndex, ref lastVisibleItemIndex);
			UIElementCollection internalChildren = ((Panel)this).InternalChildren;
			IItemContainerGenerator itemContainerGenerator = ((VirtualizingPanel)this).ItemContainerGenerator;
			GeneratorPosition val = itemContainerGenerator.GeneratorPositionFromIndex(firstVisibleItemIndex);
			int num = ((((GeneratorPosition)(ref val)).Offset == 0) ? ((GeneratorPosition)(ref val)).Index : (((GeneratorPosition)(ref val)).Index + 1));
			using (itemContainerGenerator.StartAt(val, (GeneratorDirection)0, true))
			{
				int num2 = firstVisibleItemIndex;
				bool flag = default(bool);
				while (num2 <= lastVisibleItemIndex)
				{
					DependencyObject obj = itemContainerGenerator.GenerateNext(ref flag);
					UIElement val2 = (UIElement)(object)((obj is UIElement) ? obj : null);
					if (flag)
					{
						if (num >= internalChildren.Count)
						{
							((VirtualizingPanel)this).AddInternalChild(val2);
						}
						else
						{
							((VirtualizingPanel)this).InsertInternalChild(num, val2);
						}
						itemContainerGenerator.PrepareItemContainer((DependencyObject)(object)val2);
					}
					val2.Measure(GetChildSize());
					num2++;
					num++;
				}
			}
			CleanUpItems(firstVisibleItemIndex, lastVisibleItemIndex);
		}
		catch (ArgumentOutOfRangeException)
		{
		}
		catch (NullReferenceException)
		{
		}
		return new Size(double.IsInfinity(((Size)(ref availableSize)).Width) ? 0.0 : ((Size)(ref availableSize)).Width, double.IsInfinity(((Size)(ref availableSize)).Height) ? 0.0 : ((Size)(ref availableSize)).Height);
	}

	protected override Size ArrangeOverride(Size finalSize)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		IItemContainerGenerator itemContainerGenerator = ((VirtualizingPanel)this).ItemContainerGenerator;
		UpdateScrollInfo(finalSize);
		for (int i = 0; i <= ((Panel)this).Children.Count - 1; i++)
		{
			UIElement child = ((Panel)this).Children[i];
			int itemIndex = itemContainerGenerator.IndexFromGeneratorPosition(new GeneratorPosition(i, 0));
			ArrangeChild(itemIndex, child, finalSize);
		}
		return finalSize;
	}

	protected override void OnItemsChanged(object sender, ItemsChangedEventArgs args)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		GeneratorPosition val;
		switch (args.Action)
		{
		case NotifyCollectionChangedAction.Remove:
		case NotifyCollectionChangedAction.Replace:
			val = args.Position;
			((VirtualizingPanel)this).RemoveInternalChildRange(((GeneratorPosition)(ref val)).Index, args.ItemUICount);
			break;
		case NotifyCollectionChangedAction.Move:
			val = args.OldPosition;
			((VirtualizingPanel)this).RemoveInternalChildRange(((GeneratorPosition)(ref val)).Index, args.ItemUICount);
			break;
		}
	}

	protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
	{
		((FrameworkElement)this).OnRenderSizeChanged(sizeInfo);
		SetVerticalOffset(VerticalOffset);
	}

	protected override void OnClearChildren()
	{
		((VirtualizingPanel)this).OnClearChildren();
		SetVerticalOffset(0.0);
	}

	protected override void BringIndexIntoView(int index)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		if (index >= 0 && index < ((Panel)this).Children.Count)
		{
			int num = CalculateChildrenPerRow(((UIElement)this).RenderSize);
			int num2 = index / num;
			SetVerticalOffset(num2 * ChildHeight);
		}
	}

	private void CleanUpItems(int minDesiredGenerated, int maxDesiredGenerated)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		UIElementCollection internalChildren = ((Panel)this).InternalChildren;
		IItemContainerGenerator itemContainerGenerator = ((VirtualizingPanel)this).ItemContainerGenerator;
		int itemCount = GetItemCount();
		int num = CalculateChildrenPerRow(extent);
		if (minDesiredGenerated - 2 * num > 0)
		{
			minDesiredGenerated -= 2 * num;
		}
		if (maxDesiredGenerated + 2 * num < itemCount)
		{
			maxDesiredGenerated += 2 * num;
		}
		GeneratorPosition val = default(GeneratorPosition);
		for (int i = internalChildren.Count - 1; i >= 0; i += -1)
		{
			((GeneratorPosition)(ref val))._002Ector(i, 0);
			int num2 = itemContainerGenerator.IndexFromGeneratorPosition(val);
			if ((num2 > 2 * num - 1 && num2 < minDesiredGenerated) || (num2 < itemCount - 2 * num - 1 && num2 > maxDesiredGenerated))
			{
				itemContainerGenerator.Remove(val, 1);
				((VirtualizingPanel)this).RemoveInternalChildRange(i, 1);
			}
		}
	}

	private Size CalculateExtent(Size availableSize, int itemCount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		int num = CalculateChildrenPerRow(availableSize);
		return new Size((double)(num * ChildWidth), (double)ChildHeight * Math.Ceiling(Convert.ToDouble(itemCount) / (double)num));
	}

	private void GetVisibleRange(ref int firstVisibleItemIndex, ref int lastVisibleItemIndex)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		if (double.IsInfinity(((Size)(ref viewport)).Height))
		{
			return;
		}
		int num = CalculateChildrenPerRow(extent);
		try
		{
			firstVisibleItemIndex = Convert.ToInt32(Math.Floor(((Point)(ref offset)).Y / (double)ChildHeight)) * num;
			lastVisibleItemIndex = Convert.ToInt32(Math.Ceiling((((Point)(ref offset)).Y + ((Size)(ref viewport)).Height) / (double)ChildHeight)) * num - 1;
			int itemCount = GetItemCount();
			if (lastVisibleItemIndex >= itemCount)
			{
				lastVisibleItemIndex = itemCount - 1;
			}
		}
		catch
		{
		}
	}

	private Size GetChildSize()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		return new Size((double)ChildWidth, (double)ChildHeight);
	}

	private void ArrangeChild(int itemIndex, UIElement child, Size finalSize)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		int num = CalculateChildrenPerRow(finalSize);
		int num2 = itemIndex / num;
		int num3 = itemIndex % num;
		double num4 = num3 * ChildWidth;
		child.Arrange(new Rect(num4, (double)(num2 * ChildHeight), (double)ChildWidth, (double)ChildHeight));
	}

	private int CalculateChildrenPerRow(Size availableSize)
	{
		if (((Size)(ref availableSize)).Width != double.PositiveInfinity)
		{
			return Math.Max(1, (int)(((Size)(ref availableSize)).Width / (double)ChildWidth));
		}
		return ((Panel)this).Children.Count;
	}

	private int GetItemCount()
	{
		ItemsControl itemsOwner = ItemsControl.GetItemsOwner((DependencyObject)(object)this);
		if (!itemsOwner.HasItems)
		{
			return 0;
		}
		return ((CollectionView)itemsOwner.Items).Count;
	}

	private void UpdateScrollInfo(Size availableSize)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		int itemCount = GetItemCount();
		Size val = CalculateExtent(availableSize, itemCount);
		if (val != extent)
		{
			extent = val;
			if (ScrollOwner != null)
			{
				ScrollOwner.InvalidateScrollInfo();
			}
		}
		if (availableSize != viewport)
		{
			viewport = availableSize;
			if (ScrollOwner != null)
			{
				ScrollOwner.InvalidateScrollInfo();
			}
		}
	}

	public void LineUp()
	{
		SetVerticalOffset(VerticalOffset - (double)((ScrollOffset > 0) ? ScrollOffset : ChildHeight));
	}

	public void LineDown()
	{
		SetVerticalOffset(VerticalOffset + (double)((ScrollOffset > 0) ? ScrollOffset : ChildHeight));
	}

	public void PageUp()
	{
		SetVerticalOffset(VerticalOffset - ((Size)(ref viewport)).Height);
	}

	public void PageDown()
	{
		SetVerticalOffset(VerticalOffset + ((Size)(ref viewport)).Height);
	}

	public void MouseWheelUp()
	{
		EasingSetVerticalOffset(VerticalOffset - (double)((ScrollOffset > 0) ? ScrollOffset : ChildHeight));
	}

	public void MouseWheelDown()
	{
		EasingSetVerticalOffset(VerticalOffset + (double)((ScrollOffset > 0) ? ScrollOffset : ChildHeight));
	}

	public void LineLeft()
	{
		throw new InvalidOperationException();
	}

	public void LineRight()
	{
		throw new InvalidOperationException();
	}

	public Rect MakeVisible(Visual visual, Rect rectangle)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return default(Rect);
	}

	public void MouseWheelLeft()
	{
		throw new InvalidOperationException();
	}

	public void MouseWheelRight()
	{
		throw new InvalidOperationException();
	}

	public void PageLeft()
	{
		throw new InvalidOperationException();
	}

	public void PageRight()
	{
		throw new InvalidOperationException();
	}

	public void SetHorizontalOffset(double offset)
	{
		throw new InvalidOperationException();
	}

	public void EasingSetVerticalOffset(double offset)
	{
		AdjustVerticalOffset(ref offset);
		trans.Y = 0.0 - offset;
		((UIElement)this).InvalidateMeasure();
	}

	public void SetVerticalOffset(double offset)
	{
		AdjustVerticalOffset(ref offset);
		trans.Y = 0.0 - offset;
		((UIElement)this).InvalidateMeasure();
	}

	private void AdjustVerticalOffset(ref double offset)
	{
		if (offset < 0.0 || ((Size)(ref viewport)).Height >= ((Size)(ref extent)).Height)
		{
			offset = 0.0;
		}
		else if (offset + ((Size)(ref viewport)).Height >= ((Size)(ref extent)).Height)
		{
			offset = ((Size)(ref extent)).Height - ((Size)(ref viewport)).Height;
		}
		((Point)(ref this.offset)).Y = offset;
		ScrollViewer scrollOwner = ScrollOwner;
		if (scrollOwner != null)
		{
			scrollOwner.InvalidateScrollInfo();
		}
	}
}
