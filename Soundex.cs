using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbumRecorder {
	/// <summary>
	/// Soundex algorithm for rough matching, from the Internet
	/// </summary>
	public class SoundEx {
		/// <summary>
		/// Encode a string to a Soundex
		/// </summary>
		public static string Encode(string s) {
			StringBuilder output=new StringBuilder();

			if(s.Length>0) {

				output.Append(Char.ToUpper(s[0]));

				// Stop at a maximum of 4 characters
				for(int i=1; i<s.Length && output.Length<4; i++) {
					string c=EncodeChar(s[i]);

					// Ignore duplicated chars, except a duplication with the first char
					if(i==1) {
						output.Append(c);
					} else if(c!=EncodeChar(s[i-1])) { 
						output.Append(c);
					}
				} 

				// Pad with zeros
				for(int i=output.Length; i<4; i++) {
					output.Append("0");
				}
			}
	
			return output.ToString();
		}

		/// <summary>
		/// See if two strings are roughly the same
		/// </summary>
		public static bool Equals(string s1, string s2) {
			return Encode(s1) == Encode(s2);
		}

		/// <summary>
		/// Encode a single character
		/// </summary>
		protected static string EncodeChar(char c) {
			switch (Char.ToLower(c)) {
				case 'b':
				case 'f':
				case 'p':
				case 'v':
					return "1";
				case 'c':
				case 'g':
				case 'j':
				case 'k':
				case 'q':
				case 's':
				case 'x':
				case 'z':
					return "2";
				case 'd':
				case 't':
					return "3";
				case 'l':
					return "4";
				case 'm':
				case 'n':
					return "5";
				case 'r':
					return "6";
				default:
					return string.Empty;
			}
		}
	}
}
