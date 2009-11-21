using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Moq.Tests
{
	public class ReturnSequenceHelperFixture
	{
		[Fact]
		public void PerformSequence()
		{
			var mock = new Mock<IFoo>();

			mock.SetupSequentials(x => x.Do())
				.Returns(2)
				.Returns(3)
				.Throws<InvalidOperationException>();

			Assert.Equal(2, mock.Object.Do());
			Assert.Equal(3, mock.Object.Do());
			Assert.Throws<InvalidOperationException>(() => { 
				mock.Object.Do(); 
			});
		}

		public interface IFoo
		{
			int Do();
		}
	}
}
