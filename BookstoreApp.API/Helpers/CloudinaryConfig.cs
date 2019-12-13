using BookstoreApp.API.Helpers;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;

namespace BookstoreApp.API.Helpers
{
    public interface ICloudinaryConfig
    {
        Cloudinary Cloudinary { get; set; }
    }

    public class CloudinaryConfig : ICloudinaryConfig
    {
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        public Cloudinary Cloudinary { get; set; }

        public CloudinaryConfig(IOptions<CloudinarySettings> _cloudinaryConfig)
        {
            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            Cloudinary = new Cloudinary(acc);
        }
    }
}