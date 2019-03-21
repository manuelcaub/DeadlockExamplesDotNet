using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AsyncDotNet.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DeadlockController : ControllerBase
    {
        [HttpGet("no")]
        public async Task<IActionResult> No()
        {
            var context = SynchronizationContext.Current;
            await Delay();
            return Ok("NO Deadlock");
        }

        [HttpGet("itdepends")]
        public async Task<IActionResult> ItDependsRuntime()
        {
            await Delay();
            Delay().Wait();
            return Ok("Deadlock?");
        }

        [HttpGet("configure-await")]
        public async Task<IActionResult> ConfigureAwait()
        {
            var result = await Result().ConfigureAwait(continueOnCapturedContext: false);
            var blockingResult = Result().Result;
            return Ok("Deadlock?");
        }

        private async Task Delay()
        {
            await Task.Delay(3000);
        }

        private async Task<int> Result()
        {
            var result = 1;
            await Task.Delay(3000);
            return result;
        }
    }
}
