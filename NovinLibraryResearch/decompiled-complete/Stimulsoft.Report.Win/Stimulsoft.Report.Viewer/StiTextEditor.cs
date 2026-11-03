using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Viewer;

internal class StiTextEditor
{
	private StiViewerControl viewerControl;

	public StiComponentsCollection Components { get; set; } = new StiComponentsCollection();


	public int Index { get; set; }

	public Control Editor { get; set; }

	public StiComponent Component
	{
		get
		{
			if (Components.Count <= Index)
			{
				return null;
			}
			return Components[Index];
		}
	}

	public void Next()
	{
		if (Component != null)
		{
			Component.HighlightState = StiHighlightState.Show;
			Index++;
			if (Index >= Components.Count)
			{
				Index = 0;
			}
			Component.HighlightState = StiHighlightState.Active;
		}
	}

	public void Prev()
	{
		if (Component != null)
		{
			Component.HighlightState = StiHighlightState.Show;
			Index--;
			if (Index < 0)
			{
				Index = Components.Count - 1;
			}
			Component.HighlightState = StiHighlightState.Active;
		}
	}

	public void First()
	{
		Index = 0;
		if (Component != null)
		{
			Component.HighlightState = StiHighlightState.Active;
		}
	}

	public void ScrollToCurEditor()
	{
		if (Editor != null && StiOptions.Viewer.Windows.ScrollToEditableComponent)
		{
			viewerControl.ScrollToRect(Editor.Bounds);
		}
	}

	public void CloseEditor()
	{
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		if (Editor == null)
		{
			return;
		}
		if (Components != null && Index >= 0 && Index < Components.Count)
		{
			StiOptions.Engine.GlobalEvents.InvokeEndEditingInPreview(Components[Index], EventArgs.Empty);
			if (Editor is StiCheckBoxComponent)
			{
				StiCheckBox stiCheckBox = (StiCheckBox)Components[Index];
				string[] array = null;
				if (stiCheckBox.Values.IndexOf('/') != -1)
				{
					array = stiCheckBox.Values.Split(new char[1] { '/' });
				}
				else if (stiCheckBox.Values.IndexOf(';') != -1)
				{
					array = stiCheckBox.Values.Split(new char[1] { ';' });
				}
				else if (stiCheckBox.Values.IndexOf(',') != -1)
				{
					array = stiCheckBox.Values.Split(new char[1] { ',' });
				}
				stiCheckBox.CheckedValue = (((StiCheckBoxComponent)Editor).Checked ? array[0] : array[1]);
			}
			else if (Editor is RichTextBox)
			{
				StiRichText stiRichText = (StiRichText)Components[Index];
				stiRichText.RtfText = ((RichTextBox)Editor).Rtf;
				if (stiRichText.Image != null)
				{
					stiRichText.Image.Dispose();
					stiRichText.Image = null;
				}
			}
			else
			{
				((IStiText)Components[Index]).Text.Value = Editor.Text;
			}
		}
		viewerControl.InvokeTextEditorClose(Editor);
		if (((Control)viewerControl.View).Controls.Contains(Editor))
		{
			((Control)viewerControl.View).Controls.Remove(Editor);
		}
		((Component)(object)Editor).Dispose();
		Editor = null;
	}

