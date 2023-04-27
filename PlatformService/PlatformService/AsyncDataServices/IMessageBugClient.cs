using PlatformService.Dtos;

namespace PlatformService.AsyncDataServices
{
    public interface IMessageBugClient
    {
        void PublishNewPlatform(PlatformPublishedDto platformPublishedDto);
    }
}
