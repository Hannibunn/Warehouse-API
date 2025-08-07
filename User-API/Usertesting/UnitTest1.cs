using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User_API.Controllers;
using User_API.Data;
using User_API.Models;
using User_API.Services;
using Xunit;

namespace Usertesting
{
    public class ApiTests
    {
        private readonly DbContextOptions<ApplicatonDbContext> _dbOptions;

        public ApiTests()
        {
            _dbOptions = new DbContextOptionsBuilder<ApplicatonDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
        }

        [Fact]
        [Trait("Kategorie", "Wichtig")]
        public async Task Login_ShouldReturnOk_WithJwtToken_WhenCredentialsValid()
        {
            // Arrange
            using var context = new ApplicatonDbContext(_dbOptions);

            var password = "Password123";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var existingUser = new Users
            {
                Email = "test@example.com",
                Password = hashedPassword
            };

            context.Users.Add(existingUser);
            await context.SaveChangesAsync();

            var authServiceMock = new Mock<AuthService>(null);
            authServiceMock
                .Setup(a => a.GenerateJwtToken(existingUser.Email))
                .Returns("fake-jwt-token");

            var controller = new LoginController(context, authServiceMock.Object, null);

            var loginRequest = new Users
            {
                Email = existingUser.Email,
                Password = password
            };

            // Act
            var result = controller.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            dynamic tokenObject = okResult.Value;
            Assert.Equal("fake-jwt-token", (string)tokenObject.Token);
        }

        [Fact]
        public async Task Register_ShouldAddUser_WhenEmailNotExists()
        {
            // Arrange
            using var context = new ApplicatonDbContext(_dbOptions);

            var controller = new LoginController(context, null, null);

            var registerRequest = new Users
            {
                Email = "newuser@example.com",
                Password = "NewPassword123"
            };

            // Act
            var result = await controller.Register(registerRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Benutzer erfolgreich registriert!", okResult.Value);

            var userInDb = await context.Users.FirstOrDefaultAsync(u => u.Email == registerRequest.Email);
            Assert.NotNull(userInDb);
            Assert.NotEqual(registerRequest.Password, userInDb.Password); // Passwort gehashed
        }

        [Fact]
        public async Task GetUserBoxes_ShouldReturnOnlyUserBoxes()
        {
            // Arrange
            int userId = 123;
            var userBoxes = new List<Box>
            {
               new Box { ID = 1, Name = "Meine Box", UserId = userId },
               new Box { ID = 2, Name = "Zweite Box", UserId = userId }
            };

            var authServiceMock = new Mock<IAuthService>();
            authServiceMock.Setup(a => a.GetUserIdFromToken(It.IsAny<HttpContext>())).Returns(userId);

            var boxServiceMock = new Mock<IBoxService>();
            boxServiceMock.Setup(b => b.GetBoxesByUserIdAsync(userId)).ReturnsAsync(userBoxes);

            var controller = new BoxController(boxServiceMock.Object, authServiceMock.Object);

            // Act
            var result = await controller.GetUserBoxes();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var boxes = Assert.IsAssignableFrom<IEnumerable<Box>>(okResult.Value);

            Assert.Equal(2, boxes.Count());
            Assert.All(boxes, b => Assert.Equal(userId, b.UserId));
        }
    }
}
