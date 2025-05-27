using ADotNet.Clients;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks.SetupDotNetTaskV1s;

var aDotNetClient = new ADotNetClient();

var githubPipeline = new GithubPipeline
{
    Name = "JobPortal Build Pipeline",

    OnEvents = new Events
    {
        PullRequest = new PullRequestEvent
        {
            Branches = new[] { "main" }
        },
        Push = new PushEvent
        {
            Branches = new[] { "main" }
        }
    },

    Jobs = new Dictionary<string, Job>
    {
        {
            "Build",
            new Job
            {
                RunsOn = BuildMachines.Windows2022,

                Steps = new List<GithubTask>
                {
                    new CheckoutTaskV2
                    {
                        Name = "Checking Out Code"
                    },
                    new SetupDotNetTaskV1
                    {
                        Name = "Setting Up .NET",
                        TargetDotNetVersion = new TargetDotNetVersion
                        {
                            DotNetVersion = "8.0.x"
                        }
                    },
                    new RestoreTask
                    {
                        Name = "Restoring Packages"
                    },
                    new DotNetBuildTask
                    {
                        Name = "Building Project"
                    },
                    new TestTask
                    {
                        Name = "Running Tests"
                    }
                }
            }
        }
    }
};

string buildScriptPath = Path.Combine(Directory
    .GetCurrentDirectory(), "..", "..", "..", ".github", "workflows", "dotnet.yml");

string? directoryPath = Path.GetDirectoryName(buildScriptPath);

if (!Directory.Exists(directoryPath))
{
    Directory.CreateDirectory(directoryPath!);
}

aDotNetClient.SerializeAndWriteToFile(githubPipeline, path: buildScriptPath);
