using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bitmex.Net.Client.Objects;
using Bitmex.Net.Client.Objects.Requests;
using CryptoExchange.Net.Interfaces.CommonClients;
using CryptoExchange.Net.Objects;

namespace Bitmex.Net.Client.Interfaces
{
    public interface IBitmexCommonTradeClient : IBaseRestClient
    {
        #region Execution : Raw Order and Balance Data
        /// <summary>
        /// Get all raw executions for your account
        /// This returns all raw transactions, which includes order opening and cancelation, and order status changes. It can be quite noisy. More focused information is available at /execution/tradeHistory.
        /// You may also use the filter param to target your query.Specify an array as a filter value, such as {"execType": ["Settlement", "Trade"]        ///        }
        /// to filter on multiple values.
        /// <see href="http://www.onixs.biz/fix-dictionary/5.0.SP2/msgType_8_8.html">See the  FIX Spec for explanations of these fields.</see>
        /// </summary>
        /// <param name="requestWithFilter"><see cref="BitmexRequestWithFilter"/></param>
        /// <returns></returns>
        Task<WebCallResult<List<Execution>>> GetExecutionsAsync(BitmexRequestWithFilter requestWithFilter = null, CancellationToken ct = default);

        /// <summary>
        /// Get all balance-affecting executions. This includes each trade, insurance charge, and settlement.
        /// Also see <see cref="GetExecutions(BitmexRequestWithFilter)"/>
        /// </summary>
        /// <param name="requestWithFilter"></param>
        /// <returns></returns>
        Task<WebCallResult<List<Execution>>> GetExecutionsTradeHistoryAsync(BitmexRequestWithFilter requestWithFilter = null, CancellationToken ct = default);
        #endregion
        #region Instrument : Tradeable Contracts, Indices, and History 
        /// <summary>
        /// This returns all instruments and indices, including those that have settled or are unlisted. Use this endpoint if you want to query for individual instruments or use a complex filter. Use /instrument/active to return active instruments, or use a filter like {"state": "Open"}.
        /// </summary>
        /// <param name="requestWithFilter"></param>
        /// <returns></returns>
        Task<WebCallResult<List<Instrument>>> GetInstrumentsAsync(BitmexRequestWithFilter requestWithFilter = null, CancellationToken ct = default);
        /// <summary>
        /// Get all active instruments and instruments that have expired in <24hrs
        /// </summary>
        /// <returns></returns>
        Task<WebCallResult<List<Instrument>>> GetActiveInstrumentsAsync(CancellationToken ct = default);

        /// <summary>
        /// Helper method. Gets all active instruments and all indices. This is a join of the result of /indices and /active
        /// </summary>
        /// <returns></returns>
        Task<WebCallResult<List<Instrument>>> GetActiveInstrumentsAndIndiciesAsync(CancellationToken ct = default);

        /// <summary>
        /// This endpoint is useful for determining which pairs are live. It returns two arrays of strings. The first is intervals, such as ["XBT:perpetual", "XBT:quarterly", "XBT:biquarterly", "ETH:quarterly", ...]. These identifiers are usable in any query's symbol param. The second array is the current resolution of these intervals. Results are mapped at the same index.
        /// </summary>
        /// <returns></returns>
        Task<WebCallResult<InstrumentInterval>> GetActiveIntervalsAsync(CancellationToken ct = default);

        /// <summary>
        /// Composite indices are built from multiple external price sources.
        /// Use this endpoint to get the underlying prices of an index.For example, send a symbol of .XBT to get the ticks and weights of the constituent exchanges that build the ".XBT" index.
        /// A tick with reference "BMI" and weight null is the composite index tick.
        /// </summary>
        /// <returns></returns>
        Task<WebCallResult<List<IndexComposite>>> GetCompositeIndexAsync(BitmexRequestWithFilter requestWithFilter = null, CancellationToken ct = default);

        /// <summary>
        /// Get all price indices
        /// </summary>
        /// <returns></returns>
        Task<WebCallResult<List<Instrument>>> GetIndiciesAsync(CancellationToken ct = default);
        #endregion
        #region Insurance : Insurance Fund Data 

        /// <summary>
        /// Get insurance fund history
        /// </summary>
        /// <returns></returns>
        Task<WebCallResult<List<Insurance>>> GetInsuranceFundHistoryAsync(CancellationToken ct = default);
        #endregion
        #region Order : Placement, Cancellation, Amending, and History 

        /// <summary>
        /// Get your orders
        /// To get open orders only, send {"open": true} in the filter param.
        /// <see href="http://www.onixs.biz/fix-dictionary/5.0.SP2/msgType_D_68.html"> See the FIX Spec for explanations of these fields.</see>
        /// </summary>
        /// <param name="requestWithFilter"></param>
        /// <returns></returns>
        Task<WebCallResult<List<BitmexOrder>>> GetOrdersAsync(BitmexRequestWithFilter requestWithFilter = null, CancellationToken ct = default);

