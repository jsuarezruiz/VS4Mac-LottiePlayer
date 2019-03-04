using MonoDevelop.Projects;
using VS4Mac.LottiePlayer.Controllers.Base;
using VS4Mac.LottiePlayer.Views;

namespace VS4Mac.LottiePlayer.Controllers
{
	public class LottiePlayerController : IController
	{
		readonly ILottiePlayerView _view;

		public LottiePlayerController(ILottiePlayerView view, ProjectFile projectFile)
		{
			_view = view;

			ProjectFile = projectFile;
			IsPlaying = true;

			view.SetController(this);
		}

		public ProjectFile ProjectFile { get; set; }
		public bool IsPlaying { get; set; }
	}
}