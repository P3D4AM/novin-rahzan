using System;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Dashboard.Actions;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Design.Panels;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiElementEditorForm : StiInplaceToolForm, IStiElementEditorForm
{
	internal virtual StiDesignerControl Designer => Element?.Report?.Designer?.DesignerControl as StiDesignerControl;

	internal StiReport Report => Element.Report;

	internal StiElement Element { get; }

	public StiElementEditorForm()
		: this(null)
	{
	}

	public StiElementEditorForm(StiElement element)
	{
		Element = element;
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		StiElementActionHelper.Hide();
	}

	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		base.OnFormClosed(e);
		if (Element != null && Designer != null)
		{
			StiElementActionHelper.Show(Element, Designer);
		}
	}

	public virtual void ReadPropertiesFromElement()
	{
	}

	public virtual void WritePropertiesToElement()
	{
	}

	protected internal void ActivateDictionaryPanel()
	{
		StiDictionaryPanelHelper.ActivatePanel(Designer);
	}

	protected internal void ResetComponentHighlights()
	{
		if (Element != null && Element.Page != null)
		{
			Element.Page.GetComponents().ToList().ForEach(delegate(StiComponent c)
			{
				c.HighlightState = StiHighlightState.Hide;
			});
			RefreshElementView();
		}
	}

	public void RefreshElementView()
	{
		StiDesignerControl designer = Designer;
		if (designer != null)
		{
			((Control)designer).Refresh();
		}
		Designer?.Builder.Redraw();
	}

	protected virtual void SetListHeightFromCount(StiMeterListBox list, bool multiple)
	{
		int num = Math.Min(10, ((ListBox)list).Items.Count);
		if (multiple)
		{
			((Control)list).Height = Math.Min(num + 1, 7) * ((ListBox)list).ItemHeight;
		}
		else
		{
			((Control)list).Height = Math.Max(num, 1) * ((ListBox)list).ItemHeight + StiScale.I1;
		}
	}

	public virtual void ProcessElementChanging(StiElementChangedArgs args)
	{
		StiElementChangedProcessor.ProcessElementChanging(Element, args);
	}
}