        /// <summary>
        /// Place order. See instructions at
        /// <see href="https://www.bitmex.com/api/explorer/#!/Order/Order_new">Bitmex docs</see>
        /// </summary>
        /// <param name="placeOrderRequest"></param>
        /// <returns></returns>
        Task<WebCallResult<BitmexOrder>> PlaceOrderAsync(PlaceOrderRequest placeOrderRequest, CancellationToken ct = default);

        /// <summary>
        /// Update order.
        /// Send an OrderId or origClOrdID to identify the order you wish to amend.
        /// Both order quantity and price can be amended.Only one qty field can be used to amend.
        /// Use the leavesQty field to specify how much of the order you wish to remain open. This can be useful if you want to adjust your position's delta by a certain amount,
        /// regardless of how much of the order has already filled.
        /// A leavesQty can be used to make a "Filled" order live again, if it is received within 60 seconds of the fill.
        /// Like order placement, amending can be done in bulk.Simply send a request to PUT /api/v1/order/bulk with a JSON body of the shape: { "orders": [{...}, {...}]}, each object containing the fields used in this endpoint.
        /// </summary>
        /// <param name="updateOrderRequest"></param>
        /// <returns></returns>
        Task<WebCallResult<BitmexOrder>> UpdateOrderAsync(UpdateOrderRequest updateOrderRequest, CancellationToken ct = default);

        /// <summary>
        /// Cancel order. Either an Id or a clOrdID must be provided.
        /// </summary>
        /// <param name="cancelOrderRequest"></param>
        /// <returns></returns>
        Task<WebCallResult<List<BitmexOrder>>> CancelOrderAsync(CancelOrderRequest cancelOrderRequest, CancellationToken ct = default);

        /// <summary>
        /// Cancels all orders
        /// </summary>
        /// <param name="symbol">Optional symbol. If provided, only cancels orders for that symbol.</param>
        /// <param name="filter">Optional filter for cancellation. Use to only cancel some orders, e.g. {"side": "Buy"}.</param>
        /// <param name="text">Optional cancellation annotation. e.g. 'Spread Exceeded'</param>
        /// <returns></returns>
        Task<WebCallResult<List<BitmexOrder>>> CancelAllOrdersAsync(string symbol = null, BitmexRequestWithFilter filter = null, string text = null, CancellationToken ct = default);

        /// <summary>
        /// Create multiple new orders for the same symbol
        /// Valid order types are Market, Limit, Stop, StopLimit, MarketIfTouched, LimitIfTouched, and Pegged.
        /// Each individual order object in the array should have the same properties as an individual POST /order call.
        /// This endpoint is much faster for getting many orders into the book at once. Because it reduces load on BitMEX systems, 
        /// this endpoint is ratelimited at ceil(0.1 * orders). Submitting 10 orders via a bulk order call will only count as 1 request, 15 as 2, 32 as 4, and so on.
        /// </summary>
        /// <param name="placeOrderRequests"></param>
        /// <returns></returns>
        Task<WebCallResult<List<BitmexOrder>>> PlaceOrdersBulkAsync(List<PlaceOrderRequest> placeOrderRequests, CancellationToken ct = default);

        /// <summary>
        /// Amend multiple orders for the same symbol.
        /// Similar to <see cref="UpdateOrder(UpdateOrderRequest)"/>, but with multiple orders. application/json only. Ratelimited at 10%.
        /// </summary>
        /// <param name="ordersToUpdate"></param>
        /// <returns></returns>
        Task<WebCallResult<List<BitmexOrder>>> UpdateOrdersBulkAsync(List<UpdateOrderRequest> ordersToUpdate, CancellationToken ct = default);

        /// <summary>
        /// Automatically cancel all your orders after a specified timeout.
        /// Useful as a dead-man's switch to ensure your orders are canceled in case of an outage. If called repeatedly, the existing offset will be canceled and a new one will be inserted in its place.
        /// Example usage: call this route at 15s intervals with an offset of 60000 (60s). If this route is not called within 60 seconds, all your orders will be automatically canceled
        /// This is also available via <see href="https://www.bitmex.com/app/wsAPI#Dead-Mans-Switch-Auto-Cancel"> WebSocket</see>.
        /// </summary>
        /// <param name="timeOut">Timeout in ms. Set to 0 to cancel this timer.</param>
        /// <returns></returns>
        Task<WebCallResult<object>> CancellAllAfterAsync(TimeSpan timeOut, CancellationToken ct = default);
        #endregion
        #region OrderBook : Level 2 Book Data 
        /// <summary>
        /// Get orderbook
        /// </summary>
        /// <param name="symbol">Instrument symbol. Send a series (e.g. XBT) to get data for the nearest contract in that series.</param>
        /// <param name="depth">Orderbook depth per side. Send 0 for full depth.</param>
        /// <returns></returns>
        Task<WebCallResult<OrderBookL2>> GetOrderBookAsync(string symbol, int depth = 25, CancellationToken ct = default);
        #endregion
        #region Quote : Best Bid/Offer Snapshots & Historical Bins

