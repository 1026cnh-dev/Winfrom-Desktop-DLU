using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace CD3_Bai3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnDocXML_Click(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Path.Combine(Application.StartupPath, "books.xml"));

            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/catalog/book");

            string kq = "";
            foreach (XmlNode node in nodeList)
            {
                string isbn = node.Attributes["ISBN"].Value;
                string title = node.SelectSingleNode("title").InnerText;
                string price = node.SelectSingleNode("price").InnerText;
                string firstName = node.SelectSingleNode("author/first-name").InnerText;
                string lastName = node.SelectSingleNode("author/last-name").InnerText;

                kq += isbn + "   " + title + "   " + firstName + "   " + lastName + "   " + price + "\r\n";
            }

            MessageBox.Show(kq, "Kết quả đọc XML");
        }

        private void btnGhiXML_Click(object sender, EventArgs e)
        {
            string filePath =
        Path.Combine(Application.StartupPath, "book_write.xml");

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                writer.WriteStartDocument();

                // Tạm bỏ stylesheet để trình duyệt không bị trắng
                // writer.WriteProcessingInstruction(
                //     "xml-stylesheet",
                //     "type=\"text/xsl\" href=\"book.xsl\""
                // );

                writer.WriteDocType(
                    "book",
                    null,
                    null,
                    "<!ENTITY h \"hardcover\">"
                );

                writer.WriteComment("This is a book sample XML");

                writer.WriteStartElement("book");

                writer.WriteAttributeString(
                    "ISBN",
                    "9831123212"
                );

                writer.WriteAttributeString(
                    "yearpublished",
                    "2002"
                );

                writer.WriteElementString(
                    "author",
                    "Mahesh Chand"
                );

                writer.WriteElementString(
                    "title",
                    "Visual C# Programming"
                );

                writer.WriteElementString(
                    "price",
                    "44.95"
                );

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            MessageBox.Show("Đã ghi file XML thành công!\n" + filePath,"Thông báo");
        }
        private void SaveToXmlFile(List<Book> books)
        {
            var serializer = new XmlSerializer(typeof(List<Book>));
            using (var writer = new StreamWriter("books.xml"))
            {
                serializer.Serialize(writer, books, null);
                writer.Close();
            }
        }
    }
}
