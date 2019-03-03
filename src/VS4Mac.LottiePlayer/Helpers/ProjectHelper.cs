using MonoDevelop.Ide;

namespace VS4Mac.LottiePlayer.Helpers
{
	public static class ProjectHelper
    {
        public static bool IsProjectReady()
        {
            var isBuilding = IdeApp.ProjectOperations.IsBuilding(IdeApp.ProjectOperations.CurrentSelectedSolution);
            var isRunning = IdeApp.ProjectOperations.IsRunning(IdeApp.ProjectOperations.CurrentSelectedSolution);

            return !isBuilding && !isRunning;
        }
    }
}