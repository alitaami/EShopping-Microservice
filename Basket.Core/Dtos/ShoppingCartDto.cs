using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Core.Dtos
{
    public class ShoppingCartDto
    {
        public ShoppingCartDto()
        {

        }
        public ShoppingCartDto(string username)
        {
            Username = username;
        }
        public string Username { get; set; }
        public List<ShoppingCartItemDto> Items { get; set; }
        public Decimal TotalPrice
        {
            get
            {
                decimal total = 0;
                foreach (var item in Items)
                {
                    total+= item.Price * item.Quantity;
                }
                return total;
            }
        }
    }
}
