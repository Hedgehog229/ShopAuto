using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int CarId { get; set; }
        public uint Price { get; set; }
        public virtual Car _Car { get; set; }
        public virtual Order _Order { get; set; }

        public List<OrderDetails> orderDetails { get; set; } //поле на основе класса с информацией о заказе - в список помещается описание всех товаров, которые мы приобретаем (добавили в корзину)        
    }
}
