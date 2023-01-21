using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Search.Common
{
	public static partial class Constants
	{
		#region JSON serializer

		public static readonly JsonSerializerOptions DefaultJsonSerializeOptions = new JsonSerializerOptions()
		{
			AllowTrailingCommas = false,
			IncludeFields = true, //
			DefaultIgnoreCondition = JsonIgnoreCondition.Never,
			IgnoreReadOnlyFields = false,
			IgnoreReadOnlyProperties = false,
			NumberHandling = JsonNumberHandling.WriteAsString | JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.AllowNamedFloatingPointLiterals,
			PropertyNameCaseInsensitive = false,
			//DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
			//PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			ReadCommentHandling = JsonCommentHandling.Skip,
			MaxDepth = 128,
			WriteIndented = true
		};

		#endregion


		#region File Stream reader defaults

		public const int MaxRetryCount = 5;
		public const int MinRetryDelay = 25;
		public const int MaxRetryDelay = 75;
		public const int DefaultReadBufferSize = 65536;

		public static readonly FileStreamOptions DefaultStreamReadOptions = new FileStreamOptions()
		{
			Access = FileAccess.Read,
			BufferSize = DefaultReadBufferSize,
			Mode = FileMode.Open,
			Options = FileOptions.SequentialScan | FileOptions.Asynchronous,
			Share = FileShare.Read | FileShare.Delete | FileShare.Write
		};

		#endregion


		#region Directory Enumeration default options

		public static readonly EnumerationOptions DefaultDirEnumOptions = new EnumerationOptions()
		{
			MatchCasing = MatchCasing.PlatformDefault,
			MaxRecursionDepth = 0,
			IgnoreInaccessible = false,
			RecurseSubdirectories = true,
			ReturnSpecialDirectories = false,
			AttributesToSkip = (FileAttributes)0,
			MatchType = MatchType.Simple,
		};

		#endregion

		#region String constants related to hashing and computing the digest, including command line definitions

		public const string StringDefaultHashAlgorithm = @"SHA512";

		public const string StringNullDigestSHA512 = @"cf83e1357eefb8bdf1542850d66d8007d620e4050b5715dc83f4a921d36ce9ce47d0d13c5d85f2b0ff8318d2877eec2f63b931bd47417a81a538327af927da3e";
		public const string StringNullDigestSHA384 = @"38b060a751ac96384cd9327eb1b1e36a21fdb71114be07434c0cc7bf63f6e1da274edebfe76f65fbd51ad2f14898b95b";
		public const string StringNullDigestSHA256 = @"e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";
		public const string StringNullDigestSHA1 = @"da39a3ee5e6b4b0d3255bfef95601890afd80709";
		public const string StringNullDigestMD5 = @"d41d8cd98f00b204e9800998ecf8427e";
		public const string StringNullDigestMD4 = @"31d6cfe0d16ae931b73c59d7e0c089c0";

		public const string StringOptionHashName = @"hash";
		public const string StringOptionHashPrefix = $"{StringOptionPrefix}{StringOptionHashName}{StringOptionValueSeparator}";

		#endregion

		#region String constants used to customize collection and value formatting

		public const string StringElementDelimiter = @", ";

		public const string StringElementNamePrefix = "\"";
		public const string StringElementNameSuffix = "\"";

		public const string StringElementPairDelimiter = @" = ";

		public const string StringElementValuePrefix = "\"";
		public const string StringElementValueSuffix = "\"";

		public const string StringCollectionStartMarker = @"[ ";
		public const string StringCollectionEndMarker = @" ]";

		#endregion

		#region General String constants

		public const string StringLiteralNull = @"<null>";
		public const string StringLiteralEmpty = @"<empty>";

		public const string StringOptionPrefix = @"--";
		public const string StringOptionValueSeparator = @"=";

		public const string DefaultDirEnumPattern = "*";
		public const string DefaultFileEnumPattern = "*";

		public const string StringDec = @"0123456789";
		public const string StringHexUpper = $"{StringDec}ABCDEF";
		public const string StringHexLower = $"{StringDec}abcdef";

		#endregion

		#region Searching constants
		public const int SearchAlphabetSize = 256;
		#endregion
	};

}
