using System.Diagnostics;

namespace VideoAnalyzerApp
{
    public static class FFmpegHelper
    {
        public static void ExtractAudio(string videoPath, string outputPath)
        {
            #pragma warning disable CS8602 // Dereference of a possibly null reference.
            Process.Start(new ProcessStartInfo
            {
                FileName = $"ffmpeg",
                Arguments = $"-y -i \"{videoPath}\" -vn -acodec pcm_s16le -ar 16000 -ac 1 \"{outputPath}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            }).WaitForExit();
            #pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
        public static void ExtractFramesWithFFmpeg(string inputVideoPath, string outputDir, int fps = 1)
        {
            string outputPattern = Path.Combine(outputDir, "frame_%03d.jpg");
            string arguments = $"-i \"{inputVideoPath}\" -vf fps={fps} \"{outputPattern}\"";

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = $"ffmpeg",
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.OutputDataReceived += (sender, e) => { if (e.Data != null) Console.WriteLine(e.Data); };
            process.ErrorDataReceived += (sender, e) => { if (e.Data != null) Console.WriteLine(e.Data); };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }
    }
}