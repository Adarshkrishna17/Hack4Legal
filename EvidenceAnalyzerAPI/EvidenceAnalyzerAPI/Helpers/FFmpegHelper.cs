using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace EvidenceAnalyzerAPI.Helpers
{
    public static class FFmpegHelper
    {
        
        public static List<(TimeSpan Timestamp, string FramePath)> ExtractFramesWithTimestamps(
            string videoPath, string outputFolder, int intervalSeconds = 5)
        {
            if (!File.Exists(videoPath))
                throw new FileNotFoundException("Video file not found.", videoPath);

            Directory.CreateDirectory(outputFolder);

            var frames = new List<(TimeSpan, string)>();
            var duration = GetVideoDuration(videoPath);

            for (var t = TimeSpan.Zero; t < duration; t += TimeSpan.FromSeconds(intervalSeconds))
            {
                string timestampFormatted = t.ToString(@"hh\:mm\:ss");
                string framePath = Path.Combine(outputFolder, $"frame_{timestampFormatted.Replace(":", "-")}.jpg");

                
                var args = $"-ss {timestampFormatted} -i \"{videoPath}\" -frames:v 1 \"{framePath}\" -y";
                RunFFmpegCommand(args);

                frames.Add((t, framePath));
            }

            return frames;
        }

        
        public static TimeSpan GetVideoDuration(string videoPath)
        {
            return TimeSpan.FromMinutes(1); 
        }

        private static void RunFFmpegCommand(string arguments)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using var process = Process.Start(psi);
            process?.WaitForExit();
        }
    }
}