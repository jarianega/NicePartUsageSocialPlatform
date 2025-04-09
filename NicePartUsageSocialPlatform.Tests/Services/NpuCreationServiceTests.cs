using Moq;
using NicePartUsageSocialPlatform.Models;
using NicePartUsageSocialPlatform.Repositories;
using NicePartUsageSocialPlatform.Services;

namespace NicePartUsageSocialPlatform.Tests.Services;

public class NpuCreationServiceTests
{
    private readonly Mock<INpuCreationRepository> _npuCreationRepositoryMock;
    private readonly Mock<IFileService> _fileServiceMock;
    private readonly Mock<IElementService> _elementServiceMock;
    private readonly Mock<IScoreRepository> _scoreRepositoryMock;

    private readonly NpuCreationService _npuCreationService;

    public NpuCreationServiceTests()
    {
        _npuCreationRepositoryMock = new Mock<INpuCreationRepository>();
        _fileServiceMock = new Mock<IFileService>();
        _elementServiceMock = new Mock<IElementService>();
        _scoreRepositoryMock = new Mock<IScoreRepository>();
        
        _npuCreationService = new NpuCreationService(
            _npuCreationRepositoryMock.Object,
            _scoreRepositoryMock.Object,
            _fileServiceMock.Object,
            _elementServiceMock.Object
        );
    }

    [Fact]
    public async Task UpsertNpuCreationScoreAsync_UserHasAlreadyScoredCreation_UpdatesCreativityAndUniquenessScores()
    {
        // Arrange
        _scoreRepositoryMock
            .Setup(s => s.GetNpuCreationScoreByUserIdAndCreationIdAsync(It.IsAny<string>(), It.IsAny<Guid>(),
                CancellationToken.None))
            .ReturnsAsync(new Score());

        // Act
        await _npuCreationService.UpsertNpuCreationScoreAsync(
            2,
            3,
            new User(),
            new NpuCreation(),
            CancellationToken.None);

        // Assert
        _scoreRepositoryMock.Verify(s => s.UpdateScoreAsync(It.IsAny<Score>(), 2, 3, CancellationToken.None));
    }

    [Fact]
    public async Task UpsertNpuCreationScoreAsync_UserHasNotAlreadyScoredCreation_CreatesScore()
    {
        // Arrange
        _scoreRepositoryMock
            .Setup(s => s.GetNpuCreationScoreByUserIdAndCreationIdAsync(It.IsAny<string>(), It.IsAny<Guid>(),
                CancellationToken.None))
            .ReturnsAsync((Score?)null);

        // Act
        await _npuCreationService.UpsertNpuCreationScoreAsync(
            2,
            3,
            new User(),
            new NpuCreation(),
            CancellationToken.None);

        // Assert
        _scoreRepositoryMock.Verify(s => s.CreateScoreAsync(
            It.Is<Score>(score =>
                score.CreativityScore == 2 &&
                score.UniquenessScore == 3),
            CancellationToken.None));
    }
}