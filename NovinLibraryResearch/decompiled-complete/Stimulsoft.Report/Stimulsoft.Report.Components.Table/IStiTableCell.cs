namespace Stimulsoft.Report.Components.Table;

public interface IStiTableCell
{
	int[] JoinCells { get; set; }

	int ParentJoin { get; set; }

	bool Join { get; set; }

	int ID { get; set; }

	int JoinWidth { get; set; }

	int JoinHeight { get; set; }

	bool Merged { get; }

	bool ChangeTopPosition { get; }

	bool ChangeLeftPosition { get; }

	bool ChangeRightPosition { get; }

	StiTablceCellType CellType { get; set; }

	StiDockStyle CellDockStyle { get; set; }

	int Column { get; set; }

	bool FixedWidth { get; set; }

	object TableTag { get; set; }

	StiComponent ParentJoinCell { get; set; }

	StiComponent GetJoinComponentByGuid(int id);

	StiComponent GetJoinComponentByIndex(int index);

	bool ContainsGuid(int id);

	void SetJoinSize();

	double GetRealHeightAfterInsertRows();

	double GetRealHeight();

	double GetRealTop();

	double GetRealWidth();

	double GetRealLeft();
}
