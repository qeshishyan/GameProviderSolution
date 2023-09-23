using CrashGameService.Service.Models;

namespace CrashGameService.Service
{
    public interface IBetService
    {
        ValueTask<BetResponse> Bet(BetRequest bet);
        ValueTask<CashOutResponse> CashOut(CashOutRequest request);
    }
}