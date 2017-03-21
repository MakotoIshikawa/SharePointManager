using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Globalization;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// �������⏕����g�����\�b�h��񋟂��܂��B
	/// </summary>
	public static partial class StringExtension {
		#region ���\�b�h

		#region �������O�擾

		/// <summary>
		/// �������O���擾���܂��B
		/// </summary>
		/// <param name="this">���b�Z�[�W</param>
		/// <remarks>
		/// ������t��������������擾���܂��B</remarks>
		public static string GetTimeLog(this string @this) {
			var milliSecond = DateTime.Now.ToMilliSecondString();
			return $"{milliSecond} {@this}";
		}

		#endregion

		#region �����񌟍�

		/// <summary>
		/// �w�肳�ꂽ�����񂪑��݂��邩�ǂ����������܂��B
		/// </summary>
		/// <param name="this">�e�X�g���镶����B</param>
		/// <param name="word">�������镶����</param>
		/// <returns>���̕����񂪌��������ꍇ�́Atrue�B
		/// ������Ȃ������ꍇ�� false�B</returns>
		public static bool HasString(this String @this, String word) {
			if (@this.IsEmpty() || word.IsEmpty()) {
				return false;
			}

			return !(@this.IndexOf(word) < 0);
		}

		#endregion

		#region �R�����g�A�E�g

		/// <summary>
		/// �����񂩂�w�肳�ꂽ�L���ȍ~�̕�������R�����g�A�E�g���܂��B
		/// </summary>
		/// <param name="this">�Ώە�����</param>
		/// <param name="sign">�L��</param>
		/// <returns>�R�����g�A�E�g���ꂽ�������Ԃ��܂��B</returns>
		public static string CommentOut(this string @this, string sign) {
			if (sign.IsEmpty()) {
				return @this;
			}

			var index = @this.IndexOf(sign);
			if (index != -1) {
				@this = @this.Remove(index);
			}

			return @this.TrimEnd();
		}

		#endregion

		#region Nullable �ϊ�

		#region short? �ɕϊ�

		/// <summary>
		/// short? �^�ɕϊ����܂��B
		/// </summary>
		/// <param name="s">������</param>
		/// <returns>Nullable �l�ɕϊ������l��Ԃ��܂��B</returns>
		public static short? ToNullableShort(this string s) {
			short result;
			if (short.TryParse(s, out result)) {
				return result;
			}
			return null;
		}

		#endregion

		#region int? �ɕϊ�

		/// <summary>
		/// int? �^�ɕϊ����܂��B
		/// </summary>
		/// <param name="s">������</param>
		/// <returns>Nullable �l�ɕϊ������l��Ԃ��܂��B</returns>
		public static int? ToNullableInt(this string s) {
			int result;
			if (int.TryParse(s, out result)) {
				return result;
			}
			return null;
		}

		#endregion

		#region long? �ɕϊ�

		/// <summary>
		/// long? �^�ɕϊ����܂��B
		/// </summary>
		/// <param name="s">������</param>
		/// <returns>Nullable �l�ɕϊ������l��Ԃ��܂��B</returns>
		public static long? ToNullableLong(this string s) {
			long result;
			if (long.TryParse(s, out result)) {
				return result;
			}
			return null;
		}

		#endregion

		#region uint? �ɕϊ�

		/// <summary>
		/// uint? �^�ɕϊ����܂��B
		/// </summary>
		/// <param name="s">������</param>
		/// <returns>Nullable �l�ɕϊ������l��Ԃ��܂��B</returns>
		public static uint? ToNullableUint(this string s) {
			uint result;
			if (uint.TryParse(s, out result)) {
				return result;
			}
			return null;
		}

		#endregion

		#region float? �ɕϊ�

		/// <summary>
		/// float? �^�ɕϊ����܂��B
		/// </summary>
		/// <param name="s">������</param>
		/// <returns>Nullable �l�ɕϊ������l��Ԃ��܂��B</returns>
		public static float? ToNullableFloat(this string s) {
			float result;
			if (float.TryParse(s, out result)) {
				return result;
			}
			return null;
		}

		#endregion

		#region double? �ɕϊ�

		/// <summary>
		/// double? �^�ɕϊ����܂��B
		/// </summary>
		/// <param name="s">������</param>
		/// <returns>Nullable �l�ɕϊ������l��Ԃ��܂��B</returns>
		public static double? ToNullableDouble(this string s) {
			double result;
			if (double.TryParse(s, out result)) {
				return result;
			}
			return null;
		}

		#endregion

		#region decimal? �ɕϊ�

		/// <summary>
		/// decimal? �^�ɕϊ����܂��B
		/// </summary>
		/// <param name="s">������</param>
		/// <returns>Nullable �l�ɕϊ������l��Ԃ��܂��B</returns>
		public static decimal? ToNullableDecimal(this string s) {
			decimal result;
			if (decimal.TryParse(s, out result)) {
				return result;
			}
			return null;
		}

		#endregion

		#region bool? �ɕϊ�

		/// <summary>
		/// bool? �^�ɕϊ����܂��B
		/// </summary>
		/// <param name="s">������</param>
		/// <returns>Nullable �l�ɕϊ������l��Ԃ��܂��B</returns>
		public static bool? ToNullableBool(this string s) {
			bool result;
			if (bool.TryParse(s, out result)) {
				return result;
			}
			return null;
		}

		#endregion

		#endregion

		#region �J��Ԃ������񐶐�
		/// <summary>
		/// �J��Ԃ������񐶐�
		/// </summary>
		/// <param name="s">������</param>
		/// <param name="repeat">��</param>
		/// <returns>����������</returns>
		public static string Repeat(this string s, int repeat) {
			if (s.IsEmpty()) {
				return s;
			}

			var sb = new StringBuilder();
			for (int i = 0; i < repeat; i++) {
				sb.Append(s);
			}

			return sb.ToString();
		}
		#endregion

		#region Stream����

		/// <summary>
		/// MemoryStream �𐶐����܂��B
		/// </summary>
		/// <param name="this">������</param>
		/// <param name="encoding">�G���R�[�f�B���O</param>
		/// <returns>MemoryStream �̐V�����C���X�^���X��Ԃ��܂��B</returns>
		public static MemoryStream CreateStream(this string @this, Encoding encoding) {
			return new MemoryStream(encoding.GetBytes(@this));
		}

		#endregion

		#region �󕶎�����

		/// <summary>
		/// �w�肳�ꂽ������ null �܂��� System.String.Empty ������ł��邩�ǂ����������܂��B
		/// </summary>
		/// <param name="this">������</param>
		/// <returns>null �܂��͋�̕����� ("") �̏ꍇ�� true�B
		/// ����ȊO�̏ꍇ�� false�B</returns>
		public static bool IsEmpty(this string @this) {
			return string.IsNullOrEmpty(@this);
		}

		/// <summary>
		/// �w�肳�ꂽ������ null �܂��͋�ł��邩�A�󔒕��������ō\������Ă��邩�ǂ����������܂��B
		/// </summary>
		/// <param name="this">������</param>
		/// <returns>null �܂��� �܂��͋�̕����� ("") �ł��邩�A�󔒕��������ō\������Ă���ꍇ�� true�B
		/// ����ȊO�̏ꍇ�� false�B</returns>
		public static bool IsWhiteSpace(this string @this) {
			return string.IsNullOrWhiteSpace(@this);
		}

		#endregion

		#region ������A��

		/// <summary>
		/// String �R���N�V�����̃����o�[��A�����܂��B�e�����o�[�̊Ԃɂ́A�w�肵����؂�L�����}������܂��B
		/// </summary>
		/// <param name="this">�A�����镶������i�[���Ă���R���N�V����</param>
		/// <param name="separator">��؂�L���Ƃ��Ďg�p���镶����</param>
		/// <returns>separator ������ŋ�؂�ꂽ�������Ԃ��܂��B</returns>
		public static string Join(this IEnumerable<string> @this, string separator = "") {
			if (!(@this?.Any() ?? false)) {
				return null;
			}

			return string.Join(separator, @this);
		}

		#endregion

		#region Base64 �ϊ�

		/// <summary>
		/// Base64 �̐����ŃG���R�[�h���ꂽ�����̕�����`���ɕϊ����܂��B
		/// </summary>
		/// <param name="this">������</param>
		/// <returns>Base64 �̐����ŃG���R�[�h���ꂽ�������Ԃ��܂��B</returns>
		public static string ToBase64(this string @this) {
			return @this.ToBase64(Encoding.UTF8);
		}

		/// <summary>
		/// �G���R�[�h���w�肵�āA
		/// Base64 �̐����ŃG���R�[�h���ꂽ�����̕�����`���ɕϊ����܂��B
		/// </summary>
		/// <param name="this">������</param>
		/// <param name="enc">�G���R�[�h</param>
		/// <returns>Base64 �̐����ŃG���R�[�h���ꂽ�������Ԃ��܂��B</returns>
		public static string ToBase64(this string @this, Encoding enc) {
			if (@this.IsEmpty()) {
				return string.Empty;
			}

			return Convert.ToBase64String(enc.GetBytes(@this));
		}

		/// <summary>
		/// Base64 �̐����ŃG���R�[�h���ꂽ�����񂩂當����ɕϊ����܂��B
		/// </summary>
		/// <param name="this">Base64 �����G���R�[�h������</param>
		/// <returns>�ϊ����ꂽ�������Ԃ��܂��B</returns>
		public static string FromBase64(this string @this) {
			return @this.FromBase64(Encoding.UTF8);
		}

		/// <summary>
		/// �G���R�[�h���w�肵�āA
		/// Base64 �̐����ŃG���R�[�h���ꂽ�����񂩂當����ɕϊ����܂��B
		/// </summary>
		/// <param name="this">Base64 �����G���R�[�h������</param>
		/// <param name="enc">�G���R�[�h</param>
		/// <returns>�ϊ����ꂽ�������Ԃ��܂��B</returns>
		public static string FromBase64(this string @this, Encoding enc) {
			if (@this.IsEmpty()) {
				return string.Empty;
			}

			return enc.GetString(Convert.FromBase64String(@this));
		}

		#endregion

		#region �l�擾

		/// <summary>
		/// null ���ǂ����𔻒肵�ĕ�������擾���܂��B
		/// </summary>
		/// <param name="this">������</param>
		/// <returns>null �ꍇ string.Empty ��Ԃ��܂��B</returns>
		public static string GetValueOrEmpty(this string @this) {
			return @this.IsEmpty() ? string.Empty : @this;
		}

		#endregion

		#region �؂�o��

		/// <summary>
		/// ������̍��[����w�肳�ꂽ���������̕�������擾���܂��B
		/// </summary>
		/// <param name="this">string</param>
		/// <param name="length">���o��������</param>
		/// <returns>���o�����������Ԃ��܂��B</returns>
		public static string Left(this string @this, int length) {
			return (new string(@this.Take(length).ToArray())).TrimEnd();
		}

		/// <summary>
		/// �w�肳�ꂽ�ʒu���當������擾���܂��B
		/// </summary>
		/// <param name="this">string</param>
		/// <param name="start">�J�n�ʒu</param>
		/// <returns>���o�����������Ԃ��܂��B</returns>
		public static string Mid(this string @this, int start) {
			return (new string(@this.Skip(start - 1).ToArray())).TrimEnd();
		}

		/// <summary>
		/// �w�肳�ꂽ�ʒu����A�w�肳�ꂽ���������̕�������擾���܂��B
		/// </summary>
		/// <param name="this">string</param>
		/// <param name="start">�J�n�ʒu</param>
		/// <param name="length">���o��������</param>
		/// <returns>���o�����������Ԃ��܂��B</returns>
		public static string Mid(this string @this, int start, int length) {
			return (new string(@this.Skip(start - 1).Take(length).ToArray())).TrimEnd();
		}

		/// <summary>
		/// ������̉E�[����w�肳�ꂽ���������̕�������擾���܂��B
		/// </summary>
		/// <param name="this">string</param>
		/// <param name="length">���o��������</param>
		/// <returns>���o�����������Ԃ��܂��B</returns>
		public static string Right(this string @this, int length) {
			var cnt = @this.Length - length;
			return (new string(@this.Skip(cnt).ToArray())).Trim();
		}

		#endregion

		#region �����ϊ�

		/// <summary>
		/// ������� DateTime �ɕϊ����܂��B
		/// </summary>
		/// <param name="this">string</param>
		/// <param name="format">����</param>
		/// <returns>DateTime ��Ԃ��܂��B</returns>
		public static DateTime? ToDateTime(this string @this, string format = null) {
			if (format.IsEmpty()) {
				DateTime result;
				if (!DateTime.TryParse(@this, out result)) {
					return null;
				}
				return result;
			} else {
				DateTime result;
				if (!DateTime.TryParseExact(@this, format, null, DateTimeStyles.None, out result)) {
					return null;
				}
				return result;
			}
		}

		#endregion

		#endregion
	}
}