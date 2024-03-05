using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TinyLink.Domain.DTOs;
using TinyLink.Domain.Interfaces;

namespace TinyLinkAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlShortenerController : ControllerBase
    {
        private readonly IUrlShortenerService _service;

        public UrlShortenerController(IUrlShortenerService service)
        {
            _service = service;
        }

        [HttpPost("/shorturl")]
        public async Task<ActionResult<string>> PostUrl([FromBody] UrlRequest request)
        {
            try
            {
                var shortenedUrl = await _service.ManageUrl(request.DestinationUrl);
                return Ok(shortenedUrl);
            }
            catch (UriFormatException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
