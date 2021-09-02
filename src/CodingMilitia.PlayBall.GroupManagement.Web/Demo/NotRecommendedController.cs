using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Demo
{
    [Route("tests")]
    public class NotRecommendedController : Controller
    {
        private static readonly ConcurrentDictionary<string, TaskCompletionSource<long>> DownloadedFilesInformation
            = new ConcurrentDictionary<string, TaskCompletionSource<long>>();

        private static readonly Random Random = new Random(1000);
        private static CancellationTokenSource CommonCancellationTokenSource = null;

        private readonly ILogger<NotRecommendedController> _logger;

        public NotRecommendedController(ILogger<NotRecommendedController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("download/{filename}")]
        public async Task<IActionResult> DownloadFileAsync(string filename)
        {
            var filenameLower = filename.ToLower();

            if (DownloadedFilesInformation.ContainsKey(filenameLower))
            {
                return Content($"File download was already requested. Filename: {filename}");
            }

            var tcs = new TaskCompletionSource<long>();
            DownloadedFilesInformation.TryAdd(filenameLower, tcs);

            try
            {
                var fileSize = await tcs.Task;
                _logger.LogInformation("File download finish. Filename: {filename}. Size: {fileSize}", filename, fileSize);
                return Content($"File download finish. Filename: {filename}. Size: {fileSize}");
            }
            catch (TaskCanceledException canceledException)
            {
                _logger.LogError("File download canceled. Filename: {filename}", filename);
                return Content($"File download canceled. Filename: {filename}");
            }
            catch (Exception exception)
            {
                _logger.LogError("File download failed (generic exception). Filename: {filename}. Ex: {exception}", filename, exception);
                return Content($"File download failed (generic exception). Filename: {filename}. Ex: {exception}");
            }
        }

        [HttpGet]
        [Route("complete/{filename}")]
        public IActionResult FileDownloadCompletedAsync(string filename)
        {
            var filenameLower = filename.ToLower();

            if (!DownloadedFilesInformation.ContainsKey(filenameLower))
            {
                return Content($"File download cannot be marked as completed because filename doesn't exist. Filename: {filename}");
            }

            DownloadedFilesInformation.TryGetValue(filenameLower, out var tcs);

            var fileSize = Random.Next(1000, 10000);
            tcs.SetResult(fileSize);

            DownloadedFilesInformation.Remove(filenameLower, out _);

            _logger.LogInformation("File marked as complete. Filename: {filename}. Size: {fileSize}", filename, fileSize);

            return Content($"File marked as complete. Filename: {filename}. Size: {fileSize}");
        }

        [HttpGet]
        [Route("error/{filename}")]
        public IActionResult FileDownloadCanceledAsync(string filename)
        {
            var filenameLower = filename.ToLower();

            if (!DownloadedFilesInformation.ContainsKey(filenameLower))
            {
                return Content($"File download cannot be marked as completed because filename doesn't exist. Filename: {filename}");
            }

            DownloadedFilesInformation.TryGetValue(filenameLower, out var tcs);

            tcs.SetCanceled();

            DownloadedFilesInformation.Remove(filenameLower, out _);

            _logger.LogInformation("File download canceled. Filename: {filename}.", filename);

            return Content($"File download canceled. Filename: {filename}.");
        }

        [HttpGet]
        [Route("longoperation")]
        public async Task<IActionResult> LongOperationAsyncDownloadFileAsync()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                try
                {
                    CommonCancellationTokenSource = cancellationTokenSource;
                    await Task.Delay(10000, CommonCancellationTokenSource.Token);
                    CommonCancellationTokenSource = null;
                }
                catch (TaskCanceledException canceledException)
                {
                    _logger.LogError("Long operation canceled. Ex: {exception}", canceledException);
                    return Content($"Long operation canceled");
                }
            }

            return Content("All good. Request completed.");
        }

        [HttpGet]
        [Route("canceloperation")]
        public IActionResult CancelLongOperationAsyncDownloadFile()
        {
            if (CommonCancellationTokenSource == null)
            {
                return Content("No cancelation token source to cancel");
            }

            CommonCancellationTokenSource.Cancel();
            CommonCancellationTokenSource = null;
            return Content("Request Canceled");
        }

    }
}