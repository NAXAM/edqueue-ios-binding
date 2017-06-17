using System;
using EDQueue;
using FMDB;
using Foundation;
using ObjCRuntime;

namespace EDQueue
{
	// typedef void (^EDQueueCompletionBlock)(EDQueueResult);
	delegate void EDQueueCompletionBlock (int arg0);

	[Static]
	partial interface EDQueueConstants
	{
		// extern NSString *const EDQueueDidStart;
		[Field ("EDQueueDidStart", "__Internal")]
		NSString DidStart { get; }

		// extern NSString *const EDQueueDidStop;
		[Field ("EDQueueDidStop", "__Internal")]
		NSString DidStop { get; }

		// extern NSString *const EDQueueJobDidSucceed;
		[Field ("EDQueueJobDidSucceed", "__Internal")]
		NSString JobDidSucceed { get; }

		// extern NSString *const EDQueueJobDidFail;
		[Field ("EDQueueJobDidFail", "__Internal")]
		NSString JobDidFail { get; }

		// extern NSString *const EDQueueDidDrain;
		[Field ("EDQueueDidDrain", "__Internal")]
		NSString DidDrain { get; }
	}

	// @interface EDQueue : NSObject
	[BaseType (typeof(NSObject))]
	interface EDQueue
	{
		// +(EDQueue *)sharedInstance;
		[Static]
		[Export ("sharedInstance")]
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
		void EnqueueWithData ([NullAllowed]NSObject data, string task);

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
		int ProcessJob (EDQueue queue, NSDictionary job);

		// @optional -(void)queue:(EDQueue *)queue processJob:(NSDictionary *)job completion:(EDQueueCompletionBlock)block;
		[Export ("queue:processJob:completion:")]
		void ProcessJobWithCompletion (EDQueue queue, NSDictionary job, Action<int> block);
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
		nuint FetchJobCount ();

		// -(NSDictionary *)fetchJob;
		[Export ("fetchJob")]
		NSDictionary FetchJob();

		// -(NSDictionary *)fetchJobForTask:(id)task;
		[Export ("fetchJobForTask:")]
		NSDictionary FetchJobForTask (NSObject task);
	}

	// [Static]
	// [Verify (ConstantsInterfaceAssociation)]
	// partial interface Constants
	// {
	// 	// extern double EDQueueVersionNumber;
	// 	[Field ("EDQueueVersionNumber", "__Internal")]
	// 	double EDQueueVersionNumber { get; }

	// 	// extern const unsigned char [] EDQueueVersionString;
	// 	[Field ("EDQueueVersionString", "__Internal")]
	// 	byte[] EDQueueVersionString { get; }
	// }
}
