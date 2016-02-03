using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudio.CoreAudioApi;

namespace AlbumRecorder {
	/// <summary>
	/// Class to find silence and music
	/// </summary>
	public class GapFinder {
		AudioFileReader m_Reader;
		FilteredSampleProvider m_Provider;

		public GapFinder(AudioFileReader reader) {
			m_Reader = reader;
		}

		/// <summary>
		/// Work out the average volume of the next block
		/// </summary>
		/// <param name="length">of the block in samples</param>
		/// <returns>Average volume, or NaN at EOF</returns>
		private float avgBlockVolume(int length) {
			float[] buffer = new float[length];
			float max = 0;
			if (m_Provider.Read(buffer, 0, length) != length)
				return float.NaN;
			for (int i = 0; i < length; i++)
				max += Math.Abs(buffer[i]);
			return max / length;
		}

		/// <summary>
		/// Find the start of music
		/// </summary>
		/// <param name="start">seconds</param>
		/// <param name="maxLength">Max seconds to process (0 or less means rest of file)</param>
		/// <returns>True if music found</returns>
		public bool FindMusic(float start, float maxLength) {
			return FindGap(start, false, maxLength);
		}

		/// <summary>
		/// Find the start of silence
		/// </summary>
		/// <param name="start">seconds</param>
		/// <param name="maxLength">Max seconds to process (0 or less means rest of file)</param>
		/// <returns>True if silence found</returns>
		public bool FindSilence(float start, float maxLength) {
			return FindGap(start, true, maxLength);
		}

		/// <summary>
		/// Average volume of the file skipped during find(Music|Silence)
		/// </summary>
		public float AverageVolume { get; private set; }

		/// <summary>
		/// Position of found music or silence (in seconds)
		/// </summary>
		public float Position { get; private set; }

		const float bufferSize = 0.05f;	// 1/20 second buffer

		/// <summary>
		/// Find the start of music or silence
		/// </summary>
		/// <param name="start">seconds</param>
		/// <param name="silence">true to find silence, false to find music</param>
		/// <param name="maxLength">Max seconds to process (0 or less means rest of file)</param>
		/// <returns>True if music or silence found</returns>
		public bool FindGap(float start, bool silence, float maxLength) {
			if (start < 0)
				start = 0;
			// End of file
			Position = m_Reader.WaveFormat.BytesToSeconds(m_Reader.Length);
			AverageVolume = 0;
			if (start > Position)
				return false;	// Beyond end of file
			m_Reader.Position = Math.Max(0, m_Reader.WaveFormat.SecondsToBytes(start));
			// Try to work out where the music starts and/or ends
			m_Provider = new FilteredSampleProvider(m_Reader, Properties.Settings.Default.SilenceFilterCentre, Properties.Settings.Default.SilenceFilterQ);
			int length = m_Provider.StandardBufferSize;
			// Number of samples from beginning of file
			long count = m_Reader.WaveFormat.SecondsToSamples(start);
			float silenceThreshold = silence ? Properties.Settings.Default.StartSilenceThreshold : Properties.Settings.Default.EndSilenceThreshold;
			int foundCount = 0;		// Number of contiguous matching buffers found so far
			int minBuffers = 10;	// Number of contiguous buffers required
			int maxBuffers = maxLength <= 0 ? int.MaxValue : (int)(maxLength / bufferSize);	// Max buffers to read
			float[] volumes = new float[minBuffers + 1];	// Volumes of each of the last few buffers
			float totalVolume = 0;	// Lags minBuffers behind current read
			bool result = false;
			int i = 0;
			try {
				for ( ; i < maxBuffers; i++) {
					float avg = m_Provider.AvgBlockVolume();
					totalVolume += volumes[0];	// Accumulate total from minBuffers ago
					Array.Copy(volumes, 1, volumes, 0, minBuffers);	// Shift array down 1
					volumes[minBuffers] = avg;	// And add latest value
					if (silence == (avg <= silenceThreshold)) {
						// Found a buffer of what we are looking for
						if (++foundCount == minBuffers) {
							// Have found minBuffers contiguous of what we are looking for
							count -= foundCount * length;	// Go back to first buffer that was what we are looking for
							if (!silence)
								count--;		// leave a buffer of silent gap before the start of the music
							Position = m_Reader.WaveFormat.SamplesToSeconds(count);	// Position in seconds
							result = true;
							break;
						}
					} else {
						foundCount = 0;			// Not enough contiguous buffers - restart count
					}
					count += length;
				}
			} catch (System.IO.EndOfStreamException) {
			}
			AverageVolume = totalVolume / (i - foundCount);
			return result;
		}

	}
}
