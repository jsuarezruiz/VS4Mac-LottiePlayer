using System.IO;
using MonoDevelop.Projects;
using Xwt;

namespace VS4Mac.LottiePlayer.Views
{
	public class LottiePlayerDialog : Dialog
	{
		VBox _mainBox;
		Controls.LottiePlayer _lottiePlayer;
		HBox _buttonBox;
		Button _closeButton;

		public LottiePlayerDialog(ProjectFile projectFile)
		{
			Init();
			BuildGui();
			AttachEvents();

			LoadAnimation(projectFile);
		}

		void Init()
		{
			_mainBox = new VBox();
			_lottiePlayer = new Controls.LottiePlayer();
			_buttonBox = new HBox();
			_closeButton = new Button("Close");
		}

		void BuildGui()
		{
			Title = "Lottie Player";

			Height = 500;
			Width = 500;

			var xwtLottiePlayer = Toolkit.CurrentEngine.WrapWidget(_lottiePlayer);

			_buttonBox.PackEnd(_closeButton);

			_mainBox.PackStart(xwtLottiePlayer, true);
			_mainBox.PackEnd(_buttonBox);

			_mainBox.Show();

			Content = _mainBox;
			Resizable = false;
		}

		void AttachEvents()
		{
			_closeButton.Clicked += OnCloseButtonClicked;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			_closeButton.Clicked -= OnCloseButtonClicked;
		}

		void LoadAnimation(ProjectFile projectFile)
		{
			var animationText = File.ReadAllText(projectFile.FilePath);

			_lottiePlayer.SetData(animationText);
		}

		void OnCloseButtonClicked(object sender, System.EventArgs e)
		{
			Respond(Command.Close);
			Close();
		}
	}
}