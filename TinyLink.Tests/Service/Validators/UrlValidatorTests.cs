using FluentAssertions;
using NSubstitute;
using System.Linq.Expressions;
using TinyLink.Domain.Entities;
using TinyLink.Domain.Interfaces;
using TinyLink.Service.Validators;

namespace TinyLink.Tests.Service.Validators
{
    public class UrlValidatorTests
    {
        private readonly UrlValidator _sut;  
        private readonly IUrlRepository _repository = Substitute.For<IUrlRepository>();

        public UrlValidatorTests()
        {
            // Arrange
            _sut = new UrlValidator(_repository);
        }

        [Theory]
        [MemberData(nameof(InvalidUrls))]
        public async Task ValidateDestination_ShouldReturnFalse_WhenUrlIsInvalid(string url)
        {
            // Act 
            var result = await _sut.ValidateDestination(url);

            // 
            result.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(ValidUrls))]
        public async Task ValidateDestination_ShouldReturnTrue_WhenUrlIsValid(string url)
        {
            // Act 
            var result = await _sut.ValidateDestination(url);

            // 
            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsShortUrlUnique_ShouldReturnFalse_WhenUrlExists()
        {
            // Assert 
            _repository.Exists(Arg.Any<Expression<Func<Url, bool>>>()).Returns(true);

            // Act
            var result = await _sut.IsShortUrlUnique("existingurl");

            // Assert 
            result.Should().BeFalse();
        }

        [Fact]
        public async Task IsShortUrlUnique_ShouldReturnTrue_WhenUrlNotExists()
        {
            // Assert 
            _repository.Exists(Arg.Any<Expression<Func<Url, bool>>>()).Returns(false);

            // Act
            var result = await _sut.IsShortUrlUnique("notexistingurl");

            // Assert 
            result.Should().BeTrue();
        }

        [Fact]
        public void IsExpired_ShouldReturnTrue_WhenUrlExpirationIsPast()
        {
            // Arrange
            var expiredUrl = new Url { Expiration = DateTime.UtcNow.AddDays(-1) };

            // Act
            var result = _sut.IsExpired(expiredUrl);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsExpired_ShouldReturnFalse_WhenUrlExpirationIsFuture()
        {
            // Arrange
            var futureUrl = new Url { Expiration = DateTime.UtcNow.AddDays(1) };

            // Act
            var result = _sut.IsExpired(futureUrl);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task IsDestinationExpiredUrl_ShouldReturnTrue_WhenDestinationIsExpired()
        {
            // Arrange
            var expiredUrl = new Url { Destination = "expiredDestinationUrl", Expiration = DateTime.UtcNow.AddDays(-1) };
            _repository.Exists(Arg.Any<Expression<Func<Url, bool>>>()).Returns(true);

            // Act
            var result = await _sut.IsDestinationExpiredUrl("expiredDestinationUrl");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsDestinationExpiredUrl_ShouldReturnFalse_WhenDestinationIsNotExpired()
        {
            // Arrange
            var validUrl = new Url { Destination = "validDestinationUrl", Expiration = DateTime.UtcNow.AddDays(1) };
            _repository.Exists(Arg.Any<Expression<Func<Url, bool>>>()).Returns(false);

            // Act
            var result = await _sut.IsDestinationExpiredUrl("validDestinationUrl");

            // Assert
            result.Should().BeFalse();
        }


        public static TheoryData<string> ValidUrls => new TheoryData<string>
        {
            "https://google.com",
            "http://google.com",
            "https://www.example.com",
            "http://example.com",
            "ftp://ftp.example.com",
            "http://www.example.com/path/to/page",
            "https://example.com:8080",
            "https://subdomain.example.com",
            "https://www.example.com/?query=test",
            "https://www.example.com/path/to/page?query=test",
            "https://www.example.com/#section"
        };

        public static TheoryData<string> InvalidUrls => new TheoryData<string>
        {
            "https://w.oidedjniortft4iothnfdsn3r934u9th49f",
            "www.google.com",
            "https://example..com",
            "example.com",
            "ht://example.com",
            "",
            "http://example.com:port",
            "http://localhost",
            "http://localhost:8080",
            "ftp://ftp.example.com:port",
            "://example.com",
            "http:/example.com",
            "http://.com",
            "http://example.123",
            "http://example.",
            "http://.123"
        };
    }
}
