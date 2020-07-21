using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

namespace UnityUtility
{
    /// <summary>
    /// This class can be used for storing static helper methods.
    /// These are unrelated to any individual system, but can be used to help other systems.
    /// </summary>
    public class UnityHelperMethods
    {
        /// <summary>
        /// Parses a string into an enum, throwing an exception if it fails.
        /// </summary>
        /// <typeparam name="T">The type of enum to parse out to</typeparam>
        /// <param name="str">The string value for the enum</param>
        /// <returns></returns>
        public static T ParseEnum<T>(string str) where T : struct
        {
            var success = Enum.TryParse(str, out T result);
            if (!success)
            {
                throw new Exception($"Unrecognized value for type {typeof(T)}: {success} ");
            }
            return result;
        }

        /// <summary>
        /// Parses a comma-separated list stored as a string into its individual values.
        /// </summary>
        /// <typeparam name="T">The type that we want to parse out to</typeparam>
        /// <param name="valuesString">The list of values to parse</param>
        /// <param name="parserFunc">The function for handling parsing. This will take in a string and should return the parsed value.</param>
        /// <returns></returns>
        public static List<T> ParseCommaSeparatedList<T>(string valuesString, Func<string, T> parserFunc)
        {
            // Split on the comma to get all the values to parse out
            var values = valuesString.Split(',');
            var result = new List<T>(values.Length);

            // Go through one-by-one and parse it into a value
            foreach (var value in values)
            {
                // Parse out the value & add it into our list
                result.Add(parserFunc(value));
            }

            return result;
        }

        /// <summary>
        /// A parser that can be fed into the ParseCommaSeparatedList method above.
        /// Takes in a string, and returns the same string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DefaultStringParser(string value)
        {
            return value;
        }

        /// <summary>
        /// Given a path to an image file, generates a Sprite that we can use of the image.
        /// This allows us to pop some images in through CSVs / through their path / etc., rather than doing it in the Unity editor.
        /// </summary>
        /// <param name="iconPath"></param>
        /// <returns></returns>
        public static Sprite BuildSpriteFromPath(string imagePath)
        {
            var texture = LoadTexture(imagePath);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 100.0f);
        }

        /// <summary>
        /// Builds a texture out of an image. Note that this is protected because it's mainly just used for the Sprites right now,
        /// but we can make it public later on if it fills a need.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected static Texture2D LoadTexture(string path)
        {
            // Load a PNG or JPG file from disk to a Texture2D
            // Returns null if load fails
            Texture2D Tex2d;
            byte[] FileData;

            // Verify that the file exists
            if (File.Exists(path))
            {
                FileData = File.ReadAllBytes(path);
                Tex2d = new Texture2D(2, 2);

                // Try to load it into the Texture object
                // Note that if this returns false, it means there was an error
                //  and the texture couldn't be loaded
                if (Tex2d.LoadImage(FileData))
                {
                    return Tex2d;
                }
            }

            throw new Exception($"Could not read the file specified at {path}");
        }
    }
}
