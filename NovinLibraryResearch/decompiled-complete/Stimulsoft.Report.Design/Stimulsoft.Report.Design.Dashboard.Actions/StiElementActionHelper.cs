using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Report.Design.Dashboard.Actions;

internal class StiElementActionHelper
{
	private static object lockObject = new object();

	private static StiElementActionForm CurrentForm { get; set; }

	internal static StiComponent CurrentComponent { get; set; }

	public static void CheckStatus(StiComponent comp = null, StiDesignerControl designer = null)
	{
		lock (lockObject)
		{
			StiPage stiPage = designer?.Report?.GetCurrentPage();
			if (comp == null)
			{
				comp = stiPage?.GetSelectedComponents()?.ToList()?.FirstOrDefault();
			}
			if (comp?.Report != null && comp.IsSelected && StiInplaceEditor.IsEditorAllowed(comp) && stiPage != null && stiPage.IsDashboardOrScreen && !StiInplaceEditor.IsEditorActivated && designer != null && IsAllowedAction(designer) && comp.Report.GetCurrentPage().OffsetRectangle.IsEmpty)
			{
				Show(comp, designer);
			}
			else
			{
				Hide();
			}
		}
	}

	private static bool IsAllowedAction(StiDesignerControl designer)
	{
		if (designer.SelectTool.Action != 0 && designer.SelectTool.Action != StiAction.Select)
		{
			return designer.SelectTool.Action == StiAction.Move;
		}
		return true;
	}

	public static void Show(StiComponent comp, StiDesignerControl designer)
	{
		lock (lockObject)
		{
			if (CurrentForm == null || ((Control)CurrentForm).Disposing || ((Control)CurrentForm).IsDisposed)
			{
				CurrentForm = new StiElementActionForm();
			}
			CurrentComponent = comp;
			Size size = CurrentForm.RecalculateButtons();
			((Form)CurrentForm).Size = size;
			((Form)CurrentForm).Location = GetScreenLocation(comp, designer, size);
			Form val = ((designer != null) ? ((Control)designer).FindForm() : null);
			if (val != null)
			{
				Rectangle rectangle = new Rectangle(val.Location, val.Size);
				Rectangle rect = new Rectangle(((Form)CurrentForm).Location, ((Form)CurrentForm).Size);
				if (!rectangle.IntersectsWith(rect))
				{
					return;
				}
			}
			if (!((Control)CurrentForm).Visible)
			{
				((Form)CurrentForm).Show((IWin32Window)(object)val);
			}
		}
	}

	public static void Localize()
	{
		CurrentForm?.Localize();
	}

	private static Point GetScreenLocation(StiComponent comp, StiDesignerControl designer, Size size)
	{
		Point result = StiInplaceDesigner.GetScreenPosition(comp, size);
		StiPageViewControl currentPageView = designer.PageView.GetCurrentPageView();
		Rectangle rectangle = ((currentPageView != null) ? ((Control)currentPageView).RectangleToScreen(((Control)currentPageView).ClientRectangle) : ((Control)designer).RectangleToScreen(new Rectangle(0, 0, size.Width, size.Height)));
		if (rectangle.Y > result.Y)
		{
			result.Y = rectangle.Y;
		}
		if (rectangle.Right < result.X)
		{
			result.X = rectangle.Right;
		}
		if (rectangle.Bottom < result.Y + size.Height)
		{
			result.Y = rectangle.Bottom - size.Height;
		}
		Screen val = Screen.FromHandle(((Control)designer).Handle);
		if (result.X + size.Width > val.WorkingArea.Right)
		{
			result = new Point(val.WorkingArea.Right - size.Width, result.Y);
		}
		if (result.Y + size.Height > val.WorkingArea.Bottom)
		{
			result = new Point(result.X, val.WorkingArea.Bottom - size.Height);
		}
		return result;
	}

	public static void Hide()
	{
		lock (lockObject)
		{
			if (CurrentForm != null && ((Control)CurrentForm).Visible)
			{
				((Control)CurrentForm).Visible = false;
			}
		}
	}
}
