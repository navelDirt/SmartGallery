﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartGallery.Data.Repositories.Images;
using SmartGallery.Domain.Images;

namespace SmartGallery.Services.Images
{
    public class ImageService : IImageService
    {
        private readonly IImageDataRepository _imageDataRepository;
        private readonly IImageFileRepository _imageFileRepository;

        public ImageService(IImageDataRepository imageDataRepository, IImageFileRepository imageFileRepository)
        {
            this._imageDataRepository = imageDataRepository;
            this._imageFileRepository = imageFileRepository;
        }

        public async Task<IList<ImageData>> GetAllImagesAsync(int pageNumber, int pageSize)
        {
            IList<ImageData> imagesDataList = await this._imageDataRepository.GetAllImagesAsync(pageNumber, pageSize);

            await this._imageFileRepository.LoadImageBytesAsync(imagesDataList.ToArray());

            return imagesDataList;
        }

        public async Task<IList<ImageData>> GetImagesByCategoryAsync(string categoryName, int pageNumber, int pageSize)
        {
            IList<ImageData> imagesDataList = await this._imageDataRepository.GetImagesByCategoryAsync(categoryName, pageNumber, pageSize);

            await this._imageFileRepository.LoadImageBytesAsync(imagesDataList.ToArray());

            return imagesDataList;
        }

        public async Task<ImageData> GetImageDataAsync(int imageDataId)
        {
            ImageData imageData = await this._imageDataRepository.GetImageDataAsync(imageDataId);

            await this._imageFileRepository.LoadImageBytesAsync(imageData);

            return imageData;
        }

        public async Task<ImageData> SaveImageAsync(string fileName, byte[] imageBytes, string categoryName = null,
            string description = null)
        {
            ImageData imageData = new ImageData
            {
                FileName = fileName,
                ImageBytes = imageBytes,
                Description = description
            };

            if (categoryName != null)
            {
                imageData.Category = new ImageCategory
                {
                    Name = categoryName
                };
            }

            await this._imageDataRepository.SaveImageDataAsync(imageData);

            await this._imageFileRepository.SaveImageBytesAsync(imageData, imageBytes);

            return imageData;
        }
    }
}