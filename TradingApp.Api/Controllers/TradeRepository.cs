namespace TradingApp.Api.Controllers
{
    public interface ITradeRepository
    {
        void SendTradeCommand(string message);
    }

    public class TradeRepository:ITradeRepository
    {
        public void SendTradeCommand(string message)
        {

        }
    }
}