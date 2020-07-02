using System.Collections.Generic;
using CsvHelper;
using System.IO;
using System.Globalization;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System.Diagnostics;
using System;
using UnityEngine.SocialPlatforms;
using System.CodeDom;
using FileConstants;

public class GameData
{
    /// <summary>
    /// The Google Drive service, allowing us to work with files stored in Drive.
    /// </summary>
    private static DriveService googleDrive = new DriveService(new BaseClientService.Initializer()
    {
        ApiKey = "AIzaSyCJEz0TOL3vGQr6I-MBtRlKFnpZn7OS6mg"
    });

    /// <summary>
    /// Loads a CSV by first attempting to load it from Google Drive, defaulting to a local file if the Drive request fails.
    /// </summary>
    /// <typeparam name="T">The type of object to load in. The field keys should match the headers of the CSV file.</typeparam>
    /// <param name="fileData">The FileStorageData representing the file to be loaded.</param>
    /// <param name="data">The resultant data loaded from the CSV.</param>
    public static void LoadCsv<T>(FileStorageData fileData, out IEnumerable<T> data)
    {
        // Retrieve the stream for fetching our data
        GetFileStream(fileData.GoogleDriveFileId, fileData.LocalBackupFile, "text/csv", out Stream dataStream);

        // Read the contents of the stream into T objects
        using (var reader = new StreamReader(dataStream))
        {
            FillOutCsvRecords<T>(reader, out data);
        }

        // Cleanup
        dataStream.Dispose();
    }
    
    /// <summary>
    /// Runs through the list of files to create each backup.
    /// </summary>
    public static void CreateBackups()
    {
        // Go through the list one-by-one to create the backup
        foreach (var file in Files.FilesList)
        {
            CreateBackupFile(file.GoogleDriveFileId, file.LocalBackupFile, file.MimeType);
        }
    }
    
    /// <summary>
    /// Creates a local backup of a file stored on Google Drive.
    /// </summary>
    /// <param name="googleDriveFileId">The file ID to create the backup from</param>
    /// <param name="localFilePath">The location on disk to store the backup</param>
    /// <param name="mimeType">The mime type of the file content. Defaults to text/csv.</param>
    protected static bool CreateBackupFile(string googleDriveFileId, string localFilePath, string mimeType = "text/csv")
    {
        // Attempt to download the file from Google Drive
        var canAccessGoogleDrive = DownloadFileFromGoogleDrive(googleDriveFileId, mimeType, out Stream dataStream);
        if (!canAccessGoogleDrive)
        {
            return false;
        }

        // At this point, the file data will be stored inside dataStream, so write that out to our file
        using (var writeStream = File.OpenWrite(localFilePath))
        {
            dataStream.CopyTo(writeStream);
        }

        // Clean up the data stream
        dataStream.Dispose();
        return true;
    }

    /// <summary>
    /// Retrieves a file stream, attempting to download the file from Google Drive and defaulting to a local backup if Drive fails.
    /// </summary>
    /// <param name="googleDriveFileId">The file id to download from Google Drive</param>
    /// <param name="localFilePath">The path to the same file, stored locally</param>
    /// <param name="mimeType">The mime type of the file to download</param>
    /// <param name="dataStream">The resultant data stream</param>
    protected static void GetFileStream(string googleDriveFileId, string localFilePath, string mimeType, out Stream dataStream)
    {
        // Attempt to download the file from Google Drive first, to get our most recent copy
        var canDownloadDrive = DownloadFileFromGoogleDrive(googleDriveFileId, mimeType, out dataStream);
        if (canDownloadDrive)
        {
            return;
        }

        // If that fails, default to our local file
        dataStream = File.OpenRead(localFilePath);
    }

    /// <summary>
    /// Reads CSV records, returning a list of the records stored in a given structure.
    /// </summary>
    /// <typeparam name="T">The type of object to load in. Note that the field keys should match the headers of the CSV.</typeparam>
    /// <param name="reader">The reader stream</param>
    /// <param name="resultData">The resultant data, where each record is one single row from the csv</param>
    protected static void FillOutCsvRecords<T>(TextReader reader, out IEnumerable<T> resultData)
    {
        // Creating a new List object here to store all the data
        var outputList = new List<T>();

        // Use the CsvReader library to read records
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            // Note: This all gets lazy loaded, so we need to add it in as we go through;
            // So go through every result & add it to our list one-by-one
            var results = csv.GetRecords<T>();
            foreach (var result in results)
            {
                outputList.Add(result);
            }
        }

        // Last step is to set up the output to point to our result data
        resultData = outputList;
    }


    /// <summary>
    /// Downloads a file that has been stored in Google Drive.
    /// </summary>
    /// <param name="fileId">The ID of the file to download</param>
    /// <param name="mimeType">The mime type to expect for the file</param>
    /// <param name="result">The result stream where data will be saved to</param>
    protected static bool DownloadFileFromGoogleDrive(string fileId, string mimeType, out Stream result)
    {
        result = new MemoryStream();

        // Download the file into our result stream
        var export = googleDrive.Files.Export(fileId, mimeType);
        var response = export.DownloadWithStatus(result);

        // Note: Once the data is written to the stream, the stream position will be set to the end
        // We want to reset this to the start so that data can be read
        result.Seek(0, SeekOrigin.Begin);

        // Return whether or not the request worked
        return response.Status != Google.Apis.Download.DownloadStatus.Failed;
    }
}
