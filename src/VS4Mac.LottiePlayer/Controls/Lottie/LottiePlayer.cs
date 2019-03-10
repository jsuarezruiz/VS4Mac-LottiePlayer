using System;
using System.IO;
using System.Threading.Tasks;
using WebKit;

namespace VS4Mac.LottiePlayer.Controls
{
	public class LottiePlayer : ExtendedWebView, IWKNavigationDelegate
	{
		const string HTML_RESOURCE = "LottiePlayer";

		public event EventHandler DurationChanged;

		public LottiePlayer()
		{
			NavigationDelegate = new LottiePlayerNavigationDelegate();
		}

		public string Animation { get; set; }

		public WKNavigation InitialNavigation { get; set; }

		public bool Initialized { get; set; }

		public double Duration { get; internal set; }

		public async Task SetDataAsync(string data)
		{
			Animation = data;

			if (!Initialized)
			{
				var html = CreateHtml(Animation);
				InitialNavigation = LoadHtmlString(html, null);
				await Task.Delay(500);
				LoadAnimationDuration();
				return;
			}
		}

		public void Play()
		{
			EvaluateJavaScript($"window.animation.play();", null);
		}

		public void Pause()
		{
			EvaluateJavaScript($"window.animation.pause();", null);
			LoadAnimationDuration();
		}

		public void GoToAndStop(double value)
		{
			Pause();
			EvaluateJavaScript($"window.animation.goToAndStop({value}, true);", null);
		}

		void LoadAnimationDuration()
		{
			EvaluateJavaScript($"getAnimationDuration();", (result, error) =>
			{
				var durationString = result.ToString();
				Duration = Convert.ToDouble(durationString);
				DurationChanged?.Invoke(this, null);
			});
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
		public override async void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
		{
			var lottiePlayer = webView as LottiePlayer;

			if (navigation == lottiePlayer.InitialNavigation)
			{
				lottiePlayer.InitialNavigation = null;
				lottiePlayer.Initialized = true;
				await lottiePlayer.SetDataAsync(lottiePlayer.Animation);
			}
		}
	}
}