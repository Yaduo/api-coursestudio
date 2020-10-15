using System;
using System.ComponentModel.DataAnnotations;
using CourseStudio.Domain.TraversalModel.Trades;

namespace CourseStudio.Doamin.Models.Trades
{
	public class TransactionRecord: Entity, IAggregateRoot
    {
		[Key]
		[MaxLength(50)]
		public string Id { get; set; }
        public int OrderId { get; set; }
		[MaxLength]
        public string Metadata { get; set; } // 用来存放第三方支付服务所回传的数据，可能是JSON或是XML格式，搭配ActiveRecord中的#serialize，可以轻易存取这个字段以供日后的查阅由服务所回传的原始交易数据
		public TransactionStatusEnum Status { get; set; }
		public TransactionTypeEnum Type { get; set; }
		public DateTime CreateDateUTC { get; set; }

        // Navigation Property
        public Order Order { get; set; }

		public static TransactionRecord Create(string id, bool isApproved, TransactionTypeEnum type, DateTime createTime, string metadata) 
		{
			return new TransactionRecord()
			{
				Id = id,
				CreateDateUTC = createTime,
				Status = isApproved ? TransactionStatusEnum.Approved : TransactionStatusEnum.Rejected,
				Type = type,
				Metadata = metadata
			};
		}
    }
}
