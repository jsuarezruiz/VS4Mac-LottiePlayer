using System;
using System.Runtime.InteropServices;
using Mono.Addins;
using Mono.Addins.Description;

[assembly: Addin(
    "LottiePlayer",
    Namespace = "MonoDevelop",
    Version = "0.1", 
	Category = "IDE extensions"
)]

[assembly: AddinName("Lottie player")]
[assembly: AddinCategory("IDE extensions")]
[assembly: AddinDescription("Visual Studio for macOS addin to Preview Lottie json files.")]
[assembly: AddinAuthor("Javier Suárez Ruiz")]
[assembly: AddinUrl("https://github.com/jsuarezruiz/VS4Mac-LottiePlayer")]

[assembly: CLSCompliant(false)]
[assembly: ComVisible(false)]