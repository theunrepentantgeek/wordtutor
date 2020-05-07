using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WordTutor.Core.Services;
using WordTutor.Core.Tests.Fakes;
using Xunit;

namespace WordTutor.Core.Tests.ServiceTests
{
    public class CachingRenderSpeechServiceTests
    {
        public class Constructor : CachingRenderSpeechServiceTests
        {
            [Fact]
            public void GivenNullService_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new CachingRenderSpeechService(null!));
                exception.ParamName.Should().Be("renderSpeechService");
            }
        }

        public class RenderSpeechAsync : CachingRenderSpeechServiceTests
        {
            private readonly FakeRenderSpeechService _fakeService;
            private readonly CachingRenderSpeechService _cachingService;
            private readonly string _helloWorld = "Hello World";
            private readonly string _goodbyeWorld = "Goodbye World";

            public RenderSpeechAsync()
            {
                _fakeService = new FakeRenderSpeechService();
                _cachingService = new CachingRenderSpeechService(_fakeService);
            }

            [Fact]
            public async void GivenUncachedContent_CallsWrappedService()
            {
                using var stream = new MemoryStream();
                _fakeService.ProvideResult(_helloWorld, stream);
                await _cachingService.RenderSpeechAsync(_helloWorld).ConfigureAwait(false);
                _fakeService.CallCountFor(_helloWorld).Should().Be(1);
            }

            [Fact]
            public async void WhenCalledMultipleTimesForCompletedResults_CallsWrappedServiceOnce()
            {
                using var stream = new MemoryStream();
                _fakeService.ProvideResult(_helloWorld, stream);
                await _cachingService.RenderSpeechAsync(_helloWorld).ConfigureAwait(false);
                await _cachingService.RenderSpeechAsync(_helloWorld).ConfigureAwait(false);
                await _cachingService.RenderSpeechAsync(_helloWorld).ConfigureAwait(false);
                _fakeService.CallCountFor(_helloWorld).Should().Be(1);
            }

            [Fact]
            public async void WhenCalledMultipleTimesForPendingResults_CallsWrappedServiceOnce()
            {
                using var stream = new MemoryStream();
                var taskA = _cachingService.RenderSpeechAsync(_helloWorld);
                var taskB = _cachingService.RenderSpeechAsync(_helloWorld);
                var taskC = _cachingService.RenderSpeechAsync(_helloWorld);
                _fakeService.ProvideResult(_helloWorld, stream);
                await Task.WhenAll<Stream>(taskA, taskB, taskC).ConfigureAwait(false);
                _fakeService.CallCountFor(_helloWorld).Should().Be(1);
            }
        }
    }
}
