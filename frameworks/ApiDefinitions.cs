using System;
using EDQueue;
using Foundation;
using ObjCRuntime;

namespace EDQueue
{
	// typedef void (^EDQueueCompletionBlock)(EDQueueResult);
	delegate void EDQueueCompletionBlock (EDQueueResult arg0);

	[Static]
	[Verify (ConstantsInterfaceAssociation)]
	partial interface Constants
	{
		// extern NSString *const EDQueueDidStart;
		[Field ("EDQueueDidStart", "__Internal")]
		NSString EDQueueDidStart { get; }

		// extern NSString *const EDQueueDidStop;
		[Field ("EDQueueDidStop", "__Internal")]
		NSString EDQueueDidStop { get; }

		// extern NSString *const EDQueueJobDidSucceed;
		[Field ("EDQueueJobDidSucceed", "__Internal")]
		NSString EDQueueJobDidSucceed { get; }

		// extern NSString *const EDQueueJobDidFail;
		[Field ("EDQueueJobDidFail", "__Internal")]
		NSString EDQueueJobDidFail { get; }

		// extern NSString *const EDQueueDidDrain;
		[Field ("EDQueueDidDrain", "__Internal")]
		NSString EDQueueDidDrain { get; }
	}

	// @interface EDQueue : NSObject
	[BaseType (typeof(NSObject))]
	interface EDQueue
	{
		// +(EDQueue *)sharedInstance;
		[Static]
		[Export ("sharedInstance")]
		[Verify (MethodToProperty)]
		EDQueue SharedInstance { get; }

		[Wrap ("WeakDelegate")]
		EDQueueDelegate Delegate { get; set; }

		// @property (nonatomic, weak) id<EDQueueDelegate> delegate;
		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		// @property (readonly, nonatomic) BOOL isRunning;
		[Export ("isRunning")]
		bool IsRunning { get; }

		// @property (readonly, nonatomic) BOOL isActive;
		[Export ("isActive")]
		bool IsActive { get; }

		// @property (nonatomic) NSUInteger retryLimit;
		[Export ("retryLimit")]
		nuint RetryLimit { get; set; }

		// -(void)enqueueWithData:(id)data forTask:(NSString *)task;
		[Export ("enqueueWithData:forTask:")]
		void EnqueueWithData (NSObject data, string task);

		// -(void)start;
		[Export ("start")]
		void Start ();

		// -(void)stop;
		[Export ("stop")]
		void Stop ();

		// -(void)empty;
		[Export ("empty")]
		void Empty ();

		// -(BOOL)jobExistsForTask:(NSString *)task;
		[Export ("jobExistsForTask:")]
		bool JobExistsForTask (string task);

		// -(BOOL)jobIsActiveForTask:(NSString *)task;
		[Export ("jobIsActiveForTask:")]
		bool JobIsActiveForTask (string task);

		// -(NSDictionary *)nextJobForTask:(NSString *)task;
		[Export ("nextJobForTask:")]
		NSDictionary NextJobForTask (string task);
	}

	// @protocol EDQueueDelegate <NSObject>
	[Protocol, Model]
	[BaseType (typeof(NSObject))]
	interface EDQueueDelegate
	{
		// @optional -(EDQueueResult)queue:(EDQueue *)queue processJob:(NSDictionary *)job;
		[Export ("queue:processJob:")]
		EDQueueResult Queue (EDQueue queue, NSDictionary job);

		// @optional -(void)queue:(EDQueue *)queue processJob:(NSDictionary *)job completion:(EDQueueCompletionBlock)block;
		[Export ("queue:processJob:completion:")]
		void Queue (EDQueue queue, NSDictionary job, EDQueueCompletionBlock block);
	}

	// @interface EDQueueStorageEngine : NSObject
	[BaseType (typeof(NSObject))]
	interface EDQueueStorageEngine
	{
		// @property (retain) FMDatabaseQueue * queue;
		[Export ("queue", ArgumentSemantic.Retain)]
		FMDatabaseQueue Queue { get; set; }

		// -(void)createJob:(id)data forTask:(id)task;
		[Export ("createJob:forTask:")]
		void CreateJob (NSObject data, NSObject task);

		// -(BOOL)jobExistsForTask:(id)task;
		[Export ("jobExistsForTask:")]
		bool JobExistsForTask (NSObject task);

		// -(void)incrementAttemptForJob:(NSNumber *)jid;
		[Export ("incrementAttemptForJob:")]
		void IncrementAttemptForJob (NSNumber jid);

		// -(void)removeJob:(NSNumber *)jid;
		[Export ("removeJob:")]
		void RemoveJob (NSNumber jid);

		// -(void)removeAllJobs;
		[Export ("removeAllJobs")]
		void RemoveAllJobs ();

		// -(NSUInteger)fetchJobCount;
		[Export ("fetchJobCount")]
		[Verify (MethodToProperty)]
		nuint FetchJobCount { get; }

		// -(NSDictionary *)fetchJob;
		[Export ("fetchJob")]
		[Verify (MethodToProperty)]
		NSDictionary FetchJob { get; }

		// -(NSDictionary *)fetchJobForTask:(id)task;
		[Export ("fetchJobForTask:")]
		NSDictionary FetchJobForTask (NSObject task);
	}

	[Static]
	[Verify (ConstantsInterfaceAssociation)]
	partial interface Constants
	{
		// extern double EDQueueVersionNumber;
		[Field ("EDQueueVersionNumber", "__Internal")]
		double EDQueueVersionNumber { get; }

		// extern const unsigned char [] EDQueueVersionString;
		[Field ("EDQueueVersionString", "__Internal")]
		byte[] EDQueueVersionString { get; }
	}
}
