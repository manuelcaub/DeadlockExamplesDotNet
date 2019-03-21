using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace AsyncDotNetFramework.Controllers
{
    public class DeadlockController : ApiController
    {
        [HttpGet]
        [Route("deadlock/no")]
        public async Task<IHttpActionResult> No()
        {
            var context = SynchronizationContext.Current;
            await Delay();
            return Ok("NO Deadlock");
        }

        [HttpGet]
        [Route("deadlock/depends")]
        public async Task<IHttpActionResult> ItDepends()
        {
            await Delay();
            Delay().Wait();
            return Ok("Deadlock?");
        }

        [HttpGet]
        [Route("deadlock/configure-await")]
        public async Task<IHttpActionResult> ConfigureAwait()
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