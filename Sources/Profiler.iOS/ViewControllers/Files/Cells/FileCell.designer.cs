// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Profiler.iOS
{
    [Register ("FileCell")]
    partial class FileCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel icon { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel name { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel size { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel time { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (icon != null) {
                icon.Dispose ();
                icon = null;
            }

            if (name != null) {
                name.Dispose ();
                name = null;
            }

            if (size != null) {
                size.Dispose ();
                size = null;
            }

            if (time != null) {
                time.Dispose ();
                time = null;
            }
        }
    }
}