using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using System.Diagnostics;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Logging;

namespace Installer
{
    class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            Builds.Add(new Build("v0.1.0", "url1"));
            Builds.Add(new Build("v1.1", "https://github.com/AndrewSotnikovEng/TabsHolder/archive/refs/tags/v1.1.zip"));
        }


        public void LoadVersions()
        {

        }
        public String OutputFolder { get; set; }

        Build selectedItem;
        public Build  SelectedItem { get { return selectedItem; }
            set
            {
                selectedItem = value;
            }
        }
                
        

        public void Execute()
        {
            //Download();
            //ExtractToDir();
            Compile();

        }

        private void Download()
        {
            string outputPath = OutputFolder + @"\output.zip";
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            WebClient cln = new WebClient();
            cln.DownloadFile(SelectedItem.Url, outputPath);
        }

        void ExtractToDir()
        {
            string outputPath = OutputFolder + @"\output.zip";
            ZipFile.ExtractToDirectory(outputPath, OutputFolder);
        }

        void Compile()
        {
            //string msbuild = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\amd64\MSBuild.exe ";
            string msbuild = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\amd64\MSBuild.exe ";
            string projectNmae = OutputFolder + @"\TabsHolder-1.1\TabsHolder\TabsHolder.csproj ";

            Directory.CreateDirectory(OutputFolder + "\\bin ");
            string appFolder = OutputFolder + "\\bin ";
            //string cmd = msbuild + projectNmae + $" p:OutputPath={appFolder}  p:Configuration=Release";

            ProjectCollection pc = new ProjectCollection();
            Dictionary<string, string> GlobalProperty = new Dictionary<string, string>();
            GlobalProperty.Add("Configuration", "Release");
            GlobalProperty.Add("Platform", "Any CPU");
            GlobalProperty.Add("OutputPath", Directory.GetCurrentDirectory() + "\\build\\\bin\\Release");

            BuildParameters bp = new BuildParameters(pc);
            bp.Loggers = new[] {
              new FileLogger
              {
                Verbosity = LoggerVerbosity.Detailed,
                ShowSummary = true,
                SkipProjectStartedText = true
              }
            };

            BuildManager.DefaultBuildManager.BeginBuild(bp);

            BuildRequestData BuildRequest = new BuildRequestData(projectNmae, GlobalProperty, null, new string[] { "Build" }, null);

            BuildSubmission BuildSubmission = BuildManager.DefaultBuildManager.PendBuildRequest(BuildRequest);
            BuildSubmission.Execute();
            BuildManager.DefaultBuildManager.EndBuild();
            if (BuildSubmission.BuildResult.OverallResult == BuildResultCode.Failure)
            {
                throw new Exception();
            }
        }

    
            
            

    


        public ObservableCollection<Build> Builds { get; set; } = new ObservableCollection<Build>();
    }
}
