using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Dashboard.Helper;

namespace Stimulsoft.Report.Design.Tools;

[StiServiceBitmap(typeof(StiTextEditorToolService), "Stimulsoft.Report.Design.Bmp.Tools.TextEditor.bmp")]
public class StiTextEditorToolService : StiToolService
{
	public override string ServiceName => Loc.Get("Toolbox", "TextEditor");

	public override int Position => 4;

	public override Keys Shortcut => (Keys)114;

	public StiComponent EditedComponent { get; set; }

	public Control EditorTextBox { get; set; }

	public bool AllowCursorKeys { get; set; }

	public static StiTextEditorToolService GetService()
	{
		return GetService(null);
	}

	public static StiTextEditorToolService GetService(StiDesignerControl designer)
	{
		if (designer == null)
		{
			return StiDesignerOptions.Tools.FirstOrDefault((StiToolService c) => c.GetType() == typeof(StiTextEditorToolService)) as StiTextEditorToolService;
		}
		return designer.Services.GetServices(typeof(StiToolService)).OfType<StiTextEditorToolService>().FirstOrDefault();
	}

	public void TextEditorClose()
	{
		TextEditorClose(cancel: false);
	}

	public void TextEditorClose(bool cancel)
	{
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		if (Designer == null)
		{
			return;
		}
		Designer.InplaceTextEditorActivated = false;
		if (EditorTextBox == null)
		{
			EditedComponent = null;
			return;
		}
		if (!cancel)
		{
			Control editorTextBox = EditorTextBox;
			RichTextBox val = (RichTextBox)(object)((editorTextBox is RichTextBox) ? editorTextBox : null);
			if (val != null)
			{
				((StiRichText)EditedComponent).RtfText = val.Rtf;
				((StiRichText)EditedComponent).ResetImage();
			}
			else if (EditedComponent is IStiTitleElement stiTitleElement)
			{
				stiTitleElement.Title.Text = EditorTextBox.Text;
			}
			else if (EditedComponent is IStiButtonElement stiButtonElement)
			{
				stiButtonElement.Text = EditorTextBox.Text;
			}
			else
			{
				((IStiText)EditedComponent).Text.Value = EditorTextBox.Text;
			}
		}
		EditorTextBox.KeyDown -= new KeyEventHandler(EditorTextBox_KeyDown);
		EditorTextBox.Leave -= EditorTextBox_Leave;
		((Component)(object)EditorTextBox).Dispose();
		EditorTextBox = null;
		EditedComponent = null;
		Designer.TextAliasesHash.Clear();
		StiDesignerControl designer = Designer;
		if (designer == null)
		{
			return;
		}
		StiPageViewControl currentPageView = designer.CurrentPageView;
		if (currentPageView != null)
		{
			StiViewControl view = currentPageView.View;
			if (view != null)
			{
				((Control)view).Focus();
			}
		}
	}

	public void TextEditorOpen(bool isKeystroke = false)
	{
		Point point = ((Control)Designer.CurrentPageView.View).PointToClient(Cursor.Position);
		StiComponent componentInPoint = base.CurrentPage.GetComponentInPoint(Designer.XToPage(point.X), Designer.YToPage(point.Y));
		TextEditorOpen(componentInPoint, isKeystroke);
	}

	public void TextEditorOpen(StiComponent component, bool isKeystroke = false)
	{
		if (component != null && StiRestrictionsHelper.IsAllowChange(component))
		{
			EditedComponent = component;
			Designer.InplaceTextEditorActivated = true;
			if (component is IStiTitleElement)
			{
				OpenEditorForElementTitle();
			}
			else if (component is IStiButtonElement)
			{
				OpenEditorForButtonElement(isKeystroke);
			}
			else if (component is IStiText)
			{
				OpenEditorForTextComponent();
			}
		}
	}

