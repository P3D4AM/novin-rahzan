using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Wizards;

namespace Stimulsoft.Report.Design;

public sealed class StiHighlightComponentService : IStiHighlightComponentService
{
	private StiDesignerControl designer;

	private StiText highlightedComp;

	private StiText dragingComponent;

	private StiComponent labelComp;

	private StiPopupForm popupWindow;

	private StiHighlightComponentControl highlightComponentControl;

	private string currentText;

	private string additionalText;

	public bool IsOpen { get; private set; }

	private void ButtonReplaceContent_Click(object sender, EventArgs e)
	{
		SetNewText(additionalText, removeComponents: true);
	}

	private void ButtonAddContentLeft_Click(object sender, EventArgs e)
	{
		SetNewText(additionalText + currentText, removeComponents: true);
	}

	private void ButtonAddContentRight_Click(object sender, EventArgs e)
	{
		SetNewText(currentText + additionalText, removeComponents: true);
	}

	private void ButtonAddContentNewLine_Click(object sender, EventArgs e)
	{
		SetNewText(currentText + "\r\n" + additionalText, removeComponents: true);
	}

	private void ButtonCancel_Click(object sender, EventArgs e)
	{
		SetNewText(currentText, removeComponents: false);
	}

	private void ButtonReplaceContent_MouseEnter(object sender, EventArgs e)
	{
		SetContent(additionalText);
	}

	private void ButtonAddContentLeft_MouseEnter(object sender, EventArgs e)
	{
		SetContent(additionalText + currentText);
	}

	private void ButtonAddContentRight_MouseEnter(object sender, EventArgs e)
	{
		SetContent(currentText + additionalText);
	}

	private void ButtonAddContentNewLine_MouseEnter(object sender, EventArgs e)
	{
		SetContent(currentText + "\r\n" + additionalText);
	}

	private void Button_MouseLeave(object sender, EventArgs e)
	{
		SetContent(currentText);
	}

	private void PopupWindow_ClosePopup(object sender, EventArgs e)
	{
		IsOpen = false;
		popupWindow.ClosePopup -= PopupWindow_ClosePopup;
		UnSubscribeToEvent();
		designer.SelectTool.ResetHighlightState();
		((Control)designer).Refresh();
	}

	public void Activate(StiText highlightedComp, StiText dragingComponent, StiComponent labelComp)
	{
		this.highlightedComp = highlightedComp;
		this.dragingComponent = dragingComponent;
		this.labelComp = labelComp;
		currentText = highlightedComp.Text.Value;
		additionalText = dragingComponent.Text.Value;
		popupWindow = null;
		highlightComponentControl = null;
		popupWindow = new StiPopupForm();
		popupWindow.ClosePopup += PopupWindow_ClosePopup;
		popupWindow.IsDrawShadowInPopupWindow = false;
		highlightComponentControl = new StiHighlightComponentControl();
		((Control)highlightComponentControl.btReplaceContent).Click += ButtonReplaceContent_Click;
		((Control)highlightComponentControl.btAddContentLeft).Click += ButtonAddContentLeft_Click;
		((Control)highlightComponentControl.btAddContentRight).Click += ButtonAddContentRight_Click;
		((Control)highlightComponentControl.btAddContentNewLine).Click += ButtonAddContentNewLine_Click;
		((Control)highlightComponentControl.btCancel).Click += ButtonCancel_Click;
		((Control)highlightComponentControl.btAddContentLeft).Enabled = !string.IsNullOrEmpty(dragingComponent.Text.Value);
		((Control)highlightComponentControl.btAddContentRight).Enabled = !string.IsNullOrEmpty(dragingComponent.Text.Value);
		((Control)highlightComponentControl.btAddContentNewLine).Enabled = !string.IsNullOrEmpty(dragingComponent.Text.Value);
		((Control)popupWindow).Width = 182;
		((Control)popupWindow).Height = 79;
		((Control)popupWindow).Controls.Add((Control)(object)highlightComponentControl);
		int num = 0;
		int num2 = 0;
		StiPage page = highlightedComp.Page;
		RectangleD paintRectangle = highlightedComp.GetPaintRectangle();
		Rectangle rectangle = ((Control)designer.PageView.GetCurrentPageView().View).RectangleToScreen(new Rectangle((int)paintRectangle.X, (int)paintRectangle.Y, (int)paintRectangle.Width, (int)paintRectangle.Height));
		int num3 = (int)(page.Unit.ConvertToHInches(page.Margins.Top) * page.Zoom);
		int num4 = (int)(page.Unit.ConvertToHInches(page.Margins.Left) * page.Zoom);
		if ((double)(rectangle.Y + ((Control)popupWindow).Height) + paintRectangle.Height + 5.0 > (double)Screen.PrimaryScreen.WorkingArea.Height)
		{
			if (rectangle.Y - ((Control)popupWindow).Height - 5 < 0)
			{
				num = Cursor.Position.X;
				num2 = Cursor.Position.Y;
			}
			else
			{
				num = rectangle.X + (int)((paintRectangle.Width - (double)((Control)popupWindow).Width) / 2.0) + num4;
				num2 = rectangle.Y - (((Control)popupWindow).Height + 5) + num3;
			}
		}
		else
		{
			num = rectangle.X + (int)((paintRectangle.Width - (double)((Control)popupWindow).Width) / 2.0) + num4;
			num2 = rectangle.Y + ((int)paintRectangle.Height + 5 + num3);
		}
		popupWindow.SetLocation(num, num2, 182, 79);
		SubscribeToEvent();
		popupWindow.ShowPopupForm();
		IsOpen = true;
	}

