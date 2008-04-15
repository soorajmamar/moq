using System;
using System.Reflection;
using System.Collections.Generic;

namespace Moq
{
	internal class MockEvent
	{
		List<Delegate> invocationList = new List<Delegate>();

		public Type ArgsType { get; protected set; }

		internal EventInfo Event { get; set; }

		internal void AddHandler(Delegate del)
		{
			invocationList.Add(del);
		}
	}

	internal class MockEvent<TEventArgs> : MockEvent
		where TEventArgs : EventArgs
	{
		public MockEvent()
		{
			base.ArgsType = typeof(TEventArgs);
		}

		public void Raise(TEventArgs args)
		{

		}

		public static implicit operator EventHandler<TEventArgs>(MockEvent<TEventArgs> mockEvent)
		{
			return mockEvent.Handle;
		}

		private void Handle(object sender, TEventArgs args)
		{
		}
	}
}