	private void OpenEditorForTextComponent()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		double factor = StiScale.Factor;
		Control val2 = (EditorTextBox = (Control)((EditedComponent is StiRichText) ? new RichTextBox() : new TextBox()));
		val2.KeyDown += new KeyEventHandler(EditorTextBox_KeyDown);
		val2.Leave += EditorTextBox_Leave;
		TextBox val3 = (TextBox)(object)((val2 is TextBox) ? val2 : null);
		RichTextBox val4 = (RichTextBox)(object)((val2 is RichTextBox) ? val2 : null);
		AllowCursorKeys = true;
		if (EditedComponent is StiRichText stiRichText)
		{
			((Control)val4).AutoSize = false;
			((TextBoxBase)val4).BorderStyle = (BorderStyle)0;
			val4.Rtf = stiRichText.RtfText;
			val4.ZoomFactor = (float)base.Info.Zoom;
			if (((Control)val4).Text.Length > 0)
			{
				AllowCursorKeys = false;
			}
		}
		else
		{
			((Control)val3).AutoSize = false;
			((TextBoxBase)val3).BorderStyle = (BorderStyle)0;
			SetHorAlignment(val3, EditedComponent);
			SetFont(val3, EditedComponent);
			SetTextBrush(val2, EditedComponent);
			SetTextOptions(val3, EditedComponent);
			((Control)val3).Text = ((IStiText)EditedComponent).Text.Value;
			if (((Control)val3).Text.Length > 0)
			{
				AllowCursorKeys = false;
			}
		}
		SetBrush(val2, EditedComponent);
		RectangleD rectangleD = EditedComponent.ComponentToPage(EditedComponent.ClientRectangle);
		val2.Left = StiConvert.XToScreen(Designer, rectangleD.X) + 2;
		val2.Top = StiConvert.YToScreen(Designer, rectangleD.Y) + 2;
		val2.Width = (int)(base.CurrentPage.Unit.ConvertToHInches(rectangleD.Width) * base.CurrentPage.Zoom * factor) - 4;
		val2.Height = (int)(base.CurrentPage.Unit.ConvertToHInches(rectangleD.Height) * base.CurrentPage.Zoom * factor) - 4;
		((Control)Designer.CurrentPageView.View).Controls.Add(val2);
		val2.Focus();
		InitializeTextPosition(val2);
	}

	private void OpenEditorForButtonElement(bool isKeystroke = false)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		double factor = StiScale.Factor;
		IStiButtonElement stiButtonElement = EditedComponent as IStiButtonElement;
		TextBox val = new TextBox();
		((Control)val).AutoSize = true;
		((TextBoxBase)val).BorderStyle = (BorderStyle)0;
		((Control)val).Text = stiButtonElement.Text;
		val.TextAlign = GetHorizontalAlignment((stiButtonElement as IStiTextHorAlignment).HorAlignment);
		((Control)val).Font = stiButtonElement.Font;
		TextBox val2 = val;
		((Control)val2).KeyDown += new KeyEventHandler(EditorTextBox_KeyDown);
		((Control)val2).Leave += EditorTextBox_Leave;
		EditorTextBox = (Control)(object)val2;
		SetBrush((Control)(object)val2, stiButtonElement);
		SetTextBrush((Control)(object)val2, stiButtonElement);
		SetFont(val2, stiButtonElement);
		RectangleD rect = EditedComponent.ComponentToPage(EditedComponent.ClientRectangle);
		rect = StiMarginHelper.ApplyMargin(EditedComponent as IStiElement, rect);
		rect = StiBorderElementHelper.GetBorderContentRect(rect, EditedComponent as IStiElement);
		((Control)val2).Left = StiConvert.XToScreen(Designer, rect.X);
		((Control)val2).Width = (int)(base.CurrentPage.Unit.ConvertToHInches(rect.Width) * base.CurrentPage.Zoom * factor);
		int num = (int)(base.CurrentPage.Unit.ConvertToHInches(rect.Height) * base.CurrentPage.Zoom * factor);
		((Control)val2).Top = StiConvert.YToScreen(Designer, rect.Y) + (num - ((Control)val2).Height) / 2;
		StiCornerRadius stiCornerRadius = (EditedComponent as IStiCornerRadius)?.CornerRadius;
		if (stiCornerRadius != null && (stiCornerRadius.TopLeft > 0f || stiCornerRadius.TopRight > 0f))
		{
			((Control)val2).Left = ((Control)val2).Left + StiScale.I((double)stiCornerRadius.TopLeft * base.CurrentPage.Zoom);
			((Control)val2).Width = ((Control)val2).Width - StiScale.I((double)(stiCornerRadius.TopLeft + stiCornerRadius.TopRight) * base.CurrentPage.Zoom);
		}
		((Control)Designer.CurrentPageView.View).Controls.Add((Control)(object)val2);
		((Control)val2).Focus();
		if (isKeystroke)
		{
			((TextBoxBase)val2).SelectionStart = ((Control)val2).Text.Length;
			((TextBoxBase)val2).SelectionLength = 0;
		}
		InitializeTextPosition((Control)(object)val2);
	}

	private void OpenEditorForElementTitle()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		double factor = StiScale.Factor;
		IStiTitleElement stiTitleElement = EditedComponent as IStiTitleElement;
		TextBox val = new TextBox
		{
			AutoSize = true,
			BorderStyle = (BorderStyle)0,
			Text = stiTitleElement.Title.Text,
			TextAlign = GetHorizontalAlignment(stiTitleElement.Title.HorAlignment),
			Font = stiTitleElement.Title.Font
		};
		((Control)val).KeyDown += new KeyEventHandler(EditorTextBox_KeyDown);
		((Control)val).Leave += EditorTextBox_Leave;
		EditorTextBox = (Control)(object)val;
		SetBrush((Control)(object)val, EditedComponent as IStiElement, stiTitleElement.Title);
		SetTextBrush((Control)(object)val, EditedComponent as IStiElement, stiTitleElement.Title);
		SetHorAlignment(val, stiTitleElement.Title);
		SetFont(val, stiTitleElement.Title);
		SetTextOptions(val, stiTitleElement.Title);
		SizeD? sizeD = StiTitleMeasureHelper.MeasureTitle(stiTitleElement.Title);
		RectangleD rect = EditedComponent.ComponentToPage(EditedComponent.ClientRectangle);
		rect = StiMarginHelper.ApplyMargin(EditedComponent as IStiElement, rect);
		rect = StiBorderElementHelper.GetBorderContentRect(rect, EditedComponent as IStiElement);
		rect.X += StiScale.I2;
		((Control)val).Left = StiConvert.XToScreen(Designer, rect.X);
		((Control)val).Top = StiConvert.YToScreen(Designer, rect.Y);
		if (stiTitleElement.Title.Visible)
		{
			((Control)val).Top = ((Control)val).Top + (int)(sizeD.Value.Height * base.CurrentPage.Zoom - (double)((Control)val).Height) / 2;
		}
		((Control)val).Width = (int)(base.CurrentPage.Unit.ConvertToHInches(rect.Width) * base.CurrentPage.Zoom * factor) - (int)sizeD.Value.Height;
		StiCornerRadius stiCornerRadius = (EditedComponent as IStiCornerRadius)?.CornerRadius;
		if (stiCornerRadius != null && (stiCornerRadius.TopLeft > 0f || stiCornerRadius.TopRight > 0f))
		{
			((Control)val).Left = ((Control)val).Left + StiScale.I((double)stiCornerRadius.TopLeft * base.CurrentPage.Zoom);
			((Control)val).Width = ((Control)val).Width - StiScale.I((double)(stiCornerRadius.TopLeft + stiCornerRadius.TopRight) * base.CurrentPage.Zoom);
		}
		((Control)Designer.CurrentPageView.View).Controls.Add((Control)(object)val);
		((Control)val).Focus();
		InitializeTextPosition((Control)(object)val);
	}

	private static void InitializeTextPosition(Control control)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		if (!(control is TextBox))
		{
			return;
		}
		TextBox val = (TextBox)control;
		Point point = ((Control)val).PointToClient(Cursor.Position);
		if (point.X > 0 && point.Y > 0 && point.X < control.Width && point.Y < control.Height)
		{
			int charIndexFromPosition = ((TextBoxBase)val).GetCharIndexFromPosition(point);
			if (charIndexFromPosition != -1)
			{
				((TextBoxBase)val).Select(charIndexFromPosition, 0);
			}
		}
	}

	private void SetTextOptions(TextBox textBox, object component)
	{
		if (component is IStiTextOptions stiTextOptions)
		{
			bool wordWrap2 = (((TextBoxBase)textBox).Multiline = stiTextOptions.TextOptions.WordWrap);
			((TextBoxBase)textBox).WordWrap = wordWrap2;
		}
	}

	private void SetTextBrush(Control control, object component)
	{
		Color? color = null;
		if (component is IStiTitle stiTitle)
		{
			color = stiTitle.ForeColor;
		}
		else if (component is IStiForeColor stiForeColor)
		{
			color = stiForeColor.ForeColor;
		}
		else if (component is IStiTextBrush stiTextBrush)
		{
			color = StiBrush.ToColor(stiTextBrush.TextBrush);
		}
		if (color.HasValue)
		{
			control.ForeColor = ((color.Value == Color.Transparent) ? GetForegroundColor(component).RemoveAlpha() : color.Value.RemoveAlpha());
		}
	}

	private void SetTextBrush(Control control, IStiElement element, IStiTitle title)
	{
		Color color = ((title.ForeColor != Color.Transparent) ? title.ForeColor : StiDashboardStyleHelper.GetForeColor(element));
		control.ForeColor = color.RemoveAlpha();
	}

	private void SetHorAlignment(TextBox textBox, object component)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		if (component is IStiHorAlignment stiHorAlignment)
		{
			textBox.TextAlign = GetHorizontalAlignment(stiHorAlignment.HorAlignment);
		}
		if (component is IStiTextHorAlignment stiTextHorAlignment)
		{
			switch (stiTextHorAlignment.HorAlignment)
			{
			case StiTextHorAlignment.Left:
				textBox.TextAlign = (HorizontalAlignment)0;
				break;
			case StiTextHorAlignment.Center:
			case StiTextHorAlignment.Width:
				textBox.TextAlign = (HorizontalAlignment)2;
				break;
			case StiTextHorAlignment.Right:
				textBox.TextAlign = (HorizontalAlignment)1;
				break;
			}
		}
	}

	private void SetFont(TextBox textBox, object component)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		if (component is IStiFont)
		{
			Font font = ((IStiFont)component).Font;
			((Control)textBox).Font = new Font(font.FontFamily, (float)((double)font.Size * base.CurrentPage.Zoom), font.Style, font.Unit, font.GdiCharSet, font.GdiVerticalFont);
		}
	}

	private void SetBrush(Control control, object component)
	{
		Color? color = null;
		if (component is IStiTitle stiTitle)
		{
			color = stiTitle.BackColor;
		}
		else if (component is IStiBackColor stiBackColor)
		{
			color = stiBackColor.BackColor;
		}
		else if (component is IStiBrush stiBrush)
		{
			color = StiBrush.ToColor(stiBrush.Brush);
		}
		if (color.HasValue)
		{
			control.BackColor = ((color.Value == Color.Transparent) ? GetDefaultBackgroundColor(component).RemoveAlpha() : color.Value.RemoveAlpha());
		}
	}

	private void SetBrush(Control control, IStiElement element, IStiTitle title)
	{
		Color color = ((title.BackColor != Color.Transparent) ? title.BackColor : StiDashboardStyleHelper.GetBackColor(element));
		control.BackColor = color.RemoveAlpha();
	}

	private HorizontalAlignment GetHorizontalAlignment(StiHorAlignment horAlignment)
	{
		return (HorizontalAlignment)(horAlignment switch
		{
			StiHorAlignment.Left => 0, 
			StiHorAlignment.Center => 2, 
			StiHorAlignment.Right => 1, 
			_ => 0, 
		});
	}

	private HorizontalAlignment GetHorizontalAlignment(StiTextHorAlignment horAlignment)
	{
		switch (horAlignment)
		{
		case StiTextHorAlignment.Left:
			return (HorizontalAlignment)0;
		case StiTextHorAlignment.Center:
		case StiTextHorAlignment.Width:
			return (HorizontalAlignment)2;
		case StiTextHorAlignment.Right:
			return (HorizontalAlignment)1;
		default:
			return (HorizontalAlignment)0;
		}
	}

	private static Color GetForegroundColor(object component)
	{
		if (component is IStiElement element)
		{
			return StiDashboardStyleHelper.GetForeColor(element);
		}
		return StiElementConsts.ForegroundColor;
	}

	private static Color GetDefaultBackgroundColor(object component)
	{
		if (component is IStiElement element)
		{
			return StiDashboardStyleHelper.GetBackColor(element);
		}
		return StiElementConsts.BackgroundColor;
	}

	public override Cursor GetCursor()
	{
		return Cursors.IBeam;
	}

	public void StartTextEditor(StiComponent component, bool isKeystroke = false)
	{
		if (EditorTextBox != null)
		{
			TextEditorClose();
		}
		Designer.UndoRedoSave(Loc.Get("Toolbox", "TextEditor"), onlyCurrentPage: true);
		TextEditorOpen(component, isKeystroke);
	}

	protected void EditorTextBox_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Invalid comparison between Unknown and I4
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			TextEditorClose(cancel: true);
		}
		if ((int)e.KeyCode == 13)
		{
			TextEditorClose();
		}
		if (AllowCursorKeys)
		{
			if ((int)e.KeyCode == 37)
			{
				TextEditorClose();
				StiSelectionTransfer.MoveLeft(Designer);
			}
			if ((int)e.KeyCode == 39)
			{
				TextEditorClose();
				StiSelectionTransfer.MoveRight(Designer);
			}
			if ((int)e.KeyCode == 38)
			{
				TextEditorClose();
				StiSelectionTransfer.MoveUp(Designer);
			}
			if ((int)e.KeyCode == 40)
			{
				TextEditorClose();
				StiSelectionTransfer.MoveDown(Designer);
			}
		}
	}

	protected void EditorTextBox_Leave(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		Timer timer = new Timer();
		timer.Interval = 500;
		timer.Tick += delegate
		{
			timer.Stop();
			((Component)(object)timer).Dispose();
			TextEditorClose();
		};
		timer.Start();
	}

	protected override void OnDeactivated(EventArgs e)
	{
		TextEditorClose();
	}

	public override void OnMouseDown(object sender, MouseEventArgs e)
	{
		base.OnMouseDown(sender, e);
		if (EditorTextBox != null)
		{
			Point point = ((Control)Designer.CurrentPageView.View).PointToClient(Cursor.Position);
			if (!new Rectangle(EditorTextBox.Location, EditorTextBox.Size).Contains(point.X, point.Y))
			{
				TextEditorClose();
			}
		}
		Designer.UndoRedoSave(Loc.Get("Toolbox", "TextEditor"), onlyCurrentPage: true);
		TextEditorOpen();
	}
}
