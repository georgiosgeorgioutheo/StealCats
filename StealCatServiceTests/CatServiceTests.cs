using Application.Services;
using Core.Entities;
using Core.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealCatServiceTests
{
    
   public class CatServiceTests
    {
        private readonly Mock<ICatRepository> _catRepositoryMock;
        private readonly CatService _catService;

        public CatServiceTests()
        {
            _catRepositoryMock = new Mock<ICatRepository>();
            _catService = new CatService( _catRepositoryMock.Object);
        }

      

        [Fact]
        public async Task GetCatByIdAsync_ShouldReturnCatEntity()
        {
            // Arrange
            var cat = new CatEntity { CatId = "123", Width = 500, Height = 400 };
            _catRepositoryMock.Setup(r => r.GetCatByIdAsync(It.IsAny<int>())).ReturnsAsync(cat);

            // Act
            var result = await _catService.GetCatByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("123", result.CatId);
            _catRepositoryMock.Verify(r => r.GetCatByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetCatsAsync_ShouldReturnPagedCats()
        {
            // Arrange
            var cats = new List<CatEntity>
        {
            new CatEntity { CatId = "123", Width = 500, Height = 400 },
            new CatEntity { CatId = "456", Width = 600, Height = 500 }
        };
            _catRepositoryMock.Setup(r => r.GetCatsAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(cats);

            // Act
            var result = await _catService.GetCatsAsync(1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, (result as List<CatEntity>).Count);
            _catRepositoryMock.Verify(r => r.GetCatsAsync(1, 10), Times.Once);
        }

        [Fact]
        public async Task GetCatsByTagAsync_ShouldReturnPagedCatsWithTag()
        {
            // Arrange
            var cats = new List<CatEntity>
        {
            new CatEntity { CatId = "123", Width = 500, Height = 400 },
            new CatEntity { CatId = "456", Width = 600, Height = 500 }
        };
            _catRepositoryMock.Setup(r => r.GetCatsByTagAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(cats);

            // Act
            var result = await _catService.GetCatsByTagAsync("Playful", 1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, (result as List<CatEntity>).Count);
            _catRepositoryMock.Verify(r => r.GetCatsByTagAsync("Playful", 1, 10), Times.Once);
        }
    }
}