	public void OpenEditor()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Expected O, but got Unknown
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected I4, but got Unknown
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected I4, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		if (Component == null)
		{
			return;
		}
		StiOptions.Engine.GlobalEvents.InvokeStartEditingInPreview(Component, EventArgs.Empty);
		Color backColor = GetBackColor(Component);
		if (Component is StiCheckBox)
		{
			Editor = (Control)new StiCheckBoxComponent();
		}
		else if (Component is StiRichText)
		{
			Editor = (Control)new StiRichTextBox();
		}
		else
		{
			Editor = (Control)new TextBox();
			((TextBoxBase)(TextBox)Editor).BorderStyle = (BorderStyle)0;
			((TextBoxBase)(TextBox)Editor).Multiline = true;
			((TextBoxBase)(TextBox)Editor).AcceptsTab = true;
			((TextBox)Editor).AcceptsReturn = StiOptions.Viewer.AllowAcceptTabsInEditableMode;
		}
		Editor.Name = Component.Name;
		if (Editor is StiCheckBoxComponent)
		{
			((StiCheckBoxComponent)Editor).CheckedChanged += Editor_CheckedChanged;
			((Control)(StiCheckBoxComponent)Editor).PreviewKeyDown += new PreviewKeyDownEventHandler(Editor_PreviewKeyDown);
			((Control)(StiCheckBoxComponent)Editor).KeyDown += new KeyEventHandler(Editor_KeyDown);
		}
		else
		{
			Editor.PreviewKeyDown += new PreviewKeyDownEventHandler(Editor_PreviewKeyDown);
			Editor.KeyDown += new KeyEventHandler(Editor_KeyDown);
			Editor.TextChanged += Editor_TextChanged;
		}
		if (Editor is TextBox)
		{
			if (Component is IStiHorAlignment)
			{
				StiHorAlignment horAlignment = ((IStiHorAlignment)Component).HorAlignment;
				switch (horAlignment - 1)
				{
				case 0:
					((TextBox)Editor).TextAlign = (HorizontalAlignment)0;
					break;
				case 1:
					((TextBox)Editor).TextAlign = (HorizontalAlignment)2;
					break;
				case 2:
					((TextBox)Editor).TextAlign = (HorizontalAlignment)1;
					break;
				}
			}
			if (Component is IStiTextHorAlignment)
			{
				StiTextHorAlignment horAlignment2 = ((IStiTextHorAlignment)Component).HorAlignment;
				switch ((int)horAlignment2)
				{
				case 0:
					((TextBox)Editor).TextAlign = (HorizontalAlignment)0;
					break;
				case 1:
				case 3:
					((TextBox)Editor).TextAlign = (HorizontalAlignment)2;
					break;
				case 2:
					((TextBox)Editor).TextAlign = (HorizontalAlignment)1;
					break;
				}
			}
		}
		Refresh();
		Editor.BackColor = backColor;
		if (Editor is StiCheckBoxComponent)
		{
			StiCheckBox stiCheckBox = (StiCheckBox)Component;
			((StiCheckBoxComponent)Editor).CheckStyleForTrue = (StiCheckValueStyle)stiCheckBox.CheckStyleForTrue;
			((StiCheckBoxComponent)Editor).CheckStyleForFalse = (StiCheckValueStyle)stiCheckBox.CheckStyleForFalse;
			((StiCheckBoxComponent)Editor).TextBrush = stiCheckBox.TextBrush;
			((StiCheckBoxComponent)Editor).ContourColor = stiCheckBox.ContourColor;
			((StiCheckBoxComponent)Editor).SizeValue = stiCheckBox.Size;
			if (stiCheckBox.CheckedValue == null)
			{
				((StiCheckBoxComponent)Editor).Checked = false;
			}
			else
			{
				string[] array = null;
				if (stiCheckBox.Values.IndexOf('/') != -1)
				{
					array = stiCheckBox.Values.Split(new char[1] { '/' });
				}
				else if (stiCheckBox.Values.IndexOf(';') != -1)
				{
					array = stiCheckBox.Values.Split(new char[1] { ';' });
				}
				else if (stiCheckBox.Values.IndexOf(',') != -1)
				{
					array = stiCheckBox.Values.Split(new char[1] { ',' });
				}
				((StiCheckBoxComponent)Editor).Checked = stiCheckBox.CheckedValue as string == array[0];
			}
		}
		else if (Editor is TextBox)
		{
			Editor.ForeColor = GetForeColor(Component);
			if (Component is IStiTextOptions)
			{
				((TextBoxBase)(TextBox)Editor).WordWrap = ((IStiTextOptions)Component).TextOptions.WordWrap;
			}
			((Control)(TextBox)Editor).Text = ((IStiText)Component).Text.Value;
		}
		else
		{
			((RichTextBox)Editor).Rtf = ((StiRichText)Component).RtfText;
			((TextBoxBase)(RichTextBox)Editor).BorderStyle = (BorderStyle)0;
			((RichTextBox)Editor).ZoomFactor = (float)Component.Report.Info.Zoom;
			((TextBoxBase)(RichTextBox)Editor).AcceptsTab = StiOptions.Viewer.AllowAcceptTabsInEditableMode;
			if (StiOptions.Viewer.Windows.LimitLengthOfEditableRichText)
			{
				((TextBoxBase)(RichTextBox)Editor).MaxLength = ((Control)(RichTextBox)Editor).Text.Length;
			}
		}
		((Control)viewerControl.View).Controls.Add(Editor);
		if (Editor is RichTextBox)
		{
			((RichTextBox)Editor).Rtf = ((StiRichText)Component).RtfText;
		}
		Editor.Focus();
		ScrollToCurEditor();
		viewerControl.CurrentPrintPage = viewerControl.CurrentReport.RenderedPages.IndexOf(Component.Page);
		((Control)viewerControl.View).Invalidate();
		viewerControl.InvokeTextEditorOpen(Editor);
	}

	internal Color GetBackColor(StiComponent comp)
	{
		if (comp is IStiBrush)
		{
			IStiBrush stiBrush = comp as IStiBrush;
			Color color = StiBrush.ToColor(stiBrush.Brush);
			if (color != Color.Transparent)
			{
				return color;
			}
		}
		return Color.White;
	}

	internal Color GetForeColor(StiComponent comp)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		if (comp is IStiTextBrush)
		{
			IStiTextBrush stiTextBrush = comp as IStiTextBrush;
			if (stiTextBrush.TextBrush is StiSolidBrush)
			{
				if (((StiSolidBrush)stiTextBrush.TextBrush).Color == Color.Transparent)
				{
					return Color.Black;
				}
				return ((StiSolidBrush)stiTextBrush.TextBrush).Color;
			}
		}
		return Color.Black;
	}

	public void Refresh()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		if (Component == null || Editor == null)
		{
			return;
		}
		StiPageParamView pageParamView = viewerControl.GetPageParamView(Component);
		if (pageParamView != null)
		{
			RectangleD componentRect = viewerControl.GetComponentRect(Component);
			((RectangleD)(ref componentRect)).X = ((RectangleD)(ref componentRect)).X + 1.0;
			((RectangleD)(ref componentRect)).Y = ((RectangleD)(ref componentRect)).Y + 1.0;
			((RectangleD)(ref componentRect)).Width = ((RectangleD)(ref componentRect)).Width - 2.0;
			((RectangleD)(ref componentRect)).Height = ((RectangleD)(ref componentRect)).Height - 2.0;
			Editor.Left = (int)((RectangleD)(ref componentRect)).X;
			Editor.Top = (int)((RectangleD)(ref componentRect)).Y;
			Editor.Width = (int)((RectangleD)(ref componentRect)).Width;
			Editor.Height = (int)((RectangleD)(ref componentRect)).Height;
			if (Component is IStiFont)
			{
				Font font = ((IStiFont)Component).Font;
				Font font2 = new Font(font.FontFamily, (float)((double)font.Size * viewerControl.Zoom), font.Style, font.Unit, font.GdiCharSet, font.GdiVerticalFont);
				Editor.Font = font2;
				Editor.BackColor = GetBackColor(Component);
				Editor.ForeColor = GetForeColor(Component);
			}
		}
	}

	public void EnableEditorTool()
	{
		if (viewerControl.CurrentReport == null)
		{
			return;
		}
		StiOptions.Engine.GlobalEvents.InvokeEnableEditorMode(viewerControl.CurrentReport, EventArgs.Empty);
		Components.Clear();
		StiComponentsCollection renderedComponents = viewerControl.CurrentReport.GetRenderedComponents();
		foreach (StiComponent item in renderedComponents)
		{
			if (item is IStiEditable && ((IStiEditable)item).Editable)
			{
				item.HighlightState = StiHighlightState.Show;
				Components.Add(item);
			}
		}
		if (Components.Count == 0)
		{
			viewerControl.CloseEditor();
			((BaseItem)viewerControl.tbEditor).Visible = false;
			return;
		}
		viewerControl.IsFindActivated = false;
		viewerControl.tbEditor.Checked = true;
		((BaseItem)viewerControl.tbEditor).Visible = true;
		First();
		((Control)viewerControl.View).Invalidate();
		OpenEditor();
	}

	public void Dispose()
	{
		Components = null;
		viewerControl = null;
		Editor = null;
	}

	private void Editor_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		if ((int)e.KeyCode == 9)
		{
			if (StiOptions.Viewer.AllowAcceptTabsInEditableMode && !((Enum)Control.ModifierKeys).HasFlag((Enum)(object)(Keys)131072))
			{
				return;
			}
			CloseEditor();
			if (e.Shift)
			{
				Prev();
			}
			else
			{
				Next();
			}
			OpenEditor();
		}
		if ((int)e.KeyCode == 27)
		{
			CloseEditor();
		}
	}

	private void Editor_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Invalid comparison between Unknown and I4
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		if ((int)e.KeyCode == 9)
		{
			if (StiOptions.Viewer.AllowAcceptTabsInEditableMode && !((Enum)Control.ModifierKeys).HasFlag((Enum)(object)(Keys)131072))
			{
				return;
			}
			CloseEditor();
			if (e.Shift)
			{
				Prev();
			}
			else
			{
				Next();
			}
			OpenEditor();
		}
		if ((int)e.KeyCode == 27)
		{
			CloseEditor();
		}
		if ((int)e.KeyCode == 13 && !(Component is StiRichText))
		{
			e.Handled = true;
		}
	}

	private void Editor_CheckedChanged(object sender, EventArgs e)
	{
		if (viewerControl != null && viewerControl.CurrentReport != null)
		{
			viewerControl.CurrentReport.IsEditedInViewer = true;
		}
	}

	private void Editor_TextChanged(object sender, EventArgs e)
	{
		if (viewerControl != null && viewerControl.CurrentReport != null)
		{
			viewerControl.CurrentReport.IsEditedInViewer = true;
		}
	}

	public StiTextEditor(StiViewerControl viewerControl)
	{
		this.viewerControl = viewerControl;
	}
}
