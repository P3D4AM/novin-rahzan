namespace SharpCompress.Compressors.LZMA;

public class LzmaEncoderProperties
{
	internal CoderPropId[] _propIDs;

	internal object[] _properties;

	public LzmaEncoderProperties()
		: this(eos: false)
	{
	}

	public LzmaEncoderProperties(bool eos)
		: this(eos, 1048576)
	{
	}

	public LzmaEncoderProperties(bool eos, int dictionary)
		: this(eos, dictionary, 32)
	{
	}

	public LzmaEncoderProperties(bool eos, int dictionary, int numFastBytes)
	{
		int num = 2;
		int num2 = 3;
		int num3 = 0;
		int num4 = 2;
		string text = "bt4";
		_propIDs = new CoderPropId[8]
		{
			CoderPropId.DictionarySize,
			CoderPropId.PosStateBits,
			CoderPropId.LitContextBits,
			CoderPropId.LitPosBits,
			CoderPropId.Algorithm,
			CoderPropId.NumFastBytes,
			CoderPropId.MatchFinder,
			CoderPropId.EndMarker
		};
		_properties = new object[8] { dictionary, num, num2, num3, num4, numFastBytes, text, eos };
	}
}
