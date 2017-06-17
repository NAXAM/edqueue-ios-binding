using System;
using ObjCRuntime;

namespace EDQueue
{
	[Native]
	public enum EDQueueResult : nint
	{
		Success = 0,
		Fail,
		Critical
	}
}
