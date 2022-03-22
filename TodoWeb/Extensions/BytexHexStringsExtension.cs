using System.Globalization;
using System.Text;

namespace TodoWeb.Extensions
{
    public static class BytexHexStringsExtension
    {
        public static byte[] ToByteArray(this string hexString)
        {
            if (hexString == null || (hexString.Length % 2) != 0)
            {
                throw new FormatException("Invalid hex string. Hex strings must have an even number of characters.");
            }
            byte[] bytes = new byte[hexString.Length / 2];
            for (int i=0; i<hexString.Length; i+=2)
            {
                bytes[i / 2] = byte.Parse(hexString.Substring(i, 2), NumberStyles.HexNumber);
            }
            return bytes;
        }
        public static string ToHexString(this byte[] bytes)
        {
            StringBuilder byteString = new();
            foreach (var b in bytes) {
                byteString.Append(b.ToString("X2"));
            }
            return byteString.ToString();
        }
    }
}
