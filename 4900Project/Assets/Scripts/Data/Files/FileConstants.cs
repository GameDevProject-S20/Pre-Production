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
            LocalBackupFile = $"{Application.dataPath}/BackupData/Dialog.csv"
        };

        // Items CSV
        public static readonly FileStorageData Items = new FileStorageData()
        {
            GoogleDriveFileId = "1FfBoXktnMt-L44RBr4YOiQFztfTFeJVKOX56EC_FxjE",
            LocalBackupFile = $"{Application.dataPath}/BackupData/Items.csv"
        };

        // Town CSV
        public static readonly FileStorageData Town = new FileStorageData()
        {
            GoogleDriveFileId = "1elPXxNyDorlGKN-1HzR7sPzJIHzNZAtes3FTixTM94s",
            LocalBackupFile = $"{Application.dataPath}/BackupData/Town.csv"
        };

        // Tutorial CSV
        public static readonly FileStorageData Tutorial = new FileStorageData()
        {
            GoogleDriveFileId = "1BB_AqHnOiwcTCAZ9qmOwC-Xm_faD2HiLYtrkgmK1LMA",
            LocalBackupFile = $"{Application.dataPath}/BackupData/Tutorial.csv"
        };

        // Note: For testing purposes only - test CSV
        public static readonly FileStorageData TestCsv = new FileStorageData()
        {
            GoogleDriveFileId = "1i_W43TDJLPjL08V7knEnVfbeZonzHhXc5CED3fact8g",
            LocalBackupFile = $"{Application.dataPath}/BackupData/Tests/TestFile.csv"
        };

        // JSON Tests
        // basic json
        public static readonly FileStorageData TestBasicJson = new FileStorageData()
        {
            GoogleDriveFileId = "10gaDHRTOs9TX3XkQQpcj5jYK_jxTGeSw",
            LocalBackupFile = $"{Application.dataPath}/BackupData/Tests/BasicJson.json"
        };

        // json list of strings
        public static readonly FileStorageData TestJsonStringsList = new FileStorageData()
        {
            GoogleDriveFileId = "1FJSW9lSejV3_TO0tHCc951GVdbN1YnYS",
            LocalBackupFile = $"{Application.dataPath}/BackupData/Tests/StringList.json"
        };

        // json list of classes
        public static readonly FileStorageData TestJsonListOfClass = new FileStorageData()
        {
            GoogleDriveFileId = "1YMCwaA60aDROG_ipxJjl8GF4c-mpQwMl",
            LocalBackupFile = $"{Application.dataPath}/BackupData/Tests/ListOfClass.json"
        };

        // Files list - used for creating backups.
        // Any file added into the list should also be added here
        public static readonly IEnumerable<FileStorageData> FilesList = new List<FileStorageData>()
        {
            Files.Dialog,
            Files.Items,
            Files.Town,
            Files.Tutorial,
            Files.TestCsv,
            Files.TestBasicJson,
            Files.TestJsonStringsList,
            Files.TestJsonListOfClass
        };
    }
}
