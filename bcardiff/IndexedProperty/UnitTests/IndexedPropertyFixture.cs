using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Moq.Tests
{
	public class IndexedPropertyFixture
	{
		public interface IFoo
		{
			Bar this[string key] { get; set; }
			Bar this[int key] { get; set; }
			Bar this[int key1, int key2] { get; set; }
		}

		public class Bar { }

		Bar bar;
		Mock<IFoo> m;

		public IndexedPropertyFixture()
		{
			bar = new Bar();
			m = new Mock<IFoo>(MockBehavior.Strict);
		}

		[Fact]
		public void AllowGetByExactMatch()
		{
			m.SetupGet(x => x["magic-key"]).Returns(bar);
			Assert.Equal(bar, m.Object["magic-key"]);
		}

		[Fact]
		public void FailGetNotMatchingIndex()
		{
			m.SetupGet(x => x["magic-key"]).Returns(bar);
			Assert.Throws<MockException>(() => { var r = m.Object["boom"]; });
		}

		[Fact]
		public void AllowSetByExactMatch()
		{
			m.SetupSet(x => x["magic-key"]);
			m.Object["magic-key"] = bar;
		}

		[Fact]
		public void FailSetNotMatchingIndex()
		{
			m.SetupSet(x => x["magic-key"]);
			Assert.Throws<MockException>(() => { m.Object["boom"] = bar; });
		}

		[Fact]
		public void AllowSetForAny()
		{
			m.SetupSet(x => x[It.IsAny<string>()]);
			m.Object["foo"] = null;
			m.Object["bar"] = null;
		}

		[Fact]
		public void SetupIndexedPropertyAllowGetAndSet()
		{
			m.SetupProperty(x => x[It.IsAny<string>()]);
			m.Object["a"] = bar;
			Assert.Equal(bar, m.Object["a"]);
		}

		[Fact]
		public void SetupIndexedPropertyKeyAllowGetAndSet()
		{
			m.SetupProperty(x => x["good-key"]);
			m.Object["good-key"] = bar;
			Assert.Equal(bar, m.Object["good-key"]);
		}

		[Fact]
		public void SetupIndexedPropertyKeyFailSet()
		{
			m.SetupProperty(x => x["good-key"]);
			Assert.Throws<MockException>(() => { m.Object["other-key"] = bar; });
		}

		[Fact]
		public void SetupIndexedPropertyKeyFailGet()
		{
			m.SetupProperty(x => x["good-key"]);
			Assert.Throws<MockException>(() => { var r = m.Object["other-key"]; });
		}

		// VerifyGet
		// VerifySet
	}
}