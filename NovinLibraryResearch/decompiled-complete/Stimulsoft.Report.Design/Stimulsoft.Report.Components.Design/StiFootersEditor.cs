using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Components.Design;

public class StiFootersEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)3;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Expected O, but got Unknown
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		string[] array = ((string)value).Split(new char[1] { ';' });
		Hashtable hashtable = new Hashtable();
		string[] array2 = array;
		foreach (string text in array2)
		{
			string text2 = text.Trim();
			hashtable[text2] = text2;
		}
		StiHierarchicalBand stiHierarchicalBand = context.Instance as StiHierarchicalBand;
		StiDataBandV2Builder stiDataBandV2Builder = new StiDataBandV2Builder();
		stiDataBandV2Builder.FindFooters(stiHierarchicalBand);
		Panel val = new Panel
		{
			Width = StiScale.XXI(220.0),
			Height = StiScale.YYI(60.0)
		};
		int num = 0;
		foreach (StiFooterBand footersOnAllPage in stiHierarchicalBand.DataBandInfoV2.FootersOnAllPages)
		{
			CheckBox val2 = new CheckBox
			{
				Text = footersOnAllPage.Name,
				Checked = (hashtable[footersOnAllPage.Name] != null)
			};
			((Control)val2).SetBounds(StiScale.I4, StiScale.I4 + num * StiScale.YYI(16.0), StiScale.XXI(200.0), StiScale.YYI(16.0), (BoundsSpecified)15);
			((Control)val).Controls.Add((Control)(object)val2);
			num++;
		}
		foreach (StiFooterBand item in stiHierarchicalBand.DataBandInfoV2.FootersOnLastPage)
		{
			CheckBox val3 = new CheckBox
			{
				Text = item.Name,
				Checked = (hashtable[item.Name] != null)
			};
			((Control)val3).SetBounds(StiScale.I4, StiScale.I4 + num * StiScale.YYI(16.0), StiScale.XXI(200.0), StiScale.YYI(16.0), (BoundsSpecified)15);
			((Control)val).Controls.Add((Control)(object)val3);
			num++;
		}
		((Control)val).Height = ((ArrangedElementCollection)((Control)val).Controls).Count * StiScale.YYI(16.0) + StiScale.I8;
		edSvc.DropDownControl((Control)(object)val);
		StringBuilder stringBuilder = new StringBuilder();
		foreach (CheckBox item2 in (ArrangedElementCollection)((Control)val).Controls)
		{
			CheckBox val4 = item2;
			if (val4.Checked)
			{
				if (stringBuilder.Length == 0)
				{
					stringBuilder.Append(((Control)val4).Text);
					continue;
				}
				stringBuilder.Append(";");
				stringBuilder.Append(((Control)val4).Text);
			}
		}
		stiDataBandV2Builder.ResetFooters(stiHierarchicalBand);
		return stringBuilder.ToString();
	}
}
