using System.Diagnostics;
using System.IO;
using Foundation;
using WebKit;

namespace VS4Mac.LottiePlayer.Controls
{
	public class LottiePlayer : ExtendedWebView, IWKNavigationDelegate
	{
		const string HTML_RESOURCE = "LottiePlayer";

		public LottiePlayer()
		{
			NavigationDelegate = new LottiePlayerNavigationDelegate();
		}

		public string Animation { get; set; }

		public WKNavigation InitialNavigation { get; set; }

		public bool Initialized { get; set; }

		public void SetData(string data)
		{
			Animation = data;

			if (!Initialized)
			{
				var html = CreateHtml(Animation);
				InitialNavigation = LoadHtmlString(html, null);
				return;
			}
		}

		static string CreateHtml(string data)
		{
			return LoadStringFromResource(HTML_RESOURCE)
				.Replace("{REPLACE_WITH_ANIMATION}", data);
		}

		static string LoadStringFromResource(string id)
		{
			using (var stream = typeof(LottiePlayer).Assembly.GetManifestResourceStream(id))
			using (var reader = new StreamReader(stream))
				return reader.ReadToEnd();
		}
	}

	public class LottiePlayerNavigationDelegate : WKNavigationDelegate
	{
		public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
		{
			var lottiePlayer = webView as LottiePlayer;

			if (navigation == lottiePlayer.InitialNavigation)
			{
				lottiePlayer.InitialNavigation = null;
				lottiePlayer.Initialized = true;
				lottiePlayer.SetData(lottiePlayer.Animation);
			}
		}
	}
}