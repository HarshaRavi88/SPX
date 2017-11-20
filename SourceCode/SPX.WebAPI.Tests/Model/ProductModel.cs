using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SPX.WebAPI.Tests.Model
{
  

    [XmlRoot(ElementName = "review", Namespace = "http://spxflow.com/products/XMLSchema")]
    public class Review
    {
        public int Id { get; set; }
        [XmlElement(ElementName = "rating", Namespace = "http://spxflow.com/products/XMLSchema")]
        public string Rating { get; set; }
        [XmlElement(ElementName = "comment", Namespace = "http://spxflow.com/products/XMLSchema")]
        public string Comment { get; set; }
        [XmlElement(ElementName = "user", Namespace = "http://spxflow.com/products/XMLSchema")]
        public string User { get; set; }
    }

    [XmlRoot(ElementName = "reviews", Namespace = "http://spxflow.com/products/XMLSchema")]
    public class Reviews
    {
        public int Id { get; set; }
        [XmlElement(ElementName = "review", Namespace = "http://spxflow.com/products/XMLSchema")]
        public List<Review> Review { get; set; }
    }

    [XmlRoot(ElementName = "product", Namespace = "http://spxflow.com/products/XMLSchema")]
    public class Product
    {
        [XmlElement(ElementName = "id", Namespace = "http://spxflow.com/products/XMLSchema")]
        public int Id { get; set; }
        [XmlElement(ElementName = "title", Namespace = "http://spxflow.com/products/XMLSchema")]
        public string Title { get; set; }
        [XmlElement(ElementName = "shortDescription", Namespace = "http://spxflow.com/products/XMLSchema")]
        public string ShortDescription { get; set; }
        [XmlElement(ElementName = "brand", Namespace = "http://spxflow.com/products/XMLSchema")]
        public string Brand { get; set; }
        [XmlElement(ElementName = "reviews", Namespace = "http://spxflow.com/products/XMLSchema")]
        public Reviews Reviews { get; set; }
    }

    [XmlRoot(ElementName = "products", Namespace = "http://spxflow.com/products/XMLSchema")]
    public class Products
    {
        public int Id { get; set; }
        [XmlElement(ElementName = "product", Namespace = "http://spxflow.com/products/XMLSchema")]
        public List<Product> Product { get; set; }
        //[XmlAttribute(AttributeName = "xmlns")]
        //public string Xmlns { get; set; }
        //[XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        //public string Xsi { get; set; }
        //[XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        //public string SchemaLocation { get; set; }
    }

}
