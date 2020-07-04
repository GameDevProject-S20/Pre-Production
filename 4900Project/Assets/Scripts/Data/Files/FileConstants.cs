using Google.Apis.Drive.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FileConstants
{
    /// <summary>
    /// File Storage Data. This represents a single file, with its drive file ID, local path, and mime type.
    /// </summary>
    public class FileStorageData
    {
        public string GoogleDriveFileId;
        public string LocalBackupFile;
        public string MimeType;
    }

    /// <summary>
    /// The main Files list. This contains every file that we reference through the game.
    /// </summary>
    public static class Files
    {
        // Dialog CSV
        public static readonly FileStorageData Dialog = new FileStorageData()
        {
            GoogleDriveFileId = "1_7nd016Df8DQaQcoTO5uVkwkHLHL_RIIVejc0AGOE_U",
            LocalBackupFile = $"{Application.dataPath}/BackupData/Dialog.csv",
            MimeType = "text/csv"
        };

        // Items CSV
        public static readonly FileStorageData Items = new FileStorageData()
        {
            GoogleDriveFileId = "1FfBoXktnMt-L44RBr4YOiQFztfTFeJVKOX56EC_FxjE",
            LocalBackupFile = $"{Application.dataPath}/BackupData/Items.csv",
            MimeType = "text/csv"
        };

        // Town CSV
        public static readonly FileStorageData Town = new FileStorageData()
        {
            GoogleDriveFileId = "1elPXxNyDorlGKN-1HzR7sPzJIHzNZAtes3FTixTM94s",
            LocalBackupFile = $"{Application.dataPath}/BackupData/Town.csv",
            MimeType = "text/csv"
        };

        // Tutorial CSV
        public static readonly FileStorageData Tutorial = new FileStorageData()
        {
            GoogleDriveFileId = "1BB_AqHnOiwcTCAZ9qmOwC-Xm_faD2HiLYtrkgmK1LMA",
            LocalBackupFile = $"{Application.dataPath}/BackupData/Tutorial.csv",
            MimeType = "text/csv"
        };

        // Note: For testing purposes only - test CSV
        public static readonly FileStorageData TestFile = new FileStorageData()
        {
            GoogleDriveFileId = "1i_W43TDJLPjL08V7knEnVfbeZonzHhXc5CED3fact8g",
            LocalBackupFile = $"{Application.dataPath}/BackupData/TestFile.csv",
            MimeType = "text/csv"
        };

        // Files list - used for creating backups.
        // Any file added into the list should also be added here
        public static readonly IEnumerable<FileStorageData> FilesList = new List<FileStorageData>()
        {
            Files.Dialog,
            Files.Items,
            Files.Town,
            Files.Tutorial,
            Files.TestFile
        };
    }
}
