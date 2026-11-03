using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiWinControlV1Builder : StiComponentV1Builder
{
	public override bool InternalRender(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		StiWinControl stiWinControl = masterComp as StiWinControl;
		StiImage stiImage = new StiImage(stiWinControl.ClientRectangle);
		stiImage.PutImageToDraw(stiWinControl.GetControlImage());
		stiImage.GetPointer += stiWinControl.InvokeGetPointer;
		stiImage.GetBookmark += stiWinControl.InvokeGetBookmark;
		stiImage.GetTag += stiWinControl.InvokeGetTag;
		stiImage.Click += stiWinControl.InvokeClick;
		stiImage.MouseEnter += stiWinControl.InvokeMouseEnter;
		stiImage.MouseLeave += stiWinControl.InvokeMouseLeave;
		stiImage.DockStyle = stiWinControl.DockStyle;
		stiImage.Name = stiWinControl.Name;
		stiImage.CanGrow = stiWinControl.CanGrow;
		stiImage.CanShrink = stiWinControl.CanShrink;
		stiImage.PointerValue = stiWinControl.PointerValue;
		stiImage.BookmarkValue = stiWinControl.BookmarkValue;
		stiImage.HyperlinkValue = stiWinControl.HyperlinkValue;
		stiImage.ToolTipValue = stiWinControl.ToolTipValue;
		stiImage.TagValue = stiWinControl.TagValue;
		stiImage.InvokeEvents();
		outContainer.Components.Add(stiImage);
		renderedComponent = stiImage;
		return true;
	}
}
