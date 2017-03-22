using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.ComponentModel;
using System.Xml.Linq;


namespace WindowsFormsApplication1
{
    public class Contact
    {
        private EventHandlerList events = new EventHandlerList();
        private static readonly object addressChangedEventKey = new object();

        private string address;

        public string Address
        {
            get { return this.address; }
            set
            {
                this.address = value;
                EventArgs e = new EventArgs();
                OnAddressChanged(e);
                System.Console.WriteLine("after OnAddressChanged triggered!");
            }

        }
        protected virtual void OnAddressChanged(EventArgs e)
        {
            EventHandler eh = (EventHandler)events[addressChangedEventKey];
            EventArgs ea = new EventArgs();
            if (eh != null)
            {
                eh(this, ea);
            }
            else
            {
                System.Console.WriteLine("event handler is null !");
            }
        }

        public event EventHandler AddressChanged
        {
            add
            {
                this.events.AddHandler(addressChangedEventKey, value);
            }
            remove
            {
                this.events.RemoveHandler(addressChangedEventKey, value);
            }
        }

    }
    static class Program
    {
        public static Contact c1 = new Contact();

        public static void  TimerElapsedHandler(object sender, EventArgs e){
            System.Timers.Timer t = (System.Timers.Timer)sender;
            t.Enabled = false;

            MessageBox.Show("elapsed !");
            c1.Address = "add1";
            
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            

            Form1 f1 = new Form1();
            f1.Text = "1234";
            f1.Text = f1.Text + f1.Text;
            int count = 0;

            c1.AddressChanged += delegate(object sender, EventArgs e)
            {
                System.Console.WriteLine("success to invoke AddressChanged handler1 ~~~");
                count++;
            };
            c1.AddressChanged += delegate(object sender, EventArgs e)
            {
                System.Console.WriteLine("success to invoke AddressChanged handler2");
            };

            var time = new System.Timers.Timer(2000);
            time.Elapsed += TimerElapsedHandler;
            /*time.Elapsed += delegate(object sender, ElapsedEventArgs e)
            {
                System.Console.WriteLine("delegate triggered ! {0}", count);
                count++;
                
            };*/
            time.Enabled = true;


            XElement xmlTree = new XElement("Root",
new XElement("Child1", 1),
new XElement("Child2", 2),
new XElement("Child3", 3),
new XElement("Child4", 4),
new XElement("Child5", 5)
);
            IEnumerable<XElement> elements =
                from el in xmlTree.Elements()
                where (int)el <= 3
                select el;
            foreach (XElement el in elements)
                Console.WriteLine(el);


            Application.Run(f1);
            
        }
    }
}
