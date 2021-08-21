using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; //[Display(Name = "Введите имя")], [StringLength(2)]
using Microsoft.AspNetCore.Mvc.ModelBinding; //[Required(ErrorMessage = "Длина имени не менее 2 символов")], [BindNever]



namespace Shop.Data.Models
{
    public class Order
    {
		[BindNever]// аттрибут указывает, что данное поле никогда не будет отображаться на странице (дословно "никогда не привязывать")
        public int Id { get; set; }

		[Display(Name = "Введите имя")] 
		[StringLength(15)] //Проверка, что вводимое значение не должно быть больше 15 символов
		[Required(ErrorMessage = "Длина имени не менее 2 символов")] // если меньше 2 символов - выдается сообщение 
		public string Name { get; set; }

		[Display(Name = "Фамилия")]
		[StringLength(25)] //Проверка, что вводимое значение не должно быть больше 25 символов
		[Required(ErrorMessage = "Длина фамилии не менее 2 символов")] // если меньше 2 символов - выдается сообщение 
		public string SurName { get; set; }

		[Display(Name = "Адрес")] 
		[StringLength(50)] //Проверка, что вводимое значение не должно быть больше 50 символов
		[Required(ErrorMessage = "Длина адреса не менее 15 символов")] // если меньше 15 символов - выдается сообщение 
		public string Address { get; set; }

		[Display(Name = "Телефон")]
		[DataType(DataType.PhoneNumber)]
		[StringLength(11)] //Проверка, что вводимое значение не должно быть больше 11 символов
		[Required(ErrorMessage = "Длина номера телефона не менее 11 символов")] // если меньше 11 символов - выдается сообщение 
		public string Phone { get; set; }

		[Display(Name = "EMail")]
		[DataType(DataType.EmailAddress)]
		[StringLength(35)] //Проверка, что вводимое значение не должно быть больше 35 символов
		[Required(ErrorMessage = "Длина электронного адреса не менее 5 символов")] // если меньше  5 символов - выдается сообщение 
		public string EMail { get; set; }

		[BindNever]
		[ScaffoldColumn(false)] //для того, чтобы не просто скрыть данное поле, но чтобы оно не было отображено в исходном коде (чтобы это ни значило)
		public DateTime OrderTime { get; set; }

		public List<OrderDetails> OrderDetails { get; set; }

	}
}
