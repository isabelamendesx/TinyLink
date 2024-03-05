using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TinyLink.Domain.Entities;
using TinyLink.Domain.Interfaces;
using TinyLink.Service.Services;
using TinyLink.Service.Validators;

namespace TinyLink.Tests.Service.Services
{
     public class UrlShortenerServiceTests
    {
        private readonly UrlShortenerService _sut;
        private readonly IUrlRepository _repository = Substitute.For<IUrlRepository>();
        private readonly IRandomizer _randomizer = Substitute.For<IRandomizer>();
        private readonly IUrlValidator _urlValidator = Substitute.For<IUrlValidator>();
        public UrlShortenerServiceTests()
        {
            _sut = new UrlShortenerService(_repository, _randomizer, _urlValidator);
        }

        [Fact]
        public void MatchShortUrl_ShouldReturnUrl_WhenUrlIsValid()
        {
            var expectedUrl = new Url { ShortUrl = "abc123", Expiration = DateTime.UtcNow.AddDays(1) };
            _repository.GetByParameter(Arg.Any<Expression<Func<Url, bool>>>()).Returns(expectedUrl);
            _urlValidator.IsExpired(expectedUrl).Returns(false);

            // Act
            var result = _sut.MatchShortUrl("abc123");

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(expectedUrl);
        }

        [Fact]
        public void MatchShortUrl_ShouldReturnNull_WhenUrlIsExpired()
        {
            // Arrange
            var expectedUrl = new Url { ShortUrl = "abc123", Expiration = DateTime.UtcNow.AddDays(-1) };
            _repository.GetByParameter(Arg.Any<Expression<Func<Url, bool>>>()).Returns(expectedUrl);
            _urlValidator.IsExpired(expectedUrl).Returns(true);

            // Act
            var result = _sut.MatchShortUrl("abc123");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void BuildFullShortUrl_ShouldConstructFullShortUrl()
        {
            // Arrange
            var shortUrl = "abc123";
            var expectedFullShortUrl = "https://localhost:7208/abc123";

            // Act
            var result = _sut.BuildFullShortUrl(shortUrl);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(expectedFullShortUrl);
        }

        [Fact]
        public void GenerateRandomShortUrl_ShouldReturnUrl_WithLenght7_AndOnlyContainsAllowedChars()
        {
            // Arrange
            const string ALLOWED_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            const int SHORT_URL_LENGTH = 7;

            // Act 
            var result = _sut.GenerateRandomShortUrl();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Length.Should().Be(SHORT_URL_LENGTH);
            result.All(c => ALLOWED_CHARS.Contains(c)).Should().BeTrue();
        }




        }
}
