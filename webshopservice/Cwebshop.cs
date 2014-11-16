using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace webshopservice
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
   
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,ConcurrencyMode =ConcurrencyMode.Reentrant)]
    public class Cwebshop : IWebshop, IBackOffice
    {
        

        IWebshopCallback clientcallbAck;
        private List<Item> myItemlist = new List<Item>();
        private Item item = new Item("Dracula ",10.5,5,"INFO");
        private Item item1 = new Item("A room with a view ",  15, 9,"INFO ");
        private Item item3 = new Item("Uncle Tom's cabin ", 12.5, 54,"INFO");
        private List<Order> myOrderList = new List<Order>();
        public Cwebshop()
        {
            myItemlist.Add(item);
            myItemlist.Add(item1);
            myItemlist.Add(item3);
        }
        
        public string GetWebshopName()
        {
            return "Todor's WebShop";
        }

        

        public List<Item> GetProductList()
        {
            
            return myItemlist;
        }

        public string GetProductInfo(string ProductId)
        {
            foreach (Item item in GetProductList())
            {
                if (item.ProductInfo == ProductId)
                {
                    return item.ProductInfo;
                }
            }
            return null;
        }

        public bool BuyProduct(string ProductId)
        {
            
            Console.WriteLine(ProductId);
            foreach (Item item in GetProductList())
            {
                if (item.ProductId == ProductId)
                {
                    if (item.OnSale && item.Stock > 0)
                    {
                        Order order = new Order(item.ProductId,DateTime.Now);
                        order.WebshopCallback = OperationContext.Current.GetCallbackChannel<IWebshopCallback>();
                        order.ProductId = ProductId;
                        order.Moment = DateTime.Now;
                        FireEvent(order);
                        myOrderList.Add(order);
                        item.Stock--;
                        item.OnSale = !(item.Stock <= 0);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public List<Order> GetOrderList()
        {
            return myOrderList;
        }

        public bool ShipOrder(string ProductId)
        {
            foreach (Order ord in myOrderList)
            {
                if (ord.ProductId == ProductId)
                {
                    ord.WebshopCallback.productShipped(ord.ProductId,DateTime.Now);
                    myOrderList.Remove(ord);
                    return true;
                }
            }
            return false;
        }

        static Action<Order> MEvent = delegate { };

        public void Subscribe()
        {
            IMyEvents Subscriber = OperationContext.Current.GetCallbackChannel<IMyEvents>();
            MEvent += Subscriber.OnOrder;
        }
        public void Unsubscribe()
        {
            IMyEvents Subscriber = OperationContext.Current.GetCallbackChannel<IMyEvents>();
            MEvent -= Subscriber.OnOrder;
        }
        public static void FireEvent(Order o)
        {
            MEvent(o);
        }



        public void getMoneyBank()
        {
        }
    }
}
