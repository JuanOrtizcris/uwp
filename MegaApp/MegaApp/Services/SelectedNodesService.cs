﻿using System;
using System.Collections.Generic;
using MegaApp.Enums;
using MegaApp.Interfaces;
using MegaApp.ViewModels;
using MegaApp.ViewModels.SharedFolders;

namespace MegaApp.Services
{
    public static class SelectedNodesService
    {
        #region Events

        /// <summary>
        /// Event triggered when the selected nodes have changed.
        /// </summary>
        public static event EventHandler SelectedNodesChanged;

        #endregion

        private static List<IBaseNode> _selectedNodes;
        public static List<IBaseNode> SelectedNodes
        {
            get
            {
                if (_selectedNodes != null) return _selectedNodes;
                _selectedNodes = new List<IBaseNode>();
                return _selectedNodes;
            }

            set
            {
                _selectedNodes = value;
                SelectedNodesChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public static bool IsMoveAllowed => SelectedNodes.Count > 0 && 
            SelectedNodes[0] is IncomingSharedFolderNodeViewModel;

        public static bool IsSourceFileLink => SelectedNodes?.Count == 1 && 
            (SelectedNodes[0] as NodeViewModel)?.ParentContainerType == ContainerType.FileLink;

        public static bool IsSourceFolderLink => SelectedNodes?.Count > 0 &&
            (SelectedNodes[0] as NodeViewModel)?.ParentContainerType == ContainerType.FolderLink;

        public static bool IsSourcePublicLink => IsSourceFileLink || IsSourceFolderLink;

        private static FolderViewModel _cloudDrive;
        public static FolderViewModel CloudDrive
        {
            get
            {
                if (_cloudDrive != null) return _cloudDrive;

                _cloudDrive = new FolderViewModel(SdkService.MegaSdk, ContainerType.CloudDrive, true);
                _cloudDrive.FolderRootNode =
                    NodeService.CreateNew(SdkService.MegaSdk, App.AppInformation,
                    SdkService.MegaSdk.getRootNode(), _cloudDrive);

                if (App.GlobalListener != null)
                {
                    App.GlobalListener.NodeAdded += _cloudDrive.OnNodeAdded;
                    App.GlobalListener.NodeRemoved += _cloudDrive.OnNodeRemoved;
                    App.GlobalListener.OutSharedFolderAdded += _cloudDrive.OnOutSharedFolderUpdated;
                    App.GlobalListener.OutSharedFolderRemoved += _cloudDrive.OnOutSharedFolderUpdated;
                }

                _cloudDrive.LoadChildNodes();

                return _cloudDrive;
            }
        }

        private static IncomingSharesViewModel _incomingShares;
        public static IncomingSharesViewModel IncomingShares
        {
            get
            {
                if (_incomingShares != null) return _incomingShares;

                _incomingShares = new IncomingSharesViewModel(true);
                _incomingShares.Initialize();

                return _incomingShares;
            }
        }

        public static void ClearSelectedNodes()
        {
            foreach (var node in SelectedNodes)
                if (node != null) node.DisplayMode = NodeDisplayMode.Normal;

            SelectedNodes.Clear();
        }

        /// <summary>
        /// Check if a node is in the selected nodes group for move, copy, import or any other action.
        /// </summary>        
        /// <param name="node">Node to check if is in the selected node list</param>
        /// <param name="setDisplayMode">Indicates if is needed to set the display mode of the node</param>
        /// <returns>True if is a selected node or false in other case</returns>
        public static bool IsSelectedNode(IBaseNode node, bool setDisplayMode = false)
        {
            if (!(SelectedNodes?.Count > 0)) return false;

            var count = SelectedNodes.Count;
            for (int index = 0; index < count; index++)
            {
                var selectedNode = SelectedNodes[index];
                if (node.Base64Handle == selectedNode.Base64Handle)
                {
                    if (setDisplayMode)
                    {
                        //Update the selected nodes list values
                        node.DisplayMode = NodeDisplayMode.SelectedNode;
                        SelectedNodes[index] = node;
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
