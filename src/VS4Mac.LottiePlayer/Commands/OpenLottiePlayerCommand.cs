using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Projects;
using VS4Mac.LottiePlayer.Helpers;
using VS4Mac.LottiePlayer.Views;

namespace VS4Mac.LottiePlayer.Commands
{
	public class OpenLottiePlayerCommand : CommandHandler
    {
        protected override void Run()
        {
			var projectFile = IdeApp.ProjectOperations.CurrentSelectedItem as ProjectFile;

			using (var lottiePlayerDialog = new LottiePlayerDialog(projectFile))
			{
				lottiePlayerDialog.Run(Xwt.MessageDialog.RootWindow);
			}
		}

		protected override void Update(CommandInfo info)
		{
			info.Visible =
				IsWorkspaceOpen()
				&& ProjectHelper.IsProjectReady()
				&& SelectedItemIsJson();
		}

		bool IsWorkspaceOpen()
		{
			return IdeApp.Workspace.IsOpen;
		}

		bool SelectedItemIsJson()
		{
			var projectFile = IdeApp.ProjectOperations.CurrentSelectedItem as ProjectFile;
			return FileHelper.IsJsonFile(projectFile);
		}
	}
}