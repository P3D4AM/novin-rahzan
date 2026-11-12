using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class SystemDataType : MetadataObjectBase, ISystemDataType, IScalarDataType, IDataType, IMetadataObject
{
	private abstract class SystemClrDataType : SystemDataType, ISystemClrDataType, ISystemDataType, IScalarDataType, IDataType, IMetadataObject, IClrDataType
	{
		private IMetadataCollection<IUdtMethod> m_methods;

		private IMetadataCollection<IUdtDataMember> m_dataMembers;

		public override bool IsClr => true;

		public override IClrDataType AsClrDataType => this;

		public string AssemblyName => "Microsoft.SqlServer.Types";

		public abstract string ClassName { get; }

		public bool IsBinaryOrdered => false;

		public bool IsComVisible => false;

		public bool IsNullable => true;

		public IMetadataCollection<IUdtMethod> Methods
		{
			get
			{
				if (m_methods == null)
				{
					m_methods = CreateMethodCollection();
				}
				return m_methods;
			}
		}

		public IMetadataCollection<IUdtDataMember> DataMembers
		{
			get
			{
				if (m_dataMembers == null)
				{
					m_dataMembers = CreateDataMemberCollection();
				}
				return m_dataMembers;
			}
		}

		protected SystemClrDataType(DataTypeSpec typeSpec)
			: base(typeSpec)
		{
		}

		public override T Accept<T>(IMetadataObjectVisitor<T> visitor)
		{
			if (visitor == null)
			{
				throw new ArgumentNullException("visitor");
			}
			return visitor.Visit(this);
		}

		protected abstract IMetadataCollection<IUdtMethod> CreateMethodCollection();

		protected abstract IMetadataCollection<IUdtDataMember> CreateDataMemberCollection();
	}

	private class GeographyDataType : SystemClrDataType
	{
		public override string ClassName => "Microsoft.SqlServer.Types.SqlGeography";

		public GeographyDataType()
			: base(DataTypeSpec.Geography)
		{
		}

		protected override IMetadataCollection<IUdtMethod> CreateMethodCollection()
		{
			SystemDataTypeLookup instance = SystemDataTypeLookup.Instance;
			IUdtMethod[] methods = new UdtMethod[64]
			{
				new UdtMethod("STArea", this, instance.Float, true, false),
				new UdtMethod("STAsBinary", this, instance.VarBinaryMax, true, false),
				new UdtMethod("STAsText", this, instance.NVarCharMax, true, false),
				new UdtMethod("STBuffer", this, this, true, false, new ScalarParameter("distance", instance.Float)),
				new UdtMethod("STDifference", this, this, true, false, new ScalarParameter("other_geography", this)),
				new UdtMethod("STDimension", this, instance.Int, true, false),
				new UdtMethod("STDisjoint", this, instance.Bit, true, false, new ScalarParameter("other_geography", this)),
				new UdtMethod("STDistance", this, instance.Float, true, false, new ScalarParameter("other_geography", this)),
				new UdtMethod("STEndPoint", this, this, true, false),
				new UdtMethod("STEquals", this, instance.Bit, true, false, new ScalarParameter("other_geography", this)),
				new UdtMethod("STGeometryN", this, this, true, false, new ScalarParameter("n", instance.Int)),
				new UdtMethod("STGeometryType", this, instance.NVarCharMax, true, false),
				new UdtMethod("STIntersection", this, this, true, false, new ScalarParameter("other_geography", this)),
				new UdtMethod("STIntersects", this, instance.Bit, true, false, new ScalarParameter("other_geography", this)),
				new UdtMethod("STIsClosed", this, instance.Bit, true, false),
				new UdtMethod("STIsEmpty", this, instance.Bit, true, false),
				new UdtMethod("STIsValid", this, instance.Bit, true, false),
				new UdtMethod("STLength", this, instance.Float, true, false),
				new UdtMethod("STNumGeometries", this, instance.Int, true, false),
				new UdtMethod("STNumPoints", this, instance.Int, true, false),
				new UdtMethod("STPointN", this, this, true, false, new ScalarParameter("n", instance.Int)),
				new UdtMethod("STStartPoint", this, this, true, false),
				new UdtMethod("STSymDifference", this, this, true, false, new ScalarParameter("other_geography", this)),
				new UdtMethod("STUnion", this, this, true, false, new ScalarParameter("other_geography", this)),
				new UdtMethod("AsGml", this, instance.Xml(), true, false),
				new UdtMethod("AsTextZM", this, instance.NVarCharMax, true, false),
				new UdtMethod("BufferWithCurves", this, this, true, false, new ScalarParameter("distance", instance.Float)),
				new UdtMethod("BufferWithTolerance", this, this, true, false, new ScalarParameter("distance", instance.Float), new ScalarParameter("tolerance", instance.Float), new ScalarParameter("relative", instance.Bit)),
				new UdtMethod("CurveToLineWithTolerance", this, this, true, false, new ScalarParameter("tolerance", instance.Float), new ScalarParameter("relative", instance.Bit)),
				new UdtMethod("EnvelopeAngle", this, instance.Float, true, false),
				new UdtMethod("EnvelopeCenter", this, this, true, false),
				new UdtMethod("Filter", this, instance.Bit, true, false, new ScalarParameter("other_geography", this)),
				new UdtMethod("InstanceOf", this, instance.Bit, true, false, new ScalarParameter("geometryType", instance.NVarCharMax)),
				new UdtMethod("MakeValid", this, this, true, false),
				new UdtMethod("MinDbCompatibilityLevel", this, instance.Int, true, false),
				new UdtMethod("NumRings", this, instance.Int, true, false),
				new UdtMethod("Reduce", this, this, true, false, new ScalarParameter("tolerance", instance.Float)),
				new UdtMethod("ReorientObject", this, this, true, false),
				new UdtMethod("RingN", this, this, true, false, new ScalarParameter("n", instance.Int)),
				new UdtMethod("ShortestLineTo", this, this, true, false, new ScalarParameter("geography_other", this)),
				new UdtMethod("ToString", this, instance.NVarCharMax, true, false),
				new UdtMethod("STGeomCollFromText", this, this, true, true, new ScalarParameter("geometrycollection_tagged_text", instance.NVarCharMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STGeomCollFromWKB", this, this, true, true, new ScalarParameter("wkb_geometrycollection", instance.VarBinaryMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STGeomFromText", this, this, true, true, new ScalarParameter("geometry_tagged_text", instance.NVarCharMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STGeomFromWKB", this, this, true, true, new ScalarParameter("wkb_geometry", instance.VarBinaryMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STLineFromText", this, this, true, true, new ScalarParameter("linestring_tagged_text", instance.NVarCharMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STLineFromWKB", this, this, true, true, new ScalarParameter("wkb_linestring", instance.VarBinaryMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STMLineFromText", this, this, true, true, new ScalarParameter("multilinestring_tagged_text", instance.NVarCharMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STMLineFromWKB", this, this, true, true, new ScalarParameter("wkb_multilinestring", instance.VarBinaryMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STMPointFromText", this, this, true, true, new ScalarParameter("multipoint_tagged_text", instance.NVarCharMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STMPointFromWKB", this, this, true, true, new ScalarParameter("wkb_multipoint", instance.VarBinaryMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STMPolyFromText", this, this, true, true, new ScalarParameter("multipolygon_tagged_text", instance.NVarCharMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STMPolyFromWKB", this, this, true, true, new ScalarParameter("wkb_multipolygon", instance.VarBinaryMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STPointFromText", this, this, true, true, new ScalarParameter("point_tagged_text", instance.NVarCharMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STPointFromWKB", this, this, true, true, new ScalarParameter("wkb_point", instance.VarBinaryMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STPolyFromText", this, this, true, true, new ScalarParameter("polygon_tagged_text", instance.NVarCharMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STPolyFromWKB", this, this, true, true, new ScalarParameter("wkb_polygon", instance.VarBinaryMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("CollectionAggregate", this, this, true, true, new ScalarParameter("geography_operand", this)),
				new UdtMethod("ConvexHullAggregate", this, this, true, true, new ScalarParameter("geography_operand", this)),
				new UdtMethod("EnvelopeAggregate", this, this, true, true, new ScalarParameter("geography_operand", this)),
				new UdtMethod("GeomFromGml", this, this, true, true, new ScalarParameter("GML_input", instance.Xml()), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("Parse", this, this, true, true, new ScalarParameter("s", instance.NVarCharMax)),
				new UdtMethod("Point", this, this, true, true, new ScalarParameter("latitude", instance.Float), new ScalarParameter("longitude", instance.Float), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("UnionAggregate", this, this, true, true, new ScalarParameter("geography_operand", this))
			};
			return UdtMethodCollection.Create(methods);
		}

		protected override IMetadataCollection<IUdtDataMember> CreateDataMemberCollection()
		{
			SystemDataTypeLookup instance = SystemDataTypeLookup.Instance;
			IUdtDataMember[] dataMembers = new UdtDataMember[7]
			{
				new UdtDataMember("STSrid", this, instance.Int, isStatic: false),
				new UdtDataMember("IsNull", this, instance.Bit, isStatic: false),
				new UdtDataMember("Lat", this, instance.Float, isStatic: false),
				new UdtDataMember("Long", this, instance.Float, isStatic: false),
				new UdtDataMember("M", this, instance.Float, isStatic: false),
				new UdtDataMember("Z", this, instance.Float, isStatic: false),
				new UdtDataMember("Null", this, this, isStatic: true)
			};
			return UdtDataMemberCollection.Create(dataMembers);
		}
	}

	private class GeometryDataType : SystemClrDataType
	{
		public override string ClassName => "Microsoft.SqlServer.Types.SqlGeometry";

		public GeometryDataType()
			: base(DataTypeSpec.Geometry)
		{
		}

		protected override IMetadataCollection<IUdtMethod> CreateMethodCollection()
		{
			SystemDataTypeLookup instance = SystemDataTypeLookup.Instance;
			IUdtMethod[] methods = new UdtMethod[67]
			{
				new UdtMethod("STArea", this, instance.Float, true, false),
				new UdtMethod("STAsBinary", this, instance.VarBinaryMax, true, false),
				new UdtMethod("STAsText", this, instance.NVarCharMax, true, false),
				new UdtMethod("STBoundary", this, this, true, false),
				new UdtMethod("STBuffer", this, this, true, false, new ScalarParameter("distance", instance.Float)),
				new UdtMethod("STCentroid", this, this, true, false),
				new UdtMethod("STContains", this, instance.Bit, true, false, new ScalarParameter("other_geometry", this)),
				new UdtMethod("STConvexHull", this, this, true, false),
				new UdtMethod("STCrosses", this, instance.Bit, true, false, new ScalarParameter("other_geometry", this)),
				new UdtMethod("STDifference", this, this, true, false, new ScalarParameter("other_geometry", this)),
				new UdtMethod("STDimension", this, instance.Int, true, false),
				new UdtMethod("STDisjoint", this, instance.Bit, true, false, new ScalarParameter("other_geometry", this)),
				new UdtMethod("STDistance", this, instance.Float, true, false, new ScalarParameter("other_geometry", this)),
				new UdtMethod("STEndPoint", this, this, true, false),
				new UdtMethod("STEnvelope", this, this, true, false),
				new UdtMethod("STEquals", this, instance.Bit, true, false, new ScalarParameter("other_geometry", this)),
				new UdtMethod("STExteriorRing", this, this, true, false),
				new UdtMethod("STGeometryN", this, this, true, false, new ScalarParameter("n", instance.Int)),
				new UdtMethod("STGeometryType", this, instance.NVarCharMax, true, false),
				new UdtMethod("STInteriorRingN", this, this, true, false, new ScalarParameter("n", instance.Int)),
				new UdtMethod("STIntersection", this, this, true, false, new ScalarParameter("other_geometry", this)),
				new UdtMethod("STIntersects", this, instance.Bit, true, false, new ScalarParameter("other_geometry", this)),
				new UdtMethod("STIsClosed", this, instance.Bit, true, false),
				new UdtMethod("STIsEmpty", this, instance.Bit, true, false),
				new UdtMethod("STIsRing", this, instance.Bit, true, false),
				new UdtMethod("STIsSimple", this, instance.Bit, true, false),
				new UdtMethod("STIsValid", this, instance.Bit, true, false),
				new UdtMethod("STLength", this, instance.Float, true, false),
				new UdtMethod("STNumGeometries", this, instance.Int, true, false),
				new UdtMethod("STNumInteriorRing", this, instance.Int, true, false),
				new UdtMethod("STNumPoints", this, instance.Int, true, false),
				new UdtMethod("STOverlaps", this, instance.Bit, true, false, new ScalarParameter("other_geometry", this)),
				new UdtMethod("STPointN", this, this, true, false, new ScalarParameter("n", instance.Int)),
				new UdtMethod("STPointOnSurface", this, this, true, false),
				new UdtMethod("STRelate", this, instance.Bit, true, false, new ScalarParameter("other_geometry", this), new ScalarParameter("intersectionPatternMatrix", instance.NVarCharMax)),
				new UdtMethod("STStartPoint", this, this, true, false),
				new UdtMethod("STSymDifference", this, this, true, false, new ScalarParameter("other_geometry", this)),
				new UdtMethod("STTouches", this, instance.Bit, true, false, new ScalarParameter("other_geometry", this)),
				new UdtMethod("STUnion", this, this, true, false, new ScalarParameter("other_geometry", this)),
				new UdtMethod("STWithin", this, instance.Bit, true, false, new ScalarParameter("other_geometry", this)),
				new UdtMethod("AsGml", this, instance.Xml(), true, false),
				new UdtMethod("AsTextZM", this, instance.NVarCharMax, true, false),
				new UdtMethod("BufferWithTolerance", this, this, true, false, new ScalarParameter("distance", instance.Float), new ScalarParameter("tolerance", instance.Float), new ScalarParameter("relative", instance.Bit)),
				new UdtMethod("Filter", this, instance.Bit, true, false, new ScalarParameter("other_geometry", this)),
				new UdtMethod("InstanceOf", this, instance.Bit, true, false, new ScalarParameter("geometryType", instance.NVarCharMax)),
				new UdtMethod("MakeValid", this, this, true, false),
				new UdtMethod("Reduce", this, this, true, false, new ScalarParameter("tolerance", instance.Float)),
				new UdtMethod("ToString", this, instance.NVarCharMax, true, false),
				new UdtMethod("STGeomCollFromText", this, this, true, true, new ScalarParameter("geometrycollection_tagged_text", instance.NVarCharMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STGeomCollFromWKB", this, this, true, true, new ScalarParameter("wkb_geometrycollection", instance.VarBinaryMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STGeomFromText", this, this, true, true, new ScalarParameter("geometry_tagged_text", instance.NVarCharMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STGeomFromWKB", this, this, true, true, new ScalarParameter("wkb_geometry", instance.VarBinaryMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STLineFromText", this, this, true, true, new ScalarParameter("linestring_tagged_text", instance.NVarCharMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STLineFromWKB", this, this, true, true, new ScalarParameter("wkb_linestring", instance.VarBinaryMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STMLineFromText", this, this, true, true, new ScalarParameter("multilinestring_tagged_text", instance.NVarCharMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STMLineFromWKB", this, this, true, true, new ScalarParameter("wkb_multilinestring", instance.VarBinaryMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STMPointFromText", this, this, true, true, new ScalarParameter("multipoint_tagged_text", instance.NVarCharMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STMPointFromWKB", this, this, true, true, new ScalarParameter("wkb_multipoint", instance.VarBinaryMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STMPolyFromText", this, this, true, true, new ScalarParameter("multipolygon_tagged_text", instance.NVarCharMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STMPolyFromWKB", this, this, true, true, new ScalarParameter("wkb_multipolygon", instance.VarBinaryMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STPointFromText", this, this, true, true, new ScalarParameter("point_tagged_text", instance.NVarCharMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STPointFromWKB", this, this, true, true, new ScalarParameter("wkb_point", instance.VarBinaryMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STPolyFromText", this, this, true, true, new ScalarParameter("polygon_tagged_text", instance.NVarCharMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("STPolyFromWKB", this, this, true, true, new ScalarParameter("wkb_polygon", instance.VarBinaryMax), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("GeomFromGml", this, this, true, true, new ScalarParameter("GML_input", instance.Xml()), new ScalarParameter("srid", instance.Int)),
				new UdtMethod("Parse", this, this, true, true, new ScalarParameter("s", instance.NVarCharMax)),
				new UdtMethod("Point", this, this, true, true, new ScalarParameter("x", instance.Float), new ScalarParameter("y", instance.Float), new ScalarParameter("srid", instance.Int))
			};
			return UdtMethodCollection.Create(methods);
		}

		protected override IMetadataCollection<IUdtDataMember> CreateDataMemberCollection()
		{
			SystemDataTypeLookup instance = SystemDataTypeLookup.Instance;
			IUdtDataMember[] dataMembers = new UdtDataMember[7]
			{
				new UdtDataMember("STSrid", this, instance.Int, isStatic: false),
				new UdtDataMember("STX", this, instance.Float, isStatic: false),
				new UdtDataMember("STY", this, instance.Float, isStatic: false),
				new UdtDataMember("IsNull", this, instance.Bit, isStatic: false),
				new UdtDataMember("M", this, instance.Float, isStatic: false),
				new UdtDataMember("Z", this, instance.Float, isStatic: false),
				new UdtDataMember("Null", this, this, isStatic: true)
			};
			return UdtDataMemberCollection.Create(dataMembers);
		}
	}

	private class HierarchyIdDataType : SystemClrDataType
	{
		public override string ClassName => "Microsoft.SqlServer.Types.SqlHierarchyId";

		public HierarchyIdDataType()
			: base(DataTypeSpec.HierarchyId)
		{
		}

		protected override IMetadataCollection<IUdtMethod> CreateMethodCollection()
		{
			SystemDataTypeLookup instance = SystemDataTypeLookup.Instance;
			IUdtMethod[] methods = new UdtMethod[8]
			{
				new UdtMethod("GetAncestor", this, this, true, false, new ScalarParameter("n", instance.Int)),
				new UdtMethod("GetDescendant", this, this, true, false, new ScalarParameter("child1", this), new ScalarParameter("child2", this)),
				new UdtMethod("GetLevel", this, instance.SmallInt, true, false),
				new UdtMethod("GetReparentedValue", this, this, true, false, new ScalarParameter("oldRoot", this), new ScalarParameter("newRoot", this)),
				new UdtMethod("GetRoot", this, this, true, true),
				new UdtMethod("IsDescendantOf", this, instance.Bit, true, false, new ScalarParameter("parent", this)),
				new UdtMethod("Parse", this, this, true, true, new ScalarParameter("input", instance.NVarCharMax)),
				new UdtMethod("ToString", this, instance.NVarCharMax, true, false)
			};
			return UdtMethodCollection.Create(methods);
		}

		protected override IMetadataCollection<IUdtDataMember> CreateDataMemberCollection()
		{
			return Collection<IUdtDataMember>.Empty;
		}
	}

	public class SystemDataTypeFactory
	{
		private readonly DataTypeSpec typeSpec;

		private readonly SystemDataType noArgInstance;

		private readonly SystemDataType ignoreArgInstance;

		private readonly Dictionary<int, SystemDataType> instances;

		private readonly object syncRoot;

		private static readonly SystemDataTypeFactory[] TypeFactories;

		public static readonly SystemDataTypeFactory BigInt;

		public static readonly SystemDataTypeFactory Binary;

		public static readonly SystemDataTypeFactory Bit;

		public static readonly SystemDataTypeFactory Char;

		public static readonly SystemDataTypeFactory Date;

		public static readonly SystemDataTypeFactory DateTime;

		public static readonly SystemDataTypeFactory DateTime2;

		public static readonly SystemDataTypeFactory DateTimeOffset;

		public static readonly SystemDataTypeFactory Decimal;

		public static readonly SystemDataTypeFactory Float;

		public static readonly SystemDataTypeFactory Geography;

		public static readonly SystemDataTypeFactory Geometry;

		public static readonly SystemDataTypeFactory HierarchyId;

		public static readonly SystemDataTypeFactory Image;

		public static readonly SystemDataTypeFactory Int;

		public static readonly SystemDataTypeFactory Money;

		public static readonly SystemDataTypeFactory NChar;

		public static readonly SystemDataTypeFactory NText;

		public static readonly SystemDataTypeFactory Numeric;

		public static readonly SystemDataTypeFactory NVarChar;

		public static readonly SystemDataTypeFactory NVarCharMax;

		public static readonly SystemDataTypeFactory Real;

		public static readonly SystemDataTypeFactory SmallDateTime;

		public static readonly SystemDataTypeFactory SmallInt;

		public static readonly SystemDataTypeFactory SmallMoney;

		public static readonly SystemDataTypeFactory SysName;

		public static readonly SystemDataTypeFactory Text;

		public static readonly SystemDataTypeFactory Time;

		public static readonly SystemDataTypeFactory Timestamp;

		public static readonly SystemDataTypeFactory TinyInt;

		public static readonly SystemDataTypeFactory UniqueIdentifier;

		public static readonly SystemDataTypeFactory VarBinary;

		public static readonly SystemDataTypeFactory VarBinaryMax;

		public static readonly SystemDataTypeFactory VarChar;

		public static readonly SystemDataTypeFactory VarCharMax;

		public static readonly SystemDataTypeFactory Variant;

		public static readonly SystemDataTypeFactory Xml;

		private SystemDataTypeFactory(DataTypeSpec typeSpec)
		{
			this.typeSpec = typeSpec;
			switch (typeSpec.SqlDataType)
			{
			case SqlDataType.Geography:
				noArgInstance = new GeographyDataType();
				break;
			case SqlDataType.Geometry:
				noArgInstance = new GeometryDataType();
				break;
			case SqlDataType.HierarchyId:
				noArgInstance = new HierarchyIdDataType();
				break;
			case SqlDataType.Xml:
				noArgInstance = new XmlDataType();
				break;
			default:
				noArgInstance = new SystemDataType(typeSpec);
				break;
			}
			if (typeSpec.ArgSpec1 != null)
			{
				instances = new Dictionary<int, SystemDataType>(64);
				ignoreArgInstance = new SystemDataType(typeSpec, ignoreArgs: true);
				syncRoot = new object();
			}
		}

		public SystemDataType GetDataType()
		{
			return GetDataType(ignoreArgs: false);
		}

		public SystemDataType GetDataType(bool ignoreArgs)
		{
			if (!ignoreArgs)
			{
				return noArgInstance;
			}
			return ignoreArgInstance;
		}

		public SystemDataType GetDataType(int arg)
		{
			SystemDataType value = null;
			if (typeSpec.ArgSpec2 != null)
			{
				value = GetDataType(arg, typeSpec.ArgSpec2.DefaultValue);
			}
			else if (typeSpec.ArgSpec1 != null)
			{
				if (arg == typeSpec.ArgSpec1.DefaultValue)
				{
					value = noArgInstance;
				}
				else if (arg >= typeSpec.ArgSpec1.MinValue && arg <= typeSpec.ArgSpec1.MaxValue && !instances.TryGetValue(arg, out value))
				{
					lock (syncRoot)
					{
						if (!instances.TryGetValue(arg, out value))
						{
							value = new SystemDataType(typeSpec, arg);
							instances.Add(arg, value);
						}
					}
				}
			}
			else if (typeSpec.SqlDataType == SqlDataType.Float && arg >= 1 && arg <= 53)
			{
				value = ((arg <= 24) ? Real.GetDataType() : GetDataType());
			}
			return value;
		}

		public SystemDataType GetDataType(int arg1, int arg2)
		{
			SystemDataType value = null;
			if (typeSpec.ArgSpec2 != null)
			{
				if (arg1 == typeSpec.ArgSpec1.DefaultValue && arg2 == typeSpec.ArgSpec2.DefaultValue)
				{
					value = noArgInstance;
				}
				else
				{
					int key = arg1 + (arg2 << 16);
					if (!instances.TryGetValue(key, out value))
					{
						lock (syncRoot)
						{
							if (!instances.TryGetValue(key, out value))
							{
								value = new SystemDataType(typeSpec, arg1, arg2);
								instances.Add(key, value);
							}
						}
					}
				}
			}
			return value;
		}

		static SystemDataTypeFactory()
		{
			BigInt = new SystemDataTypeFactory(DataTypeSpec.BigInt);
			Binary = new SystemDataTypeFactory(DataTypeSpec.Binary);
			Bit = new SystemDataTypeFactory(DataTypeSpec.Bit);
			Char = new SystemDataTypeFactory(DataTypeSpec.Char);
			Date = new SystemDataTypeFactory(DataTypeSpec.Date);
			DateTime = new SystemDataTypeFactory(DataTypeSpec.DateTime);
			DateTime2 = new SystemDataTypeFactory(DataTypeSpec.DateTime2);
			DateTimeOffset = new SystemDataTypeFactory(DataTypeSpec.DateTimeOffset);
			Decimal = new SystemDataTypeFactory(DataTypeSpec.Decimal);
			Float = new SystemDataTypeFactory(DataTypeSpec.Float);
			Geography = new SystemDataTypeFactory(DataTypeSpec.Geography);
			Geometry = new SystemDataTypeFactory(DataTypeSpec.Geometry);
			HierarchyId = new SystemDataTypeFactory(DataTypeSpec.HierarchyId);
			Image = new SystemDataTypeFactory(DataTypeSpec.Image);
			Int = new SystemDataTypeFactory(DataTypeSpec.Int);
			Money = new SystemDataTypeFactory(DataTypeSpec.Money);
			NChar = new SystemDataTypeFactory(DataTypeSpec.NChar);
			NText = new SystemDataTypeFactory(DataTypeSpec.NText);
			Numeric = new SystemDataTypeFactory(DataTypeSpec.Numeric);
			NVarChar = new SystemDataTypeFactory(DataTypeSpec.NVarChar);
			NVarCharMax = new SystemDataTypeFactory(DataTypeSpec.NVarCharMax);
			Real = new SystemDataTypeFactory(DataTypeSpec.Real);
			SmallDateTime = new SystemDataTypeFactory(DataTypeSpec.SmallDateTime);
			SmallInt = new SystemDataTypeFactory(DataTypeSpec.SmallInt);
			SmallMoney = new SystemDataTypeFactory(DataTypeSpec.SmallMoney);
			SysName = new SystemDataTypeFactory(DataTypeSpec.SysName);
			Text = new SystemDataTypeFactory(DataTypeSpec.Text);
			Time = new SystemDataTypeFactory(DataTypeSpec.Time);
			Timestamp = new SystemDataTypeFactory(DataTypeSpec.Timestamp);
			TinyInt = new SystemDataTypeFactory(DataTypeSpec.TinyInt);
			UniqueIdentifier = new SystemDataTypeFactory(DataTypeSpec.UniqueIdentifier);
			VarBinary = new SystemDataTypeFactory(DataTypeSpec.VarBinary);
			VarBinaryMax = new SystemDataTypeFactory(DataTypeSpec.VarBinaryMax);
			VarChar = new SystemDataTypeFactory(DataTypeSpec.VarChar);
			VarCharMax = new SystemDataTypeFactory(DataTypeSpec.VarCharMax);
			Variant = new SystemDataTypeFactory(DataTypeSpec.Variant);
			Xml = new SystemDataTypeFactory(DataTypeSpec.Xml);
			TypeFactories = new SystemDataTypeFactory[38]
			{
				null, BigInt, Binary, Bit, Char, Date, DateTime, DateTime2, DateTimeOffset, Decimal,
				Float, Geography, Geometry, HierarchyId, Image, Int, Money, NChar, NText, Numeric,
				NVarChar, NVarCharMax, Real, SmallDateTime, SmallInt, SmallMoney, SysName, Text, Time, Timestamp,
				TinyInt, UniqueIdentifier, VarBinary, VarBinaryMax, VarChar, VarCharMax, Variant, Xml
			};
		}

		public static SystemDataTypeFactory GetDataTypeFactory(SqlDataType sqlDataType)
		{
			return TypeFactories[(int)sqlDataType];
		}
	}

	private class XmlDataType : SystemDataType, IXmlDataType, ISystemDataType, IScalarDataType, IDataType, IMetadataObject
	{
		public bool IsXmlNode => false;

		public IMetadataCollection<IXmlDataTypeMethod> Methods => XmlDataTypeMethodCollection.Instance;

		public IMetadataCollection<IXmlDataTypeTableMethod> TableMethods => XmlDataTypeTableMethodCollection.Instance;

		public XmlDataType()
			: base(DataTypeSpec.Xml)
		{
		}

		public override T Accept<T>(IMetadataObjectVisitor<T> visitor)
		{
			if (visitor == null)
			{
				throw new ArgumentNullException("visitor");
			}
			return visitor.Visit(this);
		}
	}

	private const int ArgNotSet = int.MinValue;

	private readonly DataTypeSpec m_typeSpec;

	private readonly int m_length;

	private readonly int m_precision;

	private readonly int m_scale;

	private readonly bool m_ignoreArgs;

	public DataTypeSpec TypeSpec => m_typeSpec;

	public int Length => m_length;

	public int NumericPrecision => m_precision;

	public int NumericScale => m_scale;

	public bool IsSystem => true;

	public virtual bool IsClr => false;

	public bool IsXml => m_typeSpec.SqlDataType == SqlDataType.Xml;

	public bool IsVoid => false;

	public ISystemDataType BaseSystemDataType => this;

	public virtual IClrDataType AsClrDataType => null;

	public bool IsScalar => true;

	public bool IsTable => false;

	public bool IsCursor => false;

	public bool IsUnknown => false;

	public IScalarDataType AsScalarDataType => this;

	public ITableDataType AsTableDataType => null;

	public IUserDefinedType AsUserDefinedType => null;

	protected SystemDataType(DataTypeSpec typeSpec)
		: this(typeSpec, ignoreArgs: false)
	{
	}

	private SystemDataType(DataTypeSpec typeSpec, bool ignoreArgs)
		: this(typeSpec, int.MinValue, int.MinValue)
	{
		m_ignoreArgs = ignoreArgs;
	}

	private SystemDataType(DataTypeSpec typeSpec, int arg)
		: this(typeSpec, arg, int.MinValue)
	{
	}

	private SystemDataType(DataTypeSpec typeSpec, int arg1, int arg2)
		: base(GetDataTypeName(typeSpec))
	{
		m_typeSpec = typeSpec;
		if (typeSpec.ArgSpec2 != null)
		{
			if (arg1 == int.MinValue)
			{
				arg1 = typeSpec.ArgSpec1.DefaultValue;
			}
			if (arg2 == int.MinValue)
			{
				arg2 = typeSpec.ArgSpec2.DefaultValue;
			}
			m_precision = arg1;
			m_scale = arg2;
		}
		else if (typeSpec.ArgSpec1 != null)
		{
			if (arg1 == int.MinValue)
			{
				arg1 = typeSpec.ArgSpec1.DefaultValue;
			}
			if (typeSpec.ArgIsScale)
			{
				m_scale = arg1;
			}
			else
			{
				m_length = arg1;
			}
		}
	}

	public override T Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	private static string GetDataTypeName(DataTypeSpec typeSpec)
	{
		return typeSpec.Name.ToLowerInvariant();
	}
}
