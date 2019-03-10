using System.IO;
using System.Threading.Tasks;
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
		Slider _timeSlider;
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

			_controlBox = new HBox
			{
				Margin = new WidgetSpacing(0, 6, 0, 6)
			};

			_playButton = new Button
			{
				BackgroundColor = MonoDevelop.Ide.Gui.Styles.BackgroundColor,
				Image = ImageService.GetIcon("lottie-pause", Gtk.IconSize.Button),
				ImagePosition = ContentPosition.Center,
				Style = ButtonStyle.Borderless
			};

			_timeSlider = new HSlider
			{
				MinimumValue = 0,
				Visible = false
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

			_controlBox.PackStart(_playButton);
			_controlBox.PackStart(_timeSlider, true);

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
			_lottiePlayer.DurationChanged += OnLottiePlayerDurationChanged;
			_timeSlider.ValueChanged += OnTimeSliderValueChanged;
			_playButton.Clicked += OnPlayButtonClicked;  
			_closeButton.Clicked += OnCloseButtonClicked;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			_lottiePlayer.DurationChanged -= OnLottiePlayerDurationChanged;
			_timeSlider.ValueChanged -= OnTimeSliderValueChanged;
			_playButton.Clicked -= OnPlayButtonClicked;
			_closeButton.Clicked -= OnCloseButtonClicked;
		}

		public async void SetController(IController controller)
		{
			_controller = (LottiePlayerController)controller;

			await LoadAnimationAsync(); 
		}

		async Task LoadAnimationAsync()
		{
			var animationText = File.ReadAllText(_controller.ProjectFile.FilePath);

			await _lottiePlayer.SetDataAsync(animationText);
		}

		void UpdateSliderData()
		{
			_timeSlider.MaximumValue = _lottiePlayer.Duration;
			_timeSlider.StepIncrement = 0.1;
		}

		void OnLottiePlayerDurationChanged(object sender, System.EventArgs e)
		{
			UpdateSliderData();
		}

		void OnTimeSliderValueChanged(object sender, System.EventArgs e)
		{
			var timeValue = _timeSlider.Value;
			_lottiePlayer.GoToAndStop(timeValue);
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
			_timeSlider.Visible = false;
			_lottiePlayer.Play();
		}

		void Play()
		{
			_playButton.Image = ImageService.GetIcon("lottie-play", Gtk.IconSize.Button);
			_timeSlider.Visible = true;
			_lottiePlayer.Pause();
		}

		void OnCloseButtonClicked(object sender, System.EventArgs e)
		{
			Respond(Command.Close);
			Close();
		}
	}
}