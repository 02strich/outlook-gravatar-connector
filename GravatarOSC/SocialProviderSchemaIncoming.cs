using System;
using System.Xml.Serialization;

// ReSharper disable InconsistentNaming
namespace OSCSchema2
{
    [Serializable]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class hashedAddresses {

        private personAddressType[] personAddressesField;

        [XmlElementAttribute("personAddresses")]
        public personAddressType[] personAddresses {
            get { return this.personAddressesField; }
            set { this.personAddressesField = value; }
        }
    }

    [Serializable]
    [XmlTypeAttribute(Namespace = "")]
    public partial class personAddressType {
        private string[] hashedAddressField;
        private int indexField;

        [XmlElementAttribute("hashedAddress")]
        public string[] hashedAddress {
            get { return this.hashedAddressField; }
            set { this.hashedAddressField = value; }
        }

        [XmlAttributeAttribute()]
        public int index {
            get { return this.indexField; }
            set { this.indexField = value; }
        }
    }
}
// ReSharper restore InconsistentNaming