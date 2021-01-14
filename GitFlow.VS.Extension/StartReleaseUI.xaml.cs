﻿using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;

namespace GitFlowVS.Extension
{
    /// <summary>
    /// Interaction logic for StartReleaseUI.xaml
    /// </summary>
    public partial class StartReleaseUI : UserControl
    {
        private readonly GitFlowSection parent;
        private readonly StartReleaseModel model;
        private IGitRepositoryInfo ActiveRepo { get; set; }
        private IVsOutputWindowPane OutputWindow { get; set; }
        public StartReleaseUI(GitFlowSection parent, IGitRepositoryInfo activeRepo, IVsOutputWindowPane outputWindow)
        {
            model = new StartReleaseModel();
            this.parent = parent;
            ActiveRepo = activeRepo;
            OutputWindow = outputWindow;
            InitializeComponent();

            DataContext = model;
        }

        private void OnCancelRelease(object sender, RoutedEventArgs e)
        {
            parent.CancelAction();
        }

        private void OnCreateRelease(object sender, RoutedEventArgs e)
        {
            if (ActiveRepo != null)
            {
                OutputWindow.Activate();
                using (new WaitCursor())
                {
                    var gf = new VsGitFlowWrapper(ActiveRepo.RepositoryPath, OutputWindow, parent);
                    gf.StartRelease(model.ReleaseName);
                }
                parent.FinishAction();
            }
        }
    }
}
