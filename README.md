## Queue - Xamarin iOS Binding Library
#### A persistent background job queue for iOS.

While `NSOperation` and `NSOperationQueue` work well for some repetitive problems and `NSInvocation` for others, iOS doesn't really include a set of tools for managing large collections of arbitrary background tasks easily. **EDQueue provides a high-level interface for implementing a threaded job queue using [GCD](http://developer.apple.com/library/ios/#documentation/Performance/Reference/GCD_libdispatch_Ref/Reference/reference.html) and [SQLLite3](http://www.sqlite.org/). All you need to do is handle the jobs within the provided delegate method and EDQueue handles the rest.**

### Getting Started
The easiest way to get going with EDQueue is to take a look at the included example application. The CSharp project file can be found in `demo > EDQueueQs.csproj`.

### Setup
```
Install-Package Naxam.EDQueue.iOS
```

EDQueue is implemented as a singleton as to allow jobs to be created from anywhere throughout an application. However, tasks are all processed through a single delegate method and thus it often makes the most sense to setup EDQueue within the application delegate:

YourAppDelegate.h
```c#
[Register("AppDelegate")]
public class AppDelegate : UIApplicationDelegate, IEDQueueDelegate
{
    public override void OnResignActivation(UIApplication application)
    {
        EDQueue.EDQueue.SharedInstance.Stop();
    }

    public override void OnActivated(UIApplication application)
    {
        EDQueue.EDQueue.SharedInstance.WeakDelegate = this;
        EDQueue.EDQueue.SharedInstance.Start();
    }
}
```

ViewController.cs
```c#
EDQueue.EDQueue.SharedInstance.EnqueueWithData(new NSDictionary("nya", "cat"), "success");
```

In order to keep things simple, the delegate method expects a return type of `EDQueueResult` which permits three distinct states:
- `EDQueueResultSuccess`: Used to indicate that a job has completed successfully
- `EDQueueResultFail`: Used to indicate that a job has failed and should be retried (up to the specified `retryLimit`)
- `EDQueueResultCritical`: Used to indicate that a job has failed critically and should not be attempted again

### Handling Async Jobs
As of v0.6.0 queue includes a delegate method suited for handling asyncronous jobs such as HTTP requests or [Disk I/O](https://github.com/thisandagain/storage):

```c#
[Export("queue:processJob:completion:")]
public void ProcessJobWithCompletion(EDQueue.EDQueue queue, NSDictionary job, System.Action<int> block)
{
    NSThread.SleepFor(1);

    try
    {
        var jobStatus = ((NSString)job.ObjectForKey(new NSString("task")))?.ToString();

        switch (jobStatus)
        {
            case "success":
                block?.Invoke((int)EDQueueResult.Success);
                break;
            case "fail":
                block?.Invoke((int)EDQueueResult.Fail);
                break;
            default:
                block?.Invoke((int)EDQueueResult.Critical);
                break;
        }
    } catch {
        block?.Invoke((int)EDQueueResult.Critical);
    }
}
```

### Introspection
As of v0.7.0 queue includes a collection of methods to aid in queue introspection specific to each task:
```c#
bool JobExistsForTask(string task);
bool jobIsActiveForTask(string task);
NSDictionary nextJobForTask(string task);
```

---

### Methods
```c#
void EnqueueWithData(NSObject data, string task);
void Start();
void Stop();
void Empty();

bool JobExistsForTask(string task);
bool jobIsActiveForTask(string task);
NSDictionary nextJobForTask(string task);
```

### Delegate Methods
```c#
int ProcessJob(EDQueue queue, NSDictionary job);
void ProcessJobWithCompletion(EDQueue.EDQueue queue, NSDictionary job, System.Action<int> block);
```

### Result Types
```
public enum EDQueueResult : long
{
    Success = 0,
    Fail,
    Critical
}
```

### Properties
```c#
NSObject delegate;
bool IsRunning;
bool IsActive;
uint RetryLimit;
```

### Notifications
```c#
EDQueueDidStart
EDQueueDidStop
EDQueueDidDrain
EDQueueJobDidSucceed
EDQueueJobDidFail
```

---

### iOS Support
EDQueue is designed for iOS 5 and up.

### ARC
EDQueue is built using ARC. If you are including EDQueue in a project that **does not** use [Automatic Reference Counting (ARC)](http://developer.apple.com/library/ios/#releasenotes/ObjectiveC/RN-TransitioningToARC/Introduction/Introduction.html), you will need to set the `-fobjc-arc` compiler flag on all of the EDQueue source files. To do this in Xcode, go to your active target and select the "Build Phases" tab. Now select all EDQueue source files, press Enter, insert `-fobjc-arc` and then "Done" to enable ARC for EDQueue.