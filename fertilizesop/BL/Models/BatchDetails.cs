using fertilizesop.BL.Models;

public class BatchDetails
{
    public int details_id { get; private set; }
    public string batch_name { get; private set; }
    public string product_name { get; private set; }
    public int product_id { get; private set; }
    public decimal cost_price { get; private set; }
    public decimal sale_price { get; private set; }
    public int quantity_received { get; private set; }

    public BatchDetails(int details_id, string batch_name,  decimal cost_price, decimal sale_price,int product_id,string product_name, int quantity_received)
    {
        this.details_id = details_id;
        this.batch_name = batch_name;
        this.cost_price = cost_price;
        this.sale_price = sale_price;
        this.quantity_received = quantity_received;
        this.product_id = product_id;
        this.product_name = product_name;
    }



}
