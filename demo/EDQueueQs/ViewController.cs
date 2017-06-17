using System;
using Foundation;
using UIKit;

namespace EDQueueQs
{
    public partial class ViewController : UIViewController
    {
        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            var nc = NSNotificationCenter.DefaultCenter;

            nc.AddObserver(new NSString("EDQueueJobDidSucceed"), NotificationReceived);
            nc.AddObserver(new NSString("EDQueueJobDidFail"), NotificationReceived);
            nc.AddObserver(new NSString("EDQueueDidStart"), NotificationReceived);
            nc.AddObserver(new NSString("EDQueueDidStop"), NotificationReceived);
            nc.AddObserver(new NSString("EDQueueDidDrain"), NotificationReceived);

            btnAddSuccess.TouchUpInside += delegate
            {
                EDQueue.EDQueue.SharedInstance.EnqueueWithData(new NSDictionary("nya", "cat"), "success");
            };
            btnAddFail.TouchUpInside += delegate
            {
                EDQueue.EDQueue.SharedInstance.EnqueueWithData(null, "fail");
            };
            btnAddCritical.TouchUpInside += delegate
            {
                EDQueue.EDQueue.SharedInstance.EnqueueWithData(null, "critical");
            };

            txtActivity.Text = string.Empty;
        }

        void NotificationReceived(NSNotification obj)
        {
            txtActivity.Text = $@"
{txtActivity.Text}
---
{obj}         
";
            txtActivity.ScrollRangeToVisible(new NSRange(txtActivity.Text.Length, 0));
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            return (toInterfaceOrientation == UIInterfaceOrientation.Portrait);
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();

            NSNotificationCenter.DefaultCenter.RemoveObserver(this);
        }
    }
}
