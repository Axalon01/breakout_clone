using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutClone
{
	public class GameOverEventArgs : EventArgs
	{
		public string Message { get; }
		public GameOverEventArgs(string message)
		{
			Message = message;
		}
	}
}
