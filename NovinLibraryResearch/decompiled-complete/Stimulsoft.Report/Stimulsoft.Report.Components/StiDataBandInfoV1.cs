using System.Collections.Generic;
using System.ComponentModel;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Components;

public class StiDataBandInfoV1 : StiComponentInfo
{
	[Browsable(false)]
	public StiComponentsCollection SubReportsComponents { get; set; }

	[Browsable(false)]
	public StiComponentsCollection RenderedHeaders { get; set; } = new StiComponentsCollection();


	public StiComponentsCollection HeaderComponents { get; set; }

	public StiComponentsCollection FooterComponents { get; set; }

	[Browsable(false)]
	public StiComponentsCollection GroupHeaderComponents { get; set; }

	[Browsable(false)]
	public StiComponentsCollection GroupFooterComponents { get; set; }

	public StiComponentsCollection DetailComponents { get; set; }

	public StiComponentsCollection DetailDataComponents { get; set; }

	public StiComponent CurrentDetailComponent { get; set; }

	public int LastPositionRendering { get; set; }

	public bool IsFirstPassOfBreak { get; set; } = true;


	public StiComponentsCollection BreakableComps { get; set; }

	public bool AlwaysKeepChildTogether { get; set; }

	public double FreeSpace { get; set; }

	public bool DataIsPrepared { get; set; }

	public int ColumnIndex { get; set; } = 1;


	public StiContainer ParentColumnContainer { get; set; }

	public bool LatestDataBandBreaked { get; set; }

	public StiBookmark ResParentPointer { get; set; }

	public StiBookmark ResParentBookmark { get; set; }

	public int StartGroupIndex { get; set; } = -1;


	public int StartLine { get; set; }

	public int RuntimeLine { get; set; }

	public bool LastRenderBreaked { get; set; }

	public StiComponent LastComponent { get; set; }

	public bool FirstRow { get; set; }

	public bool FirstRowInPath { get; set; }

	public bool ForceStartNewPage { get; set; }

	public StiComponentsCollection RemmitedCollection { get; set; } = new StiComponentsCollection();


	public List<StiComponent> RenderedItems { get; set; }

	public bool ItemsActive { get; set; }

	public bool FirstCall { get; set; } = true;


	public bool FirstGroupOnPass { get; set; } = true;


	public bool SkipStartNewPage { get; set; }

	public int StartMasterIndex { get; set; }

	public double ResHeightOfContainerBeforeRendering { get; set; }

	public bool CrossTabExistOnDataBand { get; set; }
}
