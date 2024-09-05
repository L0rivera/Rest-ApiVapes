namespace Rest_ApiVapes.Models
{
    public class Vape
    {
        public int VapeId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public bool status { get; set; }
        public string brand { get; set; }
        public string flavor { get; set; }
        public string public_id { get; set; }
        public string secure_url { get; set; }
        public string nicotine { get; set; }
        public string e_liquid {  get; set; }
        public string mAh {  get; set; }
        public string uses { get; set; }
        public bool rechargable { get; set; }

    }
}
