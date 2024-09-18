﻿using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;

[GitHubActions("Publish NuGet",
    GitHubActionsImage.WindowsServer2022,
    OnPushTags = new[] { "v*" },
    PublishArtifacts = true,
    ImportSecrets = new[] { nameof(NuGetApiKey)
})]
partial class Build
{
    [Parameter("Api key to use when pushing the package"), Secret]
    readonly string NuGetApiKey = string.Empty;

    [Parameter("NuGet artifact target uri - Defaults to https://api.nuget.org/v3/index.json")]
    readonly string PackageSource = "https://api.nuget.org/v3/index.json";

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
        .Requires(() => NuGetApiKey)
        .Requires(() => PackageSource)
        .Executes(() =>
        {
            DotNetTasks.DotNetNuGetPush(s => s
                .SetTargetPath(PackageDirectory / $"*.nupkg")
                .SetApiKey(NuGetApiKey)
                .SetSource(PackageSource));
        });
}
