using System;
namespace PosWeb.Models.Infratructure
{
	public class TrackedEntity : Entity, ITrackedEntity
	{
		public DateTime CreatedDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedDate { get; set; }

		public string UpdatedBy { get; set; }
	}
}
