using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace AlbumRecorder {
	/// <summary>
	/// Extract a portion of a wave file
	/// </summary>
	public class ExtractWaveProvider : IWaveProvider {
		AudioFileReader _reader;
		long _length;		// Number of bytes left to read

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="start">in seconds</param>
		/// <param name="length">in seconds</param>
		public ExtractWaveProvider(AudioFileReader reader, float start, float length) {
			_reader = reader;
			// Position to start
			_reader.Position = _reader.WaveFormat.SecondsToBytes(start);
			// Number of bytes to read
			_length = _reader.WaveFormat.SecondsToBytes(length);
		}

		public int Read(byte[] buffer, int offset, int count) {
			if (count > _length)
				count = (int)_length;
			if (count <= 0)
				return 0;
			count = _reader.Read(buffer, offset, count);
			_length -= count;
			return count;
		}

		public WaveFormat WaveFormat {
			get { return _reader.WaveFormat; }
		}
	}
}
