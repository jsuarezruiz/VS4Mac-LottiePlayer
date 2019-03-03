using MonoDevelop.Projects;

namespace VS4Mac.LottiePlayer.Helpers
{
	public static class FileHelper 
	{
		internal static readonly string[] ValidJsonExtensions = { "json", "lottie" };

		public static bool IsJsonFile(ProjectFile projectFile)
		{
			if (projectFile == null)
				return false;

			var extension = projectFile.FilePath.Extension;

			foreach (var validJsonExtension in ValidJsonExtensions)
			{
				if (extension.Contains(validJsonExtension))
				{
					return true;
				}
			}

			return false;
		}
	}
}