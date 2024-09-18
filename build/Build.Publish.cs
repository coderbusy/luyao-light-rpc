using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//[GitHubActions("", GitHubActionsImage.WindowsServer2022, ImportSecrets = new[] { nameof(NuGetApiKey) })]
partial class Build
{
    [Parameter("Api key to use when pushing the package"), Secret]
    readonly string NuGetApiKey;

    [Parameter("NuGet artifact target uri - Defaults to https://api.nuget.org/v3/index.json")]
    readonly string PackageSource = "https://api.nuget.org/v3/index.json";

    [GitVersion(NoFetch = true, Framework = "net8.0")] readonly GitVersion GitVersion;

    Target Pack => _ => _
        .DependsOn(Clean, Compile)
        .Produces(PackageDirectory / "*nupkg")
        .Executes(() =>
        {
            DotNetTasks.DotNetPack(s => s
                .SetProject(Solution)
                .SetTreatWarningsAsErrors(true)
                .EnableNoBuild()
                .SetConfiguration(Configuration)
                .SetVersion(GitVersion.NuGetVersion)
                .SetOutputDirectory(PackageDirectory));
        });


    Target Push => _ => _
        .Consumes(Pack)
        .DependsOn(Pack)
        .Requires(() => NugetApiKey)
        .Requires(() => PackageSource)
        .Executes(() =>
        {
            DotNetTasks.DotNetNuGetPush(s => s
                .SetTargetPath(PackageDirectory / $"*.nupkg")
                .SetApiKey(NugetApiKey)
                .SetSource(PackageSource));
        });
}
