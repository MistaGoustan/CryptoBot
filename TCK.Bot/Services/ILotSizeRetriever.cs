﻿
namespace TCK.Bot.Services
{
    public interface ILotSizeRetriever
    {
        Task<SymbolLotSizeFilter> ForSymbolAsync(Exchange exchange, String ticker);
    }
}