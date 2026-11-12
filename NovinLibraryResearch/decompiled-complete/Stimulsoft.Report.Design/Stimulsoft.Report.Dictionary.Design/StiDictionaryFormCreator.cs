using System;
using Stimulsoft.Base;
using Stimulsoft.Report.Design.Dictionary;

namespace Stimulsoft.Report.Dictionary.Design;

public static class StiDictionaryFormCreator
{
	public static Type DefaultVariableEditorForm { get; set; } = typeof(StiVariableEditForm);


	public static IStiVariableEditForm CreateVariableEditorForm(StiReport report, StiVariable variable, bool isEditMode)
	{
		return StiActivator.CreateObject(DefaultVariableEditorForm, new object[3] { report, variable, isEditMode }) as IStiVariableEditForm;
	}
}
