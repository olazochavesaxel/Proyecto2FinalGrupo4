using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _00_DTO;
using Alpaca.Markets;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CoreApp
{
    public class AlpacaManager : BaseManager
    {
        private readonly IAlpacaTradingClient _client;
        private readonly IAlpacaDataClient _dataClient;
        private readonly IAlpacaCryptoDataClient _cryptoDataClient;

        public AlpacaManager(IOptions<AlpacaSettings> config)
        {
            var secret = new SecretKey(config.Value.KeyId, config.Value.SecretKey);
            _client = Alpaca.Markets.Environments.Paper.GetAlpacaTradingClient(secret);
            _dataClient = Alpaca.Markets.Environments.Paper.GetAlpacaDataClient(secret);
            _cryptoDataClient = Alpaca.Markets.Environments.Paper.GetAlpacaCryptoDataClient(secret);
        }

        public async Task<IAsset> GetAssetAsync(string symbol)
        {
            return await _client.GetAssetAsync(symbol);
        }

        public async Task<IReadOnlyList<IAsset>> GetActivosAsync()
        {
            var result = await _client.ListAssetsAsync(new AssetsRequest
            {
                AssetStatus = AssetStatus.Active,
                AssetClass = AssetClass.UsEquity
            });

            return result;
        }

        public record LatestPriceData(decimal price, DateTime timeStampUTC);

        public async Task<LatestPriceData> GetPrecioActual(string symbol)
        {
            if (symbol.Contains("/")) // Asumimos que es cripto
            {
                var request = new LatestDataListRequest(new[] { symbol });
                var result = await _cryptoDataClient.ListLatestTradesAsync(request);

                if (result.TryGetValue(symbol, out var trade))
                {
                    return new LatestPriceData(trade.Price, trade.TimestampUtc);
                }

                throw new Exception($"No se encontró un trade reciente para {symbol}");
            }
            else
            {
                var trade = await _dataClient.GetLatestTradeAsync(new LatestMarketDataRequest(symbol));
                return new LatestPriceData(trade.Price, trade.TimestampUtc);
            }
        }

        private static DateTime GetSafeNowUTC()
        {
            return DateTime.UtcNow.AddMinutes(-16);
        }

        public async Task<IReadOnlyList<IBar>> GetHistoricoAsync(string symbol, DateTime from, DateTime to)
        {
            var safeTo = to > GetSafeNowUTC() ? GetSafeNowUTC() : to;

            if (symbol.Contains("/"))
            {
                var request = new HistoricalCryptoBarsRequest(symbol, from, safeTo, BarTimeFrame.Day);
                var cryptoBars = await _cryptoDataClient.ListHistoricalBarsAsync(request);

                if (cryptoBars.Items == null || cryptoBars.Items.Count == 0)
                    throw new Exception($"No se encontraron datos históricos para el símbolo cripto: {symbol}");

                return cryptoBars.Items;
            }
            else
            {
                var request = new HistoricalBarsRequest(symbol, from, safeTo, BarTimeFrame.Day);
                var stockBars = await _dataClient.ListHistoricalBarsAsync(request);

                if (stockBars.Items == null || stockBars.Items.Count == 0)
                    throw new Exception($"No se encontraron datos históricos para la acción: {symbol}");

                return stockBars.Items;
            }
        }
    }
}
