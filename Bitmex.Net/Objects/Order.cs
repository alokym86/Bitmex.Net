﻿using Bitmex.Net.Client.Converters;
using CryptoExchange.Net.ExchangeInterfaces;
using Newtonsoft.Json;

namespace Bitmex.Net.Client.Objects
{
    /// <summary>Placement, Cancellation, Amending, and History</summary>
    public class Order : ICommonOrder, ICommonOrderId
    {
        [JsonProperty("orderID")]
        public string Id { get; set; }

        [JsonProperty("clOrdID")]
        public string ClOrdID { get; set; }

        [JsonProperty("clOrdLinkID")]
        public string ClOrdLinkID { get; set; }

        [JsonProperty("account")]
        public long Account { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("side")]
        [JsonConverter(typeof(BitmexOrderSideConverter))]
        public BitmexOrderSide Side { get; set; }

        [JsonProperty("simpleOrderQty")]
        public decimal? SimpleOrderQty { get; set; }

        [JsonProperty("orderQty")]
        public decimal? OrderQty { get; set; }

        [JsonProperty("price")]
        public decimal? Price { get; set; }

        [JsonProperty("displayQty")]
        public decimal? DisplayQty { get; set; }

        [JsonProperty("stopPx")]
        public decimal? StopPx { get; set; }

        [JsonProperty("pegOffsetValue")]
        public decimal? PegOffsetValue { get; set; }

        [JsonProperty("pegPriceType")]
        public string PegPriceType { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("settlCurrency")]
        public string SettlCurrency { get; set; }

        [JsonProperty("ordType"), JsonConverter(typeof(BitmexOrderTypeConverter))]
        public BitmexOrderType OrdType { get; set; }

        [JsonProperty("timeInForce")]
        public string TimeInForce { get; set; }

        [JsonProperty("execInst")]
        public string ExecInst { get; set; }

        [JsonProperty("contingencyType")]
        public string ContingencyType { get; set; }

        [JsonProperty("exDestination")]
        public string ExDestination { get; set; }

        [JsonProperty("ordStatus"), JsonConverter(typeof(BitmexOrderStatusConverter))]
        public BitmexOrderStatus Status { get; set; }

        [JsonProperty("triggered")]
        public string Triggered { get; set; }

        [JsonProperty("workingIndicator")]
        public bool? WorkingIndicator { get; set; }

        [JsonProperty("ordRejReason")]
        public string OrdRejReason { get; set; }

        [JsonProperty("simpleLeavesQty")]
        public decimal? SimpleLeavesQty { get; set; }

        [JsonProperty("leavesQty")]
        public decimal? LeavesQty { get; set; }

        [JsonProperty("simpleCumQty")]
        public decimal? SimpleCumQty { get; set; }

        [JsonProperty("cumQty")]
        public decimal? CumQty { get; set; }

        [JsonProperty("avgPx")]
        public decimal? AvgPx { get; set; }

        [JsonProperty("multiLegReportingType")]
        public string MultiLegReportingType { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("transactTime")]
        public System.DateTime? TransactTime { get; set; }

        [JsonProperty("timestamp")]
        public System.DateTime? Timestamp { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }

        public string CommonId => Id;

        public string CommonSymbol => Symbol;

        public decimal CommonPrice => Price??StopPx??0;

        public decimal CommonQuantity => OrderQty ?? 0;

        public string CommonStatus => Status.ToString();

        public bool IsActive => Status == BitmexOrderStatus.New || Status == BitmexOrderStatus.PartiallyFilled;

        public IExchangeClient.OrderSide CommonSide => Side == BitmexOrderSide.Buy ? IExchangeClient.OrderSide.Buy : IExchangeClient.OrderSide.Sell;

        public IExchangeClient.OrderType CommonType => OrdType switch
        {
            BitmexOrderType.Limit => IExchangeClient.OrderType.Limit,
            BitmexOrderType.Market => IExchangeClient.OrderType.Market,
            _ => IExchangeClient.OrderType.Other
        };
    }

}

