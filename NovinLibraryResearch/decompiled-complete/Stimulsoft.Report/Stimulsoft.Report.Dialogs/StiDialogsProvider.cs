using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Dialogs;

public abstract class StiDialogsProvider : StiService
{
	public override Type ServiceType => typeof(StiDialogsProvider);

	protected abstract StiGuiMode GuiMode { get; }

	public abstract StiReport Report { get; set; }

	public event EventHandler ButtonClick;

	public event EventHandler EventFired;

	public static StiDialogsProvider GetProvider(StiReport report)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Invalid comparison between Unknown and I4
		StiGuiMode guiMode;
		if (report != null)
		{
			guiMode = (StiGuiMode)(report.IsWpf ? 1 : 0);
		}
		else
		{
			guiMode = (StiGuiMode)(StiOptions.Configuration.IsWPF ? 1 : 0);
		}
		if ((int)guiMode == 1)
		{
			Type type = Type.GetType("Stimulsoft.Report.Dialogs.StiWpfDialogsProvider, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
			if (type != null)
			{
				return StiActivator.CreateObject(type) as StiDialogsProvider;
			}
		}
		else
		{
			using IEnumerator<StiDialogsProvider> enumerator = StiOptions.Services.DialogProviders.Where((StiDialogsProvider p) => p.GuiMode == guiMode).GetEnumerator();
			if (enumerator.MoveNext())
			{
				StiDialogsProvider current = enumerator.Current;
				return StiActivator.CreateObject(((object)current).GetType(), new object[1] { report }) as StiDialogsProvider;
			}
		}
		return null;
	}

	public abstract bool Render(StiReport report, StiFormStartMode startMode);

	public abstract void PrepareForm();

	public abstract void DisposeForm();

	public abstract void LoadForm(IStiForm formControl);

	public abstract void CloseForm();

	public abstract bool RenderForm(IStiForm formControl);

	public abstract IStiForm CreateForm(StiReport report);

	public abstract IStiTextBoxControl CreateTextBoxControl();

	public abstract IStiLabelControl CreateLabelControl();

	public abstract IStiCheckBoxControl CreateCheckBoxControl();

	public abstract IStiPictureBoxControl CreatePictureBoxControl();

	protected void InvokeButtonClick(object sender, EventArgs e)
	{
		this.ButtonClick?.Invoke(sender, e);
	}

	protected void InvokeEventFired(object sender, EventArgs e)
	{
		this.EventFired?.Invoke(sender, e);
	}
}
