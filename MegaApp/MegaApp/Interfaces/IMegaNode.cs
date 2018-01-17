﻿using System.Collections.ObjectModel;
using System.Threading.Tasks;
using mega;
using MegaApp.Classes;
using MegaApp.Enums;
using MegaApp.ViewModels;

namespace MegaApp.Interfaces
{
    /// <summary>
    /// Signature for MegaNode models in the MegaApp
    /// </summary>
    public interface IMegaNode : IBaseNode
    {
        #region Public Methods

        /// <summary>
        /// Rename the current Node
        /// </summary>
        /// <returns>Result of the action</returns>
        Task RenameAsync();

        /// <summary>
        /// Move the node from its current location to a new folder destination
        /// </summary>
        /// <param name="newParentNode">The root node of the destination folder</param>
        /// <returns>Result of the action</returns>
        Task<NodeActionResult> MoveAsync(IMegaNode newParentNode);

        /// <summary>
        /// Copy the node from its current location to a new folder destination
        /// </summary>
        /// <param name="newParentNode">The root node of the destination folder</param>
        /// <returns>Result of the action</returns>
        Task<NodeActionResult> CopyAsync(IMegaNode newParentNode);

        /// <summary>
        /// Import the node from its current location to a new folder destination
        /// </summary>
        /// <param name="newParentNode">The root node of the destination folder</param>
        /// <returns>Result of the action</returns>
        Task<NodeActionResult> ImportAsync(IMegaNode newParentNode);

        /// <summary>
        /// Delete the node permanently
        /// </summary>
        /// <param name="isMultiSelect">True if the node is in a multi-select scenario</param>
        /// <returns>Result of the action</returns>
        Task<bool> RemoveAsync(bool isMultiSelect = false);

        /// <summary>
        /// Get the node link from the Mega SDK to share the node with others 
        /// </summary>
        /// <param name="showLinkDialog">True to show a dialog with the link and options</param>
        void GetLinkAsync(bool showLinkDialog = true);

        /// <summary>
        /// Remove node link from the Mega SDK
        /// </summary>
        void RemoveLink();

        /// <summary>
        /// Dowload the node to a specified download destionation
        /// </summary>
        /// <param name="transferQueue">Global app transfer queue to add the download to</param>        
        void Download(TransferQueue transferQueue);

        /// <summary>
        /// Update core data associated with the SDK MNode object
        /// </summary>
        /// <param name="megaNode">Node to update</param>
        /// <param name="externalUpdate">Indicates if is an update external to the app. For example from an `onNodesUpdate`</param>
        void Update(MNode megaNode, bool externalUpdate = false);

        /// <summary>
        /// Load node thumbnail if available on disk. If not availble download it with the Mega SDK
        /// </summary>
        void SetThumbnailImage();

        /// <summary>
        /// Open the file that is represented by this node
        /// </summary>
        void Open();

        #endregion

        #region Properties

        /// <summary>
        /// Unique identifier of the node
        /// </summary>
        ulong Handle { get; set; }

        ObservableCollection<IMegaNode> ParentCollection { get; set; }

        ObservableCollection<IMegaNode> ChildCollection { get; set; }

        /// <summary>
        /// Specifies the node type TYPE_UNKNOWN = -1, TYPE_FILE = 0, TYPE_FOLDER = 1, TYPE_ROOT = 2, TYPE_INCOMING = 3, 
        /// TYPE_RUBBISH = 4, TYPE_MAIL = 5
        /// </summary>
        MNodeType Type { get; }

        /// <summary>
        /// Indicates how the node should be drawn on the screen
        /// </summary>
        NodeDisplayMode DisplayMode { get; set; }

        /// <summary>
        /// The TransferObjectModel that controls upload and download transfers of this node
        /// </summary>
        TransferObjectModel Transfer { get; set; }

        /// <summary>
        /// The original MNode from the Mega SDK that is the base for all app nodes 
        /// and used in as input/output in different SDK methods and functions
        /// </summary>
        MNode OriginalMNode { get; }

        /// <summary>
        /// Access level to the node
        /// </summary>
        AccessLevelViewModel AccessLevel { get; set; }

        /// <summary>
        /// Specifies if the node has read permissions
        /// </summary>
        bool HasReadPermissions { get; }

        /// <summary>
        /// Specifies if the node has read & write permissions
        /// </summary>
        bool HasReadWritePermissions { get; }

        /// <summary>
        /// Specifies if the node has full access permissions
        /// </summary>
        bool HasFullAccessPermissions { get; }

        #endregion
    }
}