        /// <summary>
        /// Get Quotes
        /// </summary>
        /// <param name="requestWithFilter"></param>
        /// <returns></returns>
        Task<WebCallResult<List<Quote>>> GetQuotesAsync(BitmexRequestWithFilter requestWithFilter = null, CancellationToken ct = default);

        /// <summary>
        /// Timestamps returned by our bucketed endpoints are the end of the period, indicating when the bucket was written to disk. Some other common systems use the timestamp as the beginning of the period. Please be aware of this when using this endpoint.
        /// </summary>
        /// <param name="binSize">Time interval to bucket by. Available options: [1m,5m,1h,1d].</param>
        /// <param name="requestWithFilter"></param>
        /// <returns></returns>
        Task<WebCallResult<List<Quote>>> GetBucketedQuotesAsync(string binSize, BitmexRequestWithFilter requestWithFilter = null, CancellationToken ct = default);
        #endregion
        #region Settlement : Historical Settlement Data 

        /// <summary>
        /// Get settlement history
        /// </summary>
        /// <param name="requestWithFilter"></param>
        /// <returns></returns>
        Task<WebCallResult<Settlement>> GetSettelmentAsync(BitmexRequestWithFilter requestWithFilter = null, CancellationToken ct = default);
        #endregion
        #region Stats : Exchange Statistics 

        /// <summary>
        /// Get exchange-wide and per-series turnover and volume statistics.
        /// </summary>
        /// <returns></returns>
        Task<WebCallResult<List<Stats>>> GetExchangeStatsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get historical exchange-wide and per-series turnover and volume statistics
        /// </summary>
        /// <returns></returns>
        Task<WebCallResult<List<StatsHistory>>> GetStatsHistoryAsync(CancellationToken ct = default);

        /// <summary>
        /// Get a summary of exchange statistics in USD
        /// </summary>
        /// <returns></returns>
        Task<WebCallResult<List<StatsUSD>>> GetStatsHistoryUsdAsync(CancellationToken ct = default);
        #endregion
        #region Trade : Individual & Bucketed Trades 

        /// <summary>
        /// Please note that indices (symbols starting with .) post trades at intervals to the trade feed. These have a size of 0 and are used only to indicate a changing price.
        /// See the <see href="https://www.onixs.biz/fix-dictionary/5.0.SP2/msgType_AE_6569.html">FIX Spec</see> for explanations of these fields.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<WebCallResult<List<BitmexTrade>>> GetTradesAsync(BitmexRequestWithFilter filter = null, CancellationToken ct = default);

        /// <summary>
        /// Timestamps returned by our bucketed endpoints are the end of the period, indicating when the bucket was written to disk. Some other common systems use the timestamp as the beginning of the period. Please be aware of this when using this endpoint.
        /// Also note the open price is equal to the close price of the previous timeframe bucket.
        /// </summary>
        /// <param name="binSize">Time interval to bucket by. Available options: [1m,5m,1h,1d].</param>
        /// <param name="partial">If true, will send in-progress (incomplete) bins for the current time period.</param>
        /// <returns></returns>
        Task<WebCallResult<List<TradeBin>>> GetTradesBucketedAsync(string binSize, bool partial = false, BitmexRequestWithFilter filter = null, CancellationToken ct = default);
        #endregion
        #region User : Account Operations
        /// <summary>
        /// Account Operations
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<User>> GetUserAccountAsync(CancellationToken ct = default);
        /// <summary>
        /// Get your current wallet information for the specified currency
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<Wallet>> GetUserWalletOneCurrencyAsync(string currency = "XBt", CancellationToken ct = default);
        /// <summary>
        /// Get your current wallet information
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<List<Wallet>>> GetUserWalletAllCurrenciesAsync(CancellationToken ct = default);
        /// <summary>
        /// Get your current wallet information for the specified currency, for all currencies use "all" as currency
        /// </summary>
        /// <param name="currency">Any currency. For all currencies specify "all"</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<List<WalletHistory>>> GetUserWalletHistoryAsync(string currency = "all", int count = 100, int startFrom = 0, CancellationToken ct = default);

        //
        //public async Task<WebCallResult<List<Transaction>>> GetUserWalletSummaryAsync(string currency = "XBt", CancellationToken ct = default)
        //{
        //    var parameters = GetParameters();
        //    parameters.Add("currency", currency);
        //    return await SendRequestAsync<List<Transaction>>(GetUrl(UserWalletSummaryEndpoint), HttpMethod.Get, ct, parameters, true);
        //}
        /// <summary>
        /// Get the execution history by day
        /// </summary>
        /// <param name="requestWithFilter">you can set Symbol and Timestamp here</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<List<Execution>>> GetUserExecutionHistoryAsync(BitmexRequestWithFilter requestWithFilter = null, CancellationToken ct = default);
        #endregion 
        #region Assets
        /// <summary>
        /// Get Assets Config
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<List<WalletAsset>>> GetWalletAssetsAsync(CancellationToken ct = default);
        #endregion
    }
}
