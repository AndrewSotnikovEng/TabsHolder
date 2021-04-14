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
using Installer.ViewModels;
using System.ComponentModel;

namespace Installer
{
    class MainWindowViewModel : ViewModelBase, IDataErrorInfo
    {
        public MainWindowViewModel()
        {
            Builds.Add(new Build("v0.1.0", "url1"));
            Builds.Add(new Build("v1.1", "https://github.com/AndrewSotnikovEng/TabsHolder/releases/download/v1.1/TabsHolder.zip"));
        }


        public void LoadVersions()
        {

        }

        public string DownloadingStageColor { get; set; }
        public string ExtractingStageColor { get; set; }
        public string CleaningStageColor { get; set; }
        public string DoneStageColor { get; set; }

        public string DownloadingStageVisibility { get => downloadingStageVisibility; set
            {
                downloadingStageVisibility = value; 
                OnPropertyChanged("DownloadingStageVisibility");
            }
        }
        public string ExtractingStageVisibility { get => extractingStageVisibility; set
            {
                extractingStageVisibility = value;
                OnPropertyChanged("ExtractingStageVisibility");
            }
        }
        public string CleaningStageVisibility { get => cleaningStageVisibility; set
            {
                cleaningStageVisibility = value;
                OnPropertyChanged("CleaningStageVisibility");
            }
        }
        public string DoneStageVisibility { get => doneStageVisibility; set
            {
                doneStageVisibility = value;
                OnPropertyChanged("DoneStageVisibility");
            }
        }

        public String OutputFolder { get; set; }

        public String OutputPath { get; set; }

        Build selectedItem;
        
        private string downloadingStageVisibility = "Hidden";
        private string extractingStageVisibility = "Hidden";
        private string cleaningStageVisibility = "Hidden";
        private string doneStageVisibility = "Hidden";

        public Build SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
            }
        }



        public void Execute()
        {
            PrepareDestination();
            Download();
            ExtractToDir();
            //Compile();
            RemoveTrash();

        }

        private void PrepareDestination()
        {
            string previousFolder = Path.Combine(OutputFolder, "TabsHolder");
            if (Directory.Exists(previousFolder))
            {
                Directory.Delete(previousFolder, true);
            }  
            
        }

        private void Download()
        {
            OutputPath = OutputFolder + @"\output.zip";
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            WebClient cln = new WebClient();
            cln.DownloadFile(SelectedItem.Url, OutputPath);
            DownloadingStageVisibility = "Visible";
        }

        void ExtractToDir()
        {
            string outputPath = OutputFolder + @"\output.zip";
            ZipFile.ExtractToDirectory(outputPath, OutputFolder);
            ExtractingStageVisibility = "Visible";
        }

        void Compile()
        {
            //string msbuild = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\amd64\MSBuild.exe ";
            string msbuild = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\amd64\MSBuild.exe ";
            string projectNmae = OutputFolder + @"\TabsHolder\TabsHolder\TabsHolder.csproj ";

            Directory.CreateDirectory(OutputFolder + "\\bin ");
            string appFolder = OutputFolder + "\\bin ";
            //string cmd = msbuild + projectNmae + $" p:OutputPath={appFolder}  p:Configuration=Release";

            ProjectCollection pc = new ProjectCollection();
            Dictionary<string, string> GlobalProperty = new Dictionary<string, string>();
            GlobalProperty.Add("Configuration", "Release");
            GlobalProperty.Add("Platform", "Any CPU");
            GlobalProperty.Add("OutputPath", OutputFolder + @"\bin");

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

        void RemoveTrash()
        {
            File.Delete(OutputPath);
            CleaningStageVisibility = "Visible";
            DoneStageVisibility = "Visible";
        }


        public ObservableCollection<Build> Builds { get; set; } = new ObservableCollection<Build>();

        string IDataErrorInfo.Error => throw new NotImplementedException();

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "OutputFolder":
                        if (!Directory.Exists(OutputFolder))
                        {
                            error = "Path not existed!";
                        }
                        break;
                }
                return error;
            }
        }
    }
}
