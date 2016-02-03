using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudio.Dsp;

namespace AlbumRecorder {
	/// <summary>
	/// Run a BiQuad filter on each channel of a SampleProvider
	/// </summary>
	public class FilteredSampleProvider : ISampleProvider {
		private readonly ISampleProvider sourceProvider;
		private readonly BiQuadFilter[] filters;
		private readonly int channels;
		public const float StandardBufferSizeSeconds = 0.05f;	// 1/20 second buffer

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="centreFrequency">For BiQuad filters</param>
		/// <param name="q">For BiQuad filters</param>
		public FilteredSampleProvider(ISampleProvider sourceProvider, float centreFrequency, float q) {
			this.sourceProvider = sourceProvider;
			channels = WaveFormat.Channels;
			StandardBufferSize = (int)WaveFormat.SecondsToSamples(StandardBufferSizeSeconds);
			filters = new BiQuadFilter[channels];
			for (int n = 0; n < channels; n++) {
				filters[n] = BiQuadFilter.BandPassFilterConstantPeakGain(WaveFormat.SampleRate, centreFrequency, q);
			}
		}

		/// <summary>
		/// Work out the average volume of the next block of length StandardBufferSize
		/// </summary>
		/// <param name="length">of the block in samples</param>
		/// <returns>Average volume</returns>
		/// <exception cref="System.IO.EndOfStreamException">At end of file</exception>
		public float AvgBlockVolume() {
			return AvgBlockVolume(StandardBufferSize);
		}

		/// <summary>
		/// Work out the average volume of the next block of given length (in samples)
		/// </summary>
		/// <param name="length">of the block in samples</param>
		/// <returns>Average volume</returns>
		/// <exception cref="System.IO.EndOfStreamException">At end of file</exception>
		public float AvgBlockVolume(int length) {
			float[] buffer = new float[length];
			float max = 0;
			if (Read(buffer, 0, length) != length)
				throw new System.IO.EndOfStreamException("End of file");
			for (int i = 0; i < length; i++)
				max += Math.Abs(buffer[i]);
			return max / length;
		}

		public int Read(float[] buffer, int offset, int count) {
			int samplesRead = sourceProvider.Read(buffer, offset, count);

			for (int n = 0; n < samplesRead; n++) {
				buffer[offset + n] = filters[n % channels].Transform(buffer[offset + n]);
			}
			return samplesRead;
		}

		public int StandardBufferSize { get; private set; }

		public WaveFormat WaveFormat {
			get { return sourceProvider.WaveFormat; }
		}
	}
}
