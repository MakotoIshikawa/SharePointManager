using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// �������⏕����g�����\�b�h��񋟂��܂��B
	/// </summary>
	public static partial class StringExtension {
		#region �������O�擾

		/// <summary>
		/// �������O���擾���܂��B
		/// </summary>
		/// <param name="this">���b�Z�[�W</param>
		/// <remarks>
		/// ������t��������������擾���܂��B</remarks>
		public static String GetTimeLog(this String @this) {
			return DateTime.Now.ToString("yyyy/MM/dd HH':'mm':'ss.fff ") + @this;
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

		#region ���\�b�h

		#region Nullable �ϊ�

		#region int? �ɕϊ�

		/// <summary>
		/// int? �ɕϊ�
		/// </summary>
		/// <param name="s">������</param>
		/// <returns>Nullable �l�ɕϊ������l��Ԃ��܂��B</returns>
		public static int? ToNullableInt(this string s) {
			int i;
			if (int.TryParse(s, out i))
				return i;
			return null;
		}

		#endregion

		#region bool? �ɕϊ�

		/// <summary>
		/// bool? �ɕϊ�
		/// </summary>
		/// <param name="s">������</param>
		/// <returns>Nullable �l�ɕϊ������l��Ԃ��܂��B</returns>
		public static bool? ToNullableBool(this string s) {
			bool b;
			if (bool.TryParse(s, out b))
				return b;
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

		#region �t�V���A����

		#region DeserializeFromXml (�I�[�o�[���[�h +1)

		/// <summary>
		/// XML ��������t�V���A�������܂��B
		/// </summary>
		/// <typeparam name="TResult">�t�V���A��������^</typeparam>
		/// <param name="this">XML ������</param>
		/// <returns>�t�V���A�������ꂽ�I�u�W�F�N�g</returns>
		public static TResult DeserializeFromXml<TResult>(this string @this) {
			return @this.DeserializeFromXml<TResult>(Encoding.UTF8);
		}

		/// <summary>
		/// XML ��������t�V���A�������܂��B
		/// </summary>
		/// <typeparam name="TResult">�t�V���A��������^</typeparam>
		/// <param name="this">XML ������</param>
		/// <param name="encoding">�G���R�[�f�B���O</param>
		/// <returns>�t�V���A�������ꂽ�I�u�W�F�N�g</returns>
		public static TResult DeserializeFromXml<TResult>(this string @this, Encoding encoding) {
			if (@this.IsEmpty()) {
				return default(TResult);
			}

			using (var ms = @this.CreateStream(encoding)) {
				return ms.DeserializeFromXml<TResult>();
			}
		}

		#endregion

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

		#region �񋓌^�ϊ�

		/// <summary>
		/// �������񋓌^�ɕϊ����܂��B
		/// </summary>
		/// <typeparam name="TResult">�񋓌^</typeparam>
		/// <param name="this">������</param>
		/// <param name="defaultValue">�f�t�H���g�l</param>
		/// <returns>�񋓌^��Ԃ��܂��B</returns>
		public static TResult ToEnum<TResult>(this string @this, TResult? defaultValue = null) where TResult : struct {
			if (@this.IsEmpty()) {
				if (!defaultValue.HasValue) {
					throw new ArgumentNullException("this", "�w�肳�ꂽ������ null �܂��� Empty �ł��B");
				}

				return defaultValue.Value;
			}

			return (TResult)Enum.Parse(typeof(TResult), @this);
		}

		/// <summary>
		/// �������񋓌^�ɕϊ����܂��B
		/// </summary>
		/// <typeparam name="TResult">�񋓌^</typeparam>
		/// <param name="this">���l(int)</param>
		/// <param name="defaultValue">�f�t�H���g�l</param>
		/// <returns>�񋓌^��Ԃ��܂��B</returns>
		public static TResult ToEnum<TResult>(this int @this, TResult? defaultValue = null) where TResult : struct {
			return @this.ToString().ToEnum<TResult>();
		}

		/// <summary>
		/// �������񋓌^�ɕϊ����܂��B
		/// </summary>
		/// <typeparam name="TResult">�񋓌^</typeparam>
		/// <param name="this">���l(short)</param>
		/// <param name="defaultValue">�f�t�H���g�l</param>
		/// <returns>�񋓌^��Ԃ��܂��B</returns>
		public static TResult ToEnum<TResult>(this short @this, TResult? defaultValue = null) where TResult : struct {
			return @this.ToString().ToEnum<TResult>();
		}

		/// <summary>
		/// �������񋓌^�ɕϊ����܂��B
		/// </summary>
		/// <typeparam name="TResult">�񋓌^</typeparam>
		/// <param name="this">���l(byte)</param>
		/// <param name="defaultValue">�f�t�H���g�l</param>
		/// <returns>�񋓌^��Ԃ��܂��B</returns>
		public static TResult ToEnum<TResult>(this byte @this, TResult? defaultValue = null) where TResult : struct {
			return @this.ToString().ToEnum<TResult>();
		}

		#endregion

		#region GetValueOrEmpty

		/// <summary>
		/// null ���ǂ����𔻒肵�ĕ�������擾���܂��B
		/// </summary>
		/// <param name="this">������</param>
		/// <returns>null �ꍇ string.Empty ��Ԃ��܂��B</returns>
		public static string GetValueOrEmpty(this string @this) {
			return @this.IsEmpty() ? string.Empty : @this;
		}

		#endregion

		#endregion
	}
}