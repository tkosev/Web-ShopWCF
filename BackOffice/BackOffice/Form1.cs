using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.ServiceModel;
using BackOffice.clientnamespace;

namespace BackOffice
{

    [CallbackBehavior(UseSynchronizationContext = false)]

    public partial class Form1 : Form, IBackOfficeCallback
    {

        clientnamespace.BackOfficeClient client;
        clientnamespace.Order[] myArray;
        //List<Order> list;
        InstanceContext instance;
        
        public Form1()
        {
            InitializeComponent();
            //list = new List<Order>();
            instance = new InstanceContext(this);
            client = new clientnamespace.BackOfficeClient(instance);
            client.Subscribe();
            
            
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            
            Label.CheckForIllegalCrossThreadCalls = false;
            client.ShipOrder(listBox1.SelectedItem.ToString());
            label1.Text = listBox1.SelectedItem.ToString() + "was shipped at :" + DateTime.Now;
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            myArray = client.GetOrderList();
            foreach (clientnamespace.Order item in myArray)
            {
                listBox2.Items.Add(item.Moment);
                listBox1.Items.Add(item.ProductId);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            try
            {
                myArray = client.GetOrderList();
                foreach (clientnamespace.Order item in myArray)
                {
                    listBox2.Items.Add(item.Moment);
                    listBox1.Items.Add(item.ProductId);
                }
            }
            catch (EntryPointNotFoundException d)
            {
                MessageBox.Show(d.Message);
            }
        }


        public void OnOrder(clientnamespace.Order ord)
        {
            ListBox.CheckForIllegalCrossThreadCalls = false;
            //list.Add(ord);
            listBox1.Items.Add(ord.ProductId);
            listBox2.Items.Add(ord.Moment);
            
        }
    }
}
