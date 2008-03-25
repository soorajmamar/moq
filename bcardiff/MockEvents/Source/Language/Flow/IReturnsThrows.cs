﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moq.Language.Flow
{
	/// <summary>
	/// Implements the fluent API.
	/// </summary>
	public interface IReturnsThrows<TResult> : IReturns<TResult>, IThrows, IHideObjectMembers
	{
	}

	/// <summary>
	/// Implements the fluent API.
	/// </summary>
	public interface IReturnsThrowsGetter<TProperty> : IReturnsGetter<TProperty>, IThrows, IHideObjectMembers
	{
	}
}