	private void SetNewText(string text, bool removeComponents)
	{
		highlightedComp.SetTextInternal(text);
		if (removeComponents)
		{
			RemoveTemporaryComponents();
		}
		ClosePopup();
	}

	private void RemoveTemporaryComponents()
	{
		if (dragingComponent.Parent != null)
		{
			dragingComponent.Parent.Components.Remove(dragingComponent);
		}
		if (labelComp != null && labelComp.Parent != null)
		{
			labelComp.Parent.Components.Remove(labelComp);
		}
	}

	public void ClosePopup()
	{
		IsOpen = false;
		popupWindow.ClosePopup -= PopupWindow_ClosePopup;
		UnSubscribeToEvent();
		popupWindow?.ClosePopupForm();
		designer.SelectTool.ResetHighlightState();
		((Control)designer).Refresh();
	}

	private void SetContent(string text)
	{
		highlightedComp.Text.Value = text;
		((Control)designer).Refresh();
	}

	private void SubscribeToEvent()
	{
		((Control)highlightComponentControl.btReplaceContent).MouseEnter += ButtonReplaceContent_MouseEnter;
		((Control)highlightComponentControl.btReplaceContent).MouseLeave += Button_MouseLeave;
		((Control)highlightComponentControl.btAddContentLeft).MouseEnter += ButtonAddContentLeft_MouseEnter;
		((Control)highlightComponentControl.btAddContentLeft).MouseLeave += Button_MouseLeave;
		((Control)highlightComponentControl.btAddContentRight).MouseEnter += ButtonAddContentRight_MouseEnter;
		((Control)highlightComponentControl.btAddContentRight).MouseLeave += Button_MouseLeave;
		((Control)highlightComponentControl.btAddContentNewLine).MouseEnter += ButtonAddContentNewLine_MouseEnter;
		((Control)highlightComponentControl.btAddContentNewLine).MouseLeave += Button_MouseLeave;
	}

	private void UnSubscribeToEvent()
	{
		((Control)highlightComponentControl.btReplaceContent).MouseEnter -= ButtonReplaceContent_MouseEnter;
		((Control)highlightComponentControl.btReplaceContent).MouseLeave -= Button_MouseLeave;
		((Control)highlightComponentControl.btAddContentLeft).MouseEnter -= ButtonAddContentLeft_MouseEnter;
		((Control)highlightComponentControl.btAddContentLeft).MouseLeave -= Button_MouseLeave;
		((Control)highlightComponentControl.btAddContentRight).MouseEnter -= ButtonAddContentRight_MouseEnter;
		((Control)highlightComponentControl.btAddContentRight).MouseLeave -= Button_MouseLeave;
		((Control)highlightComponentControl.btAddContentNewLine).MouseEnter -= ButtonAddContentNewLine_MouseEnter;
		((Control)highlightComponentControl.btAddContentNewLine).MouseLeave -= Button_MouseLeave;
	}

	public static StiComponent GetHighlightedComponent(StiPage page)
	{
		return page.GetComponents().ToList().FirstOrDefault((StiComponent c) => c.HighlightState == StiHighlightState.Active);
	}

	public StiHighlightComponentService(StiDesignerControl designer)
	{
		this.designer = designer;
	}
}
