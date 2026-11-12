using System;
using System.Windows.Forms;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Design.Editors;

public sealed class StiConditionEditor
{
	public static DialogResult EditCondition(IStiGroup component, StiConditionEditorForm.StartPage? startPage = null)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		StiGroupHeaderBand stiGroupHeaderBand = component as StiGroupHeaderBand;
		StiConditionEditorForm stiConditionEditorForm = new StiConditionEditorForm(stiGroupHeaderBand, startPage);
		try
		{
			stiGroupHeaderBand?.Report?.Designer?.AddOwnedForm((Form)(object)stiConditionEditorForm);
			return ((Form)stiConditionEditorForm).ShowDialog();
		}
		finally
		{
			((IDisposable)stiConditionEditorForm)?.Dispose();
		}
	}
}
