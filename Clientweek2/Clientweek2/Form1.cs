using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceModel;


namespace Clientweek2
{
    [CallbackBehavior(UseSynchronizationContext=false)]
    public partial class Form1 : Form, clientnamespeace.IWebshopCallback
    {
        private clientnamespeace.Item[] myItemArray;
        clientnamespeace.WebshopClient proxy;
  
        public Form1()
        {
            InitializeComponent();
            
            InstanceContext instance = new InstanceContext(this);
            proxy =  new clientnamespeace.WebshopClient(instance);
            myItemArray = proxy.GetProductList();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label2.Text = proxy.GetWebshopName();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            foreach (clientnamespeace.Item item in proxy.GetProductList())
            {
                listBox1.Items.Add(item.ProductId);
                listBox2.Items.Add(item.Price);
                listBox3.Items.Add(item.Stock);
            }
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Test");
        }

        private void button5_Click(object sender, EventArgs e)
        {

           
         if (listBox1.SelectedIndex != -1)
           {
               if (proxy.BuyProduct(listBox1.SelectedItem.ToString()))
               {
                   label7.Text = listBox1.SelectedItem.ToString() + " has been bought!";
                   listBox1.Items.Clear();
                   listBox2.Items.Clear();
                   listBox3.Items.Clear();
                   clientnamespeace.Item[] array = proxy.GetProductList();
                   foreach (clientnamespeace.Item item in array)
                   {
                       listBox1.Items.Add(item.ProductId);
                       listBox2.Items.Add(item.Price);
                       listBox3.Items.Add(item.Stock);
                   }
               }
               else
               {
                   MessageBox.Show("Item sold oyut");

               }
           }
          else
           {
              MessageBox.Show("Please select an item");
          }  
        }
        
        public void productShipped(string productId, DateTime shippingMoment)
        {
            Label.CheckForIllegalCrossThreadCalls = false;
            label7.Text = productId + " has been shippen at: " + shippingMoment;
            
        }

    }
}
