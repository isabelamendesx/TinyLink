using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyLink.Domain.Entities;
using TinyLink.Domain.Interfaces;
using static System.Net.WebRequestMethods;

namespace TinyLink.Service.Services
{
    public class UrlShortenerService : IUrlShortenerService
    {
        private readonly IUrlRepository _repository;
        private readonly IRandomizer _randomizer;
        private readonly IUrlValidator _urlValidator;
        private const string ALLOWED_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%&";
        private const int SHORT_URL_LENGTH = 7;
        private const string BASE_URL = "https://localhost:7208";
        public UrlShortenerService(IUrlRepository repository, IRandomizer randomizer, IUrlValidator urlValidator)
        {
            _repository = repository;
            _randomizer = randomizer;
            _urlValidator = urlValidator;
        }

        public Url? MatchShortUrl(string shorturl) 
        {
            var url = _repository.GetByParameter(x => x.ShortUrl == shorturl).GetAwaiter().GetResult();

            if (url != null && !_urlValidator.IsExpired(url))
                return url;

            return null;
        }

        public async Task<string> ManageUrl(string destinationUrl)
        {
    
            if (await _urlValidator.IsDestinationExpiredUrl(destinationUrl))
                return await HandleExpiredUrl(destinationUrl);

            if (await _urlValidator.ValidateDestination(destinationUrl))
               return  await CreateShortenedUrl(destinationUrl);

            throw new UriFormatException("Invalid url has been provided");
        }

        private async Task<string> CreateShortenedUrl(string destinationUrl)
        {
            var shorturl = await GenerateUniqueShortUrl();
            await SaveUrlMapping(destinationUrl, shorturl);
            return BuildFullShortUrl(shorturl);
        }

        public async Task<string> HandleExpiredUrl(string destinationUrl)
        {
            var url = await _repository.GetByParameter(x => x.Destination == destinationUrl);
            url!.Expiration = DateTime.UtcNow;
            await _repository.UpdateAsync(url);
            return BuildFullShortUrl(url.ShortUrl);
        }

        private async Task SaveUrlMapping(string destinationUrl, string shortUrl)
        {
            var url = new Url { Destination = destinationUrl, ShortUrl = shortUrl, Expiration = DateTime.UtcNow.AddMinutes(5) };
            await _repository.AddAsync(url);
        }

        private async Task<string> GenerateUniqueShortUrl()
        {
            const int maxAttempts = 5;
            int attempts = 0;

            while(attempts < maxAttempts)
            {
                string shortUrl = GenerateRandomShortUrl();

                if(await _urlValidator.IsShortUrlUnique(shortUrl))
                {
                    return shortUrl;
                }

                attempts++;
            }

            throw new InvalidOperationException("Failed to generate a unique short URL after multiple attempts.");
        }


        public string GenerateRandomShortUrl() => new string(Enumerable.Repeat(ALLOWED_CHARS, SHORT_URL_LENGTH)
                .Select(x => x[_randomizer.Next(x.Length)]).ToArray());


        public string BuildFullShortUrl(string shortUrl) 
            => $"{BASE_URL}/{shortUrl}";
        

    }
}
