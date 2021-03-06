﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.IO.Compression;
using System.IO;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Logging;
using Installer.ViewModels;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;
using Installer.Commands;

namespace Installer
{
    class MainWindowViewModel : ViewModelBase, IDataErrorInfo
    {
        public MainWindowViewModel()
        {
            LoadBuilds();
            RunInstallationCmd = new RelayCommand(o => { RunInstallation(); }, RunInstallationCanExecute);
            
        }

        private bool RunInstallationCanExecute(object arg)
        {
            bool result = false;
            if (Directory.Exists(OutputFolder) && SelectedItem != null)
            {
                result = true;
            }

            return result;
        }

        private void LoadBuilds()
        {
            string url = @"https://raw.githubusercontent.com/AndrewSotnikovEng/TabsHolder/master/place_holder.xml";

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            XmlSerializer serializer = new XmlSerializer(typeof(BuildHolder));
            XmlTextReader reader = new XmlTextReader(url);

            BuildHolder b = (BuildHolder)serializer.Deserialize(reader);
            reader.Close();

            foreach (var item in b.Builds)
            {
                Builds.Add(item);
            }

        }


        public string DownloadingStageColor { get; set; }
        public string ExtractingStageColor { get; set; }
        public string CleaningStageColor { get; set; }
        public string DoneStageColor { get; set; }

        public string DownloadingStageVisibility
        {
            get => downloadingStageVisibility; set
            {
                downloadingStageVisibility = value;
                OnPropertyChanged("DownloadingStageVisibility");
            }
        }
        public string ExtractingStageVisibility
        {
            get => extractingStageVisibility; set
            {
                extractingStageVisibility = value;
                OnPropertyChanged("ExtractingStageVisibility");
            }
        }
        public string CleaningStageVisibility
        {
            get => cleaningStageVisibility; set
            {
                cleaningStageVisibility = value;
                OnPropertyChanged("CleaningStageVisibility");
            }
        }
        public string DoneStageVisibility
        {
            get => doneStageVisibility; set
            {
                doneStageVisibility = value;
                OnPropertyChanged("DoneStageVisibility");
            }
        }

        public String OutputFolder { get => outputFolder; set
            {
                outputFolder = value;
                OnPropertyChanged("OutputFolder");
            }
        }

        public String OutputPath { get; set; }

        Build selectedItem;

        private string downloadingStageVisibility = "Hidden";
        private string extractingStageVisibility = "Hidden";
        private string cleaningStageVisibility = "Hidden";
        private string doneStageVisibility = "Hidden";
        private string outputFolder;

        public Build SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
            }
        }



        public void RunInstallation()
        {
            PrepareDestination();
            Download();
            try
            {
                ExtractToDir();
                RemoveTrash();
            }
            catch (IOException e)
            {
                MessengerStatic.NotifyAboutOutputFolderFilled(OutputFolder);
                DownloadingStageVisibility = "Hidden";
                ExtractingStageVisibility = "Hidden";
                CleaningStageVisibility = "Hidden";
                DoneStageVisibility = "Hidden";
    }
            //Compile();

        }

        private void PrepareDestination()
        {
            string previousFolder = Path.Combine(OutputFolder, "TabsHolder");
            if (Directory.Exists(previousFolder))
            {
                Directory.Delete(previousFolder, true);
            }
            string previousZip = Path.Combine(OutputFolder, @"\output.zip");
            if (File.Exists(previousZip))
            {
                File.Delete(previousZip);
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

        public RelayCommand RunInstallationCmd { get; }

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
