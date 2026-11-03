using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiCrossDataBandV1Builder : StiDataBandV1Builder
{
	public override void MoveNext(StiDataBand masterDataBand)
	{
		base.MoveNext(masterDataBand);
		StiCrossDataBand stiCrossDataBand = masterDataBand as StiCrossDataBand;
		stiCrossDataBand.SetColumnModeToParent();
		stiCrossDataBand.ColumnCurrent++;
	}

	public override void SetDataIsPrepared(StiDataBand masterDataBand, bool value)
	{
		StiCrossDataBand stiCrossDataBand = masterDataBand as StiCrossDataBand;
		if (stiCrossDataBand.ColumnMode && stiCrossDataBand.Parent is StiDataBand)
		{
			((StiDataBand)stiCrossDataBand.Parent).DataBandInfoV1.DataIsPrepared = value;
		}
		else
		{
			stiCrossDataBand.DataBandInfoV1.DataIsPrepared = value;
		}
	}

	public override bool GetDataIsPrepared(StiDataBand masterDataBand)
	{
		StiCrossDataBand stiCrossDataBand = masterDataBand as StiCrossDataBand;
		if (stiCrossDataBand.ColumnMode && stiCrossDataBand.Parent is StiDataBand)
		{
			return ((StiDataBand)stiCrossDataBand.Parent).DataBandInfoV1.DataIsPrepared;
		}
		return stiCrossDataBand.DataBandInfoV1.DataIsPrepared;
	}

	public override void SetReportVariables(StiDataBand masterDataBand)
	{
		base.SetReportVariables(masterDataBand);
		StiCrossDataBand stiCrossDataBand = masterDataBand as StiCrossDataBand;
		if (stiCrossDataBand.ColumnMode)
		{
			stiCrossDataBand.Report.Line = stiCrossDataBand.Position + 1;
			stiCrossDataBand.Report.Column = stiCrossDataBand.ColumnCurrent + 1;
		}
	}

	private void AddDelimiterComponent(StiDataBand masterDataBand, StiComponent lastComponent, double width, StiContainer outContainer)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		StiText stiText = new StiText
		{
			DelimiterComponent = true,
			Brush = (StiBrush)new StiSolidBrush(Color.Transparent),
			Width = width
		};
		outContainer.Components.Add(stiText);
		outContainer.Components.Remove(lastComponent);
		outContainer.Components.Add(lastComponent);
		stiText.DockStyle = StiDockStyle.Left;
		DecFreeSpace(masterDataBand, stiText);
		if (masterDataBand.Page.PageInfoV1.DelimiterComponentsLeft[lastComponent.Left] == null)
		{
			masterDataBand.Page.PageInfoV1.DelimiterComponentsLeft[lastComponent.Left] = stiText.Width;
		}
	}

	public override bool CheckFreeSpace(StiDataBand masterDataBand, StiContainer outContainer)
	{
		StiCrossDataBand stiCrossDataBand = masterDataBand as StiCrossDataBand;
		if (stiCrossDataBand.ColumnMode)
		{
			return false;
		}
		if (stiCrossDataBand.Page.UnlimitedWidth)
		{
			StiComponent stiComponent = outContainer.Components[outContainer.Components.Count - 1];
			if (stiCrossDataBand.Page.PageInfoV1.DelimiterComponentsLeft[stiComponent.Left] != null && !stiComponent.DelimiterComponent)
			{
				double width = (double)stiCrossDataBand.Page.PageInfoV1.DelimiterComponentsLeft[stiComponent.Left];
				AddDelimiterComponent(stiCrossDataBand, stiComponent, width, outContainer);
				return false;
			}
			if (stiCrossDataBand.DataBandInfoV1.FreeSpace < 0.0)
			{
				if (stiCrossDataBand.Page.UnlimitedBreakable)
				{
					AddDelimiterComponent(stiCrossDataBand, stiComponent, stiComponent.Width + stiCrossDataBand.DataBandInfoV1.FreeSpace, outContainer);
				}
				DecFreeSpace(stiCrossDataBand, outContainer);
				outContainer.AddSize(leftToRightDirection: true);
				IncFreeSpace(stiCrossDataBand, outContainer);
				return false;
			}
			return false;
		}
		return CheckFreeSpace(stiCrossDataBand, outContainer);
	}

	public override void IncFreeSpace(StiDataBand masterDataBand, StiComponent component)
	{
		masterDataBand.DataBandInfoV1.FreeSpace += component.Width;
	}

	public override void DecFreeSpace(StiDataBand masterDataBand, StiComponent component)
	{
		masterDataBand.DataBandInfoV1.FreeSpace -= component.Width;
	}

	public override double GetFreeSpaceFromRectangle(RectangleD rect)
	{
		return ((RectangleD)(ref rect)).Width;
	}

	public override double GetFreeSpaceFromComponent(StiComponent component)
	{
		return component.Width;
	}

	public override void Prepare(StiComponent masterComp)
	{
		base.Prepare(masterComp);
		StiCrossDataBand stiCrossDataBand = masterComp as StiCrossDataBand;
		stiCrossDataBand.ColumnCurrent = 0;
	}

	public override bool InternalRender(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		StiCrossDataBand stiCrossDataBand = masterComp as StiCrossDataBand;
		stiCrossDataBand.GetColumnModeFromParent();
		bool flag;
		do
		{
			int count = outContainer.Components.Count;
			flag = base.InternalRender((StiComponent)stiCrossDataBand, ref renderedComponent, outContainer);
			if (!stiCrossDataBand.IsRightToLeft)
			{
				continue;
			}
			for (int i = count; i < outContainer.Components.Count; i++)
			{
				if (outContainer.Components[i] is StiContainer stiContainer)
				{
					stiContainer.OffsetLocation(stiCrossDataBand.ColumnGaps, 0.0);
				}
			}
		}
		while (!flag && stiCrossDataBand.Page.UnlimitedWidth);
		return true;
	}
}
