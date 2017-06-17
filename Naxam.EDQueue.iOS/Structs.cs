using System;
using ObjCRuntime;

namespace EDQueue
{
	[Native]
	public enum EDQueueResult : long
	{
		Success = 0,
		Fail,
		Critical
	}
}
