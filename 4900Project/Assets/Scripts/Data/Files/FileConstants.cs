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
            LocalBackupFile = $"{Application.streamingAssetsPath}/BackupData/Dialog.csv",
            MimeType = "text/csv"
        };

        // Items CSV
        public static readonly FileStorageData Items = new FileStorageData()
        {
            GoogleDriveFileId = "1zT5RKm-cFOMkGjg3OQwewlrfC3SuKmbwk1d7Ei1juuA",
            LocalBackupFile = $"{Application.streamingAssetsPath}/BackupData/Items.csv",
            MimeType = "text/csv"
        };

        // Shops CSV
        public static readonly FileStorageData Shops = new FileStorageData()
        {
            GoogleDriveFileId = "1mKTfUI2aR6ihpYpN-9QE1dGP1293P4Wch3oADbjEiOk",
            LocalBackupFile = $"{Application.streamingAssetsPath}/BackupData/Shops.csv",
            MimeType = "text/csv"
        };

        // Town CSV
        public static readonly FileStorageData Town = new FileStorageData()
        {
            GoogleDriveFileId = "1elPXxNyDorlGKN-1HzR7sPzJIHzNZAtes3FTixTM94s",
            LocalBackupFile = $"{Application.streamingAssetsPath}/BackupData/Town.csv",
            MimeType = "text/csv"
        };

        // Tutorial CSV
        public static readonly FileStorageData Tutorial = new FileStorageData()
        {
            GoogleDriveFileId = "1BB_AqHnOiwcTCAZ9qmOwC-Xm_faD2HiLYtrkgmK1LMA",
            LocalBackupFile = $"{Application.streamingAssetsPath}/BackupData/Tutorial.csv",
            MimeType = "text/csv"
        };

        // json list of strings
        public static readonly FileStorageData MapNodes = new FileStorageData()
        {
            GoogleDriveFileId = "1b1C8UP12nCt-iiDYuMc3kQePnF-iLi_7bjDVvu7_2mA",
            LocalBackupFile = $"{Application.streamingAssetsPath}/BackupData/MapNodes.csv",
            MimeType = "text/csv"
        };

        // json list of classes
        public static readonly FileStorageData MapEdges = new FileStorageData()
        {
            GoogleDriveFileId = "1AENRNM6BTT52Vu0uAsx8bfJUlDVi6LDNIlzMLOAQZnM",
            LocalBackupFile = $"{Application.streamingAssetsPath}/BackupData/MapEdges.csv",
            MimeType = "text/csv"
        };

        // Note: For testing purposes only - test CSV
        public static readonly FileStorageData TestCsv = new FileStorageData()
        {
            GoogleDriveFileId = "1i_W43TDJLPjL08V7knEnVfbeZonzHhXc5CED3fact8g",
            LocalBackupFile = $"{Application.streamingAssetsPath}/BackupData/Tests/TestFile.csv",
            MimeType = "text/csv"
        };

        // JSON Tests
        // basic json
        public static readonly FileStorageData TestBasicJson = new FileStorageData()
        {
            GoogleDriveFileId = "10gaDHRTOs9TX3XkQQpcj5jYK_jxTGeSw",
            LocalBackupFile = $"{Application.streamingAssetsPath}/BackupData/Tests/BasicJson.json",
            MimeType = "application/json"
        };

        // json list of strings
        public static readonly FileStorageData TestJsonStringsList = new FileStorageData()
        {
            GoogleDriveFileId = "1FJSW9lSejV3_TO0tHCc951GVdbN1YnYS",
            LocalBackupFile = $"{Application.streamingAssetsPath}/BackupData/Tests/StringList.json",
            MimeType = "application/json"
        };

        // json list of classes
        public static readonly FileStorageData TestJsonListOfClass = new FileStorageData()
        {
            GoogleDriveFileId = "1YMCwaA60aDROG_ipxJjl8GF4c-mpQwMl",
            LocalBackupFile = $"{Application.streamingAssetsPath}/BackupData/Tests/ListOfClass.json",
            MimeType = "application/json"
        };

        // Files list - used for creating backups.
        // Any file added into the list should also be added here
        public static readonly IEnumerable<FileStorageData> FilesList = new List<FileStorageData>()
        {
            Files.Dialog,
            Files.Items,
            Files.Shops,
            Files.Town,
            Files.Tutorial,
            Files.TestCsv,
            Files.TestBasicJson,
            Files.TestJsonStringsList,
            Files.TestJsonListOfClass,
            Files.MapNodes,
            Files.MapEdges

        };
    }
}
