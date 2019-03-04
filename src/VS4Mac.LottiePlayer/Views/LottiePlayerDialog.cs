using System.IO;
using MonoDevelop.Ide;
using VS4Mac.LottiePlayer.Controllers;
using VS4Mac.LottiePlayer.Controllers.Base;
using VS4Mac.LottiePlayer.Views.Base;
using Xwt;

namespace VS4Mac.LottiePlayer.Views
{
	public interface ILottiePlayerView : IView
	{

	}

	public class LottiePlayerDialog : Dialog, ILottiePlayerView
	{
		VBox _mainBox;
		Controls.LottiePlayer _lottiePlayer;
		HBox _controlBox;
		Button _playButton;
		HBox _buttonBox;
		Button _closeButton;

		LottiePlayerController _controller;

		public LottiePlayerDialog()
		{
			Init();
			BuildGui();
			AttachEvents();
		}

		void Init()
		{
			_mainBox = new VBox
			{
				BackgroundColor = MonoDevelop.Ide.Gui.Styles.BackgroundColor,
				Margin = new WidgetSpacing(0, 0, 0, 0)
			};

			_lottiePlayer = new Controls.LottiePlayer();
			_controlBox = new HBox();

			_playButton = new Button
			{
				BackgroundColor = MonoDevelop.Ide.Gui.Styles.BackgroundColor,
				HorizontalPlacement = WidgetPlacement.Center,
				Image = ImageService.GetIcon("lottie-pause", Gtk.IconSize.Button),
				ImagePosition = ContentPosition.Center,
				Style = ButtonStyle.Borderless
			};

			_buttonBox = new HBox();
			_closeButton = new Button("Close");
		}

		void BuildGui()
		{
			Title = "Lottie Player";

			Height = 500;
			Width = 500;

			var xwtLottiePlayer = Toolkit.CurrentEngine.WrapWidget(_lottiePlayer);

			_controlBox.PackStart(_playButton, true);

			_buttonBox.PackEnd(_closeButton);

			_mainBox.PackStart(xwtLottiePlayer, true);
			_mainBox.PackStart(_controlBox); 
			_mainBox.PackEnd(_buttonBox);

			_mainBox.Show();

			Content = _mainBox;
			Resizable = false;
		}

		void AttachEvents()
		{
			_playButton.Clicked += OnPlayButtonClicked;  
			_closeButton.Clicked += OnCloseButtonClicked;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			_playButton.Clicked -= OnPlayButtonClicked;
			_closeButton.Clicked -= OnCloseButtonClicked;
		}

		public void SetController(IController controller)
		{
			_controller = (LottiePlayerController)controller;

			LoadAnimation(); 
		}

		void LoadAnimation()
		{
			var animationText = File.ReadAllText(_controller.ProjectFile.FilePath);

			_lottiePlayer.SetData(animationText);
		}

		void OnPlayButtonClicked(object sender, System.EventArgs e)
		{
			_controller.IsPlaying = !_controller.IsPlaying;

			if (_controller.IsPlaying)
			{
				Pause();
			}
			else
			{
				Play();
			}
		}

		void Pause()
		{
			_playButton.Image = ImageService.GetIcon("lottie-pause", Gtk.IconSize.Button);
			_lottiePlayer.Play();
		}

		void Play()
		{
			_playButton.Image = ImageService.GetIcon("lottie-play", Gtk.IconSize.Button);
			_lottiePlayer.Pause();
		}

		void OnCloseButtonClicked(object sender, System.EventArgs e)
		{
			Respond(Command.Close);
			Close();
		}
	}
}