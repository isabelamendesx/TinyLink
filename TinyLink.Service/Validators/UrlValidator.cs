using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TinyLink.Domain.Entities;
using TinyLink.Domain.Interfaces;

namespace TinyLink.Service.Validators
{
    public class UrlValidator : IUrlValidator
    {
        private static readonly Regex DestinationRegex = new Regex(
          @"^(https?|ftps?):\/\/(?:[a-zA-Z0-9]" +
                  @"(?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,}" +
                  @"(?::(?:0|[1-9]\d{0,3}|[1-5]\d{4}|6[0-4]\d{3}" +
                  @"|65[0-4]\d{2}|655[0-2]\d|6553[0-5]))?" +
                  @"(?:\/(?:[-a-zA-Z0-9@%_\+.~#?&=]+\/?)*)?$",
          RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(1));

        private readonly IUrlRepository _repository;

        public UrlValidator(IUrlRepository repository)
            => _repository = repository;

        public async Task<bool> ValidateDestination(string destination)
            => DestinationRegex.IsMatch(destination)
                && Uri.TryCreate(destination, UriKind.Absolute, out _);

        public async Task<bool> IsShortUrlUnique(string shorturl) =>
            !(await _repository.Exists(x => x.ShortUrl == shorturl));
       
        public async Task<bool> IsDestinationExpiredUrl(string destination) 
            => await _repository.Exists(x => x.Destination == destination && x.Expiration <= DateTime.UtcNow);

        public bool IsExpired(Url url) =>
                url.Expiration <= DateTime.UtcNow;

    }
}
