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

using System.Xml.Linq;
using System.Xml;
using System.IO; // FileWriter


namespace dragdoptree
{
    // https://msdn.microsoft.com/zh-tw/library/system.runtime.serialization.datacontractserializer(v=vs.110).aspx
    [DataContract(Name = "Customer", Namespace = "http://www.contoso.com")]
    class Person : IExtensibleDataObject
    {
        [DataMember()]
        public string FirstName;
        [DataMember]
        public string LastName;
        [DataMember()]
        public int ID;

        public Person(string newfName, string newLName, int newID)
        {
            FirstName = newfName;
            LastName = newLName;
            ID = newID;
        }

        private ExtensionDataObject extensionData_Value;

        public ExtensionDataObject ExtensionData
        {
            get
            {
                return extensionData_Value;
            }
            set
            {
                extensionData_Value = value;
            }
        }
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            // test to get the type from values and print the name
            object[] values = { "word", true, 120, 136.34, 'a', new Form1() };
            foreach (var value in values)
                Console.WriteLine("{0} - type {1}", value,
                                  value.GetType().Name);

            foreach (var value in values[5].GetType().GetMethods())
            {
                Console.WriteLine("{0} - type {1}", value.Name,
                          value.ReturnType);
            }
            
           
        }

        private void btnsave_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Test to serialize and deserial DataContract object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void test1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (true)
            {
                Person p1 = new Person("dalong", "taiwan", 31);
                XElement x1 = SerializeToXmlElement(p1);
                String s1 = x1.ToString();
                XElement x2 = XElement.Parse(s1);
                object o1 = DeserializeFromXmlElement(x2, typeof(Person));

            }
            else
            {
                WriteObject("test.xml");
                ReadObject("test.xml");
            }
        }

        public static Object DeserializeFromXmlElement(XElement xElement, Type objType)
        {
            String xElementStr = xElement.ToString();
            String[] strArray = xElementStr.Replace(">", ">\n").Split('\n'); // split each element

            foreach (String item in strArray)
            {

            }

            try
            {

                DataContractSerializer seralizer = new DataContractSerializer(objType);
                return seralizer.ReadObject(xElement.CreateReader());
            }
            catch (NullReferenceException nullEx)
            {
                throw nullEx;
            }
            catch (Exception e)
            {
                String errorMsg = String.Format("Deserialing fail {0}", xElement.Name);
                MessageBox.Show(errorMsg, "Fatal Error");
                return null;
            }
        }

        public static XElement SerializeToXmlElement(object obj)
        {
            DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
            StringBuilder objectSB = new StringBuilder();
            XmlWriter objectWriter = XmlWriter.Create(objectSB);
            serializer.WriteObject(objectWriter, obj);
            objectWriter.Flush();

            XElement xElement = XElement.Parse(objectSB.ToString());

            return xElement;
        }


        ///
        public static void WriteObject(string fileName)
        {
            Console.WriteLine(
                "Creating a Person object and serializing it.");
            Person p1 = new Person("Zighetti", "Barbara", 101);
            FileStream writer = new FileStream(fileName, FileMode.Create);
            DataContractSerializer ser =
                new DataContractSerializer(typeof(Person));
            ser.WriteObject(writer, p1);
            writer.Close();
        }

        public static void ReadObject(string fileName)
        {
            Console.WriteLine("Deserializing an instance of the object.");
            FileStream fs = new FileStream(fileName,
            FileMode.Open);
            XmlDictionaryReader reader =
                XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            DataContractSerializer ser = new DataContractSerializer(typeof(Person));

            // Deserialize the data and read it from the instance.
            Person deserializedPerson =
                (Person)ser.ReadObject(reader, true);
            reader.Close();
            fs.Close();
            Console.WriteLine(String.Format("{0} {1}, ID: {2}",
            deserializedPerson.FirstName, deserializedPerson.LastName,
            deserializedPerson.ID));
        }
        ///

           
    }
}
