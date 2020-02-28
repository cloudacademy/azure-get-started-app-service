using System;
using System.Collections.Generic;
using System.Text;

namespace LorryModels
{
    public class Pickup
    {
        public int Id { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress{ get; set; }
        public bool SignatureRequired { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? Delivered { get; set; }
        public string Barcode { get; set; }

        public Pickup()
        {            
        }

        public Pickup(int id, string fromaddress, string toaddress, bool sigreq, DateTime? pickedup, DateTime? delivered, string barcode)
        {
            Id = id;
            FromAddress = fromaddress;
            ToAddress = toaddress;
            SignatureRequired = sigreq;
            PickedUp = pickedup;
            Delivered = delivered;
            Barcode = barcode;
        }
    }
}
