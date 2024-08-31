using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;

public interface IHeaderRepository : IBaseRepository<Header>
{
    // This interface inherits all the methods from IBaseRepository<Header>
    // and can be extended with additional methods specific to the Header entity if needed.
}
