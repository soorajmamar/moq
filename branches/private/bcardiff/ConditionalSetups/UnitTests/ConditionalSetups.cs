using System;
using Xunit;

namespace Moq.Tests
{
	public class ConditionalSetups
	{
		[Fact]
		public void Lang()
		{
			var m = new Mock<IFoo>();

			m.Setup(x => x.M1())
				.When(() => true)
				.Returns("bar");

			m.Setup(x => x.M2())
				.When(() => true);
		}

		[Fact]
		public void ChooseExpectationThatHasAffirmativeCondition()
		{
			var m = new Mock<IFoo>();

			bool first = true;

			m.Setup(x => x.M1()).When(() => first).Returns("bar");
			m.Setup(x => x.M1()).When(() => !first).Returns("no bar");

			Assert.Equal("bar", m.Object.M1());
			first = false;
			Assert.Equal("no bar", m.Object.M1());
		}

		public interface IFoo
		{
			string M1();
			void M2();
		}
	}
}
