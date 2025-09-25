using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace BreakoutClone
{
	public class CachedSound
	{
		private readonly AudioFileReader reader;
		private readonly WaveOutEvent output;

		public CachedSound(string filePath)
		{
			reader = new AudioFileReader(filePath);
			output = new WaveOutEvent();
			output.Init(reader);
		}

		public void Play()
		{
			reader.Position = 0; //Rewing
			output.Play();
		}
	}
}
