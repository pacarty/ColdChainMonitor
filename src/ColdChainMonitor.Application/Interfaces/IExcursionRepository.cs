using ColdChainMonitor.Domain.Entities;

namespace ColdChainMonitor.Application.Interfaces;

public interface IExcursionRepository
{
    Task AddAsync(Excursion excursion, CancellationToken ct = default);
}
