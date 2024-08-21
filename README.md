# TaggableMediaExplorer

Project to recreate what used to be a standalone WPF app into two components and possibly crossplatform in future.

<h3>Features</h3>

<b>Server side:</b> Asset analysis, Relocation detection, Media analysis, Integrity check, Authentication (TODO).

<b>Client side:</b> Video playback, Tagging, Remote login (TODO), Crossplatform (TODO).

<h3>Tech specs:</h3>

<b>Backend:</b> <i>ASP.NET Core Web API, with a dedicated housekeeping service to sync file system changes.</i>

<b>Frontend:</b> <i>One or many options out of WPF/Avalonia/MAUI/Blazor/Razor/React UI frameworks.</i>

<b>Libraries:</b> <i>FFMPEG for server side media processing.</i>

<b>Streaming and Playing:</b> <i>LibVlc for binaries. Undecided for web apps.</i>

<h3>Roadmap</h3>

<b>Alpha-</b>

Add Features
Concern separations (Current)
Upgrade to XenoFx (Current)

<b>Beta-</b>

Optimize backend.
Housekeeping.

<b>1.0+</b>

Add frontends for more platforms.
