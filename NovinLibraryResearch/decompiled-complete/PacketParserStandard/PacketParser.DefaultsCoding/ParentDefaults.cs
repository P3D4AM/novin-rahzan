namespace PacketParser.DefaultsCoding;

public class ParentDefaults
{
	public static TafsilCodingsDefaults TafsilCoding { get; } = new TafsilCodingsDefaults();


	public static MoeinCodingsDefaults MoeinCoding { get; } = new MoeinCodingsDefaults();


	public static SanadDefalts SanadGuidDefalts { get; } = new SanadDefalts();


	public static HesabDefault HesabDefault { get; } = new HesabDefault();


	public static KolCodingsDefaults KolCoding { get; } = new KolCodingsDefaults();


	public static TolidDefault TolidCoding { get; } = new TolidDefault();


	public static HesabGroupDefaults HesabGroupGuids { get; } = new HesabGroupDefaults();


	public static TafsilLevelDefaults TafsilLevelGuids { get; } = new TafsilLevelDefaults();


	public static HesabTypeDefults HesabTypeGuids { get; } = new HesabTypeDefults();


	public static CustomerDefault CusGuids { get; } = new CustomerDefault();


	public static ProductDefault ProductGuids { get; } = new ProductDefault();


	public static CustomerRoleDefaults CustomerRole { get; } = new CustomerRoleDefaults();


	public static SarNakhDefaults SarNakh { get; } = new SarNakhDefaults();


	public static PriceDefaults PriceLevel { get; } = new PriceDefaults();

}
