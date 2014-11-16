using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace webshopservice
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    
    [ServiceContract(Namespace = "webshopservice", CallbackContract = typeof(IWebshopCallback))]
    public interface IWebshop
    {
        [OperationContract]
        string GetWebshopName();
        [OperationContract]
        List<Item> GetProductList();
        [OperationContract]
        string GetProductInfo(string ProductId);
        [OperationContract]
        bool BuyProduct(string ProductId);
    }

    [ServiceContract(Namespace = "webshopservice",CallbackContract = typeof(IMyEvents))]
    public interface IBackOffice
    {
        [OperationContract]
        List<Order> GetOrderList();

        [OperationContract]
        bool ShipOrder(string ProductId);

        [OperationContract]
        void Subscribe();
        [OperationContract]
        void Unsubscribe();

    
    }

    [ServiceContract(Namespace = "webshopservice", CallbackContract = typeof(IBank))]
    public interface IBank
    {
        [OperationContract]
        void getMoneyBank();
    
    }

    public interface IMyEvents
    {
        [OperationContract(IsOneWay = true)]
        void OnOrder(Order ord);
    
    }

    public interface IWebshopCallback
    {
        [OperationContract]
        void productShipped(string productId, DateTime shippingMoment);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "webshopservice.ContractType".
    [DataContract]
    public class Item
    {
        private string productId;
        private string productInfo;
        private double price;
        private int stock;
        private bool onSale;

        [DataMember]
        public string ProductId{get { return productId; }set { productId = value; }}
        public string ProductInfo { get { return productInfo; } set { productInfo = value; } }
        [DataMember]
        public double Price { get { return price; } set { price = value; } }
        [DataMember]
        public int Stock { get { return stock; } set { stock = value; } }
        public bool OnSale{get { return onSale; }set { onSale = value; }}

        public Item(string productId,double price,int stock,string productInfo)
        {
            this.productId = productId;
            this.price = price;
            this.stock = stock;
            this.productInfo = productInfo;
            this.onSale = true;
        }

        
    }
    [DataContract]
    
    public class Order
    {

        private string productId;
        private DateTime moment;

        [DataMember]
        public string ProductId { get { return productId; } set { productId = value; } }

        [DataMember]
        public DateTime Moment { get { return moment; } set { moment = value; } }

        public IWebshopCallback WebshopCallback { get; set; }

        public Order(string productId, DateTime moment)
        {
            this.productId = productId;
            this.moment = moment;
        }
        
       
    }

}