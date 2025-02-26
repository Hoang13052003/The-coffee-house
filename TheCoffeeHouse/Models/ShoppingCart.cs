using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheCoffeeHouse.Models
{
    public class ShoppingCart
    {
        public List<ShoppingCartItem> Items { get; set; }
        public ShoppingCart()
        {
            this.Items = new List<ShoppingCartItem>();
        }

        public void AddToCart(ShoppingCartItem item, int Quantity)
        {
            var checkExits = Items.FirstOrDefault(s =>s.ProductID == item.ProductID);
            if (checkExits != null)
            {
                checkExits.Quantity += Quantity;
                checkExits.Total = checkExits.Price * checkExits.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }

        public void Remove(int id)
        {
            var checkExits = Items.SingleOrDefault(s => s.ProductID == id);
            if (checkExits != null)
            {
                Items.Remove(checkExits);
            }
        }
        public void UpdateQuantity(int id,  int quantity)
        {
            var checkExits = Items.SingleOrDefault(s => s.ProductID == id);
            if (checkExits != null)
            {
                checkExits.Quantity = quantity;
                checkExits.Total = checkExits.Price * checkExits.Quantity;
            }
        }
        public decimal GetTotalPrice()
        {
            return Items.Sum(s => s.Total);
        }
        public int GetTotalQuantity()
        {
            return Items.Sum(s => s.Quantity);
        }
        public void ClearCart()
        {
            Items.Clear();
        }


    }
    public class ShoppingCartItem
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public decimal Total { get; set; }
        public string CateName { get; set; }
    }
}