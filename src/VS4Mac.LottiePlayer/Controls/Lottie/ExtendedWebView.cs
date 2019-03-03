using CoreGraphics;
using Gtk;
using MonoDevelop.Components.Mac;
using WebKit;

namespace VS4Mac.LottiePlayer.Controls
{
	public class ExtendedWebView : WKWebView
    {
        public ExtendedWebView() : base(new CGRect(), new WKWebViewConfiguration()) { }

        public Widget GtkWidget => gtkWidget ?? (gtkWidget = GtkMacInterop.NSViewToGtkWidget(this));

        Widget gtkWidget;
    }
